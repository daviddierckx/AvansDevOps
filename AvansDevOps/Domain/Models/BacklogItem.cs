using System;
using System.Collections.Generic;
using AvansDevOps.Domain.States;
using AvansDevOps.Domain.Notifications;

namespace AvansDevOps.Domain.Models
{
    // ============================================================
    // COMPOSITE PATTERN - Composite node
    // Doel: Behandel individuele objecten (Activity) en composities
    // (BacklogItem) op dezelfde manier via IWorkItem interface.
    // OO-principe: Liskov Substitution Principle — BacklogItem en
    // Activity zijn beide uitwisselbaar als IWorkItem.
    //
    // STATE PATTERN - Context
    // BacklogItem delegeert alle statusovergangen naar het huidige
    // IBacklogItemState object. Bij elke overgang wordt de state
    // vervangen door de nieuwe state.
    //
    // OBSERVER PATTERN - Subject
    // BacklogItem triggert NotificationManager bij statuswijzigingen
    // zodat geregistreerde observers automatisch genotificeerd worden.
    // ============================================================
    public class BacklogItem : IWorkItem
    {
        public string Name { get; set; }
        public string Description { get; set; }
        public int EffortPoints { get; set; }
        public Developer Developer { get; private set; }

        private List<Activity> _activities = new List<Activity>();
        private List<DiscussionThread> _discussionThreads = new List<DiscussionThread>();
        private IBacklogItemState _state;
        private NotificationManager _notificationManager;

        public IBacklogItemState State { get { return _state; } }
        public IReadOnlyList<Activity> Activities { get { return _activities.AsReadOnly(); } }
        public IReadOnlyList<DiscussionThread> DiscussionThreads { get { return _discussionThreads.AsReadOnly(); } }

        public BacklogItem(string name, string description, int effortPoints, NotificationManager notificationManager)
        {
            Name = name;
            Description = description;
            EffortPoints = effortPoints;
            _notificationManager = notificationManager;
            _state = new ToDoState();
        }

        // State Pattern - delegeer naar huidige state
        public void ToDo() { _state.ToDo(this); }
        public void Doing() { _state.Doing(this); }
        public void ReadyForTesting() { _state.ReadyForTesting(this); }
        public void Testing() { _state.Testing(this); }
        public void Tested() { _state.Tested(this); }
        public void Done() { _state.Done(this); }

        public void SetState(IBacklogItemState newState)
        {
            _state = newState;
        }

        public void AssignDeveloper(Developer developer)
        {
            Developer = developer;
        }

        public bool CanEdit()
        {
            return _state.GetName() != "Done";
        }

        public void AddActivity(Activity activity)
        {
            if (!CanEdit())
                throw new InvalidOperationException("Kan geen activiteit toevoegen: item is Done.");
            _activities.Add(activity);
        }

        public void AddDiscussionThread(DiscussionThread thread)
        {
            if (_state.GetName() == "Done")
                throw new InvalidOperationException("Kan geen thread toevoegen: item is Done.");
            _discussionThreads.Add(thread);
        }

        public bool AllActivitiesDone()
        {
            if (_activities.Count == 0) return true;
            foreach (Activity a in _activities)
                if (!a.IsDone()) return false;
            return true;
        }

        public void LockThreads()
        {
            foreach (DiscussionThread thread in _discussionThreads)
                thread.Lock();
        }

        public void UnlockThreads()
        {
            // Threads blijven historisch vergrendeld per casus definitie
        }

        public void NotifyObservers(string eventType)
        {
            _notificationManager.NotifyObservers(eventType, this);
        }

        // IWorkItem interface
        public bool IsDone() { return _state.GetName() == "Done"; }
        public int GetEffortPoints() { return EffortPoints; }
        public string GetStatus() { return _state.GetName(); }
    }
}