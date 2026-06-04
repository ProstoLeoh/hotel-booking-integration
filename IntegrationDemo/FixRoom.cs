using Microsoft.EntityFrameworkCore;
using IntegrationService.Data;

var options = new DbContextOptionsBuilder<AppDbContext>()
    .UseSqlite("Data Source=../IntegrationService/hotel_booking.db")
    .Options;

using var db = new AppDbContext(options);
var room = await db.Rooms.FirstOrDefaultAsync(r => r.RoomId == "r_101");
if (room != null)
{
    room.IsAvailable = true;
    await db.SaveChangesAsync();
    Console.WriteLine("Номер r_101 теперь доступен ✅");
}
else
{
    Console.WriteLine("Номер не найден");
}
