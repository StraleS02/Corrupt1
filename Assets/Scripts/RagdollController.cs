using UnityEngine;

public class RagdollController : MonoBehaviour
{
    private Rigidbody[] rigidbodies;
    private Collider[] colliders;
    private Animator animator;
    private LODGroup lodGroup;

    private bool isRagdollActive = false;

    void Start()
    {
        animator = GetComponent<Animator>();

        rigidbodies = GetComponentsInChildren<Rigidbody>(true);
        colliders = GetComponentsInChildren<Collider>(true);
        lodGroup = GetComponent<LODGroup>();

        DisableRagdoll();
    }

    // 🔴 POZIVA SE KAD POGODIŠ NPC
    public void EnableRagdoll(Vector3 hitForce, Vector3 hitPoint)
    {
        SkinnedMeshRenderer[] meshes = GetComponentsInChildren<SkinnedMeshRenderer>();

        foreach (var m in meshes)
        {
            m.updateWhenOffscreen = true;
        }

        if (isRagdollActive) return;
        isRagdollActive = true;

        // isključi animacije
        if (animator != null)
            animator.enabled = false;

        if (lodGroup != null)
            lodGroup.enabled = false;

        // uključi fiziku
        foreach (Rigidbody rb in rigidbodies)
        {
            rb.isKinematic = false;
        }

        // uključi collidere
        foreach (Collider col in colliders)
        {
            col.enabled = true;
        }

        if (TryGetComponent<CapsuleCollider>(out var mainCol))
            mainCol.enabled = false;

        // 🔥 dodaj silu na pogođeni deo
        Rigidbody hitRb = GetClosestRigidbody(hitPoint);
        if (hitRb != null)
        {
            hitRb.AddForceAtPosition(hitForce, hitPoint, ForceMode.Impulse);
        }

        GetComponent<Body>().enabled = true;
    }

    // 🟢 DRŽI NPC “ŽIVIM”
    private void DisableRagdoll()
    {
        foreach (Rigidbody rb in rigidbodies)
        {
            rb.isKinematic = false;
        }

        foreach (Collider col in colliders)
        {
            col.enabled = true;
        }

        // glavni collider ostaje uključen (da možeš da ga pogodiš)
        Collider mainCol = GetComponent<Collider>();
        if (mainCol != null)
            mainCol.enabled = true;
    }

    // 🔍 pronalazi najbliži rigidbody mestu pogotka
    private Rigidbody GetClosestRigidbody(Vector3 point)
    {
        Rigidbody closest = null;
        float minDist = float.MaxValue;

        foreach (Rigidbody rb in rigidbodies)
        {
            float dist = Vector3.Distance(rb.worldCenterOfMass, point);
            if (dist < minDist)
            {
                minDist = dist;
                closest = rb;
            }
        }

        return closest;
    }
}