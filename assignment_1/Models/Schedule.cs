using System;
using System.Collections.Generic;
using System.Linq;

namespace BusTicketBookingSystem
{
    // INTERFACE SEGREGATION PRINCIPLE (ISP) - Focused interface Schedule info er jonno
    public interface IScheduleInfo
    {
        string ScheduleId { get; }
        string DepartureCity { get; }
        string ArrivalCity { get; }
        DateTime DepartureDateTime { get; }
        decimal TicketPrice { get; }
    }

    // INTERFACE SEGREGATION PRINCIPLE (ISP) - Focused interface schedule operations er jonno
    public interface IScheduleOperations
    {
        bool IsAvailable();
        bool IsDeparturePassed();
        TimeSpan GetTimeUntilDeparture();
        decimal GetDiscountedPrice(decimal discountPercentage);
    }

    // INTERFACE SEGREGATION PRINCIPLE (ISP) 
    public interface IScheduleDisplay
    {
        void DisplayScheduleDetails();
    }

    // ABSTRACTION - Abstract base class implementation details hide kore, common functionality provide kore
    // OPEN/CLOSED PRINCIPLE (OCP) - Extension er jonno open, modification er jonno closed
    public abstract class BaseSchedule : IScheduleInfo, IScheduleOperations, IScheduleDisplay
    {
        // ENCAPSULATION - Private fields
        private string _scheduleId;
        private string _departureCity;
        private string _arrivalCity;
        private DateTime _departureDateTime;
        private decimal _ticketPrice;
        private BaseBus _assignedBus;  // ENCAPSULATION - Association with bus
        private HashSet<string>_bookedSeats;  // ENCAPSULATION - Track seats for this schedule

        protected BaseSchedule(string scheduleId, string departureCity, string arrivalCity, 
                               DateTime departureDateTime, decimal ticketPrice, BaseBus assignedBus)
        {
            ScheduleId=scheduleId;
            DepartureCity=departureCity;
            ArrivalCity=arrivalCity;
            DepartureDateTime=departureDateTime;
            TicketPrice=ticketPrice;
            _assignedBus=assignedBus;
            _bookedSeats=new HashSet<string>();
        }

        // ENCAPSULATION - Public properties with validation
        public string ScheduleId 
        { 
            get=>_scheduleId;
            private set
            {
                if(string.IsNullOrWhiteSpace(value))
                    throw new ArgumentException("Schedule ID cannot be empty");
                _scheduleId=value;
            }
        }

        public string DepartureCity
        {
            get=>_departureCity;
            private set
            {
                if(string.IsNullOrWhiteSpace(value))
                    throw new ArgumentException("Departure city cannot be empty");
                _departureCity = value;
            }
        }

        public string ArrivalCity
        {
            get => _arrivalCity;
            private set
            {
                if(string.IsNullOrWhiteSpace(value))
                    throw new ArgumentException("Arrival city cannot be empty");
                _arrivalCity=value;
            }
        }

        public DateTime DepartureDateTime
        {
            get=>_departureDateTime;
            private set
            {
                if(value<DateTime.Now)
                    throw new ArgumentException("Departure date cannot be in the past");
                _departureDateTime=value;
            }
        }

        public decimal TicketPrice
        {
            get=>_ticketPrice;
            private set
            {
                if (value<=0)
                    throw new ArgumentException("Ticket price must be greater than 0");
                _ticketPrice=value;
            }
        }

        // ENCAPSULATION - Association with bus (requirement chilo, but direct access na diye method er maddhome control rakha)
        public BaseBus AssignedBus=>_assignedBus;

        public IReadOnlySet<string> BookedSeats=>_bookedSeats;
        public int BookedSeatsCount=>_bookedSeats.Count;
        public int AvailableSeatsCount=>_assignedBus.AvailableSeatsCount - _bookedSeats.Count;
        public bool IsFullyBooked=>AvailableSeatsCount<= 0;

        // SINGLE RESPONSIBILITY PRINCIPLE (SRP) - Method shudhu schedule availability check kore, departure status o booking status consider kore
        public bool IsAvailable()
        {
            return !IsDeparturePassed()&&!IsFullyBooked;
        }

