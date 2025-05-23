using System;
using System.Collections.Generic;

namespace BookingTime.Models;

public partial class PropertyFunThingsToDo
{
    public int Id { get; set; }

    public long? PropertyDetailId { get; set; }

    public int? FunThingsId { get; set; }
}
