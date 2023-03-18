using CollSys.Matm.Kitabxana.Entities.Interfaces;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace CollSys.Matm.Kitabxana.Entities.Tables
{
    [Table("MeasurementUnits")]
    public class MeasurementUnitModel : IEntity
    {
        [Key]
        [Required]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [Column("Id")]
        public int Id { get; set; }

        [Display(Name = "Ölçü vahidi")]
        [Required]
        [MaxLength(100, ErrorMessage = " *Ölçü vahidi maksimum 100 xarakterdən ibarət ola bilər! ")]
        [Column("MeasurementUnit")]
        public string MeasurementUnit { get; set; }

        // Relations with other tables
        public virtual ICollection<ExhibitModel> EnExhibits { get; set; }
        public virtual ICollection<ExhibitModel> UzunluqExhibits { get; set; }
        public virtual ICollection<ExhibitModel> DiametrExhibits { get; set; }
        public virtual ICollection<ExhibitModel> HundurlukExhibits { get; set; }
    }
}
