using System;
using System.Collections.Generic;
using AvansDevOps.Domain.Strategy;

namespace AvansDevOps.Domain.Models
{
    public class DiscussionThread
    {
        public string Name { get; set; }
        public bool IsLocked { get; private set; }
        private List<IMessageStrategy> _messages = new List<IMessageStrategy>();

        public IReadOnlyList<IMessageStrategy> Messages
        {
            get { return _messages.AsReadOnly(); }
        }

        public DiscussionThread(string name)
        {
            Name = name;
            IsLocked = false;
        }

        public void AddMessage(IMessageStrategy message)
        {
            if (IsLocked)
                throw new InvalidOperationException("Thread is vergrendeld. Backlog item is afgerond.");
            _messages.Add(message);
            PrintMessage(message);
        }

        private void PrintMessage(IMessageStrategy message)
        {
            ConsoleColor color = HexToConsoleColor(message.GetColor());
            Console.ForegroundColor = color;
            Console.WriteLine("[" + message.Author.Role + "] " + message.Author.Name + ": " + message.Content);
            Console.ResetColor();
        }

        private ConsoleColor HexToConsoleColor(string hex)
        {
            switch (hex)
            {
                case "#FFD700": return ConsoleColor.Yellow;    // ProductOwner - goud
                case "#378ADD": return ConsoleColor.Cyan;      // ScrumMaster - blauw
                case "#1D9E75": return ConsoleColor.Green;     // Tester - groen
                default: return ConsoleColor.White;     // Developer - wit
            }
        }

        public void Lock()
        {
            IsLocked = true;
        }
    }
}