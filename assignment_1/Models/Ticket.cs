using System;
using System.Collections.Generic;
using System.Linq;

namespace BusTicketBookingSystem
{
    // INTERFACE SEGREGATION PRINCIPLE (ISP) - Focused interface for ticket data
    public interface ITicketInfo
    {
        string TicketId { get; }
        string UserId { get; }
        string ScheduleId { get; }
        int SeatNumber { get; }
        decimal PricePaid { get; }
        DateTime BookingDate { get; }
        TicketStatus Status { get; }
    }

    // INTERFACE SEGREGATION PRINCIPLE (ISP) - Focused interface for ticket operations
    public interface ITicketOperations
    {
        void MarkAsCancelled();
        void MarkAsCompleted();
        void DisplayTicketInfo();
    }

    // ENUM for ticket status
    public enum TicketStatus
    {
        Booked,
        Cancelled,
        Completed
    }

    // SINGLE RESPONSIBILITY PRINCIPLE (SRP) - Only responsible for ticket data
    // ENCAPSULATION - All fields are private with public properties
    public class Ticket : ITicketInfo, ITicketOperations
    {
        private string _ticketId;
        private string _userId;
        private string _scheduleId;
        private int _seatNumber;
        private decimal _pricePaid;
        private DateTime _bookingDate;
        private TicketStatus _status;

        public Ticket(string ticketId, string userId, string scheduleId, int seatNumber, decimal pricePaid)
        {
            TicketId = ticketId;
            UserId = userId;
            ScheduleId = scheduleId;
            SeatNumber = seatNumber;
            PricePaid = pricePaid;
            _bookingDate = DateTime.Now;
            _status = TicketStatus.Booked;
        }

        public string TicketId
        {
            get => _ticketId;
            private set
            {
                if (string.IsNullOrWhiteSpace(value))
                    throw new ArgumentException("Ticket ID cannot be empty");
                _ticketId = value;
            }
        }

        public string UserId
        {
            get => _userId;
            private set
            {
                if (string.IsNullOrWhiteSpace(value))
                    throw new ArgumentException("User ID cannot be empty");
                _userId = value;
            }
        }

        public string ScheduleId
        {
            get => _scheduleId;
            private set
            {
                if (string.IsNullOrWhiteSpace(value))
                    throw new ArgumentException("Schedule ID cannot be empty");
                _scheduleId = value;
            }
        }

        public int SeatNumber
        {
            get => _seatNumber;
            private set
            {
                if (value <= 0)
                    throw new ArgumentException("Seat number must be positive");
                _seatNumber = value;
            }
        }

        public decimal PricePaid
        {
            get => _pricePaid;
            private set
            {
                if (value <= 0)
                    throw new ArgumentException("Price must be greater than 0");
                _pricePaid = value;
            }
        }

        public DateTime BookingDate => _bookingDate;
        public TicketStatus Status => _status;

        public void MarkAsCancelled()
        {
            _status = TicketStatus.Cancelled;
        }

        public void MarkAsCompleted()
        {
            _status = TicketStatus.Completed;
        }

        public void DisplayTicketInfo()
        {
            Console.WriteLine($"\n-------------------------------------------");
            Console.WriteLine($"TICKET DETAILS");
            Console.WriteLine($"-------------------------------------------");
            Console.WriteLine($"Ticket ID:     {_ticketId}");
            Console.WriteLine($"User ID:       {_userId}");
            Console.WriteLine($"Schedule ID:   {_scheduleId}");
            Console.WriteLine($"Seat Number:   {_seatNumber}");
            Console.WriteLine($"Price Paid:    ${_pricePaid}");
            Console.WriteLine($"Booking Date:  {_bookingDate:yyyy-MM-dd HH:mm}");
            Console.WriteLine($"Status:        {_status}");
            Console.WriteLine($"-------------------------------------------");
        }

        public override string ToString()
        {
            return $"Ticket: {_ticketId} | Seat: {_seatNumber} | Price: ${_pricePaid} | Status: {_status}";
        }
    }

    // INTERFACE SEGREGATION PRINCIPLE (ISP) - Focused interface for booking service
    public interface ITicketBookingService
    {
        List<BaseSchedule> GetAvailableSchedules();
        bool IsSeatAvailableForSchedule(string scheduleId, int seatNumber);
        Ticket BookTicket(string userId, string scheduleId, int seatNumber, decimal paymentAmount);
        bool CancelTicket(string ticketId);
        Ticket GetTicket(string ticketId);
        List<Ticket> GetUserTickets(string userId);
    }

    // SINGLE RESPONSIBILITY PRINCIPLE (SRP) - Only handles ticket booking process
    // DEPENDENCY INVERSION PRINCIPLE (DIP) - Depends on abstractions (BaseSchedule, BaseUser)
    public class TicketBookingService : ITicketBookingService
    {
        private Dictionary<string, Ticket> _tickets;
        private Dictionary<string, List<string>> _userTickets;
        private Dictionary<string, HashSet<int>> _scheduleReservedSeats;
        private List<BaseSchedule> _schedules;
        private List<BaseUser> _users;
        private int _nextTicketNumber;

        public TicketBookingService(List<BaseSchedule> schedules, List<BaseUser> users)
        {
            _tickets = new Dictionary<string, Ticket>();
            _userTickets = new Dictionary<string, List<string>>();
            _scheduleReservedSeats = new Dictionary<string, HashSet<int>>();
            _schedules = schedules;
            _users = users;
            _nextTicketNumber = 1;

            // Initialize reserved seats tracking for each schedule
            foreach (var schedule in _schedules)
            {
                _scheduleReservedSeats[schedule.ScheduleId] = new HashSet<int>();
            }
        }

      
        public List<BaseSchedule> GetAvailableSchedules()
        {
            return _schedules.Where(s => s.IsAvailable()).ToList();
        }

