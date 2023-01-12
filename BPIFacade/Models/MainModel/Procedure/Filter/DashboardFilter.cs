namespace BPIFacade.Models.MainModel.Procedure.Filter
{
    public class DashboardFilter
    {
        public int pageNo { get; set; } = 0;
        public int rowPerPage { get; set; } = 0;
        public string filterNo { get; set; } = string.Empty;
        public string filterName { get; set; } = string.Empty;
        public string filterDept { get; set; } = string.Empty;
        public string filterBU { get; set; } = string.Empty;
    }
}
