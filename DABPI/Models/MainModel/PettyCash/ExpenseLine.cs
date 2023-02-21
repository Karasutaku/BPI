namespace BPIDA.Models.MainModel.PettyCash
{
    public class ExpenseLine
    {
        public string ExpenseID { get; set; } = string.Empty;
        public int LineNo { get; set; } = 0;
        public string Details { get; set; } = string.Empty;
        public decimal Amount { get; set; } = decimal.Zero;
        public decimal ActualAmount { get; set; } = decimal.Zero;
        //public string Attach { get; set; } = string.Empty;
        public string Status { get; set; } = string.Empty;
    }
}
