using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MySql.Data;
using Scryber.Components;
using Scryber.Styles;
using Scryber.Data;
using System.Xml.Linq;
using System.IO;
using System.Diagnostics;
using System.Data;
using System.Windows.Threading;
using System.IO.Compression;
using ClosedXML.Excel;
using System.Text.RegularExpressions;
using System.Net.Mail;
using System.Windows.Controls;
using System.Windows;
using System.Windows.Media.Animation;
using Outlook = Microsoft.Office.Interop.Outlook;



namespace PDFGenerator
{
    class Repository
    {
        String filename;
        String destinationPath;
        public String outputPdfFile;
        List<String> collect = new List<String>();
        List<String> emailAddresses = new List<String>();
        String tableName;
        DataTable table;
        String Subject;
        String Body;

        public Repository(String filename, String DestinationPath, String Subject, String Body)
        {
            table = new DataTable("Datatable");
            this.Subject = Subject;
            this.Body = Body;
            this.filename = filename;
            this.destinationPath = DestinationPath;
        }
        public List<String> getAll()
        {
            SampleDto dto = new SampleDto();
            var ShowEmail = new List<String>();
            String connectionString = String.Format("Server=localhost;Database=test;username=root;password=;Port=3306");
            using (MySql.Data.MySqlClient.MySqlConnection connect = new MySql.Data.MySqlClient.MySqlConnection(connectionString))
            {
                connect.Open();
                var command = connect.CreateCommand();
                command.CommandText = String.Format("SELECT * FROM {0}", table.TableName);
                command.Prepare();
                var reader = command.ExecuteReader();
                var table1 = new DataTable(this.table.TableName);
                table1.Load(reader);
                foreach (DataRow row in table1.Rows)
                {
                    var dictionary = new Dictionary<String, String>();
                    var emailCollect = new List<String>();
                    for (int i = 0, j = 0; (i < table1.Columns.Count) && (j < row.ItemArray.Length); i++, j++)
                    {
                        DataColumn col = table1.Columns[i];
                        var item = row.ItemArray[j];
                        dto.Field1 = item.ToString();
                        dictionary.Add(col.ToString(), item.ToString());
                    }
                    var outputPath = String.Format(@"{0}\{1}.pdf", destinationPath, Guid.NewGuid().ToString());
                    this.outputPdfFile = outputPath;
                    using (FileStream stream = new FileStream(outputPath, FileMode.CreateNew, FileAccess.ReadWrite))
                    using (PDFDocument document = PDFDocument.ParseDocument(filename))
                    {
                        foreach (var t in dictionary)
                        {
                            document.Items[t.Key] = isValidEmail(t.Value, t.Value);
                            if (ValidateEmail(t.Value, t.Value))
                            {
                                emailCollect.Add(t.Value);
                                emailAddresses.Add(t.Value);
                            }
                        }
                        document.ProcessDocument(stream, true);
                        document.Info.Author = "Aston University";
                        document.Info.CreationDate = DateTime.Now;
                        stream.Flush();
                        stream.Close();
                        var counter = 0;
                        foreach (var emailList in emailCollect)
                        {
                            Outlook.Recipients mailrecipients = null;
                            Outlook.MailItem mail = null;
                            Outlook.Recipient rec = null;
                            Outlook.Application app = new Outlook.Application();
                            mail = app.CreateItem(Microsoft.Office.Interop.Outlook.OlItemType.olMailItem) as Outlook.MailItem;
                            mail.Subject = Subject;
                            mail.Body = Body;
                            Outlook.Attachment attach = mail.Attachments.Add(outputPath);
                            mailrecipients = mail.Recipients;
                            rec = mailrecipients.Add(emailList);
                            rec.Resolve();
                            mail.Send();
                            getContents(Path.GetFileName(outputPath));
                            ShowEmail.Add(emailList);
                            Console.WriteLine(counter);
                        }
                    }
                    Console.WriteLine("All Messages sent");
                }
                reader.Close();
                DeleteTable(connect, command, table1);
            }
            return ShowEmail;
        }
        public void DeleteTable(MySql.Data.MySqlClient.MySqlConnection connect, MySql.Data.MySqlClient.MySqlCommand command, DataTable table)
        {
            var connectionString = String.Format("Server=localhost;Database=test;username=root;password=;Port=3306");
            using (connect = new MySql.Data.MySqlClient.MySqlConnection(connectionString))
            {
                connect.Open();
                command = connect.CreateCommand();
                command.CommandText = String.Format("DROP TABLE {0}", table.TableName);
                command.ExecuteNonQuery();
            }
        }

