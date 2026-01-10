using UnityEngine;


public class SwitchCameraPosition : MonoBehaviour
{
    public Camera cam;
    public Transform position;

    public void MoveCamera(Transform CamPos)
    {
        if (cam == null || CamPos == null) return;

        //cam.transform.position = CamPos.position;
        cam.transform.SetPositionAndRotation(CamPos.position, CamPos.rotation);
    }

   

    //room visibility
    public void ShowRoom(string activeRoom)
    {
        cam.cullingMask =
        (1 << LayerMask.NameToLayer(activeRoom)) | // show active room

        // always visible
        (1 << LayerMask.NameToLayer("Player")) |            
        (1 << LayerMask.NameToLayer("Enemy")) |             
        (1 << LayerMask.NameToLayer("UI")) |
        (1 << LayerMask.NameToLayer("Fly"));
    }
}