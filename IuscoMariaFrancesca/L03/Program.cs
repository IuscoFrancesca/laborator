using Google.Apis.Auth.OAuth2;
using Google.Apis.Drive.v3;
using Google.Apis.Util.Store;
using Newtonsoft.Json.Linq;
using System;
using System.IO;
using System.Net;
using System.Threading;

namespace L03
{
    class Program
    {
        private static DriveService _service;
        private static string _token1;
        static void Main(string[] args)
        {
            Initialize();
        }

        static void Initialize()
        {
            string[] scopes = new string[] {
                DriveService.Scope.Drive,
                DriveService.Scope.DriveFile
            };


            var clientId = "557798083870-fnimma4bvadahatnll113lloq283ljod.apps.googleusercontent.com";
            var clientSecret = "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";

            var credentials = GoogleWebAuthorizationBroker.AuthorizeAsync(
                new ClientSecrets
                {
                    ClientId = clientId,
                    ClientSecret = clientSecret
                },
                scopes,
                Environment.UserName,
                CancellationToken.None,

                new FileDataStore("Daimto.GoogleDrive.Auth.Store")
                ).Result;

            _service = new DriveService(new Google.Apis.Services.BaseClientService.Initializer()
            {
                HttpClientInitializer = credentials
            });

            _token1 = credentials.Token.AccessToken;

            Console.Write("Token:" + credentials.Token.AccessToken);

            GetMyFiles();
            UploadFile();
        }
        
        static void GetMyFiles()
        {
            var request = (HttpWebRequest)WebRequest.Create("https://www.googleapis.com/drive/v3/files?q='root'%20in%20parents");
            request.Headers.Add(HttpRequestHeader.Authorization, "Bearer " + _token1);
            request.Method = "GET";

            using(var response = request.GetResponse())
            {
                using ( Stream data = response.GetResponseStream() )
                using ( var reader = new StreamReader(data) )
                {
                    string text = reader.ReadToEnd();
                    var myData = JObject.Parse(text);
                    foreach (var file in myData["files"])
                    {
                        if(file["mimeType"].ToString()!="application/vnd.google=apps.folder")
                        {
                            Console.WriteLine("File name: " + file["name"]);
                        }
                    }
                }    
            }
        }


        static void UploadFile()
        {
            string fileName="random.txt";
            string fileType="text/plain";
            var file = new Google.Apis.Drive.v3.Data.File();
            file.Name=fileName;
            file.MimeType=fileType;
            FilesResource.CreateMediaUpload request;

            using(var stream=new System.IO.FileStream(fileName,System.IO.FileMode.Open))
            {
                request=_service.Files.Create(file,stream,fileType);
                request.Fields="id";
                request.Upload();
            }
            Console.WriteLine(request.ResponseBody);
        }
        
    }
}

