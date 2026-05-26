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
        animator.SetBool("walking", false);
        ad.Play();
        animator.SetBool("walking", true);
        while (t < 4.5f)
        {
            transform.Translate(-Vector3.forward * Time.deltaTime / 4.5f * 0.3f, Space.World);
            t += Time.deltaTime;
            yield return null;
        }
        animator.SetBool("walking", false);
        ad.Stop();
        yield return new WaitForSeconds(3.0f);

        t = 0;
        while(t < 0.2f)
        {
            transform.Translate(-Vector3.up * Time.deltaTime / 4.5f * 0.6f, Space.World);
            t += Time.deltaTime;
            yield return null;
        }

        MainManager.instance.PlayEffect(jumpScare);

        yield return new WaitForSeconds(0.5f);
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
