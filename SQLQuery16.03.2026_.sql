CREATE DATABASE OnlineSinavDB;

GO

USE OnlineSinavDB;

GO


-- 1. DERSLER (Sistemin temeli)

CREATE TABLE Dersler (

DersID INT PRIMARY KEY IDENTITY(1,1),

DersAdi NVARCHAR(100) NOT NULL,

DersKodu NVARCHAR(10) NOT NULL UNIQUE

);


-- 2. SORULAR (Test formatýnda, bir derse baðlý)

CREATE TABLE Sorular (

SoruID INT PRIMARY KEY IDENTITY(1,1),

SoruMetni NVARCHAR(MAX) NOT NULL,

SecenekA NVARCHAR(200) NOT NULL,

SecenekB NVARCHAR(200) NOT NULL,

SecenekC NVARCHAR(200) NOT NULL,

SecenekD NVARCHAR(200) NOT NULL,

DogruCevap CHAR(1) NOT NULL, -- 'A', 'B', 'C', 'D'

Puan INT DEFAULT 10,

DersID INT FOREIGN KEY REFERENCES Dersler(DersID)

);


-- 3. SINAVLAR (Bir dersin sýnavý olur)

CREATE TABLE Sinavlar (

SinavID INT PRIMARY KEY IDENTITY(1,1),

SinavAdi NVARCHAR(100) NOT NULL,

SinavTarihi DATETIME NOT NULL,

DersID INT FOREIGN KEY REFERENCES Dersler(DersID)

);


-- 4. SINAV_SORULARI (Hangi sýnavda hangi sorular çýkacak? - Çoka Çok Ýliþki)

CREATE TABLE SinavSorulari (

SinavID INT FOREIGN KEY REFERENCES Sinavlar(SinavID),

SoruID INT FOREIGN KEY REFERENCES Sorular(SoruID),

PRIMARY KEY (SinavID, SoruID)

);


-- 5. ÖÐRENCÝLER (Giriþ bilgileriyle birlikte)

CREATE TABLE Ogrenciler (

OgrenciID INT PRIMARY KEY IDENTITY(1,1),

Ad NVARCHAR(50) NOT NULL,

Soyad NVARCHAR(50) NOT NULL,

OgrenciNo NVARCHAR(20) UNIQUE NOT NULL,

Sifre NVARCHAR(20) NOT NULL

);


-- 6. ÖÐRENCÝ_YANITLARI (Sýnav anýnda verilen cevaplar)

CREATE TABLE OgrenciYanitlari (

YanitID INT PRIMARY KEY IDENTITY(1,1),

SinavID INT FOREIGN KEY REFERENCES Sinavlar(SinavID),

OgrenciID INT FOREIGN KEY REFERENCES Ogrenciler(OgrenciID),

SoruID INT FOREIGN KEY REFERENCES Sorular(SoruID),

VerilenCevap CHAR(1), -- Öðrencinin seçtiði þýk

IsCorrect BIT -- Doðru mu yanlýþ mý? (Kod tarafýnda hesaplanabilir)

);

USE OnlineSinavDB;
GO

-- 1. Önce test derslerini ekleyelim
INSERT INTO Dersler (DersAdi, DersKodu) VALUES ('Görsel Programlama', 'GPRG101'), ('Veritabaný Yönetimi', 'VTYS202');

-- 2. Test sorularýný ekleyelim (Her ders için 2 soru)
INSERT INTO Sorular (SoruMetni, SecenekA, SecenekB, SecenekC, SecenekD, DogruCevap, Puan, DersID) 
VALUES ('C# dilinde deðiþken tanýmlarken hangisi kullanýlýr?', 'int', 'var', 'string', 'Hepsi', 'D', 25, 1),
       ('MVC mimarisinde "C" harfi neyi temsil eder?', 'Context', 'Controller', 'Class', 'Core', 'B', 25, 1);

-- 3. Bir test öðrencisi ekleyelim (Arama yapmak için bunu kullanacaðýz)
INSERT INTO Ogrenciler (Ad, Soyad, OgrenciNo, Sifre) VALUES ('Cevdetcan', 'Sözütek', '1234567890', '123456');

-- 4. Bir sýnav oluþturalým
INSERT INTO Sinavlar (SinavAdi, SinavTarihi, DersID) VALUES ('Vize Sýnavý', GETDATE(), 1);

-- 5. Sýnava soru atayalým (Ara tablo kaydý)
INSERT INTO SinavSorulari (SinavID, SoruID) VALUES (1, 1), (1, 2);

