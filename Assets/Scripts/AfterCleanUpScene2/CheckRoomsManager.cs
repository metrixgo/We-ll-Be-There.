using System.Collections;
using UnityEngine;

public class CheckRoomsManager : MonoBehaviour
{
    public static CheckRoomsManager count;

    [SerializeField] private AudioSource ad;
    [SerializeField] private AudioClip tense;
    [SerializeField] private GameObject sofa;
    [SerializeField] private GameObject door;
    [SerializeField] private GameObject oldDoor;

    private int cnt = 0;
    private bool touched = false;
    private string header;
    private string enclose;

    private void Awake()
    {
        count = this;
    }

    private void Start()
    {
        if (PlayerPrefs.GetString("Language", "English") == "English")
        {
            header = "Check Rooms (";
            enclose = ")";
        }
        else
        {
            header = "¼ì²é·¿¼ä £¨";
            enclose = "£©";
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (!touched && MainManager.instance.gameState == 1)
        {
            touched = true;
            MainManager.instance.AddTrigger("dialogue;You;Where did they go? I don't see them leaving at all.");
            MainManager.instance.AddTrigger("dialogue;You;That's so weird. I need to go check all the rooms on the second floor.");
            MainManager.instance.AddTrigger("task;" + header + "0/6" + enclose);
        }
    }

    public void CheckOne()
    {
        cnt++;
        MainManager.instance.AddTrigger("cleartasks");
        MainManager.instance.AddTrigger("task;" + header + cnt + "/6" + enclose);

        if (cnt == 2)
        {
            ad.Play();
        }
        else if (cnt == 4)
        {
            door.SetActive(true);
            Destroy(oldDoor);
        }
        else if (cnt >= 6)
        {
            StartCoroutine(Realize());
        }
    }

    private IEnumerator Realize()
    {
        MainManager.instance.PlayEffect(tense);
        yield return new WaitForSeconds(6.0f);
        MainManager.instance.AddTrigger("dialogue;You;I can't find them...");
        MainManager.instance.AddTrigger("dialogue;You;Maybe it's just me hallucinating...");
        MainManager.instance.AddTrigger("dialogue;You;Anyways, I should just go and relax on the couch. It has been such a tiring and stressful day.");
        MainManager.instance.AddTrigger("cleartasks");
        MainManager.instance.AddTrigger("task;Go relax on the sofa");
        sofa.tag = "Interactable";
    }
}
