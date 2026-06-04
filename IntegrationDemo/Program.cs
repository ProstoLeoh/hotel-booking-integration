using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.EntityFrameworkCore;
using Serilog;
using IntegrationService;
using IntegrationService.Data;
using IntegrationService.Models;

class Program
{
    static async Task Main(string[] args)
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
        
        services.AddDbContext<AppDbContext>(options =>
            options.UseSqlite("Data Source=../IntegrationService/hotel_booking.db"));
        
        services.AddScoped<IHotelBookingService, HotelBookingService>();
        
        var provider = services.BuildServiceProvider();
        var dbContext = provider.GetRequiredService<AppDbContext>();
        var service = provider.GetRequiredService<IHotelBookingService>();
        var logger = provider.GetRequiredService<ILogger<Program>>();
        
        logger.LogInformation("=== Отель Бронирование Сервис Запущен ===");
        
        await dbContext.Database.EnsureCreatedAsync();
        
        // 1. Добавляем отель
        var hotel = new Hotel
        {
            HotelId = "h_001",
            Name = "Grand Hotel",
            City = "Москва",
            Address = "Тверская ул., 1",
            Rating = 4.8
        };
        
        if (!dbContext.Hotels.Any(h => h.HotelId == "h_001"))
        {
            await service.AddHotelAsync(hotel);
            logger.LogInformation("Отель добавлен ✅");
        }
        
        // 2. Добавляем номер
        var room = new Room
        {
            RoomId = "r_101",
            HotelId = "h_001",
            Type = "double",
            PricePerNight = 8000,
            Capacity = 2,
            IsAvailable = true
        };
        
        if (!dbContext.Rooms.Any(r => r.RoomId == "r_101"))
        {
            await service.AddRoomAsync(room);
            logger.LogInformation("Номер добавлен ✅");
        }
        
        Console.WriteLine("\n🏨 Отели:");
        foreach (var h in await dbContext.Hotels.ToListAsync())
        {
            Console.WriteLine($"   - {h.Name} ({h.City}) ★ {h.Rating}");
        }
        
        Console.WriteLine("\n🛏️ Номера:");
        foreach (var r in await dbContext.Rooms.ToListAsync())
        {
            Console.WriteLine($"   - {r.RoomId}: {r.Type} → {r.PricePerNight} руб/ночь");
        }
        
        // 3. Создаём бронирование
        var checkIn = DateTime.Today.AddDays(7);
        var checkOut = DateTime.Today.AddDays(10);
        
        var booking = await service.CreateBookingAsync("r_101", "Иван Петров", "ivan@example.com", checkIn, checkOut);
        
        Console.WriteLine($"\n📅 БРОНИРОВАНИЕ СОЗДАНО:");
        Console.WriteLine($"   BookingId: {booking.BookingId}");
        Console.WriteLine($"   Номер: {booking.RoomId}");
        Console.WriteLine($"   Гость: {booking.GuestName}");
        Console.WriteLine($"   Заезд: {booking.CheckIn:yyyy-MM-dd}");
        Console.WriteLine($"   Выезд: {booking.CheckOut:yyyy-MM-dd}");
        Console.WriteLine($"   Сумма: {booking.TotalAmount} руб.");
        
        // 4. Оплата
        var payment = await service.ProcessPaymentAsync(booking.BookingId, booking.TotalAmount);
        
        Console.WriteLine($"\n💳 ОПЛАТА ПРОВЕДЕНА:");
        Console.WriteLine($"   PaymentId: {payment.PaymentId}");
        Console.WriteLine($"   Статус: {payment.Status}");
        Console.WriteLine($"   Сумма: {payment.Amount} руб.");
        
        Console.WriteLine($"\n✅ Статус бронирования: {booking.Status}");
        
        Console.WriteLine("\n🎉 Отель → Номер → Бронирование → Оплата → Успех!");
        
        Log.CloseAndFlush();
    }
}
