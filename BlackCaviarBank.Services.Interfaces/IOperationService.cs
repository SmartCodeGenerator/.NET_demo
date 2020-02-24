using BlackCaviarBank.Domain.Core;

namespace BlackCaviarBank.Services.Interfaces
{
    public interface IOperationService
    {
        bool ExecuteFromCardToCard(Card from, Card to, double amount);
        bool ExecuteFromCardToAccount(Card from, Account to, double amount);
        bool ExecuteFromAccountToAccount(Account from, Account to, double amount);
        bool ExecuteFromAccountToCard(Account from, Card to, double amount);
        bool RefundFromCardToCard(Card from, Card to, double amount);
        bool RefundFromCardToAccount(Card from, Account to, double amount);
        bool RefundFromAccountToAccount(Account from, Account to, double amount);
        bool RefundFromAccountToCard(Account from, Card to, double amount);
        bool PayForSubscription(Card card, Service service);
    }
}
