using System;

namespace AvansDevOps.Domain.Strategy
{
    // ============================================================
    // STRATEGY PATTERN - pipeline acties
    // Doel: Definieer een familie van algoritmen (pipeline acties),
    // zodat ze uitwisselbaar zijn. De Pipeline klasse kan elke
    // combinatie van acties uitvoeren zonder de implementatie
    // te kennen.
    // OO-principe: Open/Closed Principle — nieuwe pipeline acties
    // toevoegen zonder Pipeline.Execute() te wijzigen.
    // ============================================================

    // Strategy Pattern - strategy interface voor pipeline acties
    public interface IActionStrategy
    {
        void Execute();
        string GetStrategyName();
    }

    // Strategy Pattern - concrete strategy: Build
    public class BuildStrategy : IActionStrategy
    {
        public void Execute()
        {
            Console.WriteLine("[PIPELINE] Build: code bouwen...");
        }
        public string GetStrategyName() { return "Build"; }
    }

    // Strategy Pattern - concrete strategy: Test
    public class TestStrategy : IActionStrategy
    {
        public void Execute()
        {
            Console.WriteLine("[PIPELINE] Test: unit tests uitvoeren...");
        }
        public string GetStrategyName() { return "Test"; }
    }

    // Strategy Pattern - concrete strategy: Deploy
    public class DeployStrategy : IActionStrategy
    {
        public void Execute()
        {
            Console.WriteLine("[PIPELINE] Deploy: deployment uitvoeren...");
        }
        public string GetStrategyName() { return "Deploy"; }
    }
}