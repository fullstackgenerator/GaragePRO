using System.ComponentModel.DataAnnotations;

namespace GaragePRO.Models;

public class Invoice
{
    public int Id { get; set; }
    [Required]
    public string InvoiceNumber { get; set; }

    public int WorkOrderId { get; set; }
    public WorkOrder WorkOrder { get; set; }

    public decimal TaxAmount { get; set; }
    public decimal SubTotal { get; set; }
    public decimal Total { get; set; }
    public decimal AmountDue { get; set; }
    public decimal AmountPaid { get; set; }
    [Required]
    public PaymentType PaymentType { get; set; }
    [Required]
    public InvoiceStatus Status { get; set; }
    [Required]
    public DateTime DateIssued { get; set; }
    public DateTime? DatePaid { get; set; }
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