# BitLife Turkish Edition - Proje Dokümantasyonu

## Proje Özeti
BitLife tarzında, tamamen Türkçe ve Türk kültürüne uyarlanmış metin tabanlı hayat simülasyonu oyunu. Unity ile C# kullanılarak Android için geliştirilecek.

## Temel Prensipler

### UI Geliştirme Kuralları
- **%100 Kod Tabanlı UI**: Tüm UI elementleri C# kodu ile oluşturulacak
- **Manuel Atama Yasak**: Inspector üzerinden hiçbir referans ataması yapılmayacak
- **Prefab Kullanımı Yok**: UI prefab'ları kullanılmayacak, her şey runtime'da oluşturulacak
- **ScriptableObject ile Veri**: Oyun verileri ScriptableObject'lerde tutulacak

### Mimari Yapı
```
Assets/
├── Scripts/
│   ├── Core/           # Temel sistemler (GameManager, EventSystem)
│   ├── UI/             # UI oluşturma ve yönetim
│   ├── Data/           # Veri modelleri ve ScriptableObject'ler
│   ├── Systems/        # Oyun sistemleri (Stat, Event, Decision)
│   ├── Localization/   # Türkçe metinler
│   └── Utils/          # Yardımcı sınıflar
├── Resources/          # Runtime yüklenecek veriler
└── StreamingAssets/    # JSON veri dosyaları
```

### Kod Standartları
- **Namespace**: `BitLifeTR` ana namespace olacak
- **Naming**: PascalCase sınıflar, camelCase değişkenler
- **Singleton Pattern**: Manager sınıfları için
- **Event-Driven**: Sistemler arası iletişim event'ler ile
- **Factory Pattern**: UI element oluşturma için

## Oyun Sistemleri

### Stat Sistemi
- Sağlık (0-100)
- Mutluluk (0-100)
- Zeka (0-100)
- Görünüm (0-100)
- Para (dinamik)
- Şöhret (0-100)
- Karma (0-100)

### Yaş Dönemleri
1. Bebeklik (0-4)
2. Çocukluk (5-11)
3. Ergenlik (12-17)
4. Genç Yetişkinlik (18-29)
5. Yetişkinlik (30-49)
6. Orta Yaş (50-64)
7. Yaşlılık (65+)

### Ana Sistemler
- Karar Ağacı Sistemi
- Rastgele Olay Üretici
- İlişki Yönetimi
- Kariyer/Eğitim Sistemi
- Sağlık/Hastalık Sistemi
- Suç/Hukuk Sistemi
- Ekonomi Sistemi

## Türkiye'ye Özgü İçerikler

### Eğitim Sistemi
- İlkokul, Ortaokul, Lise
- Üniversite Sınavı (YKS)
- Devlet/Özel Üniversite
- Yüksek Lisans, Doktora

### Meslekler
- Devlet Memuru
- Doktor, Avukat, Mühendis
- Esnaf, Tüccar
- Sanatçı, Futbolcu
- YouTuber, Influencer
- Şoför, Kuaför, vb.

### Sosyal Dinamikler
- Aile baskısı
- Komşu ilişkileri
- Bayram ziyaretleri
- Askerlik
- Düğün/Nişan gelenekleri

## Geliştirme Fazları

### Faz 1: Proje Altyapısı
Unity projesi kurulumu, temel klasör yapısı, ana manager sınıfları

### Faz 2: Temel UI Framework
Kod tabanlı UI sistemi, buton/panel/text factory'leri

### Faz 3: Veri Sistemi
Stat sistemi, karakter modeli, kaydet/yükle

### Faz 4: Olay Sistemi
Event tanımları, karar ağacı, sonuç hesaplama

### Faz 5: Ana Oyun Döngüsü
Yaş ilerleme, yıllık olaylar, ölüm sistemi

### Faz 6: İçerik Üretimi
Türkçe olaylar, meslekler, rastgele durumlar

### Faz 7: UI Ekranları
Ana menü, oyun ekranı, istatistikler, ayarlar

### Faz 8: Gelişmiş Sistemler
İlişkiler, kariyer, suç, sağlık detayları

### Faz 9: Polish & Test
Performans, bug fix, balans ayarları

### Faz 10: Build & Dağıtım
Android build, optimizasyon, release

## Genişletilebilirlik

### Yeni İçerik Ekleme
- JSON tabanlı event tanımları
- ScriptableObject ile meslek/eğitim ekleme
- Modüler sistem tasarımı

### Güncelleme Desteği
- Versiyon kontrolü
- Cloud save desteği (opsiyonel)
- Analytics entegrasyonu (opsiyonel)

## Notlar
- Minimum Android API: 21 (5.0 Lollipop)
- Unity versiyon: 2022.3 LTS önerilir
- Text Mesh Pro kullanılacak
- Türkçe karakter desteği zorunlu
