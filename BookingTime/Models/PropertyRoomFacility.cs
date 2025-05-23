using System;
using System.Collections.Generic;

namespace BookingTime.Models;

public partial class PropertyRoomFacility
{
    public int Id { get; set; }

    public long? RoomId { get; set; }

    public int? RoomFacilityId { get; set; }
}
