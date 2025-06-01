using System.ComponentModel.DataAnnotations;

namespace GaragePRO.Models;

public class Invoice
{
    public int Id { get; set; }
    [Required]

    public int WorkOrderId { get; set; }
    public WorkOrder? WorkOrder { get; set; }

    public decimal TaxAmount { get; set; }
    public decimal SubTotal { get; set; }
    public decimal Total { get; set; }
    
    public PaymentType? PaymentType { get; set; }
    public InvoiceStatus? Status { get; set; }
    
    [Required]
    public DateTime DateIssued { get; set; }
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
    Unpaid,
    Archived
}