using System.Collections;
using UnityEngine;

public class Ending1PoliceWoman : MonoBehaviour
{
    [SerializeField] private AudioSource ad1;
    [SerializeField] private AudioSource ad2;
    
    public void TalkTo()
    {
        MainManager.instance.AddTrigger("dialogue;Policewoman;I can't believe it was you...");
        MainManager.instance.AddTrigger("dialogue;You;I'm... I'm sorry...");
        MainManager.instance.AddTrigger("dialogue;Policewoman;I'm glad you turned in yourself. This makes things much easier for both you and me.");
        MainManager.instance.AddTrigger("dialogue;You;Yeah... Shall we go now?");
        MainManager.instance.AddTrigger("dialogue;Policewoman;Yes. Please sit on the back. We'll make sure to give you a fair result.");
        MainManager.instance.AddTrigger("dialogue;You;Thanks...");
        StartCoroutine(WaitForEnd());
    }

    private IEnumerator WaitForEnd()
    {
        yield return new WaitUntil(() => MainManager.instance.gameState == 1);
        MainManager.instance.AddTrigger("ending;ENDING 1/5: SURRENDER;You surrendered yourself to the police. They brought you to the police station and asked what happened. After describing what you had gone through, they let you stay in a private room to rest. You thought you made the right choice and were very relieved, but you could feel something strange was going on. And when you realized it, it was too late.");

        yield return new WaitForSeconds(2.0f);
        float t = 0, l = 10.0f;
        float a = ad1.volume, b = ad2.volume;
        while(t < l)
        {
            ad1.volume = Mathf.Lerp(a, 0, t / l);
            ad2.volume = Mathf.Lerp(b, 0, t / l);
            t += Time.deltaTime;
            yield return null;
        }
        ad1.Stop();
        ad2.Stop();
    }
}
