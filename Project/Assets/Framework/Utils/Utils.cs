using System;
using System.Collections.Generic;
using UnityEngine;
using Newtonsoft.Json;
using UnityEngine.UI;


#if UNITY_IOS
using UnityEngine.iOS;
#endif


namespace Framework.Utils
{
    public static class EnumDef
    {
        public enum Currency
        {
            Ruby = 0,
            Cash = 1,
            Coins = 2
        }
    }

    public static class Utils
    {

        public static string Validate(string text)
        {
            //https://msdn.microsoft.com/en-us/library/20bw873z(v=vs.110).aspx
            text = RemoveEmoji(text, @"\p{C}");
            return text;
        }

        static string ValidateEx(string text)
        {
            //https://en.wikipedia.org/wiki/Emoji#Emoji_in_the_Unicode_standard
            // 635 of the 766 codepoints in the Miscellaneous Symbols and Pictographs block are considered emoji.
            string pattern = @"[^\u1F300-\u1F5FF]";
            text = RemoveEmoji(text, pattern);

            // All of the 15 codepoints in the Supplemental Symbols and Pictographs block are considered emoji.
            pattern = @"[^\u1F910-\u1F918\u1F980-\u1F984\u1F9C0]";
            text = RemoveEmoji(text, pattern);

            // All of the 80 codepoints in the Emoticons block are considered emoji.
            //      pattern = @"[^\u1F60-\u1F64]";
            //      text = RemoveEmoji (text, pattern);

            // 87 of the 98 codepoints in the Transport and Map Symbols block are considered emoji.
            //      pattern = @"[^\u1F68-\u1F6F]";
            //      text = RemoveEmoji (text, pattern);

            // 77 of the 256 codepoints in the Miscellaneous Symbols block are considered emoji.
            //      pattern = @"[^\u260-\u26F]";
            //      text = RemoveEmoji (text, pattern);

            // 77 of the 256 codepoints in the Miscellaneous Symbols block are considered emoji.
            //      pattern = @"[^\u270-\u27B]";
            //      text = RemoveEmoji (text, pattern);

            return text;
        }

        static string RemoveEmoji(string text, string pattern)
        {
            return System.Text.RegularExpressions.Regex.Replace(text, pattern, string.Empty);
        }



        public static long TotalSeconds()
        {
            TimeSpan ts = (DateTime.UtcNow - new DateTime(1970, 1, 1, 0, 0, 0, DateTimeKind.Utc));
            return Convert.ToInt64(ts.TotalSeconds);
        }

        public static DateTime ParseTime(long seconds)
        {
            DateTime dt = new DateTime(1970, 1, 1, 0, 0, 0, DateTimeKind.Utc);
            return dt.AddSeconds(seconds);
        }

        public static DateTime ParseTimeMilliSecond(long milliseconds)
        {
            DateTime dt = new DateTime(1970, 1, 1, 0, 0, 0, DateTimeKind.Utc);
            return dt.AddMilliseconds(milliseconds);
        }

        public static DateTime ParseTimeFromNow(long countdown)
        {
            return DateTime.UtcNow.AddSeconds(countdown);
        }

        public static int GetDayBySeconds(long seconds)
        {
            return (int) Mathf.Floor(seconds / (3600 * 24));
        }

        public static bool IsSameDay(long seconds1, long seconds2)
        {
            return GetDayInterval(seconds1, seconds2) == 0;
        }

        public static int GetDayInterval(long seconds1, long seconds2)
        {
            int day1 = Utils.GetDayBySeconds(seconds1);
            int day2 = Utils.GetDayBySeconds(seconds2);
            return Mathf.Abs(day1 - day2);
        }

        public static List<int> GetRandomList(int beginInt, int endInt)
        {
            List<int> arr = new List<int>();
            for (int i = 0; i < endInt - beginInt + 1; i++) {
                arr.Add(beginInt + i);
            }

            for (int i = beginInt; i <= endInt; i++) {
                int index = UnityEngine.Random.Range(i, endInt);
                int tmp = arr[i];
                arr[i] = arr[index];
                arr[index] = tmp;
            }

            return arr;
        }

        /// <summary>
        /// Array to string. split with ,
        /// </summary>
        /// <returns>formatted string.</returns>
        /// <param name="array">Array.</param>
        /// <typeparam name="T">The 1st type parameter.</typeparam>
        public static string ArrayToString<T>(T[] array)
        {
            System.Text.StringBuilder sb = new System.Text.StringBuilder();
            if (array != null && array.Length > 0) {
                for (int i = 0; i < array.Length; i++) {
                    if (i != 0) {
                        sb.Append(',');
                    }

                    sb.Append(array[i]);
                }
            }

            return sb.ToString();
        }


