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
        OpenFileDialog importFile = new OpenFileDialog();
        Microsoft.Windows.Shell.JumpList jumpList = new Microsoft.Windows.Shell.JumpList();
        public MainWindow()
        {
            InitializeComponent();
            this.notify.Visible = true;
            this.notify.Icon = PDFGenerator.Resources.Resource1.NotifyIcon;
            this.notify.ContextMenu = new System.Windows.Forms.ContextMenu();
            this.notify.ContextMenu.MenuItems.Add("Test Message for NotifyIcon");
            Microsoft.Windows.Shell.JumpList.SetJumpList(Application.Current, jumpList);
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
            Microsoft.Windows.Shell.JumpPath jumppath = new Microsoft.Windows.Shell.JumpPath();
            jumppath.Path = String.Format("{0}", OpenDialog.FileName);
            jumpList.JumpItems.Add(jumppath);
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
            if (btnUploadTemplate != null) { txtFileUploadPath.Text = getPath.ToString(); progressBar2.BeginAnimation(ProgressBar.ValueProperty, null); }
            return fileDialog.FileName;
        }
        private String GetFileName(OpenFileDialog dialog)
        {
            return dialog.SafeFileName;
        }

        private String CreatePdf(String Destination, String SafeFilename)
        {
            SafeFilename = OpenDialog.FileName;
            System.IO.FileInfo info = new FileInfo(OpenDialog.FileName);
            var CreatePdf = new ConvertToPdf(SafeFilename, Destination);
            Duration duraiton = new Duration(TimeSpan.FromSeconds(0.05));
            progressBar.IsIndeterminate = false;
            progressBar.Maximum = info.Length;
            for (int i = 0; i < progressBar.Maximum; i++)
            {
                var animate = new DoubleAnimation(progressBar.Value, duraiton);
                progressBar.BeginAnimation(ProgressBar.ValueProperty,animate);
                progressBar.Value += i;
            }
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
                this.notify.ShowBalloonTip(10000, String.Format("PDFDocument has been generated"), "Document has been generated", System.Windows.Forms.ToolTipIcon.Info);
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
                var repository = new Repository(Browsefilename, BrowsedDestinationPath, txtSubject.Text, txtContent.Text);
                repository.getAll();
                DataGrid1.DataContext = repository.getContents(repository.outputPdfFile);
                lblProcess.Content = "";
                lblCompleted.Content = String.Format("{0} Files to be Generated", repository.getFiles().Count);
                this.notify.ShowBalloonTip(10000, "Your Files have been Generated", String.Format("{0} Number of files produced", repository.getFiles().Count), System.Windows.Forms.ToolTipIcon.Info);
                //need a loop to obtain list of addresses documents have been sent to and print out the loop somewhere in the balloon tip.
            }
            catch (Exception exception)
            {
                MessageBox.Show("" + exception.InnerException.ToString(), "", MessageBoxButton.OK, MessageBoxImage.Error, MessageBoxResult.OK);
                this.notify.ShowBalloonTip(10000, "Application has Stopped, Please Reprocess", String.Format("{0}", exception.InnerException), System.Windows.Forms.ToolTipIcon.Error);
            }
        }
        private void btnGen_Click(object sender, RoutedEventArgs e)
        {
            if (btnGen != null)
            {
                System.IO.FileInfo info = new FileInfo(fileDialog.FileName);
                Duration duration = new Duration(TimeSpan.FromSeconds(fileDialog.FileName.Length));
                GenBar.IsIndeterminate = false;
                GenBar.Maximum = info.Length;
                for (int i = 0; i < GenBar.Maximum; i++)
                {
                    var animate = new DoubleAnimation(GenBar.Value, duration);
                    GenBar.BeginAnimation(ProgressBar.ValueProperty, animate);
                    GenBar.Value += i;
                }
                this.ListAllJobs.IsEnabled = true;
                processGenButton(fileDialog.FileName, folderBrowserM.SelectedPath);
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
            var repo = new Repository(fileDialog.FileName, folderBrowserM.SelectedPath, txtSubject.Text, txtContent.Text);
            importFile.Filter = "Files(*.xlsx, *.csv,*.xml) | *.xlsx;*.csv,*.xml";
            importFile.FilterIndex = 1;
            importFile.Multiselect = false;
            Nullable<bool> showOut = importFile.ShowDialog();
            Duration duration = new Duration(TimeSpan.FromSeconds(importFile
                .FileName.Length));
            progressBar2.IsIndeterminate = false;
            if (showOut == true)
            {
                lblProcess.Content = "Processing.....";
                importFile.OpenFile();
                System.IO.FileInfo info = new FileInfo(importFile.FileName);//inject a label into the UI.
                var getPath = info.FullName;
                txtImport.Text = getPath.ToString();
                progressBar2.Maximum = info.Length;
                this.TaskbarItemInfo.ProgressState = System.Windows.Shell.TaskbarItemProgressState.Normal;
                for (int i = 0; i < progressBar2.Maximum; i++)
                {
                    var animate = new DoubleAnimation(progressBar2.Value, duration);
                    progressBar2.BeginAnimation(ProgressBar.ValueProperty, animate);
                    progressBar2.Value += i;
                    this.TaskbarItemInfo.ProgressValue = i;
                }
                repo.ReadExcelFields(importFile.FileName);
            }
            if (importFile.FileName.Equals(""))
            {
                MessageBox.Show("Please enter a valid path", "", MessageBoxButton.OK, MessageBoxImage.Error, MessageBoxResult.OK, MessageBoxOptions.ServiceNotification);
                return "";
            }
            System.IO.FileInfo information = new FileInfo(importFile.FileName);
            ToolTip tip = new System.Windows.Controls.ToolTip();
            tip.Content = String.Format("Shows progress of Importing {0}", System.IO.Path.GetFileName(information.FullName));
            progressBar2.ToolTip = tip;
            this.notify.ShowBalloonTip(10000, String.Format("{0} Has Been Imported Successfully", importFile.FileName), String.Format("{0}", information.FullName), System.Windows.Forms.ToolTipIcon.Info);
            return importFile.FileName.ToString();
        }

        private void btnImport_Click(object sender, RoutedEventArgs e)
        {
            var repo = new Repository(fileDialog.FileName, folderBrowserM.SelectedPath, txtSubject.Text, txtContent.Text);
            if (btnImport != null)
            {
                importBrowseDataFile();
            }
        }

        private void Button_Click_1(object sender, RoutedEventArgs e)
        {
            if (btnEditTemplate != null)
            {
                Window1 window = new Window1();
                window.Show();
            }
        }
        private void btnSubmit_Click(object sender, RoutedEventArgs e)
        {
            if (btnSubmit != null || txtSubject.Text != null || txtContent != null)
            {
                String Subject = txtSubject.Text;
                String EmailBody = txtContent.Text;
                var sendEmail = new SendEmail(Subject, EmailBody);
                MessageBox.Show("Information Will Be Contained in a Dispatched Email", "", MessageBoxButton.OK, MessageBoxImage.Information, MessageBoxResult.OK);
            }
            else
            {
                MessageBox.Show("Please Enter Information that is required", "", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void txtContent_KeyDown(object sender, KeyEventArgs e)
        {

        }
    }
}
