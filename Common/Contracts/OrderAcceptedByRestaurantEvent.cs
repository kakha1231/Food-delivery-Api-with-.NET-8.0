namespace Common.Contracts;

public record OrderAcceptedByRestaurantEvent
{
    public int OrderId { get; set; }
    public int RestaurantId { get; set; }
    
    public string RestaurantName { get; set; }
    
    public string RestaurantLocation { get; set; }
    
    public string RestaurantAddress { get; set; }
    
    public DateTime Timestamp { get; set; } = DateTime.UtcNow;
}