using KinoGlitch;
using System.Collections;
using UnityEditor.Animations;
using UnityEngine;
using UnityEngine.UI;

public class Ending3Manager : MonoBehaviour
{
    [SerializeField] private GameObject player;
    [SerializeField] private Transform playerCam;
    [SerializeField] private Transform endPlayer;
    [SerializeField] private Transform door;
    [SerializeField] private Transform door2;
    [SerializeField] private Transform water;
    [SerializeField] private GameObject lights;
    [SerializeField] private AudioClip tense;
    [SerializeField] private AudioClip jumpscare;
    [SerializeField] private Image screen;
    [SerializeField] private RawImage ri;
    [SerializeField] private Material mat;

    private ParticleSystem ps;
    private AudioSource ad;
    private DigitalGlitchController dgc;
    private Animator anim;
    private PlayerController pc;

    public void OpenDoor()
    {
        ps = water.GetComponent<ParticleSystem>();
        ad = door.GetComponent<AudioSource>();
        dgc = endPlayer.GetComponent<DigitalGlitchController>();
        anim = endPlayer.GetComponent<Animator>();
        pc = player.GetComponent<PlayerController>();
        StartCoroutine(EndIt());
    }

    private IEnumerator EndIt()
    {
        MainManager.instance.AddTrigger("wait;3");

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
        ad.Play();
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
        ad.Play();
        yield return new WaitForSeconds(0.7f);
        MainManager.instance.PlayEffect(jumpscare);
        door.Rotate(0, -95f, 0, Space.World);
        Destroy(door2.gameObject);
        lights.SetActive(true);
        pc.SetPosition(endPlayer.position - 0.75f * Vector3.up);
        pc.SetRotation(endPlayer.eulerAngles.y, endPlayer.eulerAngles.x);
        yield return new WaitForSeconds(1.0f);
        player.SetActive(true);
        Destroy(endPlayer);
    }

    private IEnumerator ChangeScreen()
    {
        float t = 0;
        while (t < 3.0f)
        {
            screen.color = Color.Lerp(Color.red * 0.6f, Color.red * 0.15f, t / 3.0f);
            dgc.SetIntensity(Mathf.Lerp(0.2f, 0.01f, t / 3.0f));
            t += Time.deltaTime;
            yield return null;
        }
        dgc.SetIntensity(0.01f);
        t = 0;
        while (true)
        {
            screen.color = Color.red * (0.15f + Mathf.Sin(t) * 0.05f);
            t += Time.deltaTime;
            yield return null;
        }
    }
}
