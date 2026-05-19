using System.Collections;
using UnityEditor.VersionControl;
using UnityEngine;
using UnityEngine.UI;

public class Ending2Manager : MonoBehaviour
{
    [SerializeField] private AudioClip glassBreak;
    [SerializeField] private AudioClip openDoor;
    [SerializeField] private AudioClip doorKnock;
    [SerializeField] private AudioClip policeRun;
    [SerializeField] private AudioClip hit;
    [SerializeField] private AudioClip tinnitus;
    [SerializeField] private AudioSource cars;
    [SerializeField] private GameObject firstPlayer;
    [SerializeField] private GameObject secondPlayer;
    [SerializeField] private GameObject player;
    [SerializeField] private GameObject police;
    [SerializeField] private GameObject policeman;
    [SerializeField] private GameObject hammer;
    [SerializeField] private RawImage ri;
    [SerializeField] private Material mat;
    [SerializeField] private RuntimeAnimatorController controller;
    [SerializeField] private Avatar idleAv;

    private bool opened = false;
    private bool isTurning = false;
    private float maxRot = 95.0f;
    private float angSpeed = 150.0f;
    private AudioSource ad;
    private PlayerController pc;

    private void Start()
    {
        pc = player.GetComponent<PlayerController>();
        ad = GetComponent<AudioSource>();
        StartCoroutine(EndIt());
    }

    public void InteractDoor()
    {
        if (!isTurning)
        {
            isTurning = true;
            StartCoroutine(OpenDoor());
        }
    }

    private IEnumerator OpenDoor()
    {
        MainManager.instance.AddTrigger("wait;6.6");
        MainManager.instance.AddTrigger("changescreen;#00000000;#ff0000ff;0");
        MainManager.instance.AddTrigger("wait;0.2");
        MainManager.instance.AddTrigger("changescreen;#ff0000ff;#000000ff;2");
        MainManager.instance.AddTrigger("wait;3");
        MainManager.instance.AddTrigger("changescreen;#000000ff;#00000088;5");
        MainManager.instance.AddTrigger("flashdialogue;Policewoman;I got him. This thing can finally end... How did you know it was him?;2");
        string s = "";
        if (CleanUpClock.errorType == "mop") s = "We found his mop on the second floor covered in blood. We believe it was used to clean up the crime scene.";
        else if (CleanUpClock.errorType == "shovel") s = "We found his shovel in the garage covered with plastic fibers and blood. We believe it was used to hide the body.";
        else if (CleanUpClock.errorType == "clothes") s = "We found his clothes with blood traces on them. We believe it was from the crime scene.";
        else if (CleanUpClock.errorType == "covered") s = "We found a suspicious spot in the backyard. We dug down and saw the actual body.";
        else if (CleanUpClock.errorType == "blood") s = "We used ultraviolet lights to find blood traces on the ground. We believe it was from the crime scene that he forgot to clean up.";
        else if (CleanUpClock.errorType == "mopbucket") s = "We found a mop bucket on the first floor storage closet with blood in it. We believe it was used to clean up blood on items.";

        yield return new WaitForSeconds(1.0f);
        cars.Stop();
        MainManager.instance.StopMusic();
        StartCoroutine(Turn());
        yield return new WaitForSeconds(3.0f);
        Vector3 startPos = player.transform.Find("Camera").position;
        Vector3 startRot = new Vector3(
            player.transform.Find("Camera").eulerAngles.x,
            player.transform.eulerAngles.y,
            player.transform.eulerAngles.z
        );
        player.SetActive(false);
        secondPlayer.SetActive(true);
        Vector3 endPos = new Vector3(-51.5f, 2.25f, -71.15f);
        Vector3 endRot = new Vector3(5.0f, 100.0f, 0);
        secondPlayer.transform.position = startPos;
        secondPlayer.transform.rotation = Quaternion.Euler(startRot);
        float t = 0;
        while(t < 1.0f)
        {
            secondPlayer.transform.position = Vector3.Lerp(startPos, endPos, t);
            secondPlayer.transform.rotation = Quaternion.Slerp(Quaternion.Euler(startRot), Quaternion.Euler(endRot), t);
            t += Time.deltaTime;
            yield return null;
        }
        startPos = endPos;
        startRot = endRot;
        endPos = new Vector3(-51.7f, 2.2f, -71.1f);
        endRot = new Vector3(10.0f, 80.0f, 0);
        MainManager.instance.PlayEffect(policeRun);
        t = 0;
        bool flg = false;
        while (t < 0.9f)
        {
            if(t > 0.7f && !flg)
            {
                flg = true;
                police.SetActive(true);
            }
            secondPlayer.transform.position = Vector3.Lerp(startPos, endPos, t / 0.9f);
            secondPlayer.transform.rotation = Quaternion.Slerp(Quaternion.Euler(startRot), Quaternion.Euler(endRot), t / 0.9f);
            t += Time.deltaTime;
            yield return null;
        }
        startRot = endRot;
        endRot = new Vector3(0, 0, 0);
        t = 0;
        while (t < 0.2f)
        {
            secondPlayer.transform.rotation = Quaternion.Euler(Vector3.Lerp(startRot, endRot, t / 0.2f));
            t += Time.deltaTime;
            yield return null;
        }
        yield return new WaitForSeconds(0.2f);
        MainManager.instance.PlayEffect(hit);
        yield return new WaitForSeconds(1.0f);
        ri.material = mat;
        police.GetComponent<Animator>().runtimeAnimatorController = controller;
        police.GetComponent<Animator>().avatar = idleAv;
        police.transform.position = new Vector3(-51.78f, 0.68f, -69.32f);
        police.transform.rotation = new Quaternion(0f, -0.84f, 0f, -0.55f);
        policeman.SetActive(true);
        Destroy(hammer);
        yield return new WaitForSeconds(4.2f);

        ad.spatialBlend = 0;
        ad.clip = tinnitus;
        ad.loop = true;
        ad.volume = 0;
        ad.Play();
        t = 0;
        while (t < 5.0f)
        {
            ad.volume = Mathf.Lerp(0, PlayerPrefs.GetFloat("Effects", 80.0f) / 500.0f, t / 4.0f);
            t += Time.deltaTime;
            yield return null;
        }
        ad.volume = PlayerPrefs.GetFloat("Effects", 80.0f) / 500.0f;
    }

