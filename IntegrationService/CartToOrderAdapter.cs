using AutoMapper;

namespace IntegrationService;

public class CartToOrderAdapter
{
    private readonly IMapper _mapper;

    public CartToOrderAdapter(IMapper mapper)
    {
        _mapper = mapper;
    }

    public OrderRequestDto Transform(CartItemDto cartItem)
    {
        return new OrderRequestDto
        {
            OrderLines = new List<OrderLineDto>
            {
                new OrderLineDto 
                { 
                    ProductId = cartItem.ProductId, 
                    Quantity = cartItem.Quantity 
                }
            },
            TotalAmount = cartItem.Price * cartItem.Quantity
        };
    }
}
