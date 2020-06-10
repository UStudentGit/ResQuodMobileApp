using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace ResQuod.Helpers
{
    public static class FileHelper
    {
        public static void SaveObjectToFile(object obj, string fileName)
        {
            string json = JsonConvert.SerializeObject(obj);

            string path = Environment.GetFolderPath(Environment.SpecialFolder.Personal);
            string filePath = Path.Combine(path, fileName);

            using (var file = File.Open(filePath, FileMode.Create, FileAccess.Write))
            using (var strm = new StreamWriter(file))
            {
                strm.Write(json);
            }
        }

        public static T ReadObjectFromFile<T>(string fileName)
        {
            string json;

            string path = Environment.GetFolderPath(Environment.SpecialFolder.Personal);
            string filePath = Path.Combine(path, fileName);

            try
            {
                using (var file = File.Open(filePath, FileMode.Open, FileAccess.Read))
                using (var strm = new StreamReader(file))
                {
                    json = strm.ReadToEnd();
                    T ob = JsonConvert.DeserializeObject<T>(json);
                    return ob;

                }
            }
            catch(Exception ex)
            {
                using (var file = File.Open(filePath, FileMode.Create, FileAccess.Write))
                using (var strm = new StreamWriter(file))
                {
                    strm.Write("");
                }

                using (var file = File.Open(filePath, FileMode.Open, FileAccess.Read))
                using (var strm = new StreamReader(file))
                {
                    json = strm.ReadToEnd();
                    T ob = JsonConvert.DeserializeObject<T>(json);
                    return ob;

                }
            }
        }
    }
}
