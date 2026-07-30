using System.Collections;
using UnityEngine;

public class SewerBooks : MonoBehaviour
{
    [SerializeField] private AudioClip tense;
    [SerializeField] private AudioClip teleport;
    [SerializeField] private SewerClock clock;
    [SerializeField] private Transform corBook;

    public static int num { get; private set; } = 0;

    private bool alreadyRead = false;
    private string to;
    private string back;
    private int page;

    private void Start()
    {
        num = 0;
        float x = corBook.position.x - transform.position.x;
        float y = corBook.position.y - transform.position.y;
        float z = corBook.position.z - transform.position.z;
        to = x + ";" + y + ";" + z;
        back = (-x) + ";" + (-y) + ";" + (-z);
    }

    public void Read()
    {
        if (!alreadyRead)
        {
            alreadyRead = true;
            num++;
            page = num;
            if (num == 9) SewerMusicManager.instance.ChangeTo(tense, 3.0f, 50.0f);
        }

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
        if (clock != null && clock.gameObject.activeSelf) clock.Mute(true);
        yield return new WaitForSeconds(1.5f);
        DisplayBook.instance.DisplayPage(page);
        yield return new WaitUntil(() => MainManager.instance.gameState == 1);

        MainManager.instance.AddTrigger("wait;1");
        MainManager.instance.AddTrigger("moveplayer;" + back);
        MainManager.instance.AddTrigger("wait;1.5");
        MainManager.instance.PlayEffect(teleport);
        yield return new WaitForSeconds(1.0f);
        if (clock != null && clock.gameObject.activeSelf) clock.Mute(false);
        RenderSettings.fogDensity = 0.15f;
        RenderSettings.ambientIntensity = 0.3f;
        yield return new WaitForSeconds(1.5f);
    }
}
