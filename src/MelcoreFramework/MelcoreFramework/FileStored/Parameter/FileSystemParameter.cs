namespace MelcoreFramework.FileStored.Parameter
{
    public class FileSystemParameter : IFileSystemParameter
    {
        /// <summary>
        /// Url veya sistemdeki kaydedilecek dosyas yolu
        /// </summary>
        public string Address { get; set; }

        /// <summary>
        /// Amazon veya minio için kova adı local sistem için Klasör adı.
        /// </summary>
        public string Bucket { get; set; }

        /// <summary>
        /// Sistem tarafından verilen Key
        /// </summary>
        public string AccessKey { get; set; }

        /// <summary>
        /// Sistem tarafımdan verilen key
        /// </summary>
        public string SecretKey { get; set; }

        /// <summary>
        /// Amazon S3 için verilen Endpoind
        /// </summary>
        public string EndPoint { get; set; }

        /// <summary>
        /// SSL Kullanılıyormu? minio
        /// </summary>
        public bool IsSSL { get; set; }
    }
}