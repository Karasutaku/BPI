using BPIWebApplication.Shared.MainModel.PettyCash;

namespace BPIWebApplication.Shared.PagesModel.PettyCash
{
    public class ReimbursementExpense
    {
        public Expense expense { get; set; } = new();
        public List<BPIWebApplication.Shared.MainModel.Stream.FileStream> filestreams { get; set; } = new();
    }
    
    public class ReimbursementMultiSelectStatusUpdate
    {
        public string docType { get; set; } = string.Empty;
        public string documentID { get; set; } = string.Empty;
        public string statusValue { get; set; } = string.Empty;
        public string approver { get; set; } = string.Empty;
        public string documentLocation { get; set; } = string.Empty;
        public string currentDocumentStatus { get; set; } = string.Empty;
    }
}
