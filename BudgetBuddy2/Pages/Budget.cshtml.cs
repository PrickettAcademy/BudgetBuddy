using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace BudgetBuddy2.Pages
{
    public class BudgetModel : PageModel
    {
        private readonly DatabaseContext dbContext;

        public BudgetModel(DatabaseContext dbContext)
        {
            this.dbContext = dbContext;
        }

        [BindProperty]
        public Budget Budget { get; set; }

        public async Task<IActionResult> OnGetAsync(int id)
        {
            if (id <= 0)
            {
                Budget = dbContext.GetBudgetTemplate();
            }
            else
            {
                Budget = dbContext.GetBudget(id);
            }

            if (Budget == null)
            {
                return new NotFoundResult();
            }

            return Page();
        }

        public async Task<IActionResult> OnPostSaveChangesAsync()
        {
            dbContext.PostBudget(Budget);
            return new RedirectToPageResult("Budget", Budget.Id);
        }

        public async Task<IActionResult> OnPostImportActualsAsync()
        {
            // Make sure to save the budget first
            dbContext.PostBudget(Budget);

            // Do import for this budget
            // TODO

            // Now redirect to the import page for this budget
            return new RedirectToPageResult("ImportDetail", Budget.Id);
        }

        public async Task<IActionResult> OnPostAddGroupAsync()
        {
            string groupId = $"{Budget.Groups.Count + 1}";
            string itemId = $"{groupId}-1";
            var group = new BudgetGroup { Id = groupId, Name = "new group", Incoming = false };
            group.Items.Add(new BudgetItem { Id = itemId, Name = "new item" });
            Budget.Groups.Add(group);
            return Page();
        }

        public async Task<IActionResult> OnPostAddItemAsync(string group)
        {
            var budgetGroup = Budget.Groups.FirstOrDefault(g => g.Name == group);
            if (budgetGroup != null)
            {
                string itemId = $"{budgetGroup.Id}-{budgetGroup.Items.Count + 1}";
                budgetGroup.Items.Add(new BudgetItem { Id = itemId, Name = "new item" });
            }
            return Page();
        }

        public async Task<IActionResult> OnPostAddActualAsync(string group, string itemId)
        {
            var budgetGroup = Budget.Groups.FirstOrDefault(g => g.Name == group);
            if (budgetGroup != null)
            {
                var item = budgetGroup.Items.FirstOrDefault(i => i.Id == itemId);
                if (item != null)
                {
                    item.Actuals.Add(new ActualItem { Id = item.Actuals.Count + 1, Date = DateTime.Now });
                }
            }
            return Page();
        }
    }
}