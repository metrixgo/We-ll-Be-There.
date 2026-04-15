using System.Collections;
using UnityEngine;

public class Microwave : MonoBehaviour
{
    [SerializeField] private AudioClip open;
    [SerializeField] private AudioClip close;
    [SerializeField] private AudioClip putIn;
    [SerializeField] private AudioClip microwaving;
    [SerializeField] private GameObject door;
    [SerializeField] private GameObject ramen;
    [SerializeField] private GameObject environment;
    [SerializeField] private GameObject table;

    private bool opened = false;
    private bool isTurning = false;
    private float maxRot = 95.0f;
    private float angSpeed = 230.0f;
    private int state = 0;
    private AudioSource ad;

    private void Start()
    {
        ad = GetComponent<AudioSource>();
    }

    public void Interact()
    {
        if (isTurning) return ;
        if (state == 0)
        {
             state++;
             isTurning = true;
             StartCoroutine(TurnDoor());
        }
        else if (state == 1)
        {
            if (!MainManager.instance.HasItem("Instant Ramen"))
            {
                state--;
                isTurning = true;
                StartCoroutine(TurnDoor());
            }
            else
            {
                ad.clip = putIn;
                ad.Play();
                ramen.transform.SetParent(environment.transform);
                ramen.transform.localPosition = new Vector3(-4.77f, 0.16f, 4.05f);
                ramen.transform.rotation = Quaternion.Euler(-90.0f, 0, 0);
                state++;
            }
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
            ad.clip = microwaving;
            ad.Play();
            MainManager.instance.ClearTasks();
        }
        else if (state == 4)
        {
            if (!ad.isPlaying)
            {
                state++;
                isTurning = true;
                StartCoroutine(TurnDoor());
            }
        }
        else if (state == 5)
        {
            state++;
            ramen.GetComponent<PickUpItem>().PickUp();
            table.SetActive(true);
            MainManager.instance.AddTrigger("wait;0.5");
            MainManager.instance.AddTrigger("dialogue;You;Okay, now I should put it on the table and eat it.");
            MainManager.instance.AddTrigger("cleartasks");
            MainManager.instance.AddTrigger("task;Put the instant ramen on the table");
        }
        else if (state == 6)
        {
            isTurning = true;
            StartCoroutine(TurnDoor());
        }
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
