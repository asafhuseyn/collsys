using CollSys.Matm.Kitabxana.Entities.Interfaces;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace CollSys.Matm.Kitabxana.Entities.Tables
{
    [Table("Images")]
    public class ImageModel : IEntity
    {
        [Key]
        [Required]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [Column("Id")]
        public int Id { get; set; }

        [Required]
        [MaxLength(100)]
        [Column("Name")]
        public string Name { get; set; }

        [MaxLength(100)]
        [Column("Folder")]
        public string Folder { get; set; }

        [Required]
        [MaxLength(500)]
        [Column("Path")]
        public string Path { get; set; } = "Images";

        // Relations with other tables
        [Required]
        [Column("ExhibitId")]
        public int ExhibitId { get; set; }
        [ForeignKey("ExhibitId")]
        public virtual ExhibitModel Exhibit { get; set; }
    }
}
