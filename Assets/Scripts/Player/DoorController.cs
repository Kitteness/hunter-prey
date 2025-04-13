using UnityEngine;
using UnityEngine.AI;
using Unity.AI.Navigation;

public class DoorController : MonoBehaviour
{
    [Header("Direct Control")]
    public bool useDirectOpen = false;
    public Vector3 doorOpenOffset = new Vector3(0, 2f, 0);
    private bool isOpen = false;
    private Vector3 initialPosition;
    private Collider doorCollider;

    [Header("NavMesh Rebuild (Optional)")]
    public NavMeshSurface navMeshSurfaceToRebuild;

    void Start()
    {
        doorCollider = GetComponent<Collider>();
        initialPosition = transform.position;
    }

    public void OpenDoor()
    {
        if (isOpen) return;

        isOpen = true;

        if (useDirectOpen)
        {
            transform.position = initialPosition + doorOpenOffset;
            if (doorCollider != null)
                doorCollider.enabled = false;
        }

        if (navMeshSurfaceToRebuild != null)
        {
            navMeshSurfaceToRebuild.BuildNavMesh();
            Debug.Log("NavMesh rebuilt after door opened.");
        }
    }
}
