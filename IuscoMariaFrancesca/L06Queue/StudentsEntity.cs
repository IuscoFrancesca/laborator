using Microsoft.Azure.Cosmos.Table;

namespace Models
{
    public class StudentsEntity : TableEntity
    {
        public StudentsEntity(string University, string CNP)
        {
            this.PartitionKey=University;
            this.RowKey=CNP;
        }

        public StudentsEntity(){}

        public string FirstName{get;set;}
        public string LastName{get;set;}
        public string Email{get;set;}
        public string PhoneNumber{get;set;}
        public int Year{get;set;}
        public string Faculty{get;set;}

    }
}