using BookingTime.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace BookingTime.Controllers
{
    public class CommonController : Controller
    {
        private readonly IConfiguration _configuration;
        private readonly AppDbContext _context;

        public CommonController(IConfiguration configuration, AppDbContext context)
        {
            _configuration = configuration ?? throw new ArgumentNullException(nameof(configuration));
            _context = context;
        }


        [HttpGet("/api/GetAllCountryList")]
        public async Task<IActionResult> GetAllCountryListAsync()
        {
            try
            {
                var country = await _context.Countries.Select(x => new { x.CountryId, x.CountryName, x.CountryCode }).ToListAsync();
                return Ok(country);

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
            catch (Exception ex)
            {
                return StatusCode(500, new { message = "Internal Server Error", error = ex.Message });
            }

        }

        [HttpGet("/api/GetCityByStateId/{stateId}")]
        public async Task<IActionResult> GetCityByStateIdListAsync(int stateId)
        {
            try
            {

                if (stateId == 0)
                {
                    var Cities = await _context.Cities.Select(x => new
                    {
                        x.CountryId,
                        x.CityId,
                        x.CityName,
                        x.StateId
                    }).ToListAsync();
                    return Ok(Cities);
                }
                else
                {
                    var Cities = await _context.Cities.Where(x => x.StateId == stateId).Select(x => new
                    {
                        x.CountryId,
                        x.CityId,
                        x.CityName,
                        x.StateId
                    }).ToListAsync();
                    return Ok(Cities);
                }

            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = "Internal Server Error", error = ex.Message });
            }

        }

        [HttpGet("/api/GetCurrencyBycountryId/{countryId}")]
        public async Task<IActionResult> GetCurrencyBycountryIdListAsync(int countryId)
        {
            try
            {

                if (countryId == 0)
                {
                    var Currency = await _context.Currencies.Select(x => new
                    {
                        x.Id,
                        x.Name,
                        x.Symbol,
                        x.CountryId
                    }).ToListAsync();
                    return Ok(Currency);
                }
                else
                {
                    var Currency = await _context.Currencies.Where(x => x.CountryId == countryId).Select(x => new
                    {
                        x.Id,
                        x.Name,
                        x.Symbol,
                        x.CountryId
                    }).ToListAsync();
                    return Ok(Currency);
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


                var Amenities = await _context.Amenities.Select(x => new
                {
                    x.Id,
                    x.Amenities
                }).ToListAsync();
                return Ok(Amenities);

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


                var Ratings = await _context.Ratings.Select(x => new
                {
                    x.Id,
                    x.Ratings
                }).ToListAsync();
                return Ok(Ratings);

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


                var HotelTypes = await _context.HotelTypes.Select(x => new
                {
                    Id = x.ListTypeId,
                    Name = x.HotelType1,
                    Description = x.Description ?? ""
                }).ToListAsync();
                return Ok(HotelTypes);

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


                var AdditionalInfo = await _context.AdditionalInfos.Select(x => new
                {
                    Id = x.Id,
                    Name = x.Name,

                }).ToListAsync();
                return Ok(AdditionalInfo);

            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = "Internal Server Error", error = ex.Message });
            }

        }

    }
}
