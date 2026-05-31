
# 🚌 Bus Ticket Booking & Billing System

## A Complete C# Console Application Demonstrating OOP Pillars and SOLID Principles

---

## 📋 Table of Contents

- [Introduction](#-introduction)
- [Project Structure](#-project-structure)
- [OOP Pillars Overview](#-oop-pillars-overview)
- [SOLID Principles Overview](#-solid-principles-overview)
- [Class 1: User.cs](#-class-1-usercs)
- [Class 2: Bus.cs](#-class-2-buscs)
- [Class 3: Schedule.cs](#-class-3-schedulecs)
- [Class 4: Ticket.cs](#-class-4-ticketcs)
- [Class 5: Invoice.cs](#-class-5-invoicecs)
- [Class 6: Program.cs](#-class-6-programcs)
- [Factory Pattern](#-factory-pattern)
- [System Workflow](#-system-workflow)
- [How to Run](#-how-to-run)
- [Test Sequence](#-test-sequence)
- [Conclusion](#-conclusion)

---

## 📖 Introduction

This project is a comprehensive **Bus Ticket Booking & Billing System** developed as part of a C# Object-Oriented Programming assignment. The application enables users to book bus tickets, select seats, generate invoices, and process payments.

### ✨ Key Features

| Feature | Description |
|:--------|:------------|
| 👤 User Management | Create and manage user accounts with unique IDs |
| 🚌 Bus Management | Manage fleet of buses with different classifications |
| 📅 Schedule Management | Create schedules with departure/arrival cities, date/time, and pricing |
| 🎫 Ticket Booking | Browse schedules, select seats, and complete reservations |
| 📄 Invoice Generation | Automatic invoice creation for every confirmed booking |
| 💰 Payment Processing | Submit payments for outstanding invoices |
| 👁️ Viewing Operations | Display users, buses, schedules, tickets, and invoices |

### 🛠️ Technology Stack

| Component | Technology |
|:----------|:-----------|
| Language | C# 10.0 |
| Framework | .NET 10.0 |
| Application Type | Console Application |
| Build Tool | dotnet CLI |

---

## 📁 Project Structure

```
BusTicketBookingSystem/
│
├── Program.cs                 # Main application entry point with all operations
│
├── Models/
│   ├── User.cs               # User management with inheritance hierarchy
│   ├── Bus.cs                # Bus management with classification system
│   ├── Schedule.cs           # Schedule management with bus association
│   ├── Ticket.cs             # Ticket booking and reservation system
│   └── Invoice.cs            # Invoice generation and payment processing
│
└── README.md                 # Project documentation
```

### 📊 Class Dependency Diagram

```
┌──────────────┐     ┌──────────────┐     ┌────────────────┐
│    User      │     │     Bus      │     │   Schedule     │
│   (👤)       │     │    (🚌)      │     │    (📅)        │
└──────┬───────┘     └──────┬───────┘     └───────┬────────┘
       │                    │                     │
       │                    └─────────────────────┼───────┐
       │                                          │       │
       ▼                                          ▼       ▼
┌──────────────┐                          ┌─────────────────────┐
│   Ticket     │◄─────────────────────────│  TicketBooking      │
│   (🎫)       │                          │    Service          │
└──────┬───────┘                          │    (🔧)             │
       │                                  └─────────────────────┘
       │                                           │
       ▼                                           ▼
┌──────────────┐                          ┌─────────────────────┐
│   Invoice    │◄─────────────────────────│    Payment          │
│   (📄)       │                          │    Service          │
└──────────────┘                          │    (💳)             │
                                          └─────────────────────┘
```

---

## 🎯 OOP Pillars Overview

| Pillar | How It's Implemented | Location |
|:-------|:---------------------|:----------|
| **🔒 Encapsulation** | Private fields with public properties containing validation logic | All classes |
| **🌳 Inheritance** | BaseUser → RegularUser/PremiumUser, BaseBus → EconomyBus/BusinessBus/LuxuryBus, BaseSchedule → RegularSchedule/ExpressSchedule | User.cs, Bus.cs, Schedule.cs |
| **🔄 Polymorphism** | Virtual/override methods like `GetDiscountedPrice()`, `DisplayScheduleDetails()` | Schedule.cs, Bus.cs, User.cs |
| **📦 Abstraction** | Abstract base classes and interfaces hiding implementation details | All classes |

---

## 📐 SOLID Principles Overview

| Principle | How It's Implemented | Location |
|:----------|:---------------------|:----------|
| **S**ingle Responsibility | Each class has one reason to change | All classes |
| **O**pen/Closed | Base classes open for extension, closed for modification | BaseUser, BaseBus, BaseSchedule |
| **L**iskov Substitution | Derived classes can replace base classes without breaking | RegularUser, EconomyBus, RegularSchedule |
| **I**nterface Segregation | Small, focused interfaces instead of one large interface | 15+ interfaces across all files |
| **D**ependency Inversion | Factories depend on abstractions, not concrete classes | UserFactory, BusFactory, ScheduleFactory |

---

## 👤 Class 1: User.cs

### 📁 File: `User.cs`

### 🎯 Purpose
Manages user accounts with support for multiple ticket bookings per user.

### 📋 Properties

| Property | Type | Description |
|:---------|:-----|:------------|
| `UserId` | string | Unique identifier (auto-generated) |
| `FullName` | string | User's full name |
| `MobileNumber` | string | 11-digit mobile number with validation |
| `Email` | string | Email address with format validation |
| `TotalBookings` | int | Total number of bookings made |
| `ActiveBookings` | int | Number of active (non-cancelled) bookings |

### 🔧 Key Methods

| Method | Description |
|:-------|:------------|
| `AddBooking(ticketId)` | Adds a ticket to user's booking list |
| `CancelBooking(ticketId)` | Cancels a specific booking |
| `GetActiveBookings()` | Returns list of active ticket IDs |
| `DisplayUserInfo()` | Displays user information |
| `DisplayAllBookings()` | Shows complete booking history |

### 🏛️ Inheritance Hierarchy

```
┌─────────────────────────────────────────────────────────────────┐
│                     <<abstract>>                                │
│                       BaseUser                                   │
├─────────────────────────────────────────────────────────────────┤
│  #_userId: string                                               │
│  #_fullName: string                                             │
│  #_mobileNumber: string                                         │
│  #_email: string                                                │
│  #_bookingRecords: List<BookingRecord>                          │
├─────────────────────────────────────────────────────────────────┤
│  +AddBooking(): void (virtual)                                  │
│  +CancelBooking(): bool (virtual)                               │
│  +DisplayUserInfo(): void (virtual)                             │
│  #GenerateUserId(): string (abstract)                           │
└─────────────────────────────────────────────────────────────────┘
                              ▲
              ┌───────────────┼───────────────┐
              │               │               │
┌─────────────────────┐ ┌─────────────────────────────────────────┐
│    RegularUser      │ │              PremiumUser                 │
│      (👤)           │ │                (⭐)                      │
├─────────────────────┤ ├─────────────────────────────────────────┤
│ Standard booking    │ │ +LoyaltyPoints: int                      │
│ behavior            │ │ +DiscountCodes: List<string>             │
│                     │ │ +AddDiscountCode(): void                 │
│                     │ │ Overridden booking behavior              │
└─────────────────────┘ └─────────────────────────────────────────┘
```

### 🔗 Interfaces Implemented

| Interface | Purpose |
|:----------|:--------|
| `IUserData` | User data access (UserId, FullName, MobileNumber, Email) |
| `IBookable` | Booking operations (AddBooking, CancelBooking, GetActiveBookings) |
| `IUserDisplay` | Display operations (DisplayUserInfo, DisplayAllBookings) |

### 🎓 OOP Pillars in User.cs

| Pillar | Implementation |
|:-------|:---------------|
| 🔒 **Encapsulation** | Private `_mobileNumber`, `_email`, `_bookingRecords` with public validated properties |
| 🌳 **Inheritance** | `BaseUser` → `RegularUser`, `BaseUser` → `PremiumUser` |
| 🔄 **Polymorphism** | Virtual `AddBooking()`, `CancelBooking()`, `DisplayUserInfo()` overridden in `PremiumUser` |
| 📦 **Abstraction** | Abstract `GenerateUserId()` method; interfaces hide implementation |

### 📐 SOLID in User.cs

| Principle | Implementation |
|:----------|:---------------|
| **S** | `User` class manages users; `BookingRecord` class separately manages booking data |
| **O** | `PremiumUser` extends `BaseUser` with loyalty points without modifying `BaseUser` |
| **L** | `RegularUser` and `PremiumUser` can replace `BaseUser` anywhere |
| **I** | Three focused interfaces instead of one large interface |
| **D** | `IUserFactory` abstraction; `UserFactory` depends on abstraction |

---

## 🚌 Class 2: Bus.cs

### 📁 File: `Bus.cs`

### 🎯 Purpose
Manages the bus fleet with different classifications and seat reservation tracking.

### 📋 Properties

| Property | Type | Description |
|:---------|:-----|:------------|
| `BusId` | string | Unique identifier for the bus |
| `CoachNumber` | string | Coach identification number |
| `Classification` | BusClassification | Economy, Business, or Luxury |
| `TotalCapacity` | int | Total seats determined by classification |
| `AvailableSeatsCount` | int | Number of available seats |
| `ReservedSeatsCount` | int | Number of reserved seats |
| `OccupancyRate` | double | Percentage of seats reserved |

### 🔧 Key Methods

| Method | Description |
|:-------|:------------|
| `ReserveSeat(seatNumber)` | Reserves a specific seat on the bus |
| `CancelReservation(seatNumber)` | Cancels seat reservation |
| `IsSeatAvailable(seatNumber)` | Checks if a seat is available |
| `GetAvailableSeatsList()` | Returns list of all available seats |
| `DisplaySeatingChart()` | Shows visual seating arrangement |
| `GetPriceMultiplier()` | Returns price multiplier based on bus class |

### 🏛️ Inheritance Hierarchy

```
┌─────────────────────────────────────────────────────────────────┐
│                     <<abstract>>                                │
│                       BaseBus                                    │
├─────────────────────────────────────────────────────────────────┤
│  #_busId: string                                                │
│  #_coachNumber: string                                          │
│  #_classification: BusClassification                            │
│  #_reservedSeats: HashSet<int>                                  │
│  #_availableSeats: HashSet<int>                                 │
├─────────────────────────────────────────────────────────────────┤
│  +ReserveSeat(): bool (virtual)                                 │
│  +CancelReservation(): bool (virtual)                           │
│  +DisplaySeatingChart(): void (virtual)                         │
│  #GetCapacityByClassification(): int (abstract)                 │
│  +GetBasePriceMultiplier(): decimal (abstract)                  │
│  +GetBusType(): string (abstract)                               │
└─────────────────────────────────────────────────────────────────┘
                              ▲
              ┌───────────────┼───────────────┐
              │               │               │
┌──────────────────┐ ┌──────────────────┐ ┌──────────────────┐
│   EconomyBus     │ │   BusinessBus    │ │    LuxuryBus     │
│     (💺)         │ │     (💼)         │ │     (✨)         │
├──────────────────┤ ├──────────────────┤ ├──────────────────┤
│ Capacity: 50     │ │ Capacity: 35     │ │ Capacity: 25     │
│ Multiplier: 1.0x │ │ Multiplier: 1.5x │ │ Multiplier: 2.5x │
│ Standard seating │ │ WiFi, Charging   │ │ All amenities    │
└──────────────────┘ └──────────────────┘ └──────────────────┘
```

### 📊 Bus Classification Details

| Classification | Capacity | Price Multiplier | Features |
|:---------------|:---------|:-----------------|:----------|
| 💺 **Economy** | 50 seats | 1.0x | Standard seating |
| 💼 **Business** | 35 seats | 1.5x | Extra legroom, WiFi, Charging ports |
| ✨ **Luxury** | 25 seats | 2.5x | Reclining seats, Entertainment, Refreshments |

### 🔗 Interfaces Implemented

| Interface | Purpose |
|:----------|:--------|
| `ISeatManageable` | Seat operations (ReserveSeat, CancelReservation, IsSeatAvailable) |
| `IBusInfo` | Bus information (BusId, CoachNumber, Classification, TotalCapacity) |
| `IPricingStrategy` | Pricing methods (GetBasePriceMultiplier, GetSeatPrice) |
| `IDisplayable` | Display methods (DisplayDetails, DisplaySeatingChart) |

---

## 📅 Class 3: Schedule.cs

### 📁 File: `Schedule.cs`

### 🎯 Purpose
Manages bus schedules including routes, departure times, and ticket pricing.

### 📋 Properties

| Property | Type | Description |
|:---------|:-----|:------------|
| `ScheduleId` | string | Unique identifier for the schedule |
| `DepartureCity` | string | Origin city |
| `ArrivalCity` | string | Destination city |
| `DepartureDateTime` | DateTime | Date and time of departure |
| `TicketPrice` | decimal | Base ticket price |
| `AssignedBus` | BaseBus | Bus assigned to this schedule |
| `BookedSeatsCount` | int | Number of seats booked for this schedule |
| `AvailableSeatsCount` | int | Number of seats available |
| `IsFullyBooked` | bool | Indicates if schedule is fully booked |

### 🔧 Key Methods

| Method | Description |
|:-------|:------------|
| `IsAvailable()` | Checks if schedule is available for booking |
| `IsDeparturePassed()` | Checks if departure time has passed |
| `GetTimeUntilDeparture()` | Returns time remaining until departure |
| `GetDiscountedPrice(percentage)` | Calculates price after discount |
| `BookSeat(seatNumber, userId)` | Books a seat for this schedule |
| `CancelSeatBooking(seatNumber, userId)` | Cancels seat booking |
| `DisplayScheduleDetails()` | Shows complete schedule information |

### 🏛️ Inheritance Hierarchy

```
┌─────────────────────────────────────────────────────────────────┐
│                     <<abstract>>                                │
│                      BaseSchedule                                │
├─────────────────────────────────────────────────────────────────┤
│  -_scheduleId: string                                           │
│  -_departureCity: string                                        │
│  -_arrivalCity: string                                          │
│  -_departureDateTime: DateTime                                  │
│  -_ticketPrice: decimal                                         │
│  -_assignedBus: BaseBus                                         │
│  -_bookedSeats: HashSet<string>                                 │
├─────────────────────────────────────────────────────────────────┤
│  +IsAvailable(): bool                                           │
│  +GetTimeUntilDeparture(): TimeSpan                             │
│  +GetDiscountedPrice(): decimal (virtual)                       │
│  +BookSeat(): bool                                              │
│  +DisplayScheduleDetails(): void (virtual)                      │
└─────────────────────────────────────────────────────────────────┘
                              ▲
              ┌───────────────┼───────────────┐
              │               │               │
┌─────────────────────┐ ┌─────────────────────────────────────────┐
│  RegularSchedule    │ │            ExpressSchedule               │
│      (📅)           │ │              (⚡)                         │
├─────────────────────┤ ├─────────────────────────────────────────┤
│ Standard discount   │ │ Limited discount (max 10%)               │
│ rules               │ │ Premium features                         │
└─────────────────────┘ └─────────────────────────────────────────┘
```

### 🔗 Interfaces Implemented

| Interface | Purpose |
|:----------|:--------|
| `IScheduleInfo` | Schedule data (ScheduleId, DepartureCity, ArrivalCity, DepartureDateTime, TicketPrice) |
| `IScheduleOperations` | Schedule operations (IsAvailable, IsDeparturePassed, GetTimeUntilDeparture, GetDiscountedPrice) |
| `IScheduleDisplay` | Display methods (DisplayScheduleDetails) |

### 🔗 Association with Bus

Every schedule has an `AssignedBus` property of type `BaseBus` (abstraction), demonstrating proper association.

```
┌──────────────────┐                    ┌──────────────────┐
│    Schedule      │                    │       Bus        │
│      (📅)        │───────────────────▶│      (🚌)        │
│                  │    AssignedBus     │                  │
│  +AssignedBus    │                    │                  │
└──────────────────┘                    └──────────────────┘
```

---

## 🎫 Class 4: Ticket.cs

### 📁 File: `Ticket.cs`

### 🎯 Purpose
Represents a confirmed ticket booking with seat assignment and status tracking.

### 📋 Properties

| Property | Type | Description |
|:---------|:-----|:------------|
| `TicketId` | string | Unique identifier for the ticket |
| `UserId` | string | ID of user who booked the ticket |
| `ScheduleId` | string | ID of the schedule |
| `SeatNumber` | int | Assigned seat number |
| `PricePaid` | decimal | Amount paid for the ticket |
| `BookingDate` | DateTime | Date and time of booking |
| `Status` | TicketStatus | Booked, Cancelled, or Completed |

### 🔧 Key Methods

| Method | Description |
|:-------|:------------|
| `MarkAsCancelled()` | Changes ticket status to Cancelled |
| `MarkAsCompleted()` | Changes ticket status to Completed |
| `DisplayTicketInfo()` | Displays complete ticket information |

### 🎫 Ticket Status Flow

```
┌─────────────────────────────────────────────────────────────────┐
│                      TICKET STATUS FLOW                         │
├─────────────────────────────────────────────────────────────────┤
│                                                                  │
│    ┌──────────┐      Booking      ┌──────────┐                  │
│    │  Created │ ─────────────────▶│  Booked  │                  │
│    └──────────┘                    └────┬─────┘                  │
│                                          │                        │
│                          ┌───────────────┼───────────────┐        │
│                          │               │               │        │
│                          ▼               ▼               ▼        │
│                    ┌──────────┐    ┌──────────┐    ┌──────────┐   │
│                    │Completed │    │Cancelled │    │  Active  │   │
│                    └──────────┘    └──────────┘    └──────────┘   │
│                                                                  │
└─────────────────────────────────────────────────────────────────┘
```

### 🔗 Interfaces Implemented

| Interface | Purpose |
|:----------|:--------|
| `ITicketInfo` | Ticket data (TicketId, UserId, ScheduleId, SeatNumber, PricePaid, BookingDate, Status) |
| `ITicketOperations` | Ticket operations (MarkAsCancelled, MarkAsCompleted, DisplayTicketInfo) |
| `ITicketBookingService` | Booking operations (GetAvailableSchedules, IsSeatAvailableForSchedule, BookTicket, CancelTicket, GetUserTickets) |

### 📋 TicketBookingService Key Methods

| Method | Description |
|:-------|:------------|
| `GetAvailableSchedules()` | Returns list of schedules available for booking |
| `IsSeatAvailableForSchedule()` | Validates seat number and checks duplicates |
| `BookTicket(userId, scheduleId, seatNumber, payment)` | Completes full booking process |
| `CancelTicket(ticketId)` | Cancels an existing ticket |
| `GetUserTickets(userId)` | Returns all tickets for a user |

### ✅ Booking Validation Rules

| Rule | Implementation |
|:-----|:---------------|
| Seat number range | Must be between 1 and bus TotalCapacity |
| Duplicate seat | Same seat cannot be booked twice for same schedule |
| Schedule availability | Cannot book past departure or fully booked schedule |
| Payment validation | Payment must be >= ticket price |

---

## 📄 Class 5: Invoice.cs

### 📁 File: `Invoice.cs`

### 🎯 Purpose
Represents an invoice generated for each confirmed ticket booking.

### 📋 Properties

| Property | Type | Description |
|:---------|:-----|:------------|
| `InvoiceId` | string | Unique identifier for the invoice |
| `TicketId` | string | Associated ticket ID |
| `UserId` | string | User ID who owns the invoice |
| `AmountDue` | decimal | Amount to be paid |
| `GenerationDate` | DateTime | Date invoice was created |
| `Status` | PaymentStatus | Unpaid, Paid, or PartiallyPaid |

### 🔧 Key Methods

| Method | Description |
|:-------|:------------|
| `MarkAsPaid()` | Changes status to Paid |
| `MarkAsPartiallyPaid(amount)` | Updates status and reduces amount due |
| `DisplayInvoice()` | Displays complete invoice information |

### 💳 Payment Status Flow

```
┌─────────────────────────────────────────────────────────────────┐
│                     PAYMENT STATUS FLOW                         │
├─────────────────────────────────────────────────────────────────┤
│                                                                  │
│    ┌──────────┐                 ┌──────────┐                    │
│    │  Unpaid  │ ────Full Payment▶│   Paid   │                    │
│    └────┬─────┘                 └──────────┘                    │
│         │                                                        │
│         │ Partial Payment                                        │
│         ▼                                                        │
│    ┌────────────────┐                                           │
│    │ PartiallyPaid  │────Full Payment──────────────────────────▶│
│    └────────────────┘                                           │
│                                                                  │
└─────────────────────────────────────────────────────────────────┘
```

### 🔗 Interfaces Implemented

| Interface | Purpose |
|:----------|:--------|
| `IInvoiceInfo` | Invoice data (InvoiceId, TicketId, UserId, AmountDue, GenerationDate, Status) |
| `IInvoiceOperations` | Invoice operations (MarkAsPaid, MarkAsPartiallyPaid, DisplayInvoice) |
| `IPaymentService` | Payment operations (GenerateInvoiceForTicket, GetUserInvoices, GetOutstandingInvoices, MakePayment, MakeFullPayment) |

### 💰 PaymentService Key Methods

| Method | Description |
|:-------|:------------|
| `GenerateInvoiceForTicket(ticket)` | Creates invoice automatically after booking |
| `GetUserInvoices(userId)` | Returns all invoices for a user |
| `GetOutstandingInvoices(userId)` | Returns unpaid invoices |
| `MakePayment(invoiceId, amount)` | Processes payment for an invoice |
| `MakeFullPayment(invoiceId)` | Processes full payment |

---

## 🖥️ Class 6: Program.cs

### 📁 File: `Program.cs`

### 🎯 Purpose
Main application entry point providing console-based user interface for all system operations.

### 📋 Available Operations (11 Total)

| Option | Operation | Description |
|:------:|:----------|:------------|
| **1** | Create User | Takes name, mobile, email, user type |
| **2** | Display All Users | Shows all registered users |
| **3** | Create Bus | Takes Bus ID, Coach number, classification |
| **4** | Display All Buses | Shows all buses in fleet |
| **5** | Create Schedule | Takes route, date/time, price, assigns to bus |
| **6** | Display All Schedules | Shows all schedules |
| **7** | Display Schedule Details | Shows detailed info with seating chart |
| **8** | Book Ticket | Complete booking with payment |
| **9** | Display User Invoices | Shows all invoices for selected user |
| **10** | Process Invoice Payment | Submit payment for outstanding invoice |
| **11** | Display User Tickets | Shows all tickets for selected user |
| **0** | Exit | Exits application |

### 🔧 Key Components

| Component | Type | Purpose |
|:----------|:-----|:--------|
| `_users` | `List<BaseUser>` | Stores all users |
| `_buses` | `List<BaseBus>` | Stores all buses |
| `_schedules` | `List<BaseSchedule>` | Stores all schedules |
| `_bookingService` | `ITicketBookingService` | Handles ticket booking |
| `_paymentService` | `IPaymentService` | Handles payments |
| `_userFactory` | `IUserFactory` | Creates users |
| `_busFactory` | `IBusFactory` | Creates buses |
| `_scheduleFactory` | `IScheduleFactory` | Creates schedules |

### 📊 Main Program Flow Diagram

```
┌─────────────────────────────────────────────────────────────────┐
│                          PROGRAM START                          │
└─────────────────────────────────────────────────────────────────┘
                                │
                                ▼
┌─────────────────────────────────────────────────────────────────┐
│              Initialize Factories and Services                  │
│  _userFactory, _busFactory, _scheduleFactory, _paymentService   │
└─────────────────────────────────────────────────────────────────┘
                                │
                                ▼
┌─────────────────────────────────────────────────────────────────┐
│                     Display Main Menu                           │
│  ┌─────────────────────────────────────────────────────────┐    │
│  │  1. Create User         2. Display All Users            │    │
│  │  3. Create Bus          4. Display All Buses            │    │
│  │  5. Create Schedule     6. Display All Schedules        │    │
│  │  7. Schedule Details    8. Book Ticket                  │    │
│  │  9. User Invoices      10. Process Payment              │    │
│  │ 11. User Tickets        0. Exit                         │    │
│  └─────────────────────────────────────────────────────────┘    │
└─────────────────────────────────────────────────────────────────┘
                                │
                                ▼
┌─────────────────────────────────────────────────────────────────┐
│                        User Input                               │
│                    (Switch Case Choice)                         │
└─────────────────────────────────────────────────────────────────┘
                                │
            ┌───────────────────┼───────────────────┐
            │                   │                   │
            ▼                   ▼                   ▼
    ┌───────────────┐   ┌───────────────┐   ┌───────────────┐
    │  Create User  │   │  Create Bus   │   │Book Ticket    │
    └───────────────┘   └───────────────┘   └───────┬───────┘
            │                   │                   │
            ▼                   ▼                   ▼
    ┌───────────────┐   ┌───────────────┐   ┌───────────────┐
    │Display Users  │   │Display Buses  │   │Generate Invoice│
    └───────────────┘   └───────────────┘   └───────────────┘
                                │
                                ▼
┌─────────────────────────────────────────────────────────────────┐
│                    Wait for Enter Key                           │
└─────────────────────────────────────────────────────────────────┘
                                │
                                ▼
┌─────────────────────────────────────────────────────────────────┐
│                       Clear Screen                              │
└─────────────────────────────────────────────────────────────────┘
                                │
                                ▼
┌─────────────────────────────────────────────────────────────────┐
│                      Loop Back to Menu                          │
└─────────────────────────────────────────────────────────────────┘
```

### 📝 Ticket Booking Flow in Program.cs

```
┌─────────────────────────────────────────────────────────────────┐
│                     TICKET BOOKING FLOW                         │
├─────────────────────────────────────────────────────────────────┤
│                                                                  │
│  Step 1: User selects option 8                                  │
│         │                                                       │
│         ▼                                                       │
│  Step 2: System calls _bookingService.GetAvailableSchedules()   │
│         │                                                       │
│         ▼                                                       │
│  Step 3: User selects schedule from list                        │
│         │                                                       │
│         ▼                                                       │
│  Step 4: System displays seating chart                          │
│         │                                                       │
│         ▼                                                       │
│  Step 5: User selects seat number                               │
│         │                                                       │
│         ▼                                                       │
│  Step 6: System validates seat availability                     │
│         │                                                       │
│         ▼                                                       │
│  Step 7: User enters payment amount                             │
│         │                                                       │
│         ▼                                                       │
│  Step 8: System calls _bookingService.BookTicket()              │
│         │                                                       │
│         ▼                                                       │
│  Step 9: System calls _paymentService.GenerateInvoiceForTicket()│
│         │                                                       │
│         ▼                                                       │
│  Step 10: System displays ticket and invoice                    │
│                                                                  │
└─────────────────────────────────────────────────────────────────┘
```

---

## 🏭 Factory Pattern

### 👤 User Factory

```csharp
┌─────────────────────────────────────────────────────────────────┐
│                     IUserFactory (Interface)                     │
├─────────────────────────────────────────────────────────────────┤
│  +CreateUser(fullName, mobileNumber, email, userType): BaseUser │
└─────────────────────────────────────────────────────────────────┘
                                ▲
                                │
┌─────────────────────────────────────────────────────────────────┐
│                       UserFactory (Concrete)                     │
├─────────────────────────────────────────────────────────────────┤
│  +CreateUser(): BaseUser                                        │
│  {                                                              │
│      return userType == "premium" ? new PremiumUser()           │
│                                    : new RegularUser();         │
│  }                                                              │
└─────────────────────────────────────────────────────────────────┘
```

### 🚌 Bus Factory

```csharp
┌─────────────────────────────────────────────────────────────────┐
│                     IBusFactory (Interface)                      │
├─────────────────────────────────────────────────────────────────┤
│  +CreateBus(busId, coachNumber, classification): BaseBus        │
└─────────────────────────────────────────────────────────────────┘
                                ▲
                                │
┌─────────────────────────────────────────────────────────────────┐
│                       BusFactory (Concrete)                      │
├─────────────────────────────────────────────────────────────────┤
│  +CreateBus(): BaseBus                                          │
│  {                                                              │
│      return classification switch                               │
│      {                                                          │
│          Economy  => new EconomyBus(),                          │
│          Business => new BusinessBus(),                         │
│          Luxury   => new LuxuryBus()                            │
│      };                                                         │
│  }                                                              │
└─────────────────────────────────────────────────────────────────┘
```

### 📅 Schedule Factory

```csharp
┌─────────────────────────────────────────────────────────────────┐
│                    IScheduleFactory (Interface)                  │
├─────────────────────────────────────────────────────────────────┤
│  +CreateSchedule(scheduleId, departureCity, arrivalCity,        │
│   departureDateTime, ticketPrice, assignedBus,                  │
│   scheduleType): BaseSchedule                                   │
└─────────────────────────────────────────────────────────────────┘
                                ▲
                                │
┌─────────────────────────────────────────────────────────────────┐
│                     ScheduleFactory (Concrete)                   │
├─────────────────────────────────────────────────────────────────┤
│  +CreateSchedule(): BaseSchedule                                │
│  {                                                              │
│      return scheduleType == "express" ? new ExpressSchedule()   │
│                                        : new RegularSchedule(); │
│  }                                                              │
└─────────────────────────────────────────────────────────────────┘
```

---

## 🔄 System Workflow

### 1️⃣ User Creation Flow

```
┌──────────┐    ┌────────────┐    ┌─────────────────┐    ┌─────────────────┐
│  User    │───▶│  User      │───▶│ RegularUser or  │───▶│ Add to _users   │
│  Input   │    │  Factory   │    │  PremiumUser    │    │  List           │
└──────────┘    └────────────┘    └─────────────────┘    └─────────────────┘
```

### 2️⃣ Bus Creation Flow

```
┌──────────┐    ┌───────────┐    ┌─────────────────────────────────────┐
│  User    │───▶│  Bus      │───▶│ EconomyBus/BusinessBus/LuxuryBus    │
│  Input   │    │  Factory  │    │                                     │
└──────────┘    └───────────┘    └─────────────────┬───────────────────┘
                                                    │
                                                    ▼
                                    ┌─────────────────────────────────┐
                                    │        Add to _buses List        │
                                    └─────────────────────────────────┘
```

### 3️⃣ Schedule Creation Flow

```
┌──────────┐    ┌─────────────────┐    ┌─────────────────────────────────┐
│  User    │───▶│  Schedule       │───▶│ RegularSchedule or              │
│  Input   │    │  Factory        │    │ ExpressSchedule                 │
└──────────┘    └─────────────────┘    └─────────────────┬───────────────┘
                                                          │
                                                          ▼
                                          ┌─────────────────────────────────┐
                                          │   Associated with Selected Bus   │
                                          │                                 │
                                          │   +AssignedBus = selectedBus    │
                                          └─────────────────┬───────────────┘
                                                            │
                                                            ▼
                                          ┌─────────────────────────────────┐
                                          │     Add to _schedules List       │
                                          └─────────────────────────────────┘
```

### 4️⃣ Ticket Booking Flow

```
┌──────────┐    ┌─────────────────────┐    ┌─────────────────────────────────┐
│  User    │───▶│  Browse Available   │───▶│  Select Schedule                │
│          │    │  Schedules          │    │                                 │
└──────────┘    └─────────────────────┘    └─────────────────┬───────────────┘
                                                              │
                                                              ▼
                                              ┌─────────────────────────────────┐
                                              │  Select Seat                    │
                                              │                                 │
                                              │  Validate:                      │
                                              │  ✓ Within capacity              │
                                              │  ✓ Not already reserved         │
                                              └─────────────────┬───────────────┘
                                                                │
                                                                ▼
                                              ┌─────────────────────────────────┐
                                              │  Make Payment                   │
                                              │                                 │
                                              │  Validate:                      │
                                              │  ✓ Payment >= Ticket Price      │
                                              └─────────────────┬───────────────┘
                                                                │
                                                                ▼
                                              ┌─────────────────────────────────┐
                                              │  Generate Ticket                │
                                              │                                 │
                                              │  - Unique Ticket ID             │
                                              │  - Mark Seat as Reserved        │
                                              │  - Add to User's Bookings       │
                                              └─────────────────┬───────────────┘
                                                                │
                                                                ▼
                                              ┌─────────────────────────────────┐
                                              │  Auto-Generate Invoice          │
                                              │                                 │
                                              │  - Status: UNPAID               │
                                              │  - Amount: Ticket Price         │
                                              └─────────────────────────────────┘
```

### 5️⃣ Payment Flow

```
┌──────────┐    ┌─────────────────────┐    ┌─────────────────────────────────┐
│  User    │───▶│  View Outstanding   │───▶│  Select Invoice                 │
│          │    │  Invoices           │    │                                 │
└──────────┘    └─────────────────────┘    └─────────────────┬───────────────┘
                                                              │
                                                              ▼
                                              ┌─────────────────────────────────┐
                                              │  Submit Payment                 │
                                              │                                 │
                                              │  Full Payment OR Partial Payment│
                                              └─────────────────┬───────────────┘
                                                                │
                                                                ▼
                                              ┌─────────────────────────────────┐
                                              │  Update Invoice Status          │
                                              │                                 │
                                              │  Full Payment  → PAID           │
                                              │  Partial Payment→ PARTIALLY_PAID│
                                              └─────────────────────────────────┘
```

---

## 🚀 How to Run

### 📋 Prerequisites

| Requirement | Version |
|:------------|:--------|
| .NET SDK | 10.0 or higher |
| C# Extension | For VS Code (optional) |
| Operating System | Windows/Linux/macOS |

### 📝 Step-by-Step Instructions

#### Step 1: Create Project

```bash
# Create new console project
dotnet new console -n BusTicketBookingSystem

# Navigate to project directory
cd BusTicketBookingSystem
```

#### Step 2: Add Class Files

Create the following files in the project directory:

```
BusTicketBookingSystem/
├── Program.cs
├── User.cs
├── Bus.cs
├── Schedule.cs
├── Ticket.cs
└── Invoice.cs
```

#### Step 3: Build the Project

```bash
dotnet build
```

#### Step 4: Run the Project

```bash
dotnet run
```

---

## 🧪 Test Sequence

Follow this sequence to test all features:

| Step | Action | Expected Result |
|:----:|:-------|:----------------|
| **1** | Select option **1** | Create a user (e.g., John Doe) |
| **2** | Select option **3** | Create an Economy bus (BUS001) |
| **3** | Select option **5** | Create a schedule (Dhaka → Chittagong) |
| **4** | Select option **6** | Display all schedules |
| **5** | Select option **7** | View schedule details with seating chart |
| **6** | Select option **8** | Book a ticket (seat 15) |
| **7** | Select option **11** | Display user tickets |
| **8** | Select option **9** | Display user invoices (should show UNPAID) |
| **9** | Select option **10** | Process payment for the invoice |
| **10** | Select option **9** | Verify invoice shows PAID status |
| **11** | Select option **2** | Display all users |
| **12** | Select option **4** | Display all buses |
| **13** | Select option **0** | Exit application |

---

## 📊 Error Handling

| Scenario | Validation | Error Message |
|:---------|:-----------|:---------------|
| Empty fields | `string.IsNullOrWhiteSpace()` | "XXX cannot be empty" |
| Invalid email format | Must contain @ and . | "Invalid email format" |
| Invalid mobile number | Must be exactly 11 digits | "Mobile number must be 11 digits" |
| Past departure date | Must be future date | "Departure date cannot be in the past" |
| Duplicate seat booking | Check `_scheduleReservedSeats` | "Seat X is already reserved" |
| Invalid seat number | Must be 1 to capacity | "Seat must be between 1 and X" |
| Insufficient payment | Payment >= ticket price | "Insufficient payment" |
| Non-existent user | Check `_users` list | "User not found" |
| Non-existent schedule | Check `_schedules` list | "Schedule not found" |

---

## 📈 Interfaces Summary (15+)

| Interface | Implemented By | Purpose |
|:----------|:---------------|:--------|
| `IUserData` | `BaseUser` | User data access |
| `IBookable` | `BaseUser` | Booking operations |
| `IUserDisplay` | `BaseUser` | Display operations |
| `ISeatManageable` | `BaseBus` | Seat operations |
| `IBusInfo` | `BaseBus` | Bus information |
| `IPricingStrategy` | `BaseBus` | Pricing methods |
| `IDisplayable` | `BaseBus` | Display methods |
| `IScheduleInfo` | `BaseSchedule` | Schedule data |
| `IScheduleOperations` | `BaseSchedule` | Schedule operations |
| `IScheduleDisplay` | `BaseSchedule` | Display methods |
| `ITicketInfo` | `Ticket` | Ticket data |
| `ITicketOperations` | `Ticket` | Ticket operations |
| `ITicketBookingService` | `TicketBookingService` | Booking service |
| `IInvoiceInfo` | `Invoice` | Invoice data |
| `IInvoiceOperations` | `Invoice` | Invoice operations |
| `IPaymentService` | `PaymentService` | Payment service |
| `IUserFactory` | `UserFactory` | User creation |
| `IBusFactory` | `BusFactory` | Bus creation |
| `IScheduleFactory` | `ScheduleFactory` | Schedule creation |

---

## 🏆 OOP Pillars Summary

| Pillar | Icon | Location in Code |
|:-------|:----:|:-----------------|
| **Encapsulation** | 🔒 | Private fields with public properties in all classes |
| **Inheritance** | 🌳 | BaseUser→RegularUser/PremiumUser, BaseBus→EconomyBus/BusinessBus/LuxuryBus, BaseSchedule→RegularSchedule/ExpressSchedule |
| **Polymorphism** | 🔄 | Virtual/override methods: `GetDiscountedPrice()`, `DisplayScheduleDetails()`, `GetBasePriceMultiplier()` |
| **Abstraction** | 📦 | Abstract base classes and 15+ interfaces |

---

## 📐 SOLID Principles Summary

| Principle | Icon | Location in Code |
|:----------|:----:|:-----------------|
| **Single Responsibility** | S | Each class has one purpose (User, Bus, Schedule, Ticket, Invoice) |
| **Open/Closed** | O | Base classes open for extension (PremiumUser extends BaseUser) |
| **Liskov Substitution** | L | Derived classes replace base classes anywhere |
| **Interface Segregation** | I | 15+ focused interfaces instead of one large interface |
| **Dependency Inversion** | D | Factory pattern with abstraction dependencies |

---

## ✅ Assignment Requirements Checklist

| Requirement | Status |
|:------------|:------:|
| 1. Create User | ✅ |
| 2. Display All Users | ✅ |
| 3. Create Bus | ✅ |
| 4. Display All Buses | ✅ |
| 5. Create Schedule | ✅ |
| 6. Display All Schedules | ✅ |
| 7. Display Schedule Details | ✅ |
| 8. Book Ticket | ✅ |
| 9. Display User Invoices | ✅ |
| 10. Process Invoice Payment | ✅ |
| 11. Display User Tickets | ✅ |
| OOP Pillars (4/4) | ✅ |
| SOLID Principles (5/5) | ✅ |

---

## 👨‍💻 Author

This project was completed as part of a C# Object-Oriented Programming assignment demonstrating mastery of OOP pillars and SOLID design principles.

---

## 📄 License

This project is for educational purposes as part of the ServerCamp OOP Assignment.

---

## 🙏 Thank You

Thank you for reviewing this Bus Ticket Booking & Billing System. The system demonstrates complete implementation of all 11 required operations with full adherence to OOP pillars and SOLID principles.
