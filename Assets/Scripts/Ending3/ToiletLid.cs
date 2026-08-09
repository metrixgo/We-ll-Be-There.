using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class ToiletLid : MonoBehaviour
{
    [SerializeField] private AudioClip tense;
    [SerializeField] private AudioClip jumpscare;
    [SerializeField] private Image screen;
    [SerializeField] private GameObject head;

    private bool opened = false;
    private bool isTurning = false;
    private bool turned = false;
    private AudioSource ad;

    private void Start()
    {
        ad = GetComponent<AudioSource>();
    }

    public void InteractLid()
    {
        if (!isTurning)
        {
            isTurning = true;
            StartCoroutine(Turn());
        }
    }

    private IEnumerator Turn()
    {
        ad.Play();
        float rot = 0;
        Vector3 angles = transform.eulerAngles;
        float goal = angles.x;
        if (!opened)
        {
            goal += 90.0f;
            while (rot < 90.0f)
            {
                rot += 150.0f * Time.deltaTime;
                transform.Rotate(-150.0f * Time.deltaTime, 0, 0, Space.World);
                yield return null;
            }
        }
        else
        {
            goal -= 90.0f;
            while (rot < 90.0f)
            {
                rot += 150.0f * Time.deltaTime;
                transform.Rotate(150.0f * Time.deltaTime, 0, 0, Space.World);
                yield return null;
            }
        }
        transform.rotation = Quaternion.Euler(goal, angles.y, angles.z);
        opened = !opened;
        isTurning = false;

        if (!turned)
        {
            turned = true;
            MainManager.instance.PlayMusic(tense);
            MainManager.instance.PlayEffect(jumpscare);
            Destroy(head);
            float t = 0;
            while (t < 3.0f)
            {
                screen.color = Color.Lerp(Color.red * 0.6f, Color.red * 0.2f, t / 3.0f);
                t += Time.deltaTime;
                yield return null;
            }
            screen.color = Color.red * 0.2f;
            t = 0;
        }

    }
}
