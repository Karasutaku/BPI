using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BPIWebApplication.Shared.MainModel.PettyCash
{
    public class ReimburseLine
    {
        public Reimburse Header { get; set; } = new Reimburse();
        public string ExpenseID { get; set; } = string.Empty;
        public int LineNo { get; set; } = 0;
        public string AccountNo { get; set; } = string.Empty;
        public string Details { get; set; } = string.Empty;
        public decimal Amount { get; set; } = decimal.Zero;
        public decimal ApprovedAmount { get; set; } = decimal.Zero;
        public string Attach { get; set; } = string.Empty;
        public string Status { get; set; } = string.Empty;
    }
}
