using Helper.Classes;
using System.IO;

namespace Database.Classes
{
    public class CreateConnectionString
    {
        /// <summary>
        /// Erzeugt den ConnectionString
        /// </summary>
        /// <returns>ConnectionString</returns>
        public static string Create()
        {
            string folder = Folders.Get("database");
            string file = "radio.db";

            if (!Directory.Exists(folder)) { Directory.CreateDirectory(folder); }

            string fullpath = Path.Join(folder, file);

            return "Data Source=" + fullpath;
        }
    }
}
