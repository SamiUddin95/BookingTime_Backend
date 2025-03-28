﻿using System;
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

    public virtual DbSet<AdditionalInfo> AdditionalInfos { get; set; }

    public virtual DbSet<AirportTaxi> AirportTaxis { get; set; }

    public virtual DbSet<Amenity> Amenities { get; set; }

    public virtual DbSet<CarBookingDetail> CarBookingDetails { get; set; }

    public virtual DbSet<CarBookingPassengerDetail> CarBookingPassengerDetails { get; set; }

    public virtual DbSet<CarDetail> CarDetails { get; set; }

    public virtual DbSet<CarImage> CarImages { get; set; }

    public virtual DbSet<City> Cities { get; set; }

    public virtual DbSet<Country> Countries { get; set; }

    public virtual DbSet<FuelType> FuelTypes { get; set; }

    public virtual DbSet<HotelType> HotelTypes { get; set; }

    public virtual DbSet<OdometerReading> OdometerReadings { get; set; }

    public virtual DbSet<PropertyAmenity> PropertyAmenities { get; set; }

    public virtual DbSet<PropertyDetail> PropertyDetails { get; set; }

    public virtual DbSet<PropertyReview> PropertyReviews { get; set; }

    public virtual DbSet<PropertyRoom> PropertyRooms { get; set; }

    public virtual DbSet<Rating> Ratings { get; set; }

    public virtual DbSet<RoomImage> RoomImages { get; set; }

    public virtual DbSet<SeatbeltType> SeatbeltTypes { get; set; }

    public virtual DbSet<State> States { get; set; }

    public virtual DbSet<User> Users { get; set; }

    public virtual DbSet<VehicleCondition> VehicleConditions { get; set; }

    public virtual DbSet<VehicleMake> VehicleMakes { get; set; }

    public virtual DbSet<VehicleYear> VehicleYears { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
#warning To protect potentially sensitive information in your connection string, you should move it out of source code. You can avoid scaffolding the connection string by using the Name= syntax to read it from configuration - see https://go.microsoft.com/fwlink/?linkid=2131148. For more guidance on storing connection strings, see http://go.microsoft.com/fwlink/?LinkId=723263.
        => optionsBuilder.UseSqlServer("Server=DESKTOP-HEDM8AQ;Database=BOOKINGTIME;Trusted_Connection=True;TrustServerCertificate=True;");

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<AdditionalInfo>(entity =>
        {
            entity.ToTable("ADDITIONAL_INFO");

            entity.Property(e => e.Id).HasColumnName("ID");
            entity.Property(e => e.Name)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasColumnName("NAME");
        });

        modelBuilder.Entity<AirportTaxi>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__Airport___3213E83F7E5696EE");

            entity.ToTable("Airport_Taxi");

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.AvailabilityStatus)
                .HasMaxLength(20)
                .HasDefaultValueSql("('available')")
                .HasColumnName("Availability_Status");
            entity.Property(e => e.BasePrice)
                .HasColumnType("decimal(10, 2)")
                .HasColumnName("Base_Price");
            entity.Property(e => e.BookedAt)
                .HasDefaultValueSql("(getdate())")
                .HasColumnName("Booked_At");
            entity.Property(e => e.BookingPerDay).HasColumnName("Booking_Per_Day");
            entity.Property(e => e.CityId).HasColumnName("City_id");
            entity.Property(e => e.ContactNumber).HasColumnName("Contact_Number");
            entity.Property(e => e.CountryId).HasColumnName("Country_id");
            entity.Property(e => e.CreatedAt)
                .HasDefaultValueSql("(getdate())")
                .HasColumnName("Created_At");
            entity.Property(e => e.Currency)
                .HasMaxLength(3)
                .HasDefaultValueSql("('USD')");
            entity.Property(e => e.DropoffLocation)
                .HasMaxLength(255)
                .HasColumnName("Dropoff_Location");
            entity.Property(e => e.Email).HasMaxLength(100);
            entity.Property(e => e.FirstName)
                .HasMaxLength(100)
                .HasColumnName("First_Name");
            entity.Property(e => e.FleetSize)
                .HasMaxLength(50)
                .HasColumnName("Fleet_Size");
            entity.Property(e => e.ImageUrl)
                .HasMaxLength(255)
                .HasColumnName("Image_Url");
            entity.Property(e => e.LastName)
                .HasMaxLength(100)
                .HasColumnName("Last_Name");
            entity.Property(e => e.OperatingAirport)
                .HasMaxLength(255)
                .HasColumnName("Operating_Airport");
            entity.Property(e => e.PaymentStatus)
                .HasMaxLength(20)
                .HasDefaultValueSql("('Unpaid')")
                .HasColumnName("Payment_Status");
            entity.Property(e => e.PickupLocation)
                .HasMaxLength(255)
                .HasColumnName("Pickup_Location");
            entity.Property(e => e.PickupTime).HasColumnName("Pickup_Time");
            entity.Property(e => e.Status)
                .HasMaxLength(20)
                .HasDefaultValueSql("('Pending')");
            entity.Property(e => e.TotalPrice)
                .HasColumnType("decimal(10, 2)")
                .HasColumnName("Total_Price");
            entity.Property(e => e.UpdatedAt)
                .HasDefaultValueSql("(getdate())")
                .HasColumnName("Updated_At");
            entity.Property(e => e.VehicleType)
                .HasMaxLength(50)
                .HasColumnName("Vehicle_Type");
            entity.Property(e => e.Website).HasMaxLength(50);

            entity.HasOne(d => d.City).WithMany(p => p.AirportTaxis)
                .HasForeignKey(d => d.CityId)
                .HasConstraintName("FK_Airport_Taxi_Cities");

            entity.HasOne(d => d.Country).WithMany(p => p.AirportTaxis)
                .HasForeignKey(d => d.CountryId)
                .HasConstraintName("FK_Airport_Taxi_Countries");
        });

        modelBuilder.Entity<Amenity>(entity =>
        {
            entity.Property(e => e.Amenities)
                .HasMaxLength(50)
                .IsUnicode(false);
        });

        modelBuilder.Entity<CarBookingDetail>(entity =>
        {
            entity.ToTable("CAR_BOOKING_DETAILS");

            entity.Property(e => e.Id).HasColumnName("ID");
            entity.Property(e => e.BookingDate)
                .HasColumnType("datetime")
                .HasColumnName("BOOKING_DATE");
            entity.Property(e => e.CarId).HasColumnName("CAR_ID");
            entity.Property(e => e.CreatedBy).HasColumnName("CREATED_BY");
            entity.Property(e => e.Distance)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasColumnName("DISTANCE");
            entity.Property(e => e.DropoffAddress)
                .HasMaxLength(100)
                .IsUnicode(false)
                .HasColumnName("DROPOFF_ADDRESS");
            entity.Property(e => e.Luggages).HasColumnName("LUGGAGES");
            entity.Property(e => e.Passengers).HasColumnName("PASSENGERS");
            entity.Property(e => e.PickupAddress)
                .HasMaxLength(100)
                .IsUnicode(false)
                .HasColumnName("PICKUP_ADDRESS");
            entity.Property(e => e.PickupDate)
                .HasColumnType("date")
                .HasColumnName("PICKUP_DATE");
            entity.Property(e => e.PickupTime).HasColumnName("PICKUP_TIME");
            entity.Property(e => e.ReturnDate)
                .HasColumnType("date")
                .HasColumnName("RETURN_DATE");
            entity.Property(e => e.ReturnTime).HasColumnName("RETURN_TIME");
            entity.Property(e => e.TotalAmount)
                .HasColumnType("decimal(10, 3)")
                .HasColumnName("TOTAL_AMOUNT");
        });

        modelBuilder.Entity<CarBookingPassengerDetail>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK_CAR_BOOKING_USER_DETAILS");

            entity.ToTable("CAR_BOOKING_PASSENGER_DETAILS");

            entity.Property(e => e.Id).HasColumnName("ID");
            entity.Property(e => e.BookingDetailId).HasColumnName("BOOKING_DETAIL_ID");
            entity.Property(e => e.Email)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasColumnName("EMAIL");
            entity.Property(e => e.Name)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasColumnName("NAME");
            entity.Property(e => e.PhoneNumber)
                .HasMaxLength(15)
                .IsUnicode(false)
                .HasColumnName("PHONE_NUMBER");
        });

        modelBuilder.Entity<CarDetail>(entity =>
        {
            entity.ToTable("CAR_DETAILS");

            entity.Property(e => e.AdditionalInfo)
                .HasMaxLength(250)
                .IsUnicode(false)
                .HasColumnName("Additional_Info");
            entity.Property(e => e.BasePrice)
                .HasColumnType("decimal(10, 3)")
                .HasColumnName("Base_Price");
            entity.Property(e => e.CityId).HasColumnName("City_Id");
            entity.Property(e => e.CountyId).HasColumnName("County_Id");
            entity.Property(e => e.EndDate)
                .HasColumnType("date")
                .HasColumnName("End_Date");
            entity.Property(e => e.EndTime).HasColumnName("End_Time");
            entity.Property(e => e.Features)
                .HasMaxLength(250)
                .IsUnicode(false);
            entity.Property(e => e.FuelTypeId).HasColumnName("FuelType_Id");
            entity.Property(e => e.MakeId).HasColumnName("Make_Id");
            entity.Property(e => e.MileageLimit).HasColumnName("Mileage_Limit");
            entity.Property(e => e.MobileNumber1)
                .HasMaxLength(15)
                .IsUnicode(false);
            entity.Property(e => e.MobileNumber2)
                .HasMaxLength(15)
                .IsUnicode(false);
            entity.Property(e => e.Model)
                .HasMaxLength(255)
                .IsUnicode(false);
            entity.Property(e => e.OdometerId).HasColumnName("Odometer_Id");
            entity.Property(e => e.PassengerCapacity).HasColumnName("Passenger_Capacity");
            entity.Property(e => e.Photos).IsUnicode(false);
            entity.Property(e => e.SeatbeltTypeId).HasColumnName("Seatbelt_type_Id");
            entity.Property(e => e.StartDate)
                .HasColumnType("date")
                .HasColumnName("Start_Date");
            entity.Property(e => e.StartTime).HasColumnName("Start_Time");
            entity.Property(e => e.StateId).HasColumnName("State_Id");
            entity.Property(e => e.Street)
                .HasMaxLength(50)
                .IsUnicode(false);
            entity.Property(e => e.Transmission)
                .HasMaxLength(50)
                .IsUnicode(false);
            entity.Property(e => e.VehicleConditionId).HasColumnName("Vehicle_Condition_Id");
            entity.Property(e => e.VehicleValue)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasColumnName("Vehicle_Value");
            entity.Property(e => e.Vin).HasColumnName("VIN");
            entity.Property(e => e.YearId).HasColumnName("Year_Id");

            entity.HasOne(d => d.FuelType).WithMany(p => p.CarDetails)
                .HasForeignKey(d => d.FuelTypeId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_CAR_DETAILS_Fuel_Type");

            entity.HasOne(d => d.Make).WithMany(p => p.CarDetails)
                .HasForeignKey(d => d.MakeId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_CAR_DETAILS_VehicleMake");

            entity.HasOne(d => d.Odometer).WithMany(p => p.CarDetails)
                .HasForeignKey(d => d.OdometerId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_CAR_DETAILS_Odometer");

            entity.HasOne(d => d.SeatbeltType).WithMany(p => p.CarDetails)
                .HasForeignKey(d => d.SeatbeltTypeId)
                .HasConstraintName("FK_CAR_DETAILS_SeatbeltType");

            entity.HasOne(d => d.VehicleCondition).WithMany(p => p.CarDetails)
                .HasForeignKey(d => d.VehicleConditionId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_CAR_DETAILS_VehicleCondition");

            entity.HasOne(d => d.Year).WithMany(p => p.CarDetails)
                .HasForeignKey(d => d.YearId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_CarDetails_VehicleYear");
        });

        modelBuilder.Entity<CarImage>(entity =>
        {
            entity.ToTable("CAR_IMAGES");

            entity.Property(e => e.Id).HasColumnName("ID");
            entity.Property(e => e.CarId).HasColumnName("CAR_ID");
            entity.Property(e => e.ImagePath)
                .HasMaxLength(250)
                .IsUnicode(false)
                .HasColumnName("IMAGE_PATH");

            entity.HasOne(d => d.Car).WithMany(p => p.CarImages)
                .HasForeignKey(d => d.CarId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_CAR_IMAGES_CAR_DETAILS");
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

        modelBuilder.Entity<FuelType>(entity =>
        {
            entity.ToTable("Fuel_Type");

            entity.Property(e => e.FuelType1)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasColumnName("Fuel_Type");
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

        modelBuilder.Entity<OdometerReading>(entity =>
        {
            entity.ToTable("Odometer_Reading");

            entity.Property(e => e.OdometerReading1)
                .HasMaxLength(50)
                .HasColumnName("Odometer_Reading");
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
                .HasMaxLength(50)
                .IsUnicode(false)
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

        modelBuilder.Entity<PropertyReview>(entity =>
        {
            entity.ToTable("PROPERTY_REVIEWS");

            entity.Property(e => e.Id).HasColumnName("ID");
            entity.Property(e => e.CreatedOn)
                .HasColumnType("datetime")
                .HasColumnName("CREATED_ON");
            entity.Property(e => e.PropertyId).HasColumnName("PROPERTY_ID");
            entity.Property(e => e.RatingId).HasColumnName("RATING_ID");
            entity.Property(e => e.Review)
                .HasMaxLength(250)
                .IsUnicode(false)
                .HasColumnName("REVIEW");
            entity.Property(e => e.UpdatedOn)
                .HasColumnType("datetime")
                .HasColumnName("UPDATED_ON");
            entity.Property(e => e.UserId).HasColumnName("USER_ID");
        });

        modelBuilder.Entity<PropertyRoom>(entity =>
        {
            entity.ToTable("PROPERTY_ROOMS");

            entity.Property(e => e.Id).HasColumnName("ID");
            entity.Property(e => e.AdditionalInfoId).HasColumnName("ADDITIONAL_INFO_ID");
            entity.Property(e => e.Discount)
                .HasColumnType("decimal(10, 3)")
                .HasColumnName("DISCOUNT");
            entity.Property(e => e.Image)
                .IsUnicode(false)
                .HasColumnName("IMAGE");
            entity.Property(e => e.Name)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasColumnName("NAME");
            entity.Property(e => e.Price)
                .HasColumnType("decimal(10, 3)")
                .HasColumnName("PRICE");
            entity.Property(e => e.PropertyId).HasColumnName("PROPERTY_ID");

            entity.HasOne(d => d.Property).WithMany(p => p.PropertyRooms)
                .HasForeignKey(d => d.PropertyId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_PROPERTY_ROOMS_PROPERTY");
        });

        modelBuilder.Entity<Rating>(entity =>
        {
            entity.Property(e => e.Ratings)
                .HasMaxLength(10)
                .IsUnicode(false);
        });

        modelBuilder.Entity<RoomImage>(entity =>
        {
            entity.ToTable("ROOM_IMAGES");

            entity.Property(e => e.Id).HasColumnName("ID");
            entity.Property(e => e.ImagePath)
                .HasMaxLength(250)
                .IsUnicode(false)
                .HasColumnName("IMAGE_PATH");
            entity.Property(e => e.RoomId).HasColumnName("ROOM_ID");

            entity.HasOne(d => d.Room).WithMany(p => p.RoomImages)
                .HasForeignKey(d => d.RoomId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_ROOM_IMAGES_ROOM");
        });

        modelBuilder.Entity<SeatbeltType>(entity =>
        {
            entity.ToTable("Seatbelt_Type");

            entity.Property(e => e.SeatbeltType1)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasColumnName("Seatbelt_Type");
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

        modelBuilder.Entity<VehicleCondition>(entity =>
        {
            entity.ToTable("Vehicle_Condition");

            entity.Property(e => e.Condition)
                .HasMaxLength(50)
                .IsUnicode(false);
        });

        modelBuilder.Entity<VehicleMake>(entity =>
        {
            entity.ToTable("Vehicle_Make");

            entity.Property(e => e.VehicleMake1)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasColumnName("Vehicle_Make");
        });

        modelBuilder.Entity<VehicleYear>(entity =>
        {
            entity.ToTable("Vehicle_Year");

            entity.Property(e => e.VehicleYear1).HasColumnName("Vehicle_Year");
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
