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
        private String filename;
        private String DestPath;
        public ConvertToPdf(String filename, String Destinationpath)
        {
            this.filename = filename;
            this.DestPath = Destinationpath;
        }

        public void CreatePdfFromPDFX()
        {
            try
            {
                //progress bar for streamIO.
                var Stopwatch = new Stopwatch();
                //var outputPath = System.IO.Path.Combine(DestPath, filename);
                var outputPath = String.Format(@"{0}\{1}.pdf", DestPath, Guid.NewGuid().ToString());
                FileStream writer = new FileStream(outputPath, FileMode.CreateNew, FileAccess.ReadWrite);
                Stopwatch.Start();
                using (PDFDocument document = PDFDocument.ParseDocument(filename))//filename needs to be pdfx..
                {
                    document.ProcessDocument(writer);
                }
                writer.Flush();
                writer.Close();
                Stopwatch.Stop();
                Console.WriteLine(String.Format("{0}", Stopwatch.Elapsed));
            }
            catch (Exception exception) { 
            //log the error somehow...
            }
            //need to find to attach a progress bar to each process for generating a pdf file for example. 
        }
    }
}
