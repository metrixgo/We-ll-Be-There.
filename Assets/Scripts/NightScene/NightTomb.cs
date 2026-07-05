using System.Collections;
using UnityEngine;

public class NightTomb : MonoBehaviour
{
    [SerializeField] private GameObject dirtPile;
    [SerializeField] private AudioClip dig;

    public void Dig()
    {
        if (!MainManager.instance.HasItem("Shovel")) MainManager.instance.AddTrigger("dialogue;You;I need a shovel to dig open this.");
        else StartCoroutine(StartDigging());
    }

    private IEnumerator StartDigging()
    {
        MainManager.instance.AddTrigger("changescreen;#00000000;#000000FF;1");
        MainManager.instance.AddTrigger("wait;9");
        MainManager.instance.AddTrigger("changescreen;#000000FF;#00000000;1");
        yield return new WaitForSeconds(2.0f);
        MainManager.instance.PlayEffect(dig);
        GetComponent<MeshRenderer>().enabled = false;
        GetComponent<Collider>().enabled = false;
        dirtPile.transform.localScale += Vector3.forward * 60.0f;
        yield return new WaitForSeconds(9.0f);
        MainManager.instance.StopMusic();
        Destroy(gameObject);
    }
}
