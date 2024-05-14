using UnityEngine;

///<summary>
/// Represents a molecule in the scene.
///</summary>
public class Molecule : MonoBehaviour
{
    // Unique ID for the molecule
    private int moleculeID;

    public int MoleculeID
    {
        get { return moleculeID; }
    }

    // Method to assign a unique ID to the molecule
    public void AssignID(int id)
    {
        moleculeID = id;
}
}
