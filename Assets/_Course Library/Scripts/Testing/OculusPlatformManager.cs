using UnityEngine;
using Oculus.Platform;
using Oculus.Platform.Models;
using System.IO;

public class OculusPlatformManager : MonoBehaviour
{
    public string game;

    private void Start()
    {
        // Initialize Oculus Platform SDK
        if (!Core.IsInitialized())
        {
            Core.Initialize();
        }

        // Check if user is entitled to the application
        Entitlements.IsUserEntitledToApplication().OnComplete(EntitlementCallback);
    }

    // Callback for entitlement check
    private void EntitlementCallback(Message msg)
    {
        if (msg.IsError)
        {
            Debug.LogError("User not entitled to the application");
            // Handle entitlement error (e.g., exit the application)
        }
        else
        {
            // Retrieve the user ID if entitled
            Users.GetLoggedInUser().OnComplete(GetUserCallback);
        }
    }

    // Callback for getting user information
    private void GetUserCallback(Message<User> msg)
    {
        if (msg.IsError)
        {
            Debug.LogError("Failed to get user information");
        }
        else
        {
            string userId = msg.Data.ID.ToString();
            LogUserIDAndTimestamp(userId);
        }
    }

    // Log user ID and timestamp
    public void LogUserIDAndTimestamp(string userId)
    {
        string timestamp = System.DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
        string logMessage = $"User ID: {userId}, Entered: {game}, Timestamp: {timestamp}";

        Debug.Log(logMessage);

        // Log to file
        LogToFile(logMessage);
    }

    // Function to log data to a file
    private void LogToFile(string logMessage)
    {
        string filePath = Path.Combine(UnityEngine.Application.persistentDataPath, "user_activity_log.txt");

        try
        {
            // Append the log to the file
            using (StreamWriter writer = new StreamWriter(filePath, true))
            {
                writer.WriteLine(logMessage);
            }
        }
        catch (System.Exception e)
        {
            Debug.LogError("Failed to write to log file: " + e.Message);
        }
    }
}