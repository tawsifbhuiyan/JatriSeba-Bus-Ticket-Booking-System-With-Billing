Introduction
This project is a comprehensive Bus Ticket Booking & Billing System developed as part of a C# Object-Oriented Programming assignment. The application enables users to book bus tickets, select seats, generate invoices, and process payments.

The primary goal of this project is to demonstrate a thorough understanding of:

Four Pillars of OOP: Encapsulation, Inheritance, Polymorphism, Abstraction

SOLID Design Principles: Single Responsibility, Open/Closed, Liskov Substitution, Interface Segregation, Dependency Inversion

Key Features
Feature	Description
User Management	Create and manage user accounts with unique IDs
Bus Management	Manage fleet of buses with different classifications
Schedule Management	Create schedules with departure/arrival cities, date/time, and pricing
Ticket Booking	Browse schedules, select seats, and complete reservations
Invoice Generation	Automatic invoice creation for every confirmed booking
Payment Processing	Submit payments for outstanding invoices
Viewing Operations	Display users, buses, schedules, tickets, and invoices
Technology Stack
Component	Technology
Language	C# 10.0
Framework	.NET 10.0
Application Type	Console Application
Build Tool	dotnet CLI
Project Structure
text
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
Class Dependency Diagram
text
┌──────────┐     ┌──────────┐     ┌──────────────┐
│   User   │     │   Bus    │     │   Schedule   │
└────┬─────┘     └────┬─────┘     └──────┬───────┘
     │                │                  │
     │                └──────────────────┼───────┐
     │                                   │       │
     ▼                                   ▼       ▼
┌──────────┐                       ┌─────────────────┐
│  Ticket  │◄──────────────────────│ TicketBooking   │
└────┬─────┘                       │    Service      │
     │                             └─────────────────┘
     │                                      │
     ▼                                      ▼
┌──────────┐                       ┌─────────────────┐
│ Invoice  │◄──────────────────────│   Payment       │
└──────────┘                       │    Service      │
                                   └─────────────────┘
OOP Pillars Implementation Overview
Pillar	How It's Implemented
Encapsulation	Private fields with public properties containing validation logic
Inheritance	BaseUser → RegularUser/PremiumUser, BaseBus → EconomyBus/BusinessBus/LuxuryBus, BaseSchedule → RegularSchedule/ExpressSchedule
Polymorphism	Virtual/override methods like GetDiscountedPrice(), DisplayScheduleDetails()
Abstraction	Abstract base classes and interfaces hiding implementation details
SOLID Principles Implementation Overview
Principle	How It's Implemented
Single Responsibility	Each class has one reason to change (User handles users, Bus handles buses, etc.)
Open/Closed	Base classes open for extension, closed for modification
Liskov Substitution	Derived classes can replace base classes without breaking functionality
Interface Segregation	Small, focused interfaces instead of one large interface
Dependency Inversion	Factories depend on abstractions, not concrete classes
Class 1: User Class
File: User.cs
Purpose
Manages user accounts with support for multiple ticket bookings per user.

Properties
Property	Type	Description
UserId	string	Unique identifier (auto-generated)
FullName	string	User's full name
MobileNumber	string	11-digit mobile number with validation
Email	string	Email address with format validation
TotalBookings	int	Total number of bookings made
ActiveBookings	int	Number of active (non-cancelled) bookings
Key Methods
Method	Description
AddBooking(ticketId)	Adds a ticket to user's booking list
CancelBooking(ticketId)	Cancels a specific booking
GetActiveBookings()	Returns list of active ticket IDs
DisplayUserInfo()	Displays user information
DisplayAllBookings()	Shows complete booking history
OOP Pillars Demonstrated
Pillar	Implementation
Encapsulation	Private fields _mobileNumber, _email, _bookingRecords with public validated properties
Inheritance	BaseUser abstract class → RegularUser, PremiumUser
Polymorphism	Virtual methods AddBooking(), CancelBooking(), DisplayUserInfo() overridden in derived classes
Abstraction	Abstract GenerateUserId() method, IUserData, IBookable, IUserDisplay interfaces
SOLID Principles Demonstrated
Principle	Implementation
Single Responsibility	User class only manages user data; BookingRecord class separately manages booking details
Open/Closed	BaseUser open for extension (PremiumUser adds loyalty points, discount codes) without modifying base
Liskov Substitution	RegularUser and PremiumUser can replace BaseUser anywhere in the code
Interface Segregation	Three focused interfaces: IUserData, IBookable, IUserDisplay instead of one large interface
Dependency Inversion	IUserFactory abstraction; UserFactory depends on abstraction, not concrete classes
Class Hierarchy
text
┌─────────────────────────────────────────────────────────────┐
│                      <<abstract>>                           │
│                        BaseUser                              │
├─────────────────────────────────────────────────────────────┤
│ #_userId: string                                             │
│ #_fullName: string                                           │
│ #_mobileNumber: string                                       │
│ #_email: string                                              │
│ #_bookingRecords: List<BookingRecord>                        │
├─────────────────────────────────────────────────────────────┤
│ +AddBooking(): void (virtual)                                │
│ +CancelBooking(): bool (virtual)                             │
│ +DisplayUserInfo(): void (virtual)                           │
│ #GenerateUserId(): string (abstract)                         │
└─────────────────────────────────────────────────────────────┘
                              ▲
              ┌───────────────┼───────────────┐
              │               │               │
