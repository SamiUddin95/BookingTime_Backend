using BookingTime.DTO.RequestModel;
using BookingTime.DTO.ResponseModel;
using BookingTime.Models;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using Newtonsoft.Json;
using System.ComponentModel.DataAnnotations;
using static BookingTime.DTO.ResponseModel.PropertiesListResponseModel;
using System.Data;
using static BookingTime.DTO.ResponseModel.CarDetailsResponseModel;
using static System.Net.Mime.MediaTypeNames;

namespace BookingTime.Controllers
{
    public class CarController : Controller
    {
        private readonly IConfiguration _configuration;

        public CarController(IConfiguration configuration)
        {
            _configuration = configuration ?? throw new ArgumentNullException(nameof(configuration));
        }


        [HttpGet("/api/GetAllFuelTypeList")]
        public async Task<IActionResult> GetAllFuelTypeListAsync()
        {
            try
            {
                var connectionString = _configuration.GetConnectionString("BookingTimeConnection");

                using (BookingtimeContext _context = new BookingtimeContext(_configuration))
                {

                    var FuelType = await _context.FuelTypes.Select(x => new
                    {
                        Id = x.Id,
                        Name = x.FuelType1,

                    }).ToListAsync();
                    return Ok(FuelType);
                }
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = "Internal Server Error", error = ex.Message });
            }

        }

        [HttpGet("/api/GetAllOdometerReadingList")]
        public async Task<IActionResult> GetAllOdometerReadingListAsync()
        {
            try
            {
                var connectionString = _configuration.GetConnectionString("BookingTimeConnection");

                using (BookingtimeContext _context = new BookingtimeContext(_configuration))
                {

                    var OdometerReading = await _context.OdometerReadings.Select(x => new
                    {
                        Id = x.Id,
                        Name = x.OdometerReading1,

                    }).ToListAsync();
                    return Ok(OdometerReading);
                }
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = "Internal Server Error", error = ex.Message });
            }

        }

        [HttpGet("/api/GetAllSeatbeltTypeList")]
        public async Task<IActionResult> GetAllSeatbeltTypeListAsync()
        {
            try
            {
                var connectionString = _configuration.GetConnectionString("BookingTimeConnection");

                using (BookingtimeContext _context = new BookingtimeContext(_configuration))
                {

                    var SeatbeltType = await _context.SeatbeltTypes.Select(x => new
                    {
                        Id = x.Id,
                        Name = x.SeatbeltType1,

                    }).ToListAsync();
                    return Ok(SeatbeltType);
                }
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = "Internal Server Error", error = ex.Message });
            }

        }

        [HttpGet("/api/GetAllVehicleConditionList")]
        public async Task<IActionResult> GetAllVehicleConditionListAsync()
        {
            try
            {
                var connectionString = _configuration.GetConnectionString("BookingTimeConnection");

                using (BookingtimeContext _context = new BookingtimeContext(_configuration))
                {

                    var VehicleCondition = await _context.VehicleConditions.Select(x => new
                    {
                        Id = x.Id,
                        Name = x.Condition,

                    }).ToListAsync();
                    return Ok(VehicleCondition);
                }
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = "Internal Server Error", error = ex.Message });
            }

        }

        [HttpGet("/api/GetAllVehicleMakeList")]
        public async Task<IActionResult> GetAllVehicleMakeListAsync()
        {
            try
            {
                var connectionString = _configuration.GetConnectionString("BookingTimeConnection");

                using (BookingtimeContext _context = new BookingtimeContext(_configuration))
                {

                    var VehicleMake = await _context.VehicleMakes.Select(x => new
                    {
                        Id = x.Id,
                        Name = x.VehicleMake1,

                    }).ToListAsync();
                    return Ok(VehicleMake);
                }
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = "Internal Server Error", error = ex.Message });
            }

        }

        [HttpGet("/api/GetAllVehicleYearList")]
        public async Task<IActionResult> GetAllVehicleYearListAsync()
        {
            try
            {
                var connectionString = _configuration.GetConnectionString("BookingTimeConnection");

                using (BookingtimeContext _context = new BookingtimeContext(_configuration))
                {

                    var VehicleYear = await _context.VehicleYears.Select(x => new
                    {
                        Id = x.Id,
                        Name = x.VehicleYear1,

                    }).ToListAsync();
                    return Ok(VehicleYear);
                }
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = "Internal Server Error", error = ex.Message });
            }

        }

        [HttpPost("/api/AddCarDetails")]
        [EnableCors("AllowAngularApp")]
        public async Task<IActionResult> AddCarDetailsAysnc([FromForm] AddCarDetailsRequestModel request)
        {
            try
            {
                BookingtimeContext _context = new BookingtimeContext(_configuration);

                string imagePath = string.Empty;
                var existingCarDetails = await _context.CarDetails
           .Where(p => p.Vin == request.vin &&
                       p.YearId == request.yearId &&
                       p.MakeId == request.makeId &&
                       p.Model == request.model &&
                       p.OdometerId == request.odometerId &&
                       p.SeatbeltTypeId == request.seatbeltTypeId &&
                       p.MobileNumber1 == request.mobileNumber1 &&
                       p.MileageLimit == request.mileageLimit &&
                       p.FuelTypeId == request.fuelTypeId &&
                       p.Features == request.features)
           .FirstOrDefaultAsync();

                if (existingCarDetails != null)
                {
                    return BadRequest(new { Message = "Car with these details already exist in the system. Please check your input and try again.", sucess = false });
                }
                if (request.image != null)
                {
                    imagePath = await SaveImageAsync(request.image);
                }

                var detail = new CarDetail
                {
                    CountyId = request.countryId,
                    CityId = request.cityId,
                    StateId = request.stateId,
                    Street = request.street,
                    Vin = request.vin,
                    YearId = request.yearId,
                    MakeId = request.makeId,
                    Model = request.model,
                    PassengerCapacity = request.capacityId,
                    BasePrice = request.basePrice,
                    OdometerId = request.odometerId,
                    VehicleValue = request.vehicleValue,
                    VehicleConditionId = request.vehicleConditionId,
                    Seatbelts = request.seatbelts,
                    SeatbeltTypeId = request.seatbeltTypeId == 0 || request.seatbeltTypeId == null ? null : request.seatbeltTypeId,
                    MobileNumber1 = request.mobileNumber1,
                    MobileNumber2 = request.mobileNumber2,
                    StartDate = request.startDate,
                    EndDate = request.endDate,
                    StartTime = request.startTime,
                    EndTime = request.endTime,
                    MileageLimit = request.mileageLimit,
                    Features = string.IsNullOrEmpty(request.features) ? null : request.features,
                    Transmission = request.transmission,
                    AdditionalInfo = request.additionalInfo,
                    FuelTypeId = request.fuelTypeId,
                    Photos = imagePath
                };

                _context.CarDetails.Add(detail);
                await _context.SaveChangesAsync();

                if (request.carImages != null && request.carImages.Count > 0)
                {
                    var cars = new List<CarImage>();

                    foreach (var car in request.carImages)
                    {
                        cars.Add(new CarImage
                        {
                            CarId = detail.Id,
                            ImagePath = await SaveImageAsync(car)
                        });
                    }

                    _context.CarImages.AddRange(cars);
                    await _context.SaveChangesAsync();
                }

                return Ok(new { Message = $@"Successfully Created : CarId : {detail.Id}", success = true });
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

        [HttpPost("/api/GetCarDetailsList")]
        [EnableCors("AllowAngularApp")]
        public async Task<CarDetailsResponseModeldetails> GetCarDetailsListAsync([FromBody] CarDetailsRequestModel request)
        {
            try
            {
                BookingtimeContext _context = new BookingtimeContext(_configuration);

                string? ConnectionString = _configuration.GetConnectionString("BookingTimeConnection");
                List<CarDetailsResponseModel> carDetailsList = new List<CarDetailsResponseModel>();
                CarDetailsResponseModeldetails model = new CarDetailsResponseModeldetails();
                SqlConnectionStringBuilder builder = new SqlConnectionStringBuilder(ConnectionString);
                builder.ConnectTimeout = 2500;
                SqlConnection con = new SqlConnection(builder.ConnectionString);
                System.Data.Common.DbDataReader sqlReader;
                con.Open();

                using (SqlCommand cmd = con.CreateCommand())
                {
                    cmd.CommandText = "Sp_CarDetailsList_Updated";
                    cmd.CommandType = System.Data.CommandType.StoredProcedure;
                    cmd.CommandTimeout = 0;

                    cmd.Parameters.AddRange(new[]
                 {
                 new SqlParameter("@CarId", request.Details.carId),
                 new SqlParameter("@MakeId", request.Details.makeId),
                 new SqlParameter("@YearId", request.Details.yearId),
                 new SqlParameter("@ConditionId", request.Details.conditionId),
                 new SqlParameter("@FuelTypeId", request.Details.fuelTypeId),
                 new SqlParameter("@Model", request.Details.model),
                 new SqlParameter("@MileageLimit", request.Details.mileageLimit),

                 new SqlParameter("@PickUpLocationId", request.Details.pickUpLocationId),
                 new SqlParameter("@DropOffLocationId", request.Details.dropOffLocationId),
                 new SqlParameter("@PickupDate", request.Details.pickUpDate),
                 new SqlParameter("@PickupTime", request.Details.pickUpTime),
                 new SqlParameter("@ReturnDate", request.Details.returnDate),
                 new SqlParameter("@ReturnTime", request.Details.returnTime),

                 new SqlParameter("@Page",request.PaginationInfo.Page),
                 new SqlParameter("@PageSize", request.PaginationInfo.RowsPerPage)
                });

                    var adapter = new SqlDataAdapter(cmd);
                    var ds = new DataSet();
                    adapter.Fill(ds);
                    DataTable count = ds.Tables[0];
                    DataTable list = ds.Tables[1];

                    carDetailsList = list.AsEnumerable()
                        .Select(row => new CarDetailsResponseModel
                        {
                            id = row["ID"] != DBNull.Value ? Convert.ToInt32(row["ID"]) : 0,
                            location = row["Location"] != DBNull.Value ? row["Location"].ToString() : string.Empty,
                            vin = row["VIN"] != DBNull.Value ? row["VIN"].ToString() : string.Empty,
                            vehicleYear = row["VehicleYear"] != DBNull.Value ? row["VehicleYear"].ToString() : string.Empty,
                            vehicleMake = row["VehicleMake"] != DBNull.Value ? row["VehicleMake"].ToString() : string.Empty,
                            model = row["Model"] != DBNull.Value ? row["Model"].ToString() : string.Empty,
                            odometerReading = row["OdometerReading"] != DBNull.Value ? row["OdometerReading"].ToString() : string.Empty,
                            vehicleValue = row["VehicleValue"] != DBNull.Value ? Convert.ToString(row["VehicleValue"]) : string.Empty,
                            vehicleCondition = row["VehicleCondition"] != DBNull.Value ? row["VehicleCondition"].ToString() : string.Empty,
                            seatblets = row["Seatbelts"] != DBNull.Value && Convert.ToBoolean(row["Seatbelts"]),
                            seatbletType = row["SeatbeltType"] != DBNull.Value ? row["SeatbeltType"].ToString() : string.Empty,
                            mobileNumber1 = row["MobileNumber1"] != DBNull.Value ? row["MobileNumber1"].ToString() : string.Empty,
                            mobileNumber2 = row["MobileNumber2"] != DBNull.Value ? row["MobileNumber2"].ToString() : string.Empty,
                            startDate = row["StartDate"] != DBNull.Value ? Convert.ToDateTime(row["StartDate"]) : DateTime.MinValue,
                            endDate = row["EndDate"] != DBNull.Value ? Convert.ToDateTime(row["EndDate"]) : DateTime.MinValue,
                            mileageLimit = row["MileageLimit"] != DBNull.Value ? Convert.ToInt32(row["MileageLimit"]) : 0,
                            fuelType = row["FuelType"] != DBNull.Value ? row["FuelType"].ToString() : string.Empty,
                            features = row["Features"] != DBNull.Value ? row["Features"].ToString() : string.Empty,
                            thumbnail = row["Photos"] != DBNull.Value ? row["Photos"].ToString() : string.Empty,
                            images = _context.CarImages
                            .Where(pa => pa.CarId == Convert.ToInt32(row["ID"]))
                            .Select(x => new image
                            {
                                carImages = x.ImagePath
                            })
                            .ToList()
                        }).ToList();
                    model.cardetails = carDetailsList;
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


        [HttpPost("/api/AddCarBookingDetail")]
        [EnableCors("AllowAngularApp")]
        public async Task<IActionResult> AddCarBookingDetailAysnc([FromBody] AddCarBookingDetailRequestModel request)
        {
            try
            {
                BookingtimeContext _context = new BookingtimeContext(_configuration);
                var detail = new CarBookingDetail
                {
                    CarId = request.carId,
                    PickupAddress = request.pickupAddress,
                    DropoffAddress = request.dropOffAddress,
                    PickupDate = request.pickUpDate,
                    PickupTime = request.pickUpTime,
                    ReturnDate = request.returnDate,
                    ReturnTime = request.returnTime,
                    TotalAmount = request.totalAmount,
                    Distance = request.distance,
                    Luggages = request.luggages,
                    Passengers = request.passengers,
                    BookingDate = DateTime.Now,
                    CreatedBy = request.userId
                };

                _context.CarBookingDetails.Add(detail);
                await _context.SaveChangesAsync();

                if (detail.Id > 0)
                {
                    var passengerDetail = new CarBookingPassengerDetail
                    {
                        BookingDetailId = detail.Id,
                        Name = request.name,
                        Email = request.email,
                        PhoneNumber = request.phoneNumber,
                    };

                    _context.CarBookingPassengerDetails.Add(passengerDetail);
                    await _context.SaveChangesAsync();
                }
                return Ok(new { Message = $@"Successfully Created : BookingId : {detail.Id}", success = true });
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

            string folderPath = _configuration["CarImagesPath"] + folder;
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
