using UnityEngine;

[RequireComponent(typeof(Camera))]
public class CameraEdgeColliders : MonoBehaviour
{
    [Header("Collider Settings")]
    public float thickness = 1f; // How thick the edge colliders are
    public bool isTrigger = false; // Should the colliders be triggers?

    private Camera cam;

    void Start()
    {
        cam = GetComponent<Camera>();
        CreateEdgeColliders();
    }

    void CreateEdgeColliders()
    {
        float height = 2f * cam.orthographicSize;
        float width = height * cam.aspect;

        Vector2 camPos = cam.transform.position;

        // Left
        CreateEdge("Left Edge", new Vector2(camPos.x - width / 2f - thickness / 2f, camPos.y), new Vector2(thickness, height*2f));

        // Right
        CreateEdge("Right Edge", new Vector2(camPos.x + width / 2f + thickness / 2f, camPos.y), new Vector2(thickness, height*2f));

        // Bottom
        CreateEdge("Bottom Edge", new Vector2(camPos.x, camPos.y - height / 2f - thickness / 2f), new Vector2(width, thickness));
    }

    void CreateEdge(string name, Vector2 position, Vector2 size)
    {
        GameObject edge = new GameObject(name);
        edge.transform.position = position;
        edge.transform.parent = this.transform;

        BoxCollider2D collider = edge.AddComponent<BoxCollider2D>();
        collider.size = size;
        collider.isTrigger = isTrigger;
    }
}