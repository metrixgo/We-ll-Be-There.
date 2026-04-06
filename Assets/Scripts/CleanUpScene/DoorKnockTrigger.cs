using System.Collections;
using UnityEngine;

public class DoorKnockTrigger : MonoBehaviour
{
    [SerializeField] private AudioClip doorKnock;
    [SerializeField] private AudioClip doorOpen;
    [SerializeField] private AudioClip woodRun;
    [SerializeField] private AudioSource ad;

    private void OnTriggerEnter(Collider other)
    {
        if(MainManager.instance.HasItem("Packed Body"))
        {
            StartCoroutine(Sounds());
        }
    }

    private IEnumerator Sounds()
    {
        ad.clip = doorKnock;
        ad.Play();
        yield return new WaitForSeconds(doorKnock.length);
        ad.clip = doorOpen;
        ad.Play();
        yield return new WaitForSeconds(doorOpen.length + 0.1f);
        ad.clip = woodRun;
        float l = woodRun.length + 0.05f;
        float t = 0;
        for(int i = 1; i <= 20; i++)
        {
            while(t < l)
            {
                t += Time.deltaTime;
                yield return null;
            }
            ad.Play();
            t = 0;
        }
        Destroy(gameObject);
    }
}
