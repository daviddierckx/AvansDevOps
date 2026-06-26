using System;
using System.Collections.Generic;

namespace AvansDevOps.Domain.Pipelines
{
    // ============================================================
    // COMPOSITE PATTERN
    // Doel: Behandel enkelvoudige acties (leaves) en groepen (composites)
    // op dezelfde manier via PipelineAction. Pipeline roept Execute()
    // aan zonder te weten of het een leaf of groep betreft.
    // OO-principe: Open/Closed Principle — nieuw actietype = nieuwe subklasse.
    // ============================================================

    public abstract class PipelineAction
    {
        public string Name { get; protected set; }
        public abstract bool Execute();
    }

    // Composite — beheert en voert een lijst van PipelineActions sequentieel uit
    public class PipelineActionGroup : PipelineAction
    {
        private List<PipelineAction> _children = new List<PipelineAction>();

        public PipelineActionGroup(string name) { Name = name; }

        public void Add(PipelineAction action) { _children.Add(action); }

        public override bool Execute()
        {
            Console.WriteLine("[GROUP] Groep '" + Name + "' starten...");
            foreach (PipelineAction action in _children)
            {
                if (!action.Execute())
                {
                    Console.WriteLine("[GROUP] Actie mislukt in groep '" + Name + "', stoppen.");
                    return false;
                }
            }
            Console.WriteLine("[GROUP] Groep '" + Name + "' succesvol.");
            return true;
        }
    }

    // Leaf actions — zeven pipeline-actietypen
    public class SourcesAction : PipelineAction
    {
        public SourcesAction() { Name = "Sources"; }
        public override bool Execute()
        {
            Console.WriteLine("[PIPELINE] Sources: broncode ophalen...");
            return true;
        }
    }

    public class PackageAction : PipelineAction
    {
        public PackageAction() { Name = "Package"; }
        public override bool Execute()
        {
            Console.WriteLine("[PIPELINE] Package: pakketten herstellen...");
            return true;
        }
    }

    public class BuildAction : PipelineAction
    {
        private bool _succeeds;
        public BuildAction(bool succeeds = true) { Name = "Build"; _succeeds = succeeds; }
        public override bool Execute()
        {
            Console.WriteLine("[PIPELINE] Build: code bouwen...");
            return _succeeds;
        }
    }

    public class TestAction : PipelineAction
    {
        private bool _succeeds;
        public TestAction(bool succeeds = true) { Name = "Test"; _succeeds = succeeds; }
        public override bool Execute()
        {
            Console.WriteLine("[PIPELINE] Test: unit tests uitvoeren...");
            return _succeeds;
        }
    }

    public class AnalyseAction : PipelineAction
    {
        public AnalyseAction() { Name = "Analyse"; }
        public override bool Execute()
        {
            Console.WriteLine("[PIPELINE] Analyse: code analyseren...");
            return true;
        }
    }

    public class DeployAction : PipelineAction
    {
        public DeployAction() { Name = "Deploy"; }
        public override bool Execute()
        {
            Console.WriteLine("[PIPELINE] Deploy: deployment uitvoeren...");
            return true;
        }
    }

    public class UtilityAction : PipelineAction
    {
        public UtilityAction() { Name = "Utility"; }
        public override bool Execute()
        {
            Console.WriteLine("[PIPELINE] Utility: hulptaak uitvoeren...");
            return true;
        }
    }
}
