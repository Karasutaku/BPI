using BPIBR.Models.DbModel;

namespace BPIBR.Models.MainModel.PettyCash
{
    public class ExpenseStream
    {
        public QueryModel<Expense> expenseDetails { get; set; } = new QueryModel<Expense>();
        public List<BPIBR.Models.MainModel.Stream.FileStream> files { get; set; } = new List<BPIBR.Models.MainModel.Stream.FileStream>();
    }
}
