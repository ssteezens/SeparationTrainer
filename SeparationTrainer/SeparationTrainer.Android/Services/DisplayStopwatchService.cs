using Android.App;
using Android.Content;
using Android.Graphics;
using Android.OS;
using Android.Support.V4.App;
using Android.Support.V4.Content;
using System;
using AndroidApp = Android.App.Application;

namespace SeparationTrainer.Droid.Services
{
    [Service]
    public class DisplayStopwatchService : Service
    {
        public const string ChannelId = "default";
        public const string TitleKey = "title";
        public const string MessageKey = "message";
        public const string BroadcastMessageKey = "broadcast_message";

        private bool _isStarted;
        private int _pendingIntentId = 0;
        private Handler _handler;
        private Action _runnable;

        public override void OnCreate()
        {
            base.OnCreate();

            _handler = new Handler();
            _runnable = new Action(() =>
            {
                var intent = new Intent(MessageKey);
                intent.PutExtra(BroadcastMessageKey, "some message");

                LocalBroadcastManager.GetInstance(this).SendBroadcast(intent);

                _handler.PostDelayed(_runnable, 1000);
            });
        }

        public override StartCommandResult OnStartCommand(Intent intent, StartCommandFlags flags, int startId)
        {
            if (intent.Action.Equals("START_SERVICE"))
            {
                if (!_isStarted)
                {
                    RegisterForegroundService();
                    _handler.PostDelayed(_runnable, 1000);
                    _isStarted = true;
                }
            }
            else if (intent.Action.Equals("STOP_SERVICE"))
            {
                StopForeground(true);
                StopSelf();
                _isStarted = false;
            }

            return StartCommandResult.Sticky;
        }

        public override void OnDestroy()
        {

            _isStarted = false;
            base.OnDestroy();
        }

        public void RegisterForegroundService()
        {
            var intent = new Intent(AndroidApp.Context, typeof(MainActivity));
            intent.PutExtra(TitleKey, "title");
            intent.PutExtra(MessageKey, "message");

            var pendingIntent = PendingIntent.GetActivity(AndroidApp.Context, _pendingIntentId++, intent, PendingIntentFlags.UpdateCurrent);

            var builder = new NotificationCompat.Builder(AndroidApp.Context, ChannelId)
                .SetContentIntent(pendingIntent)
                .SetContentTitle("Activity Started")
                .SetContentText("message")
                .SetLargeIcon(
                    BitmapFactory.DecodeResource(AndroidApp.Context.Resources, Resource.Drawable.xamarin_logo))
                .SetSmallIcon(Resource.Drawable.xamarin_logo)
                .SetPriority((int)NotificationPriority.Low)
                .SetSound(null)
                .SetDefaults((int)NotificationDefaults.Vibrate);

            var notification = builder.Build();

            StartForeground(0, notification);
        }

        public override IBinder OnBind(Intent intent)
        {
            // Return null because this is a pure started service. A hybrid service would return a binder that would
            // allow access to the GetFormattedStamp() method.
            return null;
        }
    }
}
