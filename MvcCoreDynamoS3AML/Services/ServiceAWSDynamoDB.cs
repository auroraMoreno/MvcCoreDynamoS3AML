using Amazon.DynamoDBv2;
using Amazon.DynamoDBv2.DataModel;
using Amazon.DynamoDBv2.DocumentModel;
using MvcCoreDynamoS3AML.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MvcCoreDynamoS3AML.Services
{
    public class ServiceAWSDynamoDB
    {
        private DynamoDBContext context;

        public ServiceAWSDynamoDB()
        {
            AmazonDynamoDBClient client = new AmazonDynamoDBClient();
            this.context = new DynamoDBContext(client);
        }

        public async Task CreateUser(Usuario usuario)
        {
            await this.context.SaveAsync<Usuario>(usuario);
        }

        public async Task<List<Usuario>> GetUsuariosAsync()
        {
            var tabla = this.context.GetTargetTable<Usuario>();
            var scanOptions = new ScanOperationConfig();
            var results = tabla.Scan(scanOptions);

            List<Document> data = await results.GetNextSetAsync();
            IEnumerable<Usuario> users = this.context.FromDocuments<Usuario>(data);
            return users.ToList();
        }

        public async Task<Usuario> FindUserAsync(int iduser)
        {
            return await this.context.LoadAsync<Usuario>(iduser);
        }

        public async Task DeleteUserAsync(int iduser)
        {
            await this.context.DeleteAsync<Usuario>(iduser);
        }

        //hacer método para modificar users aqui
    }
}
