
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Audio
{
    using System;
    using System.IO;

    class Program
    {
        static void Main(string[] args)
        {

            try
            {
                string fileName = @"C:\Users\serhat.ersoy\Desktop\Serhat\Idris Abkar\stream.m3u";
                var reader = new StreamReader(fileName);

                string line;
                while ((line = reader.ReadLine()) != null)
                {
                    Console.WriteLine(line);
                }
                Console.ReadLine();
            }
            catch (Exception exception)
            {
            }

        }
    }
}
