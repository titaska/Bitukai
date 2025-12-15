using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Pos.Api.taxes.model;

[Table("taxes")]
public class Tax
{
    [Key]
    [Column("id")]
    public string id { get; set; } = Guid.NewGuid().ToString();

    [Required]
    [Column("name")]
    public string name { get; set; } = string.Empty;

    [Column("description")]
    public string? description { get; set; }

    [Column("percentage")]
    public decimal percentage { get; set; }
}
