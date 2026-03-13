using System.Collections;
using UnityEngine;

public class CameraShake : MonoBehaviour
{
    private void Start()
    {
        StartCoroutine(Move());
    }

    private IEnumerator Move()
    {
        yield return new WaitForSeconds(0.2f);
        float t = 0;
        while (true)
        {
            t += Time.deltaTime / 5.0f;
            t %= Mathf.PI * 2;
            transform.rotation = Quaternion.Euler(5.0f, 180.0f + 5.0f * Mathf.Sin(t), 0);
            yield return null;
        }
    }
}
