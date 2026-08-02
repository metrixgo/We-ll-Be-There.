using System.Collections;
using TMPro;
using TreeEditor;
using UnityEngine;
using UnityEngine.UI;

public class CrowBarTrigger : MonoBehaviour
{
    [SerializeField] private AudioClip seal;
    [SerializeField] private AudioClip hit;
    [SerializeField] private AudioClip die;
    [SerializeField] private AudioClip finishHit;
    [SerializeField] private AudioClip putAway;
    [SerializeField] private GameObject sealedDoor;
    [SerializeField] private GameObject scatteredRocks;
    [SerializeField] private GameObject player;
    [SerializeField] private GameObject playerHead;
    [SerializeField] private Image screen;
    [SerializeField] private TextMeshPro txt;

    private int state = 0;
    private int cnt = 0;
    private float t = 0;
    private float hitT = 0;
    private float originalV = 0;
    private bool temp = false;
    private AudioSource ad;

    private void Start()
    {
        ad = GetComponent<AudioSource>();
        originalV = ad.volume;
    }

    private void Update()
    {
        if (state == 1 && MainManager.instance.gameState == 1)
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
                state = 2;
                transform.parent = null;
                gameObject.AddComponent<Rigidbody>();
                if (player.activeSelf)
                {
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
        }
        else if (state == 2)
        {
            t += Time.deltaTime;
            ad.volume = Mathf.Lerp(originalV, 0, t / 6.0f);
        }
        else if (state == 3)
        {
            t -= Time.deltaTime;
            if (t <= 0)
            {
                screen.color = Color.clear;
                Destroy(txt.gameObject);
                ad.Stop();
                ad.volume = originalV;
                state = 0;
            }
            else
            {
                screen.color = Color.Lerp(Color.red / 2.0f, Color.clear, 1 - t / 10.0f);
                txt.color = Color.Lerp(Color.red, Color.clear, 1 - t / 10.0f);
                ad.volume = Mathf.Lerp(originalV, 0, 1 - t / hitT);
            }
                
        }
        else if (state == 4)
        {
            t += Time.deltaTime;
            if(!temp && t > 1.0f)
            {
                temp = true;
                ad.clip = putAway;
                ad.Play();
            }
            
            if (t > 2.0f) Destroy(gameObject);
            
            if (t > 1.0f)
            {
                transform.Translate(0, -0.21f * Time.deltaTime, 0);
            }
        }
    }

    public void SealIn()
    {
        MainManager.instance.PlayEffect(seal);
        sealedDoor.SetActive(true);
        state = 1;
        ad.Play();
    }

    public void HitDoor()
    {
        if (hitT > 0) return;
        cnt++;
        if(cnt >= 10)
        {
            MainManager.instance.PlayEffect(finishHit);
            StartCoroutine(MoveCrowBar());
            Destroy(sealedDoor);
            scatteredRocks.SetActive(true);
            state = 3;
            hitT = t;
        }
        else
        {
            MainManager.instance.PlayEffect(hit);
            hitT = hit.length;
            StartCoroutine(MoveCrowBar());
        }
    }

    public void PutAway()
    {
        state = 4;
        t = 0;
        hitT = 0;
    }

    private IEnumerator MoveCrowBar()
    {
        float t = 0;
        Vector3 rot = transform.localEulerAngles;
        while (t < hit.length / 7.0f)
        {
            rot.z = Mathf.Lerp(0, 15.0f, t / (hit.length / 7.0f));
            transform.localEulerAngles = rot;
            t += Time.deltaTime;
            yield return null;
        }
        t = 0;
        while (t < 6.0f * hit.length / 7.0f)
        {
            rot.z = Mathf.Lerp(15.0f, 0, t / (6.0f * hit.length / 7.0f));
            transform.localEulerAngles = rot;
            t += Time.deltaTime;
            yield return null;
        }

        rot.z = 0;
        transform.localEulerAngles = rot;
    }
}
