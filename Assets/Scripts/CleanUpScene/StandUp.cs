using System.Collections;
using UnityEngine;

public class StandUp : MonoBehaviour
{
    [SerializeField] private GameObject player;

    private void Start()
    {
        StartCoroutine(Wait());
    }

    private IEnumerator Wait()
    {
        MainManager.instance.AddTrigger("wait;2");
        MainManager.instance.AddTrigger("changescreen;#000000FF;#00000000;8");
        yield return new WaitForSeconds(10.0f);
        player.SetActive(true);
        MainManager.instance.AddTrigger("dialogue;You;What... what... what... happened?!");
        MainManager.instance.AddTrigger("dialogue;You;Shit... why... how... where did he pop up?!");
        MainManager.instance.AddTrigger("dialogue;You;Oh no. Oh no. FUCK.");
        MainManager.instance.AddTrigger("dialogue;You;This is it. I'm done. It's over.");
        MainManager.instance.AddTrigger("dialogue;You;......");
        MainManager.instance.AddTrigger("dialogue;You;Maybe... maybe... I can clean this up? ...... H-i-d-e him? Haha.");
        MainManager.instance.AddTrigger("dialogue;You;I... well... I should rush home to get this shit cleaned up. Damn it.");
        Destroy(gameObject);
    }
}