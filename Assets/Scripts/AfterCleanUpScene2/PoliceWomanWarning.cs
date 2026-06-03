using System.Collections;
using UnityEngine;

public class PoliceWomanWarning : MonoBehaviour
{
    [SerializeField] private AudioClip jumpScare;
    [SerializeField] private GameObject policeman;
    [SerializeField] private AudioSource ad;

    private Animator animator;
    private Animator animator2;

    private void Start()
    {
        animator = GetComponent<Animator>();
        animator2 = policeman.GetComponent<Animator>();
    }

    public void MoveOut()
    {
        StartCoroutine(Move());
    }

    private IEnumerator Move()
    {
        float t = 0;
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
        yield return new WaitForSeconds(1.3f);
        MainManager.instance.PlayEffect(jumpScare);
        yield return new WaitForSeconds(0.2f);

        t = 0;
        while(t < 0.1f)
        {
            transform.Translate(-Vector3.up * Time.deltaTime / 0.1f * 0.4f, Space.World);
            t += Time.deltaTime;
            yield return null;
        }
        RenderSettings.fogColor = Color.black;
        RenderSettings.fogDensity = 1.0f;
        RenderSettings.ambientIntensity = 0.5f;
        ad.Play();
        MainManager.instance.AddTrigger("wait;4");
        MainManager.instance.AddTrigger("dialogue;Policewoman;You might have got away this time. But... hehehe... you won't get away next time... and they will be here to FIND YOU... heheheheehahahahah");
        MainManager.instance.AddTrigger("dialogue;Policeman;What are you doing?! We need to leave!");
        yield return new WaitForSeconds(1.0f);
        yield return new WaitUntil(() => MainManager.instance.gameState == 1);
        MainManager.instance.AddTrigger("changescreen;#00000000;#00000099;2");
        MainManager.instance.AddTrigger("changescreen;#00000099;#00000000;2");
        MainManager.instance.AddTrigger("changescreen;#00000000;#00000099;2");
        MainManager.instance.AddTrigger("changescreen;#00000099;#00000000;2");
        MainManager.instance.AddTrigger("changescreen;#00000000;#00000099;2");
        MainManager.instance.AddTrigger("changescreen;#00000099;#00000000;2");
        MainManager.instance.AddTrigger("wait;2");
        MainManager.instance.AddTrigger("dialogue;You;Phew... Looks like I got away with it.");
        yield return new WaitForSeconds(2.0f);

        t = 0;
        while (t < 0.6f)
        {
            RenderSettings.fogColor = Color.Lerp(Color.black, Color.gray, t / 0.6f);
            RenderSettings.fogDensity = Mathf.Lerp(1.0f, 0.01f, t / 0.6f);
            RenderSettings.ambientIntensity = Mathf.Lerp(0.5f, 1.0f, t / 0.6f);
            transform.Translate(Vector3.up * Time.deltaTime / 0.6f * 0.4f, Space.World);
            t += Time.deltaTime;
            yield return null;
        }
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
            transform.Translate(Vector3.forward * Time.deltaTime / 2.5f * 1.7f, Space.World);
            t += Time.deltaTime;
            if(t > 1.5f)
            {
                animator2.SetBool("walking", true);
                policeman.transform.Rotate(Vector3.up * Time.deltaTime / 1.0f * 120.0f, Space.World);
            }
            yield return null;
        }
        t = 0;
        while (t < 4.0f)
        {
            transform.Translate(transform.forward * Time.deltaTime / 4.5f * 3.0f, Space.World);
            policeman.transform.Translate(transform.forward * Time.deltaTime / 4.0f * 3.5f, Space.World);
            if (t <= 3.0f) transform.Rotate(-Vector3.up * Time.deltaTime / 3.0f * 90.0f, Space.World);
            if(t >= 2.0f) policeman.transform.Rotate(-Vector3.up * Time.deltaTime / 2.0f * 40.0f, Space.World);
            t += Time.deltaTime;
            yield return null;
        }
        animator.SetBool("walking", false);
    }
}
