using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace GaragePRO.Models;

public class ServiceDetail
{
    public int Id { get; set; }

    public int WorkOrderId { get; set; }
    [ForeignKey("WorkOrderId")]
    public WorkOrder? WorkOrder { get; set; }

    [Required]
    [StringLength(250)]
    public string? Description { get; set; }
    [Required]
    public decimal LaborHours { get; set; }
    [Required]
    public decimal HourlyRate { get; set; }
    public DateTime CreatedAt { get; set; }
}