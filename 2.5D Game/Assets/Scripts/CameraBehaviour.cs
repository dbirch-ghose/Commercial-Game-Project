using UnityEngine;


public class CameraBehaviour : MonoBehaviour
{

    public Transform target;
    public float smoothSpeed = 0.125f;
    public Vector3 offset;

    //cam switching
    public Camera cam;
    public Transform position;

    public void MoveCamera(Transform CamPos)
    {
        if (cam == null || CamPos == null) return;

        cam.transform.position = CamPos.position;
    }

    //room visibility
    public void ShowRoom(int roomLayer)
    {
        cam.cullingMask = 1 << roomLayer;
    }


    private void LateUpdate()
    {
        if (target != null)
        {
            Vector3 desiredPosition = target.position + offset;
            Vector3 smoothedPosition = Vector3.Lerp(transform.position, desiredPosition, smoothSpeed);
            transform.position = desiredPosition;

            // Optional: make the camera look at the target
            transform.LookAt(target);
        }
    }
}
