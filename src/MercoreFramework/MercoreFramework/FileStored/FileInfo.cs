namespace MercoreFramework.FileStored
{
    public class FileInfo : IFileInfo
    {
        /// <summary>
        /// Dosyanın Kovası / Klasörü
        /// </summary>
        public string BucketName { get; set; }

        /// <summary>
        /// Dosyanın Full Adı
        /// </summary>
        public string FullName { get; set; }

        /// <summary>
        /// Dosya Adı
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// Dosyanın Büyüklüğü
        /// </summary>
        public long Size { get; set; }
    }
}