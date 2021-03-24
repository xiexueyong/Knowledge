using System;
using UnityEngine;
using System.Collections.Generic;
using System.IO;

/// <summary>
/// 此模块废弃  新功能使用UnencryptedCPlayerPrefs
/// </summary>
//[Obsolete("已弃用，新功能使用UnencryptedCPlayerPrefs")]
public static class CPlayerPrefs
{
    public const string ConvertTittle = "CTU";

    static string GetCTUKey(string key)
    {
        return ConvertTittle + key;
    }
    #region New PlayerPref Stuff
    /// <summary>
    /// Returns true if key exists in the preferences.
    /// </summary>
    public static bool HasKey(string key)
    {
#if CONVERT
        bool uhaskey = UPlayerPrefs.HasKey(ConvertTittle + key);
        if (uhaskey)
        {
            return uhaskey;
        }
        else
        {
            string cKey = hashedKey(key);

            bool chaskey = PlayerPrefs.HasKey(cKey);
            if (chaskey)
                return chaskey;
        }
        return false;
#else
                string cKey = hashedKey(key);

        return PlayerPrefs.HasKey(cKey);
#endif
        // Get hashed key

    }

    /// <summary>
    /// Removes key and its corresponding value from the preferences.
    /// </summary>
    public static void DeleteKey(string key)
    {
#if CONVERT
        bool uhaskey = UPlayerPrefs.HasKey(ConvertTittle + key);
        if (uhaskey)
        {
            UPlayerPrefs.DeleteKey(ConvertTittle + key);
        }
        else
        {
            string cKey = hashedKey(key);

            bool chaskey = PlayerPrefs.HasKey(cKey);
            if (chaskey)
                PlayerPrefs.DeleteKey(cKey);
        }
#else
                // Get hashed key
        string cKey = hashedKey(key);

        PlayerPrefs.DeleteKey(cKey);
#endif

    }


    /// <summary>
    /// Writes all modified preferences to disk.
    /// </summary>
    public static void Save()
    {
        PlayerPrefs.Save();
    }
    #endregion

    #region New PlayerPref Setters
    /// <summary>
    /// Sets the value of the preference identified by key.
    /// </summary>
    public static void SetInt(string key, int val)
    {
#if CONVERT
        UPlayerPrefs.SetInt(GetCTUKey(key), val);
#else
         // Get crypted key
        string cKey = hashedKey(key);

        int cryptedInt = val;

        // If enabled use xor algo
        if (_useXor)
        {
            // Get crypt helper values
            int xor = computeXorOperand(key, cKey);
            int ad = computePlusOperand(xor);

            // Compute crypted int
            cryptedInt = (val + ad) ^ xor;
        }

        // If enabled use rijndael algo
        if (_useRijndael)
        {
            // Save
            string data = encrypt(cKey, string.Empty + cryptedInt);

            if (data != null)
            {
                PlayerPrefs.SetString(cKey, data);
            }
        }
        else
        {
            PlayerPrefs.SetInt(cKey, cryptedInt);
        }
#endif


    }

    /// <summary>
    /// Sets the value of the preference identified by key.
    /// </summary>
    public static void SetLong(string key, long val)
    {
#if CONVERT
        UPlayerPrefs.SetLong(GetCTUKey(key), val);
#else
        SetString(key, val.ToString());
#endif

    }

    /// <summary>
    /// Sets the value of the preference identified by key.
    /// </summary>
    public static void SetString(string key, string val)
    {
#if CONVERT
        UPlayerPrefs.SetString(GetCTUKey(key), val);
#else
         // Get crypted key
        string cKey = hashedKey(key);

        string cryptedString = val;
        // If enabled use xor algo
        if (_useXor)
        {
            // Get crypt helper values
            int xor = computeXorOperand(key, cKey);
            int ad = computePlusOperand(xor);

            // Compute crypted string
            cryptedString = "";
            foreach (char c in val)
            {
                char cryptedChar = (char)(((int)c + ad) ^ xor);
                cryptedString += cryptedChar;
            }
        }

        // If enabled use rijndael algo
        if (_useRijndael)
        {
            // Save
            string data = encrypt(cKey, cryptedString);
            if (data != null)
            {
                PlayerPrefs.SetString(cKey, data);
            }
        }
        else
        {
            PlayerPrefs.SetString(cKey, cryptedString);
        }
#endif

    }

    /// <summary>
    /// Sets the value of the preference identified by key.
    /// </summary>
    public static void SetFloat(string key, float val)
    {
#if CONVERT
        UPlayerPrefs.SetFloat(GetCTUKey(key), val);
#else
        SetString(key, val.ToString());
#endif

    }

