namespace GaragePRO.Models;

public class Mechanic
{
        public int Id { get; set; }
        public string fullName { get; set; }
        public string phone { get; set; }
        public DateTime createdAt { get; set; }

        public ICollection<WorkOrder> WorkOrders { get; set; }
    }
    