using UnityEngine;
using System.Collections.Generic;
using BitLifeTR.Core;
using BitLifeTR.Data;

namespace BitLifeTR.Systems
{
    /// <summary>
    /// Ekonomi ve finans sistemi
    /// </summary>
    public class EconomySystem : MonoBehaviour
    {
        public void ProcessYearlyEconomy(CharacterData character)
        {
            // Aylık giderler
            ProcessMonthlyExpenses(character);

            // Borç faizi
            ProcessDebt(character);

            // Varlık değer değişimleri
            ProcessAssetValues(character);

            // Emekli maaşı
            if (character.Career.IsRetired && character.Finance.MonthlyIncome > 0)
            {
                float yearlyPension = character.Finance.MonthlyIncome * 12;
                GameManager.Instance.StatSystem.ModifyStat(character, StatType.Money, yearlyPension);
            }
        }

        private void ProcessMonthlyExpenses(CharacterData character)
        {
            // Temel yaşam giderleri
            float baseExpenses = 3000f; // Aylık minimum

            // Yaşa göre giderler
            if (character.Age < 18)
            {
                baseExpenses = 0; // Aile karşılıyor
            }
            else if (character.Age > 65)
            {
                baseExpenses *= 1.5f; // Sağlık giderleri
            }

            // Varlık giderleri
            foreach (var asset in character.Finance.Assets)
            {
                baseExpenses += asset.MonthlyPayment;
            }

            // Yıllık toplam
            float yearlyExpenses = baseExpenses * 12;
            character.Finance.MonthlyExpenses = baseExpenses;

            // Eğer para yoksa borç
            if (character.Stats.Money < yearlyExpenses)
            {
                float shortfall = yearlyExpenses - character.Stats.Money;
                character.Finance.Debt += shortfall;
                character.Stats.Money = 0;
                character.AddLifeEvent($"Giderlerini karşılayamadın, {shortfall:N0} TL borçlandın.");
            }
            else
            {
                GameManager.Instance.StatSystem.ModifyStat(character, StatType.Money, -yearlyExpenses);
            }
        }

        private void ProcessDebt(CharacterData character)
        {
            if (character.Finance.Debt <= 0)
                return;

            // Yıllık %15 faiz
            float interest = character.Finance.Debt * 0.15f;
            character.Finance.Debt += interest;

            // Mutluluk düşüşü
            if (character.Finance.Debt > 50000)
            {
                GameManager.Instance.StatSystem.ModifyStat(character, StatType.Happiness, -5f);
            }
        }

        private void ProcessAssetValues(CharacterData character)
        {
            foreach (var asset in character.Finance.Assets)
            {
                // Ev değer artışı
                if (asset.Type == "Ev")
                {
                    asset.Value *= Random.Range(1.02f, 1.08f);
                }
                // Araba değer düşüşü
                else if (asset.Type == "Araba")
                {
                    asset.Value *= Random.Range(0.85f, 0.95f);
                }
            }
        }

        public bool BuyHouse(CharacterData character, float price)
        {
            // Peşinat kontrolü (%20)
            float downPayment = price * 0.2f;

            if (character.Stats.Money < downPayment)
            {
                character.AddLifeEvent("Ev almak için yeterli peşinatın yok!");
                return false;
            }

            // Kredi çek
            float loanAmount = price - downPayment;
            float monthlyPayment = loanAmount / 240; // 20 yıl

            var house = new AssetData
            {
                Name = $"Ev ({GetRandomDistrict()})",
                Type = "Ev",
                Value = price,
                MonthlyPayment = monthlyPayment,
                YearPurchased = character.Age + character.BirthYear
            };

            character.Finance.Assets.Add(house);
            GameManager.Instance.StatSystem.ModifyStat(character, StatType.Money, -downPayment);
            character.Finance.Debt += loanAmount;

            character.AddLifeEvent($"Ev satın aldın! (Fiyat: {price:N0} TL, Aylık taksit: {monthlyPayment:N0} TL)");
            GameManager.Instance.StatSystem.ModifyStat(character, StatType.Happiness, 20f);

            return true;
        }

        public bool BuyCar(CharacterData character, string carName, float price)
        {
            if (character.Stats.Money < price)
            {
                // Kredi ile alım
                float downPayment = price * 0.3f;
                if (character.Stats.Money < downPayment)
                {
                    character.AddLifeEvent("Araba almak için yeterli paran yok!");
                    return false;
                }

                float loanAmount = price - downPayment;
                float monthlyPayment = loanAmount / 48; // 4 yıl

                var car = new AssetData
                {
                    Name = carName,
                    Type = "Araba",
                    Value = price,
                    MonthlyPayment = monthlyPayment,
                    YearPurchased = character.Age + character.BirthYear
                };

                character.Finance.Assets.Add(car);
                GameManager.Instance.StatSystem.ModifyStat(character, StatType.Money, -downPayment);
                character.Finance.Debt += loanAmount;

                character.AddLifeEvent($"{carName} satın aldın! (Aylık taksit: {monthlyPayment:N0} TL)");
            }
            else
            {
                // Peşin alım
                var car = new AssetData
                {
                    Name = carName,
                    Type = "Araba",
                    Value = price,
                    MonthlyPayment = 0,
                    YearPurchased = character.Age + character.BirthYear
                };

                character.Finance.Assets.Add(car);
                GameManager.Instance.StatSystem.ModifyStat(character, StatType.Money, -price);

                character.AddLifeEvent($"{carName} satın aldın! (Peşin: {price:N0} TL)");
            }

            GameManager.Instance.StatSystem.ModifyStat(character, StatType.Happiness, 15f);
            return true;
        }

