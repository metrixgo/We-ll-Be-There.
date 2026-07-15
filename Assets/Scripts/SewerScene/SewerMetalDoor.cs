using System.Collections;
using UnityEngine;

public class SewerMetalDoor : MonoBehaviour
{
    [SerializeField] private int state = 0;
    [SerializeField] private string keyName;
    [SerializeField] private AudioClip banging;
    [SerializeField] private AudioClip unlock;
    [SerializeField] private AudioClip open;
    [SerializeField] private AudioClip close;

    private bool firstTimeTry = true;
    private AudioSource ad;

    private void Start()
    {
        ad = GetComponent<AudioSource>();
    }

    public void InteractDoor()
    {
        if (MainManager.instance.gameState != 1 || state == -1) return;

        if (state == 0)
        {
            if (MainManager.instance.HasItem(keyName))
            {
                ad.clip = unlock;
                ad.Play();
                state = 1;
                MainManager.instance.AddTrigger("wait;" + unlock.length);
            }
            else
            {
                ad.clip = banging;
                ad.Play();
                StartCoroutine(TryLocked());
                if (firstTimeTry)
                {
                    MainManager.instance.AddTrigger("dialogue;You;It's locked.");
                    firstTimeTry = false;
                }
            }
        }
        else if (state == 1 || state == 2)
        {
            if (state == 1)
            {
                state = 2;
                ad.clip = open;
            }
            else
            {
                state = 1;
                ad.clip = close;
            }
            ad.Play();
            StartCoroutine(Turn());
        }
    }

    private IEnumerator TryLocked()
    {
        state = -1;
        float rot = 0;
        Vector3 angles = transform.eulerAngles;
        float goal = angles.y;
        while (rot < 7.0f)
        {
            rot += 80.0f * Time.deltaTime;
            transform.Rotate(0, 80.0f * Time.deltaTime, 0, Space.World);
            yield return null;
        }
        rot = 0;
        while (rot < 7.0f)
        {
            rot += 30.0f * Time.deltaTime;
            transform.Rotate(0, -30.0f * Time.deltaTime, 0, Space.World);
            yield return null;
        }
        transform.rotation = Quaternion.Euler(angles.x, goal, angles.z);
        state = 0;
    }

    private IEnumerator Turn()
    {
        float rot = 0;
        Vector3 angles = transform.eulerAngles;
        float goal = angles.y;
        if (state == 2)
        {
            state = -1;
            goal += 95.0f;
            while (rot < 95.0f)
            {
                rot += 150.0f * Time.deltaTime;
                transform.Rotate(0, 150.0f * Time.deltaTime, 0, Space.World);
                yield return null;
            }
            state = 2;
        }
        else
        {
            state = -1;
            goal -= 95.0f;
            while (rot < 95.0f)
            {
                rot += 150.0f * Time.deltaTime;
                transform.Rotate(0, -150.0f * Time.deltaTime, 0, Space.World);
                yield return null;
            }
            state = 1;
        }

        transform.rotation = Quaternion.Euler(angles.x, goal, angles.z);
    }
}
