using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class CrowBarTrigger : MonoBehaviour
{
    [SerializeField] private AudioClip tense;
    [SerializeField] private AudioClip sewer;
    [SerializeField] private AudioClip seal;
    [SerializeField] private AudioClip hit;
    [SerializeField] private AudioClip die;
    [SerializeField] private GameObject sealedDoor;
    [SerializeField] private GameObject player;
    [SerializeField] private GameObject playerHead;

    [SerializeField] private Image screen;
    [SerializeField] private TextMeshPro txt;

    private bool isSealed = false;
    private float t = 0;

    private void Update()
    {
        if (!isSealed || MainManager.instance.gameState != 1) return;
        t += Time.deltaTime;
        screen.color = Color.Lerp(Color.clear, Color.red / 1.5f, t / 7.0f);
        txt.color = Color.Lerp(Color.clear, Color.red, t / 7.0f);
        if(t > 7.0f)
        {
            isSealed = false;
            transform.parent = null;
            gameObject.AddComponent<Rigidbody>();
            player.SetActive(false);
            playerHead.SetActive(true);
            MainManager.instance.PlayEffect(die);
            MainManager.instance.AddTrigger("wait;3");
            MainManager.instance.AddTrigger("loadscene;SewerScene;3");
        }
    }

    public void SealIn()
    {
        MainManager.instance.PlayEffect(seal);
        isSealed = true;
    }

    public void HitDoor()
    {
        MainManager.instance.PlayEffect(hit);
        MainManager.instance.PlayMusic(tense);
        sealedDoor.SetActive(true);
    }
}
