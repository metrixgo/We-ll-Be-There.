using UnityEngine;

public class MoveCar : MonoBehaviour
{
    private float speed = 40.0f;
    private float disCount = 0;
    private float angle = 0;
    private float angAim = 0.114f;
    private float angCount = 0;

    private void Update()
    {
        transform.Translate(Vector3.forward * speed * Time.deltaTime);
        disCount += speed * Time.deltaTime;
        if (disCount > 700.0f) Destroy(gameObject);

        if (angle != 0)
        {
            if (angAim == 0.114f)
            {
                if (angle < 0) angAim = transform.eulerAngles.y - 90.0f;
                else angAim = transform.eulerAngles.y + 90.0f;
            }
            angCount += speed * angle * Time.deltaTime;
            transform.Rotate(Vector3.up * speed * angle * Time.deltaTime);
            if (Mathf.Abs(angCount) >= 90.0f)
            {
                transform.rotation = Quaternion.Euler(0, angAim, 0);
                angle = 0;
                angAim = 0.114f;
                angCount = 0;
            }
        }
    }

    public void SetAngle(float f)
    {
        angle = f;
    }

    public void ChangeDis(float f)
    {
        disCount += f;
    }
}
