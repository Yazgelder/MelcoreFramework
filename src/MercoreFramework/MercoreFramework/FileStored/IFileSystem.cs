using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;

namespace MercoreFramework.FileStored
{
    public interface IFileSystem
    {
        /// <summary>
        /// Kovadaki / Klasördeki Tüm dosyaları Temizler
        /// </summary>
        /// <returns>Task</returns>
        Task DeleteAll();

        /// <summary>
        /// Adı verilen dosyası siler
        /// </summary>
        /// <param name="name">Silinecek dosya adı</param>
        /// <returns> Silebilrise true silemezse false döndürür </returns>
        Task<bool> DeleteFile(string name);

        /// <summary>
        /// İstenen dosyayı stream olarak getirir
        /// </summary>
        /// <param name="name">Getirilmesi istenen dosya</param>
        /// <returns> Dosyanın stream hali</returns>
        Task<Stream> Get(string name);

        /// <summary>
        /// Kovada / Klasörde ana dizinde olan dosyaların hepsini getirir
        /// </summary>
        /// <returns>Dosya listesi</returns>
        Task<List<IFileInfo>> GetList();

        /// <summary>
        /// Kovada / Klasörde ki verilen başlangıca/Alt klasöre göre getirir
        /// </summary>
        /// <param name="prefix">Alt Klasör adı</param>
        /// <returns>Dosya listesi</returns>
        Task<List<IFileInfo>> GetList(string prefix);

        /// <summary>
        /// Dosyanın kontrolunu yapar
        /// </summary>
        /// <param name="name">Kontrol edilecek dosyanın adı</param>
        /// <returns>Dosya varsa true yoksa false</returns>
        Task<bool> IsExities(string name);

        /// <summary>
        /// Dosya kayıt eder
        /// </summary>
        /// <param name="stream">Dosyanın datası</param>
        /// <param name="name">Dosyanın Kaydedilmesi istenen adı</param>
        /// <returns>task</returns>
        Task SaveFile(Stream stream, string name);

        /// <summary>
        /// Dosya kayıt eder
        /// </summary>
        /// <param name="tempName">Dosyanın localdeki adresi</param>
        /// <param name="name">Dosyanın Kaydedilmesi istenen adı</param>
        /// <returns>task</returns>
        Task SaveFile(string tempName, string name);
    }
}