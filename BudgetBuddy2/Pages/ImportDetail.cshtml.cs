using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using CsvHelper;
using CsvHelper.Configuration;
using CsvHelper.Configuration.Attributes;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace BudgetBuddy2.Pages
{
    public class ImportDetailModel : PageModel
    {
        private readonly DatabaseContext dbContext;

        public ImportDetailModel(DatabaseContext dbContext)
        {
            this.dbContext = dbContext;
        }

        public IFormFile ImportFile { get; set; }

        [BindProperty]
        public Account Account { get; set; }

        //[BindProperty]
        //public List<SelectListItem> SelectableBudgetItems { get; set; }

        public async Task<IActionResult> OnGetAsync(int id)
        {
            Account = dbContext.GetAccount(id);
            if (Account == null)
            {
                return new NotFoundResult();
            }

            //SelectableBudgetItems = new List<SelectListItem>();
            //foreach (var group in Budget.Groups)
            //{
            //    foreach (var item in group.Items)
            //    {
            //        SelectableBudgetItems.Add(new SelectListItem { Text = $"{group.Name}: {item.Name}" });
            //    }
            //}

            return Page();
        }

        public async Task<IActionResult> OnPostImportFileAsync()
        {
            var importRecords = new List<DataImportRecord>();
            using (var streamReader = new StreamReader(ImportFile.OpenReadStream()))
            {
                CsvReader csv = new CsvReader(streamReader, new Configuration() { HasHeaderRecord = true, Delimiter = "," });
                while (csv.Read())
                {
                    DataImportRecord Record = csv.GetRecord<DataImportRecord>();
                    importRecords.Add(Record);
                }
            }

            Account = dbContext.GetAccount(Account.Id);

            foreach (var record in importRecords)
            {
                Account.Details.Add(new AccountDetail()
                {
                    Date = record.Date,
                    Description = record.Description,
                    Amount = ConvertAmount(record.Amount),
                    Reconciled = false,
                });
            }

            return Page();
        }

        private double ConvertAmount(string amount)
        {
            return double.Parse(amount.Replace("$", ""));
        }

        public class DataImportRecord
        {
            public DateTime Date { get; set; }
            public string Description { get; set; }
            public string Comments { get; set; }
            [Name("Check Number")]
            public string CheckNumber { get; set; }
            public string Amount { get; set; }
            public string Balance { get; set; }
            public string Category { get; set; }
            public string Note { get; set; }
        }
    }
}