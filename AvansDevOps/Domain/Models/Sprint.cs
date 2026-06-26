using System;
using System.Collections.Generic;
using AvansDevOps.Domain.Factory;
using AvansDevOps.Domain.Pipelines;
using AvansDevOps.Domain.States;

namespace AvansDevOps.Domain.Models
{
    // ============================================================
    // STATE PATTERN — Context
    // Sprint delegeert Start() en Finish() naar het huidige
    // ISprintState object. Elke state bepaalt welke transities
    // geldig zijn en gooit een exception bij ongeldige aanroepen.
    // ============================================================
    public class Sprint
    {
        public string Name { get; private set; }
        public DateTime StartDate { get; private set; }
        public DateTime EndDate { get; private set; }
        public Developer ScrumMaster { get; private set; }
        public Pipeline Pipeline { get; private set; }

        private ISprintState _state;

        public SprintStatus Status
        {
            get
            {
                AutoFinishIfExpired(DateTime.Now);
                return _state.GetStatus();
            }
        }

        private List<Developer> _developers = new List<Developer>();
        private List<BacklogItem> _backlog = new List<BacklogItem>();

        public IReadOnlyList<Developer> Developers { get { return _developers.AsReadOnly(); } }
        public IReadOnlyList<BacklogItem> Backlog { get { return _backlog.AsReadOnly(); } }

        public Sprint(string name, DateTime startDate, DateTime endDate)
        {
            Name = name;
            StartDate = startDate;
            EndDate = endDate;
            _state = new CreatedSprintState();
        }

        public void TransitionTo(ISprintState newState)
        {
            _state = newState;
        }

        public void AutoFinishIfExpired(DateTime currentDateTime)
        {
            if (_state.GetStatus() == SprintStatus.ACTIVE && currentDateTime > EndDate)
                _state = new FinishedSprintState();
        }

        public void SetScrumMaster(Developer scrumMaster)
        {
            if (scrumMaster.Role != Role.SCRUMMASTER)
                throw new InvalidOperationException("Developer heeft niet de rol ScrumMaster.");
            ScrumMaster = scrumMaster;
        }

        public void AddDeveloper(Developer developer)
        {
            _developers.Add(developer);
        }

        public void SetPipeline(Pipeline pipeline)
        {
            if (Status == SprintStatus.ACTIVE)
                throw new InvalidOperationException("Pipeline kan niet gewijzigd worden tijdens actieve sprint.");
            Pipeline = pipeline;
        }

        public void AddBacklogItem(BacklogItem item)
        {
            if (Status == SprintStatus.ACTIVE)
                throw new InvalidOperationException("Backlog item kan niet toegevoegd worden tijdens actieve sprint.");
            _backlog.Add(item);
        }

        public void RemoveBacklogItem(BacklogItem item)
        {
            if (Status == SprintStatus.ACTIVE)
                throw new InvalidOperationException("Backlog item kan niet verwijderd worden tijdens actieve sprint.");
            _backlog.Remove(item);
        }

        public void Start()
        {
            _state.Start(this);
            AutoFinishIfExpired(DateTime.Now);
            Console.WriteLine("[SPRINT] '" + Name + "' gestart.");
        }

        public void Finish()
        {
            _state.Finish(this);
            Console.WriteLine("[SPRINT] '" + Name + "' afgerond.");
        }

        public void ChangeStatus(SprintStatus newStatus)
        {
            if (Status == SprintStatus.ACTIVE)
                throw new InvalidOperationException("Sprint status kan niet gewijzigd worden tijdens uitvoering.");

            switch (newStatus)
            {
                case SprintStatus.FINISHED:   _state = new FinishedSprintState(); break;
                case SprintStatus.CLOSED:     _state = new ClosedSprintState(); break;
                case SprintStatus.RELEASED:   _state = new ReleasedSprintState(); break;
                case SprintStatus.CANCELLED:  _state = new CancelledSprintState(); break;
                default:
                    throw new InvalidOperationException("Ongeldige status: " + newStatus);
            }
        }

        public void GenerateRapport(IRapportFactory rapportFactory)
        {
            if (Status != SprintStatus.FINISHED)
                throw new InvalidOperationException("Rapport kan alleen gegenereerd worden voor afgeronde sprint.");
            rapportFactory.Generate(this);
        }

        public int GetDoneItemCount()
        {
            int count = 0;
            foreach (BacklogItem item in _backlog)
                if (item.IsDone()) count++;
            return count;
        }
    }
}
