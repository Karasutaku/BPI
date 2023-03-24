using System.ComponentModel.DataAnnotations;

namespace BPIWebApplication.Shared.MainModel.Procedure
{
    public class BisnisUnit
    {
        [Key]
        public string BisnisUnitID { get; set; } = string.Empty;
        public string BisnisUnitName { get; set; } = string.Empty;
        public string BisnisUnitLabel { get; set; } = string.Empty;
    }
}
