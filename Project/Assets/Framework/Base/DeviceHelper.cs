using System;
using UnityEngine;
//using System.Net.NetworkInformation;
using System.Collections;
using System.Runtime.InteropServices;
using Framework;
using System.Net.NetworkInformation;
using System.Net.Sockets;
using System.Globalization;

namespace Framework
{

    public class DeviceHelper
    {
        public static string GetDeviceId()
        {
            string testDevId = PlayerPrefs.GetString("TestDevice",string.Empty);
            if (!string.IsNullOrEmpty(testDevId) && testDevId.Trim() != "null")
            {
                return testDevId;
            }
            string deviceId = SystemInfo.deviceUniqueIdentifier;

#if UNITY_EDITOR
            return deviceId + "xxy_0902x1";
#else
             return deviceId;
#endif
        }
        public static string GetAppVersion()
        {
            return Application.version;
        }

        public static int GetAppVersionCode()
        {
            //#if UNITY_ANDROID
            //            return PlayerSettings.Android.bundleVersionCode;
            //#elif UNITY_IOS
            //            return int.Parse(PlayerSettings.iOS.buildNumber);
            //#else
            //            return 0;
            //#endif
            return 0;
        }

        public static long GetTotalMemory()
        {
            return SystemInfo.systemMemorySize;
        }

        public static string GetDeviceModel()
        {
            return SystemInfo.deviceModel;
        }

        // TODO
        public static string GetRegion()
        {
            return Application.systemLanguage.ToString();
        }

        public static string GetCountry()
        {
            return RegionInfo.CurrentRegion.TwoLetterISORegionName;
        }

        public static string GetOSName()
        {
            return SystemInfo.operatingSystem;
        }

        public static string GetOSVersion()
        {
            return SystemInfo.operatingSystem;
        }

        public static string GetCPU()
        {
            return SystemInfo.processorType;
        }

        // TODO
        public static string GetTimezone()
        {
            try
            {
                return TimeZoneInfo.Local.StandardName;
            }
            catch
            {
                return "UNKNOWN";
            }
        }

        public static string GetPlatformString()
        {
#if UNITY_IOS
            if (SystemInfo.deviceModel.Contains("iPad"))
            {
                return "Ipad";
            }
            else
            {
                return "Iphone";
            }
#elif UNITY_ANDROID
            return "Google";
#elif UNITY_FACEBOOK
        return "GameRoom";
#elif UNITY_WEBGL
        return "Facebook";
#else
        return "unity_editor";
#endif

        }

        public enum NetworkStatus
        {
            Unknown = 0,
            NoNetwork = 1,
            Cellular = 2,
            Wifi = 3
        };

        public static NetworkStatus GetNetworkStatus()
        {
            switch (Application.internetReachability)
            {
                case NetworkReachability.ReachableViaCarrierDataNetwork:
                    return NetworkStatus.Cellular;
                case NetworkReachability.ReachableViaLocalAreaNetwork:
                    return NetworkStatus.Wifi;
                case NetworkReachability.NotReachable:
                    return NetworkStatus.NoNetwork;
                default:
                    return NetworkStatus.Unknown;
            }
        }

        public static string GetResolution()
        {
            return Screen.currentResolution.ToString();
        }

        public enum ADDRESSFAM
        {
            IPv4, IPv6
        }

        /// <summary>
        /// 获取本机IP
        /// </summary>
        /// <param name="Addfam">要获取的IP类型</param>
        /// <returns></returns>
        public static string GetIP(ADDRESSFAM Addfam)
        {
            if (Addfam == ADDRESSFAM.IPv6 && !Socket.OSSupportsIPv6)
            {
                return null;
            }

            string output = "";

            foreach (NetworkInterface item in NetworkInterface.GetAllNetworkInterfaces())
            {
#if UNITY_EDITOR_WIN || UNITY_STANDALONE_WIN
            NetworkInterfaceType _type1 = NetworkInterfaceType.Wireless80211;
            NetworkInterfaceType _type2 = NetworkInterfaceType.Ethernet;

            if ((item.NetworkInterfaceType == _type1 || item.NetworkInterfaceType == _type2) && item.OperationalStatus == OperationalStatus.Up)
#endif
                {
                    foreach (UnicastIPAddressInformation ip in item.GetIPProperties().UnicastAddresses)
                    {
                        //IPv4
                        if (Addfam == ADDRESSFAM.IPv4)
                        {
                            if (ip.Address.AddressFamily == AddressFamily.InterNetwork)
                            {
                                output = ip.Address.ToString();
                                //Debug.Log("IP:" + output);
                            }
                        }

                        //IPv6
                        else if (Addfam == ADDRESSFAM.IPv6)
                        {
                            if (ip.Address.AddressFamily == AddressFamily.InterNetworkV6)
                            {
                                output = ip.Address.ToString();
                            }
                        }
                    }
                }
            }
            return output;
        }

    }





    public class DeviceInfo
    {
        public string appname;
        public string packagename;
        public int appvercode;
        public string appver;
        public string deviceId;
        public string devicecountry;
        public string timestamp;
        public string netstatus;
        public string syslanguage;
        public string sysver;
        public string devicemodel;
        public string timezone;
        public string unitysdkver;



        public DeviceInfo()
        {
            appname = GameConfig.Inst.Appid;
            packagename = GameConfig.Inst.Appid;
            appvercode = DeviceHelper.GetAppVersionCode();
            appver = DeviceHelper.GetAppVersion();
            deviceId = DeviceHelper.GetDeviceId();
            devicecountry = DeviceHelper.GetCountry();
            syslanguage = Application.systemLanguage.ToString();
            netstatus = DeviceHelper.GetNetworkStatus().ToString();
            sysver = SystemInfo.operatingSystem;
            devicemodel = SystemInfo.deviceModel;

            timestamp = SystemClock.Now.ToString();
            timezone = TimeZone.CurrentTimeZone.StandardName;
            unitysdkver = Application.unityVersion;
        }
        private static DeviceInfo _deviceInfo;
        public static DeviceInfo deviceInfo
        {
            get
            {
                if (_deviceInfo == null)
                    _deviceInfo = new DeviceInfo();

                return _deviceInfo;
            }
        }
        public override string ToString()
        {
            try
            {

                string jsonStr = Newtonsoft.Json.JsonConvert.SerializeObject(this);
                return jsonStr;
            }
            catch
            {
                return "{}";
            }
        }
    }


}


