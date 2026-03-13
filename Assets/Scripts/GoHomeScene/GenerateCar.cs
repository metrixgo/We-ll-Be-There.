using UnityEngine;
using System.Collections;
using Unity.VisualScripting;

public class GenerateCar : MonoBehaviour
{
    [SerializeField] private bool isBackground = false;
    [SerializeField] private GameObject[] cars;

    private void Start()
    {
        StartCoroutine(GenerateCars());
    }

    private IEnumerator GenerateCars()
    {

        while (true)
        {
            if(!isBackground) yield return new WaitForSeconds(Random.Range(10.0f, 30.0f));
            else yield return new WaitForSeconds(Random.Range(5.0f, 10.0f));
            GameObject o = cars[Random.Range(0, cars.Length)];
            o = Instantiate(o, transform.position, transform.rotation);
            o.GetComponent<MoveCar>().SetIsBackground(isBackground);
        }
    }
}