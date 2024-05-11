using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IonType
{
    // Fields
    public int Id { get; set; }
    public string Type { get; set; }

    // Constructors
    // All-argument constructor
    public IonType(int id, string type)
    {
        this.Id = id;
        this.Type = type;
    }

    // No-argument constructor
    public IonType()
    {
        // Initialize default values
        this.Id = 0;
        this.Type = "";
    }

    // Constants for ion types
    public static readonly IonType Cation = new IonType(1, "Cation");
    public static readonly IonType Anion = new IonType(2, "Anion");
    public static readonly IonType Neutral = new IonType(3, "Neutral");
}