
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
    public void ShowRoom(string[] activeRooms)
    {
        int mask = 0;
   

        foreach (string room in activeRooms)
        {
            int layer = LayerMask.NameToLayer(room);

            cam.cullingMask =
           (1 << LayerMask.NameToLayer(room));
            mask |= (1 << layer);
        }


        // always visible
        mask |= (1 << LayerMask.NameToLayer("Player")) |            
        //(1 << LayerMask.NameToLayer("Enemy")) |             
        (1 << LayerMask.NameToLayer("UI")) |
        (1 << LayerMask.NameToLayer("Fly")) |
        (1 << LayerMask.NameToLayer("Cage"));
        cam.cullingMask = mask;
    }
}