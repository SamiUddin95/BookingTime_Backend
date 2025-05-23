using BookingTime.DTO;
using BookingTime.DTO.RequestModel;
using BookingTime.DTO.ResponseModel;
using BookingTime.Models;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using System.Globalization;

namespace BookingTime.Service {
    public interface ICarRentalService {
        Task<object> GetAvailableCarsAsync(CarFilterDTO filter);
    }
    public class CarRentalService : ICarRentalService {

        private readonly AppDbContext _context;
        private readonly QueryContext _queryContext;
        private readonly IFileLoaderService _loaderService;

        public CarRentalService(AppDbContext context, QueryContext queryContext, IFileLoaderService loaderService) {
            _context = context;
            _queryContext = queryContext;
            _loaderService = loaderService;
        }

        public async Task<object> GetAvailableCarsAsync(CarFilterDTO filter) {

            var pickupDate = DateTime.ParseExact(filter.PickupDate + " " + filter.PickupTime, "yyyy-MM-dd HH:mm", CultureInfo.InvariantCulture);
            var dropoffDate = DateTime.ParseExact(filter.DropoffDate + " " + filter.DropoffTime, "yyyy-MM-dd HH:mm", CultureInfo.InvariantCulture);

            var availableCars = await _queryContext.Set<AvailableCarsListDTO>()
                .FromSqlRaw("EXEC Sp_GetAvailableCars @StartDate, @EndDate, @PickupLocationId, @DropoffLocationId",
                    new SqlParameter("@StartDate", pickupDate),
                    new SqlParameter("@EndDate", dropoffDate),
                    new SqlParameter("@PickupLocationId", filter.PickupLocation),
                    new SqlParameter("@DropoffLocationId", filter.DropoffLocation ?? (object)DBNull.Value))
                .ToListAsync();

            foreach(var car in availableCars) {

                car.StartDate = pickupDate;
                car.EndDate = dropoffDate;

                if (!string.IsNullOrEmpty(car.CarImage))
                    car.CarImageBase64 = await _loaderService.LoadFileAsync(car.CarImage);
            }

            var categoriesDict = await _context.CarCategories.ToDictionaryAsync(c => c.Id);
            var makesDict = await _context.VehicleMakes.ToDictionaryAsync(m => m.Id);
            var odometersDict = await _context.OdometerReadings.ToDictionaryAsync(o => o.Id);

            var categoriesWithCount = availableCars
                .GroupBy(c => c.CategoryId)
                .Select(group => new {
                    Id = group.Key,
                    Name = group.Key.HasValue && categoriesDict.TryGetValue(group.Key.Value, out var cat) ? cat.Name : null,
                    Icon = group.Key.HasValue && categoriesDict.TryGetValue(group.Key.Value, out var cat2) ? cat2.Icon : null,
                    Count = group.Count()
                }).ToList();

            var makesWithCount = availableCars
                .GroupBy(c => c.MakeId)
                .Select(group => new {
                    Id = group.Key,
                    Name = group.Key.HasValue && makesDict.TryGetValue(group.Key.Value, out var cat) ? cat.VehicleMake1 : null,
                    Count = group.Count()
                }).ToList();

            var odometersWithCount = availableCars
                .GroupBy(c => c.OdometerId)
                .Select(group => new {
                    Id = group.Key,
                    Name = group.Key.HasValue && odometersDict.TryGetValue(group.Key.Value, out var cat) ? cat.OdometerReading1 : null,
                    Count = group.Count()
                }).ToList();

            var seatsWithCount = availableCars
                .GroupBy(c => c.PassengerCapacity)
                .OrderBy(g => g.Key)
                .Select(group => new {
                    Seats = group.Key,
                    Count = group.Count()
                }).ToList();


            var lowestPrice = availableCars.Min(car => car.TotalRent);
            var highestPrice = availableCars.Max(car => car.TotalRent);

            return new
            {
                Cars = availableCars,
                Categories = categoriesWithCount,
                Makes = makesWithCount,
                OdometerReadings = odometersWithCount,
                PassengerSeats = seatsWithCount,
                PriceRange = new {
                    LowestPrice = lowestPrice,
                    HighestPrice = highestPrice
                }
            };
        }

    }
}
