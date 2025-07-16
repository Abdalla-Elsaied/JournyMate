using Microsoft.AspNetCore.Hosting;

namespace JourneyMate.Application.Helper
{
    public class DocumentSetting
    {
        private readonly IWebHostEnvironment _webHostEnvironment;

        public DocumentSetting(IWebHostEnvironment webHostEnvironment)
        {
            _webHostEnvironment = webHostEnvironment;
        }
        public string UploadFile(IFormFile file, string FolderName)
        {
            string folderPath = Path.Combine(_webHostEnvironment.WebRootPath, FolderName);

            string fileExtension = Path.GetExtension(file.FileName).ToLower();

            string fileName = $"{Guid.NewGuid().ToString()}{fileExtension}";

            if (!Directory.Exists(folderPath))
                Directory.CreateDirectory(folderPath);

            string fullPath = Path.Combine(folderPath, fileName);

            using var fileStream = new FileStream(fullPath, FileMode.Create);
            file.CopyTo(fileStream);

            return $"/{FolderName}/{fileName}";
        }

        public void DeleteFile(string fileUrl)
        {
            if (string.IsNullOrWhiteSpace(fileUrl))
                return;

            // Normalize the path: remove starting slash and use OS-specific separator
            var relativePath = fileUrl.TrimStart('/').Replace('/', Path.DirectorySeparatorChar);
            var fullPath = Path.Combine(_webHostEnvironment.WebRootPath, relativePath);

            if (File.Exists(fullPath))
            {
                File.Delete(fullPath);
            }
        }

        public bool check(IFormFile Image, string ImageUrl)
        {
            bool isSameImage = false;

            string existingImagePath = Path.Combine(_webHostEnvironment.WebRootPath, ImageUrl.TrimStart('/'));
            if (File.Exists(existingImagePath))
            {

                using var existingFileStream = new FileStream(existingImagePath, FileMode.Open, FileAccess.Read);
                using var newFileStream = Image.OpenReadStream();

                isSameImage = AreFilesIdentical(existingFileStream, newFileStream);
            }

            return isSameImage;
            bool AreFilesIdentical(Stream file1, Stream file2)
            {
                if (file1.Length != file2.Length)
                    return false;

                int file1Byte, file2Byte;
                do
                {
                    file1Byte = file1.ReadByte();
                    file2Byte = file2.ReadByte();
                }
                while (file1Byte == file2Byte && file1Byte != -1);

                return file1Byte == -1;
            }
        }
    }
}
