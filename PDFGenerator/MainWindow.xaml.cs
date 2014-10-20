using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using iTextSharp.text.pdf;
using iTextSharp.text;
using iTextSharp.text.pdf.parser;
using System.Windows.Forms.Layout;
using Scryber.Data;
using Scryber.Components;
using Microsoft.Win32;
using System.IO;

namespace PDFGenerator
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// list of classes: PDFGenerate.cs
    /// FileBrowserExplorer - browse file and i grab file read it in and spit it out to a pdf simple as that using linq and all sorts. 
    /// file extensions- html,txt,doc,xls,rtf,ppt....
    /// template tab?????
    /// itextSharp library files are on local computer, programmatically create templates
    /// 
    /// Design Document
    /// 
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();

        }


        private void UploadTab_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {

        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {

        }
        private void BrowseSelectedFiles()
        {


        }

        private void BtnBrowse_Click(object sender, RoutedEventArgs e)//open dialog and obtain and parse file path and spit it out into textbox
        {
            var OpenDialog = new OpenFileDialog();
            OpenDialog.Filter = "Pdf Files(.pdf)|*.pdf";
            OpenDialog.FilterIndex = 1;
            OpenDialog.Multiselect = false;
            Nullable<bool> show = OpenDialog.ShowDialog();
            if (show == true) OpenDialog.OpenFile();

            //try
            //{
            //if (OpenDialog.FileName == null)
            //throw new FileNotFoundException(String.Format("{0}",OpenDialog.FileName));
            System.IO.FileInfo fileinfo = new FileInfo(OpenDialog.FileName);
            var GetselectedPath = fileinfo.FullName;
            txtPath.Text = GetselectedPath;
            //open pdf template and read it in and get selectedfile// cannot parse text with itext.
            System.IO.Stream stream = fileinfo.OpenRead();
            using (System.IO.StreamReader reader = new System.IO.StreamReader(stream))
            {
                reader.ReadToEnd();
                webBrowser.Navigate(GetselectedPath);
                StringBuilder builder = new StringBuilder();//use direct contentByte
                PdfReader read = new PdfReader(GetselectedPath);
                Document doc = new Document(PageSize.A4, 25, 25, 25, 25);
                FileStream writeStream = new FileStream("temp.pdf", FileMode.Create, FileAccess.ReadWrite);
                PdfWriter writer = PdfWriter.GetInstance(doc, writeStream);
                for (int i = 1; i <= read.NumberOfPages; i++)
                {
                    var currentText = PdfTextExtractor.GetTextFromPage(read, i, new LocationTextExtractionStrategy());
                    currentText = Encoding.UTF8.GetString(ASCIIEncoding.Convert(Encoding.Default, Encoding.UTF8, Encoding.Default.GetBytes(currentText)));
                    builder.Append(currentText);
                    doc.Open();
                    doc.Add(new iTextSharp.text.Paragraph(currentText));

                }
                writeStream.Flush();
                doc.Close();
                writer.Close();
                //directcontentByte..
                //read in and parse entire pdf document
                //area on the form which displays preview of template.
                //apply constraints for reading a pdf template cannot be nomore than one page otherwise program will error.
            }
            //}
            //catch (Exception exception) {
            //if error is caught then log it in db maybe or log files or even in a bog standard windows error message.
            //}   
            //iTextSharp - pdfStamper
        }
        private void ReadXmlTemplate(iTextSharp.text.Document document, PdfReader reader, string filename)
        {
            //use scryber 
            //using (FileStream stream = new FileStream()) { }
        } //reading pdf templates option for template or no template use Apitron.cs library for preview of template

        //find new way of generating pdf templates
        private void txtPath_TextChanged(object sender, TextChangedEventArgs e)
        {

        }

        private void txtFilePath_TextChanged(object sender, TextChangedEventArgs e)
        {
            //make it editable for users to enter path if known and press enter;;;;;;;????
        }

        private void BrowseXmlFile()//also used for reading xml files.
        {
            var OpenDialog = new OpenFileDialog();
            OpenDialog.Filter = "Xml Files(.xml)|*.xml";
            OpenDialog.Filter = "Pdfx Files(.pdfx)|*.pdfx";
            OpenDialog.FilterIndex = 1;
            OpenDialog.Multiselect = false;
            Nullable<bool> showOut = OpenDialog.ShowDialog();
            if (showOut == true) OpenDialog.OpenFile();
            System.IO.FileInfo info = new FileInfo(OpenDialog.FileName);
            var getPath = info.FullName;
            btnXmlBrowse.Text = getPath;

            //readStream
            FileStream writer = new FileStream("temp.pdf", FileMode.Create, FileAccess.ReadWrite);//FileNames
            using (Scryber.Components.PDFDocument document = Scryber.Components.PDFDocument.ParseDocument(getPath))
            {
                document.ProcessDocument(writer);

            }


        }

        //private String generateRandomFileName() { 

        //}

        private void Button_Click_2(object sender, RoutedEventArgs e)
        {
            if (btnUploadXml != null)
            {
                BrowseXmlFile();
            }
        }

    }
}
