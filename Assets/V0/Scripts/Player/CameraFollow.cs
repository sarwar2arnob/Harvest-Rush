using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    public Transform Player;
    public Vector3 Offset;
    void Update()
    {
        transform.position = Player.transform.position + Offset;
    }
}