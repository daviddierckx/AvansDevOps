using System;
using AvansDevOps.Domain.Models;

namespace AvansDevOps.Domain.Strategy
{
    // ============================================================
    // STRATEGY PATTERN - berichtweergave
    // Doel: Definieer een familie van algoritmen voor de weergave
    // van berichten in een DiscussionThread. De kleur van een
    // bericht wordt bepaald door de rol van de auteur.
    // OO-principe: Open/Closed Principle — nieuwe berichtstijlen
    // toevoegen zonder DiscussionThread te wijzigen.
    // ============================================================

    // Strategy Pattern - strategy interface voor berichtweergave
    public interface IMessageStrategy
    {
        string Content { get; }
        DateTime TimeStamp { get; }
        Developer Author { get; }
        string GetColor();
    }

    // Strategy Pattern - concrete strategy voor ProductOwner berichten
    // ProductOwner krijgt een gouden kleur (#FFD700)
    public class OwnerStrategy : IMessageStrategy
    {
        public string Content { get; private set; }
        public DateTime TimeStamp { get; private set; }
        public Developer Author { get; private set; }

        public OwnerStrategy(string content, Developer author)
        {
            if (author.Role != Role.PRODUCTOWNER)
                throw new InvalidOperationException("OwnerStrategy kan alleen gebruikt worden door een ProductOwner.");
            Content = content;
            Author = author;
            TimeStamp = DateTime.Now;
        }

        public string GetColor()
        {
            return "#FFD700"; // Goud voor ProductOwner
        }
    }

    // Strategy Pattern - concrete strategy voor alle andere rollen
    // Kleur wordt bepaald op basis van de rol van de auteur
    public class GuestStrategy : IMessageStrategy
    {
        public string Content { get; private set; }
        public DateTime TimeStamp { get; private set; }
        public Developer Author { get; private set; }

        public GuestStrategy(string content, Developer author)
        {
            Content = content;
            Author = author;
            TimeStamp = DateTime.Now;
        }

        public string GetColor()
        {
            // Kleur op basis van rol
            switch (Author.Role)
            {
                case Role.SCRUMMASTER:
                    return "#378ADD"; // Blauw voor ScrumMaster
                case Role.TESTER:
                    return "#1D9E75"; // Groen voor Tester
                case Role.DEVELOPER:
                    return "#FFFFFF"; // Wit voor Developer
                default:
                    return "#FFFFFF";
            }
        }
    }
}