using System;
using AvansDevOps.Domain.Models;
using AvansDevOps.Domain.Report;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace AvansDevOps.Tests.Domain.Report
{
    [TestClass]
    public class TemplateMethodTests
    {
        private Sprint FinishedSprint()
        {
            var sprint = new Sprint("Sprint 1", DateTime.Now.AddDays(-10), DateTime.Now.AddDays(-1));
            sprint.Start();
            return sprint;
        }

        // TC-45: PdfReportExporter doorloopt alle stappen en vult rapport correct
        [TestMethod]
        public void PdfReportExporter_Export_SetsContentOnReport()
        {
            ReportExporter exporter = new PdfReportExporter();
            SprintReport report = exporter.Export(FinishedSprint());

            Assert.AreEqual("PDF", report.Format);
            Assert.IsNotNull(report.Content);
            Assert.IsNotNull(report.Header);
            Assert.IsNotNull(report.Footer);
            StringAssert.Contains(report.Content, "[PDF]");
            StringAssert.Contains(report.Header, "[PDF HEADER]");
        }

        // TC-46: PngReportExporter doorloopt alle stappen en vult rapport correct
        [TestMethod]
        public void PngReportExporter_Export_SetsContentOnReport()
        {
            ReportExporter exporter = new PngReportExporter();
            SprintReport report = exporter.Export(FinishedSprint());

            Assert.AreEqual("PNG", report.Format);
            Assert.IsNotNull(report.Content);
            Assert.IsNotNull(report.Header);
            Assert.IsNotNull(report.Footer);
            StringAssert.Contains(report.Content, "[PNG]");
            StringAssert.Contains(report.Header, "[PNG HEADER]");
        }

        // TC-47: PDF en PNG produceren aantoonbaar verschillende content
        [TestMethod]
        public void PdfExporter_And_PngExporter_ProduceDifferentContent()
        {
            Sprint sprint = FinishedSprint();
            SprintReport pdfReport = new PdfReportExporter().Export(sprint);
            SprintReport pngReport = new PngReportExporter().Export(sprint);

            Assert.AreNotEqual(pdfReport.Format, pngReport.Format);
            Assert.AreNotEqual(pdfReport.Content, pngReport.Content);
            Assert.AreNotEqual(pdfReport.Header, pngReport.Header);
        }

        // TC-48: Exporter is bruikbaar via abstracte ReportExporter-referentie
        [TestMethod]
        public void ReportExporter_IsUsablePolymorphically()
        {
            ReportExporter exporter = new PdfReportExporter();
            SprintReport report = exporter.Export(FinishedSprint());
            Assert.IsNotNull(report);
        }
    }
}
