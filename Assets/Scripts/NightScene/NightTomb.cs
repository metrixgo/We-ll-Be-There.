using System.Collections;
using UnityEngine;

public class NightTomb : MonoBehaviour
{
    [SerializeField] private GameObject dirtPile;
    [SerializeField] private GameObject downRope;
    [SerializeField] private GameObject rope;
    [SerializeField] private GameObject putDownRope;
    [SerializeField] private GameObject player;
    [SerializeField] private GameObject climbCam;
    [SerializeField] private AudioClip dig;
    [SerializeField] private AudioClip setUpRope;
    [SerializeField] private AudioClip rope1;
    [SerializeField] private AudioClip rope2;

    private int state = 0;

    public void Dig()
    {
        if (state == 0)
        {
            if (!MainManager.instance.HasItem("Shovel")) MainManager.instance.AddTrigger("dialogue;You;I need a shovel to dig open this.");
            else StartCoroutine(StartDigging());
        }
        else if (state == 1)
        {
            if (!MainManager.instance.HasItem("Rope")) MainManager.instance.AddTrigger("dialogue;You;It seems pretty dangerous to go down here...");
            else StartCoroutine(StartRopping());
        }
        else if (state == 2)
        {
            StartCoroutine(ClimbDown());
        }
    }

    private IEnumerator ClimbDown()
    {
        MainManager.instance.AddTrigger("changescreen;#00000000;#000000FF;1");
        MainManager.instance.AddTrigger("wait;2");
        MainManager.instance.AddTrigger("changescreen;#000000FF;#00000000;1");
        MainManager.instance.AddTrigger("wait;6");
        yield return new WaitForSeconds(2.0f);
        player.SetActive(false);
        climbCam.SetActive(true);
        while (true)
        {
            MainManager.instance.PlayEffect(rope1);
            yield return new WaitForSeconds(1.0f);
            MainManager.instance.PlayEffect(rope2);
            yield return new WaitForSeconds(1.5f);
        }
    }

    private IEnumerator StartRopping()
    {
        MainManager.instance.AddTrigger("changescreen;#00000000;#000000FF;0.5");
        MainManager.instance.AddTrigger("wait;" + setUpRope.length);
        MainManager.instance.AddTrigger("changescreen;#000000FF;#00000000;0.5");
        yield return new WaitForSeconds(0.5f);
        MainManager.instance.PlayEffect(setUpRope);
        state = 2;
        yield return new WaitForSeconds(0.1f);
        downRope.SetActive(true);
        Destroy(rope);
        Destroy(putDownRope);
    }

    private IEnumerator StartDigging()
    {
        MainManager.instance.AddTrigger("changescreen;#00000000;#000000FF;1");
        MainManager.instance.AddTrigger("wait;" + (dig.length + 0.5f));
        MainManager.instance.AddTrigger("changescreen;#000000FF;#00000000;1");
        yield return new WaitForSeconds(1.5f);
        MainManager.instance.PlayEffect(dig);
        GetComponent<MeshRenderer>().enabled = false;
        GetComponent<Collider>().isTrigger = true;
        state = 1;
        name = "Hole";
        dirtPile.transform.localScale += Vector3.forward * 60.0f;
        yield return new WaitForSeconds(dig.length + 1.0f);
        MainManager.instance.StopMusic();
    }
}