        // SINGLE RESPONSIBILITY PRINCIPLE (SRP) - Method only check kore if departure time has passed, booking status consider kore na
        public bool IsDeparturePassed()
        {
            return DateTime.Now>_departureDateTime;
        }

        // SINGLE RESPONSIBILITY PRINCIPLE (SRP) - Method only calculate kore time until departure, booking status consider kore na
        public TimeSpan GetTimeUntilDeparture()
        {
            if (IsDeparturePassed())
                return TimeSpan.Zero;
            return _departureDateTime-DateTime.Now;
        }

        // POLYMORPHISM - Virtual method can be overridden by derived classes
        public virtual decimal GetDiscountedPrice(decimal discountPercentage)
        {
            if (discountPercentage<0||discountPercentage>100)
                throw new ArgumentException("Discount must be between 0 and 100");
            
            decimal finalPrice=_ticketPrice*(1-discountPercentage/100);
            
            // Apply bus class multiplier (POLYMORPHISM - uses bus's pricing strategy)
             finalPrice*=_assignedBus.GetBasePriceMultiplier();
            return Math.Round(finalPrice, 2);
        }

        // SINGLE RESPONSIBILITY PRINCIPLE (SRP) - Method only seat booking kore
        public bool BookSeat(int seatNumber, string userId)
        {
            
            if (seatNumber<1||seatNumber>_assignedBus.TotalCapacity)
            {
                Console.WriteLine($" Invalid seat number. Must be between 1 and {_assignedBus.TotalCapacity}");
                return false;
            }

            string seatKey=$"{seatNumber}:{userId}";
            
            
            if (_bookedSeats.Contains(seatKey))
            {
                Console.WriteLine($" Seat {seatNumber} is already booked for this schedule");
                return false;
            }

            
            if (!_assignedBus.IsSeatAvailable(seatNumber))
            {
                Console.WriteLine($" Seat {seatNumber} is not available on the bus");
                return false;
            }

            
            if (_assignedBus.ReserveSeat(seatNumber))
            {
                _bookedSeats.Add(seatKey);
                Console.WriteLine($" Seat {seatNumber} booked on Schedule {_scheduleId}");
                return true;
            }

            return false;
        }

        // SINGLE RESPONSIBILITY PRINCIPLE (SRP) - Method only seat booking cancellation kore
        public bool CancelSeatBooking(int seatNumber, string userId)
        {
            string seatKey=$"{seatNumber}:{userId}";
            
            if (!_bookedSeats.Contains(seatKey))
            {
                Console.WriteLine($" Seat {seatNumber} is not booked by this user for this schedule");
                return false;
            }

            _bookedSeats.Remove(seatKey);
            _assignedBus.CancelReservation(seatNumber);
            
            Console.WriteLine($" Booking cancelled for seat {seatNumber} on Schedule {_scheduleId}");
            return true;
        }

        // POLYMORPHISM - Virtual method display er jonno
        public virtual void DisplayScheduleDetails()
        {
            string status = IsAvailable() ? "AVAILABLE" : (IsDeparturePassed() ? "DEPARTED" : "FULLY BOOKED");
            ConsoleColor statusColor = IsAvailable() ? ConsoleColor.Green : ConsoleColor.Red;
            
            Console.WriteLine($"\n┌─────────────────────────────────────────────────────────────┐");
            Console.WriteLine($"│ SCHEDULE DETAILS                                            │");
            Console.WriteLine($"├─────────────────────────────────────────────────────────────┤");
            Console.WriteLine($"│ Schedule ID:   {_scheduleId,-45}│");
            Console.WriteLine($"│ Route:         {_departureCity} → {_arrivalCity,-37}│");
            Console.WriteLine($"│ Departure:     {_departureDateTime:yyyy-MM-dd HH:mm}, {GetTimeUntilDeparture():hh\\:mm\\:ss} remaining     │");
            Console.WriteLine($"│ Ticket Price:  ${_ticketPrice,-41}│");
            Console.WriteLine($"│ Bus:           {_assignedBus.GetBusType()} ({_assignedBus.BusId}) - {_assignedBus.CoachNumber,-24}│");
            Console.WriteLine($"│ Bus Capacity:  {_assignedBus.TotalCapacity} total seats                              │");
            Console.WriteLine($"│ Seats:         {_assignedBus.AvailableSeatsCount} available | {_bookedSeats.Count} booked on this schedule│");
            Console.WriteLine($"│ Status:        ", Console.ForegroundColor = statusColor);
            Console.Write($"{status}");
            Console.ResetColor();
            Console.WriteLine($"{(status == "AVAILABLE" ? " " + new string(' ', 38) : new string(' ', 45))}│");
            Console.WriteLine($"└─────────────────────────────────────────────────────────────┘");
        }

