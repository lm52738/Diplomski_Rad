using System.Collections.Generic;
using System.IO;
using TMPro;
using UnityEngine;

/// <summary>
/// Manages the display of atom models and information.
/// </summary>
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

    // Method to display atom model
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

    // Creates the nucleus with the specified number of subatoms using the given prefab
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

    // Generates a random position within the nucleus range
    private Vector3 RandomPositionInNucleus(float radius)
    {
        // Generate random position within the nucleus range
        float randomX = Random.Range(-radius, radius);
        float randomY = Random.Range(-radius, radius);
        return new Vector3(randomX, randomY, 0);
    }

    // Creates the electron cloud with the specified number of electrons
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

    // Updates the electron cloud size
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


    // Updates the nucleus with the addition of a subatom (neutron or proton)
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

    // Removes all displayed prefabs and deactivates the electron cloud
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

    // Resets the list of atoms by reading from file
    private void ResetAtoms()
    {
        atoms.Clear();

        // Initialize Atom objects for each element in neutral condition
        atoms.Add(new Atom("Hydrogen", "H", 1, 0, 1, 1, 1.008, IonType.Neutral,
            "With fewer electrons than protons, hydrogen acts as a potent reducing agent, donating its lone electron in chemical reactions. This property is crucial in industrial and biological redox processes.",
            "If electrons outnumber protons, hydrogen exhibits unusual behavior, seen in high-energy plasmas or astrophysical conditions, leading to unexpected reactivity and compound formation.",
            "Hydrogen is the lightest element.",
            "Hydrogen is the lightest element."));
        atoms.Add(new Atom("Helium", "He", 2, 2, 2, 4, 4.003, IonType.Neutral, "With fewer electrons than protons, helium exhibits unique behavior in extreme conditions like stellar cores, contributing to nuclear fusion and star luminosity.",
            "If electrons outnumber protons, helium ions display unexpected properties in high-energy physics experiments, aiding in fundamental particle research.",
            "Helium is a noble gas with very low reactivity.",
            "Helium is a noble gas with very low reactivity."));
        atoms.Add(new Atom("Lithium", "Li", 3, 4, 3, 7, 6.941, IonType.Neutral, "With fewer electrons than protons, lithium powers lithium-ion batteries, revolutionizing electronics and energy storage.",
            "If electrons outnumber protons, lithium ions exhibit enhanced reactivity in extreme environments, impacting fusion reactions and astrophysical phenomena.",
            "Lithium is used in rechargeable batteries.",
            "Lithium is used in rechargeable batteries."));
        atoms.Add(new Atom("Beryllium", "Be", 4, 5, 4, 9, 9.012, IonType.Neutral, "With fewer electrons than protons, beryllium excels in aerospace and nuclear applications due to its strength and low neutron absorption.",
            "If electrons outnumber protons, beryllium ions show unique properties in plasma physics and high-energy environments, aiding in advanced research.",
            "Beryllium is used in aerospace and medical applications.",
            "Beryllium is used in aerospace and medical applications."));
        atoms.Add(new Atom("Boron", "B", 5, 6, 5, 11, 10.81, IonType.Neutral, "With fewer electrons than protons, boron is utilized in semiconductors and as a neutron absorber in nuclear reactors.",
            "If electrons outnumber protons, boron ions exhibit unique characteristics in plasma physics and materials science.",
            "Boron is used in the production of glass and ceramics.",
            "Boron is used in the production of glass and ceramics."));
        atoms.Add(new Atom("Carbon", "C", 6, 6, 6, 12, 12.01, IonType.Neutral, "With fewer electrons than protons, carbon forms the basis of organic chemistry and life as a building block of molecules.",
            "If electrons outnumber protons, carbon ions display unique behavior in high-energy environments, contributing to astrophysical processes.",
            "Carbon is the basis of all organic molecules.",
            "Carbon is the basis of all organic molecules."));
        atoms.Add(new Atom("Nitrogen", "N", 7, 7, 7, 14, 14.01, IonType.Neutral, "With fewer electrons than protons, nitrogen is vital for life as a key component of proteins and DNA.",
            "If electrons outnumber protons, nitrogen ions exhibit unique behavior in plasma physics and astrophysical contexts.",
            "Nitrogen makes up about 78% of the Earth's atmosphere.",
            "Nitrogen makes up about 78% of the Earth's atmosphere."));
        atoms.Add(new Atom("Oxygen", "O", 8, 8, 8, 16, 16.00, IonType.Neutral, "With fewer electrons than protons, oxygen is essential for respiration and combustion, supporting life and energy production.",
            "If electrons outnumber protons, oxygen ions display unique properties in plasma physics and high-energy environments.",
            "Oxygen is essential for respiration and combustion.",
            "Oxygen is essential for respiration and combustion."));
        atoms.Add(new Atom("Fluorine", "F", 9, 10, 9, 19, 19.00, IonType.Neutral, "With fewer electrons than protons, fluorine is a highly reactive element used in various industrial processes and as a component in pharmaceuticals.",
            "If electrons outnumber protons, fluorine ions exhibit distinctive behavior in chemical reactions and can form stable compounds with unusual properties.",
            "Fluorine is the most reactive non-metal.",
            "Fluorine is the most reactive non-metal."));
        atoms.Add(new Atom("Neon", "Ne", 10, 10, 10, 20, 20.18, IonType.Neutral, "With fewer electrons than protons, neon is known for its inertness and use in neon lights and advertising signs.",
            "If electrons outnumber protons, neon ions show unique behavior in plasma physics and contribute to understanding high-energy phenomena.",
            "Neon is used in neon signs for lighting.",
            "Neon is used in neon signs for lighting."));
        atoms.Add(new Atom("Sodium", "Na", 11, 12, 11, 23, 22.99, IonType.Neutral, "With fewer electrons than protons, sodium is highly reactive, essential for bodily functions and widely used in industry.",
            "If electrons outnumber protons, sodium ions display unique behavior in chemical reactions, influencing various processes.",
            "Sodium is a highly reactive metal.",
            "Sodium is a highly reactive metal."));
        atoms.Add(new Atom("Magnesium", "Mg", 12, 12, 12, 24, 24.31, IonType.Neutral, "With fewer electrons than protons, magnesium is crucial for biological functions and prevalent in structural materials.",
            "If electrons outnumber protons, magnesium ions exhibit distinctive properties in chemical reactions, influencing various processes.",
            "Magnesium is used in alloys and as a supplement.",
            "Magnesium is used in alloys and as a supplement."));
        atoms.Add(new Atom("Aluminum", "Al", 13, 14, 13, 27, 26.98, IonType.Neutral, "With fewer electrons than protons, aluminum is lightweight and widely used in construction and transportation.",
            "If electrons outnumber protons, aluminum ions display unique properties in chemical reactions, impacting industrial processes.",
            "Aluminum is lightweight and corrosion-resistant.",
            "Aluminum is lightweight and corrosion-resistant."));
        atoms.Add(new Atom("Silicon", "Si", 14, 14, 14, 28, 28.09, IonType.Neutral, "With fewer electrons than protons, silicon is essential in electronics and solar technology.",
            "If electrons outnumber protons, silicon ions exhibit unique behavior in semiconductor applications, influencing device performance.",
            "Silicon is used in electronics and solar panels.",
            "Silicon is used in electronics and solar panels."));
        atoms.Add(new Atom("Phosphorus", "P", 15, 16, 15, 31, 30.97, IonType.Neutral, "With fewer electrons than protons, phosphorus is vital in biological processes and agriculture.",
            "If electrons outnumber protons, phosphorus ions exhibit unique behavior in chemical reactions, influencing environmental and industrial processes.",
            "Phosphorus is essential for life and found in DNA.",
            "Phosphorus is essential for life and found in DNA."));
        atoms.Add(new Atom("Sulfur", "S", 16, 16, 16, 32, 32.07, IonType.Neutral, "With fewer electrons than protons, sulfur is crucial in various industrial processes and biochemical reactions.",
            "If electrons outnumber protons, sulfur ions display unique behavior in chemical reactions, affecting environmental and material properties.",
            "Sulfur is used in the production of sulfuric acid.",
            "Sulfur is used in the production of sulfuric acid."));
        atoms.Add(new Atom("Chlorine", "Cl", 17, 18, 17, 35, 35.45, IonType.Neutral, "With fewer electrons than protons, chlorine is widely used in water purification and as a disinfectant.",
            "If electrons outnumber protons, chlorine ions exhibit unique behavior in chemical reactions, influencing industrial processes and environmental chemistry.",
            "Chlorine is used in water treatment and as a disinfectant.",
            "Chlorine is used in water treatment and as a disinfectant."));
        atoms.Add(new Atom("Argon", "Ar", 18, 22, 18, 40, 39.95, IonType.Neutral, "With fewer electrons than protons, argon is inert and commonly used in welding and lighting.",
            "If electrons outnumber protons, argon ions show unique behavior in plasma physics, contributing to diverse research fields.",
            "Argon is a noble gas and inert.",
            "Argon is a noble gas and inert."));

    }
}