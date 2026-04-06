using System.Collections;
using UnityEngine;

public class HomePhone : MonoBehaviour
{
    [SerializeField] private AudioClip phoneCall;

    private int cnt = 0;

    public void CallThePolice()
    {
        cnt++;
        if(cnt == 1)
        {
            MainManager.instance.AddTrigger("dialogue;You;I don't want to give up. I can still hide him. I still have a chance.");
        }
        else if (cnt == 2)
        {
            MainManager.instance.AddTrigger("dialogue;You;No. No. No. My life will be ruined. Everything will fall apart.");
        }
        else if (cnt == 3)
        {
            MainManager.instance.AddTrigger("dialogue;You;Why... I'm still in school... I can have a good future...");
        }
        else if (cnt == 4)
        {
            MainManager.instance.AddTrigger("dialogue;You;Please... I just made a mistake... we all make mistakes... I don't want this to cost my entire life...");
        }
        else if (cnt <= 7)
        {
            MainManager.instance.AddTrigger("dialogue;You;......");
        }
        else
        {
            MainManager.instance.AddTrigger("dialogue;You;Maybe... maybe you're right...");
            StartCoroutine(Ending());
        }
    }

    private IEnumerator Ending()
    {
        yield return new WaitUntil(() => MainManager.instance.gameState == 1);
        MainManager.instance.AddTrigger("wait;2");
        MainManager.instance.AddTrigger("changescreen;#00000000;#000000FF;10");
        MainManager.instance.AddTrigger("wait;2");
        yield return new WaitForSeconds(2.0f);
        MainManager.instance.PlayEffect(phoneCall);
        yield return new WaitForSeconds(12.0f);
        MainManager.instance.LoadScene("Ending1");
    }
}
