using P01_BillsPaymentSystem.Data;
using P01_BillsPaymentSystem.Data.Models;
using System;
using System.Globalization;
using System.Linq;

namespace P01_BillsPaymentSystem.App

{
    public class StartUp
    {
        public static void Main(string[] args)
        {
            using (var db = new BillsPaymentSystemContext())
            {

                // db.Database.EnsureDeleted();
                // db.Database.Migrate();
                //Seed(db);

                var userId = int.Parse(Console.ReadLine());
                Details(userId);

                userId = int.Parse(Console.ReadLine());
                var billAmmount = decimal.Parse(Console.ReadLine());
                PayBills(userId, billAmmount);
            }
        }

        private static void PayBills(int userId, decimal billAmmount)
        {
            using (var db = new BillsPaymentSystemContext())
            {
                var bankAccounts = db.PaymentMethods
                    .Where(pm => pm.UserId == userId)
                    .Where(pm => pm.Type == PaymentType.BankAccount)
                    .Select(pm => pm.BankAccount).ToList();

                var creditCards = db.PaymentMethods
                    .Where(pm => pm.UserId == userId)
                    .Where(pm => pm.Type == PaymentType.CreditCard)
                    .Select(pm => pm.CreditCard).ToList();

                var bankAvailability = bankAccounts.Select(ba => ba.Balance).Sum();
                var creditCardAvailability = creditCards.Select(cc => cc.LimitLeft).Sum();

                if (billAmmount > bankAvailability + creditCardAvailability)
                {
                    Console.WriteLine("Insufficient funds!");
                    return;
                }
                else
                {
                    for (int i = 0; i < bankAccounts.Count && billAmmount > 0; i++)
                    {
                        if (billAmmount <= bankAccounts[i].Balance)
                        {
                            bankAccounts[i].Withdraw(billAmmount);
                            billAmmount = 0;
                            db.SaveChanges();
                            return;
                        }
                        else
                        {
                            var amountPart = bankAccounts[i].Balance;
                            bankAccounts[i].Withdraw(amountPart);
                            billAmmount -= amountPart;
                        }
                    }
                    for (int i = 0; i < creditCards.Count && billAmmount > 0; i++)
                    {
                        if (billAmmount <= creditCards[i].LimitLeft)
                        {
                            creditCards[i].Withdraw(billAmmount);
                            billAmmount = 0;
                            db.SaveChanges();
                            return;
                        }
                        else
                        {
                            var amountPart = creditCards[i].LimitLeft;
                            creditCards[i].Withdraw(amountPart);
                            billAmmount -= amountPart;
                        }
                    }
                    db.SaveChanges();
                }
            }
        }

        private static void Details(int userId)
        {
            using (var db = new BillsPaymentSystemContext())
            {
                var user = db.Users
                .Where(u => u.UserId == userId)
                .Select(u => new
                {
                    Name = u.FirstName + " " + u.LastName,
                    CreditCards = u.PaymentMethods
                        .Where(pm => pm.Type == PaymentType.CreditCard)
                        .Select(pm => pm.CreditCard).ToList(),
                    BankAccounts = u.PaymentMethods
                        .Where(pm => pm.Type == PaymentType.BankAccount)
                        .Select(ba => ba.BankAccount).ToList(),
                })
                .FirstOrDefault();

                if (user == null)
                {
                    Console.WriteLine($"User with id {userId} not found!");
                    return;
                }
                Console.WriteLine($"User: {user.Name}");

                var bankAccount = user.BankAccounts;
                if (bankAccount.Any())
                {
                    Console.WriteLine($"Bank Accounts:");
                    foreach (var ba in bankAccount)
                    {
                        Console.WriteLine($@"-- ID: {ba.BankAccountId}");
                        Console.WriteLine($@"-- Balance: {ba.Balance:F2} ");
                        Console.WriteLine($@"-- Bank: {ba.BankName}");
                        Console.WriteLine($@"-- SWIFT: {ba.SwiftCode}");
                    }
                }
                var cretitCards = user.CreditCards;
                if (cretitCards.Any())
                {
                    Console.WriteLine($"Credit Cards:");
                    foreach (var cc in cretitCards)
                    {
                        Console.WriteLine($@"-- ID: {cc.CreditCardId}");
                        Console.WriteLine($@"-- Money Owned: {cc.MoneyOwned:F2} ");
                        Console.WriteLine($@"-- Limit Left: {cc.LimitLeft:F2}");
                        Console.WriteLine($@"-- Expiration Date: {
                                cc.ExpirationDate.ToString("yyyy/MM", CultureInfo.InvariantCulture)
                            }");
                    }
                }
            }
        }

        private static void Seed(BillsPaymentSystemContext db)
        {
            using (db)
            {
                var user = new User()
                {
                    FirstName = "Mario",
                    LastName = "Slavov",
                    Email = "Mario@mario.bg",
                    Password = "somepass"
                };

                var creditCards = new CreditCard[]
                {
                    new CreditCard()
                    {
                        ExpirationDate = DateTime.ParseExact("19.04.2018", "dd.MM.yyyy", CultureInfo.InvariantCulture),
                        Limit = 1000m,
                        MoneyOwned = 5m
                    },
                    new CreditCard()
                    {
                        ExpirationDate = DateTime.ParseExact("26.10.2020", "dd.MM.yyyy", CultureInfo.InvariantCulture),
                        Limit = 400m,
                        MoneyOwned = 200m
                    },
                };

                var bankAccount = new BankAccount()
                {
                    Balance = 1500m,
                    BankName = "My Bank",
                    SwiftCode = "MB"
                };

                var paymentMethods = new PaymentMethod[]
                {
                    new PaymentMethod()
                    {
                        User = user,
                        CreditCard = creditCards[0],
                        Type = PaymentType.CreditCard
                    },
                    new PaymentMethod()
                    {
                        User = user,
                        CreditCard = creditCards[1],
                        Type = PaymentType.CreditCard
                    },
                    new PaymentMethod()
                    {
                        User = user,
                        BankAccount = bankAccount,
                        Type = PaymentType.BankAccount
                    },
                };

                db.Users.Add(user);
                db.CreditCards.AddRange(creditCards);
                db.BankAccounts.Add(bankAccount);
                db.PaymentMethods.AddRange(paymentMethods);

                db.SaveChanges();
            }
        }
    }
}