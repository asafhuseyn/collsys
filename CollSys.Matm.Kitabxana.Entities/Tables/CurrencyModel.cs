using CollSys.Matm.Kitabxana.Entities.Interfaces;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace CollSys.Matm.Kitabxana.Entities.Tables
{
    [Table("Currencies")]
    public class CurrencyModel : IEntity
    {
        [Key]
        [Required]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [Column("Id")]
        public int Id { get; set; }

        [Display(Name = "Məzənnə")]
        [Required]
        [Column("Currency")]
        [MaxLength(100, ErrorMessage = " *Məzənnə maksimum 100 xarakterdən ibarət ola bilər! ")]
        public string Currency { get; set; }

        // Relations with other tables
        public virtual ICollection<ExhibitModel> Exhibits { get; set; }

    }
}
