using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace BudgetBuddy2.Pages
{
    public class AccountModel : PageModel
    {
        private readonly DatabaseContext dbContext;

        public AccountModel(DatabaseContext dbContext)
        {
            this.dbContext = dbContext;
        }

        [BindProperty]
        public Account Account { get; set; }

        public async Task<IActionResult> OnGetAsync(int id)
        {
            if (id <= 0)
            {
                Account = new Account()
                {
                    Name = "New Account",
                };
            }
            else
            {
                Account = dbContext.GetAccount(id);
            }

            if (Account == null)
            {
                return new NotFoundResult();
            }

            return Page();
        }

        public async Task<IActionResult> OnPostSaveChangesAsync()
        {
            dbContext.PostAccount(Account);
            return new RedirectToPageResult("Account", Account.Id);
        }
    }
}