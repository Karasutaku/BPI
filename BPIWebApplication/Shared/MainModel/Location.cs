namespace BPIWebApplication.Shared.MainModel
{
    public class Location
    {
        public string Condition { get; set; } = string.Empty;
        public int PageIndex { get; set; } = 0;
        public int PageSize { get; set; } = 0;
        public string FieldOrder { get; set; } = string.Empty;
        public string MethodOrder { get; set; } = string.Empty;
    }

    public class LocationResp
    {
        public int companyId { get; set; } = 0;
        public string locationId { get; set; } = string.Empty;
        public string locationName { get; set; } = string.Empty;
    }
}
