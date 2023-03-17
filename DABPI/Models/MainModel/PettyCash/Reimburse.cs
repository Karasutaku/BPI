namespace BPIDA.Models.MainModel.PettyCash
{
    public class Reimburse
    {
        public string ReimburseID { get; set; } = string.Empty;
        public string ReimburseNote { get; set; } = string.Empty;
        public DateTime ReimburseDate { get; set; } = DateTime.Now;
        public string ReimburseStatus { get; set; } = string.Empty;
        //public string ReimburseAttach { get; set; } = string.Empty;

        public string LocationID { get; set; } = string.Empty;
        public string Applicant { get; set; } = string.Empty;

        public List<ReimburseLine> lines { get; set; } = new();
        public ReimburseDocumentStatus statusDetails { get; set; } = new();

    }
}
