using System.Collections;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem.iOS;

public class Ending2Manager : MonoBehaviour
{
    [SerializeField] private AudioClip glassBreak;
    [SerializeField] private AudioClip openDoor;
    [SerializeField] private AudioClip doorKnock;
    [SerializeField] private AudioClip policeRun;
    [SerializeField] private AudioClip hit;
    [SerializeField] private AudioSource cars;
    [SerializeField] private GameObject firstPlayer;
    [SerializeField] private GameObject secondPlayer;
    [SerializeField] private GameObject player;
    [SerializeField] private GameObject police;

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
        MainManager.instance.AddTrigger("changescreen;#ff0000ff;#00000000;2");
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
        Vector3 endPos = new Vector3(-51.5f, 2.25f, -70.9f);
        Vector3 endRot = new Vector3(10.0f, 90.0f, 0);
        secondPlayer.transform.position = startPos;
        secondPlayer.transform.rotation = Quaternion.Euler(startRot);
        float t = 0;
        while(t < 0.7f)
        {
            secondPlayer.transform.position = Vector3.Lerp(startPos, endPos, t / 0.7f);
            secondPlayer.transform.rotation = Quaternion.Slerp(Quaternion.Euler(startRot), Quaternion.Euler(endRot), t / 0.7f);
            t += Time.deltaTime;
            yield return null;
        }
        MainManager.instance.PlayEffect(policeRun);
        startPos = endPos;
        startRot = endRot;
        endPos = new Vector3(-51.7f, 2.2f, -70.8f);
        endRot = new Vector3(20.0f, 80.0f, 0);
        t = 0;
        bool flg = false;
        while (t < 1.2f)
        {
            if(t > 1.0f && !flg)
            {
                flg = true;
                police.SetActive(true);
            }
            secondPlayer.transform.position = Vector3.Lerp(startPos, endPos, t / 1.2f);
            secondPlayer.transform.rotation = Quaternion.Slerp(Quaternion.Euler(startRot), Quaternion.Euler(endRot), t / 1.2f);
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
