using System.Collections;
using UnityEngine;

public class AfterManager : MonoBehaviour
{
    [SerializeField] private AudioClip glassBreak;
    [SerializeField] private AudioClip openDoor;
    [SerializeField] private AudioClip doorKnock;
    [SerializeField] private AudioClip policeRun;
    [SerializeField] private AudioClip jumpScare;
    [SerializeField] private AudioSource cars;
    [SerializeField] private GameObject firstPlayer;
    [SerializeField] private GameObject secondPlayer;
    [SerializeField] private GameObject player;
    [SerializeField] private GameObject police;
    [SerializeField] private GameObject policeman;

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
        MainManager.instance.AddTrigger("wait;8");
        MainManager.instance.AddTrigger("dialogue;Policewoman;Stop where you are!");
        MainManager.instance.AddTrigger("dialogue;Policewoman;We are now arresting you for committing first-degree murder!");
        MainManager.instance.AddTrigger("dialogue;Policeman;Um... Sorry...");
        MainManager.instance.AddTrigger("dialogue;Policewoman;What?");
        MainManager.instance.AddTrigger("dialogue;Policeman;I don't think we have any direct evidence to arrest him...");
        MainManager.instance.AddTrigger("dialogue;Policewoman;What are you saying?! We knew it was him!!");
        MainManager.instance.AddTrigger("dialogue;Policeman;No... We couldn't find anything...");
        MainManager.instance.AddTrigger("dialogue;Policewoman;I... I saw him last night!!! He... he was... he was definitely cleaning up the crime scene!!!");
        MainManager.instance.AddTrigger("dialogue;Policeman;We cannot convict him just because you saw him. We have to leave now, since we didn't find anything.");
        MainManager.instance.AddTrigger("dialogue;Policewoman;NO NO NO do the search again!!!");
        MainManager.instance.AddTrigger("dialogue;Policeman;If we stay here, we will be the ones committing crimes.");
        MainManager.instance.AddTrigger("dialogue;Policewoman;......");
        MainManager.instance.AddTrigger("dialogue;Policewoman;Fine...");
        MainManager.instance.AddTrigger("wait;8");
        MainManager.instance.AddTrigger("dialogue;Policewoman;You might have got away this time. But... hehehe... you won't get away next time... and they will be here to FIND YOU... heheheheehahahahah");
        MainManager.instance.AddTrigger("dialogue;Policeman;What are you doing?! We need to leave!");
        MainManager.instance.AddTrigger("wait;8");

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
        while (t < 1.0f)
        {
            secondPlayer.transform.position = Vector3.Lerp(startPos, endPos, t);
            secondPlayer.transform.rotation = Quaternion.Slerp(Quaternion.Euler(startRot), Quaternion.Euler(endRot), t);
            t += Time.deltaTime;
            yield return null;
        }
        startPos = endPos;
        startRot = endRot;
        endPos = new Vector3(-51.6f, 2.2f, -71.1f);
        endRot = new Vector3(10.0f, 80.0f, 0);
        MainManager.instance.PlayEffect(policeRun);
        t = 0;
        while (t < 0.9f)
        {
            secondPlayer.transform.position = Vector3.Lerp(startPos, endPos, t / 0.9f);
            secondPlayer.transform.rotation = Quaternion.Slerp(Quaternion.Euler(startRot), Quaternion.Euler(endRot), t / 0.9f);
            t += Time.deltaTime;
            yield return null;
        }
        startRot = endRot;
        endRot = new Vector3(0, 0, 0);
        police.SetActive(true);
        policeman.SetActive(true);
        t = 0;
        MainManager.instance.PlayEffect(jumpScare);
        while (t < 0.2f)
        {
            secondPlayer.transform.rotation = Quaternion.Euler(Vector3.Lerp(startRot, endRot, t / 0.2f));
            t += Time.deltaTime;
            yield return null;
        }


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
