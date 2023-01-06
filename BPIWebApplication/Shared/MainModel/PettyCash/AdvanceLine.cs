using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BPIWebApplication.Shared.MainModel.PettyCash
{
    public class AdvanceLine
    {
        public Advance Header { get; set; } = new Advance();
        public int LineNo { get; set; } = 0;
        public string Details { get; set; } = string.Empty;
        public decimal Amount { get; set; } = decimal.Zero;
        public string Status { get; set; } = string.Empty;
    }
}
