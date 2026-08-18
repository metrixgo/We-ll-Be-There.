using UnityEngine;
using System.Collections;

public class GenerateCar : MonoBehaviour
{
    [SerializeField] private GameObject[] cars;

    private void Start()
    {
        StartCoroutine(GenerateCars());
    }

    private IEnumerator GenerateCars()
    {

        while (true)
        {
            yield return new WaitForSeconds(Random.Range(10.0f, 30.0f));
            GameObject o = cars[Random.Range(0, cars.Length)];
            Instantiate(o, transform.position, transform.rotation);
        }
    }
}