-- 6. Öðrenci bu sýnava girmiþ gibi yanýtlarýný kaydedelim (Analiz ekraný için)
INSERT INTO OgrenciYanitlari (SinavID, OgrenciID, SoruID, VerilenCevap) VALUES (1, 1, 1, 'D'); -- Doðru cevap
INSERT INTO OgrenciYanitlari (SinavID, OgrenciID, SoruID, VerilenCevap) VALUES (1, 1, 2, 'A'); -- Yanlýþ cevap


USE OnlineSinavDB;
GO

-- 1. ADIM: DERSLERÝ TANIMLAYALIM
-- Eðer daha önce eklemediysen hata almamak için kontrol ederek ekler
IF NOT EXISTS (SELECT * FROM Dersler WHERE DersKodu = 'GPRG101')
    INSERT INTO Dersler (DersAdi, DersKodu) VALUES ('Görsel Programlama', 'GPRG101');
IF NOT EXISTS (SELECT * FROM Dersler WHERE DersKodu = 'VTYS202')
    INSERT INTO Dersler (DersAdi, DersKodu) VALUES ('Veritabaný Yönetimi', 'VTYS202');

-- 2. ADIM: 10 TANE ÖRNEK SORU (Hocanýn istediði Puan ve DersID iliþkisiyle)
INSERT INTO Sorular (SoruMetni, SecenekA, SecenekB, SecenekC, SecenekD, DogruCevap, Puan, DersID) VALUES
-- Görsel Programlama Sorularý (DersID: 1)
('C# dilinde kalýtým (inheritance) hangi sembol ile yapýlýr?', '.', ':', '->', '@', 'B', 10, 1),
('ASP.NET MVC''de verileri View''a taþýmak için hangisi kullanýlýr?', 'ViewBag', 'DataBag', 'ModelGate', 'Carrier', 'A', 10, 1),
('Entity Framework''te veritabaný tablolarýný temsil eden sýnýflara ne denir?', 'Controller', 'Partial', 'Entity', 'Context', 'C', 10, 1),
('C# dilinde döngüyü tamamen sonlandýrmak için kullanýlan anahtar kelime nedir?', 'continue', 'stop', 'exit', 'break', 'D', 10, 1),
('WinForms uygulamalarýnda butonun týklanma olayýnýn adý nedir?', 'OnHover', 'OnClick', 'Changed', 'Load', 'B', 10, 1),

-- Veritabaný Sorularý (DersID: 2)
('SQL''de mevcut bir kaydý güncellemek için hangi komut kullanýlýr?', 'CHANGE', 'MODIFY', 'UPDATE', 'SET', 'C', 10, 2),
('Tablodaki tüm satýrlarý silen ama tablo yapýsýný koruyan komut hangisidir?', 'DROP', 'DELETE', 'REMOVE', 'TRUNCATE', 'D', 10, 2),
('Birincil anahtar (Primary Key) özelliði hangisidir?', 'Boþ olabilir', 'Benzersizdir', 'Sadece rakamdýr', 'Gizlidir', 'B', 10, 2),
('Ýki tabloyu birleþtirmek için kullanýlan SQL deyimi nedir?', 'UNION', 'COMBINE', 'JOIN', 'GROUP BY', 'C', 10, 2),
('Verileri belirli bir kolona göre sýralamak için ne kullanýlýr?', 'SORT BY', 'ORDER BY', 'ARRANGE', 'INDEX', 'B', 10, 2);

-- 3. ADIM: BÝR ÖÐRENCÝ EKLEYELÝM (Arama ekranýnda test etmek için)
-- Eðer öðrenci zaten varsa eklemez
IF NOT EXISTS (SELECT * FROM Ogrenciler WHERE OgrenciNo = '1234567890')
    INSERT INTO Ogrenciler (Ad, Soyad, OgrenciNo, Sifre) VALUES ('Cevdetcan', 'Sözütek', '1234567890', '123456');

-- 4. ADIM: BÝR SINAV OLUÞTURUP SORULARI ATAYALIM
INSERT INTO Sinavlar (SinavAdi, SinavTarihi, DersID) VALUES ('Final Sýnavý', GETDATE(), 1);
DECLARE @LastSinavId INT = SCOPE_IDENTITY();

-- Ýlk 5 soruyu (Görsel Programlama) bu sýnava atayalým
INSERT INTO SinavSorulari (SinavID, SoruID) 
SELECT @LastSinavId, SoruID FROM Sorular WHERE DersID = 1;

