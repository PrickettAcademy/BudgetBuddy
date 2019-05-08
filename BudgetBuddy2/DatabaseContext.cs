using System;
using System.Collections.Generic;
using System.Linq;
using LiteDB;

namespace BudgetBuddy2
{
    public class DatabaseContext
    {
        private LiteDatabase liteDB;
        private LiteCollection<Budget> Budgets { get; set; }

        public DatabaseContext()
        {
            liteDB = new LiteDatabase(@"budgetdata.db");
            Budgets = liteDB.GetCollection<Budget>("budgetsV1");
        }

        public List<Budget> GetBudgets()
        {
            return Budgets.FindAll().OrderByDescending(b => b.Id).ToList();
        }

        public Budget GetBudget(int id)
        {
            return Budgets.FindById(id) ??
                new Budget
                {
                    Id = 0,
                    Name = "April 2019",
                    Groups =
                    {
                        new BudgetGroup
                        {
                            Id = "1",
                            Name = "Income",
                            Incoming = true,
                            Items =
                            {
                                new BudgetItem { Id = "1-1", Name = "Paycheck", ActualAmount = 4000, BudgetAmount = 4000 }
                            }
                        },
                        new BudgetGroup
                        {
                            Id = "2",
                            Name = "Bills",
                            Incoming = false,
                            Items =
                            {
                                new BudgetItem { Id = "2-1", Name = "House payment", BudgetAmount = 1000 },
                                new BudgetItem { Id = "2-2", Name = "Groceries", BudgetAmount = 1200 },
                            }
                        },
                    },
                    Actuals =
                    {
                        new ActualItem
                        {
                            ItemId = "1-1",
                            Id = 1,
                            Date = DateTime.Now.AddDays(-2),
                            Description = "My paycheck",
                            Amount = 4004,
                        },
                        new ActualItem
                        {
                            ItemId = "2-2",
                            Id = 2,
                            Date = DateTime.Now.AddDays(-2),
                            Description = "Harris Teeter",
                            Amount = 40,
                        },
                        new ActualItem
                        {
                            ItemId = "2-2",
                            Id = 3,
                            Date = DateTime.Now.AddDays(-1),
                            Description = "Walmart",
                            Amount = 220,
                        },
                    }
                };
        }

        public void PostBudget(Budget budget)
        {
            if (budget.Id <= 0)
            {
                budget.Id = Budgets.Count() + 1;
            }

            Budgets.Upsert(budget.Id, budget);
        }
    }
}