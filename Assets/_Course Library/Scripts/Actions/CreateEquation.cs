using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;
using TMPro;
using UnityEngine;

/// <summary>
/// This class handles the creation and management of chemical equations.
/// </summary>
public class CreateEquation : MonoBehaviour
{
    public TextMeshProUGUI equationText;
    public GameObject[] reagentsMats; // Array of reagents mats
    public GameObject[] productsMats; // Array of products mats
    public GameObject reagentsBox;
    public GameObject productsBox; 
    private int currentLevel = 0;
    private Equation[] equations; // Array of Equation objects
    private string currentEquationText = "";

    public List<(string, int)> Reagents { get; private set; }
    public List<(string, int)> Products { get; private set; }

    void Start()
    {
        InitializeEquations();
        NextLevel();
    }

    public Equation GetCurrentEquation() {
        if (currentLevel <= equations.Length)
        {
            return equations[currentLevel - 1];
        }

        return null;
    }

    public string GetEquationText()
    {
        //Debug.Log("GetEquationText: " + currentEquationText);
        return currentEquationText;
    }

    // Moves to the next level
    public void NextLevel()
    {
        currentLevel++;
        if (currentLevel <= equations.Length)
        {
            Equation currentEquation = equations[currentLevel - 1];

            ShowEquation(currentEquation.Template, currentEquation);
            // Debug.Log(equationText.text);

            DistributeMolecules(reagentsMats, currentEquation.Reagents, "Reagents");
            DistributeMolecules(productsMats, currentEquation.Products, "Products");
        }
        else
        {
            currentEquationText = "";
            equationText.text = currentEquationText;
        }
    }

    // Initializes equations from a file
    void InitializeEquations()
    {
        equations = new Equation[]
        {
            new Equation("1 CH<sub>4</sub> + 2 O<sub>2</sub> → 1 CO<sub>2</sub> + 2 H<sub>2</sub>O"),
            new Equation("4 Cu + 1 O<sub>2</sub> → 2 Cu<sub>2</sub>O"),
            new Equation("1 N<sub>2</sub> + 3 H<sub>2</sub> → 2 NH<sub>3</sub>"),
            new Equation("4 Al + 3 O<sub>2</sub> → 2 Al<sub>2</sub>O<sub>3</sub>"),
            new Equation("2 H<sub>2</sub>O → 2 H<sub>2</sub> + 1 O<sub>2</sub>")
        };
    }

    // Displays the equation
    void ShowEquation(string text, Equation equation)
    {
        currentEquationText = text;

        // Split the text into reagents and products
        string[] parts = text.Split('→');
        string reagentsText = parts[0].Trim();
        string productsText = parts[1].Trim();

        // Format reagents
        string formattedReagents = FormatMolecules(reagentsText, equation.Reagents);

        // Format products
        string formattedProducts = FormatMolecules(productsText, equation.Products);

        // Combine reagents and products
        string formattedText = formattedReagents + " → " + formattedProducts;

        // Set the formatted text to the equation text
        equationText.text = formattedText;
    }

