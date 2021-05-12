using Amazon.S3;
using Amazon.S3.Model;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace MvcCoreDynamoS3AML.Services
{
    public class ServiceAWSS3
    {
        private String bucketName;
        private IAmazonS3 awsClient;

        public ServiceAWSS3(IAmazonS3 awsclient, IConfiguration configuration)
        {
            this.awsClient = awsclient;
            this.bucketName = configuration["AWSS3:BucketName"];
        }

        public async Task<bool> UploadFile(Stream stream,
         String fileName)
        {
            PutObjectRequest request = new PutObjectRequest()
            {
                InputStream = stream,
                Key = fileName,
                BucketName = this.bucketName
            };

            PutObjectResponse response = await this.awsClient.PutObjectAsync(request);
            if (response.HttpStatusCode == System.Net.HttpStatusCode.OK)
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        public async Task<bool> DeleteFile(string filename)
        {
            DeleteObjectResponse response = await this.awsClient.DeleteObjectAsync(this.bucketName, filename);
            if (response.HttpStatusCode == System.Net.HttpStatusCode.NoContent)
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        public async Task<Stream> GetFile(String filename)
        {
            GetObjectResponse response = await this.awsClient.GetObjectAsync(this.bucketName, filename);
            if (response.HttpStatusCode == System.Net.HttpStatusCode.OK)
            {
                return response.ResponseStream;
            }
            else
            {
                return null;
            }
        }

        ////tendría que haber un método getfilesuser para mostrar links de los files

        //public async Task<List<String>> GetS3FilesUser(int idUsuario)
        //{
        //    ListVersionsResponse response =
        //        await this.awsClient.ListVersionsAsync(this.bucketName);
        //    return response.Versions.Select(x => x.Key).ToList();
        //}

    }
}
