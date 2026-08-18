using UnityEngine;

public class LoseControl : MonoBehaviour
{
    [SerializeField] private Bicycle bicycle;

    private void OnTriggerEnter(Collider other)
    {
        if (!other.CompareTag("Bicycle")) return;

        bicycle.LoseControl();
        Destroy(gameObject);
    }
}
