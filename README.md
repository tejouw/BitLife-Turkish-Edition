# BitLife Turkish Edition

BitLife tarzında, tamamen Türkçe ve Türk kültürüne uyarlanmış metin tabanlı hayat simülasyonu oyunu.

## Özellikler

- **Tamamen Türkçe** - Tüm içerik Türkçe olarak yazılmış
- **Türk Kültürü** - YKS sınavı, askerlik, Türk isimleri ve şehirleri
- **Kod Tabanlı UI** - %100 kod ile oluşturulmuş UI, manuel atama yok
- **Genişletilebilir** - Modüler yapı ile kolay içerik ekleme
- **Kaydet/Yükle** - JSON tabanlı save sistemi

## Teknik Detaylar

- **Platform**: Android (Unity)
- **Dil**: C#
- **Unity Version**: 2022.3 LTS önerilir
- **Minimum API**: Android 5.0 (API 21)

## Proje Yapısı

```
Assets/
├── Scripts/
│   ├── Core/           # GameManager, EventBus, Bootstrap
│   ├── UI/             # UIFactory, UIScreen, Screens
│   ├── Data/           # CharacterData, GameEvent, EventDatabase
│   ├── Systems/        # StatSystem, SaveManager, CareerSystem
│   ├── Utils/          # Extensions, RandomHelper, ObjectPool
│   └── Localization/   # (Gelecekte)
├── Editor/             # Build configuration
├── Resources/          # Runtime resources
└── StreamingAssets/    # JSON data files
```

## Oyun Sistemleri

### Stat Sistemi
- Sağlık, Mutluluk, Zeka, Görünüm
- Para, Şöhret, Karma

### Yaşam Dönemleri
- Bebeklik (0-4)
- Çocukluk (5-11)
- Ergenlik (12-17)
- Yetişkinlik (18+)
- Yaşlılık (65+)

### Türkiye'ye Özgü
- İlkokul/Ortaokul/Lise/Üniversite
- YKS Sınavı
- Askerlik / Bedelli
- Türk meslekleri ve şirketleri

## Build

### Android APK
1. Unity'de projeyi açın
2. `Build > Configure Android Settings`
3. `Build > Build Android APK`

### Play Store (AAB)
1. `Build > Build Android AAB`

## Geliştirme

### Yeni Olay Ekleme
`EventDatabase.cs` dosyasına yeni `GameEvent` ekleyin.

### Yeni Meslek Ekleme
`JobDatabase.cs` dosyasına yeni `JobData` ekleyin.

### UI Oluşturma
Tüm UI elementleri `UIFactory` kullanılarak oluşturulmalı.

## Lisans

MIT License

## Katkıda Bulunma

Pull request'ler kabul edilir. Büyük değişiklikler için önce issue açın.
