using System.Collections;
using UnityEngine;

public class PoliceWomanWarning : MonoBehaviour
{
    [SerializeField] private AudioClip jumpScare;

    private Animator animator;
    private AudioSource ad;

    private void Start()
    {
        animator = GetComponent<Animator>();
        ad = GetComponent<AudioSource>();
        ad.pitch = 0.5f;
    }

    public void MoveOut()
    {
        StartCoroutine(Move());
    }

    private IEnumerator Move()
    {
        float t = 0;
        ad.Play();
        animator.SetBool("walking", true);
        while (t < 2.5f)
        {
            if(MainManager.instance.gameState == 1)
            {
                transform.Translate(-Vector3.forward * Time.deltaTime / 2.5f * 0.3f, Space.World);
                t += Time.deltaTime;
            }
            yield return null;
        }
        animator.SetBool("walking", false);
        ad.Stop();
        yield return new WaitForSeconds(2.5f);

        t = 0;
        bool flg = false;
        while(t < 0.1f)
        {
            if(t > 0.05f && !flg)
            {
                flg = true;
                MainManager.instance.PlayEffect(jumpScare);
            }
            transform.Translate(-Vector3.up * Time.deltaTime / 0.1f * 0.6f, Space.World);
            t += Time.deltaTime;
            yield return null;
        }

        MainManager.instance.AddTrigger("wait;4");
        MainManager.instance.AddTrigger("dialogue;Policewoman;You might have got away this time. But... hehehe... you won't get away next time... and they will be here to FIND YOU... heheheheehahahahah");
        MainManager.instance.AddTrigger("dialogue;Policeman;What are you doing?! We need to leave!");

        yield return new WaitUntil(() => MainManager.instance.gameState == 1);
        yield return new WaitForSeconds(2.0f);

        t = 0;
        while (t < 0.6f)
        {
            transform.Translate(Vector3.up * Time.deltaTime / 0.6f * 0.6f, Space.World);
            t += Time.deltaTime;
            yield return null;
        }
        ad.Play();
        animator.SetBool("walking", true);
        t = 0;
        while (t < 1.0f)
        {
            transform.Rotate(Vector3.up * Time.deltaTime * 180.0f, Space.World);
            t += Time.deltaTime;
            yield return null;
        }
        t = 0;
        while (t < 4.0f)
        {
            transform.Translate(Vector3.right * Time.deltaTime * 2.0f, Space.World);
            t += Time.deltaTime;
            yield return null;
        }
        animator.SetBool("walking", false);
        ad.Stop();
    }
}
