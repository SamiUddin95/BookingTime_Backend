using System;
using System.Collections.Generic;

namespace BookingTime.Models;

public partial class PropertyPropertyAccessibility
{
    public int Id { get; set; }

    public long? PropertyDetailId { get; set; }

    public int? PropertyAccessibilityId { get; set; }
}
