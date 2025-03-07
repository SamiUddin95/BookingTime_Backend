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

        [HttpPost("/api/AddListingProperty")]
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

        [HttpPost("/api/GetListingPropertyList")]
        [EnableCors("AllowAngularApp")]
        public async Task<PropertyResponseModeldetails> GetListingPropertyListAsync([FromBody] PropertiesFilterRequestModel request)
        {
            try
            {
                string? ConnectionString = _configuration.GetConnectionString("BookingTimeConnection");
                List<PropertiesListResponseModel> propertylist = new List<PropertiesListResponseModel>();
                PropertyResponseModeldetails model = new PropertyResponseModeldetails();
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
         new SqlParameter("@HotelTypeId", request.Details.hotelTypeId),
         new SqlParameter("@PriceRangeFrom", request.Details.priceRangeFrom),
         new SqlParameter("@PriceRangeTo", request.Details.priceRangeTo),
         new SqlParameter("@RatingId", request.Details.ratingId),
         new SqlParameter("@Amenities", request.Details.amenities),

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
                            amenities = row["Amenities"] != DBNull.Value ? row["Amenities"].ToString() : string.Empty
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
