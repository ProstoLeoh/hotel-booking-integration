using System;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using IntegrationService.Data;
using IntegrationService.Models;
using Microsoft.EntityFrameworkCore;

namespace IntegrationService.Saga;

public interface IBookingSaga
{
    Task<string> ExecuteBookingFlow(string roomId, string guestName, string guestEmail, DateTime checkIn, DateTime checkOut);
    Task Compensate(string bookingId);
}

public class BookingSaga : IBookingSaga
{
    private readonly AppDbContext _dbContext;
    private readonly ILogger<BookingSaga> _logger;

    public BookingSaga(AppDbContext dbContext, ILogger<BookingSaga> logger)
    {
        _dbContext = dbContext;
        _logger = logger;
    }

    public async Task<string> ExecuteBookingFlow(string roomId, string guestName, string guestEmail, DateTime checkIn, DateTime checkOut)
    {
        _logger.LogInformation("========== НАЧАЛО СКВОЗНОГО СЦЕНАРИЯ ==========");
        _logger.LogInformation("1️⃣ Гость выбирает номер: {RoomId}", roomId);
        
        // Шаг 2: Проверка доступности
        _logger.LogInformation("2️⃣ Проверка доступности номера...");
        var room = await _dbContext.Rooms.FirstOrDefaultAsync(r => r.RoomId == roomId);
        if (room == null || !room.IsAvailable)
        {
            _logger.LogError("❌ Номер {RoomId} недоступен для бронирования", roomId);
            throw new Exception("Номер недоступен");
        }
        _logger.LogInformation("   ✅ Номер доступен");
        
        // Шаг 3: Создание бронирования
        _logger.LogInformation("3️⃣ Создание бронирования...");
        var nights = (checkOut - checkIn).Days;
        var totalAmount = room.PricePerNight * nights;
        
        var booking = new Booking
        {
            BookingId = Guid.NewGuid().ToString(),
            RoomId = roomId,
            GuestName = guestName,
            GuestEmail = guestEmail,
            CheckIn = checkIn,
            CheckOut = checkOut,
            TotalAmount = totalAmount,
            Status = "PENDING",
            CreatedAt = DateTime.UtcNow
        };
        
        await _dbContext.Bookings.AddAsync(booking);
        await _dbContext.SaveChangesAsync();
        _logger.LogInformation("   ✅ Бронирование создано: BookingId={BookingId}, Сумма={TotalAmount} руб", booking.BookingId, totalAmount);
        
        // Шаг 4: Оплата
        _logger.LogInformation("4️⃣ Обработка оплаты...");
        var payment = new Payment
        {
            PaymentId = Guid.NewGuid().ToString(),
            BookingId = booking.BookingId,
            Amount = totalAmount,
            Status = "SUCCESS",
            PaymentDate = DateTime.UtcNow
        };
        
        await _dbContext.Payments.AddAsync(payment);
        booking.Status = "CONFIRMED";
        await _dbContext.SaveChangesAsync();
        _logger.LogInformation("   ✅ Оплата проведена: PaymentId={PaymentId}, Статус={Status}", payment.PaymentId, payment.Status);
        
        // Шаг 5: Подтверждение
        _logger.LogInformation("5️⃣ Подтверждение бронирования...");
        _logger.LogInformation("   ✅ Бронирование ПОДТВЕРЖДЕНО!");
        
        _logger.LogInformation("========== СКВОЗНОЙ СЦЕНАРИЙ ЗАВЕРШЕН УСПЕШНО ==========");
        
        return booking.BookingId;
    }
    
    public async Task Compensate(string bookingId)
    {
        _logger.LogWarning("🔄 ЗАПУСК КОМПЕНСАЦИИ для BookingId={BookingId}", bookingId);
        
        var booking = await _dbContext.Bookings.FirstOrDefaultAsync(b => b.BookingId == bookingId);
        if (booking != null)
        {
            booking.Status = "CANCELLED";
            await _dbContext.SaveChangesAsync();
            _logger.LogWarning("❌ Бронирование {BookingId} отменено (компенсация)", bookingId);
        }
        
        _logger.LogWarning("========== КОМПЕНСАЦИЯ ЗАВЕРШЕНА ==========");
    }
}
