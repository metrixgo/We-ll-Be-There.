using System.Collections;
using System.Threading;
using UnityEngine;

public class DrunkMan : MonoBehaviour
{
    private float speed = 7.0f;
    private AudioSource ad;
    private Animator anim;

    private void Start()
    {
        ad = GetComponent<AudioSource>();
        anim = GetComponent<Animator>();
        anim.speed = 0;
    }

    public void StartMoving()
    {
        ad.Play();
        anim.speed = 1.0f;
        StartCoroutine(Move());
    }

    private IEnumerator Move()
    {
        float t = 0;
        while (t < 5.0f)
        {
            if (MainManager.instance.gameState != 1)
            {
                anim.speed = 0;
                yield return null;
                continue;
            }
            anim.speed = 1.0f;
            t += Time.deltaTime;
            transform.Translate(transform.forward * speed * Time.deltaTime, Space.World);
            yield return null;
        }
    }
}
