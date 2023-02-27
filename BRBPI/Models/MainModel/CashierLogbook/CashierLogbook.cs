namespace BPIBR.Models.MainModel.CashierLogbook
{
    public class CashierLogData
    {
        public string LogType { get; set; } = string.Empty;
        public string LogID { get; set; } = string.Empty;
        public string LocationID { get; set; } = string.Empty;
        public string Applicant { get; set; } = string.Empty;
        public DateTime LogDate { get; set; } = DateTime.Now;
        public string LogStatus { get; set; } = string.Empty;
        public DateTime LogStatusDate { get; set; } = DateTime.Now;
        public List<CashierLogCategoryDetail> header { get; set; } = new();
    }

    public class CashierLogCategoryDetail
    {
        public string LogID { get; set; } = string.Empty;
        public string BrankasCategoryID { get; set; } = string.Empty;
        public string AmountCategoryID { get; set; } = string.Empty;
        public string AmountCategoryName { get; set; } = string.Empty;
        public decimal HeaderAmount { get; set; } = decimal.Zero;
        public decimal ActualAmount { get; set; } = decimal.Zero;
        public string CategoryNote { get; set; } = string.Empty;
        public bool isLineDeleted { get; set; } = false;
        public List<CashierLogLineDetail> lines { get; set; } = new();
    }

    public class CashierLogLineDetail
    {
        public string BrankasCategoryID { get; set; } = string.Empty;
        public int LineNo { get; set; } = 0;
        public string AmountSubCategoryID { get; set; } = string.Empty;
        public string AmountSubCategoryName { get; set; } = string.Empty;
        public string AmountType { get; set; } = string.Empty;
        public string AmountDesc { get; set; } = string.Empty;
        public int ShiftID { get; set; } = 0;
        public string ShiftDesc { get; set; } = string.Empty;
        public decimal LineAmount { get; set; } = decimal.Zero;
    }

    public class CashierLogbook
    {
        public CashierLogData logDetail { get; set; } = new();
        public List<CashierLogCategoryDetail> logHeaders { get; set; } = new();
        public List<CashierLogLineDetail> logLines { get; set; } = new();
    }
}
