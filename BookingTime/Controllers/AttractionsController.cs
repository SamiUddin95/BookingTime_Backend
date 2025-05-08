using BookingTime.DTO.RequestModel;
using BookingTime.DTO.ResponseModel;
using BookingTime.Models;
using BookingTime.Service;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;

namespace BookingTime.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AttractionsController : ControllerBase
    {
        private readonly AppDbContext _context;
        private readonly IConfiguration _configuration;
        private readonly IFileLoaderService _fileLoader;

        public AttractionsController(AppDbContext context, IConfiguration configuration, IFileLoaderService fileLoaderService)
        {
            _context = context;
            _configuration = configuration;
            _fileLoader = fileLoaderService;
        }

        [HttpGet("by-city/{cityId}")]
        public async Task<IActionResult> GetByCity(long cityId)
        {
            var attractions = await _context.Attractions
                .Where(a => a.CityId == cityId)
                .Include(a => a.AttractionImages)
                .Include(a => a.City)
                    .ThenInclude(c => c.Country)
                        .ThenInclude(c => c.Currency)
                .ToListAsync();

            var attractionDtos = new List<AttractionDTO>();

            foreach (var attraction in attractions)
            {
                var attractionDto = new AttractionDTO
                {
                    Id = attraction.Id,
                    CityId = attraction.CityId,
                    Title = attraction.Title,
                    ShortDescription = attraction.ShortDescription,
                    DetailedDescription = attraction.DetailedDescription,
                    Price = $"{attraction.City.Country.Currency.Symbol} {attraction.Price}",
                    Rating = attraction.Rating,
                    CreatedAt = attraction.CreatedAt,
                    CityName = attraction.City.CityName,
                    Images = new List<string>()
                };

                foreach (var img in attraction.AttractionImages)
                {
                    var base64Image = await _fileLoader.LoadFileAsync(img.ImageUrl);
                    if (base64Image != null)
                        attractionDto.Images.Add(base64Image);
                }

                attractionDtos.Add(attractionDto);
            }

            return Ok(attractionDtos);
        }


        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            var attractionEntity = await _context.Attractions
                .Where(a => a.Id == id)
                .Include(a => a.AttractionImages)
                .Include(a => a.City)
                    .ThenInclude(c => c.Country)
                        .ThenInclude(c => c.Currency)
                .FirstOrDefaultAsync();

            if (attractionEntity == null)
                return NotFound();

            var attractionDto = new AttractionDTO
            {
                Id = attractionEntity.Id,
                CityId = attractionEntity.CityId,
                Title = attractionEntity.Title,
                ShortDescription = attractionEntity.ShortDescription,
                DetailedDescription = attractionEntity.DetailedDescription,
                Price = $"{attractionEntity.City.Country.Currency.Symbol} {attractionEntity.Price}",
                Rating = attractionEntity.Rating,
                CreatedAt = attractionEntity.CreatedAt,
                CityName = attractionEntity.City.CityName,
            };

            foreach (var img in attractionEntity.AttractionImages)
            {
                var base64Image = await _fileLoader.LoadFileAsync(img.ImageUrl);
                if (base64Image != null)
                    attractionDto.Images.Add(base64Image);
            }

            return Ok(attractionDto);
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
                    .ThenInclude(c => c.Country)
                        .ThenInclude(c => c.Currency)
                .Include(a => a.AttractionImages)
                .Include(a => a.Category)
                .AsQueryable();

            if (filter.CityIds != null && filter.CityIds.Any())
                query = query.Where(a => filter.CityIds.Contains(a.CityId));

            if (filter.CategoryIds != null && filter.CategoryIds.Any())
                query = query.Where(a => a.CategoryId.HasValue && filter.CategoryIds.Contains(a.CategoryId.Value));

            var attractions = await query.ToListAsync();

            var attractionDtos = new List<AttractionDTO>();

            foreach (var attraction in attractions)
            {
                var dto = new AttractionDTO
                {
                    Id = attraction.Id,
                    CityId = attraction.CityId,
                    Title = attraction.Title,
                    ShortDescription = attraction.ShortDescription,
                    DetailedDescription = attraction.DetailedDescription,
                    Price = $"{attraction.City.Country.Currency.Symbol} {attraction.Price}",
                    Rating = attraction.Rating,
                    CreatedAt = attraction.CreatedAt,
                    CityName = attraction.City.CityName,
                    Images = new List<string>()
                };

                var base64Tasks = attraction.AttractionImages
                    .Select(img => _fileLoader.LoadFileAsync(img.ImageUrl));

                var base64Images = await Task.WhenAll(base64Tasks);

                dto.Images.AddRange(base64Images.Where(img => img != null));

                attractionDtos.Add(dto);
            }


            return Ok(attractionDtos);
        }

        [HttpPost]
        public async Task<IActionResult> CreateAttraction([FromForm] CreateAttractionDTO dto)
        {
            var attraction = new Attraction
            {
                CityId = dto.CityId,
                Title = dto.Title,
                ShortDescription = dto.ShortDescription,
                DetailedDescription = dto.DetailedDescription,
                Price = dto.Price,
                Rating = dto.Rating,
                CategoryId = dto.CategoryId,
                CreatedAt = DateTime.UtcNow,
            };

            string rootPath = _configuration["AttractionImagesPath"];
            Directory.CreateDirectory(rootPath);

            foreach (var image in dto.Images)
            {
                if (image.Length > 0)
                {
                    var fileName = $"{Guid.NewGuid()}{Path.GetExtension(image.FileName)}";
                    var fullPath = Path.Combine(rootPath, fileName);

                    using (var stream = new FileStream(fullPath, FileMode.Create))
                    {
                        await image.CopyToAsync(stream);
                    }

                    attraction.AttractionImages.Add(new AttractionImage
                    {
                        ImageUrl = fullPath 
                    });
                }
            }

            _context.Attractions.Add(attraction);
            await _context.SaveChangesAsync();

            return CreatedAtAction(nameof(GetById), new { id = attraction.Id }, new { attraction.Id });
        }



    }
}
