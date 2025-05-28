namespace GaragePRO.Data;
using GaragePRO.Models;
using Microsoft.EntityFrameworkCore;

public class DbInitializer
{
   public static void Seed(IServiceProvider serviceProvider)
{
    using var scope = serviceProvider.CreateScope();
    var context = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();
    
    Console.WriteLine("DbInitializer: Starting seed process...");
    context.Database.Migrate();
    Console.WriteLine("DbInitializer: Database migration complete.");

    if (!context.PartCatalogs.Any())
    {
        Console.WriteLine("DbInitializer: PartCatalogs table is empty. Seeding data...");
        context.PartCatalogs.AddRange(
            new PartCatalog { PartName = "Oil Filter", PartNumber = "OF123", PartPrice = 9.99M },
            new PartCatalog { PartName = "5w-30 Oil 5L", PartNumber = "5W30-5L", PartPrice = 49.99M },
            new PartCatalog { PartName = "Brake Pad", PartNumber = "BP455", PartPrice = 25.99M },
            new PartCatalog { PartName = "Brake Rotor", PartNumber = "BR455", PartPrice = 45.99M },
            new PartCatalog { PartName = "Spark Plug", PartNumber = "SP123", PartPrice = 3.49M },
            new PartCatalog { PartName = "Timing Belt", PartNumber = "TB400", PartPrice = 89.99M },
            new PartCatalog { PartName = "Timing Chain", PartNumber = "TC400", PartPrice = 129.99M },
            new PartCatalog { PartName = "Air Filter", PartNumber = "AF210", PartPrice = 12.49M },
            new PartCatalog { PartName = "Cabin Air Filter", PartNumber = "CAF215", PartPrice = 14.99M },
            new PartCatalog { PartName = "Battery", PartNumber = "BAT800", PartPrice = 119.99M },
            new PartCatalog { PartName = "Alternator", PartNumber = "ALT320", PartPrice = 229.99M },
            new PartCatalog { PartName = "Starter Motor", PartNumber = "SM450", PartPrice = 189.99M },
            new PartCatalog { PartName = "Radiator", PartNumber = "RAD300", PartPrice = 174.49M },
            new PartCatalog { PartName = "Fuel Pump", PartNumber = "FP550", PartPrice = 199.99M },
            new PartCatalog { PartName = "Water Pump", PartNumber = "WP600", PartPrice = 79.99M },
            new PartCatalog { PartName = "Drive Belt", PartNumber = "DB220", PartPrice = 34.99M },
            new PartCatalog { PartName = "Wheel Bearing", PartNumber = "WB310", PartPrice = 59.99M },
            new PartCatalog { PartName = "Headlight Bulb", PartNumber = "HB120", PartPrice = 11.99M },
            new PartCatalog { PartName = "Thermostat", PartNumber = "TH200", PartPrice = 24.99M },
            new PartCatalog { PartName = "Oxygen Sensor", PartNumber = "O2S900", PartPrice = 89.49M }

        );
        context.SaveChanges();
        Console.WriteLine($"DbInitializer: Added {context.PartCatalogs.Count()} parts to PartCatalogs.");
    }
    else
    {
        Console.WriteLine($"DbInitializer: PartCatalogs table already contains {context.PartCatalogs.Count()} items. Skipping initial seed.");
    }
    Console.WriteLine("DbInitializer: Seed process finished.");
}
}