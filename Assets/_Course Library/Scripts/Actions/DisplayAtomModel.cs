using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEditor;
using UnityEngine;
using UnityEngine.UIElements;

public class DisplayAtomModel : MonoBehaviour
{
    public TMP_Text elementNameText;
    public GameObject neutronPrefab; // Prefab for neutron (white sphere)
    public GameObject protonPrefab;  // Prefab for proton (red sphere)
    public GameObject electronCloud; // Electron cloud (blue sphere)
    public GameObject nucleus; // Nucleus of atom (glass sphere)
    public DisplayAtomInfo displayAtomInfo; // Reference to the DisplayAtomInfo script
    public AudioClip radiusChangeSound;

    private List<Atom> atoms = new List<Atom>();
    private List<GameObject> displayed = new List<GameObject>();
    private float previousRadius = 0f;
    private Atom activeAtom;
    private AudioSource audioSource;
    
    private static float nucleusRadius = 0.25f;


    // Start is called before the first frame update
    void Start()
    {
        audioSource = GetComponent<AudioSource>();
        audioSource.clip = radiusChangeSound;

    }


    public void DisplayAtom(string symbol)
    {
        RemoveDisplayed();
        ResetAtoms();

        // Find the atom with the given symbol
        Atom atom = atoms.Find(a => a.Symbol == symbol);

        if (atom == null)
        {
            return;
        }

        activeAtom = atom;

        // Display element name and symbol
        elementNameText.text = atom.Name + " - " + atom.Symbol;

        // Create nucleus with protons
        CreateNucleus(atom.Protons, protonPrefab);

        // Create nucleus with neutrons
        CreateNucleus(atom.Neutrons, neutronPrefab);

        // Create electron cloud
        CreateElectronCloud(atom.Electrons);
    }

    private void CreateNucleus(int numberOfSubAtoms, GameObject subAtomPrefab)
    {
        // If there's only one subatom, position it at the center of the nucleus
        if (numberOfSubAtoms == 1)
        {
            float offset = subAtomPrefab.Equals(protonPrefab) ? -0.05f : 0.05f;
            Vector3 position = new Vector3(offset, 0, 0);
            GameObject subAtom = Instantiate(subAtomPrefab, nucleus.transform);
            subAtom.transform.localPosition = position;
            subAtom.transform.localScale = new Vector3(0.25f, 0.25f, 0.25f);
            return;
        }

        // Instantiate subatoms
        for (int i = 0; i < numberOfSubAtoms; i++)
        {
            // Generate random position within the nucleus range
            Vector3 position = RandomPositionInNucleus(nucleusRadius);

            // Instantiate subatom
            GameObject subAtom = Instantiate(subAtomPrefab, nucleus.transform);
            subAtom.transform.localPosition = position;
            subAtom.transform.localScale = new Vector3(0.25f, 0.25f, 0.25f);
            displayed.Add(subAtom);
        }
    }

    private Vector3 RandomPositionInNucleus(float radius)
    {
        // Generate random position within the nucleus range
        float randomX = Random.Range(-radius, radius);
        float randomY = Random.Range(-radius, radius);
        return new Vector3(randomX, randomY, 0);
    }

    private void CreateElectronCloud(int totalElectrons)
    {

        // Calculate the radius of the electron cloud sphere based on the number of shells (using total electrons)
        float electronCloudRadius = 0f;

        if (totalElectrons <= 2)
        {
            electronCloudRadius = 55f;
        }
        else if (totalElectrons <= 10)
        {
            electronCloudRadius = 65f;
        }
        else if (totalElectrons <= 28)
        {
            electronCloudRadius = 75f;
        }
        else
        {
            Debug.LogWarning("Number of electrons exceeds supported range for the Bohr model.");
        }

        previousRadius = electronCloudRadius;

        // Instantiate electron cloud sphere with calculated radius
        electronCloud.transform.localScale = new Vector3(electronCloudRadius, electronCloudRadius, 2.5f);
        electronCloud.SetActive(true);

    }

