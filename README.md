# BitLife Turkish Edition

Tamamen Türkçe ve Türk kültürüne uyarlanmış metin tabanlı hayat simülasyonu oyunu.

## Özellikler

### Temel Sistemler
- **Stat Sistemi**: Sağlık, Mutluluk, Zeka, Görünüm, Para, Şöhret, Karma
- **Yaş Dönemleri**: Bebeklik, Çocukluk, Ergenlik, Genç Yetişkinlik, Yetişkinlik, Orta Yaş, Yaşlılık
- **Olay Sistemi**: Yüzlerce Türkçe olay ve karar ağacı
- **Kaydet/Yükle**: Otomatik ve manuel kaydetme

### Türkiye'ye Özgü İçerikler
- **Eğitim**: İlkokul, Ortaokul, Lise, YKS Sınavı, Üniversite, Yüksek Lisans, Doktora
- **Meslekler**: Devlet Memuru, Doktor, Avukat, Mühendis, Esnaf, YouTuber, Futbolcu ve daha fazlası
- **Sosyal Dinamikler**: Aile baskısı, komşu ilişkileri, bayram ziyaretleri, askerlik, düğün gelenekleri
- **Ekonomi**: Türk Lirası, enflasyon, ev/araba kredileri

### Teknik Özellikler
- %100 kod tabanlı UI (prefab yok)
- Event-driven mimari
- Factory pattern ile UI oluşturma
- JSON tabanlı olay sistemi
- ScriptableObject veri modelleri

## Proje Yapısı

```
Assets/
├── Scripts/
│   ├── Core/           # GameManager, EventBus, SaveManager
│   ├── UI/             # UIManager, Factory'ler, Ekranlar
│   ├── Data/           # CharacterData, veri modelleri
│   ├── Systems/        # Stat, Event, Career, Health sistemleri
│   ├── Localization/   # Türkçe metinler
│   └── Utils/          # Yardımcı sınıflar
├── Resources/          # Runtime yüklenecek veriler
└── StreamingAssets/    # JSON veri dosyaları
```

## Geliştirme Fazları

- [x] Faz 1: Proje Altyapısı
- [x] Faz 2: Temel UI Framework
- [x] Faz 3: Veri Sistemi
- [x] Faz 4: Olay Sistemi
- [x] Faz 5: Ana Oyun Döngüsü
- [x] Faz 6: İçerik Üretimi
- [x] Faz 7: UI Ekranları
- [x] Faz 8: Gelişmiş Sistemler
- [x] Faz 9: Polish & Localization
- [x] Faz 10: Build Ayarları

## Kurulum

1. Unity 2022.3 LTS veya üstünü yükleyin
2. Projeyi Unity Hub'da açın
3. TextMeshPro'yu import edin
4. Android Build Support modülünü yükleyin
5. Player Settings'den package name'i ayarlayın

## Gereksinimler

- Unity 2022.3 LTS
- Android SDK (API 21+)
- TextMeshPro
- .NET Standard 2.1

## Katkıda Bulunma

1. Projeyi fork edin
2. Feature branch oluşturun (`git checkout -b feature/YeniOzellik`)
3. Değişikliklerinizi commit edin
4. Branch'inizi push edin
5. Pull Request açın

## Lisans

Bu proje eğitim amaçlıdır.

## İletişim

Sorularınız için issue açabilirsiniz.
