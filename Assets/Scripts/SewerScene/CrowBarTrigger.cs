using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class CrowBarTrigger : MonoBehaviour
{
    [SerializeField] private AudioClip seal;
    [SerializeField] private AudioClip hit;
    [SerializeField] private AudioClip die;
    [SerializeField] private AudioClip finishHit;
    [SerializeField] private GameObject sealedDoor;
    [SerializeField] private GameObject player;
    [SerializeField] private GameObject playerHead;
    [SerializeField] private Image screen;
    [SerializeField] private TextMeshPro txt;

    private int sealState = 0;
    private int cnt = 0;
    private float t = 0;
    private float hitT = 0;
    private float originalV = 0;
    private AudioSource ad;

    private void Start()
    {
        ad = GetComponent<AudioSource>();
        originalV = ad.volume;
    }

    private void Update()
    {
        if (sealState == 1 && MainManager.instance.gameState == 1)
        {
            t += Time.deltaTime;
            if (hitT != 0)
            {
                hitT -= Time.deltaTime;
                if (hitT <= 0) hitT = 0;
            }
            screen.color = Color.Lerp(Color.clear, Color.red / 2.0f, t / 10.0f);
            txt.color = Color.Lerp(Color.clear, Color.red, t / 10.0f);
            if (t > 10.0f)
            {
                t = 0;
                sealState = 2;
                transform.parent = null;
                gameObject.AddComponent<Rigidbody>();
                playerHead.SetActive(true);
                playerHead.transform.parent = null;
                playerHead.GetComponent<Rigidbody>().AddForce(0, 1.0f, 0, ForceMode.Impulse);
                playerHead.GetComponent<Rigidbody>().AddTorque(Vector3.up / 3.0f, ForceMode.Impulse);
                player.SetActive(false);
                MainManager.instance.PlayEffect(die);
                MainManager.instance.AddTrigger("wait;3");
                MainManager.instance.AddTrigger("loadscene;SewerScene;3");
            }
        }
        else if (sealState == 2)
        {
            t += Time.deltaTime;
            ad.volume = Mathf.Lerp(originalV, 0, t / 6.0f);
        }
        else if (sealState == 3)
        {
            t -= Time.deltaTime;
            if (t <= 0)
            {
                screen.color = Color.clear;
                Destroy(txt.gameObject);
                ad.volume = 0;
                Destroy(this);
            }
            screen.color = Color.Lerp(Color.red / 2.0f, Color.clear, 1 - t / 10.0f);
            txt.color = Color.Lerp(Color.red, Color.clear, 1 - t / 10.0f);
            ad.volume = Mathf.Lerp(originalV, 0, 1 - t / hitT);
        }
    }

    public void SealIn()
    {
        MainManager.instance.PlayEffect(seal);
        sealedDoor.SetActive(true);
        sealState = 1;
        ad.Play();
    }

    public void HitDoor()
    {
        if (hitT > 0) return;
        cnt++;
        if(cnt >= 6)
        {
            MainManager.instance.PlayEffect(finishHit);
            Destroy(sealedDoor);
            sealState = 3;
            hitT = t;
        }
        else
        {
            MainManager.instance.PlayEffect(hit);
            hitT = hit.length + 0.5f;
        }
    }
}
