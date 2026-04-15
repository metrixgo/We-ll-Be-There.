using System.Collections;
using UnityEngine;

public class Tomb : MonoBehaviour
{
    [SerializeField] private GameObject environment;
    [SerializeField] private GameObject packedBody;
    [SerializeField] private GameObject cover;
    [SerializeField] private GameObject dirtCover;
    [SerializeField] private GameObject dirtPile;
    [SerializeField] private GameObject shovel;
    [SerializeField] private GameObject shower;
    [SerializeField] private AudioClip dig;
    [SerializeField] private AudioClip putEffect;
    [SerializeField] private AudioClip night;
    [SerializeField] private PlayerController pc;

    private int state = 0;

    public void Interact()
    {
        if (!MainManager.instance.HasItem("Packed Body"))
        {
            MainManager.instance.AddTrigger("dialogue;You;Why did I come back without picking up the body?!");
            MainManager.instance.AddTrigger("dialogue;You;I'm so stupid. I need to go back to pick up that packed body.");
        }
        else if (state == 0)
        {
            state = 1;
            name = "Dig";
            shovel.tag = "Interactable";
            packedBody.transform.SetParent(environment.transform);
            packedBody.transform.position = new Vector3(-73.127f, 0.3f, 359.596f);
            packedBody.transform.rotation = Quaternion.Euler(-90.0f, -90.0f, 0);
            MainManager.instance.PlayEffect(putEffect);
            MainManager.instance.AddTrigger("wait;0.5");
            MainManager.instance.AddTrigger("dialogue;You;Nice.");
            MainManager.instance.AddTrigger("dialogue;You;Now I need to take the shovel from the garage to bury this body.");
            MainManager.instance.AddTrigger("cleartasks");
            MainManager.instance.AddTrigger("task;Take the shovel from the garage");
        }
        else if (state == 1)
        {
            if (!MainManager.instance.HasItem("Shovel"))
            {
                MainManager.instance.AddTrigger("dialogue;You;I need a shovel to bury this.");
            }
            else
            {
                tag = "Untagged";
                StartCoroutine(Dig());
                state = 2;
            }
        }
        else if (state == 2)
        {
            state = 3;
            StartCoroutine(Rebury());
        }
    }

    private IEnumerator Dig()
    {
        MainManager.instance.AddTrigger("changescreen;#00000000;#000000FF;1");
        MainManager.instance.AddTrigger("wait;9");
        MainManager.instance.AddTrigger("changescreen;#000000FF;#00000000;1");
        MainManager.instance.AddTrigger("dialogue;You;Now I just need to cover all of these up.");
        yield return new WaitForSeconds(2.0f);
        MainManager.instance.PlayEffect(dig);
        dirtPile.SetActive(true);
        packedBody.transform.position = new Vector3(-72.945f, -0.57f, 359.596f);
        Destroy(cover);
    }

    private IEnumerator Rebury()
    {
        MainManager.instance.AddTrigger("changescreen;#00000000;#000000FF;1");
        MainManager.instance.AddTrigger("wait;9");
        MainManager.instance.AddTrigger("changescreen;#000000FF;#00000000;1");
        MainManager.instance.AddTrigger("dialogue;You;Phew... Finally...");
        MainManager.instance.AddTrigger("dialogue;You;......");
        MainManager.instance.AddTrigger("dialogue;You;I'm so tired...");
        MainManager.instance.AddTrigger("dialogue;You;I should take a shower and sleep...");
        MainManager.instance.AddTrigger("dialogue;You;......");
        MainManager.instance.AddTrigger("dialogue;You;May god bless me...");
        MainManager.instance.AddTrigger("cleartasks");
        MainManager.instance.AddTrigger("task;Take a shower");
        yield return new WaitForSeconds(2.0f);
        MainManager.instance.PlayEffect(dig);
        Destroy(dirtPile);
        dirtCover.SetActive(true);
        shovel.transform.SetParent(environment.transform);
        shovel.transform.localPosition = new Vector3(-259.651f, 0.1922f, -88.235f);
        shovel.transform.rotation = Quaternion.Euler(0, 150.0f, -90.0f);
        yield return new WaitForSeconds(9.0f);
        MainManager.instance.PlayMusic(night);
        shower.tag = "Interactable";
        pc.CanRun(false);
        Destroy(gameObject);
    }
}
