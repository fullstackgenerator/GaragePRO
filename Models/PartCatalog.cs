namespace GaragePRO.Models;

public class PartCatalog
{
    public int Id { get; set; }
    public string PartName { get; set; }
    public string PartNumber { get; set; }
    public decimal PartPrice { get; set; }

    public ICollection<PartUsed> PartsUsed { get; set; }
}