        public bool ValidateEmail(String email, String fieldValue)
        {
            if (Regex.IsMatch(email, @"^(?("")(""[^""]+?""@)|(([0-9a-z]((\.(?!\.))|[-!#\$%&'\*\+/=\?\^`\{\}\|~\w])*)(?<=[0-9a-z])@)(?(\[)(\[(\d{1,3}\.){3}\d{1,3}\])|(([0-9a-z][-\w]*[0-9a-z]*\.)+[a-z0-9]{2,17}))$)", RegexOptions.IgnoreCase))
            {
                return true;
            }
            else
            {
                return false;
            }

        }
        public String isValidEmail(String email, String fieldValue)
        {
            if (Regex.IsMatch(email, @"^(?("")(""[^""]+?""@)|(([0-9a-z]((\.(?!\.))|
[-!#\$%&'\*\+/=\?\^`\{\}\|~\w])*)(?<=[0-9a-z])@)(?(\[)(\[(\d{1,3}\.){3}\d{1,3}\])|(([0-9a-z][-\w]
*[0-9a-z]*\.)+[a-z0-9]{2,17}))$)", RegexOptions.IgnoreCase))
            {
                return email;
            }
            else
            {
                return fieldValue;
            }
        }
        public DataTable gettable()
        {
            return table;
        }
        public DataTable ReadExcelFields(String inputFile)
        {
            using (var workbook = new XLWorkbook(inputFile))
            {
                var workSheet = workbook.Worksheets.First();
                var range = workSheet.FirstRow();
                foreach (var row in range.CellsUsed())
                {
                    DataColumn column = new DataColumn();
                    column.DataType = System.Type.GetType("System.String");
                    column.ColumnName = row.Value.ToString();
                    table.Columns.Add(column);
                }
                ReadExcelSpreadsheet(inputFile, table);
                return table;
            }
        }
        public void ReadExcelSpreadsheet(String inputFile, DataTable table)
        {
            string exists = null;
            var connectingString = String.Format("Server=localhost;Database=test;username=root;password=;Port=3306");
            using (var workbook = new XLWorkbook(inputFile))
            using (var connect = new MySql.Data.MySqlClient.MySqlConnection(connectingString))
            {
                var workSheet = workbook.Worksheets.First();
                var range = workSheet.RangeUsed();
                var colCount = range.ColumnCount();
                foreach (var row in range.RowsUsed())
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
                        var collection = new Dictionary<String, MySql.Data.MySqlClient.MySqlDbType>();
                        foreach (var col in table.Columns)
                        {
                            collection.Add(col.ToString(), MySql.Data.MySqlClient.MySqlDbType.VarChar);
                        }
                        StringBuilder sb = new StringBuilder();
                        sb.Append(String.Format("CREATE TABLE {0} (", table.TableName));
                        foreach (var loop in collection)
                        {
                            sb.AppendFormat("{0} {1}({2}),", loop.Key, loop.Value, 500);
                        }
                        command.CommandText = sb.ToString().Remove(sb.ToString().LastIndexOf(",")) + ")";
                        command.Prepare();
                        command.ExecuteNonQuery();
                        InsertExcelData(table);
                    }
                }
            }
        }
        public DataTable InsertExcelData(DataTable table)
        {
            var connectionString = String.Format("Server=localhost;Database=test;username=root;password=;Port=3306");
            using (var connect = new MySql.Data.MySqlClient.MySqlConnection(connectionString))
            {
                connect.Open();
                var command = connect.CreateCommand();
                foreach (DataRow row in table.Rows)
                {
                    StringBuilder builder = new StringBuilder();
                    builder.Append(string.Format("INSERT INTO {0} VALUES (", table.TableName));
                    foreach (DataColumn col in table.Columns)
                    {
                        builder.AppendFormat("'{0}',", row[col].ToString());
                    }
                    command.Prepare();
                    command.CommandText = builder.ToString().Remove(builder.ToString().LastIndexOf(",")) + ")";
                    command.ExecuteNonQuery();
                }
            }
            return table;
        }
        public DataTable getContents(String outputPath)
        {
            var table = new DataTable();
            table.Columns.Add("FileName");
            collect.Add(outputPath);
            for (int i = 0; i < collect.Count; i++)
            {
                var row = table.NewRow();
                row["FileName"] = collect[i];
                table.Rows.Add(row);
            }
            return table;
        }
        public List<String> getFiles()
        {
            return collect;
        }
        public String ProcessDirectoryInfo(String Filename)
        {
            var directoryInfo = new DirectoryInfo(Filename);
            String path = directoryInfo.Parent.FullName;
            return path;
        }
    }
}
