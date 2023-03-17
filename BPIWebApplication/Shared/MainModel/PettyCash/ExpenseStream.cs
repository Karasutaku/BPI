using BPIWebApplication.Shared.DbModel;

namespace BPIWebApplication.Shared.MainModel.PettyCash
{
    public class ExpenseStream
    {
        public QueryModel<Expense> expenseDetails { get; set; } = new QueryModel<Expense>();
        public List<BPIWebApplication.Shared.MainModel.Stream.FileStream> files { get; set; } = new List<BPIWebApplication.Shared.MainModel.Stream.FileStream>();
    }
}
