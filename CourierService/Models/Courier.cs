using System.Runtime.InteropServices.JavaScript;
using Common.Enums;

namespace CourierService.Models;

public class Courier
{
    public int Id { set; get; }
    
    public Language SpeakingLanguage { set; get; } //enum
    
    public DateOnly DateOfBirth { set; get; }
    
    public Country DeliveryCountry { set; get; }
    
    public City DeliveryCity { set; get; }
    
    public Vehicle DeliveryVehicle { set; get; }
    
    public string? RegistrationNumber { set; get; }
    
    public string UserId { set; get; } //reference to user
    
}