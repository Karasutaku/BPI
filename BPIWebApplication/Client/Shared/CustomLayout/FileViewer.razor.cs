using Microsoft.AspNetCore.Components;

namespace BPIWebApplication.Client.Shared.CustomLayout
{
    public partial class FileViewer : ComponentBase
    {
        [Parameter]
        public string? fileName { get; set; }
        [Parameter]
        public string? fileExtension { get; set; }
        [Parameter]
        public byte[]? fileContent { get; set; }

        private static bool showPrevModal { get; set; }

        private readonly string[] documentTypeFileView = { ".pdf" };
        private readonly string[] videoTypeFileExtensions = { ".mp4" };
        private readonly string[] imageTypeFileExtensions = { ".jpg", ".jpeg" };
        private readonly Dictionary<string, string> fileMimeType = new() { 
            { ".pdf", "application/pdf" }, 
            { ".mp4", "video/mp4" },
            { ".jpg", "image/jpeg" },
            { ".jpeg", "image/jpeg" }
        };

        public static void setShowModal(bool val)
        {
            showPrevModal = val;
        }


        //
    }
}
