using System;

namespace P01_BillsPaymentSystem.Data.Models
{
    public class CreditCard
    {
        public int CreditCardId { get; set; }
        public decimal Limit { get; set; }
        public decimal MoneyOwned { get; set; }
        public decimal LimitLeft => Limit - MoneyOwned;
        public DateTime ExpirationDate { get; set; }

        public int PaymentMethodId { get; set; }
        public PaymentMethod PaymentMethod { get; set; }

        public void Withdraw(decimal moneyToDrow)
        {
            this.MoneyOwned += moneyToDrow;
        }

        public void Deposit(decimal moneyToDeposit)
        {
            this.MoneyOwned -= moneyToDeposit;
        }
    }
}