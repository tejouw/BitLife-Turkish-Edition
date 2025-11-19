using UnityEngine;
using System.Collections.Generic;
using System.Linq;
using BitLifeTR.Core;
using BitLifeTR.Data;

namespace BitLifeTR.Systems
{
    /// <summary>
    /// Miras ve nesil sistemi
    /// Karakterin ölümünden sonra çocuklarla devam etme mekanizması
    /// </summary>
    public class LegacySystem : MonoBehaviour
    {
        // Miras vergi oranları (Türkiye'ye özgü)
        private const float INHERITANCE_TAX_RATE_LOW = 0.01f;      // 1% - 500K'ya kadar
        private const float INHERITANCE_TAX_RATE_MEDIUM = 0.03f;   // 3% - 500K-1M arası
        private const float INHERITANCE_TAX_RATE_HIGH = 0.05f;     // 5% - 1M-5M arası
        private const float INHERITANCE_TAX_RATE_VERY_HIGH = 0.10f; // 10% - 5M üzeri

        // Avukat ücreti
        private const float LAWYER_FEE_PERCENTAGE = 0.02f; // 2%
        private const float MIN_LAWYER_FEE = 5000f;
        private const float MAX_LAWYER_FEE = 500000f;

        /// <summary>
        /// Net serveti hesapla
        /// </summary>
        public float CalculateNetWorth(CharacterData character)
        {
            float netWorth = 0;

            // Nakit
            netWorth += character.Stats.Money;
            netWorth += character.Finance.BankBalance;

            // Varlıklar
            foreach (var asset in character.Finance.Assets)
            {
                netWorth += asset.Value;
            }

            // Borçları çıkar
            netWorth -= character.Finance.Debt;

            return Mathf.Max(0, netWorth);
        }

        /// <summary>
        /// Miras vergisini hesapla
        /// </summary>
        public float CalculateInheritanceTax(float inheritanceAmount)
        {
            float tax = 0;

            if (inheritanceAmount <= 500000)
            {
                tax = inheritanceAmount * INHERITANCE_TAX_RATE_LOW;
            }
            else if (inheritanceAmount <= 1000000)
            {
                tax = 500000 * INHERITANCE_TAX_RATE_LOW;
                tax += (inheritanceAmount - 500000) * INHERITANCE_TAX_RATE_MEDIUM;
            }
            else if (inheritanceAmount <= 5000000)
            {
                tax = 500000 * INHERITANCE_TAX_RATE_LOW;
                tax += 500000 * INHERITANCE_TAX_RATE_MEDIUM;
                tax += (inheritanceAmount - 1000000) * INHERITANCE_TAX_RATE_HIGH;
            }
            else
            {
                tax = 500000 * INHERITANCE_TAX_RATE_LOW;
                tax += 500000 * INHERITANCE_TAX_RATE_MEDIUM;
                tax += 4000000 * INHERITANCE_TAX_RATE_HIGH;
                tax += (inheritanceAmount - 5000000) * INHERITANCE_TAX_RATE_VERY_HIGH;
            }

            return tax;
        }

        /// <summary>
        /// Avukat ücretini hesapla
        /// </summary>
        public float CalculateLawyerFee(float inheritanceAmount)
        {
            float fee = inheritanceAmount * LAWYER_FEE_PERCENTAGE;
            return Mathf.Clamp(fee, MIN_LAWYER_FEE, MAX_LAWYER_FEE);
        }

        /// <summary>
        /// Mirası dağıt
        /// </summary>
        public Dictionary<string, InheritanceShare> DistributeInheritance(CharacterData deceased)
        {
            var distribution = new Dictionary<string, InheritanceShare>();
            float totalEstate = CalculateNetWorth(deceased);

            if (totalEstate <= 0)
                return distribution;

            // Vasiyetname varsa ona göre dağıt
            if (deceased.Legacy.Will.HasWill)
            {
                return DistributeByWill(deceased, totalEstate);
            }

            // Vasiyetname yoksa yasal mirasçılara dağıt
            return DistributeByLaw(deceased, totalEstate);
        }

