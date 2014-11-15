using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MySql.Data;
using Scryber.Components;
using Scryber.Styles;
using System.IO;
using System.Diagnostics.Tracing;

namespace PDFGenerator
{
    //will require the latest version of mongodb installed on pc or remote pc
    class Repository
    {
        private String outputPath;
        //find a way of logging errors as well. 

        //public static void StartMysqlService()
        //{
            //ServiceController for the program for automating the running of the mysql service and can i find a way of not having to install mysql and a user can do it that way?? Lecturers must have access to that data locally.

        //}
        public Repository(string outputpath) {
            this.outputPath = outputpath;
        }
        public void ConnectToDb()
        {
            try
            {
                String connectionString = String.Format("Server=localhost;Database=test;username=root;password=;Port=3306");
                MySql.Data.MySqlClient.MySqlConnection connect = new MySql.Data.MySqlClient.MySqlConnection(connectionString);
                connect.ConnectionString = connectionString;
                connect.Open();
            }
            catch (Exception exception)
            {
                //log error
                Console.WriteLine(String.Format(exception.StackTrace));
            }
        }
        public PDFDocument getAll()//return a row in the db table and carry out file stream processing for each file. 
        {
            return null;//return all rows and carry out a pdf document for each row open and close off each file per row.   Streams for each row inside loop.
        }
        public int GetbyID(String outputPath)
        {
            String connectionString = String.Format("Server=localhost;Database=test;username=root;password=;Port=3306");
            using (MySql.Data.MySqlClient.MySqlConnection connect = new MySql.Data.MySqlClient.MySqlConnection(connectionString))
            {
                connect.ConnectionString = connectionString;
                connect.Open();
                var command = connect.CreateCommand();
                command.CommandText = String.Format("SELECT SUN from TEST.T_TEST_DATA");
                var reader = command.ExecuteReader();
                var dto = new SampleDto();
                IEnumerable<SampleDto> read = null;
                while (reader.Read())
                {
                    foreach (var temp in read)
                    {
                        
                        //in order to do one row at a time per file maybe do a sql to linq abstraction?????
                        dto.getSUN = Convert.ToInt32(reader["SUN"]);
                        //level of abstraction needs to be separated out and this is where i would have a loop and the stream to write to for each and every row. 
                        //dto.getFirstName = reader["F_FirstName"].ToString();
                        //dto.getSecondName = reader["F_SecondName"].ToString();
                        //dto.getFirstYearGrade = Convert.ToInt32(reader["F_FirstYearGrade"]);
                        //dto.getSecondYearGrade = Convert.ToInt32(reader["F_SecondYearGrade"])
                        //dto.
                        //reader["SUN"] = int.TryParse(temp.getSUN());//reusbale methods for parsing dbvalues.
                    }
                }
               return dto.getSUN;
            }
        }
    }
}
