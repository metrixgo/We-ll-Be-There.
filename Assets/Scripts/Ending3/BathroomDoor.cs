using System.Collections;
using UnityEngine;

public class BathroomDoor : MonoBehaviour
{
    [SerializeField] private AudioClip lockedDoor;
    [SerializeField] private AudioClip open;
    [SerializeField] private AudioClip crowbar;
    [SerializeField] private Transform player;
    [SerializeField] private Transform playerCam;
    [SerializeField] private Transform playerBar;
    [SerializeField] private Transform openCam;
    [SerializeField] private Transform openBar;

    private AudioSource ad;
    private bool interacted = false;
    private bool keyInteracted = false;
    private bool locked = true;

    private void Start()
    {
        ad = GetComponent<AudioSource>();
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
        player.gameObject.SetActive(false);
        openCam.gameObject.SetActive(true);

        Vector3 startPos = playerCam.position;
        Vector3 endPos = openCam.position;
        Quaternion startRot = playerCam.rotation;
        Quaternion endRot = openCam.rotation;
        float t = 0;
        while (t < 1.0f)
        {
            openCam.position = Vector3.Lerp(startPos, endPos, t);
            openCam.rotation = Quaternion.Slerp(startRot, endRot, t);
            t += Time.deltaTime;
            yield return null;
        }
        yield return new WaitForSeconds(0.2f);

        startPos = playerBar.position;
        endPos = openBar.position;
        startRot = playerBar.rotation;
        endRot = openBar.rotation;
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
        while(t < 0.2f)
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
        while (t < 4.7f)
        {
            openBar.position = Vector3.Lerp(startPos, endPos, t / 4.7f);
            t += Time.deltaTime;
            yield return null;
        }
        yield return new WaitForSeconds(0.1f);

        startPos = openBar.position;
        endPos = openBar.position - Vector3.up * 0.1f;
        startRot = transform.rotation;
        endRot = Quaternion.Euler(transform.eulerAngles + Vector3.right * 20.0f);

        while (t < 0.2f)
        {
            openBar.position = Vector3.Lerp(startPos, endPos, t / 0.2f);
            t += Time.deltaTime;
            yield return null;
        }
    }
}