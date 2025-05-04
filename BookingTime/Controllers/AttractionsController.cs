using BookingTime.DTO.RequestModel;
using BookingTime.DTO.ResponseModel;
using BookingTime.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace BookingTime.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AttractionsController : ControllerBase
    {
        private readonly AppDbContext _context;
        public AttractionsController(AppDbContext context)
        {
            _context = context;
        }

        [HttpGet("by-city/{cityId}")]
        public async Task<IActionResult> GetByCity(long cityId)
        {
            var attractions = await _context.Attractions
                .Where(a => a.CityId == cityId)
                .Include(a => a.AttractionImages)
                .Include(a => a.City)
                .Select(a => new AttractionDTO
                {
                    Id = a.Id,
                    CityId = a.CityId,
                    Title = a.Title,
                    ShortDescription = a.ShortDescription,
                    DetailedDescription = a.DetailedDescription,
                    Price = a.Price,
                    Rating = a.Rating,
                    CreatedAt = a.CreatedAt,
                    CityName = a.City.CityName,
                    ImageUrls = a.AttractionImages.Select(i => i.ImageUrl).ToList()
                })
                .ToListAsync();

            return Ok(attractions);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            var attraction = await _context.Attractions
                .Where(a => a.Id == id)
                .Include(a => a.AttractionImages)
                .Include(a => a.City)
                .Select(a => new AttractionDTO
                {
                    Id = a.Id,
                    CityId = a.CityId,
                    Title = a.Title,
                    ShortDescription = a.ShortDescription,
                    DetailedDescription = a.DetailedDescription,
                    Price = a.Price,
                    Rating = a.Rating,
                    CreatedAt = a.CreatedAt,
                    CityName = a.City.CityName,
                    ImageUrls = a.AttractionImages.Select(i => i.ImageUrl).ToList()
                })
                .FirstOrDefaultAsync();

            if (attraction == null)
                return NotFound();

            return Ok(attraction);
        }


        [HttpGet("all-destinations")]
        public async Task<IActionResult> GetAllCitiesWithAttractions()
        {
            var data = await _context.VwTopDestinationsByAttractions
                    .OrderByDescending(x => x.AttractionCount)
                    .ToListAsync();

            return Ok(data);
        }

        [HttpGet("top-destinations")]
        public async Task<IActionResult> GetTopCitiesWithAttractions()
        {
            var data = await _context.VwTopDestinationsByAttractions
                    .OrderByDescending(x => x.AttractionCount)
                    .Take(6)
                    .ToListAsync();

            return Ok(data);
        }

        [HttpGet("all-attraction-categories")]
        public async Task<IActionResult> GetAllAttractionCategories()
        {
            var data = await _context.AttractionCategories
                    .Where(c => c.Attractions.Any()) 
                    .Select(c => new AttractionCategoryDTO
                    {
                        Id = c.Id,
                        Name = c.Name,
                        AttractionCount = c.Attractions.Count()
                    })
                    .ToListAsync();

            return Ok(data);
        }

        [HttpPost("filter")]
        public async Task<IActionResult> FilterAttractions([FromBody] AttractionFilterDTO filter)
        {
            var query = _context.Attractions
                .Include(a => a.City)
                .Include(a => a.AttractionImages)
                .Include(a => a.Category)
                .AsQueryable();

            if (filter.CityIds != null && filter.CityIds.Any())
                query = query.Where(a => filter.CityIds.Contains(a.CityId));

            if (filter.CategoryIds != null && filter.CategoryIds.Any())
                query = query.Where(a => a.CategoryId.HasValue && filter.CategoryIds.Contains(a.CategoryId.Value));

            var attractions = await query
                .Select(a => new AttractionDTO
                {
                    Id = a.Id,
                    CityId = a.CityId,
                    Title = a.Title,
                    ShortDescription = a.ShortDescription,
                    DetailedDescription = a.DetailedDescription,
                    Price = a.Price,
                    Rating = a.Rating,
                    CreatedAt = a.CreatedAt,
                    CityName = a.City.CityName,
                    ImageUrls = a.AttractionImages.Select(i => i.ImageUrl).ToList()
                })
                .ToListAsync();

            return Ok(attractions);
        }


    }
}
