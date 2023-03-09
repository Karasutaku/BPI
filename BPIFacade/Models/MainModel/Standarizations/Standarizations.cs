namespace BPIFacade.Models.MainModel.Standarizations
{
    public class StandarizationType
    {
        public string TypeID { get; set; } = string.Empty;
        public string Descriptions { get; set; } = string.Empty;
    }
    public class Standarizations
    {
        public string TypeID { get; set; } = string.Empty;
        public string StandarizationID { get; set; } = string.Empty;
        public string StandarizationDetails { get; set; } = string.Empty;
        public DateTime StandarizationDate { get; set; } = DateTime.Now;
        public List<StandarizationTag> Tags { get; set; } = new();
        public List<StandarizationAttachment> Attachments { get; set; } = new();
    }

    public class StandarizationTag
    {
        public string StandarizationID { get; set; } = string.Empty;
        public string TagDescriptions { get; set; } = string.Empty;
    }

    public class StandarizationAttachment
    {
        public string StandarizationID { get; set; } = string.Empty;
        public string Descriptions { get; set; } = string.Empty;
        public DateTime UploadDate { get; set; } = DateTime.Now;
        public string FileExtention { get; set; } = string.Empty;
        public string FilePath { get; set; } = string.Empty;
    }
}
