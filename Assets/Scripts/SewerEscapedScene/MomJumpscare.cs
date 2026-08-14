using System.Collections;
using UnityEngine;

public class MomJumpscare : MonoBehaviour
{
    [SerializeField] private PlayerController pc;
    [SerializeField] private Transform momHead;
    [SerializeField] private AudioClip jumpscare;

    private bool interacted = false;

    public void LookAtHead()
    {
        if (!interacted)
        {
            interacted = true;
            StartCoroutine(LookHead());
        }
    }

    private IEnumerator LookHead()
    {
        MainManager.instance.AddTrigger("wait;2");
        MainManager.instance.AddTrigger("dialogue;You;M...Mom?!");
        yield return new WaitForSeconds(0.5f);
        MainManager.instance.PlayEffect(jumpscare);
        pc.LookAt(momHead.position, 0.2f);
    }
}
