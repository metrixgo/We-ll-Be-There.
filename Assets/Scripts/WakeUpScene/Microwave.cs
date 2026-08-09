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
    [SerializeField] private GameObject lit;
    [SerializeField] private GameObject table;

    private bool opened = false;
    private bool isTurning = false;
    private int state = 0;
    private AudioSource ad;

    private void Start()
    {
        ad = GetComponent<AudioSource>();
    }

    public void Interact()
    {
        if (isTurning) return;
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
                ramen.transform.SetParent(null);
                ramen.transform.position = new Vector3(-58.5575905f, 1.75140953f, -63.9500008f);
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
            StartCoroutine(Heat());
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

    private IEnumerator Heat()
    {
        float t = 0;
        lit.SetActive(true);
        while (t < microwaving.length)
        {
            ramen.transform.Rotate(Vector3.up * 45.0f * Time.deltaTime, Space.World);
            t += Time.deltaTime;
            yield return null;
        }
        lit.SetActive(false);
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
            goal += 95.0f;
            while (rot < 95.0f)
            {
                rot += 230.0f * Time.deltaTime;
                door.transform.Rotate(0, 230.0f * Time.deltaTime, 0, Space.World);
                yield return null;
            }
        }
        else
        {
            goal -= 95.0f;
            while (rot < 95.0f)
            {
                rot += 230.0f * Time.deltaTime;
                door.transform.Rotate(0, -230.0f * Time.deltaTime, 0, Space.World);
                yield return null;
            }
        }
        door.transform.rotation = Quaternion.Euler(angles.x, goal, angles.z);
        opened = !opened;
        isTurning = false;
    }
}
