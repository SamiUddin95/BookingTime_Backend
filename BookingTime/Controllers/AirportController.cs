using BookingTime.DTO.RequestModel;
using BookingTime.Models;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;

namespace BookingTime.Controllers
{
    public class AirportController : Controller
    {
        private readonly IConfiguration _configuration;

        public AirportController(IConfiguration configuration)
        {
            _configuration = configuration ?? throw new ArgumentNullException(nameof(configuration));
        }

        [HttpPost("/api/AddAirportTaxiDetail")]
        [EnableCors("AllowAngularApp")]
        public async Task<IActionResult> AddAirportTaxiAysnc([FromBody] AddAirportTaxiRequestModel request)
        {
            try
            {
                BookingtimeContext _context = new BookingtimeContext(_configuration);

                var taxi = _context.AirportTaxis.Where(taxi => taxi.Email == request.Email &&
                taxi.PickupTime == request.PickupTime && taxi.VehicleType == request.VehicleType && taxi.CountryId == request.CountryId
                && taxi.CityId == request.CityId).FirstOrDefault();

                if (taxi != null)
                {
                    return BadRequest(new { Message = "A booking with the same details already exists.", success = false });
                }

                string imagePath = string.Empty;
                if (request.Image != null)
                {
                    imagePath = await SaveImageAsync(request.Image);
                }

                var detail = new AirportTaxi
                {
                    OperatingAirport = request.OperatingAirport,
                    CountryId = request.CountryId,
                    CityId = request.CityId,
                    BookingPerDay = request.BookingPerDay,
                    FleetSize = request.FleetSize,
                    Website = request.Website,
                    VehicleType = request.VehicleType,
                    Capacity = request.Capacity,
                    BasePrice = request.BasePrice,
                    Currency = request.Currency,
                    AvailabilityStatus = request.AvailabilityStatus,
                    PickupLocation = request.PickupLocation,
                    DropoffLocation = request.DropoffLocation,
                    Description = request.Description,
                    ImageUrl = imagePath,
                    FirstName = request.FirstName,
                    LastName = request.LastName,
                    Email = request.Email,
                    ContactNumber = request.ContactNumber,
                    PickupTime = request.PickupTime,
                    Status = request.Status,
                    TotalPrice = request.TotalPrice,
                    PaymentStatus = request.PaymentStatus,
                    BookedAt = request.BookedAt,
                    CreatedAt = DateTime.Now
                };

                _context.AirportTaxis.Add(detail);
                await _context.SaveChangesAsync();

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
