using System;
using System.Collections.Generic;

namespace BookingTime.Models;

public partial class GroupPermission
{
    public int Id { get; set; }

    public int GroupId { get; set; }

    public int PermissionId { get; set; }
}
