using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;

namespace BookingTime.Models;

public partial class BookingtimeContext : DbContext
{
    private readonly IConfiguration _configuration;
    public BookingtimeContext(IConfiguration configuration)
    {
        _configuration = configuration ?? throw new ArgumentNullException(nameof(configuration));
    }
    // Constructor that takes DbContextOptions
    public BookingtimeContext(DbContextOptions<BookingtimeContext> options)
        : base(options)
    {
    }

    public virtual DbSet<Amenity> Amenities { get; set; }

    public virtual DbSet<City> Cities { get; set; }

    public virtual DbSet<Country> Countries { get; set; }

    public virtual DbSet<HotelType> HotelTypes { get; set; }

    public virtual DbSet<PropertyAmenity> PropertyAmenities { get; set; }

    public virtual DbSet<PropertyDetail> PropertyDetails { get; set; }

    public virtual DbSet<Rating> Ratings { get; set; }

    public virtual DbSet<State> States { get; set; }

    public virtual DbSet<User> Users { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
#warning To protect potentially sensitive information in your connection string, you should move it out of source code. You can avoid scaffolding the connection string by using the Name= syntax to read it from configuration - see https://go.microsoft.com/fwlink/?linkid=2131148. For more guidance on storing connection strings, see http://go.microsoft.com/fwlink/?LinkId=723263.
        => optionsBuilder.UseSqlServer("Server=DESKTOP-HEDM8AQ;Database=BOOKINGTIME;Trusted_Connection=True;TrustServerCertificate=True;");

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Amenity>(entity =>
        {
            entity.Property(e => e.Amenities)
                .HasMaxLength(50)
                .IsUnicode(false);
        });

        modelBuilder.Entity<City>(entity =>
        {
            entity.HasKey(e => e.CityId).HasName("PK__City__F2D21B7614A17A41");

            entity.ToTable("City");

            entity.Property(e => e.CityName).HasMaxLength(100);

            entity.HasOne(d => d.Country).WithMany(p => p.Cities)
                .HasForeignKey(d => d.CountryId)
                .HasConstraintName("FK__City__CountryId__693CA210");
        });

        modelBuilder.Entity<Country>(entity =>
        {
            entity.HasKey(e => e.CountryId).HasName("PK__Country__10D1609FE69D8DDE");

            entity.ToTable("Country");

            entity.HasIndex(e => e.CountryCode, "UQ__Country__5D9B0D2C11B709D1").IsUnique();

            entity.Property(e => e.CountryCode).HasMaxLength(10);
            entity.Property(e => e.CountryName).HasMaxLength(100);
        });

        modelBuilder.Entity<HotelType>(entity =>
        {
            entity.HasKey(e => e.ListTypeId).HasName("PK__HotelTyp__5268C8CAE61B7CBA");

            entity.ToTable("HotelType");

            entity.HasIndex(e => e.HotelType1, "UQ__HotelTyp__F638B67254815968").IsUnique();

            entity.Property(e => e.Description).HasMaxLength(255);
            entity.Property(e => e.HotelType1)
                .HasMaxLength(100)
                .HasColumnName("Hotel_Type");
        });

        modelBuilder.Entity<PropertyAmenity>(entity =>
        {
            entity.ToTable("Property_Amenities");

            entity.Property(e => e.AmenityId).HasColumnName("Amenity_Id");
            entity.Property(e => e.PropertyDetailId).HasColumnName("Property_Detail_Id");
        });

        modelBuilder.Entity<PropertyDetail>(entity =>
        {
            entity.ToTable("PROPERTY_DETAILS");

            entity.Property(e => e.Id).HasColumnName("ID");
            entity.Property(e => e.Amenities)
                .HasMaxLength(200)
                .HasColumnName("AMENITIES");
            entity.Property(e => e.AmenitiesId).HasColumnName("Amenities_Id");
            entity.Property(e => e.BasePrice)
                .HasColumnType("decimal(18, 2)")
                .HasColumnName("BASE_PRICE");
            entity.Property(e => e.CancellationOption)
                .HasMaxLength(50)
                .HasColumnName("CANCELLATION_OPTION");
            entity.Property(e => e.Charges)
                .HasColumnType("decimal(18, 2)")
                .HasColumnName("CHARGES");
            entity.Property(e => e.CityId).HasColumnName("CITY_ID");
            entity.Property(e => e.CountryId).HasColumnName("COUNTRY_ID");
            entity.Property(e => e.CurrencyId).HasColumnName("CURRENCY_ID");
            entity.Property(e => e.Discount)
                .HasColumnType("decimal(18, 2)")
                .HasColumnName("DISCOUNT");
            entity.Property(e => e.Latitude)
                .HasMaxLength(100)
                .HasColumnName("LATITUDE");
            entity.Property(e => e.ListName)
                .HasMaxLength(500)
                .HasColumnName("LIST_NAME");
            entity.Property(e => e.ListTypeId).HasColumnName("LIST_TYPE_ID");
            entity.Property(e => e.LongDesc).HasColumnName("LONG_DESC");
            entity.Property(e => e.Longitude)
                .HasMaxLength(100)
                .HasColumnName("LONGITUDE");
            entity.Property(e => e.PolicyDesc).HasColumnName("POLICY_DESC");
            entity.Property(e => e.PostalCode)
                .HasMaxLength(100)
                .HasColumnName("POSTAL_CODE");
            entity.Property(e => e.RatingId).HasColumnName("Rating_Id");
            entity.Property(e => e.RoomArea)
                .HasMaxLength(50)
                .HasColumnName("ROOM_AREA");
            entity.Property(e => e.ShortDesc).HasColumnName("SHORT_DESC");
            entity.Property(e => e.StateId).HasColumnName("STATE_ID");
            entity.Property(e => e.Street).HasColumnName("STREET");
            entity.Property(e => e.Thumbnail)
                .IsUnicode(false)
                .HasColumnName("THUMBNAIL");
            entity.Property(e => e.TotalFloor)
                .HasMaxLength(50)
                .HasColumnName("TOTAL_FLOOR");
            entity.Property(e => e.TotalRoom)
                .HasMaxLength(50)
                .HasColumnName("TOTAL_ROOM");
            entity.Property(e => e.UsageType)
                .HasMaxLength(10)
                .IsFixedLength()
                .HasColumnName("USAGE_TYPE");

            entity.HasOne(d => d.City).WithMany(p => p.PropertyDetails)
                .HasForeignKey(d => d.CityId)
                .HasConstraintName("FK_PROPERTY_City");

            entity.HasOne(d => d.Country).WithMany(p => p.PropertyDetails)
                .HasForeignKey(d => d.CountryId)
                .OnDelete(DeleteBehavior.Cascade)
                .HasConstraintName("FK_PROPERTY_COUNTRY");

            entity.HasOne(d => d.ListType).WithMany(p => p.PropertyDetails)
                .HasForeignKey(d => d.ListTypeId)
                .HasConstraintName("FK_PROPERTY_Listtype");

            entity.HasOne(d => d.Rating).WithMany(p => p.PropertyDetails)
                .HasForeignKey(d => d.RatingId)
                .HasConstraintName("FK_PROPERTY_DETAILS_Rating");

            entity.HasOne(d => d.State).WithMany(p => p.PropertyDetails)
                .HasForeignKey(d => d.StateId)
                .HasConstraintName("FK_PROPERTY_State");
        });

        modelBuilder.Entity<Rating>(entity =>
        {
            entity.Property(e => e.Ratings)
                .HasMaxLength(10)
                .IsUnicode(false);
        });

        modelBuilder.Entity<State>(entity =>
        {
            entity.HasKey(e => e.StateId).HasName("PK__State__C3BA3B3A12B50E5B");

            entity.ToTable("State");

            entity.HasIndex(e => e.StateCode, "UQ__State__D515E98A72797C71").IsUnique();

            entity.Property(e => e.StateCode).HasMaxLength(10);
            entity.Property(e => e.StateName).HasMaxLength(100);

            entity.HasOne(d => d.Country).WithMany(p => p.States)
                .HasForeignKey(d => d.CountryId)
                .HasConstraintName("FK__State__CountryId__6A30C649");
        });

        modelBuilder.Entity<User>(entity =>
        {
            entity.ToTable("USER");

            entity.Property(e => e.Id).HasColumnName("ID");
            entity.Property(e => e.Email)
                .HasMaxLength(100)
                .IsUnicode(false)
                .HasColumnName("EMAIL");
            entity.Property(e => e.FullName)
                .HasMaxLength(100)
                .IsUnicode(false)
                .HasColumnName("FULL_NAME");
            entity.Property(e => e.IsVerified).HasColumnName("IS_VERIFIED");
            entity.Property(e => e.Password)
                .HasMaxLength(100)
                .IsUnicode(false)
                .HasColumnName("PASSWORD");
            entity.Property(e => e.TokenExpireTime)
                .HasColumnType("datetime")
                .HasColumnName("TOKEN_EXPIRE_TIME");
            entity.Property(e => e.VerificationToken)
                .HasMaxLength(100)
                .IsUnicode(false)
                .HasColumnName("VERIFICATION_TOKEN");
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
