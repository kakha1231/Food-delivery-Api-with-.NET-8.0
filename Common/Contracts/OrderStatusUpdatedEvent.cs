namespace Common.Contracts;

public record OrderStatusUpdatedEvent
{
    public int OrderId { set; get; }
    
    public int? RestaurantId { set; get; }
    
    public int? CourierId { set; get; }
    public string Status { set; get; }
    
}