    public static void SetBool(string key, bool value)
    {
#if CONVERT
        UPlayerPrefs.SetBool(GetCTUKey(key), value);
#else
         SetInt(key, value ? 1 : 0);
#endif

    }
    #endregion

    #region New PlayerPref Getters
    /// <summary>
    /// Returns the value corresponding to key in the preference file if it exists.
    /// If it doesn't exist, it will return defaultValue.
    /// </summary>
    public static int GetInt(string key, int defaultValue)
    {
#if CONVERT
        if (UPlayerPrefs.HasKey(GetCTUKey(key)))
        {
            return UPlayerPrefs.GetInt(GetCTUKey(key), defaultValue);
        }
        else
        {
            string cKey = hashedKey(key);

            // If the key doesn't exists, return defaultValue
            if (!PlayerPrefs.HasKey(cKey))
            {
                return defaultValue;
            }
            else
            {
                int storedPref = defaultValue;
                if (_useRijndael)
                {
                    try
                    {
                        storedPref = int.Parse(decrypt(cKey));
                    }
                    catch (Exception ex)
                    {
                        BetaLog.Log.Exception(ex);
                    }
                }
                else
                {
                    storedPref = PlayerPrefs.GetInt(cKey);
                }

                // If xor algo enabled
                int realValue = storedPref;
                if (_useXor)
                {
                    // Get crypt helper values
                    int xor = computeXorOperand(key, cKey);
                    int ad = computePlusOperand(xor);

                    // Compute real value
                    realValue = (xor ^ storedPref) - ad;
                }
                //转换后保存
                UPlayerPrefs.SetInt(GetCTUKey(key), realValue);
                return realValue;
            }
        }
#else
         // Get crypted key
        string cKey = hashedKey(key);

        // If the key doesn't exists, return defaultValue
        if (!PlayerPrefs.HasKey(cKey))
        {
            return defaultValue;
        }

        // Read storedPref
        int storedPref = defaultValue;
        if (_useRijndael)
        {
            try
            {
                storedPref = int.Parse(decrypt(cKey));
            }
            catch (Exception ex)
            {
                BetaLog.Log.Exception(ex);
            }
        }
        else
        {
            storedPref = PlayerPrefs.GetInt(cKey);
        }

        // If xor algo enabled
        int realValue = storedPref;
        if (_useXor)
        {
            // Get crypt helper values
            int xor = computeXorOperand(key, cKey);
            int ad = computePlusOperand(xor);

            // Compute real value
            realValue = (xor ^ storedPref) - ad;
        }

        return realValue;
#endif

    }

    public static int GetInt(string key)
    {
        return GetInt(key, 0);
    }

    /// <summary>
    /// Returns the value corresponding to key in the preference file if it exists.
    /// If it doesn't exist, it will return defaultValue.
    /// </summary>
    public static long GetLong(string key, long defaultValue)
    {
#if CONVERT
        if (UPlayerPrefs.HasKey(GetCTUKey(key)))
        {
            return UPlayerPrefs.GetLong(GetCTUKey(key), defaultValue);
        }
        else
        {
            long reallong = long.Parse(CGetString(key, defaultValue.ToString()));
            //转换后保存到本地
            UPlayerPrefs.SetLong(GetCTUKey(key), reallong);
            return reallong;
        }
#else
        return long.Parse(GetString(key, defaultValue.ToString()));
#endif
    }

    public static long GetLong(string key)
    {
        return GetLong(key, 0);
    }

    /// <summary>
    /// 内部使用的   不需要本地转换
    /// </summary>
    static string CGetString(string key, string defaultValue)
    {
#if CONVERT
        if (UPlayerPrefs.HasKey(GetCTUKey(key)))
        {
            return UPlayerPrefs.GetString(GetCTUKey(key));
        }
        else
        {
            // Get crypted key
            string cKey = hashedKey(key);

            // If the key doesn't exists, return defaultValue
            if (!PlayerPrefs.HasKey(cKey))
            {
                return defaultValue;
            }

            // Read storedPref
            string storedPref;
            if (_useRijndael)
            {
                storedPref = decrypt(cKey);
            }
            else
            {
                storedPref = PlayerPrefs.GetString(cKey);
            }

            // XOR algo enabled?
            string realString = storedPref;
            if (_useXor)
            {
                // Get crypt helper values
                int xor = computeXorOperand(key, cKey);
                int ad = computePlusOperand(xor);

                // Compute real value
                realString = "";
                foreach (char c in storedPref)
                {
                    char realChar = (char)((xor ^ c) - ad);
                    realString += realChar;
                }
            }

            return realString;
        }
#else
         // Get crypted key
        string cKey = hashedKey(key);

        // If the key doesn't exists, return defaultValue
        if (!PlayerPrefs.HasKey(cKey))
        {
            return defaultValue;
        }

        // Read storedPref
        string storedPref;
        if (_useRijndael)
        {
            storedPref = decrypt(cKey);
        }
        else
        {
            storedPref = PlayerPrefs.GetString(cKey);
        }

        // XOR algo enabled?
        string realString = storedPref;
        if (_useXor)
        {
            // Get crypt helper values
            int xor = computeXorOperand(key, cKey);
            int ad = computePlusOperand(xor);

            // Compute real value
            realString = "";
            foreach (char c in storedPref)
            {
                char realChar = (char)((xor ^ c) - ad);
                realString += realChar;
            }
        }

        return realString;
#endif

    }

