using System;
using System.Collections.Generic;

namespace AkademikSinavPortali.Models;

public partial class OgrenciYanitlari
{
    public int YanitId { get; set; }

    public int? SinavId { get; set; }

    public int? OgrenciId { get; set; }

    public int? SoruId { get; set; }

    public string? VerilenCevap { get; set; }

    public bool? IsCorrect { get; set; }

    public virtual Ogrenciler? Ogrenci { get; set; }

    public virtual Sinavlar? Sinav { get; set; }

    public virtual Sorular? Soru { get; set; }
}
