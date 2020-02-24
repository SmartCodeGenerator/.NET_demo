using BlackCaviarBank.Domain.Core;
using BlackCaviarBank.Services.Interfaces;

namespace BlackCaviarBank.Infrastructure.Business
{
    public class FinancialOperationService : IOperationService
    {
        public bool ExecuteFromCardToCard(Card from, Card to, double amount)
        {
            if(from.Balance >= amount)
            {
                to.Balance += amount;
                from.Balance -= amount;

                return true;
            }
            else
            {
                return false;
            }
        }

        public bool ExecuteFromCardToAccount(Card from, Account to, double amount)
        {
            if (from.Balance >= amount)
            {
                to.Balance += amount;
                from.Balance -= amount;

                return true;
            }
            else
            {
                return false;
            }
        }

        public bool ExecuteFromAccountToAccount(Account from, Account to, double amount)
        {
            if (from.Balance >= amount)
            {
                to.Balance += amount;
                from.Balance -= amount;

                return true;
            }
            else
            {
                return false;
            }
        }

        public bool ExecuteFromAccountToCard(Account from, Card to, double amount)
        {
            if (from.Balance >= amount)
            {
                to.Balance += amount;
                from.Balance -= amount;

                return true;
            }
            else
            {
                return false;
            }
        }

        public bool RefundFromCardToCard(Card from, Card to, double amount)
        {
            if (from.Balance >= amount)
            {
                from.Balance -= amount;
                to.Balance += amount;

                return true;
            }
            else
            {
                return false;
            }
        }

        public bool RefundFromCardToAccount(Card from, Account to, double amount)
        {
            if (from.Balance >= amount)
            {
                from.Balance -= amount;
                to.Balance += amount;

                return true;
            }
            else
            {
                return false;
            }
        }

        public bool RefundFromAccountToAccount(Account from, Account to, double amount)
        {
            if (from.Balance >= amount)
            {
                from.Balance -= amount;
                to.Balance += amount;

                return true;
            }
            else
            {
                return false;
            }
        }

        public bool RefundFromAccountToCard(Account from, Card to, double amount)
        {
            if (from.Balance >= amount)
            {
                from.Balance -= amount;
                to.Balance += amount;

                return true;
            }
            else
            {
                return false;
            }
        }

        public bool PayForSubscription(Card card, Service service)
        {
            if (card.Balance >= service.Price)
            {
                card.Balance -= service.Price;
                return true;
            }
            else
            {
                return false;
            }
        }
    }
}
