namespace Common.Contracts;

public record RestaurantRegisteredEvent
{
    public string UserId { set; get; }
}