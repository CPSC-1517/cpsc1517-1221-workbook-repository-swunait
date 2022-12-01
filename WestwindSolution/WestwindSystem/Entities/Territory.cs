using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace WestwindSystem.Entities;

public partial class Territory
{
    [Key]
    [Column("TerritoryID")]
    [StringLength(20)]
    [Required(ErrorMessage = "TerritoryId is required")]
    public string TerritoryId { get; set; } = null!;

    [StringLength(50)]
    [Required(ErrorMessage = "Territory Description is required")]
    public string TerritoryDescription { get; set; } = null!;

    [Column("RegionID")]
    [Required(ErrorMessage = "RegionId is required")]
    public int RegionId { get; set; }

    [ForeignKey("RegionId")]
    [InverseProperty("Territories")]
    public virtual Region Region { get; set; } = null!;
}
