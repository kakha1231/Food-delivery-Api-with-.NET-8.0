namespace OrderService.Models;

public class OrderItem
{
    public int Id { get; set; } 
    public int ProductId { get; set; } 
    public string ProductName { get; set; } 
    public decimal UnitPrice { get; set; } 
    public int Quantity { get; set; } 
    
    public decimal TotalPrice => UnitPrice * Quantity;
    public int OrderId { get; set; } 
    public Order Order { set; get; }
}