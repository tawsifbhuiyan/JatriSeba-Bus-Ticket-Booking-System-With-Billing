using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;

namespace BusTicketBookingSystem
{
    // ============ INTERFACE SEGREGATION PRINCIPLE (ISP) ============
   
    public interface IUserData
    {
        string UserId { get; }
        string FullName { get; set; }
        string MobileNumber { get; set; }
        string Email { get; set; }
    }

    
    public interface IBookable
    {
        void AddBooking(string ticketId);
        bool CancelBooking(string ticketId);
        IReadOnlyList<string> GetActiveBookings();
        int TotalBookings { get; }
        int ActiveBookings { get; }
    }

    
    public interface IUserDisplay
    {
        void DisplayUserInfo();
        void DisplayAllBookings();
    }

    
    /// ABSTRACTION: Abstract base class hiding implementation details
    
    public abstract class BaseUser : IUserData, IBookable, IUserDisplay
    {
        // ENCAPSULATION: Private/protected fields hidden from outside
        protected string _userId;
        protected string _fullName;
        protected string _mobileNumber;
        protected string _email;
        protected List<BookingRecord> _bookingRecords;

        protected BaseUser(string fullName, string mobileNumber, string email)
        {
            _userId = GenerateUserId();
            _fullName = fullName;
            _mobileNumber = mobileNumber;
            _email = email;
            _bookingRecords = new List<BookingRecord>(); 
        }

        // ABSTRACTION: Abstract method - derived classes must implement
        protected abstract string GenerateUserId();

        // ENCAPSULATION: Public properties with validation logic
        public string UserId => _userId;
        
        public string FullName
        {
            get => _fullName;
            set
            {
                if (string.IsNullOrWhiteSpace(value))
                    throw new ArgumentException("Full name cannot be empty");
                _fullName = value;
            }
        }
        
        public string MobileNumber
        {
            get => _mobileNumber;
            set
            {
                if (string.IsNullOrWhiteSpace(value))
                    throw new ArgumentException("Mobile number cannot be empty");
                if (!Regex.IsMatch(value, @"^\d{11}$"))
                    throw new ArgumentException("Mobile number must be 11 digits");
                _mobileNumber = value;
            }
        }
        
        public string Email
        {
            get => _email;
            set
            {
                if (string.IsNullOrWhiteSpace(value))
                    throw new ArgumentException("Email cannot be empty");
                if (!value.Contains("@") || !value.Contains("."))
                    throw new ArgumentException("Invalid email format");
                _email = value;
            }
        }

        
        public int TotalBookings => _bookingRecords.Count;
        public int ActiveBookings => _bookingRecords.Count(b => !b.IsCancelled);

        
        /// POLYMORPHISM: Virtual method - can be overridden by derived classes
        /// SINGLE RESPONSIBILITY PRINCIPLE (SRP): This method only handles adding bookings
      
        public virtual void AddBooking(string ticketId)
        {
            
            if (string.IsNullOrWhiteSpace(ticketId))
                throw new ArgumentException("Ticket ID cannot be empty");
            
           
            if (_bookingRecords.Any(b => b.TicketId == ticketId && !b.IsCancelled))
                throw new InvalidOperationException($"Ticket {ticketId} is already booked by this user");
            
           
            _bookingRecords.Add(new BookingRecord(ticketId, DateTime.Now, false));
            OnBookingAdded(ticketId);
        }

       
        /// POLYMORPHISM: Virtual method - can be overridden by derived classes
      
        public virtual bool CancelBooking(string ticketId)
        {
            var booking = _bookingRecords.FirstOrDefault(b => b.TicketId == ticketId && !b.IsCancelled);
            
            if (booking == null)
                return false;
            
            booking.IsCancelled = true;
            booking.CancelledDate = DateTime.Now;
            OnBookingCancelled(ticketId);
            return true;
        }

      
        public IReadOnlyList<string> GetActiveBookings()
        {
            return _bookingRecords
                .Where(b => !b.IsCancelled)
                .Select(b => b.TicketId)
                .ToList()
                .AsReadOnly();
        }

       
        
        /// OPEN/CLOSED PRINCIPLE (OCP): Extension points without modifying base class
               protected virtual void OnBookingAdded(string ticketId) { }
        protected virtual void OnBookingCancelled(string ticketId) { }

       
        /// POLYMORPHISM: Virtual method for display
        
