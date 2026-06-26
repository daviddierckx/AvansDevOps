using System;
using AvansDevOps.Domain.Models;

namespace AvansDevOps.Domain.States
{
    // ============================================================
    // STATE PATTERN — Sprint
    // Doel: Elke state-klasse bepaalt zelf welke transities geldig
    // zijn en welke eindstatussen worden bereikt. Sprint delegeert
    // alle aanroepen naar de huidige ISprintState.
    // OO-principe: Open/Closed Principle — nieuwe state = nieuwe klasse.
    // ============================================================

    public interface ISprintState
    {
        SprintStatus GetStatus();
        void Start(Sprint sprint);
        void Finish(Sprint sprint);
    }

    public class CreatedSprintState : ISprintState
    {
        public SprintStatus GetStatus() { return SprintStatus.CREATED; }

        public void Start(Sprint sprint)
        {
            sprint.TransitionTo(new ActiveSprintState());
        }

        public void Finish(Sprint sprint)
        {
            throw new InvalidOperationException("Sprint kan alleen afgerond worden vanuit ACTIVE.");
        }
    }

    public class ActiveSprintState : ISprintState
    {
        public SprintStatus GetStatus() { return SprintStatus.ACTIVE; }

        public void Start(Sprint sprint)
        {
            throw new InvalidOperationException("Sprint kan alleen gestart worden vanuit CREATED.");
        }

        public void Finish(Sprint sprint)
        {
            sprint.TransitionTo(new FinishedSprintState());
        }
    }

    public class FinishedSprintState : ISprintState
    {
        public SprintStatus GetStatus() { return SprintStatus.FINISHED; }

        public void Start(Sprint sprint)
        {
            throw new InvalidOperationException("Sprint kan alleen gestart worden vanuit CREATED.");
        }

        public void Finish(Sprint sprint)
        {
            throw new InvalidOperationException("Sprint kan alleen afgerond worden vanuit ACTIVE.");
        }
    }

    public class ClosedSprintState : ISprintState
    {
        public SprintStatus GetStatus() { return SprintStatus.CLOSED; }

        public void Start(Sprint sprint)
        {
            throw new InvalidOperationException("Sprint is afgesloten en kan niet opnieuw gestart worden.");
        }

        public void Finish(Sprint sprint)
        {
            throw new InvalidOperationException("Sprint is afgesloten en kan niet afgerond worden.");
        }
    }

    public class ReleasedSprintState : ISprintState
    {
        public SprintStatus GetStatus() { return SprintStatus.RELEASED; }

        public void Start(Sprint sprint)
        {
            throw new InvalidOperationException("Sprint is gereleased en kan niet opnieuw gestart worden.");
        }

        public void Finish(Sprint sprint)
        {
            throw new InvalidOperationException("Sprint is gereleased en kan niet afgerond worden.");
        }
    }

    public class CancelledSprintState : ISprintState
    {
        public SprintStatus GetStatus() { return SprintStatus.CANCELLED; }

        public void Start(Sprint sprint)
        {
            throw new InvalidOperationException("Sprint is geannuleerd en kan niet opnieuw gestart worden.");
        }

        public void Finish(Sprint sprint)
        {
            throw new InvalidOperationException("Sprint is geannuleerd en kan niet afgerond worden.");
        }
    }
}
