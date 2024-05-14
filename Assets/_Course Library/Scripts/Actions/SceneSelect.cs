using UnityEngine;
using UnityEngine.SceneManagement;

/// <summary>
/// Allows selecting scenes based on the name of the GameObject that triggers the selection.
/// </summary>
public class SceneSelect : MonoBehaviour
{
    // Loads the scene based on the name of the GameObject
    public void selectScene()
    {
        switch(this.gameObject.name)
        {
            case "PhScaleButton":
                SceneManager.LoadScene("pH_Scale_Scene");
                break;
            case "AtomStructureButton":
                SceneManager.LoadScene("Atoms_Structure_Scene");
                break;
            case "ReactionsButton":
                SceneManager.LoadScene("Reactions_Scene");
                break;
        }
    }
}
