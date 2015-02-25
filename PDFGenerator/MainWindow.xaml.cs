using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Diagnostics;
using System.Diagnostics.Tracing;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Collections.ObjectModel;
using System.Windows.Documents;
using System.ComponentModel;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Windows.Forms.Layout;
using Scryber.Data;
using Scryber.Components;
using Scryber.Text;
using Scryber.Generation;
using Microsoft.Win32;
using System.IO;
using System.Data;

namespace PDFGenerator
{
    /// <summary> 
    /// </summary>
    public partial class MainWindow : Window
    {
        OpenFileDialog OpenDialog = new OpenFileDialog();
        System.Windows.Forms.FolderBrowserDialog FolderBrowser = new System.Windows.Forms.FolderBrowserDialog();

        System.Windows.Forms.NotifyIcon notify = new System.Windows.Forms.NotifyIcon();
        //dialogs for multiple Processing of pdf documents
        OpenFileDialog fileDialog = new OpenFileDialog();
        System.Windows.Forms.FolderBrowserDialog folderBrowserM = new System.Windows.Forms.FolderBrowserDialog();

        OpenFileDialog importFile = new OpenFileDialog();

        public MainWindow()
        {
            InitializeComponent();

        }


        private void Notify_MouseDoubleClick(object sender, System.Windows.Forms.MouseEventArgs e)
        {
            this.WindowState = System.Windows.WindowState.Normal;
        }

