using System.Collections.Generic;
using AvansDevOps.Domain.Models;
using AvansDevOps.Domain.Notifications;

namespace AvansDevOps.Domain.Observer
{
    // ============================================================
    // OBSERVER PATTERN
    // Doel: Definieer een één-op-veel afhankelijkheid zodat wanneer
    // een BacklogItem van status wijzigt, alle geregistreerde
    // observers automatisch genotificeerd worden.
    // OO-principe: Open/Closed Principle — nieuwe observers
    // toevoegen zonder BacklogItem te wijzigen.
    // ============================================================

    // Observer Pattern - subject interface (observer)
    public interface IObserver
    {
        void Update(string eventType, BacklogItem item);
    }

    // Observer Pattern - concrete observer voor testers
    // Reageert op event: "ready_for_testing"
    public class TesterNotifier : IObserver
    {
        private List<Developer> _testers;
        private NotificationManager _notificationManager;

        public TesterNotifier(List<Developer> testers, NotificationManager notificationManager)
        {
            _testers = testers;
            _notificationManager = notificationManager;
        }

        public void Update(string eventType, BacklogItem item)
        {
            if (eventType == "ready_for_testing")
            {
                foreach (Developer tester in _testers)
                    _notificationManager.Notify("BacklogItem '" + item.Name + "' is klaar voor testing.", tester.Name);
            }
        }
    }

    // Observer Pattern - concrete observer voor developers
    // Reageert op event: "returned_to_todo"
    public class DeveloperNotifier : IObserver
    {
        private List<Developer> _developers;
        private NotificationManager _notificationManager;

        public DeveloperNotifier(List<Developer> developers, NotificationManager notificationManager)
        {
            _developers = developers;
            _notificationManager = notificationManager;
        }

        public void Update(string eventType, BacklogItem item)
        {
            if (eventType == "returned_to_todo")
            {
                foreach (Developer developer in _developers)
                    _notificationManager.Notify("BacklogItem '" + item.Name + "' is teruggezet naar ToDo.", developer.Name);
            }
        }
    }

    // Observer Pattern - concrete observer voor scrum master
    // Reageert op event: "returned_to_todo"
    public class ScrumMasterNotifier : IObserver
    {
        private Developer _scrumMaster;
        private NotificationManager _notificationManager;

        public ScrumMasterNotifier(Developer scrumMaster, NotificationManager notificationManager)
        {
            _scrumMaster = scrumMaster;
            _notificationManager = notificationManager;
        }

        public void Update(string eventType, BacklogItem item)
        {
            if (eventType == "returned_to_todo")
                _notificationManager.Notify("LET OP: BacklogItem '" + item.Name + "' is teruggezet naar ToDo.", _scrumMaster.Name);
        }
    }

    // Observer Pattern - concrete observer voor product owner
    // Reageert op events: "sprint_released", "sprint_cancelled"
    public class ProductOwnerObserver : IObserver
    {
        private Developer _productOwner;
        private NotificationManager _notificationManager;

        public ProductOwnerObserver(Developer productOwner, NotificationManager notificationManager)
        {
            _productOwner = productOwner;
            _notificationManager = notificationManager;
        }

        public void Update(string eventType, BacklogItem item)
        {
            if (eventType == "sprint_released" || eventType == "sprint_cancelled")
                _notificationManager.Notify("Sprint event: " + eventType + " voor item '" + item.Name + "'.", _productOwner.Name);
        }
    }
}