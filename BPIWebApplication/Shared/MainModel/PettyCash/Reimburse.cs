using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BPIWebApplication.Shared.MainModel.PettyCash
{
    public class Reimburse
    {
        public string ReimburseID { get; set; } = string.Empty;
        public DateTime ReimburseDate { get; set; } = DateTime.Now;
        public string ReimburseStatus { get; set; } = string.Empty;
        public string ReimburseAttach { get; set; } = string.Empty;

        public string LocationID { get; set; } = string.Empty;
        public Location Location { get; set; } = new Location();

        public string Applicant { get; set; } = string.Empty;

    }
}