        public void SellAsset(CharacterData character, AssetData asset)
        {
            character.Finance.Assets.Remove(asset);
            GameManager.Instance.StatSystem.ModifyStat(character, StatType.Money, asset.Value);

            character.AddLifeEvent($"{asset.Name} sattın. ({asset.Value:N0} TL)");
        }

        public void PayDebt(CharacterData character, float amount)
        {
            if (character.Stats.Money < amount)
            {
                amount = character.Stats.Money;
            }

            if (amount <= 0)
                return;

            GameManager.Instance.StatSystem.ModifyStat(character, StatType.Money, -amount);
            character.Finance.Debt = Mathf.Max(0, character.Finance.Debt - amount);

            character.AddLifeEvent($"Borcunun {amount:N0} TL'sini ödedin. (Kalan borç: {character.Finance.Debt:N0} TL)");

            if (character.Finance.Debt == 0)
            {
                character.AddLifeEvent("Tüm borçlarını ödedin!");
                GameManager.Instance.StatSystem.ModifyStat(character, StatType.Happiness, 20f);
            }
        }

        public void TakeLoan(CharacterData character, float amount)
        {
            // Kredi limiti kontrolü (gelire göre)
            float maxLoan = character.Career.Salary * 24;
            if (character.Finance.Debt + amount > maxLoan)
            {
                character.AddLifeEvent("Kredi limitini aştın!");
                return;
            }

            character.Finance.Debt += amount;
            GameManager.Instance.StatSystem.ModifyStat(character, StatType.Money, amount);

            character.AddLifeEvent($"{amount:N0} TL kredi çektin. (Toplam borç: {character.Finance.Debt:N0} TL)");
        }

        public void Gamble(CharacterData character, float amount)
        {
            if (character.Stats.Money < amount)
            {
                character.AddLifeEvent("Kumar için yeterli paran yok!");
                return;
            }

            // Kumar sonucu
            float outcome = Random.value;

            if (outcome < 0.4f) // %40 kazanma
            {
                float winnings = amount * Random.Range(1.5f, 3f);
                GameManager.Instance.StatSystem.ModifyStat(character, StatType.Money, winnings - amount);
                character.AddLifeEvent($"Kumarda {winnings:N0} TL kazandın!");
                GameManager.Instance.StatSystem.ModifyStat(character, StatType.Happiness, 15f);
            }
            else // %60 kaybetme
            {
                GameManager.Instance.StatSystem.ModifyStat(character, StatType.Money, -amount);
                character.AddLifeEvent($"Kumarda {amount:N0} TL kaybettin!");
                GameManager.Instance.StatSystem.ModifyStat(character, StatType.Happiness, -10f);
            }

            GameManager.Instance.StatSystem.ModifyStat(character, StatType.Karma, -3f);
        }

        public void InvestInStock(CharacterData character, float amount)
        {
            if (character.Stats.Money < amount)
            {
                character.AddLifeEvent("Yatırım için yeterli paran yok!");
                return;
            }

            GameManager.Instance.StatSystem.ModifyStat(character, StatType.Money, -amount);

            // Yatırım sonucu (yıl sonunda)
            float returnRate = Random.Range(-0.3f, 0.5f);
            float returns = amount * (1 + returnRate);

            GameManager.Instance.StatSystem.ModifyStat(character, StatType.Money, returns);

            if (returnRate > 0)
            {
                character.AddLifeEvent($"Borsa yatırımından {(returns - amount):N0} TL kâr ettin!");
                GameManager.Instance.StatSystem.ModifyStat(character, StatType.Happiness, 10f);
            }
            else
            {
                character.AddLifeEvent($"Borsa yatırımından {(amount - returns):N0} TL zarar ettin!");
                GameManager.Instance.StatSystem.ModifyStat(character, StatType.Happiness, -8f);
            }
        }

        public List<CarOption> GetAvailableCars()
        {
            return new List<CarOption>
            {
                new CarOption { Name = "Fiat Egea", Price = 300000 },
                new CarOption { Name = "Renault Clio", Price = 350000 },
                new CarOption { Name = "Volkswagen Polo", Price = 450000 },
                new CarOption { Name = "Toyota Corolla", Price = 600000 },
                new CarOption { Name = "Honda Civic", Price = 700000 },
                new CarOption { Name = "BMW 3 Serisi", Price = 1500000 },
                new CarOption { Name = "Mercedes C Serisi", Price = 1800000 },
                new CarOption { Name = "Porsche 911", Price = 5000000 },
                new CarOption { Name = "Ferrari", Price = 10000000 }
            };
        }

        private string GetRandomDistrict()
        {
            string[] districts = {
                "Kadıköy", "Beşiktaş", "Bakırköy", "Ataşehir",
                "Çankaya", "Bornova", "Nilüfer", "Muratpaşa"
            };
            return districts[Random.Range(0, districts.Length)];
        }
    }

    public class CarOption
    {
        public string Name;
        public float Price;
    }
}
