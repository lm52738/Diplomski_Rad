using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Atom 
{
    // Fields
    private string name;
    private string symbol;
    private int protons;
    private int neutrons;
    private int electrons;
    private int atomicMass;
    private double atomicRelativeMass;
    private IonType ionType;
    private string cationText;
    private string anionText;
    private string neutralText;
    private string description;

    // Constructors
    // All-argument constructor
    public Atom(string name, string symbol, int protons, int neutrons, int electrons, int atomicMass, double atomicRelativeMass, IonType ionType, string cationText, string anionText, string neutralText, string description)
    {
        this.name = name;
        this.symbol = symbol;
        this.protons = protons;
        this.neutrons = neutrons;
        this.electrons = electrons;
        this.atomicMass = atomicMass;
        this.atomicRelativeMass = atomicRelativeMass;
        this.ionType = ionType;
        this.cationText = cationText;
        this.anionText = anionText;
        this.neutralText = neutralText;
        this.description = description;
    }

    // No-argument constructor
    public Atom()
    {
        // Initialize default values
        this.name = "";
        this.symbol = "";
        this.protons = 0;
        this.neutrons = 0;
        this.electrons = 0;
        this.atomicMass = 0;
        this.atomicRelativeMass = 0.0;
        this.ionType = IonType.Neutral;
        this.cationText = "";
        this.anionText = "";
        this.neutralText = "";
        this.description = "";
    }

    // Display structure constructor
    public Atom(string name, string symbol, int protons, int neutrons, int electrons)
    {
        this.name = name;
        this.symbol = symbol;
        this.protons = protons;
        this.neutrons = neutrons;
        this.electrons = electrons;
        this.atomicMass = 0;
        this.atomicRelativeMass = 0.0;
        this.ionType = null;
        this.cationText = "";
        this.anionText = "";
        this.neutralText = "";
        this.description = "";
    }


    // Properties with getters and setters
    public string Name
    {
        get { return name; }
        set { name = value; }
    }

    public string Symbol
    {
        get { return symbol; }
        set { symbol = value; }
    }

    public int Protons
    {
        get { return protons; }
        set { protons = value; }
    }

    public int Neutrons
    {
        get { return neutrons; }
        set { neutrons = value; }
    }

    public int Electrons
    {
        get { return electrons; }
        set { electrons = value; }
    }

    public int AtomicMass
    {
        get { return atomicMass; }
        set { atomicMass = value; }
    }

    public double AtomicRelativeMass
    {
        get { return atomicRelativeMass; }
    }

    public IonType IonType
    {
        get { return ionType; }
        set { ionType = value; }
    }

    public string CationText
    {
        get { return cationText; }
        set { cationText = value; }
    }

    public string AnionText
    {
        get { return anionText; }
        set { anionText = value; }
    }

    public string NeutralText
    {
        get { return neutralText; }
        set { neutralText = value; }
    }

    public string Description
    {
        get { return description; }
        set { description = value; }
    }

    public override string ToString()
    {
        return string.Format("Atom: {0} ({1})\n" +
                             "Protons: {2}\n" +
                             "Neutrons: {3}\n" +
                             "Electrons: {4}\n" +
                             "Atomic Mass: {5}\n" +
                             "Atomic Relative Mass: {6}\n" +
                             "Ion Type: {7}\n" +
                             "Cation Text: {8}\n" +
                             "Anion Text: {9}\n" +
                             "Neutral Text: {11}\n" +
                             "Description: {12}",
                             name, symbol, protons, neutrons, electrons, atomicMass, atomicRelativeMass, ionType, cationText, anionText, neutralText, description);
    }
}