-- 5. ADIM: ANALÝZ EKRANI ÝÇÝN ÖÐRENCÝ YANITLARI (Bazýlarýný yanlýþ yapalým ki kýrmýzý görünsün)
INSERT INTO OgrenciYanitlari (SinavID, OgrenciID, SoruID, VerilenCevap) VALUES
(@LastSinavId, 1, 1, 'B'), -- Doðru
(@LastSinavId, 1, 2, 'A'), -- Doðru
(@LastSinavId, 1, 3, 'D'), -- Yanlýþ (Kýrmýzý görünecek)
(@LastSinavId, 1, 4, 'B'), -- Yanlýþ (Kýrmýzý görünecek)
(@LastSinavId, 1, 5, 'B'); -- Doðru

USE OnlineSinavDB;
GO

-- Önce eski test verilerini temizleyelim (Hata almamak için)
DELETE FROM OgrenciYanitlari;
DELETE FROM SinavSorulari;
DELETE FROM Sinavlar;
DELETE FROM Sorular;
DELETE FROM Ogrenciler;
DELETE FROM Dersler;

-- 1. Ders ve Öðrenci (Öðrenci No: 1234567890)
INSERT INTO Dersler (DersAdi, DersKodu) VALUES ('Görsel Programlama', 'GPRG101');
DECLARE @DersId INT = SCOPE_IDENTITY();

INSERT INTO Ogrenciler (Ad, Soyad, OgrenciNo, Sifre) VALUES ('Cevdetcan', 'Sözütek', '1234567890', '123456');
DECLARE @OgrenciId INT = SCOPE_IDENTITY();

-- 2. Sýnav
INSERT INTO Sinavlar (SinavAdi, SinavTarihi, DersID) VALUES ('Vize Sýnavý', GETDATE(), @DersId);
DECLARE @SinavId INT = SCOPE_IDENTITY();

-- 3. Sorular (Her biri 50 puan)
INSERT INTO Sorular (SoruMetni, SecenekA, SecenekB, SecenekC, SecenekD, DogruCevap, Puan, DersID) 
VALUES ('C# nedir?', 'Dil', 'Meyve', 'Araba', 'Þehir', 'A', 50, @DersId);
DECLARE @S1 INT = SCOPE_IDENTITY();

INSERT INTO Sorular (SoruMetni, SecenekA, SecenekB, SecenekC, SecenekD, DogruCevap, Puan, DersID) 
VALUES ('SQL nedir?', 'Yemek', 'Veritabaný', 'Spor', 'Müzik', 'B', 50, @DersId);
DECLARE @S2 INT = SCOPE_IDENTITY();

-- 4. Sýnav-Soru Baðlantýsý
INSERT INTO SinavSorulari (SinavID, SoruID) VALUES (@SinavId, @S1), (@SinavId, @S2);

-- 5. ÖÐRENCÝ YANITLARI (Analiz sayfasýný dolduran asýl kýsým burasýdýr!)
-- Öðrenci 1. soruya 'A' (Doðru), 2. soruya 'C' (Yanlýþ) cevabýný vermiþ olsun.
INSERT INTO OgrenciYanitlari (SinavID, OgrenciID, SoruID, VerilenCevap) 
VALUES (@SinavId, @OgrenciId, @S1, 'A'), (@SinavId, @OgrenciId, @S2, 'C');


USE OnlineSinavDB;
GO

-- 1. TEMÝZLÝK (Eski verilerle karýþmamasý için)
DELETE FROM OgrenciYanitlari;
DELETE FROM SinavSorulari;
DELETE FROM Sinavlar;
DELETE FROM Sorular;
DELETE FROM Ogrenciler;
DELETE FROM Dersler;

-- 2. DERS VE ÖÐRENCÝ TANIMLAMA
INSERT INTO Dersler (DersAdi, DersKodu) VALUES ('Görsel Programlama II', 'GPRG202');
DECLARE @DersId INT = SCOPE_IDENTITY();

INSERT INTO Ogrenciler (Ad, Soyad, OgrenciNo, Sifre) VALUES ('Cevdetcan', 'Sözütek', '1234567890', '123456');
DECLARE @OgrenciId INT = SCOPE_IDENTITY();

