using BPIWebApplication.Shared.MainModel.PettyCash;

namespace BPIWebApplication.Client.Services.PettyCashServices
{
    public interface IPettyCashService
    {
        // data pool
        List<Advance> advances { get; set; }
        List<AdvanceLine> advanceLines { get; set; }
        List<Expense> expenses { get; set; }
        List<ExpenseLine> expenseLines { get; set; }
        List<Reimburse> reimbursments { get; set; }
        List<ReimburseLine> reimburseLines { get; set; }

        // testing
        void getNewDocNumber();
        //void expensesTestData();
        void addAdvanceTestData(Advance header, List<AdvanceLine> data);
        //void reimburseTestData();
        //void getExpenseReimburse(string reimburseID);
    }
}
