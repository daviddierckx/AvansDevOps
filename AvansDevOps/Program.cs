using System;
using System.Collections.Generic;
using AvansDevOps.Domain.Models;
using AvansDevOps.Domain.Notifications;
using AvansDevOps.Domain.Observer;
using AvansDevOps.Domain.Pipelines;
using AvansDevOps.Domain.Factory;
using AvansDevOps.Domain.Strategy;
using AvansDevOps.Infrastructure;

namespace AvansDevOps
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("=== Avans DevOps - Demo ===\n");

            // ─── Adapter Pattern: notificatiekanalen instellen ───
            NotificationManager notificationManager = new NotificationManager();
            notificationManager.AddChannel(new EmailAdapter("smtp.avans.nl", 587, "devops@avans.nl", "secret"));
            notificationManager.AddChannel(new SlackAdapter("https://hooks.slack.com/xxx", "C123", "xoxb-token"));

            // ─── Domein objecten aanmaken met Role ───
            Developer developer = new Developer("Jan Janssen", Role.DEVELOPER);
            Developer tester = new Developer("Piet Pieters", Role.TESTER);
            Developer scrumMaster = new Developer("Kees Keessen", Role.SCRUMMASTER);
            Developer productOwner = new Developer("Lisa Lissens", Role.PRODUCTOWNER);

            // ─── Observer Pattern: subscribers aanmelden ───
            notificationManager.Subscribe(new TesterNotifier(
                new List<Developer> { tester }, notificationManager));
            notificationManager.Subscribe(new ScrumMasterNotifier(
                scrumMaster, notificationManager));
            notificationManager.Subscribe(new DeveloperNotifier(
                new List<Developer> { developer }, notificationManager));
            notificationManager.Subscribe(new ProductOwnerObserver(
                productOwner, notificationManager));

            // ─── Strategy Pattern: pipeline acties ───
            Pipeline pipeline = new Pipeline("Release Pipeline");
            pipeline.AddAction(new BuildStrategy());
            pipeline.AddAction(new TestStrategy());
            pipeline.AddAction(new DeployStrategy());

            // ─── Sprint aanmaken ───
            Sprint sprint = new Sprint("Sprint 1", DateTime.Now, DateTime.Now.AddDays(14));
            sprint.SetScrumMaster(scrumMaster);
            sprint.AddDeveloper(developer);
            sprint.AddDeveloper(tester);
            sprint.SetPipeline(pipeline);

            // ─── Composite Pattern: BacklogItem met Activities ───
            BacklogItem backlogItem = new BacklogItem(
                "Login functionaliteit", "Gebruiker kan inloggen", 5, notificationManager);
            backlogItem.AssignDeveloper(developer);

            Activity activity1 = new Activity("UI bouwen", "Login scherm ontwerpen");
            Activity activity2 = new Activity("API koppelen", "Backend koppeling maken");
            backlogItem.AddActivity(activity1);
            backlogItem.AddActivity(activity2);

            // ─── DiscussionThread toevoegen (Strategy Pattern - berichten) ───
            DiscussionThread thread = new DiscussionThread("Discussie over login flow");
            thread.AddMessage(new OwnerStrategy("Moeten we OAuth gebruiken?", productOwner));
            thread.AddMessage(new GuestStrategy("Ja, OAuth is veiliger.", tester));
            thread.AddMessage(new GuestStrategy("Ik ga dit implementeren.", developer));
            backlogItem.AddDiscussionThread(thread);

            // ─── Project opbouwen ───
            Project project = new Project("Avans DevOps Project");
            sprint.AddBacklogItem(backlogItem);
            project.AddSprint(sprint);
            project.AddBacklogItem(backlogItem);

            // ─── State Pattern: statusovergangen ───
            Console.WriteLine("\n--- Statusovergangen ---");
            Console.WriteLine("Status: " + backlogItem.State.GetName());

            sprint.Start();
            backlogItem.Doing();
            Console.WriteLine("Status: " + backlogItem.State.GetName());

            activity1.Complete();
            activity2.Complete();

            backlogItem.ReadyForTesting();
            Console.WriteLine("Status: " + backlogItem.State.GetName());

            backlogItem.Testing();
            Console.WriteLine("Status: " + backlogItem.State.GetName());

            backlogItem.Tested();
            Console.WriteLine("Status: " + backlogItem.State.GetName());

            backlogItem.Done();
            Console.WriteLine("Status: " + backlogItem.State.GetName());

            // ─── Sprint afronden ───
            sprint.Finish();

            // ─── Factory Method Pattern: rapporten genereren ───
            Console.WriteLine("\n--- Rapporten ---");
            sprint.GenerateRapport(new BurndownChart());
            sprint.GenerateRapport(new EffortPointsChart());
            sprint.GenerateRapport(new TeamCompChart());

            // ─── Pipeline uitvoeren ───
            Console.WriteLine("\n--- Pipeline ---");
            pipeline.Execute();

            // ─── Repository ───
            Console.WriteLine("\n--- Repository ---");
            IBacklogRepository repo = new FakeBacklogRepository();
            repo.Save(backlogItem);
            List<BacklogItem> allItems = repo.GetAll();
            Console.WriteLine("Opgeslagen items: " + allItems.Count);
        }
    }
}