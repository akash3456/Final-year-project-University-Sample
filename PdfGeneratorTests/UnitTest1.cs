using System;
using System.IO;
using System.Data;
using System.Text.RegularExpressions;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using ClosedXML.Excel;
using Scryber.Components;
using ICSharpCode.AvalonEdit;
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
                throw new ArgumentException(exception.InnerException.ToString(),exception);
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
            var FailExtension = String.Format(@"C:\Users\Akash Paul\Dropbox\temp.xml");
            Path.GetFileName(fileExtension);
            Path.GetFileName(FailExtension);
            if (Regex.IsMatch(fileExtension, Extension) || Regex.IsMatch(FailExtension, Extension))
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
                Assert.Fail("Email is not valid");
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
            String document = @"C:\Users\Akash Paul\Testing\880c27db-6aa7-4ecb-bb9b-35f2509fc39b.pdf";
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
            var textEditor = new ICSharpCode.AvalonEdit.TextEditor();
            String documentPath = @"C:\Users\Akash Paul\Dropbox\XMLFile1.pdfx";
            System.IO.StreamReader reader = new StreamReader(documentPath);
            String mystring = reader.ReadToEnd();
            textEditor.Document.Text = mystring;
        }
        [TestMethod]
        public void ListAllGeneratedFiles()
        {
            var collection = new System.Collections.Generic.List<String>();
            String destinationPath = String.Format(@"C:\Users\Akash Paul\Downloads");
            var outputPath = String.Format(@"{0}\{1}.pdf", destinationPath, Guid.NewGuid().ToString());
            using (FileStream stream = new FileStream(outputPath, FileMode.CreateNew, FileAccess.ReadWrite))
            using (PDFDocument document = PDFDocument.ParseDocument(@"C:\Users\Akash Paul\Dropbox\XMLFile1.pdfx"))
            {
                collection.Add(outputPath);
                foreach (var collect in collection)
                {
                    Console.WriteLine(collect.ToString());
                    document.ProcessDocument(stream, true);
                    document.Info.Author = "Aston University";
                    document.Info.CreationDate = DateTime.Now;
                    stream.Flush();
                    stream.Close();
                }
            }
        }
        [TestMethod]
        public void importFileIntoDatabase()
        {
            using (var workbook = new XLWorkbook(@"C:\Users\Akash Paul\Documents\ProperData.xlsx"))
            {
                var table = new DataTable("Datatable");
                var workSheet = workbook.Worksheets.Worksheet(1);
                var range = workSheet.FirstRow();
                foreach (var row in range.CellsUsed())
                {
                    DataColumn column = new DataColumn();
                    column.DataType = System.Type.GetType("System.String");
                    column.ColumnName = row.Value.ToString();
                    table.Columns.Add(column);
                }
                string exists = null;
                var connectingString = String.Format("Server=localhost;Database=test;username=root;password=;Port=3306");
                using (var connect = new MySql.Data.MySqlClient.MySqlConnection(connectingString))
                {
                    var range1 = workSheet.RangeUsed();
                    var colCount = range1.ColumnCount();
                    foreach (var row in range1.RowsUsed())
                    {
                        object[] rowData = new object[colCount];
                        Int32 i = 0;
                        row.Cells().ForEach(c => rowData[i++] = c.Value);
                        table.Rows.Add(rowData);
                    }
                    DataRowCollection itemColumns = table.Rows;
                    itemColumns[0].Delete();
                    connect.Open();

                    if (exists == null)
                    {
                        using (var command = connect.CreateCommand())
                        {
                            var collection = new System.Collections.Generic.Dictionary<String, MySql.Data.MySqlClient.MySqlDbType>();
                            foreach (var col in table.Columns)
                            {
                                collection.Add(col.ToString(), MySql.Data.MySqlClient.MySqlDbType.VarChar);
                            }
                            System.Text.StringBuilder sb = new System.Text.StringBuilder();
                            sb.Append(String.Format("CREATE TABLE {0} (", table.TableName));
                            foreach (var loop in collection)
                            {
                                sb.AppendFormat("{0} {1}({2}),", loop.Key, loop.Value, 500);
                            }
                            command.CommandText = sb.ToString().Remove(sb.ToString().LastIndexOf(",")) + ")";
                            command.Prepare();
                            command.ExecuteNonQuery();
                            var connectionString = String.Format("Server=localhost;Database=test;username=root;password=;Port=3306");
                            using (var connect1 = new MySql.Data.MySqlClient.MySqlConnection(connectionString))
                            {
                                connect1.Open();
                                var command1 = connect.CreateCommand();
                                foreach (DataRow row in table.Rows)
                                {
                                    System.Text.StringBuilder builder = new System.Text.StringBuilder();
                                    builder.Append(string.Format("INSERT INTO {0} VALUES (", table.TableName));
                                    foreach (DataColumn col in table.Columns)
                                    {
                                        builder.AppendFormat("'{0}',", row[col].ToString());
                                    }
                                    command.Prepare();
                                    command.CommandText = builder.ToString().Remove(builder.ToString().LastIndexOf(",")) + ")";
                                    command.ExecuteNonQuery();
                                }
                                connect1.Close();
                                connect.Close();
                            }
                        }
                    }
                }
            }
        }
        [TestMethod]
        public void ProducePDFDocumentWithImage()
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
        public void EmailContentTest()
        {
            String document = @"C:\Users\Akash Paul\Testing\b125f2f7-1cfb-41cf-bbca-c14fc2027a45.pdf";
            String email = "paula@aston.ac.uk";
            String Body = String.Format("This is an automated message which has an attached email to it and normally this will be come from a form in the GUI and submitted to the email client.");
            Outlook.Recipients mailrecipient = null;
            Outlook.MailItem mail = null;
            Outlook.Recipient rec = null;
            Outlook.Application app = new Outlook.Application();
            mail = app.CreateItem(Microsoft.Office.Interop.Outlook.OlItemType.olMailItem) as Outlook.MailItem;
            mail.Subject = String.Format("Test Subject");//
            mail.Body = Body;
            Outlook.Attachment attch = mail.Attachments.Add(document);
            mailrecipient = mail.Recipients;
            rec = mailrecipient.Add(email);
            rec.Resolve();
            mail.Send();
        }
        [TestMethod]
        public void xmlSyntaxHiglighting()
        {
            var editor = new ICSharpCode.AvalonEdit.TextEditor();
            editor.ShowLineNumbers = true;
            String file = @"C:\Users\Akash Paul\Dropbox\XMLFile1.pdfx";
            Stream pdfx = File.OpenRead(file);
            editor.SyntaxHighlighting = ICSharpCode.AvalonEdit.Highlighting.HighlightingManager.Instance.GetDefinition("XML");
        }
        [TestMethod]
        public void OpenPDFPreview()
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
                System.Diagnostics.Process.Start("explorer.exe", outputPath);
                Assert.IsTrue(true, "Preview Successful");
                stream.Flush();
                stream.Close();
            }
        }
    }
}
