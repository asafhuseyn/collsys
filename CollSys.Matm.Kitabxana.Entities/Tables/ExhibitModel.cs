using CollSys.Matm.Kitabxana.Entities.Interfaces;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace CollSys.Matm.Kitabxana.Entities.Tables
{
    public enum Format
    {
        /*

        «A»-р.ф.
        «Б» -р.ф.
        «А», «Б», «С»,  « Az», « Av»-az, «Av»-rus, « Аl», «05»-rus, «05»-Az. 
        K-muz,  K-N.V.  K-F.C.  K-T.B,   K-Ə.Ə.  K-Ə.R.
        Xarici ədəbiyyat.
        Dövri mətbuat.  

        */

        A_nf,
        B_nf,
        A,
        B,
        C,
        Az,
        Av_az,
        Av_rus,
        Al,
        _05_az,
        _05_rus,
        K_muz,
        K_nv,
        K_fc,
        K_tb,
        K_aa,
        K_ar,
        XariciEdebiyyat,
        DovriMetbuat
    }

    [Table("Exhibits")]
    public class ExhibitModel : IEntity
    {
        public Format Format { get; set; }

        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [Required]
        [Column("Id")]
        public int Id { get; set; }

        [Display(Name = "İnventar No.")]
        [Required]
        [MaxLength(100, ErrorMessage = " *İnventar nömrəsi maksimum 100 xarakterdən ibarət ola bilər! ")]
        [Column("InventarNo")]
        public string InventarNo { get; set; }

        [Display(Name = "DK No.")]
        [Column("DkNo")]
        public int? DkNo { get; set; }

        [Display(Name = "Təhvil Akt No.")]
        [Column("TehvilAktNo")]
        public int? TehvilAktNo { get; set; }

        [Display(Name = "Təhvil alınma tarixi")]
        [Column("TehvilAlinmaTarixi")]
        [DataType(DataType.Date)]
        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:dd/MM/yyyy}")]
        public DateTime? TehvilAlinmaTarixi { get; set; }

        [Display(Name = "Ümumi ad")]
        [Column("UmumiAd")]
        [MaxLength(200, ErrorMessage = " *Ümumi ad maksimum 200 xarakterdən ibarət ola bilər! ")]
        public string UmumiAd { get; set; }


        [Display(Name = "Qiymət")]
        [Column("Qiymet")]
        public double? Qiymet { get; set; }

        [Display(Name = "Miqdar")]
        [Column("Miqdar")]
        public int? Miqdar { get; set; }


        [Display(Name = "Mənbə")]
        [Column("Menbe")]
        [MaxLength(200, ErrorMessage = " *Mənbə bölməsi maksimum 200 xarakterdən ibarət ola bilər! ")]
        public string Menbe { get; set; }

        [Display(Name = "Saxlanma yeri")]
        [Column("SaxlanmaYeri")]
        [MaxLength(200, ErrorMessage = " *Saxlanma yeri maksimum 200 xarakterdən ibarət ola bilər! ")]
        public string SaxlanmaYeri { get; set; }

        [Display(Name = "En")]
        [Column("En")]
        public double? En { get; set; }

        [Display(Name = "Uzunluq")]
        [Column("Uzunluq")]
        public double? Uzunluq { get; set; }

        [Display(Name = "Diametr")]
        [Column("Diametr")]
        public double? Diametr { get; set; }

        [Display(Name = "Hündürlük")]
        [Column("Hundurluk")]
        public double? Hundurluk { get; set; }

        [Display(Name = "Təsvir")]
        [Column("Tesvir")]
        [DataType(DataType.MultilineText)]
        [MaxLength(10000, ErrorMessage = " *Təsvir maksimum 10000 xarakterdən ibarət ola bilər! ")]
        public string Tesvir { get; set; }

        [Display(Name = "Yaradılma tarixi")]
        [Required]
        [Column("CreationTime")]
        public DateTime CreationTime { get; set; }

        [Display(Name = "Yenilənmə tarixi")]
        [Column("ModificationTime")]
        [Required]
        public DateTime ModificationTime { get; set; }

        [Display(Name = "Qeydiyyatı aparan")]
        [Required]
        [MaxLength(100)]
        public string CreatorUser { get; set; }

        [Display(Name = "Son dəyişiklik edən")]
        [Required]
        [MaxLength(100)]
        public string ModifierUser { get; set; }

        // Relations with other tables
        public int? CurrencyId { get; set; }
        [ForeignKey("CurrencyId")]
        public virtual CurrencyModel Currency { get; set; }

        [Display(Name = "Material")]
        public int? MaterialId { get; set; }
        [ForeignKey("MaterialId")]
        public virtual MaterialModel Material { get; set; }
        
        // Kitabxana specials
        [Display(Name = "Səhifə Sayı")]
        [Column("SehifeSayi")]
        public int? SehifeSayi { get; set; }
        
        [Display(Name = "ISBN")]
        [Column("ISBN")]
        [MaxLength(200, ErrorMessage = " *ISBN bölməsi maksimum 200 xarakterdən ibarət ola bilər! ")]
        public string ISBN { get; set; }
        
        [Display(Name = "Dərc olunma tarixi")]
        [Column("DercOlunmaTarixi")]
        [MaxLength(200, ErrorMessage = " *Derc olunma tarixi maksimum 200 xarakterdən ibarət ola bilər! ")]
        public string DercOlunmaTarixi { get; set; }
        
        [Display(Name = "Yazar")]
        public int? YazarId { get; set; }
        [ForeignKey("YazarId")]
        public virtual YazarModel Yazar { get; set; }
        
        [Display(Name = "Mətbəə")]
        public int? MetbeeId { get; set; }
        [ForeignKey("MetbeeId")]
        public virtual MetbeeModel Metbee { get; set; }
        //

        [Display(Name = "Saxlanma vəziyyəti")]
        public int? SaxlanmaVeziyyetiId { get; set; }
        [ForeignKey("SaxlanmaVeziyyetiId")]
        public virtual SaxlanmaVeziyyetiModel SaxlanmaVeziyyeti { get; set; }

        [Display(Name = "İstehsal yeri")]
        public int? IstehsalYeriId { get; set; }
        [ForeignKey("IstehsalYeriId")]
        public virtual RegionModel IstehsalYeri { get; set; }


        public int? EnUnitId { get; set; }
        [ForeignKey("EnUnitId")]
        public virtual MeasurementUnitModel EnUnit { get; set; }

        public int? UzunluqUnitId { get; set; }
        [ForeignKey("UzunluqUnitId")]
        public virtual MeasurementUnitModel UzunluqUnit { get; set; }

        public int? DiametrUnitId { get; set; }
        [ForeignKey("DiametrUnitId")]
        public virtual MeasurementUnitModel DiametrUnit { get; set; }

        public int? HundurlukUnitId { get; set; }
        [ForeignKey("HundurlukUnitId")]
        public virtual MeasurementUnitModel HundurlukUnit { get; set; }

        [Display(Name = "Şəkillər")]
        public virtual ICollection<ImageModel> Images { get; set; }

    }
}
