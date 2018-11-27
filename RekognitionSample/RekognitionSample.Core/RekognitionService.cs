using Amazon;
using Amazon.Rekognition;
using Amazon.Rekognition.Model;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Threading.Tasks;

namespace RekognitionSample.Core
{
    public class RekognitionService
    {
        public async Task<List<DetectedFaceDetail>> GetFacesDetailsFromLocalFileAsync(string filePath)
        {
            var imageBytes = await GenerateImageBytesAsync(filePath);
            var memoryImage = new MemoryStream(imageBytes);

            //AWS Rekognition Client を作成
            var rekognitionClient = new AmazonRekognitionClient(Secrets.AccessKey, Secrets.SecretKey, RegionEndpoint.APNortheast1);
            var request = new DetectFacesRequest
            {
                Image = new Image
                {
                    Bytes = memoryImage
                },
                Attributes = new List<string> { "ALL" }
            };
            var response = await rekognitionClient.DetectFacesAsync(request);

            var faceList = new List<DetectedFaceDetail>();
            foreach (var face in response.FaceDetails)
            {
                float happiness = 0;
                foreach (var e in face.Emotions)
                {
                    if (e.Type == EmotionName.HAPPY)
                        happiness = e.Confidence;
                }

                faceList.Add(new DetectedFaceDetail
                {
                    Gender = face.Gender.Value,
                    GenderConfidence = face.Gender.Confidence,
                    HappinessConfidence = happiness,
                    AgeRangeHigh = face.AgeRange.High,
                    AgeRangeLow = face.AgeRange.Low
                });
            }

            return faceList;
        }

        private async Task<byte[]> GenerateImageBytesAsync(string filePath)
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
