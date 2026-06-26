using System;
using System.Collections.Generic;

namespace AvansDevOps.Domain.Pipelines
{
    // ============================================================
    // STRATEGY PATTERN — pipeline uitvoerlogica
    // Doel: Wissel uitvoerstrategie uit zonder Pipeline te wijzigen.
    // SequentialPipelineStrategy: gaat door na fout (development).
    // FastFailPipelineStrategy: stopt bij eerste fout (productie).
    // OO-principe: Open/Closed Principle — nieuwe strategie = nieuwe klasse.
    // ============================================================

    public interface IPipelineStrategy
    {
        bool Execute(IReadOnlyList<PipelineAction> actions);
    }

    // Voert alle acties uit, ook na een fout — retourneert false als er één mislukt
    public class SequentialPipelineStrategy : IPipelineStrategy
    {
        public bool Execute(IReadOnlyList<PipelineAction> actions)
        {
            bool allSucceeded = true;
            foreach (PipelineAction action in actions)
            {
                if (!action.Execute())
                {
                    Console.WriteLine("[SEQUENTIAL] Actie '" + action.Name + "' mislukt, doorgaan...");
                    allSucceeded = false;
                }
            }
            return allSucceeded;
        }
    }

    // Stopt direct bij de eerste mislukte actie
    public class FastFailPipelineStrategy : IPipelineStrategy
    {
        public bool Execute(IReadOnlyList<PipelineAction> actions)
        {
            foreach (PipelineAction action in actions)
            {
                if (!action.Execute())
                {
                    Console.WriteLine("[FAST-FAIL] Actie '" + action.Name + "' mislukt, pipeline gestopt.");
                    return false;
                }
            }
            return true;
        }
    }
}
