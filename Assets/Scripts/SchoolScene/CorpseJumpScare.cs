using System.Collections;
using UnityEngine;

public class CorpseJumpScare : MonoBehaviour
{
    [SerializeField] private PlayerController player;
    [SerializeField] private GameObject corpse;
    [SerializeField] private AudioClip jumpScareEffect;
    [SerializeField] private AudioClip tenseMusic;

    private void OnTriggerEnter(Collider other)
    {
        StartCoroutine(Triggers());
    }

    private IEnumerator Triggers()
    {
        yield return new WaitUntil(() => MainManager.instance.gameState == 1);
        corpse.SetActive(true);
        MainManager.instance.PlayEffect(jumpScareEffect);
        MainManager.instance.AddTrigger("changescreen;#FF000020;#FF000000;2");
        player.SetRotation(61.0f, 35.0f);
        yield return new WaitForSeconds(1.0f);
        MainManager.instance.PlayMusic(tenseMusic);
        MainManager.instance.AddTrigger("dialogue;You;What was that?!");
        MainManager.instance.AddTrigger("dialogue;You;Damn it, I must get outta here real quick.");
        Destroy(corpse);
        Destroy(gameObject);
    }
}
