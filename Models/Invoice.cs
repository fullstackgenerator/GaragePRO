namespace GaragePRO.Models;

public class Invoice
{
    public int Id { get; set; }
    public string InvoiceNumber { get; set; }

    public int WorkOrderId { get; set; }
    public WorkOrder WorkOrder { get; set; }

    public decimal TaxAmount { get; set; }
    public decimal SubTotal { get; set; }
    public decimal Total { get; set; }
    public decimal AmountDue { get; set; }
    public decimal AmountPaid { get; set; }
    public decimal AmountReturned { get; set; }

    public PaymentType PaymentType { get; set; }
    public InvoiceStatus Status { get; set; }

    public DateTime DateIssued { get; set; }
    public DateTime? DatePaid { get; set; }
    
    public ICollection<PartUsed> PartCatalog { get; set; }
    public ICollection<ServiceDetail>? ServiceDetails { get; set; }
    public ICollection<PartUsed>? PartsUsed { get; set; }
    public ICollection<Customer>? Customers { get; set; }
    public ICollection<Vehicle>? Vehicles { get; set; }
    public ICollection<Invoice>? Invoices { get; set; }
}

public enum PaymentType
{
    Cash,
    CreditCard,
    DebitCard
}

public enum InvoiceStatus
{
    Paid,
    Unpaid
}