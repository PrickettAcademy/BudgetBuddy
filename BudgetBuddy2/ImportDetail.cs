using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BudgetBuddy2
{
    public class ImportDetail
    {
        public string Source { get; set; }
        public DateTime Date { get; set; }
        public string Description { get; set; }
        public double Amount { get; set; }
        public int BudgetId { get; set; }
        public string GroupItemId { get; set; }
        public bool Reconciled { get; set; }
    }
}
