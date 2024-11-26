namespace Common.Contracts;

public record CourierLocationUpdatedEvent
{
    public int OrderId { set; get; }
    
    public int? CourierId { set; get; }
    
    public double Latitude { set; get; }
    
    public double Longitude { set; get; }
}