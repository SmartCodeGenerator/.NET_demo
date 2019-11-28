namespace BlackCaviarBank.Infrastructure.Data.DTOs
{
    public class PaginatedTransactionsDTO
    {
        public FilteredTransactionListDTO FilteredTransactionList { get; set; }
        public PageDTO Page { get; set; }
    }
}
