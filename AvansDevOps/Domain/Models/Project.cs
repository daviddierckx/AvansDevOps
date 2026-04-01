using System.Collections.Generic;

namespace AvansDevOps.Domain.Models
{
    public class Project
    {
        private string _name;

        public string Name
        {
            get { return _name; }
            set
            {
                if (string.IsNullOrWhiteSpace(value))
                    throw new System.ArgumentException("Projectnaam mag niet leeg zijn.", nameof(value));
                _name = value;
            }
        }
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