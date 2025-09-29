using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Lab1._2
{
    internal class FileManager
    {
        public List<string> ReadFileList(string fileName)
        {
            var fileList = new List<string>();

            try
            {
                using (StreamReader reader = new StreamReader(fileName, Encoding.UTF8))
                {
                    string line;
                    while ((line = reader.ReadLine()) != null)
                    {
                        // Видаляємо зайві пробіли та перевіряємо, чи не пуста стрічка
                        line = line.Trim();
                        if (!string.IsNullOrEmpty(line))
                        {
                            fileList.Add(line);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Помилка при читанні файлу {fileName}: {ex.Message}");
            }

            return fileList;
        }

        public string ReadTextFile(string fileName)
        {
            try
            {
                using (StreamReader reader = new StreamReader(fileName, System.Text.Encoding.UTF8))
                {
                    return reader.ReadToEnd();
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Помилка при читанні файлу {fileName}: {ex.Message}");
                return string.Empty;
            }
        }

        
        public bool FileExists(string fileName)
        {
            return File.Exists(fileName);
        }
    }
}
