using UnityEngine;

public class EyeTrackingKeyboard : MonoBehaviour
{
    public Transform eyeTransform;
    public float selectionTime = 0.5f;

    private BoxScript? lastHoveredBox = null;

    void Update()
    {
        // Optional: adjust this object to follow eye height (if needed)
        transform.position = new Vector3(
            transform.position.x,
            eyeTransform.position.y - 0.1f,
            transform.position.z
        );

        Ray ray = new Ray(eyeTransform.position, eyeTransform.forward);
        if (Physics.Raycast(ray, out RaycastHit hit, 10))
        {
            // Adjust based on your box hierarchy (assumes collider is on child of the box)
            BoxScript box = hit.transform.GetComponentInParent<BoxScript>();

            if (box != null)
            {
                // New box hit
                if (box != lastHoveredBox)
                {
                    // Reset the old one
                    if (lastHoveredBox != null)
                        lastHoveredBox.ResetBox();

                    // Animate the new one
                    box.AnimateOpen();
                    lastHoveredBox = box;
                }

                return; // Still hovering a valid box
            }
        }

        // If raycast hit nothing or something not a box, reset last
        if (lastHoveredBox != null)
        {
            lastHoveredBox.ResetBox();
            lastHoveredBox = null;
        }
    }
}