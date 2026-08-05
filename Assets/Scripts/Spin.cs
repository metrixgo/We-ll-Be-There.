using UnityEngine;

public class Spin : MonoBehaviour
{
    [SerializeField] private float speed = 360.0f;

    private void Update()
    {
        transform.Rotate(0, speed * Time.deltaTime, 0, Space.World);
    }
}
