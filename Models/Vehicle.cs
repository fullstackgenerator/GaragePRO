namespace GaragePRO.Models;

public class Vehicle
{
    public int Id { get; set; }

    public int customerId { get; set; }
    public Customer customer { get; set; }

    public string make { get; set; }
    public string model { get; set; }
    public int year { get; set; }

    public string VIN { get; set; }  // needs to be unique
    public int mileage { get; set; }

    public ICollection<WorkOrder> WorkOrders { get; set; }
}