using UnityEngine;

public class SelfDestruct : MonoBehaviour
{
    [SerializeField] private float dur = 4.0f;

    private void Start()
    {
        Destroy(gameObject, dur);
    }
}
