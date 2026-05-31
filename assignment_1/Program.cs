 using System;
using System.Collections.Generic;
using System.Linq;

namespace BusTicketBookingSystem
{
    class Program
    {
        private static List<BaseUser> _users = new List<BaseUser>();
        private static List<BaseBus> _buses = new List<BaseBus>();
        private static List<BaseSchedule> _schedules = new List<BaseSchedule>();
        private static ITicketBookingService _bookingService;
        private static IPaymentService _paymentService;
        private static IUserFactory _userFactory;
        private static IBusFactory _busFactory;
        private static IScheduleFactory _scheduleFactory;

        static void Main(string[] args)
        {
            Console.Title = "Bus Ticket Booking System";
            Console.ForegroundColor = ConsoleColor.Cyan;
            Console.WriteLine("==================================================");
            Console.WriteLine("       BUS TICKET BOOKING SYSTEM");
            Console.WriteLine("==================================================");
            Console.ResetColor();

            // Initialize factories and services
            _userFactory = new UserFactory();
            _busFactory = new BusFactory();
            _scheduleFactory = new ScheduleFactory();
            _paymentService = new PaymentService();

            bool running = true;

            while (running)
            {
                DisplayMainMenu();
                string choice = Console.ReadLine();

                switch (choice)
                {
                    case "1":
                        CreateUser();
                        break;
                    case "2":
                        DisplayAllUsers();
                        break;
                    case "3":
                        CreateBus();
                        break;
                    case "4":
                        DisplayAllBuses();
                        break;
                    case "5":
                        CreateSchedule();
                        break;
                    case "6":
                        DisplayAllSchedules();
                        break;
                    case "7":
                        DisplayScheduleDetails();
                        break;
                    case "8":
                        BookTicket();
                        break;
                    case "9":
                        DisplayUserInvoices();
                        break;
                    case "10":
                        ProcessInvoicePayment();
                        break;
                    case "11":
                        DisplayUserTickets();
                        break;
                    case "0":
                        running = false;
                        Console.WriteLine("\nThank you for using Bus Ticket Booking System");
                        break;
                    default:
                        Console.WriteLine("\nInvalid option. Please try again.");
                        break;
                }

                if (running && choice != "2" && choice != "4" && choice != "6")
                {
                    Console.WriteLine("\nPress Enter to continue...");
                    Console.ReadLine();
                }
                Console.Clear();
            }
        }

        private static void DisplayMainMenu()
        {
            Console.ForegroundColor = ConsoleColor.Yellow;
            Console.WriteLine("\n================== MAIN MENU ==================");
            Console.ResetColor();
            Console.WriteLine("1. Create User");
            Console.WriteLine("2. Display All Users");
            Console.WriteLine("3. Create Bus");
            Console.WriteLine("4. Display All Buses");
            Console.WriteLine("5. Create Schedule");
            Console.WriteLine("6. Display All Schedules");
            Console.WriteLine("7. Display Schedule Details");
            Console.WriteLine("8. Book Ticket");
            Console.WriteLine("9. Display User Invoices");
            Console.WriteLine("10. Process Invoice Payment");
            Console.WriteLine("11. Display User Tickets");
            Console.WriteLine("0. Exit");
            Console.Write("\nEnter your choice: ");
        }

        // OPERATION 1: Create User
        private static void CreateUser()
        {
            Console.Clear();
            Console.ForegroundColor = ConsoleColor.Green;
            Console.WriteLine("\n================== CREATE USER ==================");
            Console.ResetColor();

            try
            {
                Console.Write("Enter Full Name: ");
                string name = Console.ReadLine();

                Console.Write("Enter Mobile Number (11 digits): ");
                string mobile = Console.ReadLine();

                Console.Write("Enter Email Address: ");
                string email = Console.ReadLine();

                Console.Write("Enter User Type (regular/premium): ");
                string userType = Console.ReadLine();

                BaseUser newUser = _userFactory.CreateUser(name, mobile, email, userType);
                _users.Add(newUser);

                Console.ForegroundColor = ConsoleColor.Green;
                Console.WriteLine($"\nUser created successfully!");
                Console.ResetColor();
                newUser.DisplayUserInfo();
            }
            catch (Exception ex)
            {
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine($"\nError: {ex.Message}");
                Console.ResetColor();
            }
        }

