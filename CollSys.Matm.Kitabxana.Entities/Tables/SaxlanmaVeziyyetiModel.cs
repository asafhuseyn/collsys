using CollSys.Matm.Kitabxana.Entities.Interfaces;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace CollSys.Matm.Kitabxana.Entities.Tables
{
    [Table("SaxlanmaVeziyyetleri")]
    public class SaxlanmaVeziyyetiModel : IEntity
    {
        [Key]
        [Required]
        [Column("Id")]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        [Display(Name = "Saxlanma Vəziyyəti")]
        [Required]
        [MaxLength(100, ErrorMessage = " *Saxlanma vəziyyəti maksimum 100 xarakterdən ibarət ola bilər! ")]
        [Column("SaxlanmaVeziyyeti")]
        public string SaxlanmaVeziyyeti { get; set; }

        // Relations with other tables
        public virtual ICollection<ExhibitModel> Exhibits { get; set; }
    }
}
