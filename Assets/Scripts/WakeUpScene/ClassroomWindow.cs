using System.Collections;
using System.IO.Hashing;
using UnityEngine;

public class ClassroomWindow : MonoBehaviour
{
    [SerializeField] private GameObject normalWindow;
    [SerializeField] private GameObject brokenWindow;
    [SerializeField] private AudioClip breakEffect;
    [SerializeField] private AudioClip crawlEffect;

    private bool broke = false;
    private bool crawled = false;

    public void Interact()
    {
        if (!MainManager.instance.HasItem("Hammer"))
        {
            MainManager.instance.AddTrigger("dialogue;You;It looks like there're some candles in a distance...");
        }
        else if (!broke)
        {
            StartCoroutine(Break());
        }
        else
        {
            StartCoroutine(GoOver());
        }
    }

    private IEnumerator Break()
    {
        MainManager.instance.AddTrigger("changescreen;#00000000;#000000FF;1");
        MainManager.instance.AddTrigger("wait;2");
        MainManager.instance.AddTrigger("changescreen;#000000FF;#00000000;1");
        yield return new WaitForSeconds(1.0f);
        MainManager.instance.PlayEffect(breakEffect);
        Destroy(normalWindow);
        brokenWindow.SetActive(true);
        broke = true;
    }

    private IEnumerator GoOver()
    {
        MainManager.instance.AddTrigger("changescreen;#00000000;#000000FF;1");
        MainManager.instance.AddTrigger("wait;5.5");
        if(!crawled) MainManager.instance.AddTrigger("moveplayerto;-5;1.33;15.5");
        else MainManager.instance.AddTrigger("moveplayerto;-3;1.33;15.5");
        MainManager.instance.AddTrigger("changescreen;#000000FF;#00000000;1");
        yield return new WaitForSeconds(1.2f);
        MainManager.instance.PlayEffect(crawlEffect);
        crawled = !crawled;
    }
}
