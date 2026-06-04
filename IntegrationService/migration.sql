Build started...
Build succeeded.
CREATE TABLE IF NOT EXISTS "__EFMigrationsHistory" (
    "MigrationId" TEXT NOT NULL CONSTRAINT "PK___EFMigrationsHistory" PRIMARY KEY,
    "ProductVersion" TEXT NOT NULL
);

BEGIN TRANSACTION;

CREATE TABLE "Bookings" (
    "Id" INTEGER NOT NULL CONSTRAINT "PK_Bookings" PRIMARY KEY AUTOINCREMENT,
    "BookingId" TEXT NOT NULL,
    "RoomId" TEXT NOT NULL,
    "GuestName" TEXT NOT NULL,
    "GuestEmail" TEXT NOT NULL,
    "CheckIn" TEXT NOT NULL,
    "CheckOut" TEXT NOT NULL,
    "TotalAmount" TEXT NOT NULL,
    "Status" TEXT NOT NULL,
    "CreatedAt" TEXT NOT NULL
);

CREATE TABLE "Hotels" (
    "Id" INTEGER NOT NULL CONSTRAINT "PK_Hotels" PRIMARY KEY AUTOINCREMENT,
    "HotelId" TEXT NOT NULL,
    "Name" TEXT NOT NULL,
    "City" TEXT NOT NULL,
    "Address" TEXT NOT NULL,
    "Rating" REAL NOT NULL
);

CREATE TABLE "Payments" (
    "Id" INTEGER NOT NULL CONSTRAINT "PK_Payments" PRIMARY KEY AUTOINCREMENT,
    "PaymentId" TEXT NOT NULL,
    "BookingId" TEXT NOT NULL,
    "Amount" TEXT NOT NULL,
    "Status" TEXT NOT NULL,
    "PaymentDate" TEXT NOT NULL
);

CREATE TABLE "Rooms" (
    "Id" INTEGER NOT NULL CONSTRAINT "PK_Rooms" PRIMARY KEY AUTOINCREMENT,
    "RoomId" TEXT NOT NULL,
    "HotelId" TEXT NOT NULL,
    "Type" TEXT NOT NULL,
    "PricePerNight" TEXT NOT NULL,
    "Capacity" INTEGER NOT NULL,
    "IsAvailable" INTEGER NOT NULL
);

CREATE UNIQUE INDEX "IX_Bookings_BookingId" ON "Bookings" ("BookingId");

CREATE UNIQUE INDEX "IX_Hotels_HotelId" ON "Hotels" ("HotelId");

CREATE UNIQUE INDEX "IX_Payments_PaymentId" ON "Payments" ("PaymentId");

CREATE UNIQUE INDEX "IX_Rooms_RoomId" ON "Rooms" ("RoomId");

INSERT INTO "__EFMigrationsHistory" ("MigrationId", "ProductVersion")
VALUES ('20260602181704_InitialCreate', '7.0.0');

COMMIT;


