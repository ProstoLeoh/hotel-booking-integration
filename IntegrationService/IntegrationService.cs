using Microsoft.Extensions.Logging;

namespace IntegrationService;

public interface IOrderIntegrationService
{
    OrderRequestDto TransformCartToOrder(CartItemDto cartItem);
}

public class OrderIntegrationService : IOrderIntegrationService
{
    private readonly CartToOrderAdapter _adapter;
    private readonly ILogger<OrderIntegrationService> _logger;

    public OrderIntegrationService(CartToOrderAdapter adapter, ILogger<OrderIntegrationService> logger)
    {
        _adapter = adapter;
        _logger = logger;
    }

    public OrderRequestDto TransformCartToOrder(CartItemDto cartItem)
    {
        _logger.LogInformation("Transforming CartItem: ProductId={ProductId}, Quantity={Quantity}, Price={Price}", 
            cartItem.ProductId, cartItem.Quantity, cartItem.Price);
        
        var result = _adapter.Transform(cartItem);
        
        _logger.LogInformation("OrderRequest created: TotalAmount={TotalAmount}", result.TotalAmount);
        
        return result;
    }
}
