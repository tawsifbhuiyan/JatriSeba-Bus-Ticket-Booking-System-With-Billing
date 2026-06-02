using System;
using System.Collections.Generic;
using System.Linq;

namespace BusTicketBookingSystem
{
    // INTERFACE SEGREGATION PRINCIPLE (ISP) - Focused interface invoice data er presentation
    public interface IInvoiceInfo
    {
        string InvoiceId { get; }
        string TicketId { get; }
        string UserId { get; }
        decimal AmountDue { get; }
        DateTime GenerationDate { get; }
        PaymentStatus Status { get; }
    }

    // INTERFACE SEGREGATION PRINCIPLE (ISP) again - Focused interface for invoice operations
    public interface IInvoiceOperations
    {
        void MarkAsPaid();
        void DisplayInvoice();
    }

   
    public enum PaymentStatus
    {
        Unpaid,
        Paid,
        PartiallyPaid
    }

    // SINGLE RESPONSIBILITY PRINCIPLE (SRP) - Shudhu invoice data and operations handle kore
    // ENCAPSULATION - All fields are private with public properties
    public class Invoice : IInvoiceInfo, IInvoiceOperations
    {
        private string _invoiceId;
        private string _ticketId;
        private string _userId;
        private decimal _amountDue;
        private DateTime _generationDate;
        private PaymentStatus _status;

        public Invoice(string invoiceId, string ticketId, string userId, decimal amountDue)
        {
            InvoiceId=invoiceId;
            TicketId=ticketId;
            UserId=userId;
            AmountDue=amountDue;
            _generationDate=DateTime.Now;
            _status=PaymentStatus.Unpaid;
        }

        public string InvoiceId
        {
            get=>_invoiceId;
            private set
            {
                if(string.IsNullOrWhiteSpace(value))
                    throw new ArgumentException("Invoice ID cannot be empty");
                _invoiceId=value;
            }
        }

        public string TicketId
        {
            get=>_ticketId;
            private set
            {
                if(string.IsNullOrWhiteSpace(value))
                    throw new ArgumentException("Ticket ID cannot be empty");
                _ticketId=value;
            }
        }

        public string UserId
        {
            get=>_userId;
            private set
            {
                if(string.IsNullOrWhiteSpace(value))
                    throw new ArgumentException("User ID cannot be empty");
                _userId=value;
            }
        }

        public decimal AmountDue
        {
            get=>_amountDue;
            private set
            {
                if(value<=0)
                    throw new ArgumentException("Amount due must be greater than 0");
                _amountDue=value;
            }
        }

        public DateTime GenerationDate=>_generationDate;
        public PaymentStatus Status=>_status;

        public void MarkAsPaid()
        {
            _status=PaymentStatus.Paid;
        }

        public void MarkAsPartiallyPaid(decimal amountPaid)
        {
            if (amountPaid>0&&amountPaid<_amountDue)
            {
                _status=PaymentStatus.PartiallyPaid;
                _amountDue-=amountPaid;
            }
        }

        public void DisplayInvoice()
        {
            string statusText=_status==PaymentStatus.Paid ? "PAID" : (_status == PaymentStatus.PartiallyPaid ? "PARTIALLY PAID" : "UNPAID");
            ConsoleColor statusColor= _status==PaymentStatus.Paid ? ConsoleColor.Green : ConsoleColor.Red;
            
            Console.WriteLine($"\n-------------------------------------------");
            Console.WriteLine($"INVOICE DETAILS");
            Console.WriteLine($"-------------------------------------------");
            Console.WriteLine($"Invoice ID:     {_invoiceId}");
            Console.WriteLine($"Ticket ID:      {_ticketId}");
            Console.WriteLine($"User ID:        {_userId}");
            Console.WriteLine($"Amount Due:     ${_amountDue}");
            Console.WriteLine($"Generation Date:{_generationDate:yyyy-MM-dd HH:mm}");
            Console.Write($"Payment Status: ");
            Console.ForegroundColor = statusColor;
            Console.WriteLine($"{statusText}");
            Console.ResetColor();
            Console.WriteLine($"-------------------------------------------");
        }

        public override string ToString()
        {
            return $"Invoice: {_invoiceId} | Ticket: {_ticketId} | Amount: ${_amountDue} | Status: {_status}";
        }
    }

    // INTERFACE SEGREGATION PRINCIPLE (ISP) - Focused interface for payment service
    public interface IPaymentService
    {
        Invoice GenerateInvoiceForTicket(Ticket ticket);
        Invoice GetInvoice(string invoiceId);
        List<Invoice> GetUserInvoices(string userId);
        List<Invoice> GetOutstandingInvoices(string userId);
        bool MakePayment(string invoiceId, decimal amount);
        bool MakeFullPayment(string invoiceId);
    }

