using System;
using AvansDevOps.Domain.Models;

namespace AvansDevOps.Domain.Report
{
    // ============================================================
    // TEMPLATE METHOD PATTERN
    // Doel: Definieer het skelet van het export-algoritme eenmalig
    // in de abstracte klasse. Subklassen vullen de formaatspecifieke
    // stappen in zonder de volgorde te wijzigen.
    // OO-principe: Open/Closed Principle — nieuw formaat = nieuwe subklasse.
    // DRY — gedeeld algoritme staat eenmalig in de abstracte klasse.
    // ============================================================

    public class SprintReport
    {
        public string Header { get; set; }
        public string Footer { get; set; }
        public string Content { get; set; }
        public string Format { get; set; }
    }

    public abstract class ReportExporter
    {
        // Template method — definieert de vaste volgorde van stappen
        public SprintReport Export(Sprint sprint)
        {
            var report = new SprintReport();
            CollectData(sprint, report);
            ApplyHeaderFooter(sprint, report);
            WriteOutput(report);
            return report;
        }

        protected abstract void CollectData(Sprint sprint, SprintReport report);
        protected abstract void ApplyHeaderFooter(Sprint sprint, SprintReport report);
        protected abstract void WriteOutput(SprintReport report);
    }

    public class PdfReportExporter : ReportExporter
    {
        protected override void CollectData(Sprint sprint, SprintReport report)
        {
            report.Format = "PDF";
            report.Content = "[PDF] Sprint: " + sprint.Name
                + " | Items: " + sprint.Backlog.Count
                + " | Afgerond: " + sprint.GetDoneItemCount();
        }

        protected override void ApplyHeaderFooter(Sprint sprint, SprintReport report)
        {
            report.Header = "[PDF HEADER] " + sprint.Name + " - " + DateTime.Now.ToString("dd-MM-yyyy");
            report.Footer = "[PDF FOOTER] Pagina 1";
        }

        protected override void WriteOutput(SprintReport report)
        {
            Console.WriteLine("[PDF] Exporteren naar PDF...");
            Console.WriteLine("  Header : " + report.Header);
            Console.WriteLine("  Content: " + report.Content);
            Console.WriteLine("  Footer : " + report.Footer);
        }
    }

    public class PngReportExporter : ReportExporter
    {
        protected override void CollectData(Sprint sprint, SprintReport report)
        {
            report.Format = "PNG";
            report.Content = "[PNG] Sprint: " + sprint.Name
                + " | Items: " + sprint.Backlog.Count
                + " | Afgerond: " + sprint.GetDoneItemCount();
        }

        protected override void ApplyHeaderFooter(Sprint sprint, SprintReport report)
        {
            report.Header = "[PNG HEADER] " + sprint.Name + " - " + DateTime.Now.ToString("dd-MM-yyyy");
            report.Footer = "[PNG FOOTER] Gegenereerd door AvansDevOps";
        }

        protected override void WriteOutput(SprintReport report)
        {
            Console.WriteLine("[PNG] Exporteren naar PNG...");
            Console.WriteLine("  Header : " + report.Header);
            Console.WriteLine("  Content: " + report.Content);
            Console.WriteLine("  Footer : " + report.Footer);
        }
    }
}