        /// <summary>
        /// Vasiyetnameye göre dağıt
        /// </summary>
        private Dictionary<string, InheritanceShare> DistributeByWill(CharacterData deceased, float totalEstate)
        {
            var distribution = new Dictionary<string, InheritanceShare>();
            var will = deceased.Legacy.Will;

            // Hayır kurumu payı
            if (will.CharityPercentage > 0)
            {
                float charityAmount = totalEstate * (will.CharityPercentage / 100f);
                distribution["CHARITY"] = new InheritanceShare
                {
                    BeneficiaryId = "CHARITY",
                    BeneficiaryName = will.CharityName,
                    Amount = charityAmount,
                    Percentage = will.CharityPercentage,
                    Assets = new List<AssetData>()
                };
                totalEstate -= charityAmount;
            }

            // Lehdarlara dağıt
            foreach (var beneficiary in will.Beneficiaries)
            {
                float amount = totalEstate * (beneficiary.Percentage / 100f);

                // Belirli varlıklar
                var specificAssets = deceased.Finance.Assets
                    .Where(a => beneficiary.SpecificAssets.Contains(a.Name))
                    .ToList();

                distribution[beneficiary.RelationshipId] = new InheritanceShare
                {
                    BeneficiaryId = beneficiary.RelationshipId,
                    BeneficiaryName = beneficiary.Name,
                    Amount = amount,
                    Percentage = beneficiary.Percentage,
                    Assets = specificAssets
                };
            }

            return distribution;
        }

        /// <summary>
        /// Yasal mirasçılara göre dağıt (Türk Medeni Kanunu)
        /// </summary>
        private Dictionary<string, InheritanceShare> DistributeByLaw(CharacterData deceased, float totalEstate)
        {
            var distribution = new Dictionary<string, InheritanceShare>();
            var livingRelatives = deceased.Relationships.Where(r => r.IsAlive).ToList();

            // Eş
            var spouse = livingRelatives.FirstOrDefault(r => r.RelationType == RelationType.Spouse);

            // Çocuklar
            var children = livingRelatives.Where(r => r.RelationType == RelationType.Child).ToList();

            // Ebeveynler
            var parents = livingRelatives.Where(r => r.RelationType == RelationType.Parent).ToList();

            float spouseShare = 0;
            float childrenShare = 0;
            float parentsShare = 0;

            // Türk Medeni Kanunu'na göre miras paylaşımı
            if (spouse != null && children.Count > 0)
            {
                // Eş: 1/4, Çocuklar: 3/4
                spouseShare = 0.25f;
                childrenShare = 0.75f;
            }
            else if (spouse != null && children.Count == 0)
            {
                if (parents.Count > 0)
                {
                    // Eş: 1/2, Anne-Baba: 1/2
                    spouseShare = 0.50f;
                    parentsShare = 0.50f;
                }
                else
                {
                    // Sadece eş varsa: Tamamı
                    spouseShare = 1.0f;
                }
            }
            else if (children.Count > 0)
            {
                // Sadece çocuklar varsa: Eşit paylaşım
                childrenShare = 1.0f;
            }
            else if (parents.Count > 0)
            {
                // Sadece ebeveynler varsa: Eşit paylaşım
                parentsShare = 1.0f;
            }

            // Eşe dağıt
            if (spouse != null && spouseShare > 0)
            {
                distribution[spouse.Id] = new InheritanceShare
                {
                    BeneficiaryId = spouse.Id,
                    BeneficiaryName = spouse.Name,
                    Amount = totalEstate * spouseShare,
                    Percentage = spouseShare * 100,
                    Assets = new List<AssetData>()
                };
            }

            // Çocuklara dağıt
            if (children.Count > 0 && childrenShare > 0)
            {
                float perChildShare = childrenShare / children.Count;
                foreach (var child in children)
                {
                    distribution[child.Id] = new InheritanceShare
                    {
                        BeneficiaryId = child.Id,
                        BeneficiaryName = child.Name,
                        Amount = totalEstate * perChildShare,
                        Percentage = perChildShare * 100,
                        Assets = new List<AssetData>()
                    };
                }
            }

            // Ebeveynlere dağıt
            if (parents.Count > 0 && parentsShare > 0)
            {
                float perParentShare = parentsShare / parents.Count;
                foreach (var parent in parents)
                {
                    distribution[parent.Id] = new InheritanceShare
                    {
                        BeneficiaryId = parent.Id,
                        BeneficiaryName = parent.Name,
                        Amount = totalEstate * perParentShare,
                        Percentage = perParentShare * 100,
                        Assets = new List<AssetData>()
                    };
                }
            }

            return distribution;
        }

