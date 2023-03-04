namespace BPIWebApplication.Shared.PagesModel.CashierLogbook
{
    public class AmountTypes
    {
        public string AmountType { get; set; } = string.Empty;
        public string AmountDesc { get; set; } = string.Empty;
    }

    public class AmountCategories
    {
        public string AmountCategoryID { get; set; } = string.Empty;
        public string AmountCategoryName { get; set; } = string.Empty;
    }

    public class AmountSubCategories
    {
        public string AmountSubCategoryID { get; set; } = string.Empty;
        public string AmountSubCategoryName { get; set; } = string.Empty;
        public string AmountType { get; set; } = string.Empty;
    }

    public class CashierLogbookCategories
    {
        public List<AmountTypes> types { get; set; } = new();
        public List<AmountCategories> categories { get; set; } = new();
        public List<AmountSubCategories> subCategories { get; set; } = new();
    }

    public class CashierLogApproval
    {
        public string LogID { get; set; } = string.Empty;
        public int ShiftID { get; set; } = 0;
        public string LocationID { get; set; } = string.Empty;
        public string CreateUser { get; set; } = string.Empty;
        public DateTime CreateDate { get; set; } = DateTime.MinValue;
        public string ConfirmUser { get; set; } = string.Empty;
        public DateTime ConfirmDate { get; set; } = DateTime.MinValue;
        public string ApproveNote { get; set; } = string.Empty;
    }
}
