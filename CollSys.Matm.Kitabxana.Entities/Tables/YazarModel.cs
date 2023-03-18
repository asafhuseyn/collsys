using CollSys.Matm.Kitabxana.Entities.Interfaces;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace CollSys.Matm.Kitabxana.Entities.Tables
{
    [Table("Yazarlar")]
    public class YazarModel : IEntity
    {
        [Key]
        [Required]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [Column("Id")]
        public int Id { get; set; }

        [Display(Name = "Yazar")]
        [Required]
        [MaxLength(100, ErrorMessage = " *Yazar maksimum 100 xarakterdən ibarət ola bilər! ")]
        [Column("Yazar")]
        public string Yazar { get; set; }

        // Relations with other tables
        public virtual ICollection<ExhibitModel> Exhibits { get; set; }
    }
}