        public virtual void DisplayUserInfo()
        {
            Console.WriteLine($"┌─────────────────────────────────────────┐");
            Console.WriteLine($"│ USER INFORMATION                         │");
            Console.WriteLine($"├─────────────────────────────────────────┤");
            Console.WriteLine($"│ User ID:     {_userId,-33}│");
            Console.WriteLine($"│ Name:        {_fullName,-33}│");
            Console.WriteLine($"│ Mobile:      {_mobileNumber,-33}│");
            Console.WriteLine($"│ Email:       {_email,-33}│");
            Console.WriteLine($"│ Total Books: {TotalBookings,-33}│");
            Console.WriteLine($"│ Active Books:{ActiveBookings,-33}│");
            Console.WriteLine($"└─────────────────────────────────────────┘");
        }

        
        public virtual void DisplayAllBookings()
        {
            if (_bookingRecords.Count == 0)
            {
                Console.WriteLine("No bookings found for this user");
                return;
            }

            Console.WriteLine($"\n╔════════════════════════════════════════════╗");
            Console.WriteLine($"║     BOOKING HISTORY - {_fullName.ToUpper()}              ║");
            Console.WriteLine($"╠════════════════════════════════════════════╣");
            Console.WriteLine($"║ Total: {TotalBookings,-3} | Active: {ActiveBookings,-3} | Cancelled: {TotalBookings - ActiveBookings,-3} ║");
            Console.WriteLine($"╚════════════════════════════════════════════╝\n");

            foreach (var booking in _bookingRecords)
            {
                string status = booking.IsCancelled ? "CANCELLED" : "ACTIVE";
                
                if (booking.IsCancelled)
                    Console.ForegroundColor = ConsoleColor.Red;
                else
                    Console.ForegroundColor = ConsoleColor.Green;
                    
                Console.WriteLine($"  [{status}] Ticket: {booking.TicketId}");
                Console.ResetColor();
                Console.WriteLine($"          Booked: {booking.BookingDate:yyyy-MM-dd HH:mm}");
                
                if (booking.IsCancelled && booking.CancelledDate.HasValue)
                {
                    Console.WriteLine($"          Cancelled: {booking.CancelledDate.Value:yyyy-MM-dd HH:mm}");
                }
                Console.WriteLine();
            }
        }

        public override string ToString()
        {
            return $"User: {_fullName} (ID: {_userId}) | Active Bookings: {ActiveBookings}/{TotalBookings}";
        }
    }

    
    /// INHERITANCE: RegularUser inherits from BaseUser
    /// LISKOV SUBSTITUTION PRINCIPLE (LSP): Can replace BaseUser without breaking functionality
    
    public class RegularUser : BaseUser
    {
        public RegularUser(string fullName, string mobileNumber, string email) 
            : base(fullName, mobileNumber, email)
        {
        }

        protected override string GenerateUserId()
        {
            return $"USR_{Guid.NewGuid().ToString().Substring(0, 8).ToUpper()}";
        }

        // POLYMORPHISM: Overriding base behavior
        protected override void OnBookingAdded(string ticketId)
        {
            Console.WriteLine($" Booking confirmed for regular user {FullName}");
        }
    }

    
    /// INHERITANCE: PremiumUser inherits from BaseUser
    /// LISKOV SUBSTITUTION PRINCIPLE (LSP): Can replace BaseUser without breaking functionality
    /// OPEN/CLOSED PRINCIPLE (OCP): Extended behavior without modifying BaseUser
    
    public class PremiumUser : BaseUser
    {
        private List<string> _discountCodes;
        private int _loyaltyPoints;

        public PremiumUser(string fullName, string mobileNumber, string email) 
            : base(fullName, mobileNumber, email)
        {
            _discountCodes = new List<string>();
            _loyaltyPoints = 0;
        }

        public int LoyaltyPoints => _loyaltyPoints;

        protected override string GenerateUserId()
        {
            return $"PRM_{Guid.NewGuid().ToString().Substring(0, 8).ToUpper()}";
        }

        // POLYMORPHISM: Overriding with premium-specific behavior
        protected override void OnBookingAdded(string ticketId)
        {
            _loyaltyPoints += 10;
            Console.WriteLine($" Premium user {FullName} earned 10 loyalty points! Total: {_loyaltyPoints}");
        }

        protected override void OnBookingCancelled(string ticketId)
        {
            _loyaltyPoints -= 5;
            Console.WriteLine($" Premium user {FullName} lost 5 loyalty points. Total: {_loyaltyPoints}");
        }

        public void AddDiscountCode(string code)
        {
            _discountCodes.Add(code);
            Console.WriteLine($" Discount code {code} added for premium user");
        }

        public IReadOnlyList<string> GetDiscountCodes() => _discountCodes.AsReadOnly();

        // POLYMORPHISM: Overriding display with additional info
        public override void DisplayUserInfo()
        {
            base.DisplayUserInfo();
            Console.WriteLine($"│ Loyalty Pts:  {_loyaltyPoints,-33}│");
            Console.WriteLine($"│ Discounts:    {_discountCodes.Count,-33}│");
        }
    }

   
    /// ENCAPSULATION: Inner class to track individual booking details
    /// SINGLE RESPONSIBILITY PRINCIPLE (SRP): Only responsible for booking record data
    
    public class BookingRecord
    {
        public string TicketId { get; set; }
        public DateTime BookingDate { get; set; }
        public bool IsCancelled { get; set; }
        public DateTime? CancelledDate { get; set; }

        public BookingRecord(string ticketId, DateTime bookingDate, bool isCancelled)
        {
            TicketId = ticketId;
            BookingDate = bookingDate;
            IsCancelled = isCancelled;
            CancelledDate = null;
        }
    }

   
    /// DEPENDENCY INVERSION PRINCIPLE (DIP): Factory depends on abstraction (IUserFactory)
   
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
}