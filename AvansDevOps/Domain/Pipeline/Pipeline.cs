using System;
using System.Collections.Generic;
using AvansDevOps.Domain.Strategy;

namespace AvansDevOps.Domain.Pipelines
{
    // ============================================================
    // STRATEGY PATTERN - Pipeline context
    // Pipeline ondersteunt twee uitvoermodi:
    // 1. Legacy: IActionStrategy-acties via Execute() (bestaand)
    // 2. Nieuw: PipelineAction + IPipelineStrategy via ExecuteWithStrategy()
    // ============================================================
    public class Pipeline
    {
        public string Name { get; set; }
        public bool LastRunSuccessful { get; private set; }

        private List<IActionStrategy> _actions = new List<IActionStrategy>();
        private List<PipelineAction> _pipelineActions = new List<PipelineAction>();
        private IPipelineStrategy _strategy = new FastFailPipelineStrategy();

        public IReadOnlyList<IActionStrategy> Actions { get { return _actions.AsReadOnly(); } }
        public IReadOnlyList<PipelineAction> PipelineActions { get { return _pipelineActions.AsReadOnly(); } }

        public Pipeline(string name) { Name = name; }

        public void AddAction(IActionStrategy action) { _actions.Add(action); }

        public void AddPipelineAction(PipelineAction action) { _pipelineActions.Add(action); }

        public void SetStrategy(IPipelineStrategy strategy) { _strategy = strategy; }

        // Legacy uitvoering via IActionStrategy
        public void Execute()
        {
            Console.WriteLine("[PIPELINE] '" + Name + "' gestart...");
            try
            {
                foreach (IActionStrategy action in _actions)
                    action.Execute();
                LastRunSuccessful = true;
                Console.WriteLine("[PIPELINE] '" + Name + "' succesvol afgerond.");
            }
            catch (Exception ex)
            {
                LastRunSuccessful = false;
                Console.WriteLine("[PIPELINE] '" + Name + "' mislukt: " + ex.Message);
            }
        }

        // Nieuwe uitvoering via PipelineAction + IPipelineStrategy
        public bool ExecuteWithStrategy()
        {
            Console.WriteLine("[PIPELINE] '" + Name + "' gestart met strategie...");
            LastRunSuccessful = _strategy.Execute(_pipelineActions);
            Console.WriteLine("[PIPELINE] '" + Name + "' " + (LastRunSuccessful ? "succesvol." : "mislukt."));
            return LastRunSuccessful;
        }
    }
}