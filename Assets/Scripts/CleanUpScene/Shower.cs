using System.Collections;
using UnityEngine;

public class Shower : MonoBehaviour
{
    [SerializeField] private GameObject[] beds;
    [SerializeField] private AudioSource ad;
    [SerializeField] private ParticleSystem ps;

    private int times = 0;

    public void GoIn()
    {
        times++;
        if (times == 1) StartCoroutine(Showering());
        else if (times <= 3) StartCoroutine(ShoweringAgain());
        else MainManager.instance.AddTrigger("dialogue;You;I think that's enough showering. I really need to go to bed. I'm just so exhausted.");
    }

    private IEnumerator Showering()
    {
        MainManager.instance.AddTrigger("changescreen;#00000000;#000000FF;1");
        MainManager.instance.AddTrigger("moveplayerto;-58.2;4.761;356.187");
        MainManager.instance.AddTrigger("rotateplayerto;90;-20");
        MainManager.instance.AddTrigger("wait;3");
        MainManager.instance.AddTrigger("changescreen;#000000FF;#00000000;1");
        MainManager.instance.AddTrigger("wait;2");
        MainManager.instance.AddTrigger("dialogue;You;Whoo...");
        MainManager.instance.AddTrigger("wait;1");
        MainManager.instance.AddTrigger("dialogue;You;It's all done now... Finally I can have a good rest...");
        MainManager.instance.AddTrigger("wait;1");
        MainManager.instance.AddTrigger("dialogue;You;I didn't do anything wrong, right? I didn't do anything. No one saw it. Nothing happened.");
        MainManager.instance.AddTrigger("wait;1");
        MainManager.instance.AddTrigger("dialogue;You;Tomorrow will be a fresh day. I'll eat. I'll work. I'll walk. I'll play. I'll watch. I'll sleep. I'll relax. I'll be a normal person. I'll be like everyone else. I'll be fine. I'll be fine.  I'll be fine.");
        MainManager.instance.AddTrigger("wait;1");
        MainManager.instance.AddTrigger("dialogue;You;Please let this thing end...");
        MainManager.instance.AddTrigger("wait;0.5");
        MainManager.instance.AddTrigger("dialogue;You;......");
        MainManager.instance.AddTrigger("wait;0.5");
        MainManager.instance.AddTrigger("dialogue;You;......");
        MainManager.instance.AddTrigger("wait;0.5");
        MainManager.instance.AddTrigger("dialogue;You;......");
        MainManager.instance.AddTrigger("wait;3");
        MainManager.instance.AddTrigger("dialogue;You;They are watching me...");
        MainManager.instance.AddTrigger("wait;3");
        yield return new WaitForSeconds(1.5f);
        float t = 0, v = ad.volume;
        ps.Play();
        ad.volume = 0;
        ad.Play();
        while (t < 2.5f)
        {
            ad.volume = Mathf.Lerp(0, v, t / 2.5f);
            t += Time.deltaTime;
            yield return null;
        }
        ad.volume = v;
        yield return new WaitUntil(() => MainManager.instance.gameState == 1);
        MainManager.instance.AddTrigger("changescreen;#00000000;#000000FF;1");
        MainManager.instance.AddTrigger("wait;3");
        MainManager.instance.AddTrigger("moveplayerto;-58.276;4.761;357.355");
        MainManager.instance.AddTrigger("rotateplayerto;180;20");
        MainManager.instance.AddTrigger("changescreen;#000000FF;#00000000;1");
        MainManager.instance.AddTrigger("wait;1");
        MainManager.instance.AddTrigger("dialogue;You;Alright... Time to go to bed...");
        yield return new WaitForSeconds(1.5f);
        t = 0;
        ps.Stop();
        while (t < 2.5f)
        {
            ad.volume = Mathf.Lerp(v, 0, t / 2.5f);
            t += Time.deltaTime;
            yield return null;
        }
        ad.Stop();
        ad.volume = v;
        foreach(GameObject o in beds)
        {
            o.tag = "Interactable";
        }
    }

    private IEnumerator ShoweringAgain()
    {
        MainManager.instance.AddTrigger("changescreen;#00000000;#000000FF;1");
        MainManager.instance.AddTrigger("moveplayerto;-58.2;4.761;356.187");
        MainManager.instance.AddTrigger("rotateplayerto;90;-20");
        MainManager.instance.AddTrigger("wait;3");
        MainManager.instance.AddTrigger("changescreen;#000000FF;#00000000;1");
        MainManager.instance.AddTrigger("wait;2");
        MainManager.instance.AddTrigger("dialogue;You;Ahhh I just love showering...");
        MainManager.instance.AddTrigger("wait;1");
        MainManager.instance.AddTrigger("dialogue;You;Water. Please bring away my feelings...");
        MainManager.instance.AddTrigger("wait;3");
        yield return new WaitForSeconds(1.5f);
        float t = 0, v = ad.volume;
        ps.Play();
        ad.volume = 0;
        ad.Play();
        while (t < 2.5f)
        {
            ad.volume = Mathf.Lerp(0, v, t / 2.5f);
            t += Time.deltaTime;
            yield return null;
        }
        ad.volume = v;
        yield return new WaitUntil(() => MainManager.instance.gameState == 1);
        MainManager.instance.AddTrigger("changescreen;#00000000;#000000FF;1");
        MainManager.instance.AddTrigger("wait;3");
        MainManager.instance.AddTrigger("moveplayerto;-58.276;4.761;357.355");
        MainManager.instance.AddTrigger("rotateplayerto;180;20");
        MainManager.instance.AddTrigger("changescreen;#000000FF;#00000000;1");
        MainManager.instance.AddTrigger("wait;1");
        MainManager.instance.AddTrigger("dialogue;You;Alright... I really need to go to bed now.");
        yield return new WaitForSeconds(1.5f);
        t = 0;
        ps.Stop();
        while (t < 2.5f)
        {
            ad.volume = Mathf.Lerp(v, 0, t / 2.5f);
            t += Time.deltaTime;
            yield return null;
        }
        ad.Stop();
        ad.volume = v;
    }
}