        // OPERATION 2: Display All Users
        private static void DisplayAllUsers()
        {
            Console.Clear();
            Console.ForegroundColor = ConsoleColor.Green;
            Console.WriteLine("\n================== ALL USERS ==================");
            Console.ResetColor();

            if (_users.Count == 0)
            {
                Console.WriteLine("\nNo users found in the system.");
                return;
            }

            Console.WriteLine($"\nTotal Users: {_users.Count}\n");

            for (int i = 0; i < _users.Count; i++)
            {
                Console.WriteLine($"--- User #{i + 1} ---");
                _users[i].DisplayUserInfo();
                Console.WriteLine();
            }

            Console.WriteLine("Press Enter to return to main menu...");
            Console.ReadLine();
        }

        // OPERATION 3: Create Bus
        private static void CreateBus()
        {
            Console.Clear();
            Console.ForegroundColor = ConsoleColor.Green;
            Console.WriteLine("\n================== CREATE BUS ==================");
            Console.ResetColor();

            try
            {
                Console.Write("Enter Bus ID (unique): ");
                string busId = Console.ReadLine();

                Console.Write("Enter Coach Number: ");
                string coachNumber = Console.ReadLine();

                Console.WriteLine("\nBus Classifications:");
                Console.WriteLine("1. Economy (50 seats)");
                Console.WriteLine("2. Business (35 seats)");
                Console.WriteLine("3. Luxury (25 seats)");
                Console.Write("Select classification (1-3): ");
                string classChoice = Console.ReadLine();

                BusClassification classification;
                switch (classChoice)
                {
                    case "1":
                        classification = BusClassification.Economy;
                        break;
                    case "2":
                        classification = BusClassification.Business;
                        break;
                    case "3":
                        classification = BusClassification.Luxury;
                        break;
                    default:
                        throw new ArgumentException("Invalid classification");
                }

                BaseBus newBus = _busFactory.CreateBus(busId, coachNumber, classification);
                _buses.Add(newBus);

                Console.ForegroundColor = ConsoleColor.Green;
                Console.WriteLine($"\nBus created successfully!");
                Console.ResetColor();
                newBus.DisplayDetails();
            }
            catch (Exception ex)
            {
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine($"\nError: {ex.Message}");
                Console.ResetColor();
            }
        }

        // OPERATION 4: Display All Buses
        private static void DisplayAllBuses()
        {
            Console.Clear();
            Console.ForegroundColor = ConsoleColor.Green;
            Console.WriteLine("\n================== ALL BUSES ==================");
            Console.ResetColor();

            if (_buses.Count == 0)
            {
                Console.WriteLine("\nNo buses found in the fleet.");
                return;
            }

            Console.WriteLine($"\nTotal Buses: {_buses.Count}\n");

            for (int i = 0; i < _buses.Count; i++)
            {
                Console.WriteLine($"--- Bus #{i + 1} ---");
                _buses[i].DisplayDetails();
                Console.WriteLine();
            }

            Console.WriteLine("Press Enter to return to main menu...");
            Console.ReadLine();
        }

