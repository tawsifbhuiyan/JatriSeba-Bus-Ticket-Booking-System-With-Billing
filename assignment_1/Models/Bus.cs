using System;
using System.Collections.Generic;
using System.Linq;

namespace BusTicketBookingSystem
{
    // ============ Shob INTERFACES (Interface Segregation & Dependency Inversion) ============
    
   
    public interface ISeatManageable
    {
        bool ReserveSeat(int seatNumber);
        bool CancelReservation(int seatNumber);
        bool IsSeatAvailable(int seatNumber);
        int AvailableSeatsCount { get; }
        int ReservedSeatsCount { get; }
        IReadOnlySet<int> GetAvailableSeats();
        IReadOnlySet<int> GetReservedSeats();
    }
    
   
    public interface IBusInfo
    {
        string BusId { get; }
        string CoachNumber { get; }
        BusClassification Classification { get; }
        int TotalCapacity { get; }
        string GetBusInfo();
    }
    
    
    public interface IPricingStrategy
    {
        decimal GetBasePriceMultiplier();
        decimal GetSeatPrice(decimal baseFare, int seatNumber);
    }
    
    
    public interface IDisplayable
    {
        void DisplayDetails();
        void DisplaySeatingChart();
    }
    
    // ============ ABSTRACT BASE CLASS (Abstraction & Open/Closed) ============
    
        public abstract class BaseBus : ISeatManageable, IBusInfo, IPricingStrategy, IDisplayable
    {
        // Protected fields - available to derived classes (Encapsulation)
        protected string _busId;
        protected string _coachNumber;
        protected BusClassification _classification;
        protected int _totalCapacity;
        protected HashSet<int> _reservedSeats;
        protected HashSet<int> _availableSeats;
        
        
        protected BaseBus(string busId, string coachNumber, BusClassification classification)
        {
            _busId = busId;
            _coachNumber = coachNumber;
            _classification = classification;
            _totalCapacity = GetCapacityByClassification(classification);
            _reservedSeats = new HashSet<int>();
            _availableSeats = new HashSet<int>();
            InitializeAvailableSeats();
        }
        
        // Abstract method - forces derived classes to implement (Abstraction)
        protected abstract int GetCapacityByClassification(BusClassification classification);
        
        // Virtual method - can be overridden by derived classes (Polymorphism)
        protected virtual void InitializeAvailableSeats()
        {
            for (int i = 1; i <= _totalCapacity; i++)
            {
                _availableSeats.Add(i);
            }
        }
        
        // Interface implementations
        public string BusId => _busId;
        public string CoachNumber => _coachNumber;
        public BusClassification Classification => _classification;
        public int TotalCapacity => _totalCapacity;
        
        public int AvailableSeatsCount => _availableSeats.Count;
        public int ReservedSeatsCount => _reservedSeats.Count;
        
        // Virtual method - can be overridden (Polymorphism)
        public virtual bool ReserveSeat(int seatNumber)
        {
            if (seatNumber < 1 || seatNumber > _totalCapacity)
            {
                Console.WriteLine($" Seat {seatNumber} invalid. Range: 1-{_totalCapacity}");
                return false;
            }
            
            if (_reservedSeats.Contains(seatNumber))
            {
                Console.WriteLine($" Seat {seatNumber} is already reserved");
                return false;
            }
            
            if (!_availableSeats.Contains(seatNumber))
            {
                Console.WriteLine($" Seat {seatNumber} is not available");
                return false;
            }
            
            _availableSeats.Remove(seatNumber);
            _reservedSeats.Add(seatNumber);
            
            Console.WriteLine($" Seat {seatNumber} reserved on {GetBusType()} Bus {_busId}");
            return true;
        }
        
        // Virtual method - can be overridden (Polymorphism)
        public virtual bool CancelReservation(int seatNumber)
        {
            if (!_reservedSeats.Contains(seatNumber))
            {
                Console.WriteLine($" Seat {seatNumber} is not reserved");
                return false;
            }
            
            _reservedSeats.Remove(seatNumber);
            _availableSeats.Add(seatNumber);
            
            Console.WriteLine($" Reservation cancelled for seat {seatNumber}");
            return true;
        }
        
        public bool IsSeatAvailable(int seatNumber)
        {
            return seatNumber >= 1 && seatNumber <= _totalCapacity && _availableSeats.Contains(seatNumber);
        }
        
        public IReadOnlySet<int> GetAvailableSeats() => _availableSeats;
        public IReadOnlySet<int> GetReservedSeats() => _reservedSeats;
        
        // Abstract method for pricing - forces implementation (Abstraction)
        public abstract decimal GetBasePriceMultiplier();
        
        // Virtual method for seat pricing - can be overridden
        public virtual decimal GetSeatPrice(decimal baseFare, int seatNumber)
        {
            decimal multiplier = GetBasePriceMultiplier();
            decimal seatMultiplier = GetSeatPositionMultiplier(seatNumber);
            return baseFare * multiplier * seatMultiplier;
        }
        
        protected virtual decimal GetSeatPositionMultiplier(int seatNumber)
        {
            
            int seatsPerRow = 4;
            int positionInRow = ((seatNumber - 1) % seatsPerRow) + 1;
            return (positionInRow == 1 || positionInRow == seatsPerRow) ? 1.1m : 1.0m;
        }
        
        // Virtual display methods - can be overridden
        public virtual string GetBusInfo()
        {
            return $"Bus ID: {_busId}\n" +
                   $"Coach: {_coachNumber}\n" +
                   $"Type: {GetBusType()}\n" +
                   $"Class: {_classification}\n" +
                   $"Capacity: {_totalCapacity}\n" +
                   $"Available: {AvailableSeatsCount}\n" +
                   $"Reserved: {ReservedSeatsCount}";
        }
        