        /// <summary>
        /// Çocuk olarak devam et
        /// </summary>
        public CharacterData ContinueAsChild(CharacterData deceased, RelationshipData selectedChild, DeathReason deathReason)
        {
            // Mirası hesapla
            var distribution = DistributeInheritance(deceased);
            float childInheritance = 0;
            List<AssetData> inheritedAssets = new List<AssetData>();

            if (distribution.ContainsKey(selectedChild.Id))
            {
                var share = distribution[selectedChild.Id];
                childInheritance = share.Amount;
                inheritedAssets = share.Assets;

                // Vergi ve masrafları düş
                float tax = CalculateInheritanceTax(childInheritance);
                float lawyerFee = CalculateLawyerFee(childInheritance);
                childInheritance -= (tax + lawyerFee);
            }

            // Yeni karakter oluştur
            CharacterData newCharacter = new CharacterData();
            newCharacter.Id = selectedChild.Id;
            newCharacter.Name = selectedChild.Name;
            newCharacter.Surname = deceased.Surname;
            newCharacter.Gender = selectedChild.Gender;
            newCharacter.Age = selectedChild.Age;
            newCharacter.BirthYear = deceased.BirthYear + deceased.Age - selectedChild.Age;
            newCharacter.BirthCity = deceased.BirthCity;

            // Başlangıç statları (genetik + çevre faktörü)
            newCharacter.Stats.Health = UnityEngine.Random.Range(60f, 100f);
            newCharacter.Stats.Happiness = UnityEngine.Random.Range(30f, 70f); // Ebeveyn kaybı
            newCharacter.Stats.Intelligence = Mathf.Clamp(
                (deceased.Stats.Intelligence * 0.5f) + UnityEngine.Random.Range(0f, 50f),
                0f, 100f
            );
            newCharacter.Stats.Appearance = Mathf.Clamp(
                (deceased.Stats.Appearance * 0.6f) + UnityEngine.Random.Range(0f, 40f),
                0f, 100f
            );
            newCharacter.Stats.Money = childInheritance;
            newCharacter.Stats.Fame = deceased.Stats.Fame * 0.1f; // Biraz şöhret devri
            newCharacter.Stats.Karma = 50f;

            // Miras verilerini aktar
            newCharacter.Legacy.Generation = deceased.Legacy.Generation + 1;
            newCharacter.Legacy.FamilyName = deceased.Surname;
            newCharacter.Legacy.TotalFamilyWealth = deceased.Legacy.TotalFamilyWealth + CalculateNetWorth(deceased);
            newCharacter.Legacy.TotalFamilyMembers = deceased.Legacy.TotalFamilyMembers + 1;
            newCharacter.Legacy.HasReceivedInheritance = true;
            newCharacter.Legacy.InheritedAmount = childInheritance;
            newCharacter.Legacy.InheritedAssets = inheritedAssets;

            // Miras alınan varlıkları ekle
            newCharacter.Finance.Assets = new List<AssetData>(inheritedAssets);
            newCharacter.Finance.BankBalance = childInheritance * 0.3f;
            newCharacter.Stats.Money = childInheritance * 0.7f;

            // Ataları kopyala ve ölen karakteri ekle
            newCharacter.Legacy.Ancestors = new List<AncestorData>(deceased.Legacy.Ancestors);
            newCharacter.Legacy.Ancestors.Add(CreateAncestorFromCharacter(deceased, deathReason));

            // Aile geçmişini güncelle
            newCharacter.Legacy.FamilyHistory = new List<string>(deceased.Legacy.FamilyHistory);
            newCharacter.Legacy.FamilyHistory.Add(
                $"[Nesil {deceased.Legacy.Generation}] {deceased.Name} {deceased.Surname}, " +
                $"{deceased.Age} yaşında vefat etti. Net servet: ₺{CalculateNetWorth(deceased):N0}"
            );

            // İlişkileri güncelle
            UpdateRelationshipsForNewGeneration(newCharacter, deceased, selectedChild);

            // Yaşam olayı ekle
            newCharacter.AddLifeEvent($"Babanız/Anneniz {deceased.Name}'in vefatının ardından ₺{childInheritance:N0} miras aldınız.");

            return newCharacter;
        }

