using System;
using AvansDevOps.Domain.Models;
using AvansDevOps.Domain.Notifications;
using AvansDevOps.Domain.Observer;

namespace AvansDevOps.Infrastructure
{
    // ============================================================
    // OBSERVER PATTERN — concrete observers in Infrastructure-laag
    // Doel: De Application Core kent alleen IObserver. Concrete
    // kanaal-implementaties (e-mail, Slack) zitten in Infrastructure
    // zodat de afhankelijkheidsregel van de Onion Architecture
    // gerespecteerd wordt.
    // OO-principe: Open/Closed Principle — nieuw kanaal = nieuwe klasse,
    // geen enkele domeinklasse hoeft aangepast te worden.
    // ============================================================

    // Observer die notificaties verstuurt via e-mail
    public class EmailNotificationSender : IObserver
    {
        private string _recipient;
        private INotificationChannel _emailChannel;

        public EmailNotificationSender(string recipient, INotificationChannel emailChannel)
        {
            _recipient = recipient;
            _emailChannel = emailChannel;
        }

        public void Update(string eventType, BacklogItem item)
        {
            string message = "[EMAIL] Event '" + eventType + "' voor backlog item '" + item.Name + "'.";
            _emailChannel.Send(message, _recipient);
        }
    }

    // Observer die notificaties verstuurt via Slack
    public class SlackNotificationSender : IObserver
    {
        private string _recipient;
        private INotificationChannel _slackChannel;

        public SlackNotificationSender(string recipient, INotificationChannel slackChannel)
        {
            _recipient = recipient;
            _slackChannel = slackChannel;
        }

        public void Update(string eventType, BacklogItem item)
        {
            string message = "[SLACK] Event '" + eventType + "' voor backlog item '" + item.Name + "'.";
            _slackChannel.Send(message, _recipient);
        }
    }
}
