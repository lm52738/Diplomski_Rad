using UnityEngine;
using UnityEngine.SceneManagement;

/// <summary>
/// This class handles the functionality of the back button.
/// </summary>
public class BackButton : MonoBehaviour
{
    // Method to handle the functionality of the back button
    public void BackToMainMenu()
    {
        // Load the scene named "MainMenu"
        SceneManager.LoadScene("MainMenu");
    }
}
