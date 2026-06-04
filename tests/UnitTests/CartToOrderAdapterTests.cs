using Xunit;
using IntegrationService;

namespace UnitTests;

public class CartToOrderAdapterTests
{
    [Fact]
    public void Transform_ValidCartItem_ReturnsCorrectOrderRequest()
    {
        // Arrange
        var adapter = new CartToOrderAdapter(null!);
        var cartItem = new CartItemDto
        {
            ProductId = "prod_123",
            Quantity = 2,
            Price = 100m
        };

        // Act
        var result = adapter.Transform(cartItem);

        // Assert
        Assert.Equal("prod_123", result.OrderLines[0].ProductId);
        Assert.Equal(2, result.OrderLines[0].Quantity);
        Assert.Equal(200, result.TotalAmount);
    }

    [Fact]
    public void Transform_ZeroQuantity_ReturnsZeroTotal()
    {
        var adapter = new CartToOrderAdapter(null!);
        var cartItem = new CartItemDto
        {
            ProductId = "prod_123",
            Quantity = 0,
            Price = 100m
        };

        var result = adapter.Transform(cartItem);
        Assert.Equal(0, result.TotalAmount);
    }

    [Fact]
    public void Transform_NegativeQuantity_ReturnsNegativeTotal()
    {
        var adapter = new CartToOrderAdapter(null!);
        var cartItem = new CartItemDto
        {
            ProductId = "prod_123",
            Quantity = -2,
            Price = 100m
        };

        var result = adapter.Transform(cartItem);
        Assert.Equal(-200, result.TotalAmount);
    }
}
