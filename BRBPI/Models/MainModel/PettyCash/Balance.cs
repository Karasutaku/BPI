namespace BPIBR.Models.MainModel.PettyCash
{
    public class OutstandingBalance
    {
        public string locationID { get; set; } = string.Empty;
        public decimal locationOnhandAmount { get; set; } = decimal.Zero;
        public decimal advanceOutstandingAmount { get; set; } = decimal.Zero;
        public decimal expenseOutstandingAmount { get; set; } = decimal.Zero;
        public decimal advanceApprovedAmount { get; set; } = decimal.Zero;
        public decimal expenseApprovedAmount { get; set; } = decimal.Zero;
        public decimal reimbursementReqOutstandingAmount { get; set; } = decimal.Zero;
        public decimal reimbursementApvOutstandingAmount { get; set; } = decimal.Zero;
        public decimal reimbursementApvRejectedAmount { get; set; } = decimal.Zero;
        public DateTime lastFetch { get; set; } = DateTime.Now;
    }

    public class BalanceDetails
    {
        public string LocationID { get; set; } = string.Empty;
        public decimal BudgetAmount { get; set; } = decimal.Zero;
        public string LatestAuditUser { get; set; } = string.Empty;
        public DateTime AuditDate { get; set; } = DateTime.Now;

    }
    public class LocationBalanceDetails
    {
        public OutstandingBalance outstandingBalance { get; set; } = new();
        public BalanceDetails balanceDetails { get; set; } = new();
        public DateTime CutOffDate { get; set; } = DateTime.Now;
    }
    public class CutoffDetails
    {
        public string LocationID { get; set; } = string.Empty;
        public string ModuleLedgerName { get; set; } = string.Empty;
        public DateTime CutoffDate { get; set; } = DateTime.Now;
    }
}
