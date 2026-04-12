using System.Collections;
using UnityEngine;

public class PutDownRamen : MonoBehaviour
{
    [SerializeField] private Material empty;
    [SerializeField] private AudioClip eat;
    [SerializeField] private GameObject ramen;
    [SerializeField] private GameObject environment;

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
            MainManager.instance.SetTask("Eat the instant ramen");
        }
        else
        {
            MainManager.instance.SetTask("");
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
        MainManager.instance.AddTrigger("dialogue;You;That's all for now.");
        yield return new WaitForSeconds(1.5f);
        MainManager.instance.PlayEffect(eat);
        ramen.GetComponent<Renderer>().material = empty;
        Destroy(gameObject);
    }
}
