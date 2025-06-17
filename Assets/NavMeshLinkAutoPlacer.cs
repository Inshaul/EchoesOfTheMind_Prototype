using UnityEngine;
using UnityEditor;
using UnityEngine.AI;
using Unity.AI.Navigation;

public class NavMeshLinkAutoPlacer : MonoBehaviour
{
    [MenuItem("Tools/Auto-Place NavMesh Links on Doors")]
    public static void PlaceNavMeshLinks()
    {
        GameObject[] doors = GameObject.FindGameObjectsWithTag("Door");
        int placedCount = 0;

        foreach (GameObject door in doors)
        {
            // Skip if a NavMeshLink already exists on the door
            if (door.GetComponent<NavMeshLink>() != null) continue;

            // Create a NavMeshLink component
            NavMeshLink link = door.AddComponent<NavMeshLink>();
            link.bidirectional = true;

            // Estimate door width — assumes door width is along X axis
            float width = door.transform.localScale.x > 0 ? door.transform.localScale.x : 1f;
            link.width = width;

            // Set link endpoints slightly in front and behind the door
            link.startPoint = new Vector3(-width / 2, 0, 0);
            link.endPoint = new Vector3(width / 2, 0, 0);

            placedCount++;
        }

        Debug.Log($"✅ Placed NavMeshLinks on {placedCount} door(s).");
    }
}
