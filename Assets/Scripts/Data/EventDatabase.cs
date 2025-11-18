using System.Collections.Generic;
using BitLifeTR.Core;

namespace BitLifeTR.Data
{
    /// <summary>
    /// Database of all game events organized by life stage.
    /// </summary>
    public static class EventDatabase
    {
        /// <summary>
        /// Get all events for the game.
        /// </summary>
        public static List<GameEvent> GetAllEvents()
        {
            var events = new List<GameEvent>();

            events.AddRange(GetBabyEvents());
            events.AddRange(GetChildhoodEvents());
            events.AddRange(GetTeenEvents());
            events.AddRange(GetAdultEvents());
            events.AddRange(GetElderlyEvents());
            events.AddRange(GetRandomEvents());

            return events;
        }

        #region Baby Events (0-4)

        public static List<GameEvent> GetBabyEvents()
        {
            return new List<GameEvent>
            {
                new GameEvent
                {
                    Id = "baby_first_steps",
                    Title = "İlk Adımlar",
                    Description = "Bugün ilk adımlarını attın! Ailen çok mutlu.",
                    Category = EventCategory.Aile,
                    MinAge = 1, MaxAge = 2,
                    Choices = new List<EventChoice>
                    {
                        EventChoice.Create("Yürümeye devam et", EventOutcome.StatChange("İlk adımlarını attın!", happiness: 5, health: 2))
                    }
                },
                new GameEvent
                {
                    Id = "baby_first_word",
                    Title = "İlk Kelime",
                    Description = "İlk kelimeni söyledin! Annen ve baban çok heyecanlı.",
                    Category = EventCategory.Aile,
                    MinAge = 1, MaxAge = 2,
                    Choices = new List<EventChoice>
                    {
                        EventChoice.Create("\"Anne\" dedin", EventOutcome.StatChange("İlk kelimen 'Anne' oldu.", happiness: 5, intelligence: 3)),
                        EventChoice.Create("\"Baba\" dedin", EventOutcome.StatChange("İlk kelimen 'Baba' oldu.", happiness: 5, intelligence: 3))
                    }
                },
                new GameEvent
                {
                    Id = "baby_sick",
                    Title = "Hasta Oldun",
                    Description = "Grip oldun ve ateşin çıktı. Annen seni doktora götürdü.",
                    Category = EventCategory.Saglik,
                    MinAge = 0, MaxAge = 4,
                    Choices = new List<EventChoice>
                    {
                        EventChoice.Create("İyileş", EventOutcome.StatChange("Doktor seni iyileştirdi.", health: -5, happiness: -3))
                    }
                },
                new GameEvent
                {
                    Id = "baby_playground",
                    Title = "Parka Gittiniz",
                    Description = "Ailen seni parka götürdü. Salıncaklara bindin ve eğlendin.",
                    Category = EventCategory.Eglence,
                    MinAge = 2, MaxAge = 4,
                    Choices = new List<EventChoice>
                    {
                        EventChoice.Create("Eğlenmeye devam et", EventOutcome.StatChange("Parkta çok eğlendin.", happiness: 8))
                    }
                },
                new GameEvent
                {
                    Id = "baby_pet",
                    Title = "Evcil Hayvan",
                    Description = "Ailen eve bir kedi getirdi. Artık bir arkadaşın var!",
                    Category = EventCategory.Aile,
                    MinAge = 2, MaxAge = 4,
                    Choices = new List<EventChoice>
                    {
                        EventChoice.Create("Kediyle oyna", EventOutcome.StatChange("Kedinle arkadaş oldun.", happiness: 10, karma: 5))
                    }
                },
                new GameEvent
                {
                    Id = "baby_fall",
                    Title = "Düştün",
                    Description = "Koşarken düştün ve dizini sıyırdın. Çok ağladın!",
                    Category = EventCategory.Saglik,
                    MinAge = 1, MaxAge = 4,
                    Choices = new List<EventChoice>
                    {
                        EventChoice.Create("Ağla", EventOutcome.StatChange("Annen seni teselli etti.", health: -2, happiness: -5))
                    }
                },
                new GameEvent
                {
                    Id = "baby_birthday",
                    Title = "Doğum Günü Partisi",
                    Description = "Bugün doğum günün! Ailen sana güzel bir parti düzenledi.",
                    Category = EventCategory.Aile,
                    MinAge = 1, MaxAge = 4,
                    Choices = new List<EventChoice>
                    {
                        EventChoice.Create("Pastayı ye", EventOutcome.StatChange("Harika bir doğum günü geçirdin!", happiness: 15))
                    }
                },
                new GameEvent
                {
                    Id = "baby_nightmare",
                    Title = "Kabus",
                    Description = "Gece kötü bir rüya gördün ve ağlayarak uyandın.",
                    Category = EventCategory.Genel,
                    MinAge = 2, MaxAge = 4,
                    Choices = new List<EventChoice>
                    {
                        EventChoice.Create("Annenin yanına git", EventOutcome.StatChange("Annen seni sakinleştirdi.", happiness: -3))
                    }
                },
                new GameEvent
                {
                    Id = "baby_nursery",
                    Title = "Kreşe Başladın",
                    Description = "Bugün ilk kreş günün. Yeni arkadaşlarla tanışacaksın!",
                    Category = EventCategory.Egitim,
                    MinAge = 3, MaxAge = 4,
                    Choices = new List<EventChoice>
                    {
                        EventChoice.Create("Heyecanlıyım", EventOutcome.StatChange("Kreşte yeni arkadaşlar edindin.", happiness: 5, intelligence: 3)),
                        EventChoice.Create("Gitmek istemiyorum", EventOutcome.StatChange("Kreşe ağlayarak gittin.", happiness: -5))
                    }
                },
                new GameEvent
                {
                    Id = "baby_toy",
                    Title = "Yeni Oyuncak",
                    Description = "Baban sana yeni bir oyuncak aldı!",
                    Category = EventCategory.Aile,
                    MinAge = 1, MaxAge = 4,
                    Choices = new List<EventChoice>
                    {
                        EventChoice.Create("Oyuncakla oyna", EventOutcome.StatChange("Yeni oyuncağını çok sevdin.", happiness: 8))
                    }
                }
            };
        }

