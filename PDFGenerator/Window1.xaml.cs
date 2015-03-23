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
using System.Windows.Shapes;
using Microsoft.Win32;
using Scryber.Text;
using ICSharpCode.AvalonEdit.Editing;
using System.Xml;

namespace PDFGenerator
{
    /// <summary>
    /// Interaction logic for Window1.xaml
    /// </summary>
    public partial class Window1 : Window
    {
        public Window1()
        {
            InitializeComponent();
            XmlDocument doc = new XmlDocument();
            textEditor.Document.Text = String.Format("<?xml version='1.0' encoding='utf-8' ?>" + "\n"
                + "<pdf:Document xmlns:pdf='Scryber.Components, Scryber.Components, Version=0.8.0.0, Culture=neutral, PublicKeyToken=872cbeb81db952fe'" + "\n" + "xmlns:styles='Scryber.Styles, Scryber.Styles, Version=0.8.0.0, Culture=neutral, PublicKeyToken=872cbeb81db952fe'>"
                + "\n"
                + "<Info><Title></Title></Info>" + "\n"
                + "<Styles></Styles>" + "\n"
                + "<Pages> </Pages>" + "\n"
                + "\n"
                + "</pdf:Document>");
            textEditor.ShowLineNumbers = true;
        }
        private void MenuItem_Click(object sender, RoutedEventArgs e)
        {
        }
        private void MenuItem_Click_1(object sender, RoutedEventArgs e)
        {
            var dlg = new SaveFileDialog();
            dlg.Filter = "pdfx (*.pdfx)|*.pdfx";
            dlg.FilterIndex = 1;
            dlg.RestoreDirectory = true;
            dlg.DefaultExt = "pdfx";
            bool? result = dlg.ShowDialog();
            var documentContent = textEditor.Document.Text;
            if (result == true)
            {
                System.IO.File.WriteAllText(dlg.FileName, documentContent);
            }
        }

        private void DefaultTemplate()
        {

        }


        private void MenuItem_Click_2(object sender, RoutedEventArgs e)
        {

        }

        private void MenuItem_Click_3(object sender, RoutedEventArgs e)
        {

        }

        private void MenuItem_Click_4(object sender, RoutedEventArgs e)
        {

        }

        private void MenuItem_Click_5(object sender, RoutedEventArgs e)
        {
            var opendlg = new OpenFileDialog();
            opendlg.Filter = "Pdfx Files (.pdfx)|*.pdfx";
            opendlg.FilterIndex = 1;
            opendlg.Multiselect = false;
            Nullable<bool> showOut = opendlg.ShowDialog();
            if (showOut == true)
                opendlg.OpenFile();
            if (opendlg.FileName.Equals(""))
            {
                MessageBox.Show("You must provide select a valid .pdfx file", "", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }
            System.IO.StreamReader reader = new System.IO.StreamReader(opendlg.FileName);
            String mystring = reader.ReadToEnd();
            textEditor.Document.Text = mystring;
        }
    }
}
