using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneSelect : MonoBehaviour
{
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