        public override string ToString()
        {
            return $"[{_scheduleId}] {_departureCity} → {_arrivalCity} | {_departureDateTime:yyyy-MM-dd HH:mm} | ${_ticketPrice} | {_assignedBus.GetBusType()}";
        }
    }

    // INHERITANCE - Regular schedule inherits from BaseSchedule
    // LISKOV SUBSTITUTION PRINCIPLE (LSP) - Can replace BaseSchedule anywhere
    public class RegularSchedule : BaseSchedule
    {
        public RegularSchedule(string scheduleId, string departureCity, string arrivalCity,
                               DateTime departureDateTime, decimal ticketPrice, BaseBus assignedBus)
            : base(scheduleId, departureCity, arrivalCity, departureDateTime, ticketPrice, assignedBus)
        {
        }

        // POLYMORPHISM - Overriding with regular schedule specific behavior
        public override decimal GetDiscountedPrice(decimal discountPercentage)
        {
            decimal price=base.GetDiscountedPrice(discountPercentage);
            Console.WriteLine($" Regular schedule discount applied: {discountPercentage}% off");
            return price;
        }
    }

    // INHERITANCE - Express schedule inherits from BaseSchedule
    // LISKOV SUBSTITUTION PRINCIPLE (LSP) - Can replace BaseSchedule anywhere
    // OPEN/CLOSED PRINCIPLE (OCP) - Extended behavior without modifying base
    public class ExpressSchedule : BaseSchedule
    {
        public ExpressSchedule(string scheduleId, string departureCity, string arrivalCity,
                               DateTime departureDateTime, decimal ticketPrice, BaseBus assignedBus)
            : base(scheduleId, departureCity, arrivalCity, departureDateTime, ticketPrice, assignedBus)
        {
        }

        // POLYMORPHISM - Overriding with express schedule specific behavior
        public override decimal GetDiscountedPrice(decimal discountPercentage)
        {
           
            if (discountPercentage>10)
            {
                Console.WriteLine($" Express schedule discount limited to 10% (requested: {discountPercentage}%)");
                discountPercentage=10;
            }
            
            decimal price=base.GetDiscountedPrice(discountPercentage);
            Console.WriteLine($" Express schedule: {discountPercentage}% discount applied");
            return price;
        }
    }

    // DEPENDENCY INVERSION PRINCIPLE (DIP) - Factory depends on abstraction
    public interface IScheduleFactory
    {
        BaseSchedule CreateSchedule(string scheduleId, string departureCity, string arrivalCity,
                                    DateTime departureDateTime, decimal ticketPrice, 
                                    BaseBus assignedBus, string scheduleType);
    }

    public class ScheduleFactory : IScheduleFactory
    {
        public BaseSchedule CreateSchedule(string scheduleId, string departureCity, string arrivalCity,
                                           DateTime departureDateTime, decimal ticketPrice,
                                           BaseBus assignedBus, string scheduleType)
        {
            return scheduleType.ToLower() switch
            {
                "express"=>new ExpressSchedule(scheduleId, departureCity, arrivalCity, 
                                                  departureDateTime, ticketPrice, assignedBus),
                _ => new RegularSchedule(scheduleId, departureCity, arrivalCity, 
                                         departureDateTime, ticketPrice, assignedBus)
            };
        }
    }
}