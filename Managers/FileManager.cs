
using System.Text;

namespace SubstitutionCipher.Managers
{
    public static class FileManager
    {
        public static string Read(string fileName)
        {
            using (var streamReader = new FileStream(fileName, FileMode.Open, FileAccess.Read))
            using (var reader = new StreamReader(streamReader, Encoding.UTF8))
            {
                string? data = reader.ReadToEnd();
                if (data is null)
                {
                    throw new Exception("Invalid file");
                }
                return data;
            }
        }

        public static void Write(string fileName, string data)
        {
            using (var streamWriter = new FileStream(fileName, FileMode.Create, FileAccess.Write))
            using (var writer = new StreamWriter(streamWriter, Encoding.UTF8))
            {
                writer.Write(data);
            }
        }
    }
}
