using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;

/// <summary>
/// Handles the behavior of spilled liquid, including particle effects and floating text display.
/// </summary>
public class LiquidSpill : MonoBehaviour
{
    private XRGrabInteractable grabInteractable;
    private AudioSource playerAudio;
    public ParticleSystem liquidParticles;
    public AudioClip pourSound;
    public GameObject floatingText;

    private bool isGrabbed = false;

    void Start()
    {
        // Get components
        playerAudio = GetComponent<AudioSource>();
        grabInteractable = GetComponent<XRGrabInteractable>();

        // Check if XRGrabInteractable is assigned
        if (grabInteractable == null)
        {
            Debug.LogError("XRGrabInteractable not found on the GameObject.");
            enabled = false;
            return;
        }

        // Subscribe to grab events
        grabInteractable.selectEntered.AddListener(OnGrab);
        grabInteractable.selectExited.AddListener(OnRelease);

        // Show floating text if assigned
        if (floatingText)
        {
            ShowFloatingText();
        }
    }

    void Update()
    {
        if (isGrabbed)
        {
            // Check the controller's rotation to determine if it is leaning towards the floor
            Vector3 controllerRotation = grabInteractable.selectingInteractor.transform.rotation.eulerAngles;

            // Adjust the threshold as needed based on your setup
            if ((controllerRotation.x > 45f && controllerRotation.x < 315)
                    || (controllerRotation.z > 45f && controllerRotation.z < 315))
            {

                // Trigger particles when leaning towards the floor
                if (!liquidParticles.isEmitting)
                {
                    liquidParticles.Play();
                    playerAudio.PlayOneShot(pourSound, 1.0f);
                }
            }
            else
            {
                // Stop particles if not leaning enough
                liquidParticles.Stop();

            }
        }
    }

    // Displays floating text above the flask indicating the type of liquid.
    private void ShowFloatingText()
    {
        // Instantiate the floating text
        GameObject newText = Instantiate(floatingText, transform.position, Quaternion.identity, transform);

        // Get the flask's position and size
        Vector3 flaskPosition = transform.position;
        Vector3 flaskSize = GetComponent<BoxCollider>().bounds.size;

        // Offset the text's position above the flask
        Vector3 textPosition = CalculateTextPosition(flaskPosition, flaskSize);

        // Set the position of the text
        newText.transform.position = textPosition;

        // Get the rotation of the flask
        Quaternion flaskRotation = transform.rotation;

        // Create a new rotation quaternion to rotate the text on the y-axis
        Quaternion textRotation = Quaternion.Euler(0f, 180f, 0f); // Adjust the rotation as needed

        // Combine the flask's rotation with the text rotation
        Quaternion finalRotation = flaskRotation * textRotation;

        // Set the rotation of the text
        newText.transform.rotation = finalRotation;

        // Set the name of the liquid
        var temp = newText.transform.GetChild(0).GetComponent<TextMeshProUGUI>();
        temp.text = liquidParticles.tag.ToString();

    }

    private List<string> flask01LiquidTags = new List<string> { "Ammonia", "Saliva", "Blood", "Soap", "Mouthwash", "Molasses" };
    private List<string> flask05LiquidTags = new List<string> { "Soda", "Coffee", "Bleach" };
    private float yOffset = 0.1f;

    // Calculates the position for the floating text above the flask.
    private Vector3 CalculateTextPosition(Vector3 flaskPosition, Vector3 flaskSize)
    {
        // Check if the liquid particles are assigned
        if (liquidParticles != null)
        {
            string liquidTag = liquidParticles.tag;

            // Check if the liquid tag is in the list of flask 01 liquid tags
            if (flask01LiquidTags.Contains(liquidTag))
            {
                return flaskPosition + Vector3.up * (flaskSize.y / 2f) + Vector3.up * yOffset;
            }
            // Check if the liquid tag is in the list of flask 05 liquid tags
            else if (flask05LiquidTags.Contains(liquidTag))
            {
                return flaskPosition + Vector3.up * (flaskSize.y / 1f) + Vector3.up * yOffset;
            }
            else
            {
                return flaskPosition + Vector3.up * flaskSize.y + Vector3.up * yOffset;
            }
        }

        return flaskPosition + Vector3.up * flaskSize.y + Vector3.up * yOffset;
    }

    // Unsubscribe from events to prevent memory leaks
    void OnDestroy()
    {
        grabInteractable.selectEntered.RemoveListener(OnGrab);
        grabInteractable.selectExited.RemoveListener(OnRelease);
    }

    // Event triggered when the flask is released
    void OnRelease(SelectExitEventArgs arg0)
    {
        isGrabbed = false;
        liquidParticles.Stop(); // Stop particles when releasing the grab
    }

    // Event triggered when the flask is grabbed
    void OnGrab(SelectEnterEventArgs arg0)
    {
        isGrabbed = true;
    }

    
}