    public void UpdateElectronCloud()
    {
        // Add one electron to the atom object
        activeAtom.Electrons++;

        // Calculate the new radius of the electron cloud sphere based on the updated number of electrons
        float electronCloudRadius = 0f;

        if (activeAtom.Electrons <= 2)
        {
            electronCloudRadius = 55f;
        }
        else if (activeAtom.Electrons <= 10)
        {
            electronCloudRadius = 65f;
        }
        else if (activeAtom.Electrons <= 28)
        {
            electronCloudRadius = 75f;
        }
        else
        {
            Debug.LogWarning("Number of electrons exceeds supported range for the Bohr model.");
            return; // Do not update electron cloud if the number of electrons exceeds the supported range
        }

        // Update the scale of the electron cloud sphere
        //electronCloud.transform.localScale = new Vector3(electronCloudRadius, electronCloudRadius, 2.5f);

        // Check if the radius has changed
        if (previousRadius != electronCloudRadius)
        {
            // Update the scale of the electron cloud sphere
            electronCloud.transform.localScale = new Vector3(electronCloudRadius, electronCloudRadius, 2.5f);

            // Play sound since the radius has changed
            audioSource.PlayOneShot(radiusChangeSound, 1);

            // Update the previous radius
            previousRadius = electronCloudRadius;
        }

        displayAtomInfo.UpdateDisplayedAtom(activeAtom);
    }

    public void UpdateNucleus(string subAtomTag)
    {
        if (subAtomTag == "Neutron")
        {
            // Increase the number of neutrons in the atom
            activeAtom.Neutrons++;

            // Instantiate neutron prefab and add it to the nucleus
            GameObject neutron = Instantiate(neutronPrefab, nucleus.transform);
            neutron.transform.localPosition = RandomPositionInNucleus(nucleusRadius); // Position within nucleus range
            neutron.transform.localScale = new Vector3(0.25f, 0.25f, 0.25f);
            displayed.Add(neutron);
        }
        else if (subAtomTag == "Proton")
        {
            // Increase the number of protons in the atom
            activeAtom.Protons++;

            // Instantiate proton prefab and add it to the nucleus
            GameObject proton = Instantiate(protonPrefab, nucleus.transform);
            proton.transform.localPosition = RandomPositionInNucleus(nucleusRadius); // Position within nucleus range
            proton.transform.localScale = new Vector3(0.25f, 0.25f, 0.25f);
            displayed.Add(proton);
        }

        displayAtomInfo.UpdateDisplayedAtom(activeAtom);

    }

    public void RemoveDisplayed()
    {
        // Destroy all displayed prefabs
        foreach (GameObject obj in displayed)
        {
            Destroy(obj);
        }

        // Clear the displayed list
        displayed.Clear();

        // Deactivate electron cloud
        electronCloud.SetActive(false);
    }

    private void ResetAtoms()
    {
        atoms.Clear();

        // Initialize Atom objects for each element in neutral condition
        atoms.Add(new Atom("Hydrogen", "H", 1, 0, 1));
        atoms.Add(new Atom("Helium", "He", 2, 2, 2));
        atoms.Add(new Atom("Lithium", "Li", 3, 4, 3));
        atoms.Add(new Atom("Beryllium", "Be", 4, 5, 4));
        atoms.Add(new Atom("Boron", "B", 5, 6, 5));
        atoms.Add(new Atom("Carbon", "C", 6, 6, 6));
        atoms.Add(new Atom("Nitrogen", "N", 7, 7, 7));
        atoms.Add(new Atom("Oxygen", "O", 8, 8, 8));
        atoms.Add(new Atom("Fluorine", "F", 9, 10, 9));
        atoms.Add(new Atom("Neon", "Ne", 10, 10, 10));
        atoms.Add(new Atom("Sodium", "Na", 11, 12, 11));
        atoms.Add(new Atom("Magnesium", "Mg", 12, 12, 12));
        atoms.Add(new Atom("Aluminum", "Al", 13, 14, 13));
        atoms.Add(new Atom("Silicon", "Si", 14, 14, 14));
        atoms.Add(new Atom("Phosphorus", "P", 15, 16, 15));
        atoms.Add(new Atom("Sulfur", "S", 16, 16, 16));
        atoms.Add(new Atom("Chlorine", "Cl", 17, 18, 17));
        atoms.Add(new Atom("Argon", "Ar", 18, 22, 18));
    }
}