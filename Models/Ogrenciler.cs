using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace AkademikSinavPortali.Models;

public partial class Ogrenciler
{
    public int OgrenciId { get; set; }

    [Required(ErrorMessage = "Ad alanı boş bırakılamaz.")]
    [StringLength(50, MinimumLength = 2, ErrorMessage = "Ad en az 2, en fazla 50 karakter olmalıdır.")]
    public string Ad { get; set; } = null!;

    [Required(ErrorMessage = "Soyad alanı boş bırakılamaz.")]
    [StringLength(50, MinimumLength = 2, ErrorMessage = "Soyad en az 2, en fazla 50 karakter olmalıdır.")]
    public string Soyad { get; set; } = null!;

    [Required(ErrorMessage = "Öğrenci numarası zorunludur.")]
    [RegularExpression(@"^\d{10}$", ErrorMessage = "Öğrenci numarası tam olarak 10 haneli ve sadece rakamlardan oluşmalıdır.")]
    public string OgrenciNo { get; set; } = null!;

    [Required(ErrorMessage = "Şifre alanı zorunludur.")]
    public string Sifre { get; set; } = null!;

    public virtual ICollection<OgrenciYanitlari> OgrenciYanitlaris { get; set; } = new List<OgrenciYanitlari>();
}