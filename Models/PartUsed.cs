namespace GaragePRO.Models;

public class PartUsed
{
    public int Id { get; set; }

    public int WorkOrderId { get; set; }
    public WorkOrder WorkOrder { get; set; }

    public int PartCatalogId { get; set; }
    public PartCatalog PartCatalog { get; set; }

    public int Quantity { get; set; }
    public DateTime CreatedAt { get; set; }
}