namespace BPIFacade.Models.MainModel.Procedure.Report
{
    public class AccessHistoryReport
    {
        public DateTime startDate { get; set; } = DateTime.Now;
        public DateTime endDate { get; set; } = DateTime.Now;
        public string procedureNo { get; set; } = string.Empty;
    }
}