    private IEnumerator EndIt()
    {
        MainManager.instance.AddTrigger("wait;2");
        MainManager.instance.AddTrigger("changescreen;#000000ff;#00000000;1");
        MainManager.instance.AddTrigger("wait;10");
        yield return new WaitForSeconds(1.0f);
        MainManager.instance.PlayEffect(glassBreak);
        yield return new WaitForSeconds(3.0f);
        ad.clip = doorKnock;
        ad.Play();
        yield return new WaitForSeconds(9.0f);
        Destroy(firstPlayer);
        player.SetActive(true);
        pc.Freeze(true);
    }

    private IEnumerator Turn()
    {
        ad.clip = openDoor;
        ad.Play();
        float rot = 0;
        Vector3 angles = transform.eulerAngles;
        float goal = angles.y;
        if (!opened)
        {
            goal += maxRot;
            while (rot < maxRot)
            {
                rot += angSpeed * Time.deltaTime;
                transform.Rotate(0, angSpeed * Time.deltaTime, 0, Space.World);
                yield return null;
            }
            transform.rotation = Quaternion.Euler(angles.x, goal, angles.z);
        }
        else
        {
            goal -= maxRot;
            while (rot < maxRot)
            {
                rot += angSpeed * Time.deltaTime;
                transform.Rotate(0, -angSpeed * Time.deltaTime, 0, Space.World);
                yield return null;
            }
            transform.rotation = Quaternion.Euler(angles.x, goal, angles.z);
        }
        opened = !opened;
        isTurning = false;
    }
}
