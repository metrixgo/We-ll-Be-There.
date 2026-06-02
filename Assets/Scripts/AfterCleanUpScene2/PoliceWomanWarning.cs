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
                transform.Translate(-Vector3.forward * Time.deltaTime / 2.5f * 0.97f, Space.World);
                t += Time.deltaTime;
            }
            yield return null;
        }
        animator.SetBool("walking", false);
        ad.Stop();
        yield return new WaitForSeconds(2.2f);
        MainManager.instance.PlayEffect(jumpScare);
        yield return new WaitForSeconds(0.3f);

        t = 0;
        while(t < 0.1f)
        {
            transform.Translate(-Vector3.up * Time.deltaTime / 0.1f * 0.37f, Space.World);
            t += Time.deltaTime;
            yield return null;
        }
        RenderSettings.fogColor = Color.black;
        RenderSettings.fogDensity = 1.0f;
        RenderSettings.ambientIntensity = 0.5f;

        MainManager.instance.AddTrigger("wait;4");
        MainManager.instance.AddTrigger("dialogue;Policewoman;You might have got away this time. But... hehehe... you won't get away next time... and they will be here to FIND YOU... heheheheehahahahah");
        MainManager.instance.AddTrigger("dialogue;Policeman;What are you doing?! We need to leave!");

        yield return new WaitForSeconds(4.0f);
        yield return new WaitUntil(() => MainManager.instance.gameState == 1);
        yield return new WaitForSeconds(2.0f);

        t = 0;
        while (t < 0.6f)
        {
            RenderSettings.fogColor = Color.Lerp(Color.black, Color.gray, t / 0.6f);
            RenderSettings.fogDensity = Mathf.Lerp(1.0f, 0.01f, t / 0.6f);
            RenderSettings.ambientIntensity = Mathf.Lerp(0.5f, 1.0f, t / 0.6f);
            transform.Translate(Vector3.up * Time.deltaTime / 0.6f * 0.37f, Space.World);
            t += Time.deltaTime;
            yield return null;
        }
        ad.Play();
        animator.SetBool("walking", true);
        t = 0;
        while (t < 0.5f)
        {
            transform.Rotate(Vector3.up * Time.deltaTime / 0.5f * 180.0f, Space.World);
            t += Time.deltaTime;
            yield return null;
        }
        t = 0;
        while (t < 2.5f)
        {
            if (MainManager.instance.gameState == 1)
            {
                transform.Translate(Vector3.forward * Time.deltaTime / 2.5f * 2.0f, Space.World);
                t += Time.deltaTime;
            }
            yield return null;
        }
        animator.SetBool("walking", false);
        ad.Stop();
    }
}
