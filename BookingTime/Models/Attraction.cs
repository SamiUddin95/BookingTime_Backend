using System;
using System.Collections.Generic;

namespace BookingTime.Models;

public partial class Attraction
{
    public int Id { get; set; }

    public long CityId { get; set; }

    public string Title { get; set; } = null!;

    public string? ShortDescription { get; set; }

    public string? DetailedDescription { get; set; }

    public decimal Price { get; set; }

    public double? Rating { get; set; }

    public DateTime? CreatedAt { get; set; }

    public int? CategoryId { get; set; }

    public virtual ICollection<AttractionImage> AttractionImages { get; } = new List<AttractionImage>();

    public virtual AttractionCategory? Category { get; set; }

    public virtual City City { get; set; } = null!;
}
