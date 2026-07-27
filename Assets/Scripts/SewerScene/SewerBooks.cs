using System.Collections;
using UnityEngine;

public class SewerBooks : MonoBehaviour
{
    [SerializeField] private AudioClip teleport;
    [SerializeField] private AudioSource clock;
    [SerializeField] private Transform corBook;

    private static int num = 0;

    private bool alreadyRead = false;
    private string to;
    private string back;
    private int page;
    private float clockV;

    private void Start()
    {
        float x = corBook.position.x - transform.position.x;
        float y = corBook.position.y - transform.position.y;
        float z = corBook.position.z - transform.position.z;
        to = x + ";" + y + ";" + z;
        back = (-x) + ";" + (-y) + ";" + (-z);
        clockV = clock.volume;
    }

    public void Read()
    {
        if (!alreadyRead)
        {
            //alreadyRead = true;
            num++;
            page = num;
        }

        StartCoroutine(Teleport());

    }
    
    private IEnumerator Teleport()
    {
        MainManager.instance.AddTrigger("wait;1");
        MainManager.instance.AddTrigger("moveplayer;" + to);
        MainManager.instance.AddTrigger("wait;2");
        MainManager.instance.AddTrigger("waitesc");
        MainManager.instance.PlayEffect(teleport);
        yield return new WaitForSeconds(1.0f);
        if (clock != null) clock.volume = 0;
        yield return new WaitForSeconds(2.0f);
        DisplayBook.instance.DisplayPage(page);
        RenderSettings.fogDensity = 1.0f;
        RenderSettings.ambientIntensity = 0.6f;
        yield return new WaitUntil(() => MainManager.instance.gameState == 1);

        MainManager.instance.AddTrigger("wait;1");
        MainManager.instance.AddTrigger("moveplayer;" + back);
        MainManager.instance.AddTrigger("wait;2");
        MainManager.instance.PlayEffect(teleport);
        yield return new WaitForSeconds(1.0f);
        if (clock != null) clock.volume = clockV;
        yield return new WaitForSeconds(2.0f);
        RenderSettings.fogDensity = 0.15f;
        RenderSettings.ambientIntensity = 0.3f;
    }
}
