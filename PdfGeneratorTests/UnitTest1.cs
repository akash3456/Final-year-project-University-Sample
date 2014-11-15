using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace PdfGeneratorTests
{
    [TestClass]
    public class UnitTest1
    {
        [TestMethod]
        public void ReadPdfTemplates() { }
        [TestMethod]
        public void CreateExcelSpreadsheets() { }
        [TestMethod]
        public void SendEmails() { }
        [TestMethod]
        public void CreatePdfFromTemplate() { } //make sure text and acro fields are placed in the correct places.

        [TestMethod]//feedback for User interface..
        public void UsersSelectPdfOnly() { }

        [TestMethod]
        public void CanViewPdfTemplate() { }
    }
}
