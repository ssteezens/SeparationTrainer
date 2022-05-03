﻿using Android.App;
using Android.Content;
using Android.Graphics;
using Android.OS;
using Android.Support.V4.App;
using SeparationTrainer.Services.Notifications;
using System;
using Xamarin.Forms;
using AndroidApp = Android.App.Application;

[assembly: Dependency(typeof(SeparationTrainer.Droid.Services.Notifications.AndroidNotificationManager))]
namespace SeparationTrainer.Droid.Services.Notifications
{
    public class AndroidNotificationManager : INotificationManager
    {
        const string _channelId = "default";
        const string _channelName = "Default";
        const string _channelDescription = "The default channel for notifications.";

        public const string TitleKey = "title";
        public const string MessageKey = "message";

        private bool _channelInitialized;
        private int _messageId = 0;
        private int _downloadMessageId = 0;
        private int _pendingIntentId = 0;
        private int _pendingDownloadIntentId = int.MaxValue;
        private NotificationManager _manager;

        public event EventHandler NotificationReceived;

        public static AndroidNotificationManager Instance { get; private set; }

        public void Initialize()
        {
            CreateNotificationChannel();
            Instance = this;
        }

        public void SendNotification(string title, string message, DateTime? notifyTime = null, int? messageId = null, NotificationType notificationType = NotificationType.Message)
        {
            if(!_channelInitialized)
                CreateNotificationChannel();

            if (notificationType == NotificationType.Message)
                Show(title, message, messageId);
            else
                ShowDownload(title, message, messageId);
        }

        public void ReceiveNotification(string title, string message)
        {
            var args = new NotificationEventArgs()
            {
                Title = title,
                Message = message,
            };
            NotificationReceived?.Invoke(null, args);
        }

        public void ClearNotification(int messageId)
        {
            if (!_channelInitialized)
                return;

            _manager.Cancel(messageId);
        }

        public void ClearAllNotifications()
        {
            if (!_channelInitialized)
                return;

            _manager.CancelAll();
        }

        public void Show(string title, string message, int? messageId)
        {
            var intent = new Intent(AndroidApp.Context, typeof(MainActivity));
            intent.PutExtra(TitleKey, title);
            intent.PutExtra(MessageKey, message);

            var pendingIntent = PendingIntent.GetActivity(AndroidApp.Context, _pendingIntentId++, intent, PendingIntentFlags.UpdateCurrent);

            var builder = new NotificationCompat.Builder(AndroidApp.Context, _channelId)
                .SetContentIntent(pendingIntent)
                .SetContentTitle(title)
                .SetContentText(message)
                .SetLargeIcon(
                    BitmapFactory.DecodeResource(AndroidApp.Context.Resources, Resource.Drawable.notification_large))
                .SetSmallIcon(Resource.Drawable.notification_small)
                .SetPriority((int)NotificationPriority.Low)
                .SetSound(null)
                .SetDefaults((int)NotificationDefaults.Vibrate);

            var notification = builder.Build();
            var id = messageId ?? _messageId;
            _manager.Notify(id, notification);
        }

        public void ShowDownload(string title, string message, int? messageId)
        {
            var intent = new Intent(DownloadManager.ActionViewDownloads);
            intent.PutExtra(TitleKey, title);
            intent.PutExtra(MessageKey, message);

            var pendingIntent = PendingIntent.GetActivity(AndroidApp.Context, _pendingDownloadIntentId--, intent, PendingIntentFlags.OneShot);

            var builder = new NotificationCompat.Builder(AndroidApp.Context, _channelId)
                .SetContentIntent(pendingIntent)
                .SetContentTitle(title)
                .SetContentText(message)
                .SetLargeIcon(
                    BitmapFactory.DecodeResource(AndroidApp.Context.Resources, Resource.Drawable.notification_large))
                .SetSmallIcon(Resource.Drawable.notification_small)
                .SetPriority((int)NotificationPriority.Low)
                .SetSound(null)
                .SetDefaults((int)NotificationDefaults.Vibrate);

            var notification = builder.Build();
            var id = messageId ?? _downloadMessageId;
            _manager.Notify(id, notification);
        }

        private void CreateNotificationChannel()
        {
            _manager = (NotificationManager)AndroidApp.Context.GetSystemService(Context.NotificationService);

            if (Build.VERSION.SdkInt >= BuildVersionCodes.O)
            {
                var channelNameJava = new Java.Lang.String(_channelName);
                var channel = new NotificationChannel(_channelId, channelNameJava, NotificationImportance.Low)
                {
                    Description = _channelDescription,
                    Name = "Separation Trainer"
                };
                _manager.CreateNotificationChannel(channel);
            }

            _channelInitialized = true;
        }
    }
}
