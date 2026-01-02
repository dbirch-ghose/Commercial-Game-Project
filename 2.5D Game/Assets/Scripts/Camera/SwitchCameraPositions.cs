using UnityEngine;
using Fusion;

public class SwitchCameraPosition : NetworkBehaviour
{
    public Camera cam;
    public Transform position;

    public void MoveCamera(Transform CamPos)
    {
        if (cam == null || CamPos == null) return;

        cam.transform.position = CamPos.position;
    }

}