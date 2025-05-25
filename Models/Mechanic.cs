using System.ComponentModel.DataAnnotations;

namespace GaragePRO.Models;

public class Mechanic
{
        public int Id { get; set; }
        [Required]
        [StringLength(50)]
        public string FullName { get; set; }
        [Required]
        [StringLength(50)]
        public string AssignedVehicleBrand { get; set; }
        [Required]
        [StringLength(30)]
        public string Seniority { get; set; }
        [Required]
        [Range(1900, 2100, ErrorMessage = "Enter a valid year.")]
        public int EmploymentStartYear { get; set; }

        [Required]
        public Status Status { get; set; }

        [Required]
        [StringLength(30)]
        [Phone]
        public string Phone { get; set; }

        public ICollection<WorkOrder> WorkOrders { get; set; } = new List<WorkOrder>();
    }

public enum Status {
    Active,
    Absent,
    Inactive
}