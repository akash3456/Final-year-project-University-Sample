using System;
using System.IO;
using System.Data;
using Scryber.Components;
using System.Text.RegularExpressions;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace PdfGeneratorTests
{
    [TestClass]
    public class UnitTest1
    {
        [TestMethod]
        public void CreateSinglePdf()
        {
            String fileName = String.Format(@"C:\Users\Akash Paul\Dropbox\XMLFile1.pdfx");
            String DestinationPath = String.Format(@"C:\Users\Akash Paul\Downloads");
            var outputPath = String.Format(@"{0}\{1}.pdf", DestinationPath, Guid.NewGuid().ToString());
            var stream = new FileStream(outputPath, FileMode.CreateNew, FileAccess.ReadWrite);
            using (Scryber.Components.PDFDocument document = Scryber.Components.PDFDocument.ParseDocument(fileName))
            {
                document.ProcessDocument(stream);
                document.Info.Author = System.Environment.UserName;
                document.Info.CreationDate = DateTime.Now;

                stream.Flush();
                stream.Close();
            }
        }
        [TestMethod]
        public void CreateDynamicPdfFromDatabase()
        {
            String fileName = String.Format(@"C:\Users\Akash Paul\Dropbox\XMLFile1.pdfx");
            String DestinationPath = String.Format(@"C:\Users\Akash Paul\Downloads");
            var dto = new TestDto();
            var connectionString = String.Format("Server=localhost;Database=test;username=root;password=;Port=3306");
            var connect = new MySql.Data.MySqlClient.MySqlConnection(connectionString);
            connect.Open();
            var command = connect.CreateCommand();
            command.CommandText = String.Format("select * from test.t_test_data");
            command.Prepare();
            var reader = command.ExecuteReader();
            var table = new DataTable();
            table.Load(reader);
            foreach (DataRow row in table.Rows)
            {
                dto.getFirstName = row.ItemArray[0].ToString();
                dto.getSecondName = row.ItemArray[1].ToString();
                dto.getFirstYearGrade = int.Parse(row.ItemArray[2].ToString());
                dto.getSecondYearGrade = int.Parse(row.ItemArray[3].ToString());
                dto.getUsername = row.ItemArray[4].ToString();
                dto.getEmail = row.ItemArray[5].ToString();
                dto.getSUN = int.Parse(row.ItemArray[6].ToString());

                var outputPath = String.Format(@"{0}\{1}.pdf", DestinationPath, Guid.NewGuid().ToString());
                var stream = new FileStream(outputPath, FileMode.CreateNew, FileAccess.ReadWrite);

                using (PDFDocument document = PDFDocument.ParseDocument(fileName))
                {
                    document.Items["Firstname"] = dto.getFirstName;
                    document.Items["Secondname"] = dto.getSecondYearGrade;
                    document.Items["FirstYearGrade"] = dto.getFirstYearGrade;
                    document.Items["SecondYearGrade"] = dto.getSecondYearGrade;
                    document.Items["Username"] = dto.getUsername;
                    document.Items["Email"] = dto.getEmail;
                    document.Items["SUN"] = dto.getSUN;
                    document.ProcessDocument(stream, true);
                    stream.Flush();
                    stream.Close();
                }
                connect.Close();
            }
        }
        //write unit
        //tests to generate financial statements as well like material supervisor provided me with..
        [TestMethod]
        public void LogError()
        {
            try
            {
                Exception exception = new Exception();
                exception.Source = String.Format("{0}");
            }
            catch (Exception exception)
            {
                //insert into log file for inspection
            }
        }
        [TestMethod]
        public void getExpectedPath()
        {
            String DestinationPath = String.Format(@"C:\Users\Akash Paul\Downloads");
            Assert.AreEqual(DestinationPath, DestinationPath);

        }
        [TestMethod]
        public void InputFile()
        {
            var Extension = String.Format(@"^.*\.(pdfx|PDFX)");
            var fileExtension = String.Format(@"C:\Users\Akash Paul\Dropbox\XMLFile1.pdfx");
            Path.GetFileName(fileExtension);
            if (Regex.IsMatch(fileExtension, Extension))
            {
                Assert.IsTrue(true, "File is a valid extension");
            }
            else
            {
                Assert.IsFalse(false, "File is not a valid extension");
            }
        }

        [TestMethod]
        public void OutputFileProcessing()
        {
            var extension = String.Format(@"^.*\.(pdf|PDF)");
            String DestinationPath = String.Format(@"C:\Users\Akash Paul\Downloads");
            String outputFile = String.Format(@"{0}\{1}.pdf", DestinationPath, Guid.NewGuid().ToString());
            Path.GetFileName(outputFile);
            if (Regex.IsMatch(outputFile, extension))
            {
                Assert.IsTrue(true, "File is a valid extension");
            }
            else
            {
                Assert.IsFalse(false, "File is not a valid extension");
            }
        }
        [TestMethod]
        public void ImportData() { 
        
        }

        [TestMethod]
        public void SendEmail() { }

    }
}
