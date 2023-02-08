using BPIWebApplication.Shared.MainModel.PettyCash;

namespace BPIWebApplication.Shared.PagesModel.PettyCash
{
    public class ReimbursementExpense
    {
        public Expense expense { get; set; } = new();
        public List<BPIWebApplication.Shared.MainModel.Stream.FileStream> filestreams { get; set; } = new();
    }
}
