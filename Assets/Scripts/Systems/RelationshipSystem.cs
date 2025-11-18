using UnityEngine;
using System.Collections.Generic;
using System.Linq;
using BitLifeTR.Core;
using BitLifeTR.Data;

namespace BitLifeTR.Systems
{
    /// <summary>
    /// İlişki yönetim sistemi
    /// </summary>
    public class RelationshipSystem : MonoBehaviour
    {
        public void ProcessYearlyRelationships(CharacterData character)
        {
            foreach (var relationship in character.Relationships.ToList())
            {
                // Yaşı ilerlet
                relationship.Age++;

                // İlişki seviyesi rastgele değişir
                float change = Random.Range(-5f, 5f);
                ModifyRelationship(character, relationship.Id, change);

                // Ölüm kontrolü
                if (CheckPersonDeath(relationship))
                {
                    relationship.IsAlive = false;
                    character.AddLifeEvent($"{relationship.Name} hayatını kaybetti.");

                    // Üzüntü
                    GameManager.Instance.StatSystem.ModifyStat(character, StatType.Happiness, -20f);
                }
            }
        }

        public void ModifyRelationship(CharacterData character, string personId, float delta)
        {
            var relationship = character.Relationships.FirstOrDefault(r => r.Id == personId);
            if (relationship != null)
            {
                float oldValue = relationship.RelationshipLevel;
                relationship.RelationshipLevel = Mathf.Clamp(relationship.RelationshipLevel + delta, 0, 100);

                EventBus.Publish(new RelationshipChangedEvent(personId, oldValue, relationship.RelationshipLevel));
            }
        }

        public void SpendTimeWith(CharacterData character, string personId)
        {
            var relationship = character.Relationships.FirstOrDefault(r => r.Id == personId);
            if (relationship == null || !relationship.IsAlive)
                return;

            float improvement = Random.Range(5f, 15f);
            ModifyRelationship(character, personId, improvement);

            // Mutluluk artışı
            GameManager.Instance.StatSystem.ModifyStat(character, StatType.Happiness, Random.Range(2f, 5f));

            character.AddLifeEvent($"{relationship.Name} ile vakit geçirdin.");
        }

        public void GiveGift(CharacterData character, string personId, float giftValue)
        {
            var relationship = character.Relationships.FirstOrDefault(r => r.Id == personId);
            if (relationship == null || !relationship.IsAlive)
                return;

            // Para harca
            if (character.Stats.Money < giftValue)
            {
                Debug.Log("Yeterli para yok!");
                return;
            }

            GameManager.Instance.StatSystem.ModifyStat(character, StatType.Money, -giftValue);

            // İlişki iyileştirmesi
            float improvement = giftValue / 10f;
            ModifyRelationship(character, personId, improvement);

            character.AddLifeEvent($"{relationship.Name}'a hediye verdin.");
        }

        public void Argue(CharacterData character, string personId)
        {
            var relationship = character.Relationships.FirstOrDefault(r => r.Id == personId);
            if (relationship == null || !relationship.IsAlive)
                return;

            float damage = Random.Range(-20f, -5f);
            ModifyRelationship(character, personId, damage);

            // Düşük şans ile barışma
            if (Random.value > 0.7f)
            {
                ModifyRelationship(character, personId, 5f);
                character.AddLifeEvent($"{relationship.Name} ile tartıştın ama barıştınız.");
            }
            else
            {
                character.AddLifeEvent($"{relationship.Name} ile tartıştın.");
            }

            GameManager.Instance.StatSystem.ModifyStat(character, StatType.Happiness, -5f);
        }

        public RelationshipData StartRomance(CharacterData character)
        {
            // Yeni romantik ilişki başlat
            Gender partnerGender = character.Gender == Gender.Male ? Gender.Female : Gender.Male;

            RelationshipData partner = new RelationshipData
            {
                Id = System.Guid.NewGuid().ToString(),
                Name = GenerateRandomName(partnerGender),
                Gender = partnerGender,
                RelationType = RelationType.Romantic,
                Age = character.Age + Random.Range(-5, 6),
                RelationshipLevel = Random.Range(30f, 60f),
                IsAlive = true
            };

            character.Relationships.Add(partner);
            character.AddLifeEvent($"{partner.Name} ile tanıştın.");

            return partner;
        }

        public bool ProposeMarriage(CharacterData character, string partnerId)
        {
            var partner = character.Relationships.FirstOrDefault(r => r.Id == partnerId);
            if (partner == null || partner.RelationType != RelationType.Romantic)
                return false;

            // Kabul şansı ilişki seviyesine bağlı
            float acceptChance = partner.RelationshipLevel / 100f;

            if (Random.value < acceptChance)
            {
                partner.IsEngaged = true;
                character.AddLifeEvent($"{partner.Name} evlenme teklifini kabul etti!");
                GameManager.Instance.StatSystem.ModifyStat(character, StatType.Happiness, 30f);
                return true;
            }
            else
            {
                character.AddLifeEvent($"{partner.Name} evlenme teklifini reddetti.");
                ModifyRelationship(character, partnerId, -20f);
                GameManager.Instance.StatSystem.ModifyStat(character, StatType.Happiness, -20f);
                return false;
            }
        }

