using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Notifications.Android;
using System;

public class NotificationManager : MonoBehaviour
{
    [SerializeField] private List<Booster> boosters = new List<Booster>();
    [SerializeField] private float boosterExpirationReminderPercentage = 0.9f;
    [SerializeField] private CustomDate reminderInterval;
    [SerializeField] private List<string> notificationIDs = new List<string>();
    private AndroidNotificationChannel notificationChannel;
    private const string notificationChannelName = "notificationChannel";
    private List<int> boosterNotificationIds = new List<int>();
    private void Start()
    {
        notificationChannel = new AndroidNotificationChannel(notificationChannelName, "GenericNotifications", "GeneralPurposeNotifications", Importance.Default);

        AndroidNotificationCenter.RegisterNotificationChannel(notificationChannel);

        for (int i = 0; i < notificationIDs.Count; i++)
        {
            string rememberedNotification = PlayerPrefs.GetString(notificationIDs[i]);
            if(rememberedNotification != "")
            {
                int notificationID = int.Parse(rememberedNotification);

                if(i > 0)
                {
                    boosterNotificationIds.Add(notificationID);
                }
                try
                {
                    if(AndroidNotificationCenter.CheckScheduledNotificationStatus(notificationID) == NotificationStatus.Scheduled)
                    {
                        AndroidNotificationCenter.CancelNotification(notificationID);
                    }
                }
                catch
                {
                    
                }
            }
        }
    }

    private AndroidNotification ConstructNotification(string title, string content, string smallIcon, string largeIcon, TimeSpan nextTrigger, bool repetitive)
    {
        AndroidNotification notification = new AndroidNotification();

        notification.Title = title;
        notification.Text = content;
        notification.SmallIcon = smallIcon;
        notification.LargeIcon = largeIcon;
        notification.FireTime = DateTime.Now.Add(nextTrigger);
        
        if(repetitive)
        {
            notification.RepeatInterval = nextTrigger;
        }

        return notification;
    }

    private void OnApplicationQuit()
    {
        List<AndroidNotification> notifications = new List<AndroidNotification>();
        DateTime currentTime = DateTime.Now;
        TimeSpan reminderSpan = new TimeSpan(days: reminderInterval.days, hours: reminderInterval.hours, minutes: reminderInterval.minutes, seconds: reminderInterval.seconds, milliseconds: reminderInterval.milliseconds);
        notifications.Add(ConstructNotification("Shift Awaits", "Come Back and Compete in the Battle with Time", "small_icon", "large_icon", reminderSpan, true));
        /*
        for (int i = 0; i < boosters.Count; i++)
        {
            Booster currentBooster = boosters[i];
            currentBooster.CheckDate(currentTime, false);
            //TimeSpan difference = boosters[i].;
            AndroidNotification boosterNotification = ConstructNotification("", "", "", "", );
        }*/

        for (int i = 0; i < notifications.Count; i++)
        {
            int nID = AndroidNotificationCenter.SendNotification(notifications[i], notificationChannelName);
            PlayerPrefs.SetString(notificationIDs[i], nID.ToString());
        }
    }
}
