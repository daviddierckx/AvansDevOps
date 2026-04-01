using AvansDevOps.Domain.Models;

namespace AvansDevOps.Domain.States
{
    // ============================================================
    // STATE PATTERN
    // Doel: Laat een object zijn gedrag aanpassen wanneer zijn
    // interne toestand verandert. Een BacklogItem delegeert alle
    // statusovergangen naar het huidige state object.
    // OO-principe: Single Responsibility Principle — elke state
    // klasse is verantwoordelijk voor zijn eigen overgangslogica.
    // ============================================================

    // State Pattern - state interface
    public interface IBacklogItemState
    {
        void ToDo(BacklogItem item);
        void Doing(BacklogItem item);
        void ReadyForTesting(BacklogItem item);
        void Testing(BacklogItem item);
        void Tested(BacklogItem item);
        void Done(BacklogItem item);
        string GetName();
    }
}