using BookingTime.Models;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System.Net;
using System.Net.Mail;
using System.IdentityModel.Tokens.Jwt;
using Microsoft.IdentityModel.Tokens;
using System.Security.Claims;
using System.Text;
using Microsoft.Extensions.Configuration;
using System.Text.Json;
using Microsoft.AspNetCore.Cors;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;
using System.Data;
using BookingTime.DTO.RequestModel;
using BookingTime.DTO.ResponseModel;
using static BookingTime.DTO.ResponseModel.PropertiesListResponseModel;

namespace BookingTime.Controllers
{
    public class ListPropertyController : Controller
    {
        private readonly IConfiguration _configuration;
        public ListPropertyController(IConfiguration configuration)
        {
            _configuration = configuration ?? throw new ArgumentNullException(nameof(configuration));
        }

        [HttpGet("/api/GetListOFProperty")]
        [EnableCors("AllowAngularApp")]
        public object GetListOFProperty()
        {
            BookingtimeContext bTMContext = new BookingtimeContext(_configuration);
            var listOfProperty = bTMContext.PropertyDetails.ToList();
            return listOfProperty;
        }

        [HttpPost("/api/AddListingPropertyOld")]
        [EnableCors("AllowAngularApp")]
        public IActionResult AddListingProperty([FromBody] JsonElement request)
        {
            if (!request.TryGetProperty("data", out JsonElement data) || data.ValueKind != JsonValueKind.Object)
                return BadRequest("Invalid request data.");
            var propertyDetail = System.Text.Json.JsonSerializer.Deserialize<PropertyDetail>(data.GetRawText());
            if (propertyDetail == null)
                return BadRequest("Failed to parse property details.");
            BookingtimeContext bTMContext = new BookingtimeContext(_configuration);
            bTMContext.PropertyDetails.Add(propertyDetail);
            bTMContext.SaveChanges();
            return Ok(new { code = 200, msg = "Property added successfully!" });
        }

        [HttpPost("/api/AddListingProperty")]
        [EnableCors("AllowAngularApp")]
        public async Task<IActionResult> InsertProperty(AddPropertyDetailsRequestModel request)
        {
            try
            {
                BookingtimeContext _context = new BookingtimeContext(_configuration);

                var existingProperty = await _context.PropertyDetails
           .Where(p => p.ListName == request.ListName &&
                       p.CountryId == request.CountryId &&
                       p.StateId == request.StateId &&
                       p.CityId == request.CityId &&
                       (p.Latitude == request.Latitude && p.Longitude == request.Longitude))
           .FirstOrDefaultAsync();

                if (existingProperty != null)
                {
                    return BadRequest(new { Message = "Property with these details already exist in the system. Please check your input and try again.", sucess = false });
                }

                string imagePath = await SaveImageAsync(request.Thumbnail);

                var property = new PropertyDetail
                {
                    ListTypeId = request.ListTypeId,
                    ListName = request.ListName,
                    UsageType = request.UsageType,
                    ShortDesc = request.ShortDesc,
                    CountryId = request.CountryId,
                    StateId = request.StateId,
                    CityId = request.CityId,
                    PostalCode = request.PostalCode,
                    Street = request.Street,
                    Latitude = request.Latitude,
                    Longitude = request.Longitude,
                    LongDesc = request.LongDesc,
                    TotalFloor = request.TotalFloor,
                    TotalRoom = request.TotalRoom,
                    RoomArea = request.RoomArea,
                    CurrencyId = request.CurrencyId,
                    BasePrice = request.BasePrice,
                    Discount = request.Discount,
                    PolicyDesc = request.PolicyDesc,
                    CancellationOption = request.CancellationOption,
                    Charges = request.Charges,
                    RatingId = request.RatingId,
                    Thumbnail = imagePath
                };

                _context.PropertyDetails.Add(property);
                await _context.SaveChangesAsync();


                if (request.Amenities != null && request.Amenities.Count > 0)
                {
                    var amenitiesList = request.Amenities
                        .Where(a => a.AmenitiesId.HasValue)
                        .Select(a => new PropertyAmenity
                        {
                            PropertyDetailId = property.Id,
                            AmenityId = a.AmenitiesId.Value
                        }).ToList();

                    _context.PropertyAmenities.AddRange(amenitiesList);
                    await _context.SaveChangesAsync();
                }

                return Ok(new { Message = $@"Successfully Created : PropertyId : {property.Id}", sucess = true });
            }
            catch (Exception ex)
            {
                throw new Exception(ex.InnerException != null ? ex.InnerException.ToString() : "Internal Server Error");
            }
        }

        private async Task<string> SaveImageAsync(IFormFile? file)
        {
            if (file == null || file.Length == 0)
                return string.Empty;

            string folderPath = _configuration["PropertyImagesPath"];
            if (!Directory.Exists(folderPath))
            {
                Directory.CreateDirectory(folderPath);
            }
            string fileName = $"{Guid.NewGuid()}_{file.FileName}";
            string filePath = Path.Combine(folderPath, fileName);

            using (var stream = new FileStream(filePath, FileMode.Create))
            {
                await file.CopyToAsync(stream);
            }

            return $"{filePath}";
        }

