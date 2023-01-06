using BPIWebApplication.Shared.MainModel.PettyCash;

namespace BPIWebApplication.Client.Services.PettyCashServices
{
    public class PettyCashService : IPettyCashService
    {
        public List<Advance> advances { get; set; } = new List<Advance>();
        public List<AdvanceLine> advanceLines { get; set; } = new List<AdvanceLine>();
        public List<Expense> expenses { get; set; } = new List<Expense>();
        public List<ExpenseLine> expenseLines { get; set; } = new List<ExpenseLine>();
        public List<Reimburse> reimbursments { get; set; } = new List<Reimburse>();
        public List<ReimburseLine> reimburseLines { get; set; } = new List<ReimburseLine>();

        public void getNewDocNumber()
        {
            int latest = 1;
            int activeDigit = latest.ToString().Length;

            string docIdf = "ADV";

            string docDate = DateTime.Now.Year.ToString() + DateTime.Now.Month.ToString() + DateTime.Now.Date.ToString();

            string docDigit = string.Empty;

            if (activeDigit == 1)
            {
                docDigit = "000" + latest.ToString();
            } else if (activeDigit == 2)
            {
                docDigit = "00" + latest.ToString();
            } else if (activeDigit == 3)
            {
                docDigit = "0" + latest.ToString();
            } else if (activeDigit == 4)
            {
                docDigit = latest.ToString();
            }

            string docNumber = docDate + docDigit;




        }
        //public void expensesTestData()
        //{
        //    expenses.Clear();
        //    expenses.Add(new Expense
        //    {
        //        ExpenseID = "001",
        //        ExpenseDetail = "BBM",
        //        ExpenseAmount = 100000,
        //        ExpenseAttach = "asdasdsad",
        //        ExpenseCoa = "",
        //        ReimbursID = "",
        //        Reimbursment = new Reimburs(),
        //        AdvanceID = "123",
        //        ExpenseStatus = "active",
        //        LocationID = "HO",
        //        ExpenseDate = DateTime.Now,
        //        Applicant = ""
        //    });

        //    expenses.Add(new Expense
        //    {
        //        ExpenseID = "002",
        //        ExpenseDetail = "Other",
        //        ExpenseAmount = 200000,
        //        ExpenseAttach = "asdasdsad",
        //        ExpenseCoa = "",
        //        ReimbursID = "",
        //        Reimbursment = new Reimburs(),
        //        AdvanceID = "132",
        //        ExpenseStatus = "active",
        //        LocationID = "HO",
        //        ExpenseDate = DateTime.Now,
        //        Applicant = ""
        //    });

        //    expenses.Add(new Expense
        //    {
        //        ExpenseID = "003",
        //        ExpenseDetail = "Advertising",
        //        ExpenseAmount = 100000,
        //        ExpenseAttach = "asdasdsad",
        //        ExpenseCoa = "",
        //        ReimbursID = "",
        //        Reimbursment = new Reimburs(),
        //        AdvanceID = "231",
        //        ExpenseStatus = "active",
        //        LocationID = "HO",
        //        ExpenseDate = DateTime.Now,
        //        Applicant = ""
        //    });

        //    expenses.Add(new Expense
        //    {
        //        ExpenseID = "004",
        //        ExpenseDetail = "BBM",
        //        ExpenseAmount = 50000,
        //        ExpenseAttach = "asdasdsad",
        //        ExpenseCoa = "",
        //        ReimbursID = "",
        //        Reimbursment = new Reimburs(),
        //        AdvanceID = "321",
        //        ExpenseStatus = "active",
        //        LocationID = "HO",
        //        ExpenseDate = DateTime.Now,
        //        Applicant = ""
        //    });

        //    expenses.Add(new Expense
        //    {
        //        ExpenseID = "005",
        //        ExpenseDetail = "OTHER",
        //        ExpenseAmount = 1000000,
        //        ExpenseAttach = "asdasdsad",
        //        ExpenseCoa = "14045",
        //        ReimbursID = "PCR001",
        //        Reimbursment = new Reimburs(),
        //        AdvanceID = "321",
        //        ExpenseStatus = "active",
        //        LocationID = "HO",
        //        ExpenseDate = DateTime.Now,
        //        Applicant = "bambang"
        //    });

        //    expenses.Add(new Expense
        //    {
        //        ExpenseID = "006",
        //        ExpenseDetail = "MAKAN",
        //        ExpenseAmount = 300000,
        //        ExpenseAttach = "asdasdsad",
        //        ExpenseCoa = "14045",
        //        ReimbursID = "PCR001",
        //        Reimbursment = new Reimburs(),
        //        AdvanceID = "321",
        //        ExpenseStatus = "active",
        //        LocationID = "HO",
        //        ExpenseDate = DateTime.Now,
        //        Applicant = "ujay"
        //    });

        //}

        public void addAdvanceTestData(Advance header, List<AdvanceLine> data)
        {
            advances.Add(header);

            advanceLines.Clear();

            foreach (var dt in data)
            {
                dt.Header = header;
            }

            advanceLines.AddRange(data);
        }

        //public void reimburseTestData()
        //{
        //    reimbursments.Clear();
        //    reimbursments.Add(new Reimburs
        //    {
        //        ReimbursID = "PCR001",
        //        ReimbursDate = DateTime.Now,
        //        ReimbursStatus = "open",
        //        ReimbursAttach = "asd.pdf",
        //        LocationID = "50000",
        //        Location = new Location
        //        {
        //            LocationID = "50000",
        //            LocationName = "Gatsu Bali"
        //        }
        //    });
        //}

        //public void getExpenseReimburse(string reimburseID)
        //{
        //    foreach (var exp in expenses)
        //    {
        //        if (exp.ReimbursID.Contains(reimburseID))
        //        {
        //            expenseReimburse.Add(exp);
        //        }
        //    }
        //}


    }
}
