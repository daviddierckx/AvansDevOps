using System.Collections.Generic;

namespace AvansDevOps.Domain.Models
{
    public class Project
    {
        public string Name { get; set; }
        private List<Sprint> _sprints = new List<Sprint>();
        private List<BacklogItem> _backlog = new List<BacklogItem>();

        public IReadOnlyList<Sprint> Sprints { get { return _sprints.AsReadOnly(); } }
        public IReadOnlyList<BacklogItem> Backlog { get { return _backlog.AsReadOnly(); } }

        public Project(string name)
        {
            Name = name;
        }

        public void AddSprint(Sprint sprint)
        {
            _sprints.Add(sprint);
        }

        public void AddBacklogItem(BacklogItem item)
        {
            _backlog.Add(item);
        }
    }
}