using System;
using Microsoft.Azure.Functions.Worker;
using Microsoft.Extensions.Logging;
using Models;
using Newtonsoft.Json;

namespace L06Queue
{
     public static class QueueTrigger1
    {
        [Function("QueueTrigger1")]
        [TableOutput("studenti")]

        public static StudentsEntity Run([QueueTrigger("coada", Connection = "azurestoragefrancesca_STORAGE")] string myQueueItem, FunctionContext context)
        {
            var student = JsonConvert.DeserializeObject<StudentsEntity>(myQueueItem);

            return student;
        }
    }
}
