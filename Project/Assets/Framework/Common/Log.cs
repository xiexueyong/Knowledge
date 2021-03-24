#define LOG_ENABLE
#define LOG_CONSOLE
//#define LOG_FILE

using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Text;
using UnityEngine;


namespace BetaLog
{
    /// <summary>
    /// 日志输出管理
    /// </summary>
    public class Log
    {
        private const Boolean SHOW_STACK = true;

#if LOG_FILE
    private static LogFileWriter writer = new LogFileWriter(); 
#endif

        static Log()
        {
            Application.RegisterLogCallback(new Application.LogCallback(ProcessExceptionReport));
        }

        public static bool LogEnable()
        {
#if LOG_ENABLE
//            if (ConsoleManager._instacne != null && ConsoleManager._instacne.AppIsRelease)
//                return false;
            return true;
#else
            return false;
#endif
        }

        public static void Release()
        {
#if LOG_FILE
            if (writer != null)
            {
                writer.Release();
                writer = null; 
            }
#endif
        }

        public static void Info(object message, UnityEngine.Object context = null)
        {
#if LOG_ENABLE
            if (!LogEnable())
                return;
#if LOG_CONSOLE
            if (context == null)
            {
                UnityEngine.Debug.Log(message);
            }
            else
            {
                UnityEngine.Debug.Log(message, context);
            }
#endif
            LogFile(message, LogType.Log);
#endif
        }

        public static void InfoFormat(string format, params object[] args)
        {
#if LOG_ENABLE
            if (!LogEnable())
                return;
#if LOG_CONSOLE
            UnityEngine.Debug.LogFormat(format, args);
#endif
#endif
        }

        public static void InfoFormat(UnityEngine.Object context, string format, params object[] args)
        {
#if LOG_ENABLE
            if (!LogEnable())
                return;
#if LOG_CONSOLE
            UnityEngine.Debug.LogFormat(context, format, args);
#endif
#endif
        }

        public static void Assertion(string message, UnityEngine.Object context = null)
        {
#if LOG_ENABLE
            if (!LogEnable())
                return;
#if LOG_CONSOLE
            if (context == null)
            {
                UnityEngine.Debug.LogAssertion(message);
            }
            else
            {
                UnityEngine.Debug.LogAssertion(message, context);
            }
#endif
            LogFile(message, LogType.Assert);
#endif
        }

        public static void Warning(string message, UnityEngine.Object context = null)
        {
#if LOG_ENABLE
            if (!LogEnable())
                return;
#if LOG_CONSOLE
            if (context == null)
            {
                UnityEngine.Debug.LogWarning(message);
            }
            else
            {
                UnityEngine.Debug.LogWarning(message, context);
            }
#endif
            LogFile(message, LogType.Warning);
#endif
        }

        public static void WarningFormat(string format, params object[] args)
        {
#if LOG_ENABLE
            if (!LogEnable())
                return;
#if LOG_CONSOLE
            UnityEngine.Debug.LogWarningFormat(format, args);
#endif
#endif
        }

        public static void WarningFormat(UnityEngine.Object context, string format, params object[] args)
        {
#if LOG_ENABLE
            if (!LogEnable())
                return;
#if LOG_CONSOLE
            UnityEngine.Debug.LogWarningFormat(context, format, args);
#endif
#endif
        }

        public static void Error(string message, UnityEngine.Object context = null)
        {
#if LOG_ENABLE
            if (!LogEnable())
                return;
#if LOG_CONSOLE
            if (context == null)
            {
                UnityEngine.Debug.LogError(message);
            }
            else
            {
                UnityEngine.Debug.LogError(message, context);
            }
#endif
            LogFile(message, LogType.Error);
#endif
        }

        public static void ErrorFormat(string format, params object[] args)
        {
#if LOG_ENABLE
            if (!LogEnable())
                return;
#if LOG_CONSOLE
            UnityEngine.Debug.LogErrorFormat(format, args);
#endif
#endif
        }

        public static void ErrorFormat(UnityEngine.Object context, string format, params object[] args)
        {
#if LOG_ENABLE
            if (!LogEnable())
                return;
#if LOG_CONSOLE
            UnityEngine.Debug.LogErrorFormat(context, format, args);
#endif
#endif
        }

        public static void Exception(Exception exception, UnityEngine.Object context = null)
        {
#if LOG_ENABLE
            if (!LogEnable())
                return;
#if LOG_CONSOLE
            if (context == null)
            {
                UnityEngine.Debug.LogException(exception);
            }
            else
            {
                UnityEngine.Debug.LogException(exception, context);
            }
#endif
            LogFile(exception.Message, LogType.Exception);
#endif
        }

        /// <summary>
        /// 获取堆栈信息。
        /// </summary>
        /// <returns></returns>
        private static String GetStacksInfo()
        {
            StringBuilder sb = new StringBuilder();
            StackTrace st = new StackTrace();
            var sf = st.GetFrames();
            for (int i = 2; i < sf.Length; i++)
            {
                sb.AppendLine(sf[i].ToString());
            }

            return sb.ToString();
        }

        /// <summary>
        /// 获取调用栈信息。
        /// </summary>
        /// <returns>调用栈信息</returns>
        private static String GetStackInfo()
        {
            StackTrace st = new StackTrace();
            StackFrame sf = st.GetFrame(2);//[0]为本身的方法 [1]为调用方法
            var method = sf.GetMethod();
            return String.Format("{0}.{1}(): ", method.ReflectedType.Name, method.Name);
        }

        /// <summary>
        /// 写日志。
        /// </summary>
        /// <param name="message">日志内容</param>
        private static void LogFile(object message, LogType level)
        {
#if LOG_FILE
            if (message == null || level == null)
                return;
            string text = message is string ? (string)message : message.ToString();
            string stack = string.Empty;
            string type = string.Empty;
            switch (level)
            {
                case LogType.Assert:
                    type = " [ASSERT]: ";
                    stack = GetStackInfo();
                    break;
                case LogType.Error:
                    type = " [ERROR]: ";
                    stack = GetStacksInfo();
                    break;
                case LogType.Exception:
                    type = " [EXCEPTION]: ";
                    stack = GetStacksInfo();
                    break;
                case LogType.Log:
                    type = " [INFO]: ";
                    stack = GetStackInfo();
                    break;
                case LogType.Warning:
                    type = " [WARNING]: ";
                    stack = GetStackInfo();
                    break;
                default:
                    break;
            }
            if (writer == null) { 
                writer = new LogFileWriter();
            }
            writer.WriteLog(string.Concat(DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss,fff"), type, text, "\n", SHOW_STACK ? stack : "")); 
#endif
        }

        private static void ProcessExceptionReport(string message, string stackTrace, LogType type)
        {
            //string text = string.Concat(" [SYS_", type, "]: ", message, '\n', stackTrace);
            //switch (type)
            //{
            //    case LogType.Assert:
            //        Assertion(text);
            //        break;
            //    case LogType.Error:
            //    case LogType.Exception:
            //        Error(text);
            //        break;
            //    case LogType.Log:
            //        Info(text);
            //        break;
            //    case LogType.Warning:
            //        Warning(text);
            //        break;
            //    default:
            //        break;
            //}
        }
    }

    /// <summary>
    /// 日志写入文件管理类。
    /// </summary>
    public class LogFileWriter
    {
        private FileStream m_fileStream;
        private StreamWriter m_streamWriter;
        private readonly object m_locker = new object();

        /// <summary>
        /// 默认构造函数。
        /// </summary>
        public LogFileWriter()
        {
            try
            {
                string logPath = UnityEngine.Application.persistentDataPath + "/logs/";
                if (!Directory.Exists(logPath))
                {
                    Directory.CreateDirectory(logPath);
                }
                string logFilePath = String.Concat(logPath, String.Format("log_{0}.txt", DateTime.Today.ToString("yyyyMMdd")));
                m_fileStream = new FileStream(logFilePath, FileMode.Append, FileAccess.Write, FileShare.ReadWrite);
                m_streamWriter = new StreamWriter(m_fileStream);
            }
            catch (Exception ex)
            {
                UnityEngine.Debug.LogError(ex.Message);
            }
        }

        /// <summary>
        /// 释放资源。
        /// </summary>
        public void Release()
        {
            lock (m_locker)
            {
                if (m_streamWriter != null)
                {
                    m_streamWriter.Close();
                    m_streamWriter.Dispose();
                }
                if (m_fileStream != null)
                {
                    m_fileStream.Close();
                    m_fileStream.Dispose();
                }
            }
        }


        /// <summary>
        /// 写日志。
        /// </summary>
        /// <param name="msg">日志内容</param>
        public void WriteLog(string msg)
        {
            lock (m_locker)
            {
                try
                {
                    if (m_streamWriter != null)
                    {
                        m_streamWriter.WriteLine(msg);
                        m_streamWriter.Flush();
                    }
                }
                catch (Exception ex)
                {
                    UnityEngine.Debug.LogError(ex.Message);
                }
            }
        }
    }
}
