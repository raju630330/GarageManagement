using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace GarageManagement.Server.Model
{
    
        public class TechnicianMC
        {
            [Key]
            [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
            public int Id { get; set; }

            [MaxLength(500)]
            public string? Remarks { get; set; }

            [MaxLength(100)]
            public string? TechnicianSign { get; set; }

            [MaxLength(100)]
            public string? DriverSign { get; set; }

            [MaxLength(100)]
            public string? FloorSign { get; set; }

            [MaxLength(100)]
            public string? AuthSign { get; set; }

            // Navigation property
            public ICollection<CheckItemEntity>? CheckList { get; set; }
        }

        public class CheckItemEntity
        {
            [Key]
            [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
            public int Id { get; set; }

            [ForeignKey("TechnicianMC")]
            public int TechnicianMCId { get; set; }

            [MaxLength(150)]
            public string? Label { get; set; }

            [MaxLength(100)]
            public string? Control { get; set; }

            [MaxLength(50)]
            public string? Status { get; set; }

            public TechnicianMC? TechnicianMC { get; set; }
        }
  }