using System;
using System.Collections.Generic;

namespace AkademikSinavPortali.Models
{
    public class OgrenciAramaViewModel
    {
        public Ogrenciler OgrenciBilgisi { get; set; } = null!;
        public List<SinavSonucDetay> SinavSonuclari { get; set; } = new List<SinavSonucDetay>();
    }

    public class SinavSonucDetay
    {
        public int SinavId { get; set; } // BUTONUN ÇALIŞMASI İÇİN BU ŞART
        public string SinavAdi { get; set; } = null!;
        public DateTime SinavTarihi { get; set; }
        public int ToplamPuan { get; set; }
    }
}