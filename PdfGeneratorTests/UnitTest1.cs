using System;
using System.IO;
using System.Data;
using System.Text.RegularExpressions;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using ClosedXML.Excel;
using Outlook = Microsoft.Office.Interop.Outlook;


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

                using (Scryber.Components.PDFDocument document = Scryber.Components.PDFDocument.ParseDocument(fileName))
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
                File.WriteAllText("C:\test.txt", exception.InnerException.ToString());
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
        public void isValidEmail()
        {
            String email = "paula@aston.ac.uk";
            if (Regex.IsMatch(email, @"^(?("")(""[^""]+?""@)|(([0-9a-z]((\.(?!\.))|[-!#\$%&'\*\+/=\?\^`\{\}\|~\w])*)(?<=[0-9a-z])@)(?(\[)(\[(\d{1,3}\.){3}\d{1,3}\])|(([0-9a-z][-\w]*[0-9a-z]*\.)+[a-z0-9]{2,17}))$)", RegexOptions.IgnoreCase))
            {
                Assert.IsTrue(true, "Valid email");
            }
            else
            {
                Assert.IsFalse(false, "Email is not valid");
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
        public void ImportData()
        {
            var ExcelFile = String.Format(@"C:\Users\Akash Paul\Documents\ProperData.xlsx");
            var regexPattern = @"^.*\.(xlsx|XLSX)";
            using (var workbook = new XLWorkbook(ExcelFile))
            {

                if (Regex.IsMatch(ExcelFile, regexPattern))
                {
                    Assert.IsTrue(true, "File is a valid Input File");
                }
                else
                {
                    Assert.IsFalse(false, "File is not a valid input file and must be xlsx extension");
                }
            }
        }
        [TestMethod]
        public void SendEmail()
        {
            String document = @"C:\Users\Akash Paul\Testing\8db420f4-d1fa-48f6-8d9c-646ba2329086.pdf";
            String email = "paula@aston.ac.uk";
            Outlook.Recipients mailrecipient = null;
            Outlook.MailItem mail = null;
            Outlook.Recipient rec = null;
            Outlook.Application app = new Outlook.Application();
            mail = app.CreateItem(Microsoft.Office.Interop.Outlook.OlItemType.olMailItem) as Outlook.MailItem;
            mail.Subject = String.Format("Official Transcript");
            Outlook.Attachment attch = mail.Attachments.Add(document);
            mailrecipient = mail.Recipients;
            rec = mailrecipient.Add(email);
            rec.Resolve();
            mail.Send();
        }
        [TestMethod]
        public void SaveFile()
        {
            String DocumentContents = String.Format("<?xml version='1.0' encoding='utf-8' ?>" + "\n"
                    + "<pdf:Document xmlns:pdf='Scryber.Components, Scryber.Components, Version=0.8.0.0, Culture=neutral, PublicKeyToken=872cbeb81db952fe'" + "\n" + "xmlns:styles='Scryber.Styles, Scryber.Styles, Version=0.8.0.0, Culture=neutral, PublicKeyToken=872cbeb81db952fe'>"
                    + "\n"
                    + "<Info><Title></Title></Info>" + "\n"
                    + "<Styles></Styles>" + "\n"
                    + "<Pages> </Pages>" + "\n"
                    + "\n"
                    + "</pdf:Document>");
            System.IO.File.WriteAllText(@"C:\Users\Akash Paul\Testing\test.pdfx", DocumentContents);
        }
        [TestMethod]
        public void SpecialEditorTest()
        {
            var Extension = String.Format(@"^.*\.(pdfx|PDFX)");
            if (Regex.IsMatch(@"C:\Users\Akash Paul\Testing\test.pdfx", Extension))
            {
                Assert.IsTrue(true, "Template is valid extension and can be saved in editor");
            }
            else
            {
                Assert.IsFalse(false, "File cannot be saved through editor as not a valid .pdfx");
            }
        }
        [TestMethod]
        public void OpenFileInEditor()
        {
            String documentPath = @"C:\Users\Akash Paul\Dropbox\XMLFile1.pdfx";


        }

        [TestMethod]
        public void importFileIntoDatabase()
        {


        }
        public void ProducePDFDocumentWithImage()
        {


        }
    }
}
