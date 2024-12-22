using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace ProductShipping.Models;

public partial class CourierCharge
{
    [Key]
    public int Id { get; set; }

    public int? MinWeight { get; set; }

    public int? MaxWeight { get; set; }

    [Column(TypeName = "decimal(18, 2)")]
    public decimal? Charge { get; set; }
}
