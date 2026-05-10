using System.Collections;
using UnityEngine;

public class Recover : MonoBehaviour
{
    [SerializeField] private GameObject cover;
    [SerializeField] private AudioClip dig;

    public void Cover()
    {
        if (MainManager.instance.gameState != 1) return;

        if (MainManager.instance.HasItem("Shovel"))
        {
            StartCoroutine(CoverUp());
        }
        else MainManager.instance.AddTrigger("dialogue;You;I need a shovel to cover this.");
    }

    private IEnumerator CoverUp()
    {
        MainManager.instance.AddTrigger("changescreen;#00000000;#000000ff;1.2");
        MainManager.instance.AddTrigger("wait;" + (0.6f + dig.length));
        MainManager.instance.AddTrigger("changescreen;#000000ff;#00000000;1.2");
        yield return new WaitForSeconds(1.5f);
        MainManager.instance.PlayEffect(dig);
        cover.SetActive(true);
        yield return new WaitForSeconds(dig.length);
        CleanUpClock.clock.Clean("cover", true);
        CleanUpClock.clock.Clean("shovel", false);
        Destroy(gameObject);
    }
}
