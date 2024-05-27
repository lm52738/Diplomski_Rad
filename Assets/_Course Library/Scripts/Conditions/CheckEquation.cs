using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

///<summary>
/// Checks the correctness of the equation and displays appropriate messages.
///</summary>
public class CheckEquation : MonoBehaviour
{
    public GameObject canvasWithCreateEquation;
    private CreateEquation createEquationScript;
    public List<ParticleSystem> fireworksParticleSystems; // List of fireworks particle systems
    public TextMeshProUGUI messageText; // TextMeshProUGUI component to display the message
    public Button nextLevelButton; // Reference to the UI button for the next level
    public AudioClip fireworksSound;
    public AudioClip trumpetSound;
    private AudioSource audioSource;

    void Start()
    {
        audioSource = GetComponent<AudioSource>();
        audioSource.clip = fireworksSound;

        StopFireworks();

        // Make sure the canvas with CreateEquation script is active
        canvasWithCreateEquation.SetActive(true);

        // Get the CreateEquation script from the canvas
        createEquationScript = canvasWithCreateEquation.GetComponent<CreateEquation>();
        
        // Start checking the equation every 2 seconds
        StartCoroutine(CheckEquationCoroutine());
    }

    // Coroutine to periodically check the current equation.
    IEnumerator CheckEquationCoroutine()
    {
        // Check the equation every 2 seconds
        while (true)
        {
            yield return new WaitForSeconds(5f);
            CheckCurrentEquation();
        }
    }

    // Checks the current equation for correctness.
    void CheckCurrentEquation()
    {
        // Get the current equation and its text
        Equation currentEquation = createEquationScript.GetCurrentEquation();

        if (currentEquation == null)
        {
            StopFireworks();
            ShowMessage("Congratulations!\nThe game has been successfully completed."); 
            if (nextLevelButton != null)
            {
                nextLevelButton.gameObject.SetActive(false); // Deactivate the button
            }

            return;
        }

        string currentEquationText = createEquationScript.GetEquationText();

        // Get the full equation text
        string fullEquationText = currentEquation.FullEquation;

        // Debug.Log(currentEquationText + " : " + fullEquationText);

        // Check if the current equation text matches the full equation
        if (currentEquationText.Equals(fullEquationText))
        {
            //Debug.Log("Equation is correct!");
            StartFireworks(); // Call method to start the fireworks particle systems
            ShowMessage("Correct! Proceed to the next level."); // Display message on the canvas

            if (nextLevelButton != null)
            {
                nextLevelButton.gameObject.SetActive(true); // Activate the button
            }

        }
        else
        {
            StopFireworks();
            ShowMessage(""); // Clear the message
            if (nextLevelButton != null)
            {
                nextLevelButton.gameObject.SetActive(false); // Deactivate the button
            }

        }
    }

    // Starts the fireworks particle systems and plays associated sounds.
    void StartFireworks()
    {
        // Start all fireworks particle systems in the list
        foreach (ParticleSystem fireworksParticleSystem in fireworksParticleSystems)
        {
            // Check if the fireworks particle system is not null and not already playing
            if (fireworksParticleSystem != null && !fireworksParticleSystem.isEmitting)
            {
                fireworksParticleSystem.Play(); // Start the fireworks particle system
                audioSource.PlayOneShot(trumpetSound, 1);
                
                audioSource.Play();
            }
        }
    }

    // Stops the fireworks particle systems and associated sounds.
    void StopFireworks()
    {
        // Stop all fireworks particle systems in the list
        foreach (ParticleSystem fireworksParticleSystem in fireworksParticleSystems)
        {
            // Check if the fireworks particle system is not null and is playing
            if (fireworksParticleSystem != null && fireworksParticleSystem.isEmitting)
            {
                fireworksParticleSystem.Stop(); // Stop the fireworks particle system
                audioSource.Stop();
            }
        }
    }

    // Displays a message on the canvas with optional color settings.
    void ShowMessage(string message)
    {
        // Display the message on the canvas
        if (messageText != null)
        {
            messageText.text = message;
            messageText.color = Color.green; // Set the text color to green
        }
    }
}
