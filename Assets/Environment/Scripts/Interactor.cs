using UnityEngine;
using UnityEngine.InputSystem;

public interface IInteractable
{
    bool CanInteract { get; }
    void Interact();
}

// Put this on PlayerCapsule and assign the Prompt object.
public class Interactor : MonoBehaviour
{
    [SerializeField] float range = 2f;
    [SerializeField] GameObject prompt;
    [Tooltip("World-space offset from the object's centre, e.g. (0, 0.1, 0) to float above it.")]
    [SerializeField] Vector3 promptOffset = Vector3.zero;

    Camera cam;
    IInteractable current;
    Collider currentCollider;

    void Start()
    {
        cam = Camera.main;
        SetPrompt(false);
    }

    void Update()
    {
        FindTarget();
        SetPrompt(current != null);

        if (current != null && Keyboard.current != null && Keyboard.current.eKey.wasPressedThisFrame)
            current.Interact();
    }

    // Runs after the camera has moved, so the prompt doesn't lag behind the object.
    void LateUpdate()
    {
        if (current == null) return;

        Vector3 worldPos = currentCollider.bounds.center + promptOffset;
        Vector3 screenPos = cam.WorldToScreenPoint(worldPos);

        if (screenPos.z <= 0f) { SetPrompt(false); return; }
        prompt.transform.position = screenPos;
    }

    void FindTarget()
    {
        current = null;
        currentCollider = null;

        var ray = new Ray(cam.transform.position, cam.transform.forward);
        if (!Physics.Raycast(ray, out RaycastHit hit, range, ~0, QueryTriggerInteraction.Ignore))
            return;

        var target = hit.collider.GetComponentInParent<IInteractable>();
        if (target != null && target.CanInteract)
        {
            current = target;
            currentCollider = hit.collider;
        }
    }

    void SetPrompt(bool visible)
    {
        if (prompt != null && prompt.activeSelf != visible) prompt.SetActive(visible);
    }
}