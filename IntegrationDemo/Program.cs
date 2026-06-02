using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Serilog;
using IntegrationService;

class Program
{
    static void Main()
    {
        Log.Logger = new LoggerConfiguration()
            .WriteTo.Console()
            .WriteTo.File("integration.log", rollingInterval: RollingInterval.Day)
            .CreateLogger();

        var services = new ServiceCollection();
        
        services.AddLogging(builder =>
        {
            builder.ClearProviders();
            builder.AddSerilog(dispose: true);
        });
        
        services.AddAutoMapper(typeof(MappingProfile));
        services.AddScoped<CartToOrderAdapter>();
        services.AddScoped<IOrderIntegrationService, OrderIntegrationService>();
        
        var provider = services.BuildServiceProvider();
        var integrationService = provider.GetRequiredService<IOrderIntegrationService>();
        var logger = provider.GetRequiredService<ILogger<Program>>();
        
        logger.LogInformation("=== Integration Service Started ===");
        
        var cartItem = new CartItemDto
        {
            ProductId = "prod_123",
            Quantity = 2,
            Price = 100
        };
        
        var orderRequest = integrationService.TransformCartToOrder(cartItem);
        
        Console.WriteLine($"\n=== RESULT ===");
        Console.WriteLine($"OrderLines: {orderRequest.OrderLines.Count}");
        Console.WriteLine($"ProductId: {orderRequest.OrderLines[0].ProductId}");
        Console.WriteLine($"Quantity: {orderRequest.OrderLines[0].Quantity}");
        Console.WriteLine($"TotalAmount: {orderRequest.TotalAmount}");
        
        Log.CloseAndFlush();
    }
}