    // Formats molecules in the equation
    string FormatMolecules(string moleculesText, List<(string, int)> molecules)
    {
        string formattedText = "";

        // Split the molecules text into individual molecules
        string[] moleculeArray = moleculesText.Split('+');

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

                // Determine the correct count of the molecule from the Equation object
                int correctCount = 0;
                foreach (var mol in molecules)
                {
                    if (mol.Item1 == name)
                    {
                        correctCount = mol.Item2;
                        break;
                    }
                }

                // Determine the color based on the correctness of the count
                string countColor = count == 0 ? "<color=red>" : (count == correctCount ? "<color=green>" : "<color=yellow>");

                // Add the formatted molecule to the text
                formattedText += $"{countColor}{count}</color> {name}";

                // Add the '+' symbol if it's not the last molecule
                if (molecule != moleculeArray.Last())
                {
                    formattedText += " + ";
                }
            }
        }

        // Return the formatted text
        return formattedText;
    }

    // Distributes molecules to mats
    void DistributeMolecules(GameObject[] mats, List<(string, int)> molecules, string matTagPrefix)
    {
        int matIndex = 1;
        foreach ((string molecule, int count) in molecules)
        {
            string matTag = matTagPrefix + matIndex.ToString();
            GameObject mat = FindMatWithTag(mats, matTag);

            if (mat != null)
            {
                AssignMoleculeToMat(mat, molecule);
            }

            matIndex++;
        }

        while (matIndex <= mats.Length)
        {
            string matTag = matTagPrefix + matIndex.ToString();
            GameObject mat = FindMatWithTag(mats, matTag);

            if (mat != null)
            {
                AssignMoleculeToMat(mat, "");
            }

            matIndex++;
        }
    }

    // Finds mat with given tag
    GameObject FindMatWithTag(GameObject[] mats, string tag)
    {
        foreach (GameObject mat in mats)
        {
            if (mat.CompareTag(tag))
            {
                return mat;
            }
        }
        return null;
    }

    // Assigns molecule to mat
    void AssignMoleculeToMat(GameObject mat, string moleculeFormula)
    {
        CreateMolecule createMolecule = mat.GetComponentInChildren<CreateMolecule>();
        if (createMolecule != null)
        {
            createMolecule.SetMoleculeFormula(moleculeFormula);
        }
        else
        {
            Debug.LogWarning("CreateMolecule script not found on " + mat.name);
        }
    }

    // Updates the equation based on current counts
    public void UpdateEquation()
    {
        // Get the list of molecules inside the reagents box
        MoleculeCounter reagentsCounter = reagentsBox.GetComponent<MoleculeCounter>();
        List<GameObject> reagents = reagentsCounter.GetMoleculeCount();

        // Count the occurrences of each molecule type in reagents
        Dictionary<string, int> reagentsCounts = CountMolecules(reagents);

        // Get the list of molecules inside the products box
        MoleculeCounter productsCounter = productsBox.GetComponent<MoleculeCounter>();
        List<GameObject> products = productsCounter.GetMoleculeCount();

        // Count the occurrences of each molecule type in products
        Dictionary<string, int> productsCounts = CountMolecules(products);

        // Update the equation text with the counts of the molecules
        Equation currentEquation = equations[currentLevel - 1];
        string updatedEquation = currentEquation.Template;

        foreach ((string moleculeType, int count) in currentEquation.Reagents)
        {
            int reagentCount = reagentsCounts.ContainsKey(moleculeType) ? reagentsCounts[moleculeType] : 0;
            //Debug.Log("Updated molecule count for " + moleculeType + " on Reagents side from to " + reagentCount);

            if (reagentCount > 0)
            {
                updatedEquation = UpdateMoleculeCount(updatedEquation, moleculeType, reagentCount, count, "Reagents");
            }
        }

        foreach ((string moleculeType, int count) in currentEquation.Products)
        {
            int productCount = productsCounts.ContainsKey(moleculeType) ? productsCounts[moleculeType] : 0;
            //Debug.Log("Updated molecule count for " + moleculeType + " on Reagents side from to " + productCount);

            if (productCount > 0)
            {
                updatedEquation = UpdateMoleculeCount(updatedEquation, moleculeType, productCount, count, "Products");
            }
            
            
        }

        // Update the equation text
        //equationText.text = updatedEquation;
        ShowEquation(updatedEquation, currentEquation);
    }

    // Counts molecules
    private Dictionary<string, int> CountMolecules(List<GameObject> molecules)
    {
        Dictionary<string, int> moleculeCounts = new Dictionary<string, int>();

        foreach (var molecule in molecules)
        {
            string moleculeType = molecule.GetComponentInChildren<TextMeshProUGUI>().text;

            if (moleculeCounts.ContainsKey(moleculeType))
            {
                moleculeCounts[moleculeType]++;
            }
            else
            {
                moleculeCounts[moleculeType] = 1;
            }
        }

        return moleculeCounts;
    }

    // Updates molecule count
    private string UpdateMoleculeCount(string equation, string moleculeType, int count, int finalCount, string side)
    {
        //Debug.Log(equation + " -> " + side + ", " + moleculeType + " -> " + count);

        string target = @"\b0\s*" + Regex.Escape(moleculeType); // Pattern for reagents
        if (side == "Products")
            target = @"\b0\s*" + Regex.Escape(moleculeType); // Pattern for products

        string pattern = @"((\d+)\s*)?" + target + @"(?![^<]*>)";

        Match match = Regex.Match(equation, pattern);

        if (match.Success)
        {
            int newCount = count; // Just set the new count directly

            //Debug.Log("Updated molecule count for " + moleculeType + " on " + side + " side from " + existingCount + " to " + newCount);

            return Regex.Replace(equation, pattern, newCount.ToString() + " " + moleculeType);
        }
        else
        {
            // Log that no match was found
            Debug.LogWarning("No match found for molecule type " + moleculeType + " on " + side + " side");

            return equation;
        }
    }
}