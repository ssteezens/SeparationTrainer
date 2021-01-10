using System;

namespace SeparationTrainer.Services.Notifications
{
    public class NotificationEventArgs : EventArgs
    {
        public string Title { get; set; }
        public string Message { get; set; }
    }
}