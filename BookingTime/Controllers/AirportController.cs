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
    [ApiController]
    public class AirportController : Controller
    {
        private readonly IConfiguration _configuration;
        private readonly AppDbContext _context;

        public AirportController(IConfiguration configuration, AppDbContext context)
        {
            _configuration = configuration ?? throw new ArgumentNullException(nameof(configuration));
            _context = context ?? throw new ArgumentNullException(nameof(context));
        }

        [HttpGet("/api/GetAllTaxiFleetsizesList")]
        public async Task<IActionResult> GetAllTaxiFleetsizesListAsync()
        {
            try
            {
                var country = await _context.TaxiFleetsizes
                    .Select(x => new { x.Id, x.Name })
                    .ToListAsync();
                return Ok(country);
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
                var types = await _context.TaxiVechiletypes
                    .Select(x => new { x.Id, x.Name, x.Description })
                    .ToListAsync();
                return Ok(types);
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
                var existingTaxi = _context.AirportTaxis.FirstOrDefault(t =>
                    t.VehicleType == request.vehicleType &&
                    t.Country == request.country &&
                    t.City == request.city);

                if (existingTaxi != null)
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
                    CompanyName = request.companyName,
                    OperatingAirport = request.operatingAirport,
                    Country = request.country,
                    City = request.city,
                    State = request.state,
                    BookingPerDay = request.bookingPerDay,
                    FleetSize = request.fleetSize,
                    Website = request.website,
                    VehicleType = request.vehicleType,
                    Capacity = request.capacity,
                    BasePrice = request.basePrice,
                    Currency = Convert.ToInt32(request.currency),
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

                return Ok(new { Message = $"Successfully Created : TaxiId : {detail.Id}", success = true });
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

        [HttpPost("/api/AddAirportTaxiBooking")]
        [EnableCors("AllowAngularApp")]
        public async Task<IActionResult> AddAirportTaxiBookingAysnc([FromBody] AirportTaxiBookingRequestModel request)
        {
            try
            {
                if (request == null)
                    return BadRequest(new { message = "Request body is null", success = false });

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

                detail.PickupDate = request.pickupDate < new DateTime(1753, 1, 1)
                    ? throw new Exception("PickupDate is invalid")
                    : request.pickupDate;

                detail.BookingDate = request.bookingDate < new DateTime(1753, 1, 1)
                    ? throw new Exception("BookingDate is invalid")
                    : request.bookingDate;

                detail.PickupTime = request.pickupTime;

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
            var model = new taxiResponseModeldetails();
            var carDetailsList = new List<AirportTaxiResponseModel>();

            try
            {
                string connectionString = _configuration.GetConnectionString("BookingTimeConnection");

                using (var con = new SqlConnection(connectionString))
                {
                    con.Open();
                    using (var cmd = new SqlCommand("Sp_AirportTaxiList", con))
                    {
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.CommandTimeout = 60;

                        cmd.Parameters.AddWithValue("@taxiId", request.Detail.taxiId);
                        cmd.Parameters.AddWithValue("@cityId", request.Detail.cityId);
                        cmd.Parameters.AddWithValue("@PickupDate", request.Detail.pickUpDate);
                        cmd.Parameters.AddWithValue("@PickupTime", request.Detail.pickUpTime);
                        cmd.Parameters.AddWithValue("@Page", request.PaginationInfo.Page);
                        cmd.Parameters.AddWithValue("@PageSize", request.PaginationInfo.RowsPerPage);

                        var adapter = new SqlDataAdapter(cmd);
                        var ds = new DataSet();
                        adapter.Fill(ds);

                        DataTable count = ds.Tables[0];
                        DataTable list = ds.Tables[1];

                        carDetailsList = list.AsEnumerable().Select(row =>
                        {
                            var vehicleTypeIds = row["vehicletype"] != DBNull.Value
                                ? row["vehicletype"].ToString().Split(',').Select(int.Parse).ToList()
                                : new List<int>();

                            return new AirportTaxiResponseModel
                            {
                                id = Convert.ToInt32(row["ID"]),
                                country = row["CountryName"]?.ToString(),
                                city = row["CityName"]?.ToString(),
                                state = row["StateName"]?.ToString(),
                                companyName = row["companyName"]?.ToString(),
                                operatingAirport = row["operatingAirport"]?.ToString(),
                                bookingPerDay = Convert.ToInt32(row["bookingPerDay"]),
                                fleetSizeName = row["fleetSizeName"]?.ToString(),
                                website = row["website"]?.ToString(),
                                capacity = Convert.ToInt32(row["capacity"]),
                                basePrice = Convert.ToDecimal(row["BasePrice"]),
                                currency = row["Currency"]?.ToString(),
                                symbol = row["CurrencySymbol"]?.ToString(),
                                availabilityStatus = row["availabilityStatus"]?.ToString(),
                                imageUrl = row["imageUrl"]?.ToString(),
                                firstName = row["firstName"]?.ToString(),
                                lastName = row["lastName"]?.ToString(),
                                email = row["email"]?.ToString(),
                                contactNumber = row["contactNumber"]?.ToString(),
                                description = row["description"]?.ToString(),
                                status = row["status"]?.ToString(),
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
                    }
                }

                return model;
            }
            catch (ValidationException vx)
            {
                throw new ValidationException(vx.Message ?? "Validation Error");
            }
            catch (Exception ex)
            {
                throw new Exception(ex.InnerException?.ToString() ?? "Internal Server Error");
            }
        }

        [HttpPost("/api/GetAirportTaxisList")]
        [EnableCors("AllowAngularApp")]
        public async Task<List<AirportTaxisListResponseModel>> GetAirportTaxiList(string cityName = null)
        {
            var taxiVehicles = new List<AirportTaxisListResponseModel>();

            try
            {
                string connectionString = _configuration.GetConnectionString("BookingTimeConnection");

                using (var con = new SqlConnection(connectionString))
                {
                    con.Open();

                    using (var cmd = new SqlCommand("GetTaxiVehicleTypesByCity", con))
                    {
                        cmd.CommandType = CommandType.StoredProcedure;

                        if (!string.IsNullOrEmpty(cityName))
                        {
                            cmd.Parameters.Add(new SqlParameter("@CityName", SqlDbType.NVarChar, 100) { Value = cityName.ToLower().Trim() });
                        }
                        else
                        {
                            cmd.Parameters.Add(new SqlParameter("@CityName", SqlDbType.NVarChar, 100) { Value = DBNull.Value });
                        }

                        using (var reader = cmd.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                var vehicle = new AirportTaxisListResponseModel
                                {
                                    Id =Convert.ToInt32(reader["ID"]),
                                    Name = reader["Name"]?.ToString(),
                                    Description = reader["Description"]?.ToString(),
                                    Capacity = reader["CAPACITY"]?.ToString(),
                                    Suitcase = reader["SUITCASE"]?.ToString(),
                                    CityName = reader["city_name"]?.ToString(),
                                    BasePrice = Convert.ToDecimal(reader["BASE_PRICE"]),
                                    Currency = reader["Currency"].ToString(),
                                    CurrencySymbol = reader["CurrencySymbol"].ToString()
                                };

                                taxiVehicles.Add(vehicle);
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                // Handle exceptions (logging, rethrowing, etc.)
                throw new ApplicationException("Error retrieving airport taxi list", ex);
            }

            return taxiVehicles;
        }

        [HttpPost("/api/AddCitytaxiBasePrice")]
        [EnableCors("AllowAngularApp")]
        public async Task<IActionResult> AddCitytaxiBasePriceAsync(AddCitytaxiBasePriceRequestModel req)
        {
            try
            {
                if (req == null)
                    return BadRequest(new { message = "Request body is null", success = false });

                var existingCity = await _context.CityTaxiBasePrices.FirstOrDefaultAsync(c => c.CityName.ToLower() == req.cityName.ToLower());

                if (existingCity != null)
                {
                    existingCity.BasePrice = req.price;
                    _context.CityTaxiBasePrices.Update(existingCity);
                    await _context.SaveChangesAsync();

                    return Ok(new { Message = $"City '{req.cityName}' already exists. Base price updated.", success = true });
                }
                else
                {
                    var detail = new CityTaxiBasePrice
                    {
                        BasePrice = req.price,
                        CityName = req.cityName,
                        CurrencyId = req.currencyId
                    };
                    await _context.CityTaxiBasePrices.AddAsync(detail);
                    await _context.SaveChangesAsync();

                    return Ok(new { Message = $"Successfully added", success = true });
                }
            }
            catch (ValidationException vx)
            {
                throw new ValidationException(vx.Message ?? "Validation Error");
            }
            catch (Exception ex)
            {
                throw new Exception(ex.InnerException?.ToString() ?? "Internal Server Error");
            }
        }
        private async Task<string> SaveImageAsync(IFormFile? file, string folder = "")
        {
            if (file == null || file.Length == 0)
                return string.Empty;

            string folderPath = _configuration["TaxiImagesPath"] + folder;
            if (!Directory.Exists(folderPath.Trim()))
                Directory.CreateDirectory(folderPath.Trim());

            string fileName = $"{Guid.NewGuid()}_{file.FileName.Trim()}";
            string filePath = Path.Combine(folderPath.Trim(), fileName.Trim());

            using (var stream = new FileStream(filePath.Trim(), FileMode.Create))
            {
                await file.CopyToAsync(stream);
            }

            string relativePath = filePath.Substring(filePath.IndexOf("assets", StringComparison.OrdinalIgnoreCase));
            return relativePath.Replace("\\", "/");
        }
    }
}