namespace Common.Contracts;

public record OrderAcceptedByRestaurantEvent()
{
    public int OrderId { get; set; }
    public int RestaurantId { get; set; }
}