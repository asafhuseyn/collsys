using CollSys.Matm.Kitabxana.Entities.Interfaces;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace CollSys.Matm.Kitabxana.Entities.Tables
{
    [Table("Metbeeler")]
    public class MetbeeModel : IEntity
    {
        [Key]
        [Required]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [Column("Id")]
        public int Id { get; set; }

        [Display(Name = "Mətbəə")]
        [Required]
        [MaxLength(100, ErrorMessage = " *Mətbəə maksimum 100 xarakterdən ibarət ola bilər! ")]
        [Column("Metbee")]
        public string Metbee { get; set; }

        // Relations with other tables
        public virtual ICollection<ExhibitModel> Exhibits { get; set; }
    }
}