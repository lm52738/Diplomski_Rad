using System.Collections.Generic;
using System.IO;
using TMPro;
using UnityEngine;

/// <summary>
/// Manages the display of atom information on the canvas.
/// </summary>
public class DisplayAtomInfo : MonoBehaviour
{
    public TMP_Text elementNameText;
    public TMP_Text subatomicParticlesText;
    public TMP_Text atomicMassIonText;
    public TMP_Text descriptionText;

    private List<Atom> atoms = new List<Atom>();
    private Atom activeAtom;


    // Method to display atom information on canvas for a given symbol
    public void DisplayAtom(string symbol)
    {
        ResetAtoms();

        // Find the atom with the given symbol
        Atom atom = atoms.Find(a => a.Symbol == symbol);

        if (atom != null)
        {
            activeAtom = atom;
            updateDisplay();
        }
    }

    // Updates the display with current active atom information
    private void updateDisplay()
    {
        // Display element name and symbol
        elementNameText.text = "<b><color=yellow>" + activeAtom.Name + " - " + activeAtom.Symbol + "</color></b>";

        // Display subatomic particles information with symbols as superscripts
        subatomicParticlesText.text = "<color=yellow>N (p<sup>+</sup>):</color> " + activeAtom.Protons + "\n" +
                                      "<color=yellow>N (e<sup>-</sup>):</color> " + activeAtom.Electrons + "\n" +
                                      "<color=yellow>N (n<sup>0</sup>):</color> " + activeAtom.Neutrons;

        // Display atomic mass and ion information
        atomicMassIonText.text = "<color=yellow>Atomic Mass (A):</color> " + activeAtom.AtomicMass + "\n" +
                                  "<color=yellow>Atomic Relative Mass:</color> " + activeAtom.AtomicRelativeMass + "\n" +
                                  "<color=yellow>Ion Type:</color> " + activeAtom.IonType.Type;

        // Display description
        descriptionText.text = activeAtom.Description;
    }

    // Updates the displayed atom information with the provided updated atom
    public void UpdateDisplayedAtom(Atom updatedAtom)
    {

        // Check if the updated atom is not null and if there is an active atom being displayed
        if (updatedAtom == null || activeAtom == null || updatedAtom.Symbol != activeAtom.Symbol)
        {
            // Display a warning if the updated atom is null, if there is no active atom, or if their symbols don't match
            Debug.LogWarning("Cannot update displayed atom. Either the updated atom is null, there is no active atom being displayed, or their symbols don't match.");
            return;
        }

        activeAtom.Neutrons = updatedAtom.Neutrons;
        activeAtom.Protons = updatedAtom.Protons;
        activeAtom.Electrons = updatedAtom.Electrons;

        activeAtom.AtomicMass = activeAtom.Neutrons + activeAtom.Protons;

        // Check for specific situations and update the description accordingly
        if (activeAtom.Electrons < activeAtom.Protons)
        {
            activeAtom.IonType = IonType.Cation;
            activeAtom.Description = activeAtom.CationText;
        }
        else if (activeAtom.Electrons > activeAtom.Protons)
        {
            activeAtom.IonType = IonType.Anion;
            activeAtom.Description = activeAtom.AnionText;
        } else
        {
            activeAtom.Description = activeAtom.NeutralText;
        }


        updateDisplay();

    }

    // Resets the list of atoms by reading from file
    private void ResetAtoms()
    {
        atoms.Clear();

        try
        {
            string[] lines = File.ReadAllLines("Assets/_Course Library/Scripts/ScriptAssets/Atoms.txt");

            foreach (string line in lines)
            {
                string[] parts = line.Split('|');
                if (parts.Length == 10)
                {
                    string name = parts[0].Trim();
                    string symbol = parts[1].Trim();
                    int protons = int.Parse(parts[2].Trim());
                    int neutrons = int.Parse(parts[3].Trim());
                    int electrons = int.Parse(parts[4].Trim());
                    int atomicMass = int.Parse(parts[5].Trim());
                    double atomicRelativeMass = double.Parse(parts[6].Trim());
                    string cationText = parts[7].Trim();
                    string anionText = parts[8].Trim();
                    string neutralText = parts[9].Trim();

                    Atom atom = new Atom(name, symbol, protons, neutrons, electrons, atomicMass, atomicRelativeMass, IonType.Neutral, cationText, anionText, neutralText, neutralText);
                    atoms.Add(atom);
                }
                else
                {
                    Debug.LogError("Invalid format in atom line: " + line);
                }
            }
        }
        catch (FileNotFoundException)
        {
            Debug.LogError("Atoms file not found!");
        }
    }
}
