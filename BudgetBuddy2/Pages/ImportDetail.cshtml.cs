using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using CsvHelper;
using CsvHelper.Configuration;
using CsvHelper.Configuration.Attributes;
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

        [BindProperty]
        public Budget Budget { get; set; }

        [BindProperty]
        public List<ImportDetail> ImportDetails { get; set; }

        [BindProperty]
        public List<SelectListItem> SelectableBudgetItems { get; set; }

        public async Task<IActionResult> OnGetAsync(int id)
        {
            Budget = dbContext.GetBudget(id);
            if (Budget == null)
            {
                return new NotFoundResult();
            }

            SelectableBudgetItems = new List<SelectListItem>();
            foreach (var group in Budget.Groups)
            {
                foreach (var item in group.Items)
                {
                    SelectableBudgetItems.Add(new SelectListItem { Text = $"{group.Name}: {item.Name}" });
                }
            }

            ImportDetails = dbContext.GetImportDetails(id, "bank", reconciled: false);

            var importRecords = await ImportFromFile(@"C:\Users\jpricket\Downloads\export_20190706.csv");
            foreach (var record in importRecords)
            {
                ImportDetails.Add(new ImportDetail()
                {
                    BudgetId = id,
                    Date = record.Date,
                    Description = record.Description,
                    Amount = ConvertAmount(record.Amount),
                    Source = "Coastal Federal Credit Union",
                    Reconciled = false,
                });
            }

            return Page();
        }

        private double ConvertAmount(string amount)
        {
            return double.Parse(amount.Replace("$", ""));
        }

        private async Task<List<DataImportRecord>> ImportFromFile(string filename)
        {
            var result = new List<DataImportRecord>();
            using (TextReader reader = System.IO.File.OpenText(filename))
            {
                CsvReader csv = new CsvReader(reader, new Configuration() { HasHeaderRecord = true, Delimiter = "," });
                while (csv.Read())
                {
                    DataImportRecord Record = csv.GetRecord<DataImportRecord>();
                    result.Add(Record);
                }
            }

            return result;
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