using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace BudgetBuddy2
{
    public class Budget
    {
        public Budget()
        {
            Groups = new List<BudgetGroup>();
            Actuals = new List<ActualItem>();
        }

        public int Id { get; set; }

        [Required]
        public string Name { get; set; }
        public IList<BudgetGroup> Groups { get; private set; }
        public IList<ActualItem> Actuals { get; private set; }
    }

    public class BudgetGroup
    {
        public BudgetGroup()
        {
            Items = new List<BudgetItem>();
        }
        public string Id { get; set; }

        [Required]
        public string Name { get; set; }
        public bool Incoming { get; set; }
        public IList<BudgetItem> Items { get; private set; }
    }

    public class BudgetItem
    {
        public string Id { get; set; }

        [Required]
        public string Name { get; set; }
        public double BudgetAmount { get; set; }
        public double ActualAmount { get; set; }
    }

    public class ActualItem
    {
        public string ItemId { get; set; }
        public int Id { get; set; }
        public DateTime Date { get; set; }
        public string Description { get; set; }
        public double Amount { get; set; }
    }
}