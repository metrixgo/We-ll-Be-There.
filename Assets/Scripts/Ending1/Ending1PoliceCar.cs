using UnityEngine;

public class Ending1PoliceCar : MonoBehaviour
{
    private float disCnt = 0;

    private void Update()
    {
        if(disCnt < 70.0f)
        {
            transform.Translate(Vector3.forward * 5.0f * Time.deltaTime, Space.World);
            disCnt += 5.0f * Time.deltaTime;
        }
    }
}