┌──────────────────┐ ┌─────────────────────────────────────────┐
│   RegularUser    │ │              PremiumUser                 │
├──────────────────┤ ├─────────────────────────────────────────┤
│ Standard booking │ │ +LoyaltyPoints: int                      │
│ behavior         │ │ +DiscountCodes: List<string>             │
└──────────────────┘ │ +AddDiscountCode(): void                 │
                     │ Overridden booking behavior              │
                     └─────────────────────────────────────────┘
Class 2: Bus Class
File: Bus.cs
Purpose
Manages the bus fleet with different classifications and seat reservation tracking.

Properties
Property	Type	Description
BusId	string	Unique identifier for the bus
CoachNumber	string	Coach identification number
Classification	BusClassification	Economy, Business, or Luxury
TotalCapacity	int	Total seats determined by classification
AvailableSeatsCount	int	Number of available seats
ReservedSeatsCount	int	Number of reserved seats
OccupancyRate	double	Percentage of seats reserved
Key Methods
Method	Description
ReserveSeat(seatNumber)	Reserves a specific seat on the bus
CancelReservation(seatNumber)	Cancels seat reservation
IsSeatAvailable(seatNumber)	Checks if a seat is available
GetAvailableSeatsList()	Returns list of all available seats
DisplaySeatingChart()	Shows visual seating arrangement
GetPriceMultiplier()	Returns price multiplier based on bus class
OOP Pillars Demonstrated
Pillar	Implementation
Encapsulation	Private _reservedSeats, _availableSeats HashSets with public read-only access
Inheritance	BaseBus abstract class → EconomyBus, BusinessBus, LuxuryBus
Polymorphism	Virtual ReserveSeat(), GetBasePriceMultiplier(), DisplaySeatingChart() overridden in derived classes
Abstraction	Abstract GetCapacityByClassification() and GetBusType() methods; ISeatManageable, IBusInfo, IPricingStrategy interfaces
SOLID Principles Demonstrated
Principle	Implementation
Single Responsibility	Bus class only manages bus and seat data; separate factory handles creation
Open/Closed	BaseBus open for extension (new bus types can be added without modifying base)
Liskov Substitution	Any derived bus type can replace BaseBus in ticketing operations
Interface Segregation	Separate interfaces: ISeatManageable, IBusInfo, IPricingStrategy, IDisplayable
Dependency Inversion	IBusFactory abstraction; BusFactory depends on abstraction
Bus Classification Capacities
Classification	Capacity	Price Multiplier	Features
Economy	50 seats	1.0x	Standard seating
Business	35 seats	1.5x	Extra legroom, WiFi
Luxury	25 seats	2.5x	Reclining seats, entertainment, refreshments
Class Hierarchy
text
┌─────────────────────────────────────────────────────────────┐
│                      <<abstract>>                           │
│                        BaseBus                               │
├─────────────────────────────────────────────────────────────┤
│ #_busId: string                                              │
│ #_coachNumber: string                                        │
│ #_classification: BusClassification                          │
│ #_reservedSeats: HashSet<int>                                │
│ #_availableSeats: HashSet<int>                               │
├─────────────────────────────────────────────────────────────┤
│ +ReserveSeat(): bool (virtual)                               │
│ +CancelReservation(): bool (virtual)                         │
│ +DisplaySeatingChart(): void (virtual)                       │
│ #GetCapacityByClassification(): int (abstract)               │
│ +GetBasePriceMultiplier(): decimal (abstract)                │
│ +GetBusType(): string (abstract)                             │
└─────────────────────────────────────────────────────────────┘
                              ▲
              ┌───────────────┼───────────────┐
              │               │               │