        // REQUIREMENT: Validate seat against bus capacity
        // REQUIREMENT: Prevent duplicate reservations for same schedule
        public bool IsSeatAvailableForSchedule(string scheduleId, int seatNumber)
        {
           
            var schedule = _schedules.FirstOrDefault(s => s.ScheduleId == scheduleId);
            if (schedule == null)
            {
                Console.WriteLine($"Schedule {scheduleId} not found");
                return false;
            }

            if (seatNumber < 1 || seatNumber > schedule.AssignedBus.TotalCapacity)
            {
                Console.WriteLine($"Invalid seat number. Seat must be between 1 and {schedule.AssignedBus.TotalCapacity}");
                return false;
            }

           
            if (_scheduleReservedSeats.ContainsKey(scheduleId) && _scheduleReservedSeats[scheduleId].Contains(seatNumber))
            {
                Console.WriteLine($"Seat {seatNumber} is already reserved for schedule {scheduleId}");
                return false;
            }

           
            if (!schedule.AssignedBus.IsSeatAvailable(seatNumber))
            {
                Console.WriteLine($"Seat {seatNumber} is not available on the bus");
                return false;
            }

            return true;
        }

        // REQUIREMENT: Complete ticket reservation with payment
        // REQUIREMENT: Generate ticket and issue to user
        // REQUIREMENT: Permanently mark seat as reserved
        public Ticket BookTicket(string userId, string scheduleId, int seatNumber, decimal paymentAmount)
        {
            
            var user = _users.FirstOrDefault(u => u.UserId == userId);
            if (user == null)
            {
                Console.WriteLine($"User {userId} not found");
                return null;
            }

            
            var schedule = _schedules.FirstOrDefault(s => s.ScheduleId == scheduleId);
            if (schedule == null)
            {
                Console.WriteLine($"Schedule {scheduleId} not found");
                return null;
            }

           
            if (!schedule.IsAvailable())
            {
                Console.WriteLine($"Schedule {scheduleId} is not available");
                return null;
            }

           
            if (!IsSeatAvailableForSchedule(scheduleId, seatNumber))
            {
                return null;
            }

            
            decimal finalPrice = schedule.GetDiscountedPrice(0);
            
            
            if (paymentAmount < finalPrice)
            {
                Console.WriteLine($"Insufficient payment. Required: ${finalPrice}, Received: ${paymentAmount}");
                return null;
            }

            
            string ticketId = GenerateTicketId();

            
            Ticket newTicket = new Ticket(ticketId, userId, scheduleId, seatNumber, finalPrice);

          
            _tickets[ticketId] = newTicket;

            
            if (!_userTickets.ContainsKey(userId))
            {
                _userTickets[userId] = new List<string>();
            }
            _userTickets[userId].Add(ticketId);

            
            if (!_scheduleReservedSeats.ContainsKey(scheduleId))
            {
                _scheduleReservedSeats[scheduleId] = new HashSet<int>();
            }
            _scheduleReservedSeats[scheduleId].Add(seatNumber);

           
            bool seatReserved = schedule.AssignedBus.ReserveSeat(seatNumber);
            
            if (!seatReserved)
            {
                Console.WriteLine("Failed to reserve seat on bus");
                return null;
            }

            
            user.AddBooking(ticketId);

            Console.WriteLine($"Ticket {ticketId} generated successfully for user {user.FullName}");
            Console.WriteLine($"Seat {seatNumber} permanently marked as reserved for schedule {scheduleId}");
            Console.WriteLine($"Payment of ${paymentAmount} received. Change: ${paymentAmount - finalPrice}");

            return newTicket;
        }

        public bool CancelTicket(string ticketId)
        {
            if (!_tickets.ContainsKey(ticketId))
            {
                Console.WriteLine($"Ticket {ticketId} not found");
                return false;
            }

            Ticket ticket = _tickets[ticketId];
            
            if (ticket.Status == TicketStatus.Cancelled)
            {
                Console.WriteLine($"Ticket {ticketId} is already cancelled");
                return false;
            }

          
            var schedule = _schedules.FirstOrDefault(s => s.ScheduleId == ticket.ScheduleId);
            if (schedule != null)
            {
                
                if (_scheduleReservedSeats.ContainsKey(ticket.ScheduleId))
                {
                    _scheduleReservedSeats[ticket.ScheduleId].Remove(ticket.SeatNumber);
                }
                
                
                schedule.AssignedBus.CancelReservation(ticket.SeatNumber);
            }

            ticket.MarkAsCancelled();
            Console.WriteLine($"Ticket {ticketId} cancelled successfully");
            return true;
        }

        public Ticket GetTicket(string ticketId)
        {
            if (_tickets.ContainsKey(ticketId))
                return _tickets[ticketId];
            
            Console.WriteLine($"Ticket {ticketId} not found");
            return null;
        }

        public List<Ticket> GetUserTickets(string userId)
        {
            List<Ticket> userTickets = new List<Ticket>();
            
            if (_userTickets.ContainsKey(userId))
            {
                foreach (string ticketId in _userTickets[userId])
                {
                    if (_tickets.ContainsKey(ticketId))
                    {
                        userTickets.Add(_tickets[ticketId]);
                    }
                }
            }
            
            return userTickets;
        }

        public HashSet<int> GetReservedSeatsForSchedule(string scheduleId)
        {
            if (_scheduleReservedSeats.ContainsKey(scheduleId))
                return _scheduleReservedSeats[scheduleId];
            
            return new HashSet<int>();
        }

        private string GenerateTicketId()
        {
            return $"TKT_{DateTime.Now:yyyyMMdd}_{_nextTicketNumber++:D4}";
        }
    }
}