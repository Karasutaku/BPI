namespace BPIDA.Models.MainModel.CashierLogbook
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
}
