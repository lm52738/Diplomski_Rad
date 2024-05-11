using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OnDartCollision : MonoBehaviour
{
    public DisplayAtomModel displayAtomModel; // Reference to the DisplayAtomModel script

    private void OnTriggerEnter(Collider other)
    {
        string dartTag = other.tag;
        string collidedLayer = LayerMask.LayerToName(gameObject.layer);

        if (dartTag == "Electron" && collidedLayer == "Electron Cloud")
        {
            Debug.Log("Dart with tag 'Electron' collided with the electron cloud!");

            // Call the UpdateElectronCloud method from the DisplayAtomModel script
            displayAtomModel.UpdateElectronCloud();
        }
        else if ((dartTag == "Neutron" || dartTag == "Proton") && collidedLayer == "Nucleus")
        {
            Debug.Log("Dart with tag '" + dartTag + "' collided with the nucleus!");

            // Call the UpdateNucleus method from the DisplayAtomModel script
            displayAtomModel.UpdateNucleus(dartTag);
        }
    }
}