        public void GetMarried(CharacterData character, string partnerId)
        {
            var partner = character.Relationships.FirstOrDefault(r => r.Id == partnerId);
            if (partner == null || !partner.IsEngaged)
                return;

            partner.IsMarried = true;
            partner.IsEngaged = false;
            partner.RelationType = RelationType.Spouse;

            // Düğün maliyeti
            float weddingCost = Random.Range(10000f, 100000f);
            GameManager.Instance.StatSystem.ModifyStat(character, StatType.Money, -weddingCost);

            character.AddLifeEvent($"{partner.Name} ile evlendin! (Düğün maliyeti: {weddingCost:N0} TL)");
            GameManager.Instance.StatSystem.ModifyStat(character, StatType.Happiness, 40f);
            GameManager.Instance.StatSystem.ModifyStat(character, StatType.Karma, 10f);
        }

        public RelationshipData HaveChild(CharacterData character)
        {
            // Eş kontrolü
            var spouse = character.Relationships.FirstOrDefault(r => r.RelationType == RelationType.Spouse && r.IsAlive);
            if (spouse == null)
            {
                Debug.Log("Çocuk sahibi olmak için evli olmalısın!");
                return null;
            }

            Gender childGender = (Gender)Random.Range(0, 2);
            RelationshipData child = new RelationshipData
            {
                Id = System.Guid.NewGuid().ToString(),
                Name = GenerateRandomName(childGender),
                Gender = childGender,
                RelationType = RelationType.Child,
                Age = 0,
                RelationshipLevel = Random.Range(70f, 100f),
                IsAlive = true
            };

            character.Relationships.Add(child);
            character.AddLifeEvent($"Bir {(childGender == Gender.Male ? "oğlun" : "kızın")} oldu: {child.Name}!");
            GameManager.Instance.StatSystem.ModifyStat(character, StatType.Happiness, 30f);
            GameManager.Instance.StatSystem.ModifyStat(character, StatType.Karma, 10f);

            return child;
        }

        public void Divorce(CharacterData character, string spouseId)
        {
            var spouse = character.Relationships.FirstOrDefault(r => r.Id == spouseId && r.RelationType == RelationType.Spouse);
            if (spouse == null)
                return;

            spouse.IsMarried = false;
            spouse.RelationType = RelationType.Romantic; // Eski eş

            // Varlıkların yarısını kay bet
            float assetLoss = character.Finance.BankBalance * 0.5f;
            GameManager.Instance.StatSystem.ModifyStat(character, StatType.Money, -assetLoss);

            character.AddLifeEvent($"{spouse.Name} ile boşandın. (Kayıp: {assetLoss:N0} TL)");
            GameManager.Instance.StatSystem.ModifyStat(character, StatType.Happiness, -30f);
            ModifyRelationship(character, spouseId, -50f);
        }

        private bool CheckPersonDeath(RelationshipData person)
        {
            if (!person.IsAlive)
                return false;

            // Yaşa göre ölüm şansı
            float deathChance = 0f;

            if (person.Age > 70)
            {
                deathChance = (person.Age - 70) * 0.01f;
            }
            else if (person.Age > 50)
            {
                deathChance = (person.Age - 50) * 0.002f;
            }

            return Random.value < deathChance;
        }

        private string GenerateRandomName(Gender gender)
        {
            string[] maleNames = { "Emre", "Burak", "Serkan", "Can", "Mert", "Kaan", "Efe", "Berk", "Yiğit", "Cem" };
            string[] femaleNames = { "Selin", "Elif", "Zeynep", "Melis", "Ceren", "İrem", "Gizem", "Deniz", "Ece", "Naz" };

            return gender == Gender.Male
                ? maleNames[Random.Range(0, maleNames.Length)]
                : femaleNames[Random.Range(0, femaleNames.Length)];
        }

        public List<RelationshipData> GetRelationshipsByType(CharacterData character, RelationType type)
        {
            return character.Relationships.Where(r => r.RelationType == type && r.IsAlive).ToList();
        }

        public RelationshipData GetSpouse(CharacterData character)
        {
            return character.Relationships.FirstOrDefault(r => r.RelationType == RelationType.Spouse && r.IsAlive);
        }

        public List<RelationshipData> GetChildren(CharacterData character)
        {
            return GetRelationshipsByType(character, RelationType.Child);
        }

        public List<RelationshipData> GetParents(CharacterData character)
        {
            return GetRelationshipsByType(character, RelationType.Parent);
        }
    }
}
