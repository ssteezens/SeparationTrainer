using System;

namespace SeparationTrainer.Services.Notifications
{
    public interface INotificationManager
    {
        event EventHandler NotificationReceived;
        void Initialize();
        void SendNotification(string title, string message, DateTime? notifyTime = null, int? messageId = null);
        void ReceiveNotification(string title, string message);
        void ClearNotification(int messageId);
        void ClearAllNotifications();
    }
}
