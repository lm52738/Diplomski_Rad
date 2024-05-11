using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

/// <summary>
/// Displays pH level and liquids in the bowl using TextMeshProUGUI.
/// </summary>
public class PhLevelDisplay : MonoBehaviour
{
    private FillBowl fillBowl; // Reference to the FillBowl component
    public TextMeshProUGUI textMeshPro;
    private static string INSTRUCTIONS = "Use the 'Empty Bowl' button to clear your mixing vessel, and if you ever need a fresh start, simply hit the 'Reset' button.";

    // Start is called before the first frame update
    void Start()
    {
        // Set initial text
        textMeshPro.text = INSTRUCTIONS;

        // Find the FillBowl component in the scene
        fillBowl = FindObjectOfType<FillBowl>();
        if (fillBowl == null)
        {
            // Log a warning if FillBowl component is not found
            Debug.LogWarning("FillBowl component not found.");
        }
    }

    // Update is called once per frame
    void Update()
    {
        // Check if FillBowl component is available
        if (fillBowl != null)
        {
            // Get pH level from FillBowl
            double phLevel = fillBowl.getPhLevel();

            // Get list of liquids from FillBowl
            List<string> liquids = fillBowl.getLiquids();

            // Update the TextMeshProUGUI with pH level and list of liquids
            textMeshPro.text = INSTRUCTIONS + "\n\n" + "pH Level: " + phLevel.ToString("F2") + "\n";
            textMeshPro.text += "Liquids in the Bowl:\n";

            // Initialize a variable to keep track of the number of lines displayed
            int numLinesDisplayed = 0;


            foreach (string liquid in liquids)
            {
                // Check if the number of lines displayed exceeds a certain limit
                if (numLinesDisplayed >= 10)
                {
                    // If it exceeds, break out of the loop
                    textMeshPro.text += "..."; // Indicate that more lines are present but not displayed
                    break;
                }

                // Add the liquid to the display
                textMeshPro.text += liquid;
                numLinesDisplayed++;

                // Check if this is not the last liquid in the list
                if (liquid != liquids[liquids.Count - 1])
                {
                    // Add a comma and space after the liquid
                    textMeshPro.text += ", ";
                }
            }
        }
    }
}