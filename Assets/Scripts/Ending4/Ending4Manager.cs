using System.Collections;
using UnityEngine;

public class Ending4Manager : MonoBehaviour
{
    [SerializeField] private Transform cam;
    [SerializeField] private PlayerController pc;

    private Coroutine moveCr;
    private Vector3 startPos;
    private Vector3 endPos;

    private void Start()
    {
        pc.Freeze(true);
        MainManager.instance.AddTrigger("changescreen;#000000FF;#00000000;4");
        MainManager.instance.AddTrigger("dialogue;You;......;1");
        MainManager.instance.AddTrigger("dialogue;Dad;...and I was so surprised you know, we both didn't see when that happened.;1");
        MainManager.instance.AddTrigger("dialogue;Dad;I suspect someone broke it with a hammer, or else the hole on the glass door wouldn't be so uniform.;1");
        MainManager.instance.AddTrigger("dialogue;Mom;Son, do you know who broke the glass door?;1");
        MainManager.instance.AddTrigger("dialogue;You;I... don't know...;1");
        MainManager.instance.AddTrigger("dialogue;You;Mom, is there a hole in the backyard?;1");
        MainManager.instance.AddTrigger("dialogue;Mom;......;1");
        MainManager.instance.AddTrigger("dialogue;Mom;I do think there are some traces of dirt there. But there should be no holes.;1");
        MainManager.instance.AddTrigger("dialogue;Dad;What are you saying, didn't we already see that? Like the one that's very deep into the ground, with all kinds of...;1");
        MainManager.instance.AddTrigger("dialogue;You;What? What do you mean? Dad!;1");
        MainManager.instance.AddTrigger("dialogue;Dad;N... Nothing.;1");
        MainManager.instance.AddTrigger("dialogue;You;???;1");
        MainManager.instance.AddTrigger("dialogue;Dad;Haha, got you with a joke, huh? There are no holes, what are you even worrying about.;1");
        StartCoroutine(MoreDialogues());
    }

    private IEnumerator MoreDialogues()
    {
        yield return new WaitForSeconds(2.0f);
        yield return new WaitUntil(() => !MainManager.instance.IsExecutingTriggers());
        pc.gameObject.SetActive(false);
        cam.gameObject.SetActive(true);
        MoveBack();
        MainManager.instance.AddTrigger("dialogue;You;......");
        yield return new WaitUntil(() => MainManager.instance.gameState == 1);
        MoveBack();
        MainManager.instance.AddTrigger("dialogue;Dad;Come on, are you not feeling well right now?");
        yield return new WaitUntil(() => MainManager.instance.gameState == 1);
        MoveBack();
        MainManager.instance.AddTrigger("dialogue;Dad;Let's sing together, how about that?");
        yield return new WaitUntil(() => MainManager.instance.gameState == 1);
        MoveBack();
        MainManager.instance.AddTrigger("dialogue;Mom;Um... he's not feeling good right now. Give him some space. Let's just eat.");
        yield return new WaitUntil(() => MainManager.instance.gameState == 1);
        MoveBack();
        MainManager.instance.AddTrigger("dialogue;Dad;Ok. Yeah. Sure. Let's eat.");
        yield return new WaitUntil(() => MainManager.instance.gameState == 1);
        MoveBack();
        MainManager.instance.AddTrigger("dialogue;You;......");
        yield return new WaitUntil(() => MainManager.instance.gameState == 1);
        MoveBack();
        MainManager.instance.AddTrigger("dialogue;Dad;......");
        yield return new WaitUntil(() => MainManager.instance.gameState == 1);
        MoveBack();
        MainManager.instance.AddTrigger("dialogue;???;......");
        yield return new WaitUntil(() => MainManager.instance.gameState == 1);
        MoveBack();
        MainManager.instance.AddTrigger("dialogue;???;......");
        yield return new WaitUntil(() => MainManager.instance.gameState == 1);
        MoveBack();
    }

    private void MoveBack()
    {
        startPos = cam.position;
        endPos = cam.position - Vector3.right * 2.0f;
        if (moveCr != null) StopCoroutine(moveCr);
        moveCr = StartCoroutine(ShiftBack());
    }

    private IEnumerator ShiftBack()
    {
        float t = 0;
        while (t < 2.0f)
        {
            cam.position = Vector3.Lerp(startPos, endPos, Mathf.SmoothStep(0, 1.0f, t / 2.0f));
            t += Time.deltaTime;
            yield return null;
        }
    }

}
