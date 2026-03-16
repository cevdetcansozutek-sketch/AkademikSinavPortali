using System.Collections.Generic;

namespace AkademikSinavPortali.Models
{
    public class SinavHazirlamaViewModel
    {
        // Oluşturulacak yeni sınavın bilgileri (Sınav Adı, Tarihi, DersId)
        public Sinavlar YeniSinav { get; set; } = new Sinavlar();

        // Ekranda listelenecek mevcut sorular
        public List<Sorular> MevcutSorular { get; set; } = new List<Sorular>();

        // Hocanın checkbox'lardan seçeceği soruların ID'lerini tutacak liste
        public List<int> SecilenSoruIdleri { get; set; } = new List<int>();
    }
}