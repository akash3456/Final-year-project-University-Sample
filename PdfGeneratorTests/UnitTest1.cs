using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace PdfGeneratorTests
{
    [TestClass]
    public class UnitTest1
    {
        //one class for testing all aspects of pdf generator..
        [TestMethod]
        public void TestMethod1()
        {

        }
        [TestMethod]
        public void ReadPdfTemplates() { }
        [TestMethod]
        public void CreateExcelSpreadsheets() { } //from what is the question
        [TestMethod]
        public void SendEmails() { }              //parsed from textbox.
        [TestMethod]
        public void CreatePdfFromTemplate() { } //make sure text and acro fields are placed in the correct places.

        [TestMethod]
        public void GeneratePasswordProtectedDocuments() { } //Generate passwordProtectedDocs????

        [TestMethod]//feedback for User interface..
        public void UsersSelectPdfOnly() { }

        [TestMethod]
        public void CanViewPdfTemplate() { }

        //[TestMethod]
        //public void 
    }
}
