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
        public async Task<List<DetectedFaceDetail>> GetFaceDetailsFromLocalFileAsync(string filePath)
        {
            //画像のMemoryStreamを作成
            var imageStream = await GenerateImageStreamFromLocalFileAsync(filePath);
            if (imageStream == null)
                return null;

            try
            {
                //AWS Rekognition Client を作成
                using (var rekognitionClient = new AmazonRekognitionClient(Secrets.AccessKey, 
                                                                           Secrets.SecretKey, 
                                                                           Secrets.Region))
                {
                    var request = new DetectFacesRequest
                    {
                        Image = new Image
                        {
                            Bytes = imageStream
                        },
                        Attributes = new List<string> { "ALL" }
                    };

                    //responseを受け取り、必要な情報を抽出
                    var response = await rekognitionClient.DetectFacesAsync(request);

                    var faceList = new List<DetectedFaceDetail>();
                    foreach (var face in response.FaceDetails)
                    {
                        faceList.Add(new DetectedFaceDetail
                        {
                            Gender = face.Gender.Value,
                            GenderConfidence = face.Gender.Confidence,
                            HappinessConfidence = face.Emotions.Find(x => x.Type.Value == EmotionName.HAPPY).Confidence,
                            AgeRangeHigh = face.AgeRange.High,
                            AgeRangeLow = face.AgeRange.Low
                        });
                    }

                    return faceList;
                }
            }
            catch (Exception e)
            {
                Debug.WriteLine(e.Message);
            }

            return null;
        }



        private async Task<MemoryStream> GenerateImageStreamFromLocalFileAsync(string filePath)
        {
            try
            {
                using (var fileStream = new FileStream(filePath, FileMode.Open, FileAccess.Read))
                {
                    using (var memoryStream = new MemoryStream())
                    {
                        await fileStream.CopyToAsync(memoryStream);

                        return memoryStream;
                    }
                }
            }
            catch (Exception e)
            {
                Debug.WriteLine(e.Message);
            }

            return null;
        }
    }
}
