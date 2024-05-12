using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class DisplayAtomInfo : MonoBehaviour
{
    public TMP_Text elementNameText;
    public TMP_Text subatomicParticlesText;
    public TMP_Text atomicMassIonText;
    public TMP_Text descriptionText;

    private List<Atom> atoms = new List<Atom>();
    private Atom activeAtom;

    // Start is called before the first frame update
    void Start()
    {
    
    }

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

    private void ResetAtoms()
    {
        atoms.Clear();

        // TODO: izdvoji u poseban text file
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
