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
using Microsoft.AspNetCore.Mvc.ApplicationModels;
using static BookingTime.DTO.RequestModel.PropertiesFilterRequestModel;
using static BookingTime.DTO.ResponseModel.PropertyReviewsResponseModel;

namespace BookingTime.Controllers
{
    public class ListPropertyController : Controller
    {
        private readonly IConfiguration _configuration;
        public ListPropertyController(IConfiguration configuration)
        {
            _configuration = configuration ?? throw new ArgumentNullException(nameof(configuration));
        }

        /*        [HttpGet("/api/GetListOFProperty")]
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
                }*/

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
                    return BadRequest(new { Message = "Property with these details already exist in the system. Please check your input and try again.", success = false });
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
                    var amenitiesList = new List<PropertyAmenity>();

                    foreach (var amenity in request.Amenities)
                    {
                        if (amenity.amenitiesId.HasValue)
                        {
                            amenitiesList.Add(new PropertyAmenity
                            {
                                PropertyDetailId = property.Id,
                                AmenityId = amenity.amenitiesId.Value
                            });
                        }
                    }

                    _context.PropertyAmenities.AddRange(amenitiesList);
                    await _context.SaveChangesAsync();
                }

                if (request.rooms != null && request.rooms.Count > 0)
                {
                    var rooms = new List<PropertyRoom>();

                    foreach (var room in request.rooms)
                    {
                        rooms.Add(new PropertyRoom
                        {
                            PropertyId = property.Id,
                            Name = room.name,
                            Price = room.price,
                            Discount = room.discount,
                            AdditionalInfoId = room.additionalInfoId,
                            Image = await SaveImageAsync(room.Image, $@"{property.ListName}\Rooms")
                        });
                    }

                    _context.PropertyRooms.AddRange(rooms);
                    await _context.SaveChangesAsync();
                }


