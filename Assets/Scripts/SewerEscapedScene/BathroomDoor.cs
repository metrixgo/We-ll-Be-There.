using System.Collections;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class BathroomDoor : MonoBehaviour
{
    [SerializeField] private AudioClip lockedDoor;
    [SerializeField] private AudioClip open;
    [SerializeField] private AudioClip crowbar;
    [SerializeField] private AudioClip creakOpen;
    [SerializeField] private AudioClip night;
    [SerializeField] private ParticleControl water;
    [SerializeField] private GameObject bathroom;
    [SerializeField] private GameObject player;
    [SerializeField] private Image screen;
    [SerializeField] private Transform playerCam;
    [SerializeField] private Transform playerBar;
    [SerializeField] private Transform openCam;
    [SerializeField] private Transform openBar;

    private AudioSource ad;
    private bool interacted = false;
    private bool keyInteracted = false;
    private bool locked = true;
    private Vector3 openBarPos;
    private Quaternion openBarRot;

    private void Start()
    {
        ad = GetComponent<AudioSource>();
        openBarPos = openBar.position;
        openBarRot = openBar.rotation;
        openBar.localPosition = new Vector3(0, -0.15f, -0.05f);
        openBar.localRotation = Quaternion.Euler(5.0f, 0, 0);
    }

    public void TryOpen()
    {
        if (MainManager.instance.HasItem("Crowbar"))
        {
            if (locked)
            {
                locked = false;
                StartCoroutine(CrowbarOpen());
            }
        }
        else if (MainManager.instance.HasItem("Key"))
        {
            if (!keyInteracted)
            {
                keyInteracted = true;
                MainManager.instance.AddTrigger("wait;" + lockedDoor.length);
                MainManager.instance.AddTrigger("dialogue;You;Fuck... The key won't fit... It's over...");
            }
            if (!ad.isPlaying)
            {
                ad.clip = lockedDoor;
                ad.Play();
            }
        }
        else
        {
            if (!interacted)
            {
                interacted = true;
                MainManager.instance.AddTrigger("wait;" + lockedDoor.length);
                MainManager.instance.AddTrigger("dialogue;You;It's locked?! I need to find the key NOW.");
            }
            if (!ad.isPlaying)
            {
                ad.clip = lockedDoor;
                ad.Play();
            }
        }
    }

    private IEnumerator CrowbarOpen()
    {
        MainManager.instance.AddTrigger("wait;20.5");
        player.SetActive(false);
        openCam.gameObject.SetActive(true);

        Vector3 startPos = playerCam.position;
        Vector3 endPos = openCam.position;
        Quaternion startRot = playerCam.rotation;
        Quaternion endRot = openCam.rotation;
        Color scrCol = screen.color;
        float t = 0;
        while (t < 1.0f)
        {
            openCam.position = Vector3.Lerp(startPos, endPos, t);
            openCam.rotation = Quaternion.Slerp(startRot, endRot, t);
            screen.color = Color.Lerp(scrCol, Color.clear, t);
            RenderSettings.fogDensity = Mathf.Lerp(0.2f, 0.4f, t);
            t += Time.deltaTime;
            yield return null;
        }
        screen.color = Color.clear;
        yield return new WaitForSeconds(0.2f);

        startPos = openBar.position;
        endPos = openBarPos;
        startRot = openBar.rotation;
        endRot = openBarRot;
        t = 0;
        while (t < 0.5f)
        {
            openBar.position = Vector3.Lerp(startPos, endPos, t / 0.5f);
            openBar.rotation = Quaternion.Slerp(startRot, endRot, t / 0.5f);
            t += Time.deltaTime;
            yield return null;
        }
        yield return new WaitForSeconds(0.1f);

        t = 0;
        startRot = openBar.rotation;
        endRot = Quaternion.Euler(openBar.eulerAngles + Vector3.right * 20.0f);
        while (t < 0.2f)
        {
            openBar.rotation = Quaternion.Slerp(startRot, endRot, t / 0.2f);
            t += Time.deltaTime;
            yield return null;
        }
        yield return new WaitForSeconds(0.1f);

        ad.clip = crowbar;
        ad.Play();
        t = 0;
        startPos = openBar.position;
        endPos = openBar.position - Vector3.up * 0.4f;
        while (t < 5.0f)
        {
            openBar.position = Vector3.Lerp(startPos, endPos, t / 5.0f);
            t += Time.deltaTime;
            yield return null;
        }
        yield return new WaitForSeconds(0.5f);

        ad.clip = open;
        ad.Play();
        t = 0;
        startPos = openBar.position;
        endPos = openBar.position - Vector3.up * 0.1f;
        startRot = transform.rotation;
        endRot = Quaternion.Euler(transform.eulerAngles + Vector3.up * 5.0f);
        while (t < 0.2f)
        {
            openBar.position = Vector3.Lerp(startPos, endPos, t / 0.2f);
            transform.rotation = Quaternion.Slerp(startRot, endRot, t / 0.2f);
            t += Time.deltaTime;
            yield return null;
        }
        yield return new WaitForSeconds(0.1f);

        t = 0;
        startPos = openBar.position;
        endPos = openBar.position + Vector3.up * 0.5f;
        startRot = openBar.rotation;
        endRot = Quaternion.Euler(openBar.eulerAngles - Vector3.right * 20.0f);
        while (t < 0.5f)
        {
            openBar.position = Vector3.Lerp(startPos, endPos, t / 0.5f);
            openBar.rotation = Quaternion.Slerp(startRot, endRot, t / 0.5f);
            t += Time.deltaTime;
            yield return null;
        }
        yield return new WaitForSeconds(0.1f);

        t = 0;
        startRot = openCam.rotation;
        endRot = Quaternion.Euler(openCam.eulerAngles + Vector3.up * 30.0f);
        while (t < 0.1f)
        {
            openCam.rotation = Quaternion.Slerp(startRot, endRot, t / 0.1f);
            t += Time.deltaTime;
            yield return null;
        }
        openCam.rotation = endRot;
        openBar.AddComponent<Rigidbody>();
        openBar.GetComponent<Rigidbody>().AddForce(new Vector3(-0.8f, 0, -1.0f), ForceMode.Impulse);
        openBar.SetParent(null);
        openBar.GetComponent<AudioSource>().Play();
        yield return new WaitForSeconds(0.1f);

        openCam.GetComponent<Animator>().enabled = true;
        yield return new WaitForSeconds(1.0f);

        ad.clip = creakOpen;
        ad.Play();
        MainManager.instance.StopMusic();
        if (water.IsOpened()) water.ChangeParticles();
        t = 0;
        startRot = transform.rotation;
        endRot = Quaternion.Euler(transform.eulerAngles + Vector3.up * 40.0f);
        while (t < 2.0f)
        {
            transform.rotation = Quaternion.Slerp(startRot, endRot, t / 2.0f);
            t += Time.deltaTime;
            yield return null;
        }
        yield return new WaitForSeconds(8.5f);

        PlayerController pc = player.GetComponent<PlayerController>();
        Destroy(playerBar.gameObject);
        Destroy(openBar.gameObject);
        pc.SetPosition(openCam.position - Vector3.up * 0.75f);
        pc.SetRotation(openCam.eulerAngles.y, openCam.eulerAngles.x);
        Destroy(bathroom);
        player.SetActive(true);
    }
}