using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace BudgetBuddy2
{
    public class Budget
    {
        public int Id { get; set; }

        [Required]
        public string Name { get; set; }
        public IList<BudgetGroup> Groups { get; private set; } = new List<BudgetGroup>();
    }

    public class BudgetGroup
    {
        public string Id { get; set; }

        [Required]
        public string Name { get; set; }
        public bool Incoming { get; set; }
        public IList<BudgetItem> Items { get; private set; } = new List<BudgetItem>();
    }

    public class BudgetItem
    {
        public string Id { get; set; }

        [Required]
        public string Name { get; set; }

        //[DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:c}")]
        public double BudgetAmount { get; set; }

        public IList<ActualItem> Actuals { get; private set; } = new List<ActualItem>();

        //[DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:c}")]
        public double ActualAmount
        {
            get
            {
                double sum = 0;
                foreach (var actual in Actuals)
                {
                    sum += actual.Amount;
                }
                return sum;
            }
        }
    }

    public class ActualItem
    {
        public int Id { get; set; }

        //[DataType(DataType.Date)]
        //[DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:d}")]
        public DateTime Date { get; set; }

        public string Description { get; set; }

        //[DataType(DataType.Currency)]
        //[DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:c}")]
        public double Amount { get; set; }
    }
}