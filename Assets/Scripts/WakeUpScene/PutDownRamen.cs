using System.Collections;
using UnityEngine;

public class PutDownRamen : MonoBehaviour
{
    [SerializeField] private Material empty;
    [SerializeField] private AudioClip eat;
    [SerializeField] private GameObject ramen;
    [SerializeField] private GameObject environment;
    [SerializeField] private GameObject police;

    private int state = 0;

    public void Interact()
    {
        if(state == 0)
        {
            state++;
            ramen.transform.SetParent(environment.transform);
            ramen.transform.localPosition = new Vector3(-1.55f, 0.05f, 1.65f);
            ramen.transform.rotation = Quaternion.Euler(-90.0f, 0, 0);
            name = "Eat";
            MainManager.instance.SetPrompt("Eat");
            MainManager.instance.ClearTasks();
            MainManager.instance.AddTask("Eat the instant ramen");
        }
        else
        {
            MainManager.instance.ClearTasks();
            StartCoroutine(Eat());
            state++;
        }
    }

    private IEnumerator Eat()
    {
        MainManager.instance.AddTrigger("changescreen;#00000000;#000000FF;1");
        MainManager.instance.AddTrigger("wait;11");
        MainManager.instance.AddTrigger("changescreen;#000000FF;#00000000;1");
        MainManager.instance.AddTrigger("wait;0.5");
        MainManager.instance.AddTrigger("dialogue;You;Mmm... Very good.");
        tag = "Untagged";
        yield return new WaitForSeconds(1.5f);
        MainManager.instance.PlayEffect(eat);
        ramen.GetComponent<Renderer>().material = empty;
        yield return new WaitUntil(() => MainManager.instance.gameState == 1);
        yield return new WaitForSeconds(1.0f);
        police.SetActive(true);
        police.GetComponent<AudioSource>().Play();
        Destroy(gameObject);
    }
}
