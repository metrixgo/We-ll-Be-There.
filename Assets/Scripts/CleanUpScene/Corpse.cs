using System.Collections;
using UnityEngine;

public class Corpse : MonoBehaviour
{
    [SerializeField] private AudioClip packingEffect;
    [SerializeField] private GameObject bag;
    [SerializeField] private GameObject packedBody;
    [SerializeField] private GameObject tomb;
    
    public void Pack()
    {
        if (MainManager.instance.HasItem("Plastic Bag"))
        {
            StartCoroutine(PackUp());
        }
        else
        {
            MainManager.instance.AddTrigger("dialogue;You;I need to grab a plastic bag from home to pack this body in.");
        }
    }

    private IEnumerator PackUp()
    {
        MainManager.instance.AddTrigger("changescreen;#00000000;#000000FF;2");
        MainManager.instance.AddTrigger("wait;9");
        MainManager.instance.AddTrigger("changescreen;#000000FF;#00000000;2");
        MainManager.instance.AddTrigger("dialogue;You;Ok this looks... fine...");
        MainManager.instance.AddTrigger("dialogue;You;Time to bury it in my backyard.");
        MainManager.instance.AddTrigger("dialogue;You;I hope no one will notice me on my way back... hopefully...");
        yield return new WaitForSeconds(2.5f);
        MainManager.instance.PlayEffect(packingEffect);
        packedBody.SetActive(true);
        tomb.SetActive(true);
        Destroy(bag);
        Destroy(gameObject);
    }
}
