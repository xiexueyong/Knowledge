using System.Net.Mime;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
#if UNITY_ANDROID
// using Unity.Notifications.Android;
#elif UNITY_IOS
using Unity.Notifications.iOS;
#endif

public class NotifyManager : D_MonoSingleton<NotifyManager>
{
    #if UNITY_ANDROID
    // public void schaduleAndroidNotifications()
    // {
    //     string notifyIds = "";
    //     var startDate = DateTime.Now;
    //       //测试通知
    //     // int testNotifyId = schaduleAndroidNotification(
    //     //     "testNotify",
    //     //     "test notify contetn",
    //     //     (int)(startDate.AddSeconds(10) - startDate).TotalSeconds
    //     // );
    //     // notifyIds += testNotifyId;
    //
    //     for(int i = 1;i<7;i++){
    //         DateTime nextDate = startDate.AddDays(i);
    //         DateTime toDate = new DateTime(nextDate.Year,nextDate.Month,nextDate.Day,10,0,0);
    //         int nid = schaduleAndroidNotification(
    //             "企鹅叫你来南极",
    //             "和企鹅一起玩消除",
    //             (int)(toDate - startDate).TotalSeconds
    //         );
    //         notifyIds += "," + nid;
    //     }
    //     PlayerPrefs.SetString("PoloarBlast_Notify",notifyIds);
    //     CreateAndroidNotifChannel();
    // }
    // void CreateAndroidNotifChannel()
    // {
    //     var c = new AndroidNotificationChannel()
    //     {
    //         Id = "polarblast",
    //         Name = "Polar Blast",
    //         Importance = Importance.Default,
    //         Description ="Reminds the player to play the game",
    //     };
    //     AndroidNotificationCenter.RegisterNotificationChannel(c);
    // }
    //
    // public void cancleAndroidSchaduleAndroidNotification()
    // {
    //     string a = PlayerPrefs.GetString("PoloarBlast_Notify",string.Empty);
    //     PlayerPrefs.DeleteKey("PoloarBlast_Notify");
    //     if (!string.IsNullOrEmpty(a))
    //     {
    //         string[] idStrArr = a.Split(',');
    //         for (int i = 0; i < idStrArr.Length; i++)
    //         {
    //             int id;
    //             bool r = int.TryParse(idStrArr[i], out id);
    //             if(r)
    //                 AndroidNotificationCenter.CancelNotification(id);
    //         }
    //     }
    // }
    //
    // private int schaduleAndroidNotification(string title,string content,int seconds){
    //     var notification = new AndroidNotification();
    //     notification.Title = title;
    //     notification.Text = content;
    //     notification.FireTime = System.DateTime.Now.AddSeconds(seconds);
    //     Debug.Log("-------Notify Android date: "+notification.FireTime.ToString());
    //     return AndroidNotificationCenter.SendNotification(notification, "polarblast");
    // }






//IOS========================================================================================
#elif UNITY_IOS
    public void schaduleIosNotifications(){
        if (NotifyGranted)
        {
            schaduleAndroidNotificationsActual();
        }
    }
    bool NotifyGranted;
    IEnumerator RequestAuthorization()
    {
        var authorizationOption = AuthorizationOption.Alert | AuthorizationOption.Badge;
        using (var req = new AuthorizationRequest(authorizationOption, true))
        {
            while (!req.IsFinished)
            {
                yield return null;
            };

            NotifyGranted = req.Granted;

            string res = "\n RequestAuthorization:";
            res += "\n finished: " + req.IsFinished;
            res += "\n granted :  " + req.Granted;
            res += "\n error:  " + req.Error;
            res += "\n deviceToken:  " + req.DeviceToken;
            Debug.Log(res);
        }
    }
    
    
    public void cancleIOsSchaduleAndroidNotification()
    {
        iOSNotificationCenter.RemoveAllDeliveredNotifications();
        iOSNotificationCenter.RemoveAllScheduledNotifications();
        StartCoroutine(RequestAuthorization());
    }
public void schaduleAndroidNotificationsActual(){
        var startDate = DateTime.Now;
        //测试通知
        // schaduleIosNotification(
        //     "testNotify",
        //     "test notify contetn",
        //     (startDate.AddMinutes(1) - startDate)
        // );


        for(int i = 1;i<7;i++){
            DateTime nextDate = startDate.AddDays(i);
            DateTime toDate = new DateTime(nextDate.Year,nextDate.Month,nextDate.Day,10,0,0);
            schaduleIosNotification(
                "企鹅叫你来南极",
                "和企鹅一起玩消除",
                (toDate - startDate)
            );
        }
    }

    private void schaduleIosNotification(string title,string content,TimeSpan timeSpan){
        var timeTrigger = new iOSNotificationTimeIntervalTrigger()
        {
            TimeInterval = timeSpan,
            Repeats = false
        };
        var notification = new iOSNotification()
        {
            // You can specify a custom identifier which can be used to manage the notification later.
            // If you don't provide one, a unique string will be generated automatically.
            //Identifier = "_notification_01",
            Title = title,
            Body = content,
            //Subtitle = "This is a subtitle, something, something important...",
            ShowInForeground = true,
            ForegroundPresentationOption = (PresentationOption.Alert | PresentationOption.Sound),
            //CategoryIdentifier = "category_a",
            //ThreadIdentifier = "thread1",
            Trigger = timeTrigger,
        };
        iOSNotificationCenter.ScheduleNotification(notification);
    }
    #endif
}
