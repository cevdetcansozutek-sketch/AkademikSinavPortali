using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using AkademikSinavPortali.Models;

namespace AkademikSinavPortali.Controllers
{
    public class HocaController : Controller
    {
        private readonly OnlineSinavDbContext _context;

        public HocaController(OnlineSinavDbContext context)
        {
            _context = context;
        }

        // --- 1. SORU EKLEME EKRANI (ViewBag Dropdown) ---
        [HttpGet]
        public IActionResult SoruEkle()
        {
            ViewBag.Dersler = new SelectList(_context.Derslers, "DersId", "DersAdi");
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult SoruEkle(Sorular soru)
        {
            if (ModelState.IsValid)
            {
                _context.Sorulars.Add(soru);
                _context.SaveChanges();
                TempData["Mesaj"] = "Soru başarıyla eklendi!";
                return RedirectToAction(nameof(SoruEkle));
            }

            ViewBag.Dersler = new SelectList(_context.Derslers, "DersId", "DersAdi", soru.DersId);
            return View(soru);
        }

        // --- 2. SINAVA GÖRE SORU FİLTRELEME (A MADDESİ) ---
        [HttpGet]
        public IActionResult SinavaGoreSorular(int? secilenSinavId)
        {
            ViewBag.Sinavlar = new SelectList(_context.Sinavlars, "SinavId", "SinavAdi", secilenSinavId);

            if (secilenSinavId == null)
            {
                return View(new List<Sorular>());
            }

            // Many-to-Many ilişki üzerinden filtreleme
            var sinavinSorulari = _context.Sorulars
                .Where(soru => soru.Sinavs.Any(sinav => sinav.SinavId == secilenSinavId))
                .ToList();

            return View(sinavinSorulari);
        }

        // --- 3. ÖĞRENCİ ARAMA VE SINAV GEÇMİŞİ (B MADDESİ) ---
        [HttpGet]
        public IActionResult OgrenciArama(string aranacakKelime)
        {
            if (string.IsNullOrEmpty(aranacakKelime))
            {
                return View();
            }

            var ogrenci = _context.Ogrencilers
                .FirstOrDefault(o => o.OgrenciNo == aranacakKelime || o.Ad.Contains(aranacakKelime) || o.Soyad.Contains(aranacakKelime));

            if (ogrenci == null)
            {
                ViewBag.Mesaj = "Aradığınız kriterlere uygun öğrenci bulunamadı.";
                return View();
            }

            var yanitlar = _context.OgrenciYanitlaris
                .Include(y => y.Soru)
                .Include(y => y.Sinav)
                .Where(y => y.OgrenciId == ogrenci.OgrenciId)
                .ToList();

            var sinavGecmisi = yanitlar
                .GroupBy(y => new { y.Sinav.SinavId, y.Sinav.SinavAdi, y.Sinav.SinavTarihi })
                .Select(grup => new SinavSonucDetay
                {
                    SinavId = grup.Key.SinavId,
                    SinavAdi = grup.Key.SinavAdi,
                    SinavTarihi = grup.Key.SinavTarihi,
                    ToplamPuan = grup.Sum(y => y.VerilenCevap == y.Soru.DogruCevap ? (y.Soru.Puan ?? 10) : 0)
                })
                .ToList();

            var model = new OgrenciAramaViewModel
            {
                OgrenciBilgisi = ogrenci,
                SinavSonuclari = sinavGecmisi
            };

            return View(model);
        }

        // --- 4. SINAV ANALİZ EKRANI (C MADDESİ - Conditional Styling) ---
        [HttpGet]
        public IActionResult SinavAnaliz(int ogrenciId, int sinavId)
        {
            var analizVerisi = _context.OgrenciYanitlaris
                .Include(y => y.Soru)
                .Include(y => y.Ogrenci)
                .Include(y => y.Sinav)
                .Where(y => y.OgrenciId == ogrenciId && y.SinavId == sinavId)
                .ToList();

            if (!analizVerisi.Any())
            {
                ViewBag.Mesaj = "Bu sınava ait detaylı analiz verisi bulunamadı.";
                return View(new List<OgrenciYanitlari>());
            }

            ViewBag.OgrenciAdSoyad = $"{analizVerisi.First().Ogrenci.Ad} {analizVerisi.First().Ogrenci.Soyad}";
            ViewBag.SinavAdi = analizVerisi.First().Sinav.SinavAdi;

            return View(analizVerisi);
        }

        // --- 5. SINAV HAZIRLAMA VE SORU ATAMA (GET) ---
        [HttpGet]
        public IActionResult SinavHazirla()
        {
            ViewBag.Dersler = new SelectList(_context.Derslers, "DersId", "DersAdi");

            var model = new SinavHazirlamaViewModel
            {
                MevcutSorular = _context.Sorulars.ToList()
            };

            return View(model);
        }

        // --- 6. SINAV HAZIRLAMA VE SORU ATAMA (POST) ---
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult SinavHazirla(SinavHazirlamaViewModel model)
        {
            // 1. Yeni Sınavı Kaydet
            _context.Sinavlars.Add(model.YeniSinav);
            _context.SaveChanges();

            // 2. Seçilen soruları ara tabloya SQL ile ekle (EF Core Many-to-Many sınıfı üretmediği için en güvenli yol budur)
            if (model.SecilenSoruIdleri != null && model.SecilenSoruIdleri.Any())
            {
                foreach (var soruId in model.SecilenSoruIdleri)
                {
                    _context.Database.ExecuteSqlRaw("INSERT INTO SinavSorulari (SinavID, SoruID) VALUES ({0}, {1})", model.YeniSinav.SinavId, soruId);
                }
            }

            TempData["Mesaj"] = "Sınav başarıyla oluşturuldu ve sorular atandı!";
            return RedirectToAction(nameof(SinavHazirla));
        }
    }
}