┌──────────────────┐ ┌──────────────────┐ ┌──────────────────┐
│   EconomyBus     │ │   BusinessBus    │ │    LuxuryBus     │
├──────────────────┤ ├──────────────────┤ ├──────────────────┤
│ Capacity: 50     │ │ Capacity: 35     │ │ Capacity: 25     │
│ Multiplier: 1.0x │ │ Multiplier: 1.5x │ │ Multiplier: 2.5x │
│ Standard seating │ │ WiFi, Charging   │ │ All amenities    │
└──────────────────┘ └──────────────────┘ └──────────────────┘
Class 3: Schedule Class
File: Schedule.cs
Purpose
Manages bus schedules including routes, departure times, and ticket pricing.

Properties
Property	Type	Description
ScheduleId	string	Unique identifier for the schedule
DepartureCity	string	Origin city
ArrivalCity	string	Destination city
DepartureDateTime	DateTime	Date and time of departure
TicketPrice	decimal	Base ticket price
AssignedBus	BaseBus	Bus assigned to this schedule
BookedSeatsCount	int	Number of seats booked for this schedule
AvailableSeatsCount	int	Number of seats available
IsFullyBooked	bool	Indicates if schedule is fully booked
Key Methods
Method	Description
IsAvailable()	Checks if schedule is available for booking
IsDeparturePassed()	Checks if departure time has passed
GetTimeUntilDeparture()	Returns time remaining until departure
GetDiscountedPrice(percentage)	Calculates price after discount
BookSeat(seatNumber, userId)	Books a seat for this schedule
CancelSeatBooking(seatNumber, userId)	Cancels seat booking
DisplayScheduleDetails()	Shows complete schedule information
OOP Pillars Demonstrated
Pillar	Implementation
Encapsulation	Private fields with public validated properties; _bookedSeats HashSet tracks reservations
Inheritance	BaseSchedule abstract class → RegularSchedule, ExpressSchedule
Polymorphism	Virtual GetDiscountedPrice(), DisplayScheduleDetails() overridden in derived classes
Abstraction	Abstract base class; IScheduleInfo, IScheduleOperations, IScheduleDisplay interfaces
SOLID Principles Demonstrated
Principle	Implementation
Single Responsibility	Schedule class only manages schedule data and seat bookings for that schedule
Open/Closed	BaseSchedule open for extension (new schedule types with different discount rules)
Liskov Substitution	RegularSchedule and ExpressSchedule can replace BaseSchedule
Interface Segregation	Three focused interfaces: IScheduleInfo, IScheduleOperations, IScheduleDisplay
Dependency Inversion	IScheduleFactory abstraction; ScheduleFactory depends on abstraction
Class Hierarchy
text
┌─────────────────────────────────────────────────────────────┐
│                      <<abstract>>                           │
│                      BaseSchedule                            │
├─────────────────────────────────────────────────────────────┤
│ -_scheduleId: string                                         │
│ -_departureCity: string                                      │
│ -_arrivalCity: string                                        │
│ -_departureDateTime: DateTime                                │
│ -_ticketPrice: decimal                                       │
│ -_assignedBus: BaseBus                                       │
│ -_bookedSeats: HashSet<string>                               │
├─────────────────────────────────────────────────────────────┤
│ +IsAvailable(): bool                                         │
│ +GetTimeUntilDeparture(): TimeSpan                           │
│ +GetDiscountedPrice(): decimal (virtual)                     │
│ +BookSeat(): bool                                            │
│ +DisplayScheduleDetails(): void (virtual)                    │
└─────────────────────────────────────────────────────────────┘
                              ▲
              ┌───────────────┼───────────────┐
              │               │               │
