using CollSys.Matm.Kitabxana.Entities.Interfaces;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace CollSys.Matm.Kitabxana.Entities.Tables
{
    [Table("Regions")]
    public class RegionModel : IEntity
    {
        [Key]
        [Required]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [Column("Id")]
        public int Id { get; set; }

        [Display(Name = "Region")]
        [Required]
        [MaxLength(200, ErrorMessage = " *Region maksimum 200 xarakterdən ibarət ola bilər! ")]
        [Column("Region")]
        public string Region { get; set; }

        // Relations with other tables
        public virtual ICollection<ExhibitModel> IstehsalExhibits { get; set; }
    }
}