        public abstract string GetBusType(); 
        
        public virtual void DisplayDetails()
        {
            Console.WriteLine($"\n┌─────────────────────────────────────────┐");
            Console.WriteLine($"│ {GetBusType().ToUpper()} BUS DETAILS                      │");
            Console.WriteLine($"├─────────────────────────────────────────┤");
            Console.WriteLine($"│ {GetBusInfo().Replace("\n", "\n│ ")}");
            Console.WriteLine($"└─────────────────────────────────────────┘");
        }
        
        public virtual void DisplaySeatingChart()
        {
            Console.WriteLine($"\n╔════════════════════════════════════════════╗");
            Console.WriteLine($"║     {GetBusType()} BUS - SEATING CHART              ║");
            Console.WriteLine($"╠════════════════════════════════════════════╣");
            Console.WriteLine($"║ Bus: {_busId,-8} | Available: {AvailableSeatsCount,-3} | Reserved: {ReservedSeatsCount,-3} ║");
            Console.WriteLine($"╚════════════════════════════════════════════╝");
            
            Console.WriteLine("\nLegend: [A] Available  [R] Reserved\n");
            
            int seatsPerRow = 4;
            for (int i = 1; i <= _totalCapacity; i++)
            {
                if (_reservedSeats.Contains(i))
                {
                    Console.ForegroundColor = ConsoleColor.Red;
                    Console.Write($" S{i,2}[R] ");
                    Console.ResetColor();
                }
                else if (_availableSeats.Contains(i))
                {
                    Console.ForegroundColor = ConsoleColor.Green;
                    Console.Write($" S{i,2}[A] ");
                    Console.ResetColor();
                }
                
                if (i % seatsPerRow == 0)
                    Console.WriteLine();
            }
            Console.WriteLine();
        }
        
        public override string ToString()
        {
            return $"[{GetBusType()}] {_busId} | {_coachNumber} | {AvailableSeatsCount}/{_totalCapacity} seats available";
        }
    }
    
    // ============ CONCRETE CLASSES (Inheritance & Liskov Substitution) ============
    
   
    public class EconomyBus : BaseBus
    {
        public EconomyBus(string busId, string coachNumber) 
            : base(busId, coachNumber, BusClassification.Economy)
        {
        }
        
        protected override int GetCapacityByClassification(BusClassification classification)
        {
            return 50; // Economy: 50 seats
        }
        
        public override string GetBusType() => "Economy";
        
        public override decimal GetBasePriceMultiplier()
        {
            return 1.0m; // Base price multiplier
        }
    }
    
       public class BusinessBus : BaseBus
    {
        private bool _hasWiFi = true;
        private bool _hasChargingPorts = true;
        
        public BusinessBus(string busId, string coachNumber) 
            : base(busId, coachNumber, BusClassification.Business)
        {
        }
        
        protected override int GetCapacityByClassification(BusClassification classification)
        {
            return 35; 
        }
        
        public override string GetBusType() => "Business";
        
        public override decimal GetBasePriceMultiplier()
        {
            return 1.5m; // 50% more expensive than economy
        }
        
        
        protected override decimal GetSeatPositionMultiplier(int seatNumber)
        {
            
            int seatsPerRow = 3; 
            int positionInRow = ((seatNumber - 1) % seatsPerRow) + 1;
            return (positionInRow == 1 || positionInRow == seatsPerRow) ? 1.05m : 1.0m;
        }
        
        public override string GetBusInfo()
        {
            return base.GetBusInfo() + $"\nAmenities: WiFi, Charging Ports";
        }
        
        public override void DisplaySeatingChart()
        {
            base.DisplaySeatingChart();
            Console.WriteLine($" Business Class Amenities: WiFi | Charging Ports | Extra Legroom");
        }
    }
    
   
    public class LuxuryBus : BaseBus
    {
        private List<string> _amenities;
        
        public LuxuryBus(string busId, string coachNumber) 
            : base(busId, coachNumber, BusClassification.Luxury)
        {
            _amenities = new List<string> 
            { 
                "WiFi", "Charging Ports", "Reclining Seats", 
                "Entertainment System", "Refreshments", "AC", "Reading Lights"
            };
        }
        
        protected override int GetCapacityByClassification(BusClassification classification)
        {
            return 25; // Luxury: 25 seats (maximum comfort)
        }
        
        public override string GetBusType() => "Luxury";
        
        public override decimal GetBasePriceMultiplier()
        {
            return 2.5m; // 150% more expensive than economy
        }
        
        protected override decimal GetSeatPositionMultiplier(int seatNumber)
        {
            // Luxury: All seats are premium, minimal difference
            return 1.02m;
        }
        
        public override bool ReserveSeat(int seatNumber)
        {
            // Additional luxury-specific logic
            bool result = base.ReserveSeat(seatNumber);
            if (result)
            {
                Console.WriteLine($" Luxury perk: Complimentary snack included for seat {seatNumber}");
            }
            return result;
        }
        
        public override string GetBusInfo()
        {
            return base.GetBusInfo() + $"\nAmenities: {string.Join(", ", _amenities)}";
        }
        
        public override void DisplayDetails()
        {
            base.DisplayDetails();
            Console.WriteLine($"\n LUXURY AMENITIES:");
            foreach (var amenity in _amenities)
            {
                Console.WriteLine($"   • {amenity}");
            }
        }
        
        public override void DisplaySeatingChart()
        {
            base.DisplaySeatingChart();
            Console.WriteLine($" Luxury Experience: Reclining Seats | Entertainment | Refreshments");
        }
    }
    
    // ============ FACTORY PATTERN (Dependency Inversion) ============
    
    
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
    
    
    
    public enum BusClassification
    {
        Economy,
        Business,
        Luxury
    }
}