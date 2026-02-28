using UnityEngine;
using UnityEngine.Tilemaps;

public class CameraFollow : MonoBehaviour
{
    public Transform target;
    public Tilemap tilemap; 

    private float minX, maxX, minY, maxY;
    private float camHalfWidth, camHalfHeight;
    private Camera cam;

    public Vector2 cameraLimitsMin => new Vector2(minX, minY);
    public Vector2 cameraLimitsMax => new Vector2(maxX, maxY);

    void Start()
    {
        cam = Camera.main;
        Camera.main.orthographicSize = 5f;

        camHalfHeight = cam.orthographicSize;
        camHalfWidth = camHalfHeight * ((float)Screen.width / Screen.height);

        BoundsInt cellBounds = tilemap.cellBounds;
        Vector3 min = tilemap.CellToWorld(cellBounds.min);
        Vector3 max = tilemap.CellToWorld(cellBounds.max);

        minX = min.x + camHalfWidth;
        maxX = max.x - camHalfWidth;
        minY = min.y + camHalfHeight;
        maxY = max.y - camHalfHeight;
    }

    void LateUpdate()
    {
        target = GameObject.FindGameObjectWithTag("Player").transform;

        if (target == null)
        {
            GameObject playerObj = GameObject.FindGameObjectWithTag("Player");
            if (playerObj != null)
                target = playerObj.transform;
        }

        if (target == null) return;

        float targetX = Mathf.Clamp(target.position.x, minX, maxX);
        float targetY = Mathf.Clamp(target.position.y, minY, maxY);

        transform.position = new Vector3(targetX, targetY, transform.position.z);
    }

}
