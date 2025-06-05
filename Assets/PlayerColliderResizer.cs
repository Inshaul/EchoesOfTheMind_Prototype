using UnityEngine;

[RequireComponent(typeof(CapsuleCollider))]
public class PlayerColliderResizer : MonoBehaviour
{
    public Transform cameraTransform;
    private CapsuleCollider capsule;

    public float skinWidth = 0.1f; // extra margin

    void Start()
    {
        capsule = GetComponent<CapsuleCollider>();
    }

    void Update()
    {
        if (cameraTransform == null || capsule == null) return;

        // Get local height of head from rig
        // float headHeight = Mathf.Clamp(cameraTransform.localPosition.y, 1f, 2f);
        float headHeight = Mathf.Max(cameraTransform.localPosition.y, 1f);

        capsule.height = headHeight;

        // Center the capsule to match player height
        capsule.center = new Vector3(cameraTransform.localPosition.x, headHeight / 2f + skinWidth, cameraTransform.localPosition.z);
    }
}
