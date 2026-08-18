# Kurumsal Bütçe Tahminleme Sistemi

Kurumsal işletmelerin departman bazlı geçmiş harcama verilerini ve makroekonomik göstergeleri (enflasyon, döviz kurları, altın fiyatları) kullanarak, gelecek yıllara ait bütçe ihtiyaçlarını yüksek doğrulukla hesaplayan bulut tabanlı bir karar destek sistemidir.

## 💻 Arayüz
<img width="1197" height="798" alt="ui" src="https://github.com/user-attachments/assets/e73c5c9a-4679-42ea-80ac-e5ddfb69b3ca" />

## 🚀 Proje Hakkında

Bu proje veri girişi süreçlerini optimize ederek kullanıcı yükünü minimuma indirmeyi hedefler. Geçmiş yıllara ait bütçe harcamaları doğrudan veritabanından çekilirken, kullanıcı yalnızca departman seçimi ve hedef yıla ait beklenen ekonomik göstergeleri girerek anında sonuç alır.

**Temel Özellikler:**
* **Otomatik Veri Çekimi:** Geçmiş dönem bütçe verilerinin sistem tarafından otomatik işlenmesi.
* **Minimalist Kullanıcı Deneyimi (UX):** Sadece gerekli verilerin alındığı, karmaşadan uzak kurumsal web arayüzü.
* **Bulut Veritabanı:** Neon Cloud PostgreSQL altyapısı ile eşzamanlı ve güvenli veri erişimi.
* **Hızlı ve Doğru Hesaplama:** C# ve .NET Core mimarisi üzerinde çalışan matematiksel tahminleme modeli.

## 🛠 Kullanılan Teknolojiler

* **Backend:** C#, .NET Core, Entity Framework Core (Code-First)
* **Veritabanı:** PostgreSQL (Neon Cloud)
* **Frontend:** HTML5, CSS3, JavaScript (Single Page Application - SPA)

## 👥 Ekip
Projenin arkasında yazılım ve veri analizi disiplinlerini uyum içinde birleştiren beş kişilik bir ekip bulunmaktadır:
* **Backend, Database, UI/UX ve API Geliştirme:** Aslıhan, Ayhan, Ayşegül
* **Gereksinim Analizi ve Modelleme:** Merve, Hayrullah

## ⚙️ Kurulum ve Çalıştırma

1. Projeyi bilgisayarınıza klonlayın.
2. Gerekli bağımlılıkları yüklemek için appsettings.json içerisindeki veritabanı bağlantı dizesini (connection string) kendi Neon Cloud bilgilerinizle güncelleyin.
3. Frontend tarafındaki index.html dosyasını bir canlı sunucu (Live Server) eklentisiyle veya doğrudan tarayıcınızda açarak sistemi kullanmaya başlayabilirsiniz.