-- 3. 20 TANE EFSANE SORU (Her biri 5 puan, toplam 100)
INSERT INTO Sorular (SoruMetni, SecenekA, SecenekB, SecenekC, SecenekD, DogruCevap, Puan, DersID) VALUES
('C# dilinde deðiþken tipi "int" neyi ifade eder?', 'Metin', 'Tam Sayý', 'Ondalýklý Sayý', 'Mantýksal', 'B', 5, @DersId),
('MVC mimarisinde "C" hangi kelimenin kýsaltmasýdýr?', 'Context', 'Class', 'Controller', 'Core', 'C', 5, @DersId),
('Veritabanýnda verileri silmek için kullanýlan komut hangisidir?', 'UPDATE', 'SELECT', 'DELETE', 'INSERT', 'C', 5, @DersId),
('ASP.NET Core projesinde baþlangýç ayarlarý hangi dosyada yapýlýr?', 'Program.cs', 'Index.cshtml', 'Site.css', 'Model.cs', 'A', 5, @DersId),
('Entity Framework''te veritabaný iþlemlerini yöneten ana sýnýf hangisidir?', 'DbSet', 'DbContext', 'DbTable', 'DbView', 'B', 5, @DersId),
('HTML''de sayfa baþlýðý hangi etiketler arasýna yazýlýr?', '<body>', '<div>', '<head>', '<title>', 'D', 5, @DersId),
('SQL''de verileri sýralamak için ne kullanýlýr?', 'GROUP BY', 'ORDER BY', 'SORT BY', 'ARRANGE', 'B', 5, @DersId),
('Birincil anahtar (Primary Key) özelliði nedir?', 'Boþ olabilir', 'Tekrar edebilir', 'Benzersizdir', 'Gizlidir', 'C', 5, @DersId),
('C# dilinde hata yakalamak için hangi blok kullanýlýr?', 'try-catch', 'if-else', 'while', 'switch', 'A', 5, @DersId),
('Veritabanýnda iki tabloyu birleþtirmek için ne kullanýlýr?', 'UNION', 'ADD', 'JOIN', 'SUM', 'C', 5, @DersId),
('C# dilinde döngüyü kýrmak için hangisi kullanýlýr?', 'continue', 'break', 'return', 'exit', 'B', 5, @DersId),
('MVC''de kullanýcýya gösterilen arayüz katmaný hangisidir?', 'Model', 'Controller', 'View', 'Router', 'C', 5, @DersId),
('Veritabanýnda kayýtlarý güncellemek için ne kullanýlýr?', 'MODIFY', 'SET', 'UPDATE', 'CHANGE', 'C', 5, @DersId),
('Bootstrap kütüphanesi ne için kullanýlýr?', 'Veritabaný', 'Tasarým/Arayüz', 'Sunucu Yönetimi', 'Yapay Zeka', 'B', 5, @DersId),
('C# dilinde "string" ne tür veriler için kullanýlýr?', 'Sayý', 'Tarih', 'Metin', 'Doðru/Yanlýþ', 'C', 5, @DersId),
('SQL''de "SELECT *" ifadesindeki yýldýz ne anlama gelir?', 'Hepsini Seç', 'Sýrala', 'Filtrele', 'Sil', 'A', 5, @DersId),
('EF Core''da Migration ne için kullanýlýr?', 'Veri silmek', 'Tablo yapýsýný güncellemek', 'Hýzlandýrmak', 'Yedeklemek', 'B', 5, @DersId),
('C# dilinde statik metodlar nasýl çaðrýlýr?', 'Nesne ile', 'Sýnýf ismiyle', 'Çaðrýlamaz', 'New ile', 'B', 5, @DersId),
('SQL''de toplama iþlemi yapan fonksiyon hangisidir?', 'TOTAL', 'COUNT', 'SUM', 'ADD', 'C', 5, @DersId),
('Projenizdeki "Validation" kurallarý nerede tanýmlanýr?', 'Controller', 'View', 'Model', 'SQL', 'C', 5, @DersId);

-- 4. SINAV OLUÞTURMA VE 20 SORUYU ATAMA
INSERT INTO Sinavlar (SinavAdi, SinavTarihi, DersID) VALUES ('Yýl Sonu Final Sýnavý', GETDATE(), @DersId);
DECLARE @SinavId INT = SCOPE_IDENTITY();

INSERT INTO SinavSorulari (SinavID, SoruID) 
SELECT @SinavId, SoruID FROM Sorular;

-- 5. CEVDETCAN'IN YANITLARI (Analiz ekraný þenlensin diye bazýlarý doðru, bazýlarý yanlýþ)
-- Ýlk 15 soru doðru (75 Puan), son 5 soru yanlýþ olsun.
INSERT INTO OgrenciYanitlari (SinavID, OgrenciId, SoruId, VerilenCevap)
SELECT @SinavId, @OgrenciId, SoruID, DogruCevap FROM Sorular WHERE SoruID <= (SELECT MIN(SoruID)+14 FROM Sorular);

-- Son 5 soruyu yanlýþ cevapla (Hepsine 'X' diyelim ki hata listesinde kýrmýzý yansýn)
INSERT INTO OgrenciYanitlari (SinavID, OgrenciId, SoruId, VerilenCevap)
SELECT @SinavId, @OgrenciId, SoruID, 'X' FROM Sorular WHERE SoruID > (SELECT MIN(SoruID)+14 FROM Sorular);