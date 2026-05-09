using System.Collections;
using UnityEngine;

public class WashingMachine : MonoBehaviour
{
    [SerializeField] private AudioClip open;
    [SerializeField] private AudioClip close;
    [SerializeField] private AudioClip putIn;
    [SerializeField] private AudioClip washing;
    [SerializeField] private AudioClip beep;
    [SerializeField] private GameObject door;
    [SerializeField] private GameObject clothes;

    private bool opened = false;
    private bool isTurning = false;
    private float maxRot = 95.0f;
    private float angSpeed = 230.0f;
    private float duration = 60.0f;
    private bool finished = false;
    private int state = 0;
    private AudioSource ad;

    private void Start()
    {
        ad = GetComponent<AudioSource>();
    }

    public void Interact()
    {
        if (MainManager.instance.gameState != 1 || isTurning) return ;

        if (state == 0)
        {
            state++;
            isTurning = true;
            StartCoroutine(TurnDoor());
        }
        else if (state == 1)
        {
            state++;
            clothes.SetActive(true);
            ad.clip = putIn;
            ad.Play();
        }
        else if (state == 2)
        {
            state++;
            isTurning = true;
            StartCoroutine(TurnDoor());
        }
        else if (state == 3)
        {
            state++;
            StartCoroutine(Wash());
        }
        else if (state == 4)
        {
            if (!finished) MainManager.instance.AddTrigger("dialogue;You;It's still washing...");
            else MainManager.instance.AddTrigger("It's done. I think I'll just leave the clothes in there.");
        }

    }

    private IEnumerator Wash()
    {
        ad.clip = beep;
        ad.Play();
        yield return new WaitForSeconds(beep.length + 0.1f);
        ad.clip = washing;
        ad.Play();
        while (duration > 0) {
            if(!MainManager.instance.AtPausedScreen()) duration -= Time.deltaTime;
            if(!ad.isPlaying) ad.Play();
            yield return null;
        }
        finished = true;
        ad.clip = beep;
        ad.Play();
    }

    private IEnumerator TurnDoor()
    {
        if (opened) ad.clip = close;
        else ad.clip = open;
        ad.Play();
        float rot = 0;
        Vector3 angles = door.transform.eulerAngles;
        float goal = angles.y;
        if (!opened)
        {
            goal += maxRot;
            while (rot < maxRot)
            {
                rot += angSpeed * Time.deltaTime;
                door.transform.Rotate(0, angSpeed * Time.deltaTime, 0, Space.World);
                yield return null;
            }
            door.transform.rotation = Quaternion.Euler(angles.x, goal, angles.z);
        }
        else
        {
            goal -= maxRot;
            while (rot < maxRot)
            {
                rot += angSpeed * Time.deltaTime;
                door.transform.Rotate(0, -angSpeed * Time.deltaTime, 0, Space.World);
                yield return null;
            }
            door.transform.rotation = Quaternion.Euler(angles.x, goal, angles.z);
        }
        opened = !opened;
        isTurning = false;
    }
}
