namespace BPIFacade.Models.MainModel.PettyCash
{
    public class AdvanceLine
    {
        public string AdvanceID { get; set; } = string.Empty;
        public int LineNo { get; set; } = 0;
        public string Details { get; set; } = string.Empty;
        public decimal Amount { get; set; } = decimal.Zero;
        public string Status { get; set; } = string.Empty;
    }
}
