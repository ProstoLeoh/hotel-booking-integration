using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Caching.Memory;
using System.Diagnostics;
using IntegrationService.Data;
using IntegrationService.Saga;
using IntegrationService.Models;

namespace HotelBooking.API.Controllers;

[ApiController]
[Route("api/[controller]")]
public class BookingController : ControllerBase
{
    private readonly AppDbContext _dbContext;
    private readonly IBookingSaga _saga;
    private readonly IMemoryCache _cache;
    private readonly ILogger<BookingController> _logger;

    public BookingController(AppDbContext dbContext, IBookingSaga saga, IMemoryCache cache, ILogger<BookingController> logger)
    {
        _dbContext = dbContext;
        _saga = saga;
        _cache = cache;
        _logger = logger;
    }

    [HttpGet("rooms")]
    public async Task<IActionResult> GetRooms()
    {
        var sw = Stopwatch.StartNew();
        const string cacheKey = "rooms_list";
        
        // Пытаемся получить из кэша
        if (_cache.TryGetValue(cacheKey, out List<Room> cachedRooms))
        {
            sw.Stop();
            _logger.LogInformation("⏱️ GetRooms из кэша: {Elapsed} мс", sw.ElapsedMilliseconds);
            return Ok(cachedRooms);
        }
        
        _logger.LogInformation("Кэш пуст, идём в БД...");
        
        var rooms = await _dbContext.Rooms.ToListAsync();
        
        // Сохраняем в кэш
        _cache.Set(cacheKey, rooms, new MemoryCacheEntryOptions
        {
            AbsoluteExpirationRelativeToNow = TimeSpan.FromMinutes(5)
        });
        
        sw.Stop();
        _logger.LogInformation("⏱️ GetRooms из БД: {Elapsed} мс", sw.ElapsedMilliseconds);
        
        return Ok(rooms);
    }

    [HttpGet("bookings")]
    public async Task<IActionResult> GetBookings()
    {
        var bookings = await _dbContext.Bookings.ToListAsync();
        return Ok(bookings);
    }

    [HttpPost("book")]
    public async Task<IActionResult> CreateBooking([FromBody] CreateBookingRequest request)
    {
        try
        {
            var bookingId = await _saga.ExecuteBookingFlow(
                request.RoomId, request.GuestName, request.GuestEmail,
                request.CheckIn, request.CheckOut);
            
            var booking = await _dbContext.Bookings.FirstOrDefaultAsync(b => b.BookingId == bookingId);
            
            // Инвалидируем кэш при создании брони
            _cache.Remove("rooms_list");
            _logger.LogInformation("Кэш rooms_list очищен");
            
            return Ok(new { success = true, bookingId = bookingId, totalAmount = booking?.TotalAmount });
        }
        catch (Exception ex)
        {
            return StatusCode(500, new { success = false, error = ex.Message });
        }
    }
}

public class CreateBookingRequest
{
    public string RoomId { get; set; }
    public string GuestName { get; set; }
    public string GuestEmail { get; set; }
    public DateTime CheckIn { get; set; }
    public DateTime CheckOut { get; set; }
}
