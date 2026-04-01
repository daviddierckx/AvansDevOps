namespace AvansDevOps.Domain.Models
{
    // ============================================================
    // COMPOSITE PATTERN - Component interface
    // Doel: Uniforme interface voor zowel enkelvoudige objecten
    // (Activity) als samengestelde objecten (BacklogItem).
    // Clients kunnen beide op dezelfde manier behandelen.
    // ============================================================
    public interface IWorkItem
    {
        bool IsDone();
        int GetEffortPoints();
        string GetStatus();
    }
}