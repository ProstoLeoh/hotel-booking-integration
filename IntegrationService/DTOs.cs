namespace IntegrationService;

public class CartItemDto
{
    public string ProductId { get; set; }
    public int Quantity { get; set; }
    public decimal Price { get; set; }
}

public class OrderLineDto
{
    public string ProductId { get; set; }
    public int Quantity { get; set; }
}

public class OrderRequestDto
{
    public List<OrderLineDto> OrderLines { get; set; }
    public decimal TotalAmount { get; set; }
}
