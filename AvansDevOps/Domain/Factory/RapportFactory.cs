using System;
using AvansDevOps.Domain.Models;

namespace AvansDevOps.Domain.Factory
{
    // ============================================================
    // FACTORY METHOD PATTERN
    // Doel: Definieer een interface voor het aanmaken van rapporten,
    // zodat subklassen bepalen welk type rapport gegenereerd wordt.
    // OO-principe: Open/Closed Principle — nieuwe rapporttypen
    // toevoegen zonder bestaande code te wijzigen.
    // ============================================================

    // Factory Method Pattern - abstracte creator interface
    public interface IRapportFactory
    {
        void Generate(Sprint sprint);
        string GetStrategyName();
    }

    // Factory Method Pattern - concrete creator: BurndownChart
    public class BurndownChart : IRapportFactory
    {
        public void Generate(Sprint sprint)
        {
            Console.WriteLine("[RAPPORT] Burndown chart voor sprint: " + sprint.Name);
            Console.WriteLine("  Start: " + sprint.StartDate.ToString("dd-MM-yyyy") + " | Eind: " + sprint.EndDate.ToString("dd-MM-yyyy"));
            Console.WriteLine("  Totaal items: " + sprint.Backlog.Count);
            Console.WriteLine("  Afgerond: " + sprint.GetDoneItemCount());
        }

        public string GetStrategyName() { return "BurndownChart"; }
    }

    // Factory Method Pattern - concrete creator: EffortPointsChart
    public class EffortPointsChart : IRapportFactory
    {
        public void Generate(Sprint sprint)
        {
            Console.WriteLine("[RAPPORT] Effort points chart voor sprint: " + sprint.Name);
            foreach (Developer dev in sprint.Developers)
            {
                int points = 0;
                foreach (BacklogItem item in sprint.Backlog)
                    if (item.Developer != null && item.Developer.Name == dev.Name)
                        points += item.EffortPoints;
                Console.WriteLine("  " + dev.Name + ": " + points + " punten");
            }
        }

        public string GetStrategyName() { return "EffortPointsChart"; }
    }

    // Factory Method Pattern - concrete creator: TeamCompChart
    public class TeamCompChart : IRapportFactory
    {
        public void Generate(Sprint sprint)
        {
            Console.WriteLine("[RAPPORT] Team samenstelling voor sprint: " + sprint.Name);
            Console.WriteLine("  Scrum Master: " + (sprint.ScrumMaster != null ? sprint.ScrumMaster.Name : "Niet ingesteld"));
            foreach (Developer dev in sprint.Developers)
                Console.WriteLine("  Developer: " + dev.Name);
        }

        public string GetStrategyName() { return "TeamCompChart"; }
    }
}