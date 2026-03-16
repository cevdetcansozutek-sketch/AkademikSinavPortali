using System;
using System.Collections.Generic;

namespace AkademikSinavPortali.Models;

public partial class Dersler
{
    public int DersId { get; set; }

    public string DersAdi { get; set; } = null!;

    public string DersKodu { get; set; } = null!;

    public virtual ICollection<Sinavlar> Sinavlars { get; set; } = new List<Sinavlar>();

    public virtual ICollection<Sorular> Sorulars { get; set; } = new List<Sorular>();
}
