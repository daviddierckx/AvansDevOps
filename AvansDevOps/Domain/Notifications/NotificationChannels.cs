using System;

namespace AvansDevOps.Domain.Notifications
{
    // ============================================================
    // ADAPTER PATTERN
    // Doel: Zet de interface van bestaande klassen (Email, Slack)
    // om naar een uniforme INotificationChannel interface,
    // zodat de NotificationManager niet afhankelijk is van
    // concrete implementaties.
    // OO-principe: Dependency Inversion Principle — depend on
    // abstractions, not on concretions.
    // ============================================================

    // Adapter Pattern - target interface
    public interface INotificationChannel
    {
        void Send(string message, string recipient);
    }

    // Adapter Pattern - concrete adapter voor e-mail
    public class EmailAdapter : INotificationChannel
    {
        private string _smtpServer;
        private int _port;
        private string _username;
        private string _password;

        public EmailAdapter(string smtpServer, int port, string username, string password)
        {
            _smtpServer = smtpServer;
            _port = port;
            _username = username;
            _password = password;
        }

        // Adapter Pattern - adapteert Send() naar SMTP logica
        public void Send(string message, string recipient)
        {
            Connect();
            Console.WriteLine("[EMAIL] Aan: " + recipient + " | " + message);
            Disconnect();
        }

        public void Connect()
        {
            Console.WriteLine("[EMAIL] Verbinding met " + _smtpServer + ":" + _port);
        }

        public void Disconnect()
        {
            Console.WriteLine("[EMAIL] Verbinding verbroken.");
        }
    }

    // Adapter Pattern - concrete adapter voor Slack
    public class SlackAdapter : INotificationChannel
    {
        private string _webhookUrl;
        private string _channelId;
        private string _botToken;

        public SlackAdapter(string webhookUrl, string channelId, string botToken)
        {
            _webhookUrl = webhookUrl;
            _channelId = channelId;
            _botToken = botToken;
        }

        // Adapter Pattern - adapteert Send() naar Slack webhook logica
        public void Send(string message, string recipient)
        {
            Connect();
            Console.WriteLine("[SLACK] Kanaal: " + _channelId + " | Aan: " + recipient + " | " + message);
            Disconnect();
        }

        public void Connect()
        {
            Console.WriteLine("[SLACK] Verbinding met " + _webhookUrl);
        }

        public void Disconnect()
        {
            Console.WriteLine("[SLACK] Verbinding verbroken.");
        }

        public void ValidateToken()
        {
            Console.WriteLine("[SLACK] Token gevalideerd.");
        }
    }
}