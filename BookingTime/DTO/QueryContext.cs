using BookingTime.DTO.ResponseModel;
using Microsoft.EntityFrameworkCore;

namespace BookingTime.DTO
{
    public class QueryContext : DbContext
    {
        public QueryContext(DbContextOptions<QueryContext> options) : base(options) { }

        public DbSet<AvailableCarsListDTO> AvailableCars { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<AvailableCarsListDTO>().HasNoKey();
        }
    }
}
