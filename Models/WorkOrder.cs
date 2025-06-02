namespace GaragePRO.Models;

public class WorkOrder
{
    public int Id { get; set; }

    public int VehicleId { get; set; }
    public Vehicle? Vehicle { get; set; }

    public int MechanicId { get; set; }
    public Mechanic? Mechanic { get; set; }

    public DateTime DateIn { get; set; }
    public DateTime? DateOut { get; set; }
    public string? Notes { get; set; }

    public WorkOrderStatus Status { get; set; }
    public DateTime CreatedAt { get; set; }

    public ICollection<ServiceDetail>? ServiceDetails { get; set; }
    public ICollection<PartUsed>? PartsUsed { get; set; }
    public Invoice? Invoice { get; set; }
}

public enum WorkOrderStatus
{
    Pending,
    Scheduled,
    InProgress,
    Completed,
    Cancelled,
    Archived
}