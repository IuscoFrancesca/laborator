using System.Collections.Generic;
using System.Net;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;
using Microsoft.WindowsAzure.Storage;
using Microsoft.WindowsAzure.Storage.Table;
using Models;
using Newtonsoft.Json;
using Azure.Storage.Queues;

namespace L06WebApi
{
    public class StudentsRepository : IStudentsRepository
    {
        private string connectionString;
        private CloudTableClient tableClient;
        private CloudTable studentsTable;

        private async Task InitializeTable()
        {
            var account = CloudStorageAccount.Parse(connectionString);
            tableClient = account.CreateCloudTableClient();
            studentsTable = tableClient.GetTableReference("studenti");
            await studentsTable.CreateIfNotExistsAsync();
        }

        public StudentsRepository(IConfiguration configuration)
        {
            connectionString = configuration.GetValue(typeof(string), "AzureStorageAccountConnectionString").ToString();

            Task.Run(async () => { await InitializeTable(); }).GetAwaiter().GetResult();
        }

        public async Task<List<StudentEntity>> GetAllStudents()
        {
            var student = new List<StudentEntity>();
            TableQuery<StudentEntity> query = new TableQuery<StudentEntity>();
            TableContinuationToken token = null;
            
            do
            {
                TableQuerySegment<StudentEntity> segment = await studentsTable.ExecuteQuerySegmentedAsync( query, token );
                token = segment.ContinuationToken;
                student.AddRange( segment.Results );
            }while( token != null );

            return student;
        }

        public async Task<StudentEntity> GetStudent(string id)
        {
            var parsedId = ParseStudentId( id );
            var partitionKey = parsedId.Item1;
            var rowKey = parsedId.Item2;
            var query = TableOperation.Retrieve<StudentEntity>( partitionKey, rowKey );
            var result = await studentsTable.ExecuteAsync( query );
            return (StudentEntity)result.Result;

        }

        public async Task AddNewStudent(StudentEntity student)
        {
            /*var addOperation=TableOperation.Insert(student);
            await studentsTable.ExecuteAsync(addOperation);*/

           var jsonStudent = JsonConvert.SerializeObject(student);
            var plainTextBytes = System.Text.Encoding.UTF8.GetBytes(jsonStudent);
            var base64String = System.Convert.ToBase64String(plainTextBytes);

            QueueClient queueClient = new QueueClient(
                connectionString,
                "coada"
            );

            queueClient.CreateIfNotExists();
            await queueClient.SendMessageAsync(base64String);
        }

        public async Task DeleteStudent(string id)
        {
             var parsedId = ParseStudentId( id );
            var partitionKey = parsedId.Item1;
            var rowKey = parsedId.Item2;
            
            var entity = new DynamicTableEntity(partitionKey, rowKey)
            {
                ETag="*"
            };
            await studentsTable.ExecuteAsync(TableOperation.Delete(entity));
            
        }

        public async Task EditStudent(StudentEntity student)
        {
            var editOperation=TableOperation.Merge(student);

            try
            {
                await studentsTable.ExecuteAsync(editOperation);
            }
            catch(StorageException e)
            {
                if(e.RequestInformation.HttpStatusCode==(int)HttpStatusCode.PreconditionFailed)
                throw new System.Exception("Studentul a fost deja modificat. Te rog sa reincarci");
            }
        }


        
        private(string, string) ParseStudentId(string id)
        {
            var element = id.Split('-');

            return( element[0], element[1] );
        }

    }
}