                return Ok(new { Message = $@"Successfully Created : PropertyId : {property.Id}", success = true });
            }
            catch (Exception ex)
            {
                throw new Exception(ex.InnerException != null ? ex.InnerException.ToString() : "Internal Server Error");
            }
        }

        private async Task<string> SaveImageAsync(IFormFile? file, string folder = "")
        {
            if (file == null || file.Length == 0)
                return string.Empty;

            string folderPath = _configuration["PropertyImagesPath"] + folder;
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

            string relativePath = filePath.Substring(filePath.IndexOf("assets", StringComparison.OrdinalIgnoreCase));

            return relativePath.Replace("\\", "/");
        }

        [HttpPost("/api/UpdateListingProperty")]
        [EnableCors("AllowAngularApp")]
        public async Task<IActionResult> UpdateProperty(UpdatePropertyDetailsRequestModel request)
        {
            try
            {
                BookingtimeContext _context = new BookingtimeContext(_configuration);

                var property = await _context.PropertyDetails.FirstOrDefaultAsync(p => p.Id == request.PropertyId);

                if (property == null)
                {
                    return NotFound(new { Message = "Property not found.", success = false });
                }

                property.ListTypeId = request.ListTypeId;
                property.ListName = request.ListName;
                property.UsageType = request.UsageType;
                property.ShortDesc = request.ShortDesc;
                property.CountryId = request.CountryId;
                property.StateId = request.StateId;
                property.CityId = request.CityId;
                property.PostalCode = request.PostalCode;
                property.Street = request.Street;
                property.Latitude = request.Latitude;
                property.Longitude = request.Longitude;
                property.LongDesc = request.LongDesc;
                property.TotalFloor = request.TotalFloor;
                property.TotalRoom = request.TotalRoom;
                property.RoomArea = request.RoomArea;
                property.CurrencyId = request.CurrencyId;
                property.BasePrice = request.BasePrice;
                property.Discount = request.Discount;
                property.PolicyDesc = request.PolicyDesc;
                property.CancellationOption = request.CancellationOption;
                property.Charges = request.Charges;
                property.RatingId = request.RatingId;

                if (request.Thumbnail != null)
                {
                    property.Thumbnail = await SaveImageAsync(request.Thumbnail);
                }

                await _context.SaveChangesAsync();

                if (request.Amenities != null)
                {
                    var existingAmenities = _context.PropertyAmenities.Where(a => a.PropertyDetailId == property.Id);
                    _context.PropertyAmenities.RemoveRange(existingAmenities);

                    var amenitiesList = request.Amenities.Select(a => new PropertyAmenity
                    {
                        PropertyDetailId = property.Id,
                        AmenityId = a.amenitiesId.Value
                    }).ToList();

                    _context.PropertyAmenities.AddRange(amenitiesList);
                    await _context.SaveChangesAsync();
                }

                if (request.rooms != null)
                {
                    var existingRooms = _context.PropertyRooms.Where(r => r.PropertyId == property.Id);
                    _context.PropertyRooms.RemoveRange(existingRooms);

                    var rooms = new List<PropertyRoom>();

                    foreach (var room in request.rooms)
                    {
                        rooms.Add(new PropertyRoom
                        {
                            PropertyId = property.Id,
                            Name = room.name,
                            Price = room.price,
                            Discount = room.discount,
                            AdditionalInfoId = room.additionalInfoId,
                            Image = await SaveImageAsync(room.Image, $"{property.ListName}/Rooms")
                        });
                    }

                    _context.PropertyRooms.AddRange(rooms);
                    await _context.SaveChangesAsync();
                }

                return Ok(new { Message = "Property updated successfully.", success = true });
            }
            catch (Exception ex)
            {
                throw new Exception(ex.InnerException != null ? ex.InnerException.ToString() : "Internal Server Error");
            }
        }


        [HttpPost("/api/GetListingPropertyList")]
        [EnableCors("AllowAngularApp")]
        public async Task<PropertyResponseModeldetails> GetListingPropertyListAsync([FromBody] PropertiesFilterRequestModel request)
        {
            try
            {
                BookingtimeContext _context = new BookingtimeContext(_configuration);
                string amenities = null;
                string hotelTypes = null;

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
                   ? string.Join(",", request.Details.amenities.Select(a => a.amenitiesId))
                   : null;
                }
                if (request.Details.hotelTypes != null)
                {
                    amenities = request.Details.hotelTypes != null && request.Details.hotelTypes.Any()
                   ? string.Join(",", request.Details.hotelTypes.Select(h => h.hotelTypeId))
                   : null;
                }

                using (SqlCommand cmd = con.CreateCommand())
                {
                    cmd.CommandText = "Sp_PropertyList";
                    cmd.CommandType = System.Data.CommandType.StoredProcedure;
                    cmd.CommandTimeout = 0;

                    cmd.Parameters.AddRange(new[]
                 {
                 new SqlParameter("@HotelTypeId", hotelTypes),
                 new SqlParameter("@HotelId", null),
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
                            thumbnail = row["Thumbnail"] != DBNull.Value ? row["Thumbnail"].ToString() : string.Empty,
                            featuredImages = row["FeaturedImages"] != DBNull.Value ? row["FeaturedImages"].ToString() : string.Empty,
                            totalReviews = row["TotalReviews"] != DBNull.Value ? Convert.ToInt32(row["TotalReviews"]) : 0,
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
                            .ToList(),
                            rooms = _context.PropertyRooms
                            .Where(pr => pr.PropertyId == Convert.ToInt32(row["ID"]))
                            .Join(_context.AdditionalInfos,
                                  ai => ai.AdditionalInfoId,
                                   a => a.Id,
                                  (pr, a) => new propertyRooms
                                  {
                                      id = pr.Id,
                                      name = pr.Name,
                                      price = pr.Price,
                                      discount = pr.Discount,
                                      additionalInfoId = a.Id,
                                      additionalInfo = a.Name,
                                      thumbnail = pr.Image,
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

        [HttpGet("/api/GetListingPropertyById")]
        [EnableCors("AllowAngularApp")]
        public async Task<PropertiesListResponseModel> GetListingPropertyByIdAsync(int Id)
        {

            try
            {
                BookingtimeContext _context = new BookingtimeContext(_configuration);

                string? ConnectionString = _configuration.GetConnectionString("BookingTimeConnection");
                PropertiesListResponseModel propertylist = new PropertiesListResponseModel();
                SqlConnectionStringBuilder builder = new SqlConnectionStringBuilder(ConnectionString);
                builder.ConnectTimeout = 2500;
                SqlConnection con = new SqlConnection(builder.ConnectionString);
                System.Data.Common.DbDataReader sqlReader;
                con.Open();


                using (SqlCommand cmd = con.CreateCommand())
                {
                    cmd.CommandText = "Sp_PropertyList";
                    cmd.CommandType = System.Data.CommandType.StoredProcedure;
                    cmd.CommandTimeout = 0;

                    cmd.Parameters.AddRange(new[]
                 {
                 new SqlParameter("@HotelTypeId", null),
                 new SqlParameter("@HotelId", Id),
                 new SqlParameter("@PriceRangeFrom", null),
                 new SqlParameter("@PriceRangeTo", null),
                 new SqlParameter("@RatingId", null),
                 new SqlParameter("@Amenities", null),

                 new SqlParameter("@Page",null),
                 new SqlParameter("@PageSize", null)
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
                            thumbnail = row["Thumbnail"] != DBNull.Value ? row["Thumbnail"].ToString() : string.Empty,
                            featuredImages = row["FeaturedImages"] != DBNull.Value ? row["FeaturedImages"].ToString() : string.Empty,
                            totalReviews = row["TotalReviews"] != DBNull.Value ? Convert.ToInt32(row["TotalReviews"]) : 0,
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
                        }).FirstOrDefault();
                    return propertylist;


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

        [HttpGet("/api/GetFeaturedHotel")]
        [EnableCors("AllowAngularApp")]
        public async Task<List<CityGroupedHotelsResponse>> GetFeaturedHotelsAysnc()
        {
            try
            {
                int Records = Convert.ToInt32(_configuration["GetFeaturedHotelRecords"]);
                if (Records == 0)
                    Records = 4;

                List<FeaturedHotelsResponseModel> properties = new List<FeaturedHotelsResponseModel>();
                BookingtimeContext _context = new BookingtimeContext(_configuration);
                string? ConnectionString = _configuration.GetConnectionString("BookingTimeConnection");
                using (SqlConnection con = new SqlConnection(ConnectionString))
                {
                    using (SqlCommand cmd = new SqlCommand("Sp_GetFeaturedHotels", con))
                    {
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Parameters.Add(new SqlParameter("@count", Records));

                        con.Open();
                        using (SqlDataReader reader = await cmd.ExecuteReaderAsync())
                        {
                            while (await reader.ReadAsync())
                            {
                                properties.Add(new FeaturedHotelsResponseModel
                                {
                                    ID = Convert.ToInt32(reader["ID"]),
                                    ListName = reader["LIST_NAME"].ToString(),
                                    ListTypeDescription = reader["ListType Description"].ToString(),
                                    UsageType = reader["USAGE_TYPE"].ToString(),
                                    ShortDesc = reader["SHORT_DESC"].ToString(),
                                    PolicyDesc = reader["POLICY_DESC"].ToString(),
                                    TotalFloor = reader["TOTAL_FLOOR"].ToString(),
                                    TotalRoom = reader["TOTAL_ROOM"].ToString(),
                                    RoomArea = reader["ROOM_AREA"].ToString(),
                                    BasePrice = Convert.ToDecimal(reader["BASE_PRICE"]),
                                    Charges = Convert.ToDecimal(reader["CHARGES"]),
                                    Discount = Convert.ToDecimal(reader["DISCOUNT"]),
                                    CurrencyId = Convert.ToInt32(reader["CURRENCY_ID"]),
                                    CityName = reader["CityName"].ToString(),
                                    CountryName = reader["CountryName"].ToString(),
                                    StateName = reader["StateName"].ToString(),
                                    Rating = reader["Rating"].ToString(),
                                    Thumbnail = reader["Thumbnail"].ToString(),
                                    amenity = _context.PropertyAmenities
                                .Where(pa => pa.PropertyDetailId == Convert.ToInt32(reader["ID"]))
                                .Join(_context.Amenities,
                                      pa => pa.AmenityId,
                                      a => a.Id,
                                      (pa, a) => new amenity1
                                      {
                                          amenityId = a.Id,
                                          amenityName = a.Amenities
                                      })
                                .ToList()
                                });
                            }
                        }
                    }
                }
                var groupedResult = properties
                          .GroupBy(p => p.CityName)
                          .Select(g => new CityGroupedHotelsResponse
                          {
                              CityName = g.Key,
                              Properties = g.ToList()
                          })
                          .ToList();

                return groupedResult;
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

        [HttpGet("/api/GetPopularAttraction")]
        [EnableCors("AllowAngularApp")]
        public async Task<List<CityGroupedPopularAttractionResponse>> GetPopularAttractionListAysnc()
        {
            try
            {
                int Records = Convert.ToInt32(_configuration["GetFeaturedHotelRecords"]);
                if (Records == 0)
                    Records = 4;

                List<PopularAttractionResponseModel> properties = new List<PopularAttractionResponseModel>();
                BookingtimeContext _context = new BookingtimeContext(_configuration);
                string? ConnectionString = _configuration.GetConnectionString("BookingTimeConnection");
                using (SqlConnection con = new SqlConnection(ConnectionString))
                {
                    using (SqlCommand cmd = new SqlCommand("Sp_GetPopularAttraction", con))
                    {
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Parameters.Add(new SqlParameter("@count", Records));

                        con.Open();
                        using (SqlDataReader reader = await cmd.ExecuteReaderAsync())
                        {
                            while (await reader.ReadAsync())
                            {
                                properties.Add(new PopularAttractionResponseModel
                                {
                                    ID = Convert.ToInt32(reader["ID"]),
                                    ListName = reader["LIST_NAME"].ToString(),
                                    ListTypeDescription = reader["ListType Description"].ToString(),
                                    UsageType = reader["USAGE_TYPE"].ToString(),
                                    ShortDesc = reader["SHORT_DESC"].ToString(),
                                    PolicyDesc = reader["POLICY_DESC"].ToString(),
                                    TotalFloor = reader["TOTAL_FLOOR"].ToString(),
                                    TotalRoom = reader["TOTAL_ROOM"].ToString(),
                                    RoomArea = reader["ROOM_AREA"].ToString(),
                                    BasePrice = Convert.ToDecimal(reader["BASE_PRICE"]),
                                    Charges = Convert.ToDecimal(reader["CHARGES"]),
                                    Discount = Convert.ToDecimal(reader["DISCOUNT"]),
                                    CurrencyId = Convert.ToInt32(reader["CURRENCY_ID"]),
                                    CityName = reader["CityName"].ToString(),
                                    CountryName = reader["CountryName"].ToString(),
                                    StateName = reader["StateName"].ToString(),
                                    Reviews = reader["ReviewCount"].ToString(),
                                    Thumbnail = reader["Thumbnail"].ToString(),
                                    amenity = _context.PropertyAmenities
                                .Where(pa => pa.PropertyDetailId == Convert.ToInt32(reader["ID"]))
                                .Join(_context.Amenities,
                                      pa => pa.AmenityId,
                                      a => a.Id,
                                      (pa, a) => new amenity1
                                      {
                                          amenityId = a.Id,
                                          amenityName = a.Amenities
                                      })
                                .ToList()
                                });
                            }
                        }
                    }
                }
                var groupedResult = properties
                          .GroupBy(p => p.CityName)
                          .Select(g => new CityGroupedPopularAttractionResponse
                          {
                              CityName = g.Key,
                              Properties = g.ToList()
                          })
                          .ToList();

                return groupedResult;
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

        [HttpPost("/api/AddPropertyReview")]
        [EnableCors("AllowAngularApp")]
        public async Task<IActionResult> AddPropertyReviewAysnc([FromBody] AddPropertyReviewRequestModel req)
        {
            try
            {
                BookingtimeContext _context = new BookingtimeContext(_configuration);
                var propertyDetails = _context.PropertyDetails.Where(x => x.Id == req.propertyId).FirstOrDefault();
                if (propertyDetails == null)
                    throw new Exception("Property not found");

                var propertyReview = new PropertyReview()
                {
                    PropertyId = req.propertyId,
                    UserId = req.userId,
                    RatingId = req.ratingId,
                    Review = req.review,
                    CreatedOn = DateTime.Now
                };
                _context.PropertyReviews.Add(propertyReview);
                _context.SaveChanges();

                return Ok(new { Message = $@"Successfully added", success = true });
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


        [HttpPut("/api/UpdatePropertyReview")]
        [EnableCors("AllowAngularApp")]
        public async Task<IActionResult> UpdatePropertyReviewAysnc([FromBody] UpdatePropertyReviewRequestModel req)
        {
            try
            {
                BookingtimeContext _context = new BookingtimeContext(_configuration);
                var propertyReview = _context.PropertyReviews.Where(x => x.Id == req.reviewId).FirstOrDefault();
                if (propertyReview == null)
                    throw new Exception("propertyReview not found");

                propertyReview.RatingId = req.ratingId;
                propertyReview.Review = req.review;
                propertyReview.UpdatedOn = DateTime.Now;


                _context.PropertyReviews.Update(propertyReview);
                _context.SaveChanges();

                return Ok(new { Message = $@"Successfully Updated ", success = true });
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


        [HttpPost("/api/AddPropertyFeaturedImages")]
        [EnableCors("AllowAngularApp")]
        public async Task<bool> AddPropertyFeaturedImagesAysnc([FromForm] PropertyFeaturedImagesRequestModel req)
        {
            try
            {
                BookingtimeContext _context = new BookingtimeContext(_configuration);
                List<string> imagePaths = new List<string>();
                var propertyDetails = _context.PropertyDetails.Where(x => x.Id == req.propertyId).FirstOrDefault();
                if (propertyDetails == null)
                    throw new Exception("Property not found");

                //foreach (var img in req.images)
                //{
                //    string path = await SaveImageAsync(img, propertyDetails.ListName.ToLower().Trim());
                //    imagePaths.Add(path);
                //}

//                propertyDetails.Images = string.Join(",", imagePaths);
                _context.PropertyDetails.Update(propertyDetails);
                await _context.SaveChangesAsync();
                return true;
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

        [HttpPost("/api/GetReviewList")]
        [EnableCors("AllowAngularApp")]
        public async Task<PropertyReviewsResponseModeldetails> GetReviewListAysnc([FromBody] ReviewListRequestModel req)
        {
            try
            {
                List<PropertyReviewsResponseModel> reviews = new List<PropertyReviewsResponseModel>();
                PropertyReviewsResponseModeldetails model = new PropertyReviewsResponseModeldetails();
                BookingtimeContext _context = new BookingtimeContext(_configuration);
                string? ConnectionString = _configuration.GetConnectionString("BookingTimeConnection");
                SqlConnectionStringBuilder builder = new SqlConnectionStringBuilder(ConnectionString);
                builder.ConnectTimeout = 2500;
                SqlConnection con = new SqlConnection(builder.ConnectionString);
                System.Data.Common.DbDataReader sqlReader;
                con.Open();
                using (SqlCommand cmd = con.CreateCommand())
                {
                    cmd.CommandText = "Sp_ReviewList";
                    cmd.CommandType = System.Data.CommandType.StoredProcedure;
                    cmd.CommandTimeout = 0;

                    cmd.Parameters.AddRange(new[]
                    {
                 new SqlParameter("@propertyId", req.propertyId),
                 new SqlParameter("@Page",req.PaginationInfo.Page),
                 new SqlParameter("@PageSize", req.PaginationInfo.RowsPerPage)
                });

                    var adapter = new SqlDataAdapter(cmd);
                    var ds = new DataSet();
                    adapter.Fill(ds);
                    DataTable count = ds.Tables[0];
                    DataTable list = ds.Tables[1];

                    reviews = list.AsEnumerable()
                        .Select(row => new PropertyReviewsResponseModel
                        {
                            reviewId = Convert.ToInt32(row["Id"]),
                            userName = row["User Name"].ToString(),
                            rating = Convert.ToInt32(row["Ratings"].ToString()),
                            review = row["Review"].ToString(),
                            reviewDate = Convert.ToDateTime(row["Review Date"].ToString())
                        }).ToList();
                    model.reviewdetails = reviews;
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

        [HttpGet("/api/GetPropertyRatingPercentage/{PropertyId}")]
        [EnableCors("AllowAngularApp")]
        public async Task<IActionResult> GetPropertyRatingPercentageAysnc(int PropertyId)
        {
            try
            {
                BookingtimeContext _context = new BookingtimeContext(_configuration);
                var propertyId = new SqlParameter("@PropertyId", PropertyId);
                var results = new List<RatingPercentageResponseModel>();

                using (var command = _context.Database.GetDbConnection().CreateCommand())
                {
                    command.CommandText = "EXEC GetPropertyRatingPercentage @PropertyId";
                    command.Parameters.Add(propertyId);
                    command.CommandType = System.Data.CommandType.Text;

                    _context.Database.OpenConnection();

                    using (var reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            results.Add(new RatingPercentageResponseModel
                            {
                                Rating = reader.GetString(0),
                                Percentage = reader.GetDecimal(1)
                            });
                        }
                    }
                }


                return Ok(results);
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
