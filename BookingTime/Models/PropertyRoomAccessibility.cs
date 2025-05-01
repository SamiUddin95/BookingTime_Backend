using System;
using System.Collections.Generic;

namespace BookingTime.Models;

public partial class PropertyRoomAccessibility
{
    public int Id { get; set; }

    public long? RoomId { get; set; }

    public int? RoomAccessibilityId { get; set; }
}
