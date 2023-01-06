using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BPIWebApplication.Shared.MainModel.PettyCash
{
    public class Expense
    {
        public string ExpenseID { get; set; } = string.Empty;
        public string AdvanceID { get; set; } = string.Empty; // can be empty
        public string ExpenseStatus { get; set; } = string.Empty;

        public string LocationID { get; set;} = string.Empty;
        public Location Location { get; set; } = new Location();
        
        public string Applicant { get; set; } = string.Empty; // from audit
        public DateTime ExpenseDate { get; set; } = DateTime.Now; // from audit
    }
}
