using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace BudgetBuddy2.Pages
{
    public class AccountsModel : PageModel
    {
        private readonly DatabaseContext dbContext;

        public AccountsModel(DatabaseContext dbContext)
        {
            this.dbContext = dbContext;
        }

        [BindProperty]
        public List<Account> Accounts { get; set; }

        public async Task<IActionResult> OnGetAsync()
        {
            Accounts = dbContext.GetAccounts();
            return Page();
        }

        public async Task<IActionResult> OnPostDeleteAsync()
        {
            return Page();
        }
    }
}