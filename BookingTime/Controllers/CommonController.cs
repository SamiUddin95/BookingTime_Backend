using BookingTime.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace BookingTime.Controllers
{
    public class CommonController : Controller
    {
        private readonly IConfiguration _configuration;

        public CommonController(IConfiguration configuration)
        {
            _configuration = configuration ?? throw new ArgumentNullException(nameof(configuration));
        }


        [HttpGet("/api/GetAllCountryList")]
        public async Task<IActionResult> GetAllCountryListAsync()
        {
            try
            {
                var connectionString = _configuration.GetConnectionString("BookingTimeConnection");

                using (BookingtimeContext _context = new BookingtimeContext(_configuration))
                {
                    var country = await _context.Countries.Select(x => new { x.CountryId, x.CountryName, x.CountryCode }).ToListAsync();
                    return Ok(country);
                }
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = "Internal Server Error", error = ex.Message });
            }

        }

        [HttpGet("/api/GetStateByCountryId/{countryId}")]
        public async Task<IActionResult> GetStateByCountryIdListAsync(int countryId)
        {
            try
            {
                var connectionString = _configuration.GetConnectionString("BookingTimeConnection");

                using (BookingtimeContext _context = new BookingtimeContext(_configuration))
                {
                    if (countryId == 0)
                    {
                        var state = await _context.States.Select(x => new
                        {
                            x.CountryId,
                            x.StateId,
                            x.StateName
                        }).ToListAsync();
                        return Ok(state);
                    }
                    else
                    {
                        var state = await _context.States.Where(x => x.CountryId == countryId).Select(x => new
                        {
                            x.CountryId,
                            x.StateId,
                            x.StateName
                        }).ToListAsync();
                        return Ok(state);
                    }
                }
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = "Internal Server Error", error = ex.Message });
            }

        }

        [HttpGet("/api/GetCityByCountryId/{countryId}")]
        public async Task<IActionResult> GetCityByCountryIdListAsync(int countryId)
        {
            try
            {
                var connectionString = _configuration.GetConnectionString("BookingTimeConnection");

                using (BookingtimeContext _context = new BookingtimeContext(_configuration))
                {
                    if (countryId == 0)
                    {
                        var Cities = await _context.Cities.Select(x => new
                        {
                            x.CountryId,
                            x.CityId,
                            x.CityName
                        }).ToListAsync();
                        return Ok(Cities);
                    }
                    else
                    {
                        var Cities = await _context.Cities.Where(x => x.CountryId == countryId).Select(x => new
                        {
                            x.CountryId,
                            x.CityId,
                            x.CityName
                        }).ToListAsync();
                        return Ok(Cities);
                    }
                }
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = "Internal Server Error", error = ex.Message });
            }

        }

        [HttpGet("/api/GetAllAmenitiesList")]
        public async Task<IActionResult> GetAllAmenitiesListAsync()
        {
            try
            {
                var connectionString = _configuration.GetConnectionString("BookingTimeConnection");

                using (BookingtimeContext _context = new BookingtimeContext(_configuration))
                {

                    var Amenities = await _context.Amenities.Select(x => new
                    {
                        x.Id,
                        x.Amenities
                    }).ToListAsync();
                    return Ok(Amenities);
                }
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = "Internal Server Error", error = ex.Message });
            }

        }

        [HttpGet("/api/GetAllRatingsList")]
        public async Task<IActionResult> GetAllRatingsListAsync()
        {
            try
            {
                var connectionString = _configuration.GetConnectionString("BookingTimeConnection");

                using (BookingtimeContext _context = new BookingtimeContext(_configuration))
                {

                    var Ratings = await _context.Ratings.Select(x => new
                    {
                        x.Id,
                        x.Ratings
                    }).ToListAsync();
                    return Ok(Ratings);
                }
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = "Internal Server Error", error = ex.Message });
            }

        }

        [HttpGet("/api/GetAllHotelTypesList")]
        public async Task<IActionResult> GetAllHotelTypesListAsync()
        {
            try
            {
                var connectionString = _configuration.GetConnectionString("BookingTimeConnection");

                using (BookingtimeContext _context = new BookingtimeContext(_configuration))
                {

                    var HotelTypes = await _context.HotelTypes.Select(x => new
                    {
                        Id = x.ListTypeId,
                        Name = x.HotelType1,
                        Description = x.Description ?? ""
                    }).ToListAsync();
                    return Ok(HotelTypes);
                }
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = "Internal Server Error", error = ex.Message });
            }

        }

        [HttpGet("/api/GetAllAdditionalInfoList")]
        public async Task<IActionResult> GetAllAdditionalInfoListAsync()
        {
            try
            {
                var connectionString = _configuration.GetConnectionString("BookingTimeConnection");

                using (BookingtimeContext _context = new BookingtimeContext(_configuration))
                {

                    var AdditionalInfo = await _context.AdditionalInfos.Select(x => new
                    {
                        Id = x.Id,
                        Name = x.Name,

                    }).ToListAsync();
                    return Ok(AdditionalInfo);
                }
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = "Internal Server Error", error = ex.Message });
            }

        }



    }
}