        [HttpPost("/api/GetListingPropertyList")]
        [EnableCors("AllowAngularApp")]
        public async Task<PropertyResponseModeldetails> GetListingPropertyListAsync([FromBody] PropertiesFilterRequestModel request)
        {
            try
            {
                BookingtimeContext _context = new BookingtimeContext(_configuration);
                string amenities = null;

                string? ConnectionString = _configuration.GetConnectionString("BookingTimeConnection");
                List<PropertiesListResponseModel> propertylist = new List<PropertiesListResponseModel>();
                PropertyResponseModeldetails model = new PropertyResponseModeldetails();
                SqlConnectionStringBuilder builder = new SqlConnectionStringBuilder(ConnectionString);
                builder.ConnectTimeout = 2500;
                SqlConnection con = new SqlConnection(builder.ConnectionString);
                System.Data.Common.DbDataReader sqlReader;
                con.Open();
                if (request.Details.amenities != null)
                {
                    amenities = request.Details.amenities != null && request.Details.amenities.Any()
                   ? string.Join(",", request.Details.amenities.Select(a => a.AmenitiesId))
                   : null;
                }

                using (SqlCommand cmd = con.CreateCommand())
                {
                    cmd.CommandText = "Sp_PropertyList";
                    cmd.CommandType = System.Data.CommandType.StoredProcedure;
                    cmd.CommandTimeout = 0;

                    cmd.Parameters.AddRange(new[]
                 {
                 new SqlParameter("@HotelTypeId", request.Details.hotelTypeId),
                 new SqlParameter("@PriceRangeFrom", request.Details.priceRangeFrom),
                 new SqlParameter("@PriceRangeTo", request.Details.priceRangeTo),
                 new SqlParameter("@RatingId", request.Details.ratingId),
                 new SqlParameter("@Amenities", amenities),

                 new SqlParameter("@Page",request.PaginationInfo.Page),
                 new SqlParameter("@PageSize", request.PaginationInfo.RowsPerPage)
                });

                    var adapter = new SqlDataAdapter(cmd);
                    var ds = new DataSet();
                    adapter.Fill(ds);
                    DataTable count = ds.Tables[0];
                    DataTable list = ds.Tables[1];

                    propertylist = list.AsEnumerable()
                        .Select(row => new PropertiesListResponseModel
                        {
                            id = row["ID"] != DBNull.Value ? Convert.ToInt32(row["ID"]) : 0,
                            listName = row["LIST_NAME"] != DBNull.Value ? row["LIST_NAME"].ToString() : string.Empty,
                            listTypeDescription = row["ListType Description"] != DBNull.Value ? row["ListType Description"].ToString() : string.Empty,
                            usageType = row["USAGE_TYPE"] != DBNull.Value ? row["USAGE_TYPE"].ToString() : string.Empty,
                            shortDesc = row["SHORT_DESC"] != DBNull.Value ? row["SHORT_DESC"].ToString() : string.Empty,
                            policyDesc = row["POLICY_DESC"] != DBNull.Value ? row["POLICY_DESC"].ToString() : string.Empty,
                            totalFloor = row["TOTAL_FLOOR"] != DBNull.Value ? row["TOTAL_FLOOR"].ToString() : string.Empty,
                            totalRoom = row["TOTAL_ROOM"] != DBNull.Value ? row["TOTAL_ROOM"].ToString() : string.Empty,
                            roomArea = row["ROOM_AREA"] != DBNull.Value ? row["ROOM_AREA"].ToString() : string.Empty,
                            basePrice = row["BASE_PRICE"] != DBNull.Value ? Convert.ToDecimal(row["BASE_PRICE"]) : 0m,
                            charges = row["CHARGES"] != DBNull.Value ? Convert.ToDecimal(row["CHARGES"]) : 0m,
                            discount = row["DISCOUNT"] != DBNull.Value ? Convert.ToDecimal(row["DISCOUNT"]) : 0m,
                            currencyId = row["CURRENCY_ID"] != DBNull.Value ? Convert.ToInt32(row["CURRENCY_ID"]) : 0,
                            cityName = row["CityName"] != DBNull.Value ? row["CityName"].ToString() : string.Empty,
                            countryName = row["CountryName"] != DBNull.Value ? row["CountryName"].ToString() : string.Empty,
                            stateName = row["StateName"] != DBNull.Value ? row["StateName"].ToString() : string.Empty,
                            rating = row["Rating"] != DBNull.Value ? row["Rating"].ToString() : string.Empty,
                            amenities = _context.PropertyAmenities
                            .Where(pa => pa.PropertyDetailId == Convert.ToInt32(row["ID"]))
                            .Join(_context.Amenities,
                                  pa => pa.AmenityId,
                                  a => a.Id,
                                  (pa, a) => new amenities
                                  {
                                      id = a.Id,
                                      name = a.Amenities
                                  })
                            .ToList()
                        }).ToList();
                    model.propertydetails = propertylist;
                    model.TotalCount = Convert.ToInt32(count.Rows[0]["TotalCount"]);

                    return model;
                }

            }
            catch (ValidationException vx)
            {
                throw new ValidationException(vx.Message != null ? vx.Message.ToString() : "Validation Error");
            }
            catch (Exception ex)
            {
                throw new Exception(ex.InnerException != null ? ex.InnerException.ToString() : "Internal Server Error");

            }
        }
    }
}
