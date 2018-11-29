using Amazon;
using Amazon.Rekognition;
using Amazon.Rekognition.Model;
using Amazon.S3;
using Amazon.S3.Model;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace RekognitionSample.Core
{
    public class S3Service
    {
        public async Task<int?> WriteS3ObjectFromLocalFileAsync(string keyName, string filePath)
        {
            try
            {
                using (var client = new AmazonS3Client(Secrets.AccessKey, Secrets.SecretKey, Secrets.Region))
                {
                    var request = new PutObjectRequest
                    {
                        BucketName = Secrets.BucketName,
                        Key = keyName,
                        FilePath = filePath
                    };
                    var res = await client.PutObjectAsync(request);

                    return (int)res.HttpStatusCode;
                }
            }
            catch (AmazonS3Exception e)
            {
                Debug.WriteLine($"Error Message: {this.ToString()} : {e.Message}");
            }
            catch (Exception e)
            {
                Debug.WriteLine($"Unkown Error Message: {this.ToString()} : {e.Message}");
            }

            return null;
        }

        public async Task<List<string>> GetAllObjectsInfoAsync()
        {
            var list = new List<string>();

            try
            {
                using (var client = new AmazonS3Client(Secrets.AccessKey, Secrets.SecretKey, Secrets.Region))
                {
                    var request = new ListObjectsRequest
                    {
                        BucketName = Secrets.BucketName,
                        //Prefix = directory
                    };
                    var res = await client.ListObjectsAsync(request);

                    foreach (var o in res.S3Objects)
                    {
                        list.Add(o.Key);
                    }

                    return list;
                }

            }
            catch (AmazonS3Exception e)
            {
                Debug.WriteLine($"Error Message: {this.ToString()} : {e.Message}");
            }
            catch (Exception e)
            {
                Debug.WriteLine($"Unkown Error Message: {this.ToString()} : {e.Message}");
            }

            return list;
        }

        public async Task GetS3ObjectAsync(string keyName)
        {
            try
            {
                using (var s3Client = new AmazonS3Client(Secrets.AccessKey, Secrets.SecretKey, Secrets.Region))
                {
                    var cts = new CancellationTokenSource();
                    var ct = cts.Token;

                    using (var res = await s3Client.GetObjectAsync(Secrets.BucketName, keyName))
                    {
                        await res.WriteResponseStreamToFileAsync(@"D:\res.jpg", false, ct);
                    }
                }
            }
            catch (AmazonS3Exception e)
            {
                Debug.WriteLine($"Error Message: {this.ToString()} : {e.Message}");
            }
            catch (Exception e)
            {
                Debug.WriteLine($"Unkown Error Message: {this.ToString()} : {e.Message}");
            }
        }
    }
}
