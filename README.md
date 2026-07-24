# Bütçe Tahminleme Yazılımı - Yazılım Ekibi Referans Belgesi

## 1. Proje Özeti ve Kapsamı
- **Problem Tanımı:** Geleneksel bütçe planlama yöntemlerinin enflasyon, döviz kurları ve altın fiyatları gibi dış ekonomik şoklara ve dalgalanmalara karşı yetersiz kalması.
- **Hedef:** Son 10 yılın gerçekleşen kurumsal harcamalarını ve makroekonomik verileri (enflasyon, dolar, altın) baz alarak, bir sonraki yılın departman bütçelerini istatistiksel ağırlıklı formüllerle yüksek doğrulukta tahmin eden bir Web Service (API) geliştirmek.
- **Kapsam:** Bu proje görsel bir arayüz (UI) barındırmaz; tamamen arka planda çalışan (backend) bir hesaplama motorudur. Dışarıdan gelen parametreleri alır, iş mantığını çalıştırır ve diğer sistemlerin tüketebileceği standart bir JSON yanıtı üretir.

## 2. Analiz, Gereksinimler ve Çıktılar
- **Analiz ve Gereksinimler:** 
  - TCMB'den (makro veriler) ve KAP'tan (gerçekleşen kurumsal giderler) güvenilir 10 yıllık verinin toplanması.
  - C# tarafında tip dönüşüm hatalarını önlemek için Excel verilerindeki tüm ondalık sayıların KESİNLİKLE virgül (,) yerine nokta (.) ile ayrıştırılması.
  - API uç noktalarının (endpoints) esnek senaryolara (yüksek enflasyon, stabil kur vb.) anında yanıt verebilecek asenkron yapıda tasarlanması.
- **Proje Çıktıları:** 
  - Mimari standartlara uygun geliştirilmiş C# ASP.NET Core RESTful API.
  - Geçmiş verileri tutan yapılandırılmış SQLite veritabanı (`budget.db`).
  - Swagger ve Postman üzerinden test edilmiş, formatı standartlaştırılmış JSON Request/Response nesneleri.
  - Projenin çalışma mantığını ve mimarisini açıklayan maksimum 5 dakikalık demo videosu ve yazılı rapor (10 Ağustos teslimine hazır şekilde).

---

## 3. Proje Durumu ve Teknik Altyapı
- **Son Teslim Tarihi:** 10 Ağustos 2026 Pazartesi, Mesai Bitimi.
- **Analiz Ekibi Durumu:** Merve ve Hayrullah matematiksel tahmin formülünü (ağırlık katsayılarını) tamamladı ve dataları belirlenen uygun Excel formatında teslim etti.
- **Geliştirme Ortamı:** Visual Studio 2026 (ASP.NET ve Web Geliştirme iş yükü aktif).
- **Versiyon Kontrol:** Ortak GitHub reposu oluşturuldu.
- **Teknoloji Yığını:** C#, ASP.NET Core Web API, Entity Framework Core (SQLite).

---

## 4. Modüler Görev Dağılımı ve Detaylı İşlemler

Git çakışmalarını (conflict) önlemek ve kodlamayı eşzamanlı (paralel) yürütebilmek için proje 3 bağımsız katmana bölünmüştür. 

### Katman 1: Veri ve Mimari (Models & DB)
**Sorumlu:** [ .................................... ]
**Temel Görev:** Projenin veritabanı iskeletini ve dış dünya ile iletişim kuracak JSON transfer nesnelerini inşa etmek.
**Yapılacak İşlemler:**
1. **Modeller:** `Models` klasöründe `EconomicIndicator` (kurlar) ve `DepartmentBudget` (geçmiş harcamalar) Entity sınıflarını oluşturmak. Finansal verilerde hassasiyet için `decimal` veri tipini kullanmak.
2. **Bağlantı:** `Data` klasöründe `AppDbContext` sınıfını yazarak Entity'leri `DbSet` olarak tanımlamak.
3. **Konfigürasyon:** `appsettings.json` içerisine SQLite bağlantı dizesini (Connection String) eklemek ve `Program.cs` üzerinde veritabanı servisini kaydetmek.
4. **Veritabanı İnşası:** Package Manager Console üzerinden `Add-Migration InitialCreate` ve `Update-Database` komutlarını çalıştırarak fiziksel veritabanını yaratmak.
5. **DTO'lar:** Dışarıdan gelecek ve dışarı çıkacak veriler için `DTOs` klasöründe `BudgetRequestDto` ve `BudgetResponseDto` sınıflarını hazırlamak.

