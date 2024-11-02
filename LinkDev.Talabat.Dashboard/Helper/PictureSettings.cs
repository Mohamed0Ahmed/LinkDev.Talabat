namespace LinkDev.Talabat.Dashboard.Helper
{
    public static class PictureSettings
    {


        public static string UploadFile(IFormFile file, string folderName)
        {
            if (file == null || string.IsNullOrWhiteSpace(folderName))
                throw new ArgumentException("Invalid file or folder name");

            var fileName = Guid.NewGuid() + Path.GetExtension(file.FileName);
            var folderPath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "images", folderName.TrimEnd('/'));

            if (!Directory.Exists(folderPath))
                Directory.CreateDirectory(folderPath);

            var filePath = Path.Combine(folderPath, fileName);

            using (var fs = new FileStream(filePath, FileMode.Create))
            {
                file.CopyTo(fs);
            }

            return $"/images/{folderName.Trim('/')}/{fileName}";
        }


        public static void DeleteFile(string folderName, string fileName)
        {
            if (string.IsNullOrWhiteSpace(folderName) || string.IsNullOrWhiteSpace(fileName))
                throw new ArgumentException("Folder name and file name cannot be null or empty");

            var filePath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "images", folderName, fileName);

            if (File.Exists(filePath))
            {
                try
                {
                    File.Delete(filePath);
                }
                catch (Exception ex)
                {
                    throw new IOException("Failed to delete file", ex);
                }
            }
        }

    }
}
