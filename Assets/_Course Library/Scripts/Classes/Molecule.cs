using UnityEngine;

public class Molecule : MonoBehaviour
{
    // Unique ID for the molecule
    private int moleculeID;
    private int nextMoleculeID = 1; // Counter for the next molecule ID;

    public int MoleculeID
    {
        get { return moleculeID; }
    }

    // Method to assign a unique ID to the molecule
    public void AssignID()
    {
        moleculeID = nextMoleculeID;
        nextMoleculeID++;
}
}
