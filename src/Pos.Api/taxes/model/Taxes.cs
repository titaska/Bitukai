using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Pos.Api.taxes.model;

[Table("taxes")]
public class Taxes
{
    [Key]
    [Column("id")]
    private Guid _id { get; set; }
    
    [Column("name")]
    private string _name { get; set; }
    
    [Column("description")]
    private string _description { get; set; }
    
    [Column("percentage")]
    private decimal _percentage { get; set; }
}