        #endregion

        #region Childhood Events (5-11)

        public static List<GameEvent> GetChildhoodEvents()
        {
            return new List<GameEvent>
            {
                new GameEvent
                {
                    Id = "child_bully",
                    Title = "Okul Kabadayısı",
                    Description = "Okulda bir çocuk seni zorbalık yapıyor. Ne yapacaksın?",
                    Category = EventCategory.Egitim,
                    MinAge = 6, MaxAge = 11,
                    Choices = new List<EventChoice>
                    {
                        EventChoice.Create("Öğretmene söyle", EventOutcome.StatChange("Öğretmen duruma müdahale etti.", happiness: 5, karma: 5)),
                        EventChoice.Create("Karşılık ver", EventOutcome.StatChange("Kavga ettin ve ikisini de cezalandırdılar.", happiness: -5, karma: -5)),
                        EventChoice.Create("Görmezden gel", EventOutcome.StatChange("Zorbalık devam etti.", happiness: -10))
                    }
                },
                new GameEvent
                {
                    Id = "child_friend",
                    Title = "Yeni Arkadaş",
                    Description = "Sınıfına yeni bir öğrenci geldi. Arkadaş olmak ister misin?",
                    Category = EventCategory.Iliski,
                    MinAge = 6, MaxAge = 11,
                    Choices = new List<EventChoice>
                    {
                        EventChoice.Create("Arkadaş ol", EventOutcome.StatChange("Yeni bir arkadaş edindin!", happiness: 10)),
                        EventChoice.Create("İlgilenme", EventOutcome.StatChange("Fırsatı kaçırdın.", happiness: -2))
                    }
                },
                new GameEvent
                {
                    Id = "child_exam",
                    Title = "Matematik Sınavı",
                    Description = "Yarın matematik sınavın var. Çalışacak mısın?",
                    Category = EventCategory.Egitim,
                    MinAge = 6, MaxAge = 11,
                    Choices = new List<EventChoice>
                    {
                        EventChoice.CreateWithChance("Çok çalış",
                            (EventOutcome.StatChange("Sınavdan 100 aldın!", happiness: 10, intelligence: 5), 0.7f),
                            (EventOutcome.StatChange("İyi çalıştın ama 85 aldın.", happiness: 5, intelligence: 3), 0.3f)),
                        EventChoice.CreateWithChance("Biraz çalış",
                            (EventOutcome.StatChange("70 aldın, idare eder.", happiness: 2, intelligence: 1), 0.6f),
                            (EventOutcome.StatChange("50 aldın, ailenden azar işittin.", happiness: -5), 0.4f)),
                        EventChoice.Create("Hiç çalışma", EventOutcome.StatChange("Sınavdan kaldın!", happiness: -10, intelligence: -2))
                    }
                },
                new GameEvent
                {
                    Id = "child_sport",
                    Title = "Spor Takımı",
                    Description = "Okul futbol takımı seçmeleri yapılıyor. Katılmak ister misin?",
                    Category = EventCategory.Eglence,
                    MinAge = 7, MaxAge = 11,
                    Choices = new List<EventChoice>
                    {
                        EventChoice.CreateWithChance("Seçmelere katıl",
                            (EventOutcome.StatChange("Takıma seçildin!", happiness: 15, health: 5), 0.5f),
                            (EventOutcome.StatChange("Seçilemedin ama deneme güzeldi.", happiness: -3, health: 2), 0.5f)),
                        EventChoice.Create("İlgilenme", EventOutcome.StatChange("Futbol oynamadın.", happiness: 0))
                    }
                },
                new GameEvent
                {
                    Id = "child_steal",
                    Title = "Markette",
                    Description = "Markette çok istediğin bir şeker gördün ama paran yok. Çalmayı düşünüyor musun?",
                    Category = EventCategory.Suc,
                    MinAge = 7, MaxAge = 11,
                    Choices = new List<EventChoice>
                    {
                        EventChoice.Create("Hayır, çalma", EventOutcome.StatChange("Doğru olanı yaptın.", karma: 10)),
                        EventChoice.CreateWithChance("Şekeri çal",
                            (EventOutcome.StatChange("Yakalanmadan çaldın ama vicdan azabı çektin.", happiness: -5, karma: -15), 0.4f),
                            (EventOutcome.StatChange("Yakalandın! Ailen çok kızdı.", happiness: -20, karma: -10), 0.6f))
                    }
                },
                new GameEvent
                {
                    Id = "child_pet_death",
                    Title = "Evcil Hayvan Kaybı",
                    Description = "Sevgili evcil hayvanın öldü. Çok üzgünsün.",
                    Category = EventCategory.Aile,
                    MinAge = 6, MaxAge = 11,
                    Choices = new List<EventChoice>
                    {
                        EventChoice.Create("Yas tut", EventOutcome.StatChange("Evcil hayvanını kaybettin.", happiness: -20))
                    }
                },
                new GameEvent
                {
                    Id = "child_talent",
                    Title = "Yetenek Keşfi",
                    Description = "Resim dersinde öğretmenin senin çok yetenekli olduğunu söyledi!",
                    Category = EventCategory.Egitim,
                    MinAge = 6, MaxAge = 11,
                    Choices = new List<EventChoice>
                    {
                        EventChoice.Create("Resim yapmaya devam et", EventOutcome.StatChange("Sanatsal yeteneğini geliştirdin.", happiness: 10, intelligence: 3)),
                        EventChoice.Create("İlgilenme", EventOutcome.StatChange("Fırsatı kaçırdın.", happiness: 0))
                    }
                },
                new GameEvent
                {
                    Id = "child_camping",
                    Title = "Kamp",
                    Description = "Ailen seni yaz kampına göndermek istiyor.",
                    Category = EventCategory.Eglence,
                    MinAge = 8, MaxAge = 11,
                    Choices = new List<EventChoice>
                    {
                        EventChoice.Create("Kampa git", EventOutcome.StatChange("Kampta harika vakit geçirdin!", happiness: 15, health: 5)),
                        EventChoice.Create("Evde kal", EventOutcome.StatChange("Evde kaldın, biraz sıkıldın.", happiness: -5))
                    }
                },
                new GameEvent
                {
                    Id = "child_homework",
                    Title = "Ödev Yardımı",
                    Description = "Arkadaşın senden ödev kopya istiyor. Ne yapacaksın?",
                    Category = EventCategory.Egitim,
                    MinAge = 7, MaxAge = 11,
                    Choices = new List<EventChoice>
                    {
                        EventChoice.Create("Yardım et (açıklayarak)", EventOutcome.StatChange("Arkadaşına yardım ettin.", happiness: 5, karma: 5)),
                        EventChoice.Create("Kopya ver", EventOutcome.StatChange("Kopya verdin.", karma: -5)),
                        EventChoice.Create("Reddet", EventOutcome.StatChange("Arkadaşın kızdı.", happiness: -3, karma: 5))
                    }
                },
                new GameEvent
                {
                    Id = "child_grandparents",
                    Title = "Büyükanne ve Büyükbaba",
                    Description = "Yaz tatilinde büyükanne ve büyükbabanı ziyaret edeceksin.",
                    Category = EventCategory.Aile,
                    MinAge = 5, MaxAge = 11,
                    Choices = new List<EventChoice>
                    {
                        EventChoice.Create("Onlarla vakit geçir", EventOutcome.StatChange("Harika bir yaz geçirdin!", happiness: 12, karma: 5))
                    }
                },
                new GameEvent
                {
                    Id = "child_music",
                    Title = "Müzik Dersi",
                    Description = "Annen seni piyano dersine yazdırmak istiyor.",
                    Category = EventCategory.Egitim,
                    MinAge = 6, MaxAge = 11,
                    Choices = new List<EventChoice>
                    {
                        EventChoice.Create("Derslere git", EventOutcome.StatChange("Piyano çalmayı öğrendin!", intelligence: 5, happiness: 5)),
                        EventChoice.Create("İstemiyorum", EventOutcome.StatChange("Annen biraz üzüldü.", happiness: -3))
                    }
                },
                new GameEvent
                {
                    Id = "child_accident",
                    Title = "Bisiklet Kazası",
                    Description = "Bisiklet sürerken düştün ve kolunu incittin!",
                    Category = EventCategory.Saglik,
                    MinAge = 6, MaxAge = 11,
                    Choices = new List<EventChoice>
                    {
                        EventChoice.Create("Hastaneye git", EventOutcome.StatChange("Kolun alçıya alındı.", health: -10, happiness: -8))
                    }
                }
            };
        }

