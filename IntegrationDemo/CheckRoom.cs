using IntegrationService.Data;
using Microsoft.EntityFrameworkCore;

var options = new DbContextOptionsBuilder<AppDbContext>()
    .UseSqlite("Data Source=../IntegrationService/hotel_booking.db")
    .Options;

using (var db = new AppDbContext(options))
{
    var room = await db.Rooms.FirstOrDefaultAsync(r => r.RoomId == "r_101");
    if (room == null)
    {
        Console.WriteLine("❌ Номер r_101 не найден в базе!");
        Console.WriteLine("Добавляем...");
        
        var newRoom = new IntegrationService.Models.Room
        {
            RoomId = "r_101",
            HotelId = "h_001",
            Type = "double",
            PricePerNight = 8000,
            Capacity = 2,
            IsAvailable = true
        };
        db.Rooms.Add(newRoom);
        await db.SaveChangesAsync();
        Console.WriteLine("✅ Номер r_101 добавлен!");
    }
    else
    {
        Console.WriteLine($"✅ Номер найден: RoomId={room.RoomId}, IsAvailable={room.IsAvailable}, Price={room.PricePerNight}");
        
        if (!room.IsAvailable)
        {
            room.IsAvailable = true;
            await db.SaveChangesAsync();
            Console.WriteLine("✅ Номер r_101 теперь доступен (IsAvailable=true)");
        }
    }
}