        /// <summary>
        /// Ölen karakterden ata verisi oluştur
        /// </summary>
        private AncestorData CreateAncestorFromCharacter(CharacterData character, DeathReason deathReason)
        {
            var ancestor = new AncestorData
            {
                Id = character.Id,
                Name = character.Name,
                Surname = character.Surname,
                Gender = character.Gender,
                BirthYear = character.BirthYear,
                DeathYear = character.BirthYear + character.Age,
                DeathAge = character.Age,
                DeathReason = deathReason,
                NetWorth = CalculateNetWorth(character),
                Occupation = character.Career.CurrentJob ?? "İşsiz",
                ChildrenCount = character.Relationships.Count(r => r.RelationType == RelationType.Child)
            };

            // Başarıları ekle
            if (character.Stats.Fame > 80)
                ancestor.Achievements.Add("Ünlü");
            if (character.Stats.Money > 10000000)
                ancestor.Achievements.Add("Milyoner");
            if (character.Education.HighestLevel >= EducationLevel.Doctorate)
                ancestor.Achievements.Add("Doktor unvanı");
            if (character.Career.TotalWorkYears > 40)
                ancestor.Achievements.Add("Kariyer uzmanı");
            if (character.Age > 90)
                ancestor.Achievements.Add("Uzun ömür");

            return ancestor;
        }

        /// <summary>
        /// Yeni nesil için ilişkileri güncelle
        /// </summary>
        private void UpdateRelationshipsForNewGeneration(CharacterData newCharacter, CharacterData deceased, RelationshipData selectedChild)
        {
            newCharacter.Relationships = new List<RelationshipData>();

            foreach (var rel in deceased.Relationships)
            {
                if (!rel.IsAlive) continue;
                if (rel.Id == selectedChild.Id) continue; // Kendisini ekleme

                var newRel = new RelationshipData
                {
                    Id = rel.Id,
                    Name = rel.Name,
                    Age = rel.Age,
                    RelationshipLevel = rel.RelationshipLevel * 0.8f, // Biraz azalt
                    IsAlive = rel.IsAlive,
                    Gender = rel.Gender,
                    Occupation = rel.Occupation
                };

                // İlişki tipini güncelle
                switch (rel.RelationType)
                {
                    case RelationType.Spouse:
                        // Ölen karakterin eşi, yeni karakterin üvey ebeveyni
                        newRel.RelationType = RelationType.Parent;
                        break;
                    case RelationType.Child:
                        // Diğer çocuklar kardeş olur
                        newRel.RelationType = RelationType.Sibling;
                        break;
                    case RelationType.Parent:
                        // Büyükanne/büyükbaba olur
                        newRel.RelationType = RelationType.Relative;
                        break;
                    case RelationType.Sibling:
                        // Amca/teyze olur
                        newRel.RelationType = RelationType.Relative;
                        break;
                    default:
                        newRel.RelationType = rel.RelationType;
                        break;
                }

                newCharacter.Relationships.Add(newRel);
            }

            // Ölen ebeveyni ekle (ölü olarak)
            newCharacter.Relationships.Add(new RelationshipData
            {
                Id = deceased.Id,
                Name = deceased.Name,
                Age = deceased.Age,
                RelationType = RelationType.Parent,
                RelationshipLevel = 100f,
                IsAlive = false,
                Gender = deceased.Gender
            });
        }

