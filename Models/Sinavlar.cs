using System;
using System.Collections.Generic;

namespace AkademikSinavPortali.Models;

public partial class Sinavlar
{
    public int SinavId { get; set; }

    public string SinavAdi { get; set; } = null!;

    public DateTime SinavTarihi { get; set; }

    public int? DersId { get; set; }

    public virtual Dersler? Ders { get; set; }

    public virtual ICollection<OgrenciYanitlari> OgrenciYanitlaris { get; set; } = new List<OgrenciYanitlari>();

    public virtual ICollection<Sorular> Sorus { get; set; } = new List<Sorular>();
}
