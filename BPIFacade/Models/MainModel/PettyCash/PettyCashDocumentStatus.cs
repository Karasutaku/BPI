namespace BPIFacade.Models.MainModel.PettyCash
{
    public class AdvanceDocumentStatus
    {
        public DateTime? submitDate { get; set; } = DateTime.Now;
        public string? submitUser { get; set; } = string.Empty;
        public DateTime? confirmDate { get; set; } = DateTime.Now;
        public string? confirmUser { get; set; } = string.Empty;
        public DateTime? rejectDate { get; set; } = DateTime.Now;
        public string? rejectUser { get; set; } = string.Empty;
    }

    public class ExpenseDocumentStatus
    {
        public DateTime? submitDate { get; set; } = DateTime.Now;
        public string? submitUser { get; set; } = string.Empty;
        public DateTime? confirmDate { get; set; } = DateTime.Now;
        public string? confirmUser { get; set; } = string.Empty;
        public DateTime? rejectDate { get; set; } = DateTime.Now;
        public string? rejectUser { get; set; } = string.Empty;
    }

    public class ReimburseDocumentStatus
    {
        public DateTime? confirmDate { get; set; } = DateTime.Now;
        public string? confirmUser { get; set; } = string.Empty;
        public DateTime? verifyDate { get; set; } = DateTime.Now;
        public string? verifyUser { get; set; } = string.Empty;
        public DateTime? releaseDate { get; set; } = DateTime.Now;
        public string? releaseUser { get; set; } = string.Empty;
        public DateTime? approveDate { get; set; } = DateTime.Now;
        public string? approveUser { get; set; } = string.Empty;
        public DateTime? resolveDate { get; set; } = DateTime.Now;
        public string? resolveUser { get; set; } = string.Empty;
        public DateTime? claimDate { get; set; } = DateTime.Now;
        public string? claimUser { get; set; } = string.Empty;
        public DateTime? rejectDate { get; set; } = DateTime.Now;
        public string? rejectUser { get; set; } = string.Empty;
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