        public static string GetTimeString(string format, int seconds)
        {
            string label = format;
            int ms = seconds * 1000;
            int s = seconds;
            int m = s / 60;
            int h = m / 60;
            int d = h / 24;

            string t = "";
            //处理天
            if (label.Contains("%dd")) {
                t = d >= 10 ? d.ToString() : ("0" + d.ToString());
                label = label.Replace("%dd", t);
                h = h % 24;
            }
            else if (label.Contains("%d")) {
                label = label.Replace("%d", d.ToString());
                h = h % 24;
            }

            //处理小时
            if (label.Contains("%hh")) {
                t = h >= 10 ? h.ToString() : ("0" + h.ToString());
                label = label.Replace("%hh", t);
                m = m % 60;
            }
            else if (label.Contains("%h")) {
                label = label.Replace("%h", h.ToString());
                m = m % 60;
            }

            //处理分
            if (label.Contains("%mm")) {
                t = m >= 10 ? m.ToString() : ("0" + m.ToString());
                label = label.Replace("%mm", t);
                s = s % 60;
            }
            else if (label.Contains("%m")) {
                label = label.Replace("%m", m.ToString());
                s = s % 60;
            }

            //处理秒
            if (label.Contains("%ss")) {
                t = s >= 10 ? s.ToString() : ("0" + s.ToString());
                label = label.Replace("%ss", t);
                ms = ms % 1000;
            }
            else if (label.Contains("%s")) {
                label = label.Replace("%s", s.ToString());
                ms = ms % 1000;
            }

            //处理毫秒
            if (label.Contains("ms")) {
                t = ms.ToString();
                label = label.Replace("%ms", t);
            }

            return label;
        }

        // 把一个记录的时间戳转换成日期显示(毫秒)
        public static string GetTimeStampDateString(double timeStamp, string format = "yyyy-MM-dd HH:mm:ss")
        {
            var offset = TimeZone.CurrentTimeZone.GetUtcOffset(DateTime.Now);
            var dt = new DateTime(1970, 1, 1, 0, 0, 0, DateTimeKind.Utc).Add(offset).AddMilliseconds(timeStamp);

            return dt.ToString(format);
        }

        public static T ArrayFind<T>(T[] array, Predicate<T> condition)
        {
            T item = default(T);
            if (array != null && array.Length > 0) {
                for (int i = 0; i < array.Length; i++) {
                    if (condition(array[i])) {
                        item = array[i];
                        break;
                    }
                }
            }

            return item;
        }

        public static int ArrayFindIndex<T>(T[] array, Predicate<T> condition)
        {
            int index = -1;
            if (array != null && array.Length > 0) {
                for (int i = 0; i < array.Length; i++) {
                    if (condition(array[i])) {
                        index = i;
                        break;
                    }
                }
            }

            return index;
        }

        public static T[] ArrayAdd<T>(T[] array, T item)
        {
            List<T> newArray = new List<T>();
            if (array != null) {
                newArray.AddRange(array);
            }

            newArray.Add(item);
            return newArray.ToArray();
        }

        public static int RandomByWeight(Dictionary<int, int> idWeights)
        {
            int weights = 0;
            int id = 0;
            foreach (var kv in idWeights) {
                weights += kv.Value;
                id = kv.Key;
            }

            int pos = UnityEngine.Random.Range(0, weights);
            weights = 0;
            foreach (var kv in idWeights) {
                weights += kv.Value;
                if (pos < weights) {
                    id = kv.Key;
                    break;
                }
            }

            return id;
        }

        public static int RandomByWeight(List<int> weightList)
        {
            int weights = 0;
            int idx = 0;
            for (int i = 0; i < weightList.Count; i++) {
                weights += weightList[i];
            }

            int pos = UnityEngine.Random.Range(0, weights);
            weights = 0;
            for (int i = 0; i < weightList.Count; i++) {
                weights += weightList[i];
                if (pos < weights) {
                    idx = i;
                    break;
                }
            }

            return idx;
        }

        public static int RandomByWeight(int[] weightList)
        {
            int weights = 0;
            int idx = 0;
            for (int i = 0; i < weightList.Length; i++) {
                weights += weightList[i];
            }

            int pos = UnityEngine.Random.Range(0, weights);
            weights = 0;
            for (int i = 0; i < weightList.Length; i++) {
                weights += weightList[i];
                if (pos < weights) {
                    idx = i;
                    break;
                }
            }

            return idx;
        }


