using UnityEngine;

public class TurnTrigger : MonoBehaviour
{
    [SerializeField] private float angle;

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Bicycle"))
        {
            other.GetComponent<Bicycle>().SetAngle(angle);
        }
        else if (other.CompareTag("Car"))
        {
            other.GetComponent<MoveCar>().SetAngle(angle);
        }
    }
}
