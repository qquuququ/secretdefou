using UnityEngine;

namespace Archipelago
{
    public static class Logging
    {
        public static void Log(string message, bool onScreen = false, bool inConsole = true, bool error = false)
        {
            if (inConsole)
            {
                if (error)
                    Debug.LogError(message);
                else
                    Debug.Log(message);
            }
        }

        public static void LogDebug(string message)
        {
            Debug.Log("[DEBUG] " + message);
        }

        public static void LogError(string message, bool onScreen = false, bool inConsole = true)
        {
            Debug.LogError(message);
        }

        // ✅ Add this method
        public static void TryUpdateLog()
        {
        }
    }
}