        private void Window_StateChanged(object sender, EventArgs e)
        {
            if (this.WindowState == System.Windows.WindowState.Minimized)
            {
                this.ShowInTaskbar = false;
                notify.BalloonTipTitle = "Minimize Successful";
                notify.BalloonTipText = "Minimized the app";
                notify.ShowBalloonTip(400);
                notify.Visible = true;
            }
            else if (this.WindowState == WindowState.Normal)
            {
                notify.Visible = false;
                this.ShowInTaskbar = true;
            }
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
        private void txtPath_TextChanged(object sender, TextChangedEventArgs e)
        {

        }
        private void txtFilePath_TextChanged(object sender, TextChangedEventArgs e)
        {
            //make it editable for users to enter path if known and press enter;;;;;;;????
        }
        private String BrowseXmlFile()//also used for reading xml files.
        {
            OpenDialog.Filter = "Pdfx Files(.pdfx)|*.pdfx";//file must be in pdfx in order to to fully parsed and content preserved.
            //maybe uplooad more than one template per file in the form of an archive.//maybe extract it to a temporary directory grab the file accordingly.
            OpenDialog.FilterIndex = 1;
            OpenDialog.Multiselect = false;
            Nullable<bool> showOut = OpenDialog.ShowDialog();
            if (showOut == true) OpenDialog.OpenFile();
            if (OpenDialog.FileName.Equals(""))
            {
                MessageBox.Show("You must provide a valid path", "Error", MessageBoxButton.OK, MessageBoxImage.Error, MessageBoxResult.OK, MessageBoxOptions.DefaultDesktopOnly);
                return "";
            }
            System.IO.FileInfo info = new FileInfo(OpenDialog.FileName);
            var getPath = info.FullName;
            if (btnUploadXml != null) { btnXmlBrowse.Text = getPath; }
            //if (btnGenerate != null) { txtFileUploadPath.Text = getPath; }
            return OpenDialog.FileName.ToString();
        }
        //Batch Processing
        private String BrowseXmlFileBatch()
        {
            fileDialog.Filter = "Pdfx Files(.pdfx)|*.pdfx";
            fileDialog.FilterIndex = 1;
            fileDialog.Multiselect = false;
            Nullable<bool> showOut = fileDialog.ShowDialog();
            if (showOut == true) fileDialog.OpenFile();
            if (fileDialog.FileName.Equals(""))
            {
                MessageBox.Show("You must provide a valid path", "Error", MessageBoxButton.OK, MessageBoxImage.Error, MessageBoxResult.OK, MessageBoxOptions.DefaultDesktopOnly);
                return "";
            }
            System.IO.FileInfo info = new FileInfo(fileDialog.FileName);
            var getPath = info.FullName;
            if (btnUploadTemplate != null) { txtFileUploadPath.Text = getPath.ToString(); }
            //if(btnGen != null){ }
            return fileDialog.FileName;
        }
        private String GetFileName(OpenFileDialog dialog)
        {
            return dialog.SafeFileName;
        }
        private void CreatePdf(String Destination, String SafeFilename)
        {
            SafeFilename = OpenDialog.FileName;
            var CreatePdf = new ConvertToPdf(SafeFilename, Destination);
            CreatePdf.CreatePdfFromPDFX();
        }

        private String SpecifyDestinationPath()
        {
            //filename is the pdf equivalent.
            FolderBrowser.ShowNewFolderButton = true;
            System.Windows.Forms.DialogResult result = FolderBrowser.ShowDialog();
            if (result.ToString() == "OK" && btnGenerate != null) { txtDestination.Text = FolderBrowser.SelectedPath; }
            return FolderBrowser.SelectedPath;
        }
        //Batch Processing
        private String SpecifyDestPathForBatch()
        {
            folderBrowserM.ShowNewFolderButton = true;
            System.Windows.Forms.DialogResult result = folderBrowserM.ShowDialog();
            if (result.ToString() == "OK" && btnGen != null) { txtDestinationForPdfBatch.Text = folderBrowserM.SelectedPath; }
            return folderBrowserM.SelectedPath;
        }
        private void Button_Click_2(object sender, RoutedEventArgs e)
        {
            if (btnUploadXml != null)
            {
                BrowseXmlFile();
                txtDestination.IsEnabled = true;
                btnDestPath.IsEnabled = true;
            }
        }
        private void btnDestPath_Click(object sender, RoutedEventArgs e)
        {
            if (btnDestPath != null) { SpecifyDestinationPath(); btnGenerate.IsEnabled = true; }
        }
        //Alter table to add in a SUN number and map it to the filename
        //private String CreateFileCredentials() {
        //generate a filename based on a SUN id......
        //}
        private void btnGenerate_Click(object sender, RoutedEventArgs e)
        {
            if (btnGenerate != null)
            {
                for (int i = 0; i < 100; i++)
                {
                    //create a progress bar for each allocated job..
                    progressBar.Value++;//has created empty pdf files but corrupt ones..
                }
                CreatePdf(FolderBrowser.SelectedPath, String.Format("")); System.Threading.Thread.Sleep(100);
                MessageBox.Show("Your File has been Created", "", MessageBoxButton.OK);
                //run process to show up windows explorer and to the relevant files....
            }
        }
        private void btnUploadTemplate_Click(object sender, RoutedEventArgs e)
        {
            if (btnUploadTemplate != null) { BrowseXmlFileBatch(); btnDestinationPath.IsEnabled = true; txtDestinationForPdfBatch.IsEnabled = true; }
        }
        private void btnDestinationPath_Click(object sender, RoutedEventArgs e)
        {
            if (btnDestinationPath != null)
                SpecifyDestPathForBatch();
            btnGen.IsEnabled = true;
        }
        private void btnGenerateBatch_Click(object sender, RoutedEventArgs e)
        {
        }

        private void processGenButton(String Browsefilename, String BrowsedDestinationPath)
        {
            try
            {
                Browsefilename = fileDialog.FileName;
                BrowsedDestinationPath = folderBrowserM.SelectedPath;
                var repository = new Repository(Browsefilename, BrowsedDestinationPath);
                repository.getAll();
                DataGrid1.DataContext = repository.getContents(repository.outputPdfFile);
            }
            catch (Exception exception)
            {
                Console.WriteLine(String.Format("{0}", exception.InnerException));
            }
        }
        private void btnGen_Click(object sender, RoutedEventArgs e)
        {
            if (btnGen != null)
                this.ListAllJobs.IsEnabled = true;
            processGenButton(fileDialog.FileName, folderBrowserM.SelectedPath);
            //var iconHandle =
            //notify.Icon = new System.Drawing.Icon(String.Format("{0}"));
            //notify.MouseDoubleClick += new System.Windows.Forms.MouseEventHandler(Notify_MouseDoubleClick);
        }

        private void DataGrid1_Loaded(object sender, RoutedEventArgs e)
        {

        }

        private void ListAllJobs_Click(object sender, RoutedEventArgs e)
        {
            var repo = new Repository(fileDialog.FileName, folderBrowserM.SelectedPath);
            //var check = repo.Progress;
            //kick off progress bars and display notifyIcon but first message box
        }

        private String importBrowseDataFile()
        {
            var repo = new Repository(fileDialog.FileName, folderBrowserM.SelectedPath);
            importFile.Filter = "Files(*.xls,*.xlsx, *.csv,*.xml) | *.xls;*.xlsx;*.csv,*.xml";
            importFile.FilterIndex = 1;
            importFile.Multiselect = false;
            Nullable<bool> showOut = importFile.ShowDialog();
            if (showOut == true)
                importFile.OpenFile();
            if (importFile.FileName.Equals(""))
            {
                MessageBox.Show("Please enter a valid path", "", MessageBoxButton.OK, MessageBoxImage.Error, MessageBoxResult.OK, MessageBoxOptions.ServiceNotification);
                return "";
            }
            System.IO.FileInfo info = new FileInfo(importFile.FileName);
            var getPath = info.FullName;
            if (btnImport != null)
            {
                txtImport.Text = getPath.ToString();
            }
            repo.ReadExcel(importFile.FileName);
            return importFile.FileName.ToString();
        }

        private void btnImport_Click(object sender, RoutedEventArgs e)
        {
            if (btnImport != null)
                importBrowseDataFile();
            //call a method to read all file extensions, separate methods for each file extension.

        }



    }
}
