using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BPIWebApplication.Shared.MainModel.PettyCash
{
    public class Advance
    {
        public string AdvanceID { get; set; } = string.Empty;
        public DateTime AdvanceDate { get; set; } = DateTime.Now;
        public string AdvanceStatus { get; set; } = string.Empty;

        public string LocationID { get; set; } = string.Empty;
        public string Applicant { get; set; } = string.Empty; // from audit
    }
}
