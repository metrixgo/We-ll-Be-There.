using System.Collections;
using UnityEngine;

public class TenthBook : MonoBehaviour
{
    [SerializeField] private AudioClip teleport;
    [SerializeField] private Transform corBook;

    private string to;
    private string back;

    private void Start()
    {
        float x = corBook.position.x - transform.position.x;
        float y = corBook.position.y - transform.position.y;
        float z = corBook.position.z - transform.position.z;
        to = x + ";" + y + ";" + z;
        back = (-x) + ";" + (-y) + ";" + (-z);
    }

    public void Read()
    {
        StartCoroutine(Teleport());
    }

    private IEnumerator Teleport()
    {
        MainManager.instance.AddTrigger("wait;1");
        MainManager.instance.AddTrigger("moveplayer;" + to);
        MainManager.instance.AddTrigger("wait;1.5");
        MainManager.instance.AddTrigger("waitesc");
        MainManager.instance.PlayEffect(teleport);
        yield return new WaitForSeconds(1.0f);
        RenderSettings.fogDensity = 1.0f;
        RenderSettings.ambientIntensity = 0.6f;
        yield return new WaitForSeconds(1.5f);
        DisplayBook.instance.DisplayPage(10);
        yield return new WaitUntil(() => MainManager.instance.gameState == 1);

        MainManager.instance.AddTrigger("wait;1");
        MainManager.instance.AddTrigger("moveplayer;" + back);
        MainManager.instance.AddTrigger("wait;1.5");
        MainManager.instance.PlayEffect(teleport);
        yield return new WaitForSeconds(1.0f);
        RenderSettings.fogDensity = 0.3f;
        RenderSettings.ambientIntensity = 1.0f;
        yield return new WaitForSeconds(1.5f);
    }
}
