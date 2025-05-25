using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace GaragePRO.Models;

public class Vehicle
{
   public int Id { get; set; }

   public int CustomerId { get; set; }

   [ForeignKey("CustomerId")]
   public Customer? Customer { get; set; }


    [Required]
    [StringLength(50)]
    public string Make { get; set; }

    [Required]
    [StringLength(50)]
    public string Model { get; set; }

    [Required]
    [Range(1900, 2100)]
    public int Year { get; set; }

    [Required]
    [StringLength(17, MinimumLength = 17)]
    public string VIN { get; set; }  // needs to be unique

    [Required]
    [Range(0, int.MaxValue)]
    public int Mileage { get; set; }

    public ICollection<WorkOrder>? WorkOrders { get; set; }

}