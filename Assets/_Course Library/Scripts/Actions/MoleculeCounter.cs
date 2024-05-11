using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class MoleculeCounter : MonoBehaviour
{
    public GameObject canvasWithCreateEquation;
    private CreateEquation createEquationScript;
    private List<GameObject> moleculesInside; // List to store molecules inside the box
    private List<int> collidedMoleculeIDs = new List<int>(); // List to store IDs of collided molecules
    private int boxLayer; // Layer of the box collider
    private AudioSource audioSource;
    public AudioClip collisionSound;

    private void Start()
    {
        moleculesInside = new List<GameObject>();

        // Get the layer of the box collider
        boxLayer = gameObject.layer;

        // Make sure the canvas with CreateEquation script is active
        canvasWithCreateEquation.SetActive(true);

        // Get the CreateEquation script from the canvas
        createEquationScript = canvasWithCreateEquation.GetComponent<CreateEquation>();

        audioSource = GetComponent<AudioSource>();
        audioSource.clip = collisionSound;
    }


    private void OnCollisionEnter(Collision collision)
    {
        GameObject collidedObject = collision.gameObject;

        // Check if the collided object is a molecule and has the same layer as the box
        if (collidedObject.CompareTag("Molecule") && collidedObject.layer == boxLayer)
        {
            audioSource.PlayOneShot(collisionSound, 1);
            Molecule molecule = collidedObject.GetComponent<Molecule>();
            int moleculeID = molecule.MoleculeID;

            // Check if the molecule with this ID has already collided
            if (!collidedMoleculeIDs.Contains(moleculeID))
            {
                // Add the molecule ID to the list
                collidedMoleculeIDs.Add(moleculeID);

                // Add the molecule to the list and log
                moleculesInside.Add(collidedObject);

                LogBoxContents();

                createEquationScript.UpdateEquation();
            }             
        }
        else
        {
            // Remove the collided object if it doesn't match the layer
            Destroy(collidedObject);
        }
    }

    private void OnCollisionExit(Collision collision)
    {
        GameObject collidedObject = collision.gameObject;

        // Check if the collided object is a molecule and has the same layer as the box
        if (collidedObject.CompareTag("Molecule") && collidedObject.layer == boxLayer)
        {
            // Remove the molecule from the list and log
            moleculesInside.Remove(collidedObject);

            Molecule molecule = collidedObject.GetComponent<Molecule>();
            int moleculeID = molecule.MoleculeID;

            // Remove the molecule ID from the list
            collidedMoleculeIDs.Remove(moleculeID);

            LogBoxContents();
            createEquationScript.UpdateEquation();
        }
    }

    private void LogBoxContents()
    {
        //Debug.Log("Contents of the box " + LayerMask.LayerToName(boxLayer) + ": ");

        foreach (GameObject molecule in moleculesInside)
        {
            // Retrieve the molecule formula from the collided molecule GameObject
            string moleculeFormula = molecule.GetComponentInChildren<TextMeshProUGUI>().text;
            //Debug.Log("- " + moleculeFormula);
        }
    }

    public List<GameObject> GetMoleculeCount()
    {
        return moleculesInside;
    }
}