        #endregion

        #region Teen Events (12-17)

        public static List<GameEvent> GetTeenEvents()
        {
            return new List<GameEvent>
            {
                new GameEvent
                {
                    Id = "teen_first_love",
                    Title = "İlk Aşk",
                    Description = "Okulda birinden hoşlanıyorsun. Duygularını belli edecek misin?",
                    Category = EventCategory.Iliski,
                    MinAge = 13, MaxAge = 17,
                    Choices = new List<EventChoice>
                    {
                        EventChoice.CreateWithChance("Açıl",
                            (EventOutcome.StatChange("O da senden hoşlanıyormuş! Çıkmaya başladınız.", happiness: 25), 0.4f),
                            (EventOutcome.StatChange("Red edildin. Çok üzgünsün.", happiness: -15), 0.6f)),
                        EventChoice.Create("Sessiz kal", EventOutcome.StatChange("Duygularını içinde tuttun.", happiness: -5))
                    }
                },
                new GameEvent
                {
                    Id = "teen_smoking",
                    Title = "Sigara Teklifi",
                    Description = "Arkadaşların sana sigara teklif ediyor. \"Bir kere dene\" diyorlar.",
                    Category = EventCategory.Saglik,
                    MinAge = 13, MaxAge = 17,
                    Choices = new List<EventChoice>
                    {
                        EventChoice.Create("Reddet", EventOutcome.StatChange("Hayır dedin. Doğru kararı verdin.", karma: 10, health: 5)),
                        EventChoice.Create("Dene", EventOutcome.StatChange("Sigara içtin. Pek hoşuna gitmedi ama...", health: -5, karma: -5))
                    }
                },
                new GameEvent
                {
                    Id = "teen_part_time",
                    Title = "Part-time İş",
                    Description = "Mahallenizdeki market yarı zamanlı eleman arıyor.",
                    Category = EventCategory.Kariyer,
                    MinAge = 15, MaxAge = 17,
                    Choices = new List<EventChoice>
                    {
                        EventChoice.Create("Başvur", new EventOutcome
                        {
                            Description = "İşe alındın! Artık harçlığın var.",
                            MoneyChange = 500,
                            StatChanges = new StatModifierData { Happiness = 10 }
                        }),
                        EventChoice.Create("İlgilenme", EventOutcome.StatChange("Fırsatı geçtin.", happiness: 0))
                    }
                },
                new GameEvent
                {
                    Id = "teen_party",
                    Title = "Parti Daveti",
                    Description = "Arkadaşların seni bir ev partisine davet ediyor. Ailen bilmiyor.",
                    Category = EventCategory.Eglence,
                    MinAge = 15, MaxAge = 17,
                    Choices = new List<EventChoice>
                    {
                        EventChoice.CreateWithChance("Gizlice git",
                            (EventOutcome.StatChange("Harika bir gece geçirdin!", happiness: 20), 0.6f),
                            (EventOutcome.StatChange("Ailen öğrendi ve ceza yedin!", happiness: -15), 0.4f)),
                        EventChoice.Create("Ailene söyle ve izin iste", EventOutcome.StatChange("İzin vermediler.", happiness: -10)),
                        EventChoice.Create("Gitme", EventOutcome.StatChange("Evde kaldın.", happiness: -5))
                    }
                },
                new GameEvent
                {
                    Id = "teen_fight",
                    Title = "Kavga",
                    Description = "Okulda biriyle tartışma büyüdü ve kavgaya dönüşmek üzere.",
                    Category = EventCategory.Genel,
                    MinAge = 13, MaxAge = 17,
                    Choices = new List<EventChoice>
                    {
                        EventChoice.Create("Kavga et", EventOutcome.StatChange("Kavga ettin ve disipline gittin.", happiness: -10, karma: -10)),
                        EventChoice.Create("Sakinleş", EventOutcome.StatChange("Olgun davrandın.", karma: 10, intelligence: 2))
                    }
                },
                new GameEvent
                {
                    Id = "teen_social_media",
                    Title = "Sosyal Medya",
                    Description = "Sosyal medyada çok takipçi kazanmaya başladın. İçerik üreticisi olabilirsin!",
                    Category = EventCategory.Kariyer,
                    MinAge = 14, MaxAge = 17,
                    Choices = new List<EventChoice>
                    {
                        EventChoice.Create("İçerik üretmeye devam et", EventOutcome.StatChange("Sosyal medyada popülerleştin.", fame: 10, happiness: 10)),
                        EventChoice.Create("Bırak", EventOutcome.StatChange("Sosyal medyayı bıraktın.", happiness: -3))
                    }
                },
                new GameEvent
                {
                    Id = "teen_cheating",
                    Title = "Kopya",
                    Description = "Sınavda kopya çekme fırsatın var. Çok zor bir sınav.",
                    Category = EventCategory.Egitim,
                    MinAge = 13, MaxAge = 17,
                    Choices = new List<EventChoice>
                    {
                        EventChoice.Create("Kopya çekme", EventOutcome.StatChange("Dürüst davrandın.", karma: 10)),
                        EventChoice.CreateWithChance("Kopya çek",
                            (EventOutcome.StatChange("Yakalanmadan geçtin ama vicdan azabı çektin.", intelligence: 5, karma: -15), 0.5f),
                            (EventOutcome.StatChange("Yakalandın! Sınavdan sıfır aldın.", happiness: -20, karma: -10), 0.5f))
                    }
                },
                new GameEvent
                {
                    Id = "teen_driving",
                    Title = "Ehliyet",
                    Description = "18 yaşına geldin ve ehliyet alabilirsin.",
                    Category = EventCategory.Genel,
                    MinAge = 17, MaxAge = 17,
                    Choices = new List<EventChoice>
                    {
                        EventChoice.CreateWithChance("Ehliyet sınavına gir",
                            (new EventOutcome { Description = "Ehliyetini aldın!", MoneyChange = -1500, StatChanges = new StatModifierData { Happiness = 15 } }, 0.7f),
                            (new EventOutcome { Description = "Sınavda kaldın.", MoneyChange = -1500, StatChanges = new StatModifierData { Happiness = -10 } }, 0.3f)),
                        EventChoice.Create("Şimdilik gerek yok", EventOutcome.StatChange("Ehliyet almadın.", happiness: 0))
                    }
                },
                new GameEvent
                {
                    Id = "teen_volunteer",
                    Title = "Gönüllü Çalışma",
                    Description = "Hayvan barınağı gönüllü arıyor. Katılmak ister misin?",
                    Category = EventCategory.Genel,
                    MinAge = 14, MaxAge = 17,
                    Choices = new List<EventChoice>
                    {
                        EventChoice.Create("Gönüllü ol", EventOutcome.StatChange("Hayvanlara yardım ettin, harika hissediyorsun!", happiness: 10, karma: 15)),
                        EventChoice.Create("Vaktim yok", EventOutcome.StatChange("Gönüllü olmadın.", happiness: 0))
                    }
                },
                new GameEvent
                {
                    Id = "teen_course",
                    Title = "Dershane",
                    Description = "Ailen seni üniversite sınavı için dershaneye göndermek istiyor.",
                    Category = EventCategory.Egitim,
                    MinAge = 15, MaxAge = 17,
                    Choices = new List<EventChoice>
                    {
                        EventChoice.Create("Dershaneye git", new EventOutcome
                        {
                            Description = "Dershaneye başladın, çok çalışıyorsun.",
                            MoneyChange = -5000,
                            StatChanges = new StatModifierData { Intelligence = 8, Happiness = -5 }
                        }),
                        EventChoice.Create("Evde çalış", EventOutcome.StatChange("Kendi başına çalışmaya karar verdin.", intelligence: 3))
                    }
                },
                new GameEvent
                {
                    Id = "teen_breakup",
                    Title = "Ayrılık",
                    Description = "Sevgilin seninle ayrılmak istiyor.",
                    Category = EventCategory.Iliski,
                    MinAge = 14, MaxAge = 17,
                    Conditions = new List<EventCondition> { EventCondition.HasRelationship(RelationType.Sevgili) },
                    Choices = new List<EventChoice>
                    {
                        EventChoice.Create("Kabul et", EventOutcome.StatChange("Ayrıldınız. Çok üzgünsün.", happiness: -20)),
                        EventChoice.Create("Yalvar", EventOutcome.StatChange("Yalvardın ama işe yaramadı.", happiness: -25))
                    }
                },
                new GameEvent
                {
                    Id = "teen_prom",
                    Title = "Mezuniyet Balosu",
                    Description = "Lise mezuniyet balonu yaklaşıyor!",
                    Category = EventCategory.Eglence,
                    MinAge = 17, MaxAge = 17,
                    Choices = new List<EventChoice>
                    {
                        EventChoice.Create("Baloya git", new EventOutcome
                        {
                            Description = "Baloda harika vakit geçirdin!",
                            MoneyChange = -500,
                            StatChanges = new StatModifierData { Happiness = 20 }
                        }),
                        EventChoice.Create("Gitme", EventOutcome.StatChange("Baloyu kaçırdın.", happiness: -10))
                    }
                }
            };
        }

