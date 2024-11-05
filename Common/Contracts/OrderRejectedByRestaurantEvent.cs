namespace Common.Contracts;

public record OrderRejectedByRestaurantEvent
{
    public int OrderId { get; set; }
    public int RestaurantId { get; set; }
    public DateTime Timestamp { get; set; } = DateTime.UtcNow;
}