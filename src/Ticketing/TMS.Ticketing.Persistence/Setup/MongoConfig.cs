using System.ComponentModel.DataAnnotations;

namespace TMS.Ticketing.Persistence.Setup;

public class MongoConfig
{
    [Required]
    public required string ConnectionString { get; set; }
    
    [Required]
    public required string DatabaseName { get; set; }
}