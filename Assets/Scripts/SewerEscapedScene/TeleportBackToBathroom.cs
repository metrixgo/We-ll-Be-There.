using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class TeleportBackToBathroom : MonoBehaviour
{
    [SerializeField] private Transform player;
    [SerializeField] private CorpseHeadChase corpseHead;
    [SerializeField] private HomeDoor door;
    [SerializeField] private RawImage ri;
    [SerializeField] private Material mat;
    [SerializeField] private AudioClip breathing;
    [SerializeField] private AudioClip night;

    private static bool touched = false;
    private AudioSource tinAd;

    private void Start()
    {
        tinAd = GetComponent<AudioSource>();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (MainManager.instance.gameState == 1 && !touched)
        {
            touched = true;
            StartCoroutine(EscapeIt());
        }
    }

    private IEnumerator EscapeIt()
    {
        tinAd.Play();
        corpseHead.EndChase();
        door.InteractDoor();
        ri.material = mat;
        MainManager.instance.PlayEffect(breathing);
        MainManager.instance.PlayMusic(night);
        RenderSettings.fogDensity = 0.1f;

        player.SetParent(transform);
        Vector3 relPos = player.localPosition;
        Quaternion relRot = player.localRotation;
        player.SetParent(door.transform);
        player.localPosition = relPos;
        player.localRotation = relRot;
        player.SetParent(null);

        MainManager.instance.AddTrigger("wait;4");
        float t = 0;
        while (t < 5.0f)
        {
            RenderSettings.fogDensity = Mathf.Lerp(0.4f, 0.1f, t / 5.0f);
            tinAd.volume = Mathf.Lerp(PlayerPrefs.GetFloat("Effects", 80.0f) / 200.0f, 0, t / 5.0f);
            t += Time.deltaTime;
            yield return null;
        }
        tinAd.Stop();
    }
}
