using IntegrationService.Data;
using IntegrationService.Models;
using Microsoft.EntityFrameworkCore;

var options = new DbContextOptionsBuilder<AppDbContext>()
    .UseSqlite("Data Source=../IntegrationService/hotel_booking.db")
    .Options;

using (var db = new AppDbContext(options))
{
    // Создаём БД, если её нет
    await db.Database.EnsureCreatedAsync();
    
    // Добавляем отель
    var hotel = new Hotel
    {
        HotelId = "h_001",
        Name = "Grand Hotel",
        City = "Москва",
        Address = "Тверская, 1",
        Rating = 4.8
    };
    
    if (!db.Hotels.Any(h => h.HotelId == "h_001"))
    {
        db.Hotels.Add(hotel);
        await db.SaveChangesAsync();
        Console.WriteLine("✅ Отель добавлен");
    }
    else
    {
        Console.WriteLine("Отель уже есть");
    }
    
    // Добавляем номер
    var room = new Room
    {
        RoomId = "r_101",
        HotelId = "h_001",
        Type = "double",
        PricePerNight = 8000,
        Capacity = 2,
        IsAvailable = true
    };
    
    if (!db.Rooms.Any(r => r.RoomId == "r_101"))
    {
        db.Rooms.Add(room);
        await db.SaveChangesAsync();
        Console.WriteLine("✅ Номер r_101 добавлен (доступен)");
    }
    else
    {
        // Обновляем статус
        var existingRoom = await db.Rooms.FirstOrDefaultAsync(r => r.RoomId == "r_101");
        if (existingRoom != null)
        {
            existingRoom.IsAvailable = true;
            await db.SaveChangesAsync();
            Console.WriteLine("✅ Номер r_101 обновлён (теперь доступен)");
        }
    }
    
    // Проверяем
    var checkRoom = await db.Rooms.FirstOrDefaultAsync(r => r.RoomId == "r_101");
    Console.WriteLine($"\nПроверка: RoomId={checkRoom?.RoomId}, IsAvailable={checkRoom?.IsAvailable}");
}
