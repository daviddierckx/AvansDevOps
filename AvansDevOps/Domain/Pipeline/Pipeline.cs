using System;
using System.Collections.Generic;
using AvansDevOps.Domain.Strategy;

namespace AvansDevOps.Domain.Pipelines
{
    // ============================================================
    // STRATEGY PATTERN - Pipeline context
    // Pipeline is de context klasse die een lijst van IActionStrategy
    // objecten bevat en uitvoert. Elke strategie vertegenwoordigt
    // een stap in de development pipeline (Build, Test, Deploy).
    // Door strategies toe te voegen via AddAction() is de pipeline
    // volledig configureerbaar zonder Pipeline zelf te wijzigen.
    // ============================================================
    public class Pipeline
    {
        public string Name { get; set; }
        public bool LastRunSuccessful { get; private set; }
        private List<IActionStrategy> _actions = new List<IActionStrategy>();

        public IReadOnlyList<IActionStrategy> Actions { get { return _actions.AsReadOnly(); } }

        public Pipeline(string name)
        {
            Name = name;
        }

        public void AddAction(IActionStrategy action)
        {
            _actions.Add(action);
        }

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
    }
}