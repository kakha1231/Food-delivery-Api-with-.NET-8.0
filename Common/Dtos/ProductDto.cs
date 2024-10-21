namespace Common.Dtos;

public record ProductDto
{
    public int Id { set; get; }
    
    public string? Name { get; set; }
    
    public decimal Price { get; set; }
    
    public int Quantity { set; get; }
}