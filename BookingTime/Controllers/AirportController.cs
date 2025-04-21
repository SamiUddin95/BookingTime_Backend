using BookingTime.DTO.RequestModel;
using BookingTime.DTO.ResponseModel;
using BookingTime.Models;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;
using static BookingTime.DTO.ResponseModel.CarDetailsResponseModel;
using System.Data;

namespace BookingTime.Controllers
{
    public class AirportController : Controller
    {
        private readonly IConfiguration _configuration;

        public AirportController(IConfiguration configuration)
        {
            _configuration = configuration ?? throw new ArgumentNullException(nameof(configuration));
        }

        [HttpGet("/api/GetAllTaxiFleetsizesList")]
        public async Task<IActionResult> GetAllTaxiFleetsizesListAsync()
        {
            try
            {
                var connectionString = _configuration.GetConnectionString("BookingTimeConnection");

                using (BookingtimeContext _context = new BookingtimeContext(_configuration))
                {
                    var country = await _context.TaxiFleetsizes.Select(x => new { x.Id, x.Name, }).ToListAsync();
                    return Ok(country);
                }
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = "Internal Server Error", error = ex.Message });
            }

        }
        [HttpGet("/api/GetAllTaxiVechiletypesList")]
        public async Task<IActionResult> GetAllTaxiVechiletypesListAsync()
        {
            try
            {
                var connectionString = _configuration.GetConnectionString("BookingTimeConnection");

                using (BookingtimeContext _context = new BookingtimeContext(_configuration))
                {
                    var country = await _context.TaxiVechiletypes.Select(x => new { x.Id, x.Name, x.Description }).ToListAsync();
                    return Ok(country);
                }
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = "Internal Server Error", error = ex.Message });
            }

        }

        [HttpPost("/api/AddAirportTaxiDetail")]
        [EnableCors("AllowAngularApp")]
        public async Task<IActionResult> AddAirportTaxiAysnc([FromForm] AddAirportTaxiRequestModel request)
        {
            try
            {
                BookingtimeContext _context = new BookingtimeContext(_configuration);

                var taxi = _context.AirportTaxis.Where(taxi => taxi.VehicleType == request.vehicleType && taxi.CountryId == request.countryId
                && taxi.CityId == request.cityId).FirstOrDefault();

                if (taxi != null)
                {
                    return BadRequest(new { Message = "A Taxi with the same details already exists.", success = false });
                }

                string imagePath = string.Empty;
                if (request.image != null)
                {
                    imagePath = await SaveImageAsync(request.image);
                }

                var detail = new AirportTaxi
                {
                    OperatingAirport = request.operatingAirport,
                    CountryId = request.countryId,
                    CityId = request.cityId,
                    StateId = request.stateId,
                    BookingPerDay = request.bookingPerDay,
                    FleetSize = request.fleetSize,
                    Website = request.website,
                    VehicleType = request.vehicleType,
                    Capacity = request.capacity,
                    BasePrice = request.basePrice,
                    Currency = request.currency,
                    AvailabilityStatus = request.availabilityStatus,
                    Description = request.description,
                    ImageUrl = imagePath,
                    FirstName = request.firstName,
                    LastName = request.lastName,
                    Email = request.email,
                    ContactNumber = request.contactNumber,
                    Status = request.status,
                    CreatedAt = DateTime.Now
                };

                _context.AirportTaxis.Add(detail);
                await _context.SaveChangesAsync();

                return Ok(new { Message = $@"Successfully Created : TaxiId : {detail.Id}", success = true });
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

        [HttpPost("/api/AddAirportTaxiBooking")]
        [EnableCors("AllowAngularApp")]
        public async Task<IActionResult> AddAirportTaxiBookingAysnc([FromBody] AirportTaxiBookingRequestModel request)
        {
            try
            {
                if (request == null)
                {
                    return BadRequest(new { message = "Request body is null", success = false });
                }

                BookingtimeContext _context = new BookingtimeContext(_configuration);

                var detail = new AirportTaxiBooking
                {
                    TaxiId = request.taxiId,
                    Description = request.description,
                    PickupLocation = request.pickupLocation,
                    DropoffLocation = request.dropoffLocation,
                    FirstName = request.firstName,
                    LastName = request.lastName,
                    Contact = request.contact,
                    Price = request.price,
                    Email = request.email,
                    CreatedBy = request.createdBy,
                    CreatedAt = DateTime.UtcNow 
                };

                try
                {
                    detail.PickupDate = request.pickupDate < new DateTime(1753, 1, 1) ? throw new Exception("PickupDate is invalid") : request.pickupDate;
                }
                catch (Exception e)
                {
                    return BadRequest(new { message = $"Invalid PickupDate: {request.pickupDate}", error = e.Message });
                }

                try
                {
                    detail.BookingDate = request.bookingDate < new DateTime(1753, 1, 1) ? throw new Exception("BookingDate is invalid") : request.bookingDate;
                }
                catch (Exception e)
                {
                    return BadRequest(new { message = $"Invalid BookingDate: {request.bookingDate}", error = e.Message });
                }

                try
                {
                    detail.PickupTime = request.pickupTime;
                }
                catch (Exception e)
                {
                    return BadRequest(new { message = $"Invalid PickupTime: {request.pickupTime}", error = e.Message });
                }

                _context.AirportTaxiBookings.Add(detail);
                await _context.SaveChangesAsync();

                return Ok(new { Message = $"Successfully Created: BookingId : {detail.Id}", success = true });
            }
            catch (ValidationException vx)
            {
                return BadRequest(new { message = vx.Message, success = false });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = ex.InnerException?.Message ?? ex.Message, success = false });
            }
        }


        [HttpPost("/api/GetAirportTaxiList")]
        [EnableCors("AllowAngularApp")]
        public async Task<taxiResponseModeldetails> GetAirportTaxiDetailsListAsync([FromBody] AirportTaxiDetailsRequestModel request)
        {
            try
            {
                BookingtimeContext _context = new BookingtimeContext(_configuration);
                taxiResponseModeldetails model = new taxiResponseModeldetails();

                string? ConnectionString = _configuration.GetConnectionString("BookingTimeConnection");
                List<AirportTaxiResponseModel> carDetailsList = new List<AirportTaxiResponseModel>();
                SqlConnectionStringBuilder builder = new SqlConnectionStringBuilder(ConnectionString);
                builder.ConnectTimeout = 2500;
                SqlConnection con = new SqlConnection(builder.ConnectionString);
                System.Data.Common.DbDataReader sqlReader;
                con.Open();

                using (SqlCommand cmd = con.CreateCommand())
                {
                    cmd.CommandText = "Sp_AirportTaxiList";
                    cmd.CommandType = System.Data.CommandType.StoredProcedure;
                    cmd.CommandTimeout = 0;

                    cmd.Parameters.AddRange(new[]
                 {
                 new SqlParameter("@taxiId", request.Detail.taxiId),
                 new SqlParameter("@cityId", request.Detail.cityId),
                 new SqlParameter("@PickupDate", request.Detail.pickUpDate),
                 new SqlParameter("@PickupTime", request.Detail.pickUpTime),

                 new SqlParameter("@Page",request.PaginationInfo.Page),
                 new SqlParameter("@PageSize", request.PaginationInfo.RowsPerPage)
                });

                    var adapter = new SqlDataAdapter(cmd);
                    var ds = new DataSet();
                    adapter.Fill(ds);
                    DataTable count = ds.Tables[0];
                    DataTable list = ds.Tables[1];

                    carDetailsList = list.AsEnumerable()
                    .Select(row =>
                    {
                        var vehicleTypeIds = row["vehicletype"] != DBNull.Value
                            ? row["vehicletype"].ToString().Split(',').Select(int.Parse).ToList()
                            : new List<int>();

                        return new AirportTaxiResponseModel
                        {
                            id = row["ID"] != DBNull.Value ? Convert.ToInt32(row["ID"]) : 0,
                            country = row["CountryName"] != DBNull.Value ? row["CountryName"].ToString() : string.Empty,
                            city = row["CityName"] != DBNull.Value ? row["CityName"].ToString() : string.Empty,
                            state = row["StateName"] != DBNull.Value ? row["StateName"].ToString() : string.Empty,
                            operatingAirport = row["operatingAirport"] != DBNull.Value ? row["operatingAirport"].ToString() : string.Empty,
                            bookingPerDay = row["bookingPerDay"] != DBNull.Value ? Convert.ToInt32(row["bookingPerDay"]) : 0,
                            fleetSizeName = row["fleetSizeName"] != DBNull.Value ? row["fleetSizeName"].ToString() : string.Empty,
                            website = row["website"] != DBNull.Value ? row["website"].ToString() : string.Empty,
                            capacity = row["capacity"] != DBNull.Value ? Convert.ToInt32(row["capacity"]) : 0,
                            basePrice = row["BasePrice"] != DBNull.Value ? Convert.ToDecimal(row["BasePrice"]) : 0,
                            currency = row["currency"] != DBNull.Value ? row["currency"].ToString() : string.Empty,
                            availabilityStatus = row["availabilityStatus"] != DBNull.Value ? row["availabilityStatus"].ToString() : string.Empty,
                            imageUrl = row["imageUrl"] != DBNull.Value ? row["imageUrl"].ToString() : string.Empty,
                            firstName = row["firstName"] != DBNull.Value ? row["firstName"].ToString() : string.Empty,
                            lastName = row["lastName"] != DBNull.Value ? row["lastName"].ToString() : string.Empty,
                            email = row["email"] != DBNull.Value ? row["email"].ToString() : string.Empty,
                            contactNumber = row["contactNumber"] != DBNull.Value ? row["contactNumber"].ToString() : string.Empty,
                            description = row["description"] != DBNull.Value ? row["description"].ToString() : string.Empty,
                            status = row["status"] != DBNull.Value ? row["status"].ToString() : string.Empty,

                            vehicleTypes = _context.TaxiVechiletypes
                                .Where(a => vehicleTypeIds.Contains(a.Id))
                                .Select(x => new VehicleTypeDto
                                {
                                    Id = x.Id,
                                    Name = x.Name
                                })
                                .ToList()
                        };
                    }).ToList();
                    model.Taxidetails = carDetailsList;
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


        private async Task<string> SaveImageAsync(IFormFile? file, string folder = "")
        {
            if (file == null || file.Length == 0)
                return string.Empty;

            string folderPath = _configuration["TaxiImagesPath"] + folder;
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


    }
}
