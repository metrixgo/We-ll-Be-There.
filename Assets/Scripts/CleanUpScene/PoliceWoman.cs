using System.Collections;
using UnityEngine;

public class PoliceWoman : MonoBehaviour
{
    [SerializeField] private Interactable door;
    
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
        door.Interact();
        float t = 0;
        animator.SetBool("walking", false);
        yield return new WaitForSeconds(2.0f);
        ad.Play();
        animator.SetBool("walking", true);
        while (t < 4.0f)
        {
            transform.Translate(Vector3.left * Time.deltaTime * 2.0f, Space.World);
            t+= Time.deltaTime;
            yield return null;
        }
        animator.SetBool("walking", false);
        ad.Stop();
        yield return new WaitUntil(() => MainManager.instance.gameState == 1);

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
        door.Interact();
        animator.SetBool("walking", false);
        ad.Stop();
    }
}
