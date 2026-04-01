using System;

namespace AvansDevOps.Domain.Models
{
    public class Message
    {
        public string Content { get; private set; }
        public DateTime Timestamp { get; private set; }
        public Developer Author { get; private set; }

        public Message(string content, Developer author)
            : this(content, DateTime.Now, author)
        {
        }

        public Message(string content, DateTime timestamp, Developer author)
        {
            if (string.IsNullOrWhiteSpace(content))
                throw new ArgumentException("Berichtinhoud mag niet leeg zijn.", nameof(content));
            if (author == null)
                throw new ArgumentNullException(nameof(author));

            Content = content;
            Timestamp = timestamp;
            Author = author;
        }

        public virtual string GetColor()
        {
            switch (Author.Role)
            {
                case Role.PRODUCTOWNER: return "#FFD700";
                case Role.SCRUMMASTER: return "#378ADD";
                case Role.TESTER: return "#1D9E75";
                default: return "#FFFFFF";
            }
        }
    }
}
