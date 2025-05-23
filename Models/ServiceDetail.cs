namespace GaragePRO.Models;

public class ServiceDetail
{
    public int Id { get; set; }

    public int WorkOrderId { get; set; }
    public WorkOrder WorkOrder { get; set; }

    public string Description { get; set; }
    public decimal LaborHours { get; set; }
    public decimal HourlyRate { get; set; }
    public DateTime CreatedAt { get; set; }
}