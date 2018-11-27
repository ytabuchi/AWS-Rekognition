using Amazon;
using Amazon.Rekognition;
using Amazon.Rekognition.Model;
using System;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;

namespace RekognitionSample.Core
{
    public class RekognitionService
    {
        public async Task GetFacesAsync(string filePath)
        {
            var imageBytes = await EncodeAsync(filePath);
            var memoryImage = new MemoryStream(imageBytes);

            //AWS Rekognition Client を作成
            var rekognitionClient = new AmazonRekognitionClient(Secrets.AccessKey, Secrets.SecretKey, RegionEndpoint.APNortheast1);
            var request = new DetectFacesRequest
            {
                Image = new Image
                {
                    Bytes = memoryImage
                }
            };
            var response = await rekognitionClient.DetectFacesAsync(request);

        }

        private async Task<byte[]> EncodeAsync(string filePath)
        {
            byte[] imageBytes;

            using (var stream = new FileStream(filePath, FileMode.Open, FileAccess.Read))
            {
                imageBytes = new byte[stream.Length];
                await stream.ReadAsync(imageBytes, 0, (int)stream.Length);
            }

            return imageBytes;
        }
    }
}