        // OPERATION 5: Create Schedule
        private static void CreateSchedule()
        {
            Console.Clear();
            Console.ForegroundColor = ConsoleColor.Green;
            Console.WriteLine("\n================== CREATE SCHEDULE ==================");
            Console.ResetColor();

            try
            {
                if (_buses.Count == 0)
                {
                    Console.WriteLine("\nNo buses available. Please create a bus first.");
                    return;
                }

                Console.Write("Enter Schedule ID: ");
                string scheduleId = Console.ReadLine();

                Console.Write("Enter Departure City: ");
                string departureCity = Console.ReadLine();

                Console.Write("Enter Arrival City: ");
                string arrivalCity = Console.ReadLine();

                Console.Write("Enter Departure Date (yyyy-mm-dd): ");
                DateTime departureDate = DateTime.Parse(Console.ReadLine());

                Console.Write("Enter Departure Time (HH:mm): ");
                TimeSpan departureTime = TimeSpan.Parse(Console.ReadLine());
                DateTime departureDateTime = departureDate.Add(departureTime);

                Console.Write("Enter Ticket Price: ");
                decimal ticketPrice = decimal.Parse(Console.ReadLine());

                Console.WriteLine("\nAvailable Buses:");
                for (int i = 0; i < _buses.Count; i++)
                {
                    Console.WriteLine($"{i + 1}. {_buses[i].GetBusType()} - {_buses[i].BusId} ({_buses[i].CoachNumber})");
                }
                Console.Write("Select bus number: ");
                int busChoice = int.Parse(Console.ReadLine()) - 1;

                if (busChoice < 0 || busChoice >= _buses.Count)
                {
                    throw new ArgumentException("Invalid bus selection");
                }

                Console.Write("Enter Schedule Type (regular/express): ");
                string scheduleType = Console.ReadLine();

                BaseSchedule newSchedule = _scheduleFactory.CreateSchedule(
                    scheduleId, departureCity, arrivalCity, departureDateTime, 
                    ticketPrice, _buses[busChoice], scheduleType);
                
                _schedules.Add(newSchedule);

                Console.ForegroundColor = ConsoleColor.Green;
                Console.WriteLine($"\nSchedule created successfully!");
                Console.ResetColor();
                newSchedule.DisplayScheduleDetails();
            }
            catch (Exception ex)
            {
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine($"\nError: {ex.Message}");
                Console.ResetColor();
            }
        }

        // OPERATION 6: Display All Schedules
        private static void DisplayAllSchedules()
        {
            Console.Clear();
            Console.ForegroundColor = ConsoleColor.Green;
            Console.WriteLine("\n================== ALL SCHEDULES ==================");
            Console.ResetColor();

            if (_schedules.Count == 0)
            {
                Console.WriteLine("\nNo schedules found in the system.");
                return;
            }

            Console.WriteLine($"\nTotal Schedules: {_schedules.Count}\n");

            for (int i = 0; i < _schedules.Count; i++)
            {
                Console.WriteLine($"--- Schedule #{i + 1} ---");
                Console.WriteLine(_schedules[i].ToString());
                Console.WriteLine();
            }

            Console.WriteLine("Press Enter to return to main menu...");
            Console.ReadLine();
        }

        // OPERATION 7: Display Schedule Details
        private static void DisplayScheduleDetails()
        {
            Console.Clear();
            Console.ForegroundColor = ConsoleColor.Green;
            Console.WriteLine("\n================== SCHEDULE DETAILS ==================");
            Console.ResetColor();

            if (_schedules.Count == 0)
            {
                Console.WriteLine("\nNo schedules found in the system.");
                return;
            }

            Console.WriteLine("\nAvailable Schedules:");
            for (int i = 0; i < _schedules.Count; i++)
            {
                Console.WriteLine($"{i + 1}. {_schedules[i].ToString()}");
            }

            Console.Write("\nSelect schedule number: ");
            if (int.TryParse(Console.ReadLine(), out int choice) && choice >= 1 && choice <= _schedules.Count)
            {
                _schedules[choice - 1].DisplayScheduleDetails();
                
                // Also show seating chart
                Console.WriteLine("\nSeating Chart for this schedule:");
                _schedules[choice - 1].AssignedBus.DisplaySeatingChart();
            }
            else
            {
                Console.WriteLine("Invalid selection.");
            }
        }

