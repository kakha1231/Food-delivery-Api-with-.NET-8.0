namespace Common.Contracts;

public record OrderAcceptedByCourierEvent
{
    public string OrderId { get; set; }
    
    public int CourierId { get; set; }
};