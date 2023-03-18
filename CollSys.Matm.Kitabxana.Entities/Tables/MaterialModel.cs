using CollSys.Matm.Kitabxana.Entities.Interfaces;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace CollSys.Matm.Kitabxana.Entities.Tables
{
    [Table("Materials")]
    public class MaterialModel : IEntity
    {
        [Key]
        [Required]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [Column("Id")]
        public int Id { get; set; }

        [Display(Name = "Material")]
        [Required]
        [MaxLength(100, ErrorMessage = " *Material maksimum 100 xarakterdən ibarət ola bilər! ")]
        [Column("Material")]
        public string Material { get; set; }

        // Relations with other tables
        public virtual ICollection<ExhibitModel> Exhibits { get; set; }
    }
}
