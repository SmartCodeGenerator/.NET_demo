using BlackCaviarBank.Domain.Core;
using System.Collections.Generic;

namespace BlackCaviarBank.Infrastructure.Data.DTOs
{
    public class FilteredTransactionListDTO
    {
        public IEnumerable<Transaction> Transactions { get; set; }
        public string From { get; set; }
        public string To { get; set; }
    }
}
