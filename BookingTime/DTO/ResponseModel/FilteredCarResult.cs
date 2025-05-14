using BookingTime.DTO.ResponseModel;
using BookingTime.Models;
public class FilteredCarResult {
    public List<AvailableCarsListDTO> Cars { get; set; }
    public List<CarCategory> Categories { get; set; }
    public List<VehicleMake> Makes { get; set; }
}

