using UnityEngine;
using System.Collections.Generic;
using System.Linq;
using BitLifeTR.Core;
using BitLifeTR.Data;

namespace BitLifeTR.Systems
{
    /// <summary>
    /// Evcil hayvan yönetim sistemi
    /// </summary>
    public class PetSystem : MonoBehaviour
    {
        // Hayvan fiyatları
        private readonly Dictionary<PetType, float> petPrices = new Dictionary<PetType, float>
        {
            { PetType.Dog, 3000f },
            { PetType.Cat, 2000f },
            { PetType.Bird, 500f },
            { PetType.Fish, 100f },
            { PetType.Hamster, 300f },
            { PetType.Rabbit, 800f },
            { PetType.Turtle, 1000f },
            { PetType.Parrot, 5000f }
        };

        // Veteriner maliyetleri
        private readonly Dictionary<PetType, float> vetCosts = new Dictionary<PetType, float>
        {
            { PetType.Dog, 500f },
            { PetType.Cat, 400f },
            { PetType.Bird, 200f },
            { PetType.Fish, 50f },
            { PetType.Hamster, 150f },
            { PetType.Rabbit, 250f },
            { PetType.Turtle, 300f },
            { PetType.Parrot, 350f }
        };

        // Yıllık bakım maliyetleri
        private readonly Dictionary<PetType, float> yearlyCosts = new Dictionary<PetType, float>
        {
            { PetType.Dog, 2000f },
            { PetType.Cat, 1500f },
            { PetType.Bird, 300f },
            { PetType.Fish, 200f },
            { PetType.Hamster, 400f },
            { PetType.Rabbit, 600f },
            { PetType.Turtle, 400f },
            { PetType.Parrot, 800f }
        };

        public void ProcessYearlyPets(CharacterData character)
        {
            foreach (var pet in character.Pets.ToList())
            {
                if (!pet.IsAlive) continue;

                // Yaşlandır
                pet.Age++;

                // Yıllık bakım maliyeti
                float cost = yearlyCosts[pet.Type];
                GameManager.Instance.StatSystem.ModifyStat(character, StatType.Money, -cost);

                // Sağlık ve mutluluk düşüşü
                pet.Health -= Random.Range(5f, 15f);
                pet.Happiness -= Random.Range(3f, 10f);

                // İlişki seviyesi rastgele değişir
                pet.RelationshipLevel += Random.Range(-5f, 5f);
                pet.RelationshipLevel = Mathf.Clamp(pet.RelationshipLevel, 0, 100);

                // Ölüm kontrolü
                if (CheckPetDeath(pet))
                {
                    pet.IsAlive = false;
                    character.AddLifeEvent($"{pet.Name} adlı {pet.GetTypeName()} hayatını kaybetti.");

                    // Üzüntü - ilişki seviyesine göre
                    float sadness = -10f - (pet.RelationshipLevel / 10f);
                    GameManager.Instance.StatSystem.ModifyStat(character, StatType.Happiness, sadness);
                }
                else
                {
                    // Mutluluk bonusu - evcil hayvan sahibi olmaktan
                    float happinessBonus = pet.RelationshipLevel / 20f;
                    GameManager.Instance.StatSystem.ModifyStat(character, StatType.Happiness, happinessBonus);
                }
            }
        }

        public bool AdoptPet(CharacterData character, PetType type, string name)
        {
            float price = petPrices[type];

            // Para kontrolü
            if (character.Stats.Money < price)
            {
                Debug.Log("Yeterli para yok!");
                return false;
            }

            // Yaş kontrolü (çocuklar sahiplenemez)
            if (character.Age < 10)
            {
                Debug.Log("Evcil hayvan sahiplenmek için çok küçüksün!");
                return false;
            }

            // Para kes
            GameManager.Instance.StatSystem.ModifyStat(character, StatType.Money, -price);

            // Hayvanı oluştur
            PetData pet = new PetData(name, type)
            {
                YearAdopted = character.BirthYear + character.Age
            };

            character.Pets.Add(pet);
            character.AddLifeEvent($"{name} adlı bir {pet.GetTypeName()} sahiplendin! (₺{price:N0})");

            // Mutluluk artışı
            GameManager.Instance.StatSystem.ModifyStat(character, StatType.Happiness, 15f);
            GameManager.Instance.StatSystem.ModifyStat(character, StatType.Karma, 5f);

            return true;
        }

        public void PlayWithPet(CharacterData character, string petId)
        {
            var pet = character.Pets.FirstOrDefault(p => p.Id == petId);
            if (pet == null || !pet.IsAlive) return;

            // Mutluluk ve ilişki artışı
            pet.Happiness = Mathf.Min(100, pet.Happiness + Random.Range(5f, 15f));
            pet.RelationshipLevel = Mathf.Min(100, pet.RelationshipLevel + Random.Range(3f, 8f));

            // Karakter mutluluğu
            GameManager.Instance.StatSystem.ModifyStat(character, StatType.Happiness, Random.Range(3f, 8f));

            character.AddLifeEvent($"{pet.Name} ile oynadın.");
        }