### Katman 2: İş Mantığı ve Algoritma (Services)
**Sorumlu:** [ .................................... ]
**Temel Görev:** Analiz ekibinin oluşturduğu matematiksel tahmin formülünü koda dökmek.
**Yapılacak İşlemler:**
1. **Arayüz:** `Services` klasöründe dış dünyanın hesaplama talep edebileceği `IBudgetPredictionService` arayüzünü (interface) tasarlamak.
2. **İmplementasyon:** Aynı klasörde `BudgetPredictionManager` sınıfını oluşturup arayüzü bağlamak.
3. **Hesaplama:** Analiz ekibinden gelen formülü ve ağırlık (W) katsayılarını kullanarak, `BudgetRequestDto`'dan alınan geçmiş harcama ve kur beklentileri üzerinden yeni bütçeyi hesaplayacak metodu yazmak.
4. **Yanıt:** Çıkan sonucu ve yüzde değişimini `BudgetResponseDto` nesnesine paketleyerek geriye döndürmek.
5. **Enjeksiyon:** `Program.cs` dosyasında Dependency Injection (Bağımlılık Enjeksiyonu) için `AddScoped` ile servisi sisteme kaydetmek.

### Katman 3: API Uç Noktaları (Controllers)
**Sorumlu:** [ .................................... ]
**Temel Görev:** İstemci (Swagger/Postman) ile arka plandaki Service katmanı arasındaki HTTP trafiğini yönetmek.
**Yapılacak İşlemler:**
1. **Denetleyici:** `Controllers` klasöründe `BudgetController` sınıfını (`[ApiController]`) oluşturmak.
2. **Servis Bağlantısı:** Yazılan `IBudgetPredictionService` arayüzünü yapıcı metot (constructor) üzerinden Controller'a enjekte etmek.
3. **Endpoint (Uç Nokta):** `[HttpPost("predict")]` rotasıyla dışarıdan `BudgetRequestDto` JSON objesi kabul eden bir metot yazmak.
4. **Doğrulama (Validation):** Gelen istekte boş veya mantıksız veri varsa, servisi yormadan doğrudan `BadRequest` (HTTP 400) hatası döndürmek.
5. **Yanıt:** Veriler kurallara uygunsa işlemi Service'e devretmek ve oradan dönen sonucu `Ok()` (HTTP 200) ile dışarı aktarmak.

---

## 5. Geliştirme Süreci Başlangıç Checklist'i

- [ ] Visual Studio 2026 üzerinden ortak GitHub reposu bilgisayarlara klonlandı.
- [ ] Yeni bir ASP.NET Core Web API projesi (Controllers kullanılarak) repoya eklendi.
- [ ] NuGet Package Manager üzerinden projenin ana kütüphaneleri yüklendi:
  - `Microsoft.EntityFrameworkCore.Sqlite`
  - `Microsoft.EntityFrameworkCore.Tools`
  - `Microsoft.EntityFrameworkCore.Design`
- [ ] Proje ana dizininde `Models`, `Data`, `DTOs`, `Services`, `Controllers` klasörleri açıldı.
- [ ] Ekip üyeleri için görev dağılımı yapılarak herkesin kendi branch'ini (dallanmasını) oluşturması kararlaştırıldı.
- [ ] Hazırlanan boş proje iskeleti ana branch'e gönderilerek (push), herkesin paralel geliştirmeye başlayabilmesi için en güncel yapıyı çekmesi (pull) sağlandı.
