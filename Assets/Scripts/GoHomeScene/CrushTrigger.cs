using System.Collections;
using UnityEngine;

public class CrushTrigger : MonoBehaviour
{
    [SerializeField] private GameObject man;
    [SerializeField] private GameObject corpse;
    [SerializeField] private AudioClip crash;
    [SerializeField] private AudioClip tense;
    [SerializeField] private GameObject bicycle;
    [SerializeField] private GameObject standingUpPlayer;
    [SerializeField] private GameObject crashedBike;

    private void OnTriggerEnter(Collider other)
    {
        if(other.CompareTag("Bicycle")) StartCoroutine(Crash());
    }

    private IEnumerator Crash()
    {
        bicycle.GetComponent<Bicycle>().Crash();
        MainManager.instance.PlayEffect(crash);
        MainManager.instance.PlayMusic(tense);
        MainManager.instance.AddTrigger("changescreen;#00000000;#000000FF;0");
        yield return new WaitForSeconds(5.0f);
        man.SetActive(false);
        bicycle.SetActive(false);
        corpse.SetActive(true);
        crashedBike.SetActive(true);
        standingUpPlayer.SetActive(true);
        Destroy(gameObject);
    }
}
