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
using System.Windows.Media.Animation;
using System.Data;
using ICSharpCode.AvalonEdit;

namespace PDFGenerator
{
    /// <summary> 
    /// </summary>
    public partial class MainWindow : Window
    {
        OpenFileDialog OpenDialog = new OpenFileDialog();
        System.Windows.Forms.FolderBrowserDialog FolderBrowser = new System.Windows.Forms.FolderBrowserDialog();

        System.Windows.Forms.NotifyIcon notify = new System.Windows.Forms.NotifyIcon();
        OpenFileDialog fileDialog = new OpenFileDialog();
        System.Windows.Forms.FolderBrowserDialog folderBrowserM = new System.Windows.Forms.FolderBrowserDialog();
        Window1 window = new Window1();
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
        private String BrowseXmlFile()
        {
            OpenDialog.Filter = "Pdfx Files(.pdfx)|*.pdfx";
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
            return OpenDialog.FileName.ToString();
        }
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
            return fileDialog.FileName;
        }
        private String GetFileName(OpenFileDialog dialog)
        {
            return dialog.SafeFileName;
        }

        private String CreatePdf(String Destination, String SafeFilename)
        {
            SafeFilename = OpenDialog.FileName;
            var CreatePdf = new ConvertToPdf(SafeFilename, Destination);
            CreatePdf.CreatePdfFromPDFX();
            return CreatePdf.CreatePdfFromPDFX().ToString();
        }

        private String SpecifyDestinationPath()
        {
            FolderBrowser.ShowNewFolderButton = true;
            System.Windows.Forms.DialogResult result = FolderBrowser.ShowDialog();
            if (result.ToString() == "OK" && btnGenerate != null) { txtDestination.Text = FolderBrowser.SelectedPath; }
            return FolderBrowser.SelectedPath;
        }
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
        private void btnGenerate_Click(object sender, RoutedEventArgs e)
        {
            if (btnGenerate != null)
            {
                CreatePdf(FolderBrowser.SelectedPath, String.Format("")); System.Threading.Thread.Sleep(100);
                MessageBox.Show("Your File has been Created", "", MessageBoxButton.OK);
                var path = CreatePdf(FolderBrowser.SelectedPath, OpenDialog.FileName).ToString();
                this.webBrowser.Navigate(path);
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
                MessageBox.Show("" + exception.InnerException, "", MessageBoxButton.OK, MessageBoxImage.Error, MessageBoxResult.OK);
            }
        }
        private void btnGen_Click(object sender, RoutedEventArgs e)
        {
            if (btnGen != null)
            {
                this.ListAllJobs.IsEnabled = true;
                processGenButton(fileDialog.FileName, folderBrowserM.SelectedPath);
                MessageBox.Show("Your Documents have been produced", "", MessageBoxButton.OK, MessageBoxImage.Information, MessageBoxResult.OK, MessageBoxOptions.DefaultDesktopOnly);
            }
        }

        private void DataGrid1_Loaded(object sender, RoutedEventArgs e)
        {

        }

        private void ListAllJobs_Click(object sender, RoutedEventArgs e)
        {

        }

        private String importBrowseDataFile()
        {
            var repo = new Repository(fileDialog.FileName, folderBrowserM.SelectedPath);
            importFile.Filter = "Files(*.xlsx, *.csv,*.xml) | *.xlsx;*.csv,*.xml";
            importFile.FilterIndex = 1;
            importFile.Multiselect = false;
            Nullable<bool> showOut = importFile.ShowDialog();
            progressBar2.IsIndeterminate = true;
            if (showOut == true)
            {
                importFile.OpenFile();
            }
            if (importFile.FileName.Equals(""))
            {
                MessageBox.Show("Please enter a valid path", "", MessageBoxButton.OK, MessageBoxImage.Error, MessageBoxResult.OK, MessageBoxOptions.ServiceNotification);
                return "";
            }
            System.IO.FileInfo info = new FileInfo(importFile.FileName);
            var getPath = info.FullName;
            txtImport.Text = getPath.ToString();
            repo.ReadExcelFields(importFile.FileName);
            MessageBox.Show("Your File has been Imported and your documents are now ready to be produced.", "", MessageBoxButton.OK, MessageBoxImage.Information);
            return importFile.FileName.ToString();
        }

        private void btnImport_Click(object sender, RoutedEventArgs e)
        {
            var repo = new Repository(fileDialog.FileName, folderBrowserM.SelectedPath);
            if (btnImport != null)
            {
                importBrowseDataFile();
                progressBar2.IsIndeterminate = false;
            }
        }

        private void btnShowLocationDocuments_Click(object sender, RoutedEventArgs e)
        {

        }

        private void Button_Click_1(object sender, RoutedEventArgs e)
        {
            window.Show();
        }

        private void btnSubmit_Click(object sender, RoutedEventArgs e)
        {
            if (btnSubmit != null)
            {
                String Subject = txtSubject.Text;
                String EmailBody = txtContent.Text;
                var sendEmail = new SendEmail(Subject, EmailBody);
                MessageBox.Show("Information has been cofirmed and will be contained in a Distpacthed Email", "", MessageBoxButton.OK, MessageBoxImage.Information, MessageBoxResult.OK);
            }
        }
    }
}
