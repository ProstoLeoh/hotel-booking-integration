using IntegrationService.Data;
using IntegrationService.Models;

var options = new DbContextOptionsBuilder<AppDbContext>()
    .UseSqlite("Data Source=../IntegrationService/hotel_booking.db")
    .Options;

using (var db = new AppDbContext(options))
{
    // Добавляем отель
    var hotel = new Hotel
    {
        HotelId = "h_001",
        Name = "Grand Hotel",
        City = "Москва",
        Address = "Тверская, 1",
        Rating = 4.8
    };
    db.Hotels.Add(hotel);
    
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
    db.Rooms.Add(room);
    
    await db.SaveChangesAsync();
    Console.WriteLine("✅ Данные добавлены: отель и номер r_101 (доступен)");
}