        public void FeedPet(CharacterData character, string petId)
        {
            var pet = character.Pets.FirstOrDefault(p => p.Id == petId);
            if (pet == null || !pet.IsAlive) return;

            // Yiyecek maliyeti
            float cost = Random.Range(20f, 100f);
            if (character.Stats.Money < cost)
            {
                Debug.Log("Yeterli para yok!");
                return;
            }

            GameManager.Instance.StatSystem.ModifyStat(character, StatType.Money, -cost);

            // Sağlık ve mutluluk artışı
            pet.Health = Mathf.Min(100, pet.Health + Random.Range(3f, 8f));
            pet.Happiness = Mathf.Min(100, pet.Happiness + Random.Range(5f, 10f));

            character.AddLifeEvent($"{pet.Name}'i besledin.");
        }

        public void TakeToVet(CharacterData character, string petId)
        {
            var pet = character.Pets.FirstOrDefault(p => p.Id == petId);
            if (pet == null || !pet.IsAlive) return;

            float cost = vetCosts[pet.Type];

            // Para kontrolü
            if (character.Stats.Money < cost)
            {
                Debug.Log("Veteriner için yeterli para yok!");
                return;
            }

            GameManager.Instance.StatSystem.ModifyStat(character, StatType.Money, -cost);

            // Sağlık iyileştirmesi
            float healing = Random.Range(20f, 40f);
            pet.Health = Mathf.Min(100, pet.Health + healing);

            character.AddLifeEvent($"{pet.Name}'i veterinere götürdün. (₺{cost:N0})");

            // Karma artışı
            GameManager.Instance.StatSystem.ModifyStat(character, StatType.Karma, 3f);
        }

        public void AbandonPet(CharacterData character, string petId)
        {
            var pet = character.Pets.FirstOrDefault(p => p.Id == petId);
            if (pet == null || !pet.IsAlive) return;

            character.Pets.Remove(pet);
            character.AddLifeEvent($"{pet.Name} adlı {pet.GetTypeName()}'i terk ettin.");

            // Karma düşüşü
            GameManager.Instance.StatSystem.ModifyStat(character, StatType.Karma, -20f);

            // Hafif mutluluk düşüşü
            GameManager.Instance.StatSystem.ModifyStat(character, StatType.Happiness, -5f);
        }

        public void GiveAwayPet(CharacterData character, string petId)
        {
            var pet = character.Pets.FirstOrDefault(p => p.Id == petId);
            if (pet == null || !pet.IsAlive) return;

            character.Pets.Remove(pet);
            character.AddLifeEvent($"{pet.Name} adlı {pet.GetTypeName()}'i başka birine verdin.");

            // Hafif karma artışı (terk etmekten iyi)
            GameManager.Instance.StatSystem.ModifyStat(character, StatType.Karma, 2f);
            GameManager.Instance.StatSystem.ModifyStat(character, StatType.Happiness, -3f);
        }

        private bool CheckPetDeath(PetData pet)
        {
            if (!pet.IsAlive) return false;

            // Sağlık sıfırsa öl
            if (pet.Health <= 0) return true;

            // Yaşa göre ölüm şansı
            int maxAge = pet.GetMaxAge();
            float ageRatio = (float)pet.Age / maxAge;

            float deathChance = 0f;

            if (ageRatio >= 1.0f)
            {
                deathChance = 0.5f + (ageRatio - 1.0f) * 0.3f;
            }
            else if (ageRatio >= 0.8f)
            {
                deathChance = (ageRatio - 0.8f) * 0.5f;
            }
            else if (ageRatio >= 0.6f)
            {
                deathChance = (ageRatio - 0.6f) * 0.1f;
            }

            // Düşük sağlık ölüm şansını artırır
            if (pet.Health < 30)
            {
                deathChance += (30 - pet.Health) / 100f;
            }

            return Random.value < deathChance;
        }

        public float GetPetPrice(PetType type)
        {
            return petPrices.ContainsKey(type) ? petPrices[type] : 1000f;
        }

        public float GetVetCost(PetType type)
        {
            return vetCosts.ContainsKey(type) ? vetCosts[type] : 300f;
        }

        public float GetYearlyCost(PetType type)
        {
            return yearlyCosts.ContainsKey(type) ? yearlyCosts[type] : 500f;
        }

        public List<PetData> GetAlivePets(CharacterData character)
        {
            return character.Pets.Where(p => p.IsAlive).ToList();
        }

        public int GetPetCount(CharacterData character)
        {
            return character.Pets.Count(p => p.IsAlive);
        }

        public string GetPetTypeName(PetType type)
        {
            return type switch
            {
                PetType.Dog => "Köpek",
                PetType.Cat => "Kedi",
                PetType.Bird => "Kuş",
                PetType.Fish => "Balık",
                PetType.Hamster => "Hamster",
                PetType.Rabbit => "Tavşan",
                PetType.Turtle => "Kaplumbağa",
                PetType.Parrot => "Papağan",
                _ => "Hayvan"
            };
        }

        public List<PetType> GetAvailablePetTypes()
        {
            return new List<PetType>
            {
                PetType.Dog,
                PetType.Cat,
                PetType.Bird,
                PetType.Fish,
                PetType.Hamster,
                PetType.Rabbit,
                PetType.Turtle,
                PetType.Parrot
            };
        }
    }
}
