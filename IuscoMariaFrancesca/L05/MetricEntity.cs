using Microsoft.WindowsAzure.Storage.Table;


namespace L05
{
    public class MetricEntity : TableEntity
    {
        public MetricEntity(string University, string TimeStamp)
        {
            this.PartitionKey=University;
            this.RowKey=TimeStamp;
        }

        public MetricEntity(){}

        public int Count{get;set;}

    }
}
