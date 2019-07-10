using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BudgetBuddy2
{
    public class Account
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public double Balance { get; set; }
        public DateTime LastImportDate { get; set; }
        public List<AccountDetail> Details { get; private set; } = new List<AccountDetail>();
    }

    public class AccountDetail
    {
        public DateTime Date { get; set; }
        public string Description { get; set; }
        public double Amount { get; set; }
        public bool Reconciled { get; set; }
    }
}
