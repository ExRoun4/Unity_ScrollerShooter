using UnityEngine;
using UnityEditor;

[ExecuteAlways]
public class PlayerVisualizer : MonoBehaviour
{
    public Camera cameraForProportions;

    private void Update()
    {
        transform.position = new Vector3(0.0f, 0.0f, Mathf.Max(transform.position.z, 0.0f));
        transform.rotation = Quaternion.identity;
    }
}

public class PlayerVisualizerGizmos{
    [DrawGizmo(GizmoType.Selected | GizmoType.NonSelected)]
    public static void DrawGizmos(PlayerVisualizer playerVisualizer, GizmoType gizmoType){
        // DRAW CAMERA CORNERS
        Camera cam = playerVisualizer.cameraForProportions;

        Vector3 upperLeftCorner = cam.ViewportToWorldPoint(new Vector3(0, 1, cam.transform.localPosition.y));
        Vector3 upperRightCorner = cam.ViewportToWorldPoint(new Vector3(1, 1, cam.transform.localPosition.y));
        Vector3 lowerLeftCorner = cam.ViewportToWorldPoint(new Vector3(1, 0, cam.transform.localPosition.y));
        Vector3 lowerRightCorner = cam.ViewportToWorldPoint(new Vector3(0, 0, cam.transform.localPosition.y));
        Vector3 up = Vector3.up * cam.transform.localPosition.y;
        
        Vector3[] points = {
            upperLeftCorner, upperRightCorner, lowerRightCorner, lowerLeftCorner, 
            upperLeftCorner, lowerRightCorner, upperRightCorner, lowerLeftCorner,
            upperLeftCorner + up, upperRightCorner + up, lowerRightCorner + up, lowerLeftCorner + up, 
            upperLeftCorner + up, lowerRightCorner + up, upperRightCorner + up, lowerLeftCorner + up,
            upperLeftCorner, upperLeftCorner + up, upperRightCorner, upperRightCorner + up,
            lowerLeftCorner, lowerLeftCorner + up, lowerRightCorner, lowerRightCorner + up,
        };

        Gizmos.color = Color.blue;
        Gizmos.DrawLineList(points);
    }
}
