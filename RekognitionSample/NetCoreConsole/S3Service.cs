using Amazon;
using Amazon.Rekognition;
using Amazon.Rekognition.Model;
using Amazon.Runtime;
using Amazon.S3;
using Amazon.S3.Model;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace NetCoreConsole
{
    public class S3Service
    {
        private const string userName = "ytabuchi";
        private const string bucketName = "ytabuchi-rekognition-from-api";
        // Example creates two objects (for simplicity, we upload same file twice).
        // You specify key names for these objects.
        private const string keyName1 = "sample";
        private const string keyName2 = "pic2";
        private const string filePath = @"D:\drive.jpg";
        private static readonly RegionEndpoint bucketRegion = RegionEndpoint.APNortheast1;

        private const string accessKey = "AKIAJEHKKDSAWHBGZVPQ";
        private const string secretKey = "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";

        public async Task WritingAnObjectAsync()
        {
            try
            {
                using (var client = new AmazonS3Client(accessKey, secretKey, bucketRegion))
                {
                    // 1. Put object-specify only key name for the new object.
                    var putRequest1 = new PutObjectRequest
                    {
                        BucketName = bucketName,
                        Key = userName + "/" + keyName1,
                        FilePath = filePath,
                    };
                    putRequest1.StreamTransferProgress += progressHandler;

                    var response1 = await client.PutObjectAsync(putRequest1);

                    //var putRequest2 = new PutObjectRequest
                    //{
                    //    BucketName = bucketName,
                    //    Key = keyName2,
                    //    FilePath = filePath
                    //};


                }
            }
            catch (AmazonS3Exception e)
            {
                Console.WriteLine(
                        "Error encountered ***. Message:'{0}' when writing an object"
                        , e.Message);
            }
            catch (Exception e)
            {
                Console.WriteLine(
                    "Unknown encountered on server. Message:'{0}' when writing an object"
                    , e.Message);
            }
        }

        private void progressHandler(object sender, StreamTransferProgressArgs e)
        {
            Debug.WriteLine($"{e.PercentDone}% Done");
        }

        public async Task GetXxxInfoAsync()
        {
            using (var client = new AmazonS3Client(accessKey, secretKey, bucketRegion))
            {
                var request = new ListObjectsRequest
                {
                    BucketName = bucketName,
                    Prefix = userName
                };
                var res = await client.ListObjectsAsync(request);
                
                foreach(var o in res.S3Objects)
                {
                    Console.WriteLine(o.Key);
                }
            }
        }

        public async Task GetS3ObjectAsync()
        {
            try
            {
                using(var client = new AmazonS3Client(accessKey, secretKey, bucketRegion))
                {
                    var tokenSource = new CancellationTokenSource();
                    var cancelToken = tokenSource.Token;

                    using (var res = await client.GetObjectAsync(bucketName, keyName1))
                    {
                        await res.WriteResponseStreamToFileAsync(@"D:\res.jpg", false, cancelToken);
                    }
                }
            }
            catch (AmazonS3Exception e)
            {
                Console.WriteLine(
                        "Error encountered ***. Message:'{0}' when reading an object"
                        , e.Message);
            }
            catch (Exception e)
            {
                Console.WriteLine(
                    "Unknown encountered on server. Message:'{0}' when reading an object"
                    , e.Message);
            }
        }

        public async Task GetFaceDetailAsync()
        {
            try
            {
                using(var client = new AmazonRekognitionClient(accessKey, secretKey, RegionEndpoint.APNortheast1))
                {
                    var request = new DetectFacesRequest
                    {
                        Image = new Image
                        {
                            S3Object = new Amazon.Rekognition.Model.S3Object
                            {
                                Bucket = bucketName,
                                Name = keyName1
                            }
                        },
                        Attributes = new List<string> { "ALL" }
                    };

                    var response = await client.DetectFacesAsync(request);

                    
                }
            }
            catch (AmazonRekognitionException e)
            {
                Console.WriteLine(
                        "Error encountered ***. Message:'{0}' when detecting an image"
                        , e.Message);
            }
            catch (Exception e)
            {
                Console.WriteLine(
                    "Unknown encountered on server. Message:'{0}' when writing an object"
                    , e.Message);
            }
        }
    }
}
