using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace AkademikSinavPortali.Models;

public partial class Sorular
{
    public int SoruId { get; set; }

    [Required(ErrorMessage = "Soru metni boş olamaz.")]
    public string SoruMetni { get; set; } = null!;

    [Required(ErrorMessage = "A şıkkı boş olamaz.")]
    public string SecenekA { get; set; } = null!;

    [Required(ErrorMessage = "B şıkkı boş olamaz.")]
    public string SecenekB { get; set; } = null!;

    [Required(ErrorMessage = "C şıkkı boş olamaz.")]
    public string SecenekC { get; set; } = null!;

    [Required(ErrorMessage = "D şıkkı boş olamaz.")]
    public string SecenekD { get; set; } = null!;

    [Required(ErrorMessage = "Doğru cevap boş geçilemez.")]
    public string DogruCevap { get; set; } = null!;

    [Range(0, 100, ErrorMessage = "Puan 0 ile 100 arasında olmalıdır.")]
    public int? Puan { get; set; }

    public int? DersId { get; set; }

    public virtual Dersler? Ders { get; set; }
    public virtual ICollection<OgrenciYanitlari> OgrenciYanitlaris { get; set; } = new List<OgrenciYanitlari>();
    public virtual ICollection<Sinavlar> Sinavs { get; set; } = new List<Sinavlar>();
}