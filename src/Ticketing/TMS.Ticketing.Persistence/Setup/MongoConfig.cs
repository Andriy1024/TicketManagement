using System.ComponentModel.DataAnnotations;

namespace TMS.Ticketing.Persistence.Setup;

public class MongoConfig
{
    [Required]
    public string ConnectionString { get; set; }
    
    [Required]
    public string DatabaseName { get; set; }

    [Required]
    public string[] Collections { get; set; }
}