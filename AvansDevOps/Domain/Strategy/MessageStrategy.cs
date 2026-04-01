using System;
using AvansDevOps.Domain.Models;

namespace AvansDevOps.Domain.Strategy
{
    // Kept for compatibility with existing Program usage.
    public class OwnerStrategy : Message
    {
        public OwnerStrategy(string content, Developer author)
            : base(content, DateTime.Now, author)
        {
        }

        public override string GetColor()
        {
            return "#FFD700";
        }
    }

    // Kept for compatibility with existing Program usage.
    public class GuestStrategy : Message
    {
        public GuestStrategy(string content, Developer author)
            : base(content, DateTime.Now, author)
        {
        }

        public override string GetColor()
        {
            return "#FFFFFF";
        }
    }
}