┌──────────────────┐ ┌─────────────────────────────────────────┐
│ RegularSchedule  │ │              ExpressSchedule             │
├──────────────────┤ ├─────────────────────────────────────────┤
│ Standard discount│ │ Limited discount (max 10%)               │
│ rules            │ │ Premium features                         │
└──────────────────┘ └─────────────────────────────────────────┘
Class 4: Ticket Class
File: Ticket.cs
Purpose
Represents a confirmed ticket booking with seat assignment and status tracking.

Properties
Property	Type	Description
TicketId	string	Unique identifier for the ticket
UserId	string	ID of user who booked the ticket
ScheduleId	string	ID of the schedule
SeatNumber	int	Assigned seat number
PricePaid	decimal	Amount paid for the ticket
BookingDate	DateTime	Date and time of booking
Status	TicketStatus	Booked, Cancelled, or Completed
Key Methods
Method	Description
MarkAsCancelled()	Changes ticket status to Cancelled
MarkAsCompleted()	Changes ticket status to Completed
DisplayTicketInfo()	Displays complete ticket information
OOP Pillars Demonstrated
Pillar	Implementation
Encapsulation	Private fields with validation in property setters
Inheritance	Implements ITicketInfo and ITicketOperations interfaces
Polymorphism	Virtual DisplayTicketInfo() method
Abstraction	Interfaces hide implementation details
SOLID Principles Demonstrated
Principle	Implementation
Single Responsibility	Ticket class only represents ticket data; booking logic is in TicketBookingService
Open/Closed	Ticket status enum allows extension without modifying class
Interface Segregation	ITicketInfo and ITicketOperations separate data from behavior
Dependency Inversion	ITicketBookingService abstraction; service depends on abstraction
Class 5: TicketBookingService Class
File: Ticket.cs
Purpose
Handles the complete ticket booking process including validation, seat reservation, and ticket generation.

Properties
Property	Type	Description
Tickets	Dictionary	Stores all tickets by ID
UserTickets	Dictionary	Maps users to their tickets
ScheduleReservedSeats	Dictionary	Tracks reserved seats per schedule
Key Methods
Method	Description
GetAvailableSchedules()	Returns list of schedules available for booking
IsSeatAvailableForSchedule()	Validates seat number and checks duplicates
BookTicket(userId, scheduleId, seatNumber, payment)	Completes full booking process
CancelTicket(ticketId)	Cancels an existing ticket
GetUserTickets(userId)	Returns all tickets for a user
OOP Pillars Demonstrated
Pillar	Implementation
Encapsulation	Private dictionaries store ticket data; public methods control access
Polymorphism	Implements ITicketBookingService interface
Abstraction	ITicketBookingService interface hides implementation
SOLID Principles Demonstrated
Principle	Implementation
Single Responsibility	Service only handles ticket booking operations
Open/Closed	Can add new booking rules without modifying existing methods
Dependency Inversion	Depends on BaseSchedule and BaseUser abstractions, not concrete classes
Class 6: Invoice Class
File: Invoice.cs
Purpose
Represents an invoice generated for each confirmed ticket booking.

Properties
Property	Type	Description
InvoiceId	string	Unique identifier for the invoice
TicketId	string	Associated ticket ID
UserId	string	User ID who owns the invoice
AmountDue	decimal	Amount to be paid
GenerationDate	DateTime	Date invoice was created
Status	PaymentStatus	Unpaid, Paid, or PartiallyPaid
Key Methods
Method	Description
MarkAsPaid()	Changes status to Paid
MarkAsPartiallyPaid(amount)	Updates status and reduces amount due
DisplayInvoice()	Displays complete invoice information
OOP Pillars Demonstrated
Pillar	Implementation
Encapsulation	Private fields with validation in property setters
Polymorphism	Implements IInvoiceInfo and IInvoiceOperations interfaces
Abstraction	Interfaces hide implementation details
Class 7: PaymentService Class
File: Invoice.cs
Purpose
Handles invoice generation and payment processing.

