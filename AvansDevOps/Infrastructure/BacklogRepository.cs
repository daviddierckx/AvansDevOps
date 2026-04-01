using System;
using System.Collections.Generic;
using AvansDevOps.Domain.Models;

namespace AvansDevOps.Infrastructure
{
    // ============================================================
    // CLEAN ARCHITECTURE - Repository pattern
    // Doel: Abstracteer de opslag van BacklogItems achter een
    // interface zodat de application core onafhankelijk blijft
    // van de infrastructure laag (NFR4).
    // OO-principe: Dependency Inversion Principle — de core
    // hangt af van IBacklogRepository, niet van de implementatie.
    // FakeBacklogRepository is een in-memory stub voor testen.
    // ============================================================

    // Repository interface - application core gebruikt alleen dit
    public interface IBacklogRepository
    {
        void Save(BacklogItem item);
        BacklogItem GetById(Guid id);
        List<BacklogItem> GetAll();
    }

    // Fake in-memory implementatie (stub)
    public class FakeBacklogRepository : IBacklogRepository
    {
        private Dictionary<Guid, BacklogItem> _store = new Dictionary<Guid, BacklogItem>();

        public void Save(BacklogItem item)
        {
            Guid id = Guid.NewGuid();
            _store[id] = item;
            Console.WriteLine("[REPO] BacklogItem '" + item.Name + "' opgeslagen.");
        }

        public BacklogItem GetById(Guid id)
        {
            BacklogItem item;
            return _store.TryGetValue(id, out item) ? item : null;
        }

        public List<BacklogItem> GetAll()
        {
            List<BacklogItem> result = new List<BacklogItem>();
            foreach (BacklogItem item in _store.Values)
                result.Add(item);
            return result;
        }
    }
}