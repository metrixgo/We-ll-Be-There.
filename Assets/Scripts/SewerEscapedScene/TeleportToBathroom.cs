using KinoGlitch;
using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class TeleportToBathroom : MonoBehaviour
{
    [SerializeField] private GameObject player;
    [SerializeField] private Transform playerCam;
    [SerializeField] private Transform endPlayer;
    [SerializeField] private Transform door;
    [SerializeField] private Transform door2;
    [SerializeField] private GameObject door3;
    [SerializeField] private AlarmClock radio;
    [SerializeField] private Transform water;
    [SerializeField] private GameObject monster;
    [SerializeField] private AudioClip tense;
    [SerializeField] private AudioClip jumpscare;
    [SerializeField] private Image screen;
    [SerializeField] private RawImage ri;
    [SerializeField] private Material mat;
    [SerializeField] private Material blackSky;

    private ParticleSystem ps;
    private AudioSource doorAd;
    private AudioSource playerAd;
    private DigitalGlitchController dgc;
    private Animator anim;
    private PlayerController pc;

    public void OpenDoor()
    {
        ps = water.GetComponent<ParticleSystem>();
        doorAd = door.GetComponent<AudioSource>();
        playerAd = endPlayer.GetComponent<AudioSource>();
        dgc = endPlayer.GetComponent<DigitalGlitchController>();
        anim = endPlayer.GetComponent<Animator>();
        pc = player.GetComponent<PlayerController>();
        StartCoroutine(EndIt());
    }

    private IEnumerator EndIt()
    {
        MainManager.instance.AddTrigger("wait;20");
        MainManager.instance.AddTrigger("dialogue;You;What... Is... Happening...");
        MainManager.instance.AddTrigger("dialogue;You;I need to get out... I need to get out of here...");
        Vector3 startPos = playerCam.position;
        Quaternion startRot = playerCam.rotation;
        Vector3 endPos = endPlayer.position;
        Quaternion endRot = endPlayer.rotation;

        float t = 0;
        endPlayer.position = playerCam.position;
        endPlayer.rotation = playerCam.rotation;
        endPlayer.gameObject.SetActive(true);
        player.SetActive(false);

        while (t < 1.0f)
        {
            endPlayer.position = Vector3.Lerp(startPos, endPos, t);
            endPlayer.rotation = Quaternion.Slerp(startRot, endRot, t);
            t += Time.deltaTime;
            yield return null;
        }
        endPlayer.position += new Vector3(59.15624712f, -4.313906077f, -228.965973f);
        water.position += new Vector3(59.15624712f, -4.313906077f, -228.965973f);
        door2.position += new Vector3(59.15624712f, -4.313906077f, -228.965973f);
        yield return new WaitForSeconds(0.5f);

        ps.Play();
        doorAd.Play();
        float rot = 0;
        Vector3 angles = door.eulerAngles;
        float goal = angles.y + 95.0f;
        while (rot < 45.0f)
        {
            rot += 150.0f * Time.deltaTime;
            door.Rotate(0, 150.0f * Time.deltaTime, 0, Space.World);
            yield return null;
        }
        MainManager.instance.PlayMusic(tense);
        MainManager.instance.PlayEffect(jumpscare);
        ri.material = mat;
        StartCoroutine(ChangeScreen());
        while (rot < 95.0f)
        {
            rot += 150.0f * Time.deltaTime;
            door.Rotate(0, 150.0f * Time.deltaTime, 0, Space.World);
            yield return null;
        }
        door.rotation = Quaternion.Euler(angles.x, goal, angles.z);
        yield return new WaitForSeconds(1.0f);
        anim.enabled = true;
        yield return new WaitForSeconds(11.0f);
        doorAd.Play();
        door.Rotate(0, -95f, 0, Space.World);
        yield return new WaitForSeconds(0.9f);
        monster.SetActive(true);
        MainManager.instance.PlayEffect(jumpscare);
        playerAd.Play();
        screen.color = Color.red * 0.4f;
        dgc.SetIntensity(0.2f);
        door2.position -= new Vector3(59.15624712f, -4.313906077f, -228.965973f);
        RenderSettings.skybox = blackSky;
        yield return new WaitForSeconds(0.5f);
        screen.color = Color.red * 0.2f;
        dgc.SetIntensity(0.01f);
        Destroy(monster);
        door3.SetActive(true);
        radio.Interact();
        pc.SetPosition(endPlayer.position - 0.75f * Vector3.up);
        pc.SetRotation(endPlayer.eulerAngles.y, endPlayer.eulerAngles.x);
        pc.ResetCamT();
        yield return new WaitUntil(() => MainManager.instance.gameState == 1);
        player.SetActive(true);
        Destroy(endPlayer.gameObject);
        
        Destroy(gameObject);
    }

    private IEnumerator ChangeScreen()
    {
        float t = 0;
        while (t < 3.0f)
        {
            screen.color = Color.Lerp(Color.red * 0.6f, Color.red * 0.2f, t / 3.0f);
            dgc.SetIntensity(Mathf.Lerp(0.2f, 0.01f, t / 3.0f));
            t += Time.deltaTime;
            yield return null;
        }
        dgc.SetIntensity(0.01f);
        screen.color = Color.red * 0.2f;
        t = 0;
    }
}