        /// <summary>
        /// Vasiyetname oluştur/güncelle
        /// </summary>
        public void CreateOrUpdateWill(CharacterData character, WillDistribution distributionType,
            List<WillBeneficiary> beneficiaries, float charityPercentage = 0, string charityName = "")
        {
            character.Legacy.Will.HasWill = true;
            character.Legacy.Will.DistributionType = distributionType;
            character.Legacy.Will.Beneficiaries = beneficiaries;
            character.Legacy.Will.CharityPercentage = charityPercentage;
            character.Legacy.Will.CharityName = charityName;
            character.Legacy.Will.LastUpdatedYear = character.BirthYear + character.Age;

            character.AddLifeEvent("Vasiyetnamenizi güncellediniz.");
        }

        /// <summary>
        /// Otomatik vasiyetname oluştur (eşit dağıtım)
        /// </summary>
        public void CreateDefaultWill(CharacterData character)
        {
            var beneficiaries = new List<WillBeneficiary>();
            var livingFamily = character.Relationships
                .Where(r => r.IsAlive && (r.RelationType == RelationType.Spouse || r.RelationType == RelationType.Child))
                .ToList();

            if (livingFamily.Count == 0) return;

            float percentagePerPerson = 100f / livingFamily.Count;

            foreach (var family in livingFamily)
            {
                beneficiaries.Add(new WillBeneficiary
                {
                    RelationshipId = family.Id,
                    Name = family.Name,
                    RelationType = family.RelationType,
                    Percentage = percentagePerPerson
                });
            }

            CreateOrUpdateWill(character, WillDistribution.Equal, beneficiaries);
        }

        /// <summary>
        /// Devam edilebilir çocukları getir
        /// </summary>
        public List<RelationshipData> GetEligibleChildren(CharacterData character)
        {
            return character.Relationships
                .Where(r => r.IsAlive && r.RelationType == RelationType.Child && r.Age >= 0)
                .OrderByDescending(r => r.Age)
                .ToList();
        }

        /// <summary>
        /// Nesil özeti oluştur
        /// </summary>
        public string GetGenerationSummary(CharacterData character)
        {
            string summary = $"=== {character.Surname} Ailesi - Nesil {character.Legacy.Generation} ===\n\n";
            summary += $"Toplam Aile Serveti: ₺{character.Legacy.TotalFamilyWealth:N0}\n";
            summary += $"Aile Üye Sayısı: {character.Legacy.TotalFamilyMembers}\n\n";

            if (character.Legacy.Ancestors.Count > 0)
            {
                summary += "=== Atalar ===\n";
                foreach (var ancestor in character.Legacy.Ancestors)
                {
                    summary += $"\n{ancestor.Name} {ancestor.Surname}\n";
                    summary += $"  Yaşam: {ancestor.BirthYear} - {ancestor.DeathYear} ({ancestor.DeathAge} yaş)\n";
                    summary += $"  Meslek: {ancestor.Occupation}\n";
                    summary += $"  Servet: ₺{ancestor.NetWorth:N0}\n";
                    if (ancestor.Achievements.Count > 0)
                    {
                        summary += $"  Başarılar: {string.Join(", ", ancestor.Achievements)}\n";
                    }
                }
            }

            return summary;
        }
    }

    /// <summary>
    /// Miras payı
    /// </summary>
    public class InheritanceShare
    {
        public string BeneficiaryId;
        public string BeneficiaryName;
        public float Amount;
        public float Percentage;
        public List<AssetData> Assets;
    }
}
