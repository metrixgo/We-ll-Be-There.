using System.Collections;
using UnityEngine;

public class Ending2Manager : MonoBehaviour
{
    [SerializeField] private AudioClip glassBreak;
    [SerializeField] private AudioClip doorKnock;
    [SerializeField] private GameObject firstPlayer;
    [SerializeField] private GameObject player;

    private bool opened = false;
    private bool isTurning = false;
    private float maxRot = 95.0f;
    private float angSpeed = 150.0f;
    private AudioSource ad;
    private PlayerController pc;

    private void Start()
    {
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
        MainManager.instance.AddTrigger("wait;1");
        yield return new WaitForSeconds(1.0f);
        pc.LookAt(transform.position, 0.2f);
        yield return StartCoroutine(Turn());

    }

    private IEnumerator EndIt()
    {
        MainManager.instance.AddTrigger("wait;2");
        MainManager.instance.AddTrigger("changescreen;#000000ff;#00000000;1");
        MainManager.instance.AddTrigger("wait;10");
        ad.clip = glassBreak;
        ad.Play();
        yield return new WaitForSeconds(13.0f);
        Destroy(firstPlayer);
        player.SetActive(true);
        pc.Freeze(true);
    }

    private IEnumerator Turn()
    {
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
