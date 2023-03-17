namespace BPIDA.Models.MainModel.PettyCash
{
    public class PettyCashLedger
    {
        public DateTime TransactionDate { get; set; } = DateTime.Now;
        public string TransactionType { get; set; } = string.Empty;
        public string DocumentID { get; set; } = string.Empty;
        public string LocationID { get; set; } = string.Empty;
        public decimal Amount { get; set; } = decimal.Zero;
        public string Actor { get; set; } = string.Empty;
        public string ExternalDocument { get; set; } = string.Empty;
        public string Applicant { get; set; } = string.Empty;
        public DateTime DocumentDate { get; set; } = DateTime.Now;
        public string DepartmentID { get; set; } = string.Empty;
        public string Note { get; set; } = string.Empty;
        public string NIK { get; set; } = string.Empty;
        public string Type { get; set; } = string.Empty;
        public string BankAccount { get; set; } = string.Empty;
        public string Status { get; set; } = string.Empty;
    }

    public class ledgerParam
    {
        public DateTime startDate { get; set; } = DateTime.Now;
        public DateTime endDate { get; set; } = DateTime.Now;
        public string locationID { get; set; } = string.Empty;
    }
}