    // SINGLE RESPONSIBILITY PRINCIPLE (SRP) - Shudhu invoice generation, retrieval, and payment handling kore
    // DEPENDENCY INVERSION PRINCIPLE (DIP) - Abstraction er upor depend kore, na je concrete implementation er upor
    public class PaymentService : IPaymentService
    {
        private Dictionary<string, Invoice> _invoices;
        private Dictionary<string, List<string>> _userInvoices;
        private Dictionary<string, string> _ticketToInvoice;
        private int _nextInvoiceNumber;

        public PaymentService()
        {
            _invoices=new Dictionary<string, Invoice>();
            _userInvoices=new Dictionary<string, List<string>>();
            _ticketToInvoice=new Dictionary<string, string>();
            _nextInvoiceNumber=1;
        }

        // REQUIREMENT: Every confirmed ticket booking must automatically generate an invoice
        public Invoice GenerateInvoiceForTicket(Ticket ticket)
        {
            if (ticket==null)
            {
                Console.WriteLine("Cannot generate invoice for null ticket");
                return null;
            }

           
            if (_ticketToInvoice.ContainsKey(ticket.TicketId))
            {
                string existingInvoiceId=_ticketToInvoice[ticket.TicketId];
                Console.WriteLine($"Invoice already exists for ticket {ticket.TicketId}. Invoice ID: {existingInvoiceId}");
                return _invoices[existingInvoiceId];
            }

          
            string invoiceId=GenerateInvoiceId();

          
            Invoice newInvoice=new Invoice(invoiceId, ticket.TicketId, ticket.UserId, ticket.PricePaid);

           
            _invoices[invoiceId]=newInvoice;

            
            _ticketToInvoice[ticket.TicketId]=invoiceId;

          
            if (!_userInvoices.ContainsKey(ticket.UserId))
            {
                _userInvoices[ticket.UserId]=new List<string>();
            }
            _userInvoices[ticket.UserId].Add(invoiceId);

            Console.WriteLine($"Invoice {invoiceId} generated automatically for ticket {ticket.TicketId}");
            return newInvoice;
        }

        public Invoice GetInvoice(string invoiceId)
        {
            if (_invoices.ContainsKey(invoiceId))
                return _invoices[invoiceId];
            
            Console.WriteLine($"Invoice {invoiceId} not found");
            return null;
        }

        // REQUIREMENT: Retrieve and view all generated invoices for a user
        public List<Invoice>GetUserInvoices(string userId)
        {
            List<Invoice> userInvoices=new List<Invoice>();
            
            if (_userInvoices.ContainsKey(userId))
            {
                foreach (string invoiceId in _userInvoices[userId])
                {
                    if (_invoices.ContainsKey(invoiceId))
                    {
                        userInvoices.Add(_invoices[invoiceId]);
                    }
                }
            }
            
            return userInvoices;
        }

        // REQUIREMENT: Get outstanding (unpaid) invoices
        public List<Invoice>GetOutstandingInvoices(string userId)
        {
            List<Invoice>allUserInvoices=GetUserInvoices(userId);
            return allUserInvoices.Where(i=>i.Status!=PaymentStatus.Paid).ToList();
        }

        // REQUIREMENT: Submit payment for any outstanding invoice
        public bool MakePayment(string invoiceId, decimal amount)
        {
            if (!_invoices.ContainsKey(invoiceId))
            {
                Console.WriteLine($"Invoice {invoiceId} not found");
                return false;
            }

            Invoice invoice=_invoices[invoiceId];

            if (invoice.Status==PaymentStatus.Paid)
            {
                Console.WriteLine($"Invoice {invoiceId} is already fully paid");
                return false;
            }

            if (amount<=0)
            {
                Console.WriteLine("Payment amount must be greater than 0");
                return false;
            }

            decimal remainingAmount=invoice.AmountDue;

            if (amount>=remainingAmount)
            {
                
                invoice.MarkAsPaid();
                Console.WriteLine($"Full payment of ${remainingAmount} received for invoice {invoiceId}");
                Console.WriteLine($"Invoice {invoiceId} is now PAID");
            }
            else
            {
                
                invoice.MarkAsPartiallyPaid(amount);
                Console.WriteLine($"Partial payment of ${amount} received for invoice {invoiceId}");
                Console.WriteLine($"Remaining amount due: ${invoice.AmountDue}");
            }

            return true;
        }

       
        public bool MakeFullPayment(string invoiceId)
        {
            if (!_invoices.ContainsKey(invoiceId))
            {
                Console.WriteLine($"Invoice {invoiceId} not found");
                return false;
            }

            Invoice invoice=_invoices[invoiceId];
            return MakePayment(invoiceId, invoice.AmountDue);
        }

        private string GenerateInvoiceId()
        {
            return $"INV_{DateTime.Now:yyyyMMdd}_{_nextInvoiceNumber++:D4}";
        }
    }
}