namespace Common.Contracts;

public record CourierRegisteredEvent
{
    public string UserId { set; get; }
}