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
            var task = rekognition.GetFacesDetailsFromLocalFileAsync(@"D:\family.jpg");
            var res = task.Result;

            Console.WriteLine($"{res.Count} detected.");

            var i = 1;
            foreach (var face in res)
            {
                Console.WriteLine($"No. {i}\n" +
                    $"Gender: {face.Gender}, Confidence: {face.GenderConfidence}%\n" +
                    $"AgeRange {face.AgeRangeLow} ~ {face.AgeRangeHigh}\n" +
                    $"Happiness: {face.HappinessConfidence}%\n\n");
                i++;
            }
        }
    }
}
