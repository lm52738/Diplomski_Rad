using System.Collections;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using UnityEngine;

public class Equation
{
    public string FullEquation { get; private set; }
    public string Template { get; private set; }
    public List<(string, int)> Reagents { get; private set; }
    public List<(string, int)> Products { get; private set; }

    public Equation(string fullEquation)
    {
        FullEquation = fullEquation;
        Reagents = new List<(string, int)>();
        Products = new List<(string, int)>();

        ParseEquation();
    }

    private void ParseEquation()
    {
        // Split the equation into reagents and products
        string[] parts = FullEquation.Split('→');
        string reagents = parts[0].Trim();
        string products = parts[1].Trim();

        // Set the template with "0" in front of each molecule
        Template = AddZeroes(reagents) + " → " + AddZeroes(products);

        // Parse reagents
        ParseMolecules(reagents, true);

        // Parse products
        ParseMolecules(products, false);
    }

    private string AddZeroes(string molecules)
    {
        // Use a regular expression to add "0" in front of each molecule
        return Regex.Replace(molecules, @"(?<!<sub>)\b(\d+)?\s*([A-Za-z0-9<sub>\-</sub>]+)\b", "0 $2");

    }

    private void ParseMolecules(string molecules, bool isReagent)
    {
        // Split the molecules string into individual molecules
        string[] moleculeArray = molecules.Split('+');

        // Iterate through each molecule
        foreach (string molecule in moleculeArray)
        {
            // Use regular expressions to extract the count and name of the molecule
            Match match = Regex.Match(molecule.Trim(), @"(\d*)\s*([A-Za-z0-9<sub>\-</sub>]+)");

            if (match.Success)
            {
                // Get the count and name of the molecule
                int count = string.IsNullOrEmpty(match.Groups[1].Value) ? 1 : int.Parse(match.Groups[1].Value);
                string name = match.Groups[2].Value;

                // Add the molecule to the appropriate list (reagents or products)
                if (isReagent)
                    Reagents.Add((name, count));
                else
                    Products.Add((name, count));
            }
        }
    }
}