Properties
Property	Type	Description
Invoices	Dictionary	Stores all invoices by ID
UserInvoices	Dictionary	Maps users to their invoices
TicketToInvoice	Dictionary	Maps tickets to their invoices
Key Methods
Method	Description
GenerateInvoiceForTicket(ticket)	Creates invoice automatically after booking
GetUserInvoices(userId)	Returns all invoices for a user
GetOutstandingInvoices(userId)	Returns unpaid invoices
MakePayment(invoiceId, amount)	Processes payment for an invoice
MakeFullPayment(invoiceId)	Processes full payment
OOP Pillars Demonstrated
Pillar	Implementation
Encapsulation	Private dictionaries store invoice data
Polymorphism	Implements IPaymentService interface
Abstraction	IPaymentService interface hides implementation
SOLID Principles Demonstrated
Principle	Implementation
Single Responsibility	Service only handles invoice and payment operations
Dependency Inversion	Depends on Ticket abstraction, not concrete classes
Class 8: Program Class
File: Program.cs
Purpose
Main application entry point providing console-based user interface for all system operations.

Available Operations
Option	Operation	Description
1	Create User	Creates new user account
2	Display All Users	Shows all registered users
3	Create Bus	Adds new bus to fleet
4	Display All Buses	Shows all buses in fleet
5	Create Schedule	Creates new bus schedule
6	Display All Schedules	Shows all schedules
7	Display Schedule Details	Shows detailed schedule info with seating chart
8	Book Ticket	Complete ticket booking process
9	Display User Invoices	Shows all invoices for a user
10	Process Invoice Payment	Submit payment for outstanding invoice
11	Display User Tickets	Shows all tickets for a user
0	Exit	Exits the application
Key Components
Component	Description
_users	List storing all User objects
_buses	List storing all Bus objects
_schedules	List storing all Schedule objects
_bookingService	TicketBookingService instance
_paymentService	PaymentService instance
_userFactory	Factory for creating users
_busFactory	Factory for creating buses
_scheduleFactory	Factory for creating schedules
Main Program Flow
text
START
  │
  ▼
Initialize Factories and Services
  │
  ▼
Display Main Menu
  │
  ▼
User Input ──────────────────────────────────────┐
  │                                               │
  ▼                                               │
Switch Based on Choice                            │
  │                                               │
  ├──► 1: CreateUser()                            │
  ├──► 2: DisplayAllUsers()                       │
  ├──► 3: CreateBus()                             │
  ├──► 4: DisplayAllBuses()                       │
  ├──► 5: CreateSchedule()                        │
  ├──► 6: DisplayAllSchedules()                   │
  ├──► 7: DisplayScheduleDetails()                │
  ├──► 8: BookTicket() ──► Auto-generate Invoice  │
  ├──► 9: DisplayUserInvoices()                   │
  ├──► 10: ProcessInvoicePayment()                │
  ├──► 11: DisplayUserTickets()                   │
  └──► 0: Exit                                    │
  │                                               │
  ▼                                               │
Wait for Enter Key ──────────────────────────────┘
  │
  ▼
Clear Screen
  │
  ▼
Loop Back to Display Menu
Factory Pattern Implementation
User Factory
csharp
public interface IUserFactory
{
    BaseUser CreateUser(string fullName, string mobileNumber, string email, string userType);
}

public class UserFactory : IUserFactory
{
    public BaseUser CreateUser(string fullName, string mobileNumber, string email, string userType)
    {
        return userType.ToLower() switch
        {
            "premium" => new PremiumUser(fullName, mobileNumber, email),
            _ => new RegularUser(fullName, mobileNumber, email)
        };
    }
}
Bus Factory
csharp
public interface IBusFactory
{
    BaseBus CreateBus(string busId, string coachNumber, BusClassification classification);
}

public class BusFactory : IBusFactory
{
    public BaseBus CreateBus(string busId, string coachNumber, BusClassification classification)
    {
        return classification switch
        {
            BusClassification.Economy => new EconomyBus(busId, coachNumber),
            BusClassification.Business => new BusinessBus(busId, coachNumber),
            BusClassification.Luxury => new LuxuryBus(busId, coachNumber),
            _ => throw new ArgumentException("Unknown bus classification")
        };
    }
}
Schedule Factory
csharp
public interface IScheduleFactory
{
    BaseSchedule CreateSchedule(string scheduleId, string departureCity, string arrivalCity,
                                DateTime departureDateTime, decimal ticketPrice, 
                                BaseBus assignedBus, string scheduleType);
}

