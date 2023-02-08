using BPIFacade.Models.DbModel;

namespace BPIFacade.Models.MainModel.PettyCash
{
    public class ExpenseStream
    {
        public QueryModel<Expense> expenseDetails { get; set; } = new QueryModel<Expense>();
        public List<BPIFacade.Models.MainModel.Stream.FileStream> files { get; set; } = new List<BPIFacade.Models.MainModel.Stream.FileStream>();
    }
}
