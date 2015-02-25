using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using Scryber.Components.Layout;
using Scryber.Components;
using System.Diagnostics;
using System.Xml.XPath;
using Scryber.Data;
using System.Xml;


namespace PDFGenerator
{
    class ConvertToPdf
    {
        String filename;
        String DestPath;
        //class may have to be used for xpath naviagation as well as generating the pdf...
        public ConvertToPdf(String filename, String Destinationpath)
        {
            this.filename = filename;
            this.DestPath = Destinationpath;
        }
        public PDFDocument CreatePdfFromPDFX()
        {
            //try
            //{
            var Stopwatch = new Stopwatch();
            var outputPath = String.Format(@"{0}\{1}.pdf", DestPath, Guid.NewGuid().ToString());
            FileStream writer = new FileStream(outputPath, FileMode.CreateNew, FileAccess.ReadWrite);
            Stopwatch.Start();
            using (PDFDocument document = PDFDocument.ParseDocument(filename))
            {
                document.ProcessDocument(writer);
                document.Info.Author = System.Environment.UserName;
                document.Info.CreationDate = DateTime.Now;

                writer.Flush();
                writer.Close();
                Stopwatch.Stop();
                Console.WriteLine(String.Format("Files took {0} to create", Stopwatch.Elapsed));
                return document;
            }
        }
        public static String outputFilePath(String destpath)
        {
            var path = String.Format(@"{0}\{1}.pdf", destpath);
            return path;
        }
        public static String getFilename(String filename)
        {
            return filename;
        }
        public static String DestinationPath(String destinationPath)
        {
            return destinationPath;
        }
        //Maybe have a remove file for users once uploaded if they make a mistake but need to finish batch production phase.
    }
}