        // OPERATION 8: Book Ticket
        private static void BookTicket()
        {
            Console.Clear();
            Console.ForegroundColor = ConsoleColor.Green;
            Console.WriteLine("\n================== BOOK TICKET ==================");
            Console.ResetColor();

            try
            {
                if (_users.Count == 0)
                {
                    Console.WriteLine("\nNo users found. Please create a user first.");
                    return;
                }

                if (_schedules.Count == 0)
                {
                    Console.WriteLine("\nNo schedules found. Please create a schedule first.");
                    return;
                }

                // Initialize booking service with current data
                _bookingService = new TicketBookingService(_schedules, _users);

                // Show available schedules
                Console.WriteLine("\nAvailable Schedules:");
                var availableSchedules = _bookingService.GetAvailableSchedules();
                
                if (availableSchedules.Count == 0)
                {
                    Console.WriteLine("No available schedules found.");
                    return;
                }

                for (int i = 0; i < availableSchedules.Count; i++)
                {
                    Console.WriteLine($"{i + 1}. {availableSchedules[i].ToString()}");
                }

                Console.Write("\nSelect schedule: ");
                int scheduleChoice = int.Parse(Console.ReadLine()) - 1;
                
                if (scheduleChoice < 0 || scheduleChoice >= availableSchedules.Count)
                {
                    throw new ArgumentException("Invalid schedule selection");
                }

                BaseSchedule selectedSchedule = availableSchedules[scheduleChoice];

                // Show seating chart
                selectedSchedule.AssignedBus.DisplaySeatingChart();

                // Select user
                Console.WriteLine("\nSelect User:");
                for (int i = 0; i < _users.Count; i++)
                {
                    Console.WriteLine($"{i + 1}. {_users[i].FullName} (ID: {_users[i].UserId})");
                }
                Console.Write("Enter user number: ");
                int userChoice = int.Parse(Console.ReadLine()) - 1;

                if (userChoice < 0 || userChoice >= _users.Count)
                {
                    throw new ArgumentException("Invalid user selection");
                }

                BaseUser selectedUser = _users[userChoice];

                // Select seat
                Console.Write($"\nEnter seat number (1-{selectedSchedule.AssignedBus.TotalCapacity}): ");
                int seatNumber = int.Parse(Console.ReadLine());

                // Enter payment
                decimal finalPrice = selectedSchedule.GetDiscountedPrice(0);
                Console.WriteLine($"\nTicket Price: ${finalPrice}");
                Console.Write("Enter payment amount: ");
                decimal paymentAmount = decimal.Parse(Console.ReadLine());

                // Book ticket
                Ticket newTicket = _bookingService.BookTicket(
                    selectedUser.UserId, selectedSchedule.ScheduleId, seatNumber, paymentAmount);

                if (newTicket != null)
                {
                    Console.ForegroundColor = ConsoleColor.Green;
                    Console.WriteLine("\nTicket booked successfully!");
                    Console.ResetColor();
                    newTicket.DisplayTicketInfo();

                    // Automatically generate invoice
                    Invoice newInvoice = _paymentService.GenerateInvoiceForTicket(newTicket);
                    Console.WriteLine("\nInvoice generated automatically:");
                    newInvoice.DisplayInvoice();
                }
            }
            catch (Exception ex)
            {
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine($"\nError: {ex.Message}");
                Console.ResetColor();
            }
        }

        // OPERATION 9: Display User Invoices
        private static void DisplayUserInvoices()
        {
            Console.Clear();
            Console.ForegroundColor = ConsoleColor.Green;
            Console.WriteLine("\n================== USER INVOICES ==================");
            Console.ResetColor();

            if (_users.Count == 0)
            {
                Console.WriteLine("\nNo users found.");
                return;
            }

            Console.WriteLine("\nSelect User:");
            for (int i = 0; i < _users.Count; i++)
            {
                Console.WriteLine($"{i + 1}. {_users[i].FullName}");
            }
            Console.Write("Enter user number: ");

            if (int.TryParse(Console.ReadLine(), out int choice) && choice >= 1 && choice <= _users.Count)
            {
                BaseUser selectedUser = _users[choice - 1];
                List<Invoice> userInvoices = _paymentService.GetUserInvoices(selectedUser.UserId);

                if (userInvoices.Count == 0)
                {
                    Console.WriteLine($"\nNo invoices found for {selectedUser.FullName}");
                    return;
                }

                Console.WriteLine($"\nInvoices for {selectedUser.FullName}:");
                foreach (var invoice in userInvoices)
                {
                    invoice.DisplayInvoice();
                }
            }
            else
            {
                Console.WriteLine("Invalid selection.");
            }
        }

