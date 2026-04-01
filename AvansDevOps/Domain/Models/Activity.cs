namespace AvansDevOps.Domain.Models
{
    // ============================================================
    // COMPOSITE PATTERN - Leaf node
    // Activity is het enkelvoudige object in de Composite structuur.
    // Een BacklogItem (Composite) bevat 0 of meer Activities (Leaf).
    // IsDone() geeft true wanneer de activiteit voltooid is.
    // ============================================================
    public class Activity : IWorkItem
    {
        public string Name { get; set; }
        public string Description { get; set; }
        public bool IsCompleted { get; private set; }

        public Activity(string name, string description)
        {
            Name = name;
            Description = description;
            IsCompleted = false;
        }

        public void Complete()
        {
            IsCompleted = true;
        }

        public bool IsDone()
        {
            return IsCompleted;
        }

        public int GetEffortPoints()
        {
            return 0;
        }

        public string GetStatus()
        {
            return IsCompleted ? "Done" : "Todo";
        }
    }
}