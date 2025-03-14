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
                    Location = request.location,
                    Vin = request.vin,
                    YearId = request.yearId,
                    MakeId = request.makeId,
                    Model = request.model,
                    OdometerId = request.odometerId,
                    VehicleValue = request.vehicleValue,
                    VehicleConditionId= request.vehicleConditionId,
                    Seatbelts = request.seatbelts,
                    SeatbeltTypeId = request.seatbeltTypeId,
                    MobileNumber1 = request.mobileNumber1,
                    MobileNumber2 = request.mobileNumber2,
                    StartDate = request.startDate,
                    EndDate = request.endDate,
                    MileageLimit = request.mileageLimit,
                    Features = request.features,
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

                return Ok(new { Message = $@"Successfully Created : CarId : {detail.Id}", sucess = true });
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