        public static void JumpToAnimation(Animator animator, string stateName, float normalizedTime, int layer = -1)
        {
            animator.Play(stateName, layer, normalizedTime);
        }

        public static bool IsChinScreen()
        {
            return IsIphoneX();
        }

		public static bool IsIphoneX()
		{
			bool IsIphoneXDevice = false;
			string modelStr = SystemInfo.deviceModel;
#if UNITY_IOS
			// iPhoneX:"iPhone10,3","iPhone10,6"  iPhoneXR:"iPhone11,8"  iPhoneXS:"iPhone11,2"  iPhoneXS Max:"iPhone11,6"   iPhone11:"iPhone12,5"
			IsIphoneXDevice = modelStr.Equals("iPhone10,3") || modelStr.Equals("iPhone10,6") || modelStr.Equals("iPhone11,8") || modelStr.Equals("iPhone11,2") || modelStr.Equals("iPhone11,6")||modelStr.Equals("iPhone12,5");
            IsIphoneXDevice |= Mathf.Approximately(Screen.height / Screen.width, 1792f / 828f);
#endif
            if (IsIphoneXDevice || Mathf.Abs((float)Screen.height / Screen.width-2436 / 1125f) <= 0.01f) {
				return true;
			}
			return false;
		}

		private static System.Random mRandom;
		public static System.Random Random 
		{
			get {
				if (mRandom == null) {
					mRandom = new System.Random (Utils.GetRandomSeed ());
				} 
				return mRandom;
			}
		}

		//获取随机种子
		public static int GetRandomSeed( )
		{
			byte[] bytes = new byte[4];
			System.Security.Cryptography.RNGCryptoServiceProvider rng = new System.Security.Cryptography.RNGCryptoServiceProvider( );
			rng.GetBytes( bytes );
			return BitConverter.ToInt32(bytes, 0);
		}

		public static void ClearChilds(Transform parent)
		{
			for (int i = 0; i < parent.childCount; i++) {
				Transform ts = parent.GetChild (i);
				GameObject.Destroy (ts.gameObject);
			}
		}

		/// <summary>
		/// 获取组件
		/// </summary>
		/// <returns>The component.</returns>
		/// <param name="go">Go.</param>
		/// <typeparam name="T">The 1st type parameter.</typeparam>
		public static T GetComponent<T>(GameObject go) where T : Component
		{
			T com = go.GetComponent<T> ();
			if (com == null) {
				com = go.AddComponent<T> ();
			}
			return com;
		}

		public static DateTime ConvertTime(string timestring, DateTime defaultTime)
		{
			try {
				DateTime time;
				bool ok = DateTime.TryParse(timestring, out time);
				if (!ok) {
					time = defaultTime;
				}
				return time;
			} catch (Exception) {
				return defaultTime;
			}
		}
			
		/// <summary>
		/// 获取中间点
		/// </summary>
		/// <param name="begin"></param>
		/// <param name="end"></param>
		/// <param name="delta"></param>
		/// <returns></returns>
		public static Vector3 GetMiddlePoint(Vector3 begin, Vector3 end, float delta = 0)
		{
			Vector3 center = Vector3.Lerp(begin, end, 0.5f);
			Vector3 beginEnd = end - begin;
			Vector3 perpendicular = new Vector3(-beginEnd.y, beginEnd.x, 0).normalized;
			Vector3 middle = center + perpendicular * delta;
			return middle;
		}


		public static string GetMonthName(int month)
		{
			switch (month) {
			case 1:
				return "January";
			case 2:
				return "February";
			case 3:
				return "March";
			case 4:
				return "April";
			case 5:
				return "May";
			case 6:
				return "June";
			case 7:
				return "July";
			case 8:
				return "August";
			case 9:
				return "September";
			case 10:
				return "October";
			case 11:
				return "November";
			case 12:
				return "December";
			}
			return "error";
		}

		

        public static void SetRectTransformParent(RectTransform rectTransform, Transform parent)
        {
            rectTransform.SetParent(parent);
            rectTransform.SetAsFirstSibling();
            rectTransform.anchorMin = Vector2.zero;
            rectTransform.anchorMax = Vector2.one;
            rectTransform.offsetMax = Vector3.zero;
            rectTransform.offsetMin = Vector3.zero;
            rectTransform.localRotation = Quaternion.identity;
        }

        public static GameObject CloneGameObject(GameObject obj, Transform parent)
        {
            GameObject go = GameObject.Instantiate(obj) as GameObject;
            go.gameObject.SetActive(true);
            go.transform.parent = parent;
            go.transform.localScale = Vector3.one;
            go.transform.position = obj.transform.position;
            return go;
        }
    }
}