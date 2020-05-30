namespace MelcoreFramework.FileStored
{
    public interface IFileInfo
    {
        #region Public Properties

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

        #endregion Public Properties
    }
}