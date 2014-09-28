using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MongoDB.Bson;
using MongoDB.Driver;
using System.IO;

namespace PDFGenerator
{
    //will require the latest version of mongodb installed on pc or remote pc
    class Repository
    {
        public void CreateMongoService() {
        //parse command line arguments and run service which will create and install service provided mongodb is installed. 
        }
        public void ConnectToDatabase() {
            try {
                var connectionString = "mongodb://localhost"; 
                MongoServer mongo = MongoServer.Create(connectionString);  //client is thread safe
                mongo.Connect();
                var database = mongo.GetDatabase("PdfGenerate");
            }
            catch(Exception exception){
               //log the errors to a file if any exist
                //StreamWriter writer = new StreamWriter();
                //writer.
            }
        
        }
    }
}
