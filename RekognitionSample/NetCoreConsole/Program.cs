using RekognitionSample.Core;
using System;

namespace NetCoreConsole
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Hello World!");

            var rekognition = new RekognitionService();
            rekognition.GetFacesAsync(@"D:\family.jpg").Wait();

            Console.ReadLine();
        }
    }
}