    public static string GetString(string key)
    {
        return GetString(key, "");
    }
    public static string GetString(string key, string defaultValue)
    {
#if CONVERT
        if (UPlayerPrefs.HasKey(GetCTUKey(key)))
        {
            return UPlayerPrefs.GetString(GetCTUKey(key));
        }
        else
        {
            // Get crypted key
            string cKey = hashedKey(key);

            // If the key doesn't exists, return defaultValue
            if (!PlayerPrefs.HasKey(cKey))
            {
                return defaultValue;
            }

            // Read storedPref
            string storedPref;
            if (_useRijndael)
            {
                storedPref = decrypt(cKey);
            }
            else
            {
                storedPref = PlayerPrefs.GetString(cKey);
            }

            // XOR algo enabled?
            string realString = storedPref;
            if (_useXor)
            {
                // Get crypt helper values
                int xor = computeXorOperand(key, cKey);
                int ad = computePlusOperand(xor);

                // Compute real value
                realString = "";
                foreach (char c in storedPref)
                {
                    char realChar = (char)((xor ^ c) - ad);
                    realString += realChar;
                }
            }
            //转换一下
            UPlayerPrefs.SetString(GetCTUKey(key), realString);
            return realString;
        }
#else
         // Get crypted key
        string cKey = hashedKey(key);

        // If the key doesn't exists, return defaultValue
        if (!PlayerPrefs.HasKey(cKey))
        {
            return defaultValue;
        }

        // Read storedPref
        string storedPref;
        if (_useRijndael)
        {
            storedPref = decrypt(cKey);
        }
        else
        {
            storedPref = PlayerPrefs.GetString(cKey);
        }

        // XOR algo enabled?
        string realString = storedPref;
        if (_useXor)
        {
            // Get crypt helper values
            int xor = computeXorOperand(key, cKey);
            int ad = computePlusOperand(xor);

            // Compute real value
            realString = "";
            foreach (char c in storedPref)
            {
                char realChar = (char)((xor ^ c) - ad);
                realString += realChar;
            }
        }

        return realString;
#endif

    }

    /// <summary>
    /// Returns the value corresponding to key in the preference file if it exists.
    /// If it doesn't exist, it will return defaultValue.
    /// </summary>
    public static float GetFloat(string key, float defaultValue)
    {
#if CONVERT
        if (UPlayerPrefs.HasKey(GetCTUKey(key)))
        {
            return UPlayerPrefs.GetFloat(GetCTUKey(key), defaultValue);
        }
        else
        {
            float realfloat = float.Parse(CGetString(key, defaultValue.ToString()));
            //转换后保存在本地
            UPlayerPrefs.SetFloat(GetCTUKey(key), realfloat);
            return realfloat;
        }
#else
        return float.Parse(GetString(key, defaultValue.ToString()));
#endif

    }

    public static float GetFloat(string key)
    {
        return GetFloat(key, 0);
    }

    public static bool GetBool(string key, bool defaultValue = false)
    {
#if CONVERT
        if (UPlayerPrefs.HasKey(GetCTUKey(key)))
        {
            return UPlayerPrefs.GetBool(GetCTUKey(key), defaultValue);
        }
        else
        {
            if (!HasKey(key))
                return defaultValue;

            bool realbool = GetInt(key) == 1;
            //转换后保存到本地
            UPlayerPrefs.SetBool(GetCTUKey(key), realbool);
            return realbool;
        }
#else
        if (!HasKey(key))
            return defaultValue;

        return GetInt(key) == 1;
#endif

    }
    #endregion

    #region Double
    public static void SetDouble(string key, double value)
    {
#if CONVERT
        UPlayerPrefs.SetDouble(GetCTUKey(key), value);
#else
        PlayerPrefs.SetString(key, DoubleToString(value));
#endif

    }

