using System;
using System.Threading.Tasks;
using System.Linq;
using Microsoft.Extensions.Logging;
using Microsoft.EntityFrameworkCore;
using IntegrationService.Data;
using IntegrationService.Models;

namespace IntegrationService;

public interface IHotelBookingService
{
    Task<Hotel> AddHotelAsync(Hotel hotel);
    Task<Room> AddRoomAsync(Room room);
    Task<Booking> CreateBookingAsync(string roomId, string guestName, string guestEmail, DateTime checkIn, DateTime checkOut);
    Task<Payment> ProcessPaymentAsync(string bookingId, decimal amount);
}

public class HotelBookingService : IHotelBookingService
{
    private readonly AppDbContext _dbContext;
    private readonly ILogger<HotelBookingService> _logger;

    public HotelBookingService(AppDbContext dbContext, ILogger<HotelBookingService> logger)
    {
        _dbContext = dbContext;
        _logger = logger;
    }

    public async Task<Hotel> AddHotelAsync(Hotel hotel)
    {
        _logger.LogInformation("Добавление отеля: {HotelName}", hotel.Name);
        _dbContext.Hotels.Add(hotel);
        await _dbContext.SaveChangesAsync();
        return hotel;
    }
    
    public async Task<Room> AddRoomAsync(Room room)
    {
        _logger.LogInformation("Добавление номера: {RoomId} в отель {HotelId}", room.RoomId, room.HotelId);
        _dbContext.Rooms.Add(room);
        await _dbContext.SaveChangesAsync();
        return room;
    }
    
    public async Task<Booking> CreateBookingAsync(string roomId, string guestName, string guestEmail, DateTime checkIn, DateTime checkOut)
    {
        _logger.LogInformation("Создание бронирования для номера: {RoomId}", roomId);
        
        var room = await _dbContext.Rooms
            .FirstOrDefaultAsync(r => r.RoomId == roomId);
        
        if (room == null)
        {
            _logger.LogWarning("Номер {RoomId} не найден", roomId);
            throw new Exception("Room not found");
        }
        
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
        
        _dbContext.Bookings.Add(booking);
        await _dbContext.SaveChangesAsync();
        
        _logger.LogInformation("Бронирование создано: {BookingId}, сумма: {TotalAmount}", booking.BookingId, booking.TotalAmount);
        return booking;
    }
    
    public async Task<Payment> ProcessPaymentAsync(string bookingId, decimal amount)
    {
        _logger.LogInformation("Обработка оплаты для бронирования: {BookingId}", bookingId);
        
        var booking = await _dbContext.Bookings
            .FirstOrDefaultAsync(b => b.BookingId == bookingId);
        
        if (booking == null)
        {
            _logger.LogWarning("Бронирование {BookingId} не найдено", bookingId);
            throw new Exception("Booking not found");
        }
        
        var payment = new Payment
        {
            PaymentId = Guid.NewGuid().ToString(),
            BookingId = bookingId,
            Amount = amount,
            Status = "SUCCESS",
            PaymentDate = DateTime.UtcNow
        };
        
        // Обновляем статус бронирования
        booking.Status = "CONFIRMED";
        
        _dbContext.Payments.Add(payment);
        await _dbContext.SaveChangesAsync();
        
        _logger.LogInformation("Оплата проведена: {PaymentId}", payment.PaymentId);
        return payment;
    }
}