public class ScheduleFactory : IScheduleFactory
{
    public BaseSchedule CreateSchedule(...)
    {
        return scheduleType.ToLower() switch
        {
            "express" => new ExpressSchedule(...),
            _ => new RegularSchedule(...)
        };
    }
}
Complete System Workflow
1. User Creation Flow
text
User Input ──► UserFactory ──► RegularUser/PremiumUser ──► Add to _users List
2. Bus Creation Flow
text
User Input ──► BusFactory ──► EconomyBus/BusinessBus/LuxuryBus ──► Add to _buses List
3. Schedule Creation Flow
text
User Input ──► ScheduleFactory ──► RegularSchedule/ExpressSchedule ──► Add to _schedules List
                    │
                    ▼
            Associated with Bus
4. Ticket Booking Flow
text
User ──► Browse Available Schedules
  │
  ▼
Select Schedule
  │
  ▼
Select Seat ──► Validate Seat (Capacity & Duplicate Check)
  │
  ▼
Make Payment
  │
  ▼
Generate Ticket ──► Mark Seat as Reserved on Bus
  │
  ▼
Auto-Generate Invoice (Status: Unpaid)
  │
  ▼
Add Booking to User's History
5. Payment Flow
text
User ──► View Outstanding Invoices
  │
  ▼
Select Invoice
  │
  ▼
Submit Payment ──► Full or Partial Payment
  │
  ▼
Update Invoice Status (Paid/PartiallyPaid)
How to Run the Project
Prerequisites
Requirement	Version
.NET SDK	10.0 or higher
C# Extension	For VS Code (optional)
Operating System	Windows/Linux/macOS
Step 1: Clone or Create Project
bash
# Create new console project
dotnet new console -n BusTicketBookingSystem
cd BusTicketBookingSystem
Step 2: Add Class Files
Create the following files in the project directory:

text
BusTicketBookingSystem/
├── Program.cs
├── User.cs
├── Bus.cs
├── Schedule.cs
├── Ticket.cs
└── Invoice.cs
Step 3: Build the Project
bash
dotnet build
Step 4: Run the Project
bash
dotnet run
Sample Test Sequence
Follow this sequence to test all features:

Step	Action	Expected Result
1	Select option 1	Create a user (John Doe)
2	Select option 3	Create an Economy bus (BUS001)
3	Select option 5	Create a schedule (Dhaka → Chittagong)
4	Select option 6	Display all schedules
5	Select option 7	View schedule details with seating chart
6	Select option 8	Book a ticket (seat 15)
7	Select option 11	Display user tickets
8	Select option 9	Display user invoices (should show unpaid)
9	Select option 10	Process payment for the invoice
10	Select option 9	Verify invoice shows PAID status
11	Select option 2	Display all users
12	Select option 4	Display all buses
Error Handling
The system includes validation for:

Scenario	Validation
Empty fields	Throws ArgumentException
Invalid email format	Must contain @ and .
Invalid mobile number	Must be exactly 11 digits
Past departure date	Cannot create schedule in past
Duplicate seat booking	Prevents same seat on same schedule
Invalid seat number	Must be within bus capacity
Insufficient payment	Validates against ticket price
Non-existent user/schedule	Returns appropriate error message
Conclusion
This Bus Ticket Booking & Billing System successfully demonstrates:

OOP Pillars
Pillar	Demonstration
Encapsulation	Private fields exposed through validated public properties
Inheritance	Three complete inheritance hierarchies (User, Bus, Schedule)
Polymorphism	Virtual/override methods providing different behaviors
Abstraction	Abstract base classes and interfaces hiding complexity
SOLID Principles
Principle	Demonstration
Single Responsibility	7+ focused classes each with single purpose
Open/Closed	Base classes allow extension without modification
Liskov Substitution	Derived classes replace base classes seamlessly
Interface Segregation	11+ focused interfaces
Dependency Inversion	Factory pattern with abstraction dependencies
Assignment Requirements
All 11 required operations are implemented and fully functional:

Create User

Display All Users

Create Bus

Display All Buses

Create Schedule

Display All Schedules

Display Schedule Details

Book Ticket

Display User Invoices

Process Invoice Payment

Display User Tickets

Author
This project was completed as part of a C# Object-Oriented Programming assignment demonstrating mastery of OOP pillars and SOLID design principles.

License
This project is for educational purposes as part of the ServerCamp OOP Assignment.