        // OPERATION 10: Process Invoice Payment
        private static void ProcessInvoicePayment()
        {
            Console.Clear();
            Console.ForegroundColor = ConsoleColor.Green;
            Console.WriteLine("\n================== PROCESS PAYMENT ==================");
            Console.ResetColor();

            if (_users.Count == 0)
            {
                Console.WriteLine("\nNo users found.");
                return;
            }

            Console.WriteLine("\nSelect User:");
            for (int i = 0; i < _users.Count; i++)
            {
                Console.WriteLine($"{i + 1}. {_users[i].FullName}");
            }
            Console.Write("Enter user number: ");

            if (int.TryParse(Console.ReadLine(), out int choice) && choice >= 1 && choice <= _users.Count)
            {
                BaseUser selectedUser = _users[choice - 1];
                List<Invoice> outstandingInvoices = _paymentService.GetOutstandingInvoices(selectedUser.UserId);

                if (outstandingInvoices.Count == 0)
                {
                    Console.WriteLine($"\nNo outstanding invoices for {selectedUser.FullName}");
                    return;
                }

                Console.WriteLine($"\nOutstanding Invoices for {selectedUser.FullName}:");
                for (int i = 0; i < outstandingInvoices.Count; i++)
                {
                    Console.WriteLine($"{i + 1}. Invoice {outstandingInvoices[i].InvoiceId} - Amount Due: ${outstandingInvoices[i].AmountDue}");
                }

                Console.Write("\nSelect invoice number to pay: ");
                if (int.TryParse(Console.ReadLine(), out int invoiceChoice) && invoiceChoice >= 1 && invoiceChoice <= outstandingInvoices.Count)
                {
                    Invoice selectedInvoice = outstandingInvoices[invoiceChoice - 1];
                    Console.Write($"Enter payment amount (Due: ${selectedInvoice.AmountDue}): $");
                    
                    if (decimal.TryParse(Console.ReadLine(), out decimal paymentAmount))
                    {
                        bool success = _paymentService.MakePayment(selectedInvoice.InvoiceId, paymentAmount);
                        
                        if (success)
                        {
                            Console.ForegroundColor = ConsoleColor.Green;
                            Console.WriteLine("\nPayment processed successfully!");
                            Console.ResetColor();
                            selectedInvoice.DisplayInvoice();
                        }
                    }
                    else
                    {
                        Console.WriteLine("Invalid payment amount.");
                    }
                }
            }
            else
            {
                Console.WriteLine("Invalid selection.");
            }
        }

        // OPERATION 11: Display User Tickets
        private static void DisplayUserTickets()
        {
            Console.Clear();
            Console.ForegroundColor = ConsoleColor.Green;
            Console.WriteLine("\n================== USER TICKETS ==================");
            Console.ResetColor();

            if (_users.Count == 0)
            {
                Console.WriteLine("\nNo users found.");
                return;
            }

            if (_bookingService == null)
            {
                _bookingService = new TicketBookingService(_schedules, _users);
            }

            Console.WriteLine("\nSelect User:");
            for (int i = 0; i < _users.Count; i++)
            {
                Console.WriteLine($"{i + 1}. {_users[i].FullName}");
            }
            Console.Write("Enter user number: ");

            if (int.TryParse(Console.ReadLine(), out int choice) && choice >= 1 && choice <= _users.Count)
            {
                BaseUser selectedUser = _users[choice - 1];
                List<Ticket> userTickets = _bookingService.GetUserTickets(selectedUser.UserId);

                if (userTickets.Count == 0)
                {
                    Console.WriteLine($"\nNo tickets found for {selectedUser.FullName}");
                    return;
                }

                Console.WriteLine($"\nTickets for {selectedUser.FullName}:");
                foreach (var ticket in userTickets)
                {
                    ticket.DisplayTicketInfo();
                }
            }
            else
            {
                Console.WriteLine("Invalid selection.");
            }
        }
    }
}
