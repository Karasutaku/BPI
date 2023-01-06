using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BPIWebApplication.Shared.FileUploadModel
{
    public class FileReadyDownload
    {
        public string fileName { get; set; } = string.Empty;
        public byte[] content { get; set; } = new byte[0];
    }
}
