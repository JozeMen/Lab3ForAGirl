using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleApp2
{
    internal class Program
    {
        static void Main(string[] args)
        {
            using (FileStream fileStream = new FileStream("temp.bin", FileMode.Open))
            {
                long sectionStart = 1024; // Начало секции
                long sectionLength = 4096; // Длина секции

                using (SectionStreamReader sectionReader = new SectionStreamReader(fileStream, sectionStart, sectionLength))
                {
                    byte[] buffer = new byte[1024];
                    int bytesRead;

                    while ((bytesRead = sectionReader.Read(buffer, 0, buffer.Length)) > 0)
                    {
                        // Обработка прочитанных данных
                        // В данном примере мы просто выводим их в консоль
                        Console.WriteLine($"Read {bytesRead} bytes: {BitConverter.ToString(buffer, 0, bytesRead)}");
                    }
                }
            }
            Console.ReadKey(); 
        }

    }
}
