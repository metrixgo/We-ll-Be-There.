using KinoGlitch;
using System.Collections;
using UnityEngine;

public class Ending4Manager : MonoBehaviour
{
    [SerializeField] private Transform cam;
    [SerializeField] private PlayerController pc;
    [SerializeField] private GameObject police;
    [SerializeField] private GameObject killer;
    [SerializeField] private AudioClip tense;
    [SerializeField] private GameObject sounds;

    private Coroutine moveCr;
    private Vector3 startPos;
    private Vector3 endPos;
    private DigitalGlitchController dgc;
    private AudioSource subTense;

    private void Start()
    {
        subTense = GetComponent<AudioSource>();
        endPos = cam.position;
        dgc = cam.GetComponent<DigitalGlitchController>();
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

    private void Update()
    {
        dgc.SetIntensity(Mathf.Max((8.0f - Vector3.Distance(cam.position, killer.transform.position)) / 240.0f, 0));
    }

    private IEnumerator MoreDialogues()
    {
        yield return new WaitForSeconds(2.0f);
        yield return new WaitUntil(() => !MainManager.instance.IsExecutingTriggers());
        pc.gameObject.SetActive(false);
        cam.gameObject.SetActive(true);
        MoveBack();
        yield return StartCoroutine(DisplayMove("You", "......"));
        yield return StartCoroutine(DisplayMove("Dad", "Come on, are you not feeling well right now?"));
        yield return StartCoroutine(DisplayMove("Dad", "Let's sing together, how about that?"));
        yield return StartCoroutine(DisplayMove("Mom", "Um... he's not feeling good right now. Give him some space. Let's just eat."));
        yield return StartCoroutine(DisplayMove("Dad", "Ok. Yeah. Sure. Let's eat."));
        yield return StartCoroutine(DisplayMove("You", "......"));
        yield return StartCoroutine(DisplayMove("Dad", "......"));
        police.SetActive(true);
        yield return StartCoroutine(DisplayMove("???", "......"));
        yield return StartCoroutine(DisplayMove("???", "......"));
        MainManager.instance.AddTrigger("wait;5");
        MainManager.instance.AddTrigger("dialogue;Policewoman;Is it about time?");
        yield return new WaitForSeconds(3.0f);
        Destroy(sounds);
        MainManager.instance.StopMusic();
        police.GetComponent<Animator>().SetBool("turn", true);
        yield return new WaitForSeconds(1.0f);
        police.GetComponent<Animator>().SetBool("turn", false);
        yield return new WaitUntil(() => MainManager.instance.gameState == 1);
        MainManager.instance.AddTrigger("wait;3");
        MainManager.instance.AddTrigger("dialogue;Mayor;It is about time.");
        killer.SetActive(true);
        yield return new WaitForSeconds(2.0f);
        cam.position = new Vector3(-74.2829971f, 2.2f, -66.23f);
        cam.rotation = Quaternion.Euler(0, -90.0f, 0);
        MainManager.instance.PlayMusic(tense);
        yield return new WaitUntil(() => MainManager.instance.gameState == 1);
        MainManager.instance.AddTrigger("wait;5");
        MainManager.instance.AddTrigger("changescreen;#000000FF;#00000000;5");
        yield return new WaitForSeconds(2.0f);
        cam.position = new Vector3(-76.0f, 2.2f, -66.35f);
        cam.rotation = Quaternion.Euler(0, -90.0f, 0);
        yield return new WaitForSeconds(3.0f);
        pc.gameObject.SetActive(true);
        Destroy(cam.gameObject);
        subTense.Play();
        float t = 0;
        while(t < 5.0f)
        {
            subTense.volume = Mathf.Lerp(0, PlayerPrefs.GetFloat("Music", 30.0f) / 100.0f, t / 5.0f);
            t += Time.deltaTime;
            yield return null;
        }


    }

    private IEnumerator DisplayMove(string speaker, string content)
    {
        MainManager.instance.AddTrigger("dialogue;" + speaker + ";" + content);
        yield return new WaitForSeconds(0.1f);
        yield return new WaitUntil(() => MainManager.instance.gameState == 1);
        MoveBack();
    }

    private void MoveBack()
    {
        startPos = cam.position;
        endPos = endPos - Vector3.right * 2.0f;
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
