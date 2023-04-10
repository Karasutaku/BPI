namespace BPIFacade.Models.MainModel.EPKRS
{
    public class ItemCase
    {
        public string RiskID { get; set; } = string.Empty;
        public string DocumentID { get; set; } = string.Empty;
        public string SiteReporter { get; set; } = string.Empty;
        public string SiteSender { get; set; } = string.Empty;
        public DateTime ReportDate { get; set; } = DateTime.Now;
        public DateTime ItemPickupDate { get; set; } = DateTime.Now;
        public string LoadingDocumentID { get; set; } = string.Empty;
        public DateTime LoadingDocumentDate { get; set; } = DateTime.Now;
        public int VarianceDate { get; set; } = 0;
        public bool isLate { get; set; } = false;
        public bool isCCTVCoverable { get; set; } = false;
        public bool isReportedtoSender { get; set; } = false;
        public string DocumentStatus { get; set; } = string.Empty;
    }

    public class ItemLine
    {
        public string DocumentID { get; set; } = string.Empty;
        public int LineNum { get; set; } = 0;
        public string TRID { get; set; } = string.Empty;
        public DateTime TRDate { get; set; } = DateTime.Now;
        public string ItemCode { get; set; } = string.Empty;
        public string ItemDescription { get; set; } = string.Empty;
        public string ItemRiskCategoryID { get; set; } = string.Empty;
        public string CategoryID { get; set; } = string.Empty;
        public int ItemQuantity { get; set; } = 0;
        public string UOM { get; set; } = string.Empty;
        public decimal ItemValue { get; set; } = decimal.Zero;
        public int ItemStock { get; set; } = 0;
    }

    public class IncidentAccident
    {
        public string RiskID { get; set; } = string.Empty;
        public string DocumentID { get; set; } = string.Empty;
        public DateTime ReportDate { get; set; } = DateTime.Now;
        public DateTime OccurenceDate { get; set; } = DateTime.Now;
        public string SiteReporter { get; set; } = string.Empty;
        public string DepartmentReporter { get; set; } = string.Empty;
        public string RiskRPName { get; set; } = string.Empty;
        public string RiskRPEmail { get; set; } = string.Empty;
        public string DORMName { get; set; } = string.Empty;
        public string DORMEmail { get; set; } = string.Empty;
        public string CaseDescription { get; set; } = string.Empty;
        public string DepartmentAffected { get; set; } = string.Empty;
        public string Cronology { get; set; } = string.Empty;
        public string RootCause { get; set; } = string.Empty;
        public string LossDescription { get; set; } = string.Empty;
        public decimal LossEstimation { get; set; } = decimal.Zero;
        public decimal ReturnAmount { get; set; } = decimal.Zero;
        public string RiskDescription { get; set; } = string.Empty;
        public string CauseDescription { get; set; } = string.Empty;
        public string PIC { get; set; } = string.Empty;
        public string ActionPlan { get; set; } = string.Empty;
        public DateTime TargetDate { get; set;} = DateTime.Now;
        public string MitigationPlan { get; set; } = string.Empty;
        public DateTime MitigationDate { get; set; } = DateTime.Now;
        public string ExtendedRootCause { get; set; } = string.Empty;
        public string ExtendedMitigationPlan { get; set; } = string.Empty;
        public string DocumentStatus { get; set; } = string.Empty;
    }

    public class CaseAttachment
    {
        public string DocumentID { get; set; } = string.Empty;
        public int LineNum { get; set; } = 0;
        public DateTime UploadDate { get; set;} = DateTime.Now;
        public string FileExtension { get; set; } = string.Empty;
        public string FilePath { get; set; } = string.Empty;
    }

    public class DocumentDiscussion
    {
        public string DocumentID { get; set; } = string.Empty;
        public string UserName { get; set; } = string.Empty;
        public DateTime CommentDate { get; set; } = DateTime.Now;
        public string Comment { get; set; } = string.Empty;
        public bool isEdited { get; set; } = false;
    }
}
