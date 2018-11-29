﻿using RekognitionSample.Core;
using System;
using System.Diagnostics;

namespace NetCoreConsole
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Hello World!");

            var s3 = new S3Service();
            s3.WritingAnObjectAsync().Wait();
            //s3.GetS3ObjectAsync().Wait();
            //s3.GetFaceDetailAsync().Wait();
            s3.GetXxxInfoAsync().Wait();

            //var rekognition = new RekognitionService();
            //var task = rekognition.GetFaceDetailsFromLocalFileAsync(@"D:\family.jpg");
            //var res = task.Result;

            //Debug.WriteLine($"DEBUG: {res.Count} detected.");

            //var i = 1;
            //foreach (var face in res)
            //{
            //    Console.WriteLine($"No. {i}\n" +
            //        $"Gender: {face.Gender}, Confidence: {face.GenderConfidence}%\n" +
            //        $"AgeRange {face.AgeRangeLow} ~ {face.AgeRangeHigh}\n" +
            //        $"Happiness: {face.HappinessConfidence}%\n\n");
            //    i++;
            //}


            Console.ReadLine();
        }
    }
}
