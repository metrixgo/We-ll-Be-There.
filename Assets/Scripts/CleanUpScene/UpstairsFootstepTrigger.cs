using System.Collections;
using UnityEngine;

public class UpstairsFootstepTrigger : MonoBehaviour
{
    [SerializeField] private AudioClip doorOpen;
    [SerializeField] private AudioClip woodRun;
    [SerializeField] private AudioSource ad;

    private bool triggered = false;

    private void OnTriggerEnter(Collider other)
    {
        if (MainManager.instance.HasItem("Plastic Bag") && !triggered)
        {
            triggered = true;
            StartCoroutine(Sounds());
        }
    }

    private IEnumerator Sounds()
    {
        ad.clip = doorOpen;
        ad.Play();
        yield return new WaitForSeconds(doorOpen.length + 0.1f);
        ad.clip = woodRun;
        ad.Play();
        yield return new WaitForSeconds(woodRun.length + 0.1f);
        Destroy(gameObject);
    }
}

