﻿namespace MelcoreFramework.FileStored.Parameter
{
    public interface IFileSystemParameter
    {
        #region Public Properties

        /// <summary>
        /// Sistem tarafından verilen Key
        /// </summary>
        string AccessKey { get; set; }

        /// <summary>
        /// Url veya sistemdeki kaydedilecek dosyas yolu
        /// </summary>
        string Address { get; set; }

        /// <summary>
        /// Amazon veya minio için kova adı local sistem için Klasör adı.
        /// </summary>
        string Bucket { get; set; }

        /// <summary>
        /// Amazon S3 için verilen Endpoind
        /// </summary>
        string EndPoint { get; set; }

        /// <summary>
        /// Sistem tarafımdan verilen key
        /// </summary>
        string SecretKey { get; set; }

        #endregion Public Properties
    }
}