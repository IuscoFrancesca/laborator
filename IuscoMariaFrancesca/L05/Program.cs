using System;
using System.Threading.Tasks;
using Microsoft.WindowsAzure.Storage;
using Microsoft.WindowsAzure.Storage.Table;


namespace L05
{
    class Program
    {
        private static CloudTableClient tableClient;
        private static CloudTable studentsTable;
        private static CloudTable l05Table;
        static void Main(string[] args)
        {
            Task.Run(async () => {await Initialize(); })
            .GetAwaiter()
            .GetResult();
        }

        static async Task Initialize()
        {
            string storageConnectionString_L04 = "DefaultEndpointsProtocol=https;AccountName=azurestoragefrancesca;AccountKey=DfWMwrCY4TTU973D/PZ9iZRw9vVs6WClCq5mn8NLX4SL/cPP2ZkrwTWpHVowYwjwsb6cgcLVfRLia+UConsAAA==;EndpointSuffix=core.windows.net";

            var account = CloudStorageAccount.Parse(storageConnectionString_L04);
            tableClient = account.CreateCloudTableClient();

            studentsTable = tableClient.GetTableReference("studenti");

            l05Table = tableClient.GetTableReference("metrici");

            await studentsTable.CreateIfNotExistsAsync();
            await l05Table.CreateIfNotExistsAsync();

            await GetStudents();
        }

        private static async Task AddNewMetric(string uni, int count)
        {
            var metric = new MetricEntity(uni, DateTime.UtcNow.ToString("yyyy-MM-dd-HH-mm-ss-fffffff"));
            metric.Count = count;

            var addOperation=TableOperation.Insert(metric);
            await l05Table.ExecuteAsync(addOperation);
            
        }

        private static async Task GetStudents()
        {
            //Console.WriteLine("Universitate\tCNP\tNume\tEmail\tNumar Telefon\tAn");
            TableQuery<StudentEntity> query = new TableQuery<StudentEntity>();
            TableContinuationToken token = null;

            string[] universitate= new string[10];
            int[] count = new int[10];

            do 
            {
                TableQuerySegment<StudentEntity> resultSegment = await studentsTable.ExecuteQuerySegmentedAsync(query, token);
                token = resultSegment.ContinuationToken;

                int i=0;
                int k=0;
                int lenght=0;
                foreach(StudentEntity entity in resultSegment.Results)
                {
                    if(entity.PartitionKey!=universitate[k])
                    {
                        k++;
                        universitate[k]=entity.PartitionKey; 
                    }


                    int index = Array.IndexOf(universitate, entity.PartitionKey);

                    if (index > -1)
                    {
                        count[index]++;
                    }
                    else
                    {
                         universitate[i]=entity.PartitionKey;
                        i++;
                    }
                    lenght=index;
                }

               universitate[lenght+1]="General";
               for(int a=0;a<=lenght;a++)
               {
                   count[lenght+1]+=count[a];
               }
                

                for (int j = 1; j <= lenght+1; j++)
                {
                    await AddNewMetric(universitate[j],count[j]);
                }
                
            }while(token != null);

            
        }
    }
}


