namespace Common.Contracts;

public record OrderRejectedByRestaurantEvent()
{
    public int OrderId { get; set; }
    public int RestaurantId { get; set; }
}