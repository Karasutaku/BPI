namespace BPIBR.Models.MainModel.CashierLogbook
{
    public class CashierLogAction
    {
        public string LogID { get; set; } = string.Empty;
        public string LocationID { get; set; } = string.Empty;
        public string UserEmail { get; set; } = string.Empty;
        public string LogAction { get; set; } = string.Empty;
        public DateTime ActionDate { get; set; } = DateTime.Now;
    }
}
