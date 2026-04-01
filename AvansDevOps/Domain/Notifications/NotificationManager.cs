using System.Collections.Generic;
using AvansDevOps.Domain.Models;
using AvansDevOps.Domain.Observer;

namespace AvansDevOps.Domain.Notifications
{
    public class NotificationManager
    {
        private List<INotificationChannel> _channels = new List<INotificationChannel>();
        private List<IObserver> _observers = new List<IObserver>();

        public void AddChannel(INotificationChannel channel)
        {
            _channels.Add(channel);
        }

        public void Subscribe(IObserver observer)
        {
            _observers.Add(observer);
        }

        public void Unsubscribe(IObserver observer)
        {
            _observers.Remove(observer);
        }

        public void Notify(string message, string recipient)
        {
            foreach (INotificationChannel channel in _channels)
                channel.Send(message, recipient);
        }

        public void NotifyObservers(string eventType, BacklogItem item)
        {
            foreach (IObserver observer in _observers)
                observer.Update(eventType, item);
        }
    }
}