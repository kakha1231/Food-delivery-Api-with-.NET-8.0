using Common.Enums;
using CourierService.Models;

namespace CourierService.Dtos.Request;

public class CourierRegistrationDto
{
    public Language SpeakingLanguage { set; get; } //enum
    
    public DateOnly DateOfBirth { set; get; }
    
    public Country DeliveryCountry { set; get; }
    
    public City DeliveryCity { set; get; }
    
    public Vehicle DeliveryVehicle { set; get; }
    
    public string? RegistrationNumber { set; get; }

    public Courier CreateCourier()
    {
        return new Courier
        {
            SpeakingLanguage = SpeakingLanguage,
            DateOfBirth = DateOfBirth,
            DeliveryCountry = DeliveryCountry,
            DeliveryCity = DeliveryCity,
            DeliveryVehicle = DeliveryVehicle,
            RegistrationNumber = RegistrationNumber
        };
    }
}