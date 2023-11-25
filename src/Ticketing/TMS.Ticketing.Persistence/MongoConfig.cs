using System.ComponentModel.DataAnnotations;

namespace TMS.Ticketing.Persistence;

public class MongoConfig
{
    [Required]
    public required string ConnectionString { get; set; }

    [Required]
    public required string DatabaseName { get; set; }
}