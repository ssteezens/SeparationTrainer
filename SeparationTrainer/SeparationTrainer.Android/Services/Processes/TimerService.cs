using Android;
using Android.App;
using Android.Content;
using Android.Graphics;
using Android.OS;
using Android.Support.V4.App;
using AndroidX.Core.App;
using AndroidX.Core.Content;
using SeparationTrainer.Extensions;
using System;
using System.Timers;
using Android.Content.PM;
using Xamarin.Forms;
using Application = Android.App.Application;

namespace SeparationTrainer.Droid.Services.Processes
{
    [Service (Label = "TimerService")]
    public class TimerService : Service
    {
        public const string ChannelId = "default";
        public const string TitleKey = "title";
        public const string MessageKey = "message";
        public const string BroadcastMessageKey = "broadcast_message";

        private bool _serviceStarted;
        private int _lastNotificationSecond = 0;

        #region Service Stuff

        /// <summary>
        ///     Not needed for our purposes
        /// </summary>
        /// <param name="intent"></param>
        /// <returns></returns>
        public override IBinder OnBind(Intent intent)
        {
            return null;
        }

        /// <summary>
        ///     Handles creating the service.
        /// </summary>
        public override void OnCreate()
        {
            // service already started
            if (_serviceStarted)
                return;

            _serviceStarted = true;

            ElapsedTime = TimeSpan.MinValue;
            TimerStart = DateTime.MinValue;
            StopWatchTimer = new Timer(100) { Enabled = false };
            StopWatchTimer.Elapsed += OnStopWatchTimerOnElapsed;
            StartStopTimer();

            base.OnCreate();

            if (!_channelInitialized)
                CreateNotificationChannel();

            var notification = ShowNotification("Activity Started", "hello", 0);
            RegisterService(0, notification);
        }
        
        /// <summary>
        ///     Handles tearing down the service.
        /// </summary>
        public override void OnDestroy()
        {
            base.OnDestroy();
            StartStopTimer();

            if (Build.VERSION.SdkInt >= BuildVersionCodes.N)
            {
                StopForeground(StopForegroundFlags.Remove);
            }
            else
            {
                StopForeground(true);
            }

            StopSelf();

            StopWatchTimer.Elapsed -= OnStopWatchTimerOnElapsed;
            StopWatchTimer = null;
            StopWatchIsRunning = false;
            _serviceStarted = false;
            ClearAllNotifications();
        }

        /// <summary>
        ///     Registers the service.
        /// </summary>
        /// <param name="notificationId"> The id of the notification that will be showing while the service is active. </param>
        /// <param name="notification"> The notification to show while the service is active. </param>
        public void RegisterService(int notificationId, Notification notification)
        {
            StartForeground(notificationId, notification);
        }

        #endregion

        #region Timer Logic

        public void StartStopTimer()
        {
            // set the timer start if not set
            if (TimerStart == DateTime.MinValue)
                TimerStart = DateTime.Now;

            StopWatchIsRunning = !StopWatchIsRunning;
            StopWatchTimer.Enabled = StopWatchIsRunning;
        }

        private void OnStopWatchTimerOnElapsed(object sender, ElapsedEventArgs e)
        {
            ElapsedTime = e.SignalTime - TimerStart;

            // update notification every second
            if ((int)ElapsedTime.TotalSeconds > _lastNotificationSecond)
            {
                var timerText = ElapsedTime.ToShortStopwatchForm();

                // update notification showing elapsed time
                ShowNotification("Activity Started", timerText, 0);
                // send message that time is updated
                _lastNotificationSecond = (int)ElapsedTime.TotalSeconds;
            }
        }

        public Timer StopWatchTimer { get; set; }

        public TimeSpan ElapsedTime { get; set; } = TimeSpan.MinValue;

        public DateTime TimerStart { get; set; } = DateTime.MinValue;

        public bool StopWatchIsRunning { get; set; }

        #endregion

        #region Notification stuff
        
        const string _channelId = "default";
        const string _channelName = "Default";
        const string _channelDescription = "The default channel for notifications.";

        private bool _channelInitialized;
        private int _messageId = 0;
        private int _pendingIntentId = 0;
        private NotificationManager _manager;

        public Notification ShowNotification(string title, string message, int? messageId)
        {
            var intent = new Intent(Application.Context, typeof(MainActivity));
            intent.PutExtra(TitleKey, title);
            intent.PutExtra(MessageKey, message);

            var pendingIntentFlags = (Build.VERSION.SdkInt >= BuildVersionCodes.S)
                ? PendingIntentFlags.UpdateCurrent | PendingIntentFlags.Mutable
                : PendingIntentFlags.UpdateCurrent;
            var pendingIntent = PendingIntent.GetActivity(Application.Context, _pendingIntentId++, intent, pendingIntentFlags);

            var builder = new NotificationCompat.Builder(Application.Context, _channelId)
                .SetContentIntent(pendingIntent)
                .SetContentTitle(title)
                .SetContentText(message)
                .SetLargeIcon(
                    BitmapFactory.DecodeResource(Application.Context.Resources, Resource.Drawable.notification_large))
                .SetSmallIcon(Resource.Drawable.notification_small)
                .SetPriority((int)NotificationPriority.Low)
                .SetSound(null)
                .SetDefaults((int)NotificationDefaults.Vibrate);

            var notification = builder.Build();
            var id = messageId ?? _messageId;

            var hasPermission = Build.VERSION.SdkInt < BuildVersionCodes.Tiramisu 
                                || Xamarin.Essentials.Platform.AppContext.CheckSelfPermission(Manifest.Permission.PostNotifications) == Permission.Granted;
            if (hasPermission)
            {
                _manager.Notify(id, notification);
            }

            return notification;
        }

        private void CreateNotificationChannel()
        {
            _manager = (NotificationManager)Application.Context.GetSystemService(Context.NotificationService);

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

        #endregion
    }
}
