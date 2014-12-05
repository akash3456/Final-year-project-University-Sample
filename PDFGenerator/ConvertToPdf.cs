using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using Scryber.Components.Layout;
using Scryber.Components;
using System.Diagnostics;


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

        public void CreatePdfFromPDFX()
        {
            try
            {
                var Stopwatch = new Stopwatch();
                var outputPath = String.Format(@"{0}\{1}.pdf", DestPath, Guid.NewGuid().ToString());
                FileStream writer = new FileStream(outputPath, FileMode.CreateNew, FileAccess.ReadWrite);
                Stopwatch.Start();
                using (PDFDocument document = PDFDocument.ParseDocument(filename))
                {
                    document.ProcessDocument(writer);
                }
                writer.Flush();
                writer.Close();
                Stopwatch.Stop();
                Console.WriteLine(String.Format("Files took {0} to create", Stopwatch.Elapsed));
            }
            catch (Exception exception)
            {
                //log the error somehow...
            }
            //need to find to attach a progress bar to each process for generating a pdf file for example. 
        }
        public static String outputFilePath(String destpath)
        {
            var path = String.Format(@"{0}\{1}.pdf", destpath);
            return path;
        }

        public void CreatePdfBatch()//use xpath navigator in this method
        {

            //var getRepo = new Repository();
            //getRepo
        }
        //i want to integrate an editor in this program for users to easily edit pdfx files and with intellisense with greg's editor 2.0
    }
}
