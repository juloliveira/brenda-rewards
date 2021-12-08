using Amazon.S3;
using Amazon.S3.Model;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Threading.Tasks;

namespace Brenda.Infrastructure.AWS
{
    public class AwsUploader : IDisposable
    {
        private readonly AmazonS3Client _awsClient;
        const string BucketName = "brendarewards";

        public AwsUploader()
        {
            _awsClient = new AmazonS3Client("AKIAUENCKRUASG4INK4A", "OTvYq5UQNXfM7+HNKvODwodarz8DVKzqJcuhecnv", Amazon.RegionEndpoint.USEast2);
        }

        public void Dispose()
        {
            _awsClient.Dispose();
        }

        public async Task<string> WritingAnObjectAsync(string key, Stream fileStream, string fileType)
        {
            try
            {
                var putRequest1 = new PutObjectRequest
                {
                    BucketName = BucketName,
                    Key = key,
                    InputStream = fileStream,
                    ContentType = fileType
                };

                var response = await _awsClient.PutObjectAsync(putRequest1);
                return $"https://brendarewards.s3.us-east-2.amazonaws.com/{key}";
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

            return null;
        }
    }
}