    public static double GetDouble(string key, double defaultValue)
    {
#if CONVERT
        if (UPlayerPrefs.HasKey(GetCTUKey(key)))
        {
            return UPlayerPrefs.GetDouble(GetCTUKey(key), defaultValue);
        }
        else
        {
            string defaultVal = DoubleToString(defaultValue);
            double realdouble = StringToDouble(PlayerPrefs.GetString(key, defaultVal));
            //转换后保存本地
            UPlayerPrefs.SetDouble(GetCTUKey(key), realdouble);
            return realdouble;
        }
#else
        string defaultVal = DoubleToString(defaultValue);
        return StringToDouble(PlayerPrefs.GetString(key, defaultVal));
#endif

    }

    public static double GetDouble(string key)
    {
        return GetDouble(key, 0d);
    }

    private static string DoubleToString(double target)
    {
        return target.ToString("R");
    }

    private static double StringToDouble(string target)
    {
        if (string.IsNullOrEmpty(target))
            return 0d;

        return double.Parse(target);
    }
    #endregion

    #region Crypto


    /// <summary>
    /// fix by handsome YZY Special Problem
    /// </summary>
    /// <param name="cKey"></param>
    /// <returns></returns>
    private static string decrypt(string cKey)
    {
        string result = "";
        try
        {
            result = YZY.CryptoPlayerPrefs.Helper.DecryptString(PlayerPrefs.GetString(cKey), getEncryptionPassword(cKey));
        }
        catch (Exception ex)
        {
            BetaLog.Log.Exception(ex);
        }
        return result;
    }

    private static Dictionary<string, string> keyHashs;

    private static string hashedKey(string key)
    {
        if (!_useHashKey) return key;
        // Initialize HashKey-Dictionary
        if (keyHashs == null)
        {
            keyHashs = new Dictionary<string, string>();
        }

        // Check if hashed key already was computed
        if (keyHashs.ContainsKey(key))
        {
            // Return computed key
            return keyHashs[key];
        }

        // Create hashed key and add to dictionary
        string cKey = hashSum(key);
        keyHashs.Add(key, cKey);

        return cKey;
    }

    private static Dictionary<string, int> xorOperands;

    private static int computeXorOperand(string key, string cryptedKey)
    {
        if (xorOperands == null)
        {
            xorOperands = new Dictionary<string, int>();
        }

        if (xorOperands.ContainsKey(key))
        {
            return xorOperands[key];
        }

        int xorOperand = 0;
        foreach (char c in cryptedKey)
        {
            xorOperand += (int)c;
        }
        xorOperand += salt;

        xorOperands.Add(key, xorOperand);
        return xorOperand;
    }

    private static int computePlusOperand(int xor)
    {
        return xor & xor << 1;
    }

    public static string hashSum(string strToEncrypt)
    {
        System.Text.UTF8Encoding ue = new System.Text.UTF8Encoding();
        byte[] bytes = ue.GetBytes(strToEncrypt);


        byte[] hashBytes = YZY.CryptoPlayerPrefs.Helper.hashBytes(bytes);

        // Convert the encrypted bytes back to a string (base 16)
        string hashString = "";

        for (int i = 0; i < hashBytes.Length; i++)
        {
            hashString += System.Convert.ToString(hashBytes[i], 16).PadLeft(2, '0');
        }

        return hashString.PadLeft(32, '0');
    }

    private static string getEncryptionPassword(string pw)
    {
        return hashSum(pw + salt);
    }
    #endregion

    #region Settings
    private static int salt = int.MaxValue;

    /// <summary>
    /// Sets the salt. 
    /// Define this before use of the CryptoPlayerPrefs Class for your project. 
    /// NEVER change this again for this project!
    /// </summary>
    /// <param name='s'>
    /// Salt value
    /// </param>
    public static void setSalt(int s)
    {
        salt = s;
    }

    private static bool _useRijndael = false;

    /// <summary>
    /// Sets if Rijndael Algo should be used. 
    /// Define this before use of the CryptoPlayerPrefs Class for your project. 
    /// NEVER change this again for this project!
    /// </summary>
    /// <param name='use'>
    /// Use Rijndael or not
    /// </param>
    public static void useRijndael(bool use)
    {
        _useRijndael = use;
    }

    private static bool _useHashKey = false;
    private static bool _useXor = false;

    /// <summary>
    /// Sets if XOR Algo should be used. 
    /// Define this before use of the CryptoPlayerPrefs Class for your project. 
    /// NEVER change this again for this project!
    /// </summary>
    /// <param name='use'>
    /// Use XOR Algo or not
    /// </param>
    public static void useXor(bool use)
    {
        _useXor = use;
    }

    private static string encrypt(string cKey, string data)
    {
        try
        {
            return YZY.CryptoPlayerPrefs.Helper.EncryptString(data, getEncryptionPassword(cKey));
        }
        catch (Exception ex)
        {
            BetaLog.Log.Exception(ex);
        }

        return null;
    }
    #endregion

}

