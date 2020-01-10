using UnityEngine;

public class CameraScript : MonoBehaviour
{
    public Transform player;
    public float smoothSpeed = 0.125f;
    public Vector3 offset;
    public Camera cam;

    int dist;

    private void FixedUpdate()
    {
        if (Input.GetKey("space") || FloorHandler.playing==false)
        {
            dist = FloorHandler.CameraDistance;
            cam.orthographic = true;
            transform.position = player.position + new Vector3(0,10,0);
            cam.orthographicSize = (dist * 2) +2;
            transform.rotation = Quaternion.Euler(90, 0, 0);
            return;

        }
        Vector3 desiredPosition = player.position + offset;
        Vector3 smoothPosition = Vector3.Lerp(transform.position, desiredPosition, smoothSpeed);
        //transform.position = smoothPosition;

        transform.position = player.position - player.forward * 3 + player.up * 2;

        transform.rotation = player.rotation;

        transform.Rotate(10, 0, 0 * Time.deltaTime);

        cam.orthographic = false;
        
    }

}