        #endregion

        #region Adult Events (18+)

        public static List<GameEvent> GetAdultEvents()
        {
            return new List<GameEvent>
            {
                new GameEvent
                {
                    Id = "adult_job_offer",
                    Title = "İş Teklifi",
                    Description = "Bir şirketten iş teklifi aldın. Maaş güzel görünüyor.",
                    Category = EventCategory.Kariyer,
                    MinAge = 18, MaxAge = 60,
                    Conditions = new List<EventCondition> { EventCondition.HasJob(false) },
                    Choices = new List<EventChoice>
                    {
                        EventChoice.Create("Kabul et", new EventOutcome
                        {
                            Description = "İşe başladın!",
                            StatChanges = new StatModifierData { Happiness = 15 },
                            Effects = new OutcomeEffects { GetJob = true, JobName = "Ofis Çalışanı" }
                        }),
                        EventChoice.Create("Reddet", EventOutcome.StatChange("Teklifi reddeddin.", happiness: -5))
                    }
                },
                new GameEvent
                {
                    Id = "adult_promotion",
                    Title = "Terfi",
                    Description = "Patronun seni terfi ettirmek istiyor!",
                    Category = EventCategory.Kariyer,
                    MinAge = 22, MaxAge = 60,
                    Conditions = new List<EventCondition> { EventCondition.HasJob(true) },
                    Choices = new List<EventChoice>
                    {
                        EventChoice.Create("Kabul et", new EventOutcome
                        {
                            Description = "Terfi aldın ve maaşın arttı!",
                            MoneyChange = 2000,
                            StatChanges = new StatModifierData { Happiness = 20, Fame = 5 }
                        })
                    }
                },
                new GameEvent
                {
                    Id = "adult_marriage_proposal",
                    Title = "Evlilik Teklifi",
                    Description = "Sevgilin sana evlilik teklif ediyor!",
                    Category = EventCategory.Iliski,
                    MinAge = 20, MaxAge = 50,
                    Conditions = new List<EventCondition>
                    {
                        EventCondition.IsMarried(false),
                        EventCondition.HasRelationship(RelationType.Sevgili)
                    },
                    Choices = new List<EventChoice>
                    {
                        EventChoice.Create("Evet!", new EventOutcome
                        {
                            Description = "Evlendiniz! Ne mutlu size!",
                            StatChanges = new StatModifierData { Happiness = 30 },
                            Effects = new OutcomeEffects { GetMarried = true }
                        }),
                        EventChoice.Create("Hayır", EventOutcome.StatChange("Teklifi reddeddin. İlişkiniz bitti.", happiness: -20))
                    }
                },
                new GameEvent
                {
                    Id = "adult_baby",
                    Title = "Bebek",
                    Description = "Eşinle bir bebeğiniz oldu!",
                    Category = EventCategory.Aile,
                    MinAge = 20, MaxAge = 45,
                    Conditions = new List<EventCondition> { EventCondition.IsMarried(true) },
                    Choices = new List<EventChoice>
                    {
                        EventChoice.Create("Kutla", new EventOutcome
                        {
                            Description = "Bebeğiniz dünyaya geldi! Artık anne/babasın!",
                            StatChanges = new StatModifierData { Happiness = 25, Karma = 10 },
                            Effects = new OutcomeEffects { HaveChild = true }
                        })
                    }
                },
                new GameEvent
                {
                    Id = "adult_house",
                    Title = "Ev Alma",
                    Description = "Güzel bir ev gördün ve satın almak istiyorsun.",
                    Category = EventCategory.Finans,
                    MinAge = 25, MaxAge = 60,
                    Conditions = new List<EventCondition> { EventCondition.Money(ComparisonOperator.GreaterOrEqual, 100000) },
                    Choices = new List<EventChoice>
                    {
                        EventChoice.Create("Satın al", new EventOutcome
                        {
                            Description = "Ev aldın! Artık ev sahibisin!",
                            MoneyChange = -150000,
                            StatChanges = new StatModifierData { Happiness = 30 }
                        }),
                        EventChoice.Create("Vazgeç", EventOutcome.StatChange("Evi almadın.", happiness: 0))
                    }
                },
                new GameEvent
                {
                    Id = "adult_car",
                    Title = "Araba Alma",
                    Description = "Güzel bir araba gördün. Satın alacak mısın?",
                    Category = EventCategory.Finans,
                    MinAge = 18, MaxAge = 70,
                    Conditions = new List<EventCondition> { EventCondition.Money(ComparisonOperator.GreaterOrEqual, 30000) },
                    Choices = new List<EventChoice>
                    {
                        EventChoice.Create("Satın al", new EventOutcome
                        {
                            Description = "Araba aldın!",
                            MoneyChange = -40000,
                            StatChanges = new StatModifierData { Happiness = 20, Looks = 5 }
                        }),
                        EventChoice.Create("Vazgeç", EventOutcome.StatChange("Arabayı almadın.", happiness: 0))
                    }
                },
                new GameEvent
                {
                    Id = "adult_fired",
                    Title = "İşten Çıkarılma",
                    Description = "Şirket küçülüyor ve maalesef işten çıkarıldın.",
                    Category = EventCategory.Kariyer,
                    MinAge = 20, MaxAge = 60,
                    Conditions = new List<EventCondition> { EventCondition.HasJob(true) },
                    Choices = new List<EventChoice>
                    {
                        EventChoice.Create("Kabul et", new EventOutcome
                        {
                            Description = "İşini kaybettin.",
                            StatChanges = new StatModifierData { Happiness = -25 },
                            Effects = new OutcomeEffects { LoseJob = true }
                        })
                    }
                },
                new GameEvent
                {
                    Id = "adult_lottery",
                    Title = "Piyango",
                    Description = "Piyango bileti almak ister misin?",
                    Category = EventCategory.Finans,
                    MinAge = 18, MaxAge = 100,
                    Choices = new List<EventChoice>
                    {
                        EventChoice.CreateWithChance("Bilet al (50 TL)",
                            (new EventOutcome { Description = "BÜYÜK İKRAMİYE! Zengin oldun!", MoneyChange = 1000000, StatChanges = new StatModifierData { Happiness = 50, Fame = 20 } }, 0.001f),
                            (new EventOutcome { Description = "Küçük ikramiye kazandın!", MoneyChange = 500, StatChanges = new StatModifierData { Happiness = 10 } }, 0.05f),
                            (new EventOutcome { Description = "Kaybettin.", MoneyChange = -50, StatChanges = new StatModifierData { Happiness = -2 } }, 0.949f)),
                        EventChoice.Create("Alma", EventOutcome.StatChange("Piyango oynamadın.", happiness: 0))
                    }
                },
                new GameEvent
                {
                    Id = "adult_divorce",
                    Title = "Boşanma",
                    Description = "Evliliğiniz iyi gitmiyor. Boşanmayı düşünüyor musun?",
                    Category = EventCategory.Iliski,
                    MinAge = 20, MaxAge = 80,
                    Conditions = new List<EventCondition> { EventCondition.IsMarried(true) },
                    Choices = new List<EventChoice>
                    {
                        EventChoice.Create("Boşan", new EventOutcome
                        {
                            Description = "Boşandınız.",
                            MoneyChange = -20000,
                            StatChanges = new StatModifierData { Happiness = -30 },
                            Effects = new OutcomeEffects { GetDivorced = true }
                        }),
                        EventChoice.Create("Evliliğe devam et", EventOutcome.StatChange("Evliliğe devam ediyorsunuz.", happiness: -10))
                    }
                },
                new GameEvent
                {
                    Id = "adult_health_scare",
                    Title = "Sağlık Sorunu",
                    Description = "Rutin kontrolde doktor bir sorun buldu. Tedavi gerekiyor.",
                    Category = EventCategory.Saglik,
                    MinAge = 30, MaxAge = 80,
                    Choices = new List<EventChoice>
                    {
                        EventChoice.Create("Tedavi ol", new EventOutcome
                        {
                            Description = "Tedavi oldun ve iyileştin.",
                            MoneyChange = -5000,
                            StatChanges = new StatModifierData { Health = 10, Happiness = -5 }
                        }),
                        EventChoice.Create("Görmezden gel", EventOutcome.StatChange("Tedaviyi reddeddin. Risk alıyorsun.", health: -20))
                    }
                },
                new GameEvent
                {
                    Id = "adult_inheritance",
                    Title = "Miras",
                    Description = "Uzak bir akrabandan miras kaldı!",
                    Category = EventCategory.Finans,
                    MinAge = 25, MaxAge = 80,
                    Choices = new List<EventChoice>
                    {
                        EventChoice.Create("Mirası kabul et", new EventOutcome
                        {
                            Description = "Miras aldın!",
                            MoneyChange = 50000,
                            StatChanges = new StatModifierData { Happiness = 15 }
                        })
                    }
                },
                new GameEvent
                {
                    Id = "adult_startup",
                    Title = "Girişim",
                    Description = "Bir iş fikrin var. Kendi şirketini kurmak ister misin?",
                    Category = EventCategory.Kariyer,
                    MinAge = 25, MaxAge = 50,
                    Conditions = new List<EventCondition> { EventCondition.Money(ComparisonOperator.GreaterOrEqual, 50000) },
                    Choices = new List<EventChoice>
                    {
                        EventChoice.CreateWithChance("Şirket kur",
                            (new EventOutcome { Description = "Şirketin başarılı oldu! Zenginleştin!", MoneyChange = 200000, StatChanges = new StatModifierData { Happiness = 30, Fame = 20, Intelligence = 5 } }, 0.3f),
                            (new EventOutcome { Description = "Şirket battı. Parasını kaybettin.", MoneyChange = -50000, StatChanges = new StatModifierData { Happiness = -25 } }, 0.7f)),
                        EventChoice.Create("Vazgeç", EventOutcome.StatChange("Girişim yapmadın.", happiness: 0))
                    }
                }
            };
        }

