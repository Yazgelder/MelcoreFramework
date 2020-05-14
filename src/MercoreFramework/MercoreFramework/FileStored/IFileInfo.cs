namespace MercoreFramework.FileStored
{
    public interface IFileInfo
    {
        /// <summary>
        /// Dosyanın Kovası / Klasörü
        /// </summary>
        string BucketName { get; set; }

        /// <summary>
        /// Dosyanın Full Adı
        /// </summary>
        string FullName { get; set; }

        /// <summary>
        /// Dosya Adı
        /// </summary>
        string Name { get; set; }

        /// <summary>
        /// Dosyanın Büyüklüğü
        /// </summary>
        long Size { get; set; }
    }
}