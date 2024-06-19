using TMPro;
using UnityEngine;

/// <summary>
/// Handles the creation and management of individual molecules.
/// </summary>
public class CreateMolecule : MonoBehaviour
{
    public GameObject moleculePrefab;
    private float spawnHeightOffset = 0.0764912f; // Adjust this value to set the height offset
    private string moleculeFormula;
    private GameObject[] spawnedMolecules; // Array to store all spawned molecules
    private int nextMoleculeID = 1; // Counter for the next molecule ID;

    private void Start()
    {
        spawnedMolecules = new GameObject[0]; // Initialize the array
    }

    private void Update()
    {
        // Check if the mat center is empty
        if (IsEmpty() && moleculeFormula != null && !moleculeFormula.Equals(""))
        {
            // Get the height of the mat
            float matHeight = transform.position.y + GetComponent<MeshRenderer>().bounds.size.y / 2f;

            // Calculate the spawn position directly on top of the mat with offset
            Vector3 spawnPosition = new Vector3(transform.position.x, matHeight + spawnHeightOffset, transform.position.z);

            GameObject moleculeInstance = Instantiate(moleculePrefab, spawnPosition, Quaternion.identity);

            // Assign a unique ID to the molecule
            Molecule moleculeScript = moleculeInstance.GetComponent<Molecule>();
            moleculeScript.AssignID(nextMoleculeID++);

            // Set the layer of the spawned molecule to match the layer of the mat
            moleculeInstance.layer = gameObject.layer;

            // Rotate the molecule on the y-axis for 180 degrees if it's spawned with the "Reagents" layer
            if (gameObject.layer == LayerMask.NameToLayer("Reagents"))
            {
                moleculeInstance.transform.Rotate(0f, 180f, 0f);
            }

            // Add the new molecule to the array of spawned molecules
            AddSpawnedMolecule(moleculeInstance);

            ChangeText(moleculeInstance, moleculeFormula); // Pass the formula to ChangeText
        }
    }

    // New method to set molecule formula
    public void SetMoleculeFormula(string formula)
    {
        moleculeFormula = formula;

        // Destroy all previously spawned molecules
        if (spawnedMolecules != null)
        {
            DestroyAllMolecules();
        }
    }

    // ChangeText method now accepts the formula as input
    void ChangeText(GameObject moleculeInstance, string formula)
    {
        // Find all TextMeshProUGUI components in the instantiated moleculePrefab
        TextMeshProUGUI[] cubeTexts = moleculeInstance.GetComponentsInChildren<TextMeshProUGUI>();

        // Update the text for each side of the cube
        foreach (TextMeshProUGUI textComponent in cubeTexts)
        {
            // Set the text to the provided formula
            textComponent.text = formula;
        }

        // Check if any text component was found
        if (cubeTexts.Length == 0)
        {
            Debug.LogWarning("TextMeshProUGUI components not found in moleculePrefab.");
        }
    }

    bool IsEmpty()
    {
        Collider[] colliders = Physics.OverlapBox(transform.position, transform.localScale / 2f);

        foreach (Collider collider in colliders)
        {
            if (collider.gameObject.CompareTag("Molecule"))
            {
                return false; // There is a molecule on the carpet
            }
        }

        return true; // No molecule on the carpet
    }

    // Method to destroy all spawned molecules
    void DestroyAllMolecules()
    {
        foreach (GameObject molecule in spawnedMolecules)
        {
            Destroy(molecule);
        }
        // Clear the array
        spawnedMolecules = new GameObject[0];
    }

    // Method to add a spawned molecule to the array
    void AddSpawnedMolecule(GameObject molecule)
    {
        // Resize the array and add the new molecule
        System.Array.Resize(ref spawnedMolecules, spawnedMolecules.Length + 1);
        spawnedMolecules[spawnedMolecules.Length - 1] = molecule;
    }

}
