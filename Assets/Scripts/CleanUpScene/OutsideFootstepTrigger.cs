using UnityEngine;

public class OutsideFootstepTrigger : MonoBehaviour
{
    [SerializeField] private AudioSource ad;

    private bool triggered = false;

    private void OnTriggerEnter(Collider other)
    {
        if (!triggered)
        {
            triggered = true;
            ad.Play();
        }
    }
}

