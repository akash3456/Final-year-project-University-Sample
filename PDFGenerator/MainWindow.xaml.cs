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
using System.Windows.Forms.Layout;
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
            //AddControlsToTab();
        }

        private void AddControlsToTab() {//for testing purposes. 
            Button Tbutton = new Button();
            Tbutton.Name = "Click";
            layout.Children.Add(Tbutton);

            TextBox box = new TextBox();
            box.Name = "";
            box.Text = "";
            layout.Children.Add(box);
            
            //this.layout
        }
        private void ReadTemplate() { } //reading pdf templates option for template or no template use Apitron.cs library for preview of template
        
        
        private void UploadTab_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {

        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {

        }
        private void BrowseSelectedFiles() { 
        

        }

        private void BtnBrowse_Click(object sender, RoutedEventArgs e)//open dialog and obtain and parse file path and spit it out into textbox
        {
            var OpenDialog = new OpenFileDialog();
            OpenDialog.Filter = "Pdf Files(.pdf)|*.pdf";
            OpenDialog.FilterIndex = 1;
            OpenDialog.Multiselect = false;

            Nullable<bool> show = OpenDialog.ShowDialog();
            if (show == true) OpenDialog.OpenFile();

            System.IO.FileInfo fileinfo = new FileInfo(OpenDialog.FileName);
            var GetselectedPath = fileinfo.DirectoryName;
        }

        private String getFileInfo() {
            var OpenDialog = new OpenFileDialog();
            OpenDialog.Filter = "Pdf Files(.pdf)|*.pdf";
            OpenDialog.FilterIndex = 1;
            OpenDialog.Multiselect = false;

            Nullable<bool> show = OpenDialog.ShowDialog();
            if (show == true) OpenDialog.OpenFile();

            System.IO.FileInfo fileinfo = new FileInfo(OpenDialog.FileName);
            var GetselectedPath = fileinfo.DirectoryName;
            return GetselectedPath;
        }

        private void txtPath_TextChanged(object sender, TextChangedEventArgs e)
        {
            txtPath.AppendText(getFileInfo());
        }

    }
}
