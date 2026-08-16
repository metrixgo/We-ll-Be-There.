using KinoGlitch;
using System.Collections;
using TMPro;
using UnityEngine;

public class Ending4Manager : MonoBehaviour
{
    [SerializeField] private Transform cam;
    [SerializeField] private PlayerController pc;
    [SerializeField] private GameObject jumpscareCam;
    [SerializeField] private GameObject police;
    [SerializeField] private GameObject killer;
    [SerializeField] private AudioClip tense;
    [SerializeField] private AudioClip monsterOut;
    [SerializeField] private AudioClip lightsOut;
    [SerializeField] private AudioClip tinnitus;
    [SerializeField] private AudioClip horrorAmb;
    [SerializeField] private AudioClip jumpscare;
    [SerializeField] private TextMeshProUGUI title;
    [SerializeField] private GameObject sounds;

    private Coroutine moveCr;
    private Vector3 startPos;
    private Vector3 endPos;
    private DigitalGlitchController dgc;
    private AudioSource subTense;
    private Animator policeAnim;

    private void Start()
    {
        policeAnim = police.GetComponent<Animator>();
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
        if (cam != null && killer != null) dgc.SetIntensity(Mathf.Max((8.0f - Vector3.Distance(cam.position, killer.transform.position)) / 80.0f, 0));
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
        foreach (Transform o in sounds.transform)
        {
            o.GetComponent<AudioSource>().enabled = false;
        }
        Destroy(sounds);
        MainManager.instance.StopMusic();
        policeAnim.SetBool("turn", true);
        yield return new WaitForSeconds(2.0f);
        policeAnim.SetBool("turn", false);
        yield return new WaitUntil(() => MainManager.instance.gameState == 1);
        MainManager.instance.AddTrigger("wait;2");
        MainManager.instance.AddTrigger("changescreen;#FF000066;#FF000000;" + (1 + jumpscare.length));
        MainManager.instance.AddTrigger("dialogue;Mayor;It is about time.");
        yield return new WaitForSeconds(2.0f);
        cam.position = new Vector3(-74.2829971f, 2.2f, -66.23f);
        cam.rotation = Quaternion.Euler(0, -90.0f, 0);
        killer.SetActive(true);
        MainManager.instance.PlayMusic(tense);
        MainManager.instance.PlayEffect(jumpscare);

        yield return new WaitUntil(() => MainManager.instance.gameState == 1);
        MainManager.instance.AddTrigger("wait;5");
        MainManager.instance.AddTrigger("changescreen;#000000FF;#00000000;5");
        MainManager.instance.AddTrigger("dialogue;Mom;...speaking of which, do you hear anything?;1");
        MainManager.instance.AddTrigger("dialogue;Dad;I don't hear anything.;1");
        MainManager.instance.AddTrigger("dialogue;You;......;1");
        MainManager.instance.AddTrigger("dialogue;You;I'm sorry...;1");
        MainManager.instance.AddTrigger("dialogue;Dad;What?;1");
        MainManager.instance.AddTrigger("dialogue;You;It's too late...;1");
        yield return new WaitForSeconds(2.0f);
        cam.position = new Vector3(-76.0f, 2.2f, -66.35f);
        cam.rotation = Quaternion.Euler(0, 90.0f, 0);
        yield return new WaitForSeconds(3.0f);
        pc.gameObject.SetActive(true);
        Vector3 startPos = new Vector3(-66.0885925f, 0.637409568f, -66.822998f);
        Vector3 endPos = startPos + Vector3.right * 6.0f;
        Destroy(cam.gameObject);
        subTense.Play();
        float t = 0;
        while (t < 10.0f)
        {
            killer.transform.position = Vector3.Lerp(startPos, endPos, t / 10.0f);
            subTense.volume = Mathf.Lerp(0, PlayerPrefs.GetFloat("Music", 30.0f) / 100.0f, t / 5.0f);
            RenderSettings.fogDensity = Mathf.Lerp(0.2f, 0.4f, t / 10.0f);
            t += Time.deltaTime;
            yield return null;
        }
        yield return new WaitUntil(() => !MainManager.instance.IsExecutingTriggers());

        startPos = endPos;
        endPos = startPos + Vector3.right * 3.0f;
        MainManager.instance.PlayEffect(monsterOut);
        t = 0;
        while (t < monsterOut.length)
        {
            killer.transform.position = Vector3.Lerp(startPos, endPos, t / monsterOut.length);
            RenderSettings.fogDensity = Mathf.Lerp(0.4f, 0.8f, t / monsterOut.length);
            t += Time.deltaTime;
            yield return null;
        }
        MainManager.instance.PlayEffect(lightsOut);
        MainManager.instance.StopMusic();
        subTense.Stop();
        RenderSettings.ambientIntensity = 0;
        RenderSettings.fogDensity = 1.0f;
        yield return new WaitForSeconds(2.0f);
        yield return new WaitUntil(() => MainManager.instance.gameState == 1);
        MainManager.instance.SetPrompt("Press [xxxxx] to run", true);
        MainManager.instance.AddTrigger("dialogue;Dad;What is happening?!");
        MainManager.instance.AddTrigger("flashdialogue;Mom;......Please......Help......Me............And............;1");
        yield return new WaitForSeconds(1.0f);
        yield return new WaitUntil(() => !MainManager.instance.IsExecutingTriggers());
        pc.gameObject.SetActive(false);
        jumpscareCam.SetActive(true);
        RenderSettings.fogDensity = 0.7f;
        RenderSettings.ambientIntensity = 0.3f;
        MainManager.instance.PlayEffect(jumpscare);
        MainManager.instance.StopMusic();
        MainManager.instance.AddTrigger("wait;0.3");
        MainManager.instance.AddTrigger("changescreen;#FF0000FF;#FF0000FF;3");
        MainManager.instance.AddTrigger("changescreen;#FF0000FF;#000000FF;3");
        MainManager.instance.AddTrigger("wait;5");
        yield return new WaitForSeconds(0.3f);
        subTense.clip = tinnitus;
        subTense.Play();
        MainManager.instance.StopEffect();
        yield return new WaitForSeconds(3.0f);
        t = 0;
        while(t < 3.0f)
        {
            subTense.volume = Mathf.Lerp(PlayerPrefs.GetFloat("Music", 30.0f) / 100.0f, 0, t / 3.0f);
            t += Time.deltaTime;
            yield return null;
        }
        subTense.Stop();
        yield return new WaitForSeconds(2.0f);
        title.gameObject.SetActive(true);
        MainManager.instance.PlayMusic(horrorAmb);
        MainManager.instance.PlayEffect(jumpscare);
        yield return new WaitForSeconds(3.0f);
        t = 0;
        while (t < 3.0f)
        {
            title.color = Color.Lerp(Color.white, Color.clear, t / 3.0f);
            t += Time.deltaTime;
            yield return null;
        }
        title.color = Color.clear;
        MainManager.instance.DisplayEnding("FINAL ENDING 4/5 - We'll Be There.", "You managed to survive to the end. You went through all the hallucinations. You realized they would be here. You faced your ending calmly. You knew it was going to happen. You've paid back your mistake.");
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
