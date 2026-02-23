using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace GarageManagement.Server.Model
{
    
    public class TechnicianMC : BaseEntity
    {
            public string? Remarks { get; set; }
            public string? TechnicianSign { get; set; }
            public string? DriverSign { get; set; }
            public string? FloorSign { get; set; }
            public string? AuthSign { get; set; }
            public ICollection<CheckItemEntity>? CheckList { get; set; }
            public long? RepairOrderId { get; set; }
            public RepairOrder? RepairOrder { get; set; }
    }

    public class CheckItemEntity : BaseEntity
    {

            [ForeignKey("TechnicianMC")]
            public long TechnicianMCId { get; set; }
            public string? Label { get; set; }
            public string? Control { get; set; }
            public string? Status { get; set; }
            public TechnicianMC? TechnicianMC { get; set; }
    }
  }