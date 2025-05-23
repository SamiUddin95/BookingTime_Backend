using System;
using System.Collections.Generic;

namespace BookingTime.Models;

public partial class GroupUser
{
    public int GroupId { get; set; }

    public string UserId { get; set; } = null!;
}
