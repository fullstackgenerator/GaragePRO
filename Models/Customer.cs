using System.ComponentModel.DataAnnotations;

namespace GaragePRO.Models;

public class Customer
{
    public int Id { get; set; }
    [Required]
    [StringLength(100)]
    public string FullName { get; set; }
    public string Address { get; set; }
    public string City { get; set; }
    public int PostalCode { get; set; }
    [Phone]
    public string Phone { get; set; }
    [EmailAddress]
    public string Email { get; set; }

    public ICollection<Vehicle> Vehicles { get; set; } = new List<Vehicle>();
}