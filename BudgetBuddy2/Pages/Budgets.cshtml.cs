using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace BudgetBuddy2.Pages
{
    public class BudgetsModel : PageModel
    {
        private readonly DatabaseContext dbContext;

        public BudgetsModel(DatabaseContext dbContext)
        {
            this.dbContext = dbContext;
        }

        [BindProperty]
        public List<Budget> Budgets { get; set; }

        public async Task<IActionResult> OnGetAsync()
        {
            Budgets = dbContext.GetBudgets();
            return Page();
        }

        public async Task<IActionResult> OnPostDeleteAsync()
        {
            return Page();
        }
    }
}