        #endregion

        #region Elderly Events (65+)

        public static List<GameEvent> GetElderlyEvents()
        {
            return new List<GameEvent>
            {
                new GameEvent
                {
                    Id = "elderly_grandchild",
                    Title = "Torun",
                    Description = "Bir torunun oldu! Ne güzel bir haber!",
                    Category = EventCategory.Aile,
                    MinAge = 50, MaxAge = 90,
                    Conditions = new List<EventCondition> { EventCondition.HasChildren(true) },
                    Choices = new List<EventChoice>
                    {
                        EventChoice.Create("Kutla", EventOutcome.StatChange("Torun sahibi oldun!", happiness: 25, karma: 10))
                    }
                },
                new GameEvent
                {
                    Id = "elderly_memory",
                    Title = "Anılar",
                    Description = "Eski fotoğraflara bakıyorsun. Güzel anılar canlanıyor.",
                    Category = EventCategory.Genel,
                    MinAge = 65, MaxAge = 100,
                    Choices = new List<EventChoice>
                    {
                        EventChoice.Create("Nostaljiye dal", EventOutcome.StatChange("Güzel anıları hatırladın.", happiness: 10))
                    }
                },
                new GameEvent
                {
                    Id = "elderly_health",
                    Title = "Sağlık Kontrolü",
                    Description = "Yaşın ilerledikçe sağlık kontrolleri önem kazanıyor.",
                    Category = EventCategory.Saglik,
                    MinAge = 65, MaxAge = 100,
                    Choices = new List<EventChoice>
                    {
                        EventChoice.Create("Kontrole git", new EventOutcome
                        {
                            Description = "Düzenli kontrol yaptırdın.",
                            MoneyChange = -500,
                            StatChanges = new StatModifierData { Health = 5 }
                        }),
                        EventChoice.Create("Gitme", EventOutcome.StatChange("Kontrolü atladın. Risk alıyorsun.", health: -5))
                    }
                },
                new GameEvent
                {
                    Id = "elderly_hobby",
                    Title = "Hobi",
                    Description = "Emeklilikte bir hobi edinmek istiyorsun.",
                    Category = EventCategory.Eglence,
                    MinAge = 65, MaxAge = 100,
                    Choices = new List<EventChoice>
                    {
                        EventChoice.Create("Bahçıvanlık", EventOutcome.StatChange("Bahçıvanlık yapıyorsun.", happiness: 10, health: 3)),
                        EventChoice.Create("Satranç", EventOutcome.StatChange("Satranç oynuyorsun.", happiness: 8, intelligence: 5)),
                        EventChoice.Create("El işi", EventOutcome.StatChange("El işi yapıyorsun.", happiness: 10))
                    }
                },
                new GameEvent
                {
                    Id = "elderly_will",
                    Title = "Vasiyet",
                    Description = "Vasiyetini yazmayı düşünüyor musun?",
                    Category = EventCategory.Finans,
                    MinAge = 70, MaxAge = 100,
                    Choices = new List<EventChoice>
                    {
                        EventChoice.Create("Vasiyet yaz", EventOutcome.StatChange("Vasiyetini yazdın.", karma: 5)),
                        EventChoice.Create("Gerek yok", EventOutcome.StatChange("Vasiyet yazmadın.", happiness: 0))
                    }
                }
            };
        }

        #endregion

        #region Random Events

        public static List<GameEvent> GetRandomEvents()
        {
            return new List<GameEvent>
            {
                new GameEvent
                {
                    Id = "random_wallet",
                    Title = "Kayıp Cüzdan",
                    Description = "Yolda içi para dolu bir cüzdan buldun. Ne yapacaksın?",
                    Category = EventCategory.Random,
                    MinAge = 10, MaxAge = 80,
                    Choices = new List<EventChoice>
                    {
                        EventChoice.Create("Sahibine ulaştır", EventOutcome.StatChange("Sahibi sana teşekkür etti!", happiness: 10, karma: 15)),
                        EventChoice.Create("Parayı al", new EventOutcome { Description = "Parayı aldın.", MoneyChange = 200, StatChanges = new StatModifierData { Karma = -20 } }),
                        EventChoice.Create("Polise ver", EventOutcome.StatChange("Doğru olanı yaptın.", karma: 10))
                    }
                },
                new GameEvent
                {
                    Id = "random_weather",
                    Title = "Fırtına",
                    Description = "Şiddetli bir fırtına çıktı ve evde mahsur kaldın.",
                    Category = EventCategory.Random,
                    MinAge = 5, MaxAge = 100,
                    Choices = new List<EventChoice>
                    {
                        EventChoice.Create("Evde kal", EventOutcome.StatChange("Fırtına geçene kadar evde bekledin.", happiness: -3))
                    }
                },
                new GameEvent
                {
                    Id = "random_celebrity",
                    Title = "Ünlü Karşılaşması",
                    Description = "Sokakta ünlü biriyle karşılaştın!",
                    Category = EventCategory.Random,
                    MinAge = 10, MaxAge = 80,
                    Choices = new List<EventChoice>
                    {
                        EventChoice.Create("Fotoğraf çek", EventOutcome.StatChange("Ünlüyle fotoğraf çektirdin!", happiness: 15)),
                        EventChoice.Create("Rahatsız etme", EventOutcome.StatChange("Saygılı davrandın.", karma: 5))
                    }
                },
                new GameEvent
                {
                    Id = "random_traffic",
                    Title = "Trafik Kazası",
                    Description = "Küçük bir trafik kazası geçirdin.",
                    Category = EventCategory.Random,
                    MinAge = 18, MaxAge = 100,
                    Choices = new List<EventChoice>
                    {
                        EventChoice.Create("Sigortayı ara", new EventOutcome
                        {
                            Description = "Sigorta hasarı karşıladı.",
                            MoneyChange = -1000,
                            StatChanges = new StatModifierData { Health = -5, Happiness = -10 }
                        })
                    }
                },
                new GameEvent
                {
                    Id = "random_charity",
                    Title = "Yardım Kampanyası",
                    Description = "Bir yardım kuruluşu bağış istiyor.",
                    Category = EventCategory.Random,
                    MinAge = 18, MaxAge = 100,
                    Choices = new List<EventChoice>
                    {
                        EventChoice.Create("Bağış yap", new EventOutcome
                        {
                            Description = "Bağış yaptın. İyi hissediyorsun.",
                            MoneyChange = -500,
                            StatChanges = new StatModifierData { Happiness = 10, Karma = 15 }
                        }),
                        EventChoice.Create("Geç", EventOutcome.StatChange("Bağış yapmadın.", karma: -3))
                    }
                },
                new GameEvent
                {
                    Id = "random_compliment",
                    Title = "İltifat",
                    Description = "Birisi sana güzel bir iltifat etti!",
                    Category = EventCategory.Random,
                    MinAge = 10, MaxAge = 100,
                    Choices = new List<EventChoice>
                    {
                        EventChoice.Create("Teşekkür et", EventOutcome.StatChange("Günün güzelleşti!", happiness: 8))
                    }
                },
                new GameEvent
                {
                    Id = "random_scam",
                    Title = "Dolandırıcılık",
                    Description = "Biri seni telefonda dolandırmaya çalışıyor!",
                    Category = EventCategory.Random,
                    MinAge = 18, MaxAge = 100,
                    Choices = new List<EventChoice>
                    {
                        EventChoice.Create("Telefonu kapat", EventOutcome.StatChange("Akıllıca davrandın.", intelligence: 2)),
                        EventChoice.Create("Dinle", new EventOutcome
                        {
                            Description = "Dolandırıldın!",
                            MoneyChange = -5000,
                            StatChanges = new StatModifierData { Happiness = -20 }
                        })
                    }
                }
            };
        }

        #endregion
    }
}
