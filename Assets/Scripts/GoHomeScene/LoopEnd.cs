using UnityEngine;

public class LoopEnd : MonoBehaviour
{
    [SerializeField] private GameObject bicycle;
    [SerializeField] private DrunkMan man;

    private int cnt = 0;

    private void OnTriggerEnter(Collider other)
    {
        if (!other.CompareTag("Bicycle")) return ;

        cnt++;
        if (cnt <= 11)
        {
            bicycle.transform.Translate(0, 0, -102.5f);
            GameObject[] cars = GameObject.FindGameObjectsWithTag("Car");
            foreach(GameObject car in cars)
            {
                car.transform.Translate(0, 0, -102.5f, Space.World);
                car.GetComponent<MoveCar>().ChangeDis(-102.5f);
            }
        }
        else
        {
            man.StartMoving();
            Destroy(gameObject);
        }
    }

}
