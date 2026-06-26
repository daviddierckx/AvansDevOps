using System;
using AvansDevOps.Domain.Models;
using AvansDevOps.Domain.Pipelines;

namespace AvansDevOps.Domain.Factory
{
    // ============================================================
    // FACTORY METHOD PATTERN
    // Doel: Delegeer de aanmaak van sprint-objecten naar gespecialiseerde
    // fabrieken. De aanroepende code werkt uitsluitend met de abstracte
    // SprintFactory en het abstracte Sprint-type.
    // OO-principe: Open/Closed Principle — nieuw sprinttype vereist
    // alleen nieuwe subklassen van fabriek en sprint.
    // ============================================================

    public abstract class SprintFactory
    {
        public abstract Sprint CreateSprint(string name, DateTime startDate, DateTime endDate);
    }

    public class ReviewSprintFactory : SprintFactory
    {
        public override Sprint CreateSprint(string name, DateTime startDate, DateTime endDate)
        {
            return new ReviewSprint(name, startDate, endDate);
        }
    }

    public class ReleaseSprintFactory : SprintFactory
    {
        public override Sprint CreateSprint(string name, DateTime startDate, DateTime endDate)
        {
            return new ReleaseSprint(name, startDate, endDate);
        }
    }

    // ============================================================
    // ReviewSprint — sprint met verplicht review-document voor afsluiting
    // ============================================================
    public class ReviewSprint : Sprint
    {
        public bool HasReviewDocument { get; private set; }

        public ReviewSprint(string name, DateTime startDate, DateTime endDate)
            : base(name, startDate, endDate) { }

        public void UploadReviewDocument()
        {
            HasReviewDocument = true;
            Console.WriteLine("[REVIEW SPRINT] Review document geüpload voor sprint: " + Name);
        }

        public void Close()
        {
            if (!HasReviewDocument)
                throw new InvalidOperationException("Review-sprint kan niet worden afgesloten zonder review-document.");
            ChangeStatus(SprintStatus.CLOSED);
            Console.WriteLine("[REVIEW SPRINT] Sprint afgesloten: " + Name);
        }
    }

    // ============================================================
    // ReleaseSprint — sprint die bij afsluiting automatisch de pipeline start
    // ============================================================
    public class ReleaseSprint : Sprint
    {
        public ReleaseSprint(string name, DateTime startDate, DateTime endDate)
            : base(name, startDate, endDate) { }

        public bool Release()
        {
            if (Pipeline == null)
                throw new InvalidOperationException("Release-sprint heeft geen gekoppelde pipeline.");

            Console.WriteLine("[RELEASE SPRINT] Pipeline starten voor sprint: " + Name);
            Pipeline.Execute();

            if (Pipeline.LastRunSuccessful)
            {
                ChangeStatus(SprintStatus.RELEASED);
                Console.WriteLine("[RELEASE SPRINT] Sprint succesvol gereleased: " + Name);
                return true;
            }
            else
            {
                Console.WriteLine("[RELEASE SPRINT] Pipeline mislukt, sprint blijft in FINISHED: " + Name);
                return false;
            }
        }
    }
}
