# 🏨 Hotel Booking Integration System

Система бронирования отелей с микросервисной архитектурой, кэшированием и CI/CD.

## 📦 Модули

| Модуль | Технология | Порт |
|--------|-----------|------|
| HotelBooking.API | ASP.NET Core 8 + Swagger | 5071 |
| IntegrationService | C# Class Library | — |
| База данных | SQLite | — |
| Кэш | IMemoryCache (встроенный) | — |

## 🚀 Запуск

### Требования
- .NET 8 SDK
- Git

### Шаги

bash
# 1. Клонировать репозиторий
git clone https://github.com/ProstoLeoh/hotel-booking-integration.git
cd hotel-booking-integration

# 2. Восстановить пакеты
dotnet restore

# 3. Запустить API
cd HotelBooking.API
dotnet run

