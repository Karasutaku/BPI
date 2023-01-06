using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BPIWebApplication.Shared
{
    public class ActiveUser<T>
    {
        public T? UserLogin { get; set; }
        public string Name { get; set; } = string.Empty;
        public string role { get; set; } = string.Empty;
    }
}
