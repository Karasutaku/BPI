namespace BPIBR.Models.MainModel.PettyCash
{
    public class ReimburseLine
    {
        public string ReimburseID { get; set; } = string.Empty;
        public string ExpenseID { get; set; } = string.Empty;
        public int LineNo { get; set; } = 0;
        public string AccountNo { get; set; } = string.Empty;
        public string Details { get; set; } = string.Empty;
        public decimal Amount { get; set; } = decimal.Zero;
        public decimal ApprovedAmount { get; set; } = decimal.Zero;
        //public string Attach { get; set; } = string.Empty;
        public string Status { get; set; } = string.Empty;
    }
}
