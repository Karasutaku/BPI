namespace BPIDA.Models.MainModel.Procedure.Filter
{
    public class AccessHistoryFilter
    {
        public int pageNo { get; set; } = 0;
        public int rowPerPage { get; set; } = 0;
        public string filterType { get; set; } = string.Empty;
        public string filterDetails { get; set; } = string.Empty;
    }
}
