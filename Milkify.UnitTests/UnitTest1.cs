using System.Collections;
using System.Collections.Generic;
using NUnit.Framework;
using Milkify.PdfGenerator;
using Milkify.Services;

namespace Tests
{
    public class Tests
    {
        [SetUp]
        public void Setup()
        {
        }

        [Test]
        public void Test1()
        {
            PdfGenerator a = new PdfGenerator();
            string header = "Invoice For Order #311228";
            IList<string> tableHeader = new List<string>();
            tableHeader.Add("Product Name");
            tableHeader.Add("Price");
            tableHeader.Add("Quantity");
            tableHeader.Add("Total Price");
            IList<string> tableData = new List<string>();
            tableData.Add("AMD Phenom X4 9850 Quad-Core");
            tableData.Add("$16.99");
            tableData.Add("2");
            tableData.Add("$33.98");
            tableData.Add("Intel Pentium 4 PC computer P4 3.20GHZ");
            tableData.Add("$8.00");
            tableData.Add("30");
            tableData.Add("$240.00");
            //a.GenerateTablePdf(header, tableHeader, tableData);
            Assert.Pass();
        }
    }
}