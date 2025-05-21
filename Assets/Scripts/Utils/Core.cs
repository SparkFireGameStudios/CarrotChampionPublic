using UnityEngine;

namespace Utils
{
    public class Core : MonoBehaviour
    {
        public static void Log(string message, params object[] args)
        {
            // 编辑器模式下打印日志
            if (UnityEngine.Application.isEditor)
            {
                Debug.Log(string.Format(message, args));
            }
        }

        public static void LogWarning(string message, params object[] args)
        {
            Debug.LogWarning(string.Format(message, args));
        }

        public static void LogError(string message, params object[] args)
        {
            Debug.LogError(string.Format(message, args));
        }
    }
}
