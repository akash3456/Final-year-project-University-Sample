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



namespace PDFGenerator
{
    class Repository
    {
        String filename;
        String destinationPath;
        public String outputPdfFile;
        //Input File can be .xml/.csv/.xls - closedxml, could be any file with input data
        List<String> collect = new List<String>();
        List<Object> times = new List<Object>();
        Stopwatch watch = new Stopwatch();

        public Repository(String filename, String DestinationPath)
        {
            watch.Start();
            this.filename = filename;
            this.destinationPath = DestinationPath;
        }

        public SampleDto getAll()
        {
            SampleDto dto = new SampleDto();
            String connectionString = String.Format("Server=localhost;Database=test;username=root;password=;Port=3306");
            using (MySql.Data.MySqlClient.MySqlConnection connect = new MySql.Data.MySqlClient.MySqlConnection(connectionString))
            {
                connect.Open();
                var command = connect.CreateCommand();
                command.CommandText = String.Format("SELECT * FROM TEST.T_TEST_DATA");
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

                    var outputPath = String.Format(@"{0}\{1}.pdf", destinationPath, Guid.NewGuid().ToString());
                    this.outputPdfFile = outputPath;
                    
                    FileStream stream = new FileStream(outputPath, FileMode.CreateNew, FileAccess.ReadWrite);
                    using (PDFDocument document = PDFDocument.ParseDocument(filename))
                    {
                        document.Items["Firstname"] = dto.getFirstName;
                        document.Items["Secondname"] = dto.getSecondName;
                        document.Items["FirstYearGrade"] = dto.getFirstYearGrade;
                        document.Items["SecondYearGrade"] = dto.getSecondYearGrade;
                        document.Items["AstonUsername"] = dto.getUsername;
                        document.Items["EmailAddress"] = dto.getEmail;
                        document.Items["SUN"] = dto.getSUN;
                        document.ProcessDocument(stream, true);
                        document.Info.Author = System.Environment.UserName;
                        document.Info.CreationDate = DateTime.Now;
                        stream.Flush();
                        stream.Close();
                        this.Progress = watch;
                        getContents(Path.GetFileName(outputPath));
                    }
                }
                watch.Stop();
                reader.Close();
            }
            return dto;
        }

        public void ReadCsv(String inputFile) { 
        
        }

        //import into database table thats the reason why..
        public void ReadExcel(String inputFile) {//,xlsx
            var collection = new List<Object>();
            var workbook = new XLWorkbook(inputFile);
            var workSheet = workbook.Worksheets.First();

            var range = workSheet.RangeUsed();
            var colCount = range.ColumnCount();
            foreach (var row in range.RowsUsed())
            {
                object[] rowData = new object[colCount];
                Int32 i = 0;
                row.Cells().ForEach(c => rowData[i++] = c.Value);
                collection.Add(rowData);

                var connectionString = String.Format("Server=localhost;Database=test;username=root;password=;Port=3306");
                var connect = new MySql.Data.MySqlClient.MySqlConnection(connectionString);
                connect.Open();
                //communicate with the database..
            }
        }

        public void ReadXml(String inputFile) { 
        
        
        }



        public Stopwatch Progress { get; set; }
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
        public System.Diagnostics.Stopwatch getProgressCheck()
        {
            return watch;
        }
        public String ProcessDirectoryInfo(String Filename)
        {
            var directoryInfo = new DirectoryInfo(Filename);
            String path = directoryInfo.Parent.FullName;
            return path;
        }
    }
}
