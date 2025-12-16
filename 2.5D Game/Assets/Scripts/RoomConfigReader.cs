using System;
using System.IO;
using UnityEngine;

/// <summary>
/// Reads room configuration from a text file on the user's desktop.
/// The file should be named "RoomConfig.txt" and contain just the room name/ID.
/// </summary>
public class RoomConfigReader : MonoBehaviour
{
    private static string _configuredRoomName;
    private static bool _hasReadConfig = false;
    
    /// <summary>
    /// Gets the configured room name from the desktop config file.
    /// Returns "TestRoom" as default if file doesn't exist or is empty.
    /// </summary>
    public static string GetConfiguredRoomName()
    {
        if (!_hasReadConfig)
        {
            ReadConfigFile();
            _hasReadConfig = true;
        }
        
        return _configuredRoomName;
    }
    
    private static void ReadConfigFile()
    {
        // Default room name
        _configuredRoomName = "TestRoom";
        
        try
        {
            // Get desktop path (works on Windows, Mac, and Linux)
            string desktopPath = Environment.GetFolderPath(Environment.SpecialFolder.Desktop);
            string configFilePath = Path.Combine(desktopPath, "RoomConfig.txt");
            
            if (File.Exists(configFilePath))
            {
                string roomName = File.ReadAllText(configFilePath).Trim();
                
                if (!string.IsNullOrEmpty(roomName))
                {
                    _configuredRoomName = roomName;
                    Debug.Log($"[RoomConfig] Successfully loaded room name from desktop: '{_configuredRoomName}'");
                    Debug.Log($"[RoomConfig] Config file location: {configFilePath}");
                }
                else
                {
                    Debug.LogWarning($"[RoomConfig] Config file is empty. Using default room: '{_configuredRoomName}'");
                }
            }
            else
            {
                Debug.LogWarning($"[RoomConfig] Config file not found at: {configFilePath}");
                Debug.LogWarning($"[RoomConfig] Using default room: '{_configuredRoomName}'");
                Debug.LogWarning($"[RoomConfig] Create a file named 'RoomConfig.txt' on your desktop with the desired room name.");
            }
        }
        catch (Exception ex)
        {
            Debug.LogError($"[RoomConfig] Error reading config file: {ex.Message}");
            Debug.LogWarning($"[RoomConfig] Using default room: '{_configuredRoomName}'");
        }
    }
    
    /// <summary>
    /// Force a re-read of the config file (useful for testing)
    /// </summary>
    public static void RefreshConfig()
    {
        _hasReadConfig = false;
        ReadConfigFile();
        _hasReadConfig = true;
    }
}
