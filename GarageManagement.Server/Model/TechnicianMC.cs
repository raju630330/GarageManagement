using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace GarageManagement.Server.Model
{
    
    public class TechnicianMC : BaseEntity
    {
            public string Remarks { get; set; } = string.Empty;
            public string TechnicianSign { get; set; } = string.Empty;
            public string DriverSign { get; set; } = string.Empty;
            public string FloorSign { get; set; } = string.Empty;
            public string AuthSign { get; set; } = string.Empty;
            public ICollection<CheckItemEntity>? CheckList { get; set; }
            public long RepairOrderId { get; set; }
            public RepairOrder? RepairOrder { get; set; }
    }

    public class CheckItemEntity : BaseEntity
    {

            [ForeignKey("TechnicianMC")]
            public long TechnicianMCId { get; set; }
            public string Label { get; set; } = string.Empty;
            public string Control { get; set; } = string.Empty;
            public string Status { get; set; } = string.Empty;
            public TechnicianMC? TechnicianMC { get; set; }
    }
  }