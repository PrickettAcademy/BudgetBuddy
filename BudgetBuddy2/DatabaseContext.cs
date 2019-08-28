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
        private LiteCollection<Account> Accounts { get; set; }

        public DatabaseContext()
        {
            liteDB = new LiteDatabase(@"budgetdata.db");
            Budgets = liteDB.GetCollection<Budget>("budgetsV1");
            Accounts = liteDB.GetCollection<Account>("accountsV1");
        }

        public List<Budget> GetBudgets(int recentCount = 12)
        {
            return Budgets.FindAll().OrderByDescending(b => b.Id).Take(recentCount).ToList();
        }

        public Budget GetBudget(int id)
        {
            return Budgets.FindById(id);
        }

        public Budget GetBudgetTemplate()
        {
            return
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
                                new BudgetItem { Id = "1-1", Name = "Paycheck", BudgetAmount = 4000,
                                    Actuals =
                                    {
                                        new ActualItem
                                        {
                                            Id = 1,
                                            Date = DateTime.Now.AddDays(-2),
                                            Description = "My paycheck",
                                            Amount = 4004,
                                        },
                                    }
                                }
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
                                new BudgetItem { Id = "2-2", Name = "Groceries", BudgetAmount = 1200,
                                    Actuals =
                                    {
                                        new ActualItem
                                        {
                                            Id = 1,
                                            Date = DateTime.Now.AddDays(-2),
                                            Description = "Harris Teeter",
                                            Amount = 40,
                                        },
                                        new ActualItem
                                        {
                                            Id = 2,
                                            Date = DateTime.Now.AddDays(-1),
                                            Description = "Walmart",
                                            Amount = 220,
                                        },
                                    }
                                },
                            }
                        },
                    },
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

        public List<Account> GetAccounts(int recentCount = 12)
        {
            return Accounts.FindAll().OrderByDescending(a => a.Id).Take(recentCount).ToList();
        }

        internal Account GetAccount(int id)
        {
            return Accounts.FindById(id);
        }

        internal void PostAccount(Account account)
        {
            if (account.Id <= 0)
            {
                account.Id = Accounts.Count() + 1;
            }

            Accounts.Upsert(account.Id, account);
        }
    }
}