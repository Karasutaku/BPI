using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BPIWebApplication.Shared.PagesModel.AddEditProject
{
    public class Project
    {
        public string ProjectName { get; set; } = string.Empty;
        public string ProjectStatus { get; set; } = string.Empty;
        public string ProjectNote { get; set; } = string.Empty;
    }
}
