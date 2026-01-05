using System;
using System.Collections.Generic;

namespace WebApplication3.Models;

public partial class PropertyType
{
    public int PropertyTypeId { get; set; }

    public string TypeName { get; set; } = null!;

    public DateTime? CreatedAt { get; set; }

    public DateTime? UpdatedAt { get; set; }

    public virtual ICollection<ProjectAreaPropertyType> ProjectAreaPropertyTypes { get; set; } = new List<ProjectAreaPropertyType>();
}
