using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace BudgetBuddy2.Pages
{
    public class ReconcileModel : PageModel
    {
        private readonly DatabaseContext dbContext;

        public ReconcileModel(DatabaseContext dbContext)
        {
            this.dbContext = dbContext;
        }

        [BindProperty]
        public List<SelectListItem> SelectableBudgets { get; set; }

        [BindProperty]
        public Budget SelectedBudget { get; set; }

        [BindProperty]
        public List<SelectListItem> SelectableAccounts { get; set; }

        [BindProperty]
        public Account SelectedAccount { get; set; }

        public async Task<IActionResult> OnGetAsync()
        {
            var budgets = dbContext.GetBudgets();
            SelectableBudgets = new List<SelectListItem>();
            foreach (var b in budgets)
            {
                SelectableBudgets.Add(new SelectListItem { Text = $"{b.Name} ({b.Id})", Value = b.Id.ToString() });
            }

            var accounts = dbContext.GetAccounts();
            SelectableAccounts = new List<SelectListItem>();
            foreach (var a in accounts)
            {
                SelectableAccounts.Add(new SelectListItem { Text = $"{a.Name} ({a.Id})", Value = a.Id.ToString() });
            }

            return Page();
        }

        public async Task<IActionResult> OnPostReconcileAsync()
        {
            var accountId = SelectableAccounts.FirstOrDefault(a => a.Selected)?.Value;
            var budgetId = SelectableBudgets.FirstOrDefault(b => b.Selected)?.Value;

            if (accountId != null && int.TryParse(accountId, out int accountIdValue))
            {
                SelectedAccount = dbContext.GetAccount(accountIdValue);
            }

            if (budgetId != null && int.TryParse(budgetId, out int budgetIdValue))
            {
                SelectedBudget = dbContext.GetBudget(budgetIdValue);
            }

            return Page();
        }
    }
}