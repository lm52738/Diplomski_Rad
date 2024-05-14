using System.IO;
using UnityEngine;

public class UserLogger : MonoBehaviour
{
    public string game;

    private void Start()
    {
        LogUserIDAndTimestamp();
    }

    // This method should be called when the user enters each game
    public void LogUserIDAndTimestamp()
    {
        string userId = GetOculusUserID();
        string timestamp = System.DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
        string logMessage = "User ID: " + userId + ", Entered: " + game + ", Timestamp: " + timestamp;

        Debug.Log(logMessage);

        // Log to file
        LogToFile(logMessage);
    }

    // Function to get the Oculus UserID
    private string GetOculusUserID()
    {
        return "360vb8fsa57";
    }

    // Function to log data to a file
    private void LogToFile(string logMessage)
    {
        string filePath = "Assets/Logs/user_activity_log.txt";

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
