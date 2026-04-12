using System.Collections;
using UnityEngine;

public class Monster : MonoBehaviour
{
    [SerializeField] private AudioSource playerAd;
    [SerializeField] private AudioSource alarmClock;
    [SerializeField] private AudioClip hit;
    [SerializeField] private AudioClip scream;
    [SerializeField] private AudioClip breath;
    [SerializeField] private AudioClip morning;
    [SerializeField] private GameObject player;
    [SerializeField] private GameObject attackedPlayer;
    [SerializeField] private GameObject dream;
    [SerializeField] private GameObject dirLight;
    [SerializeField] private Material blueSky;

    private Animator anim;
    private PlayerController pc;

    private void Start()
    {
        anim = GetComponent<Animator>();
        pc = player.GetComponent<PlayerController>();
    }

    private void Update()
    {
        if (MainManager.instance.gameState != 1 || !anim.GetBool("Run") || anim.GetBool("Attack")) return ;

        transform.Translate(Vector3.left * 5.2f * Time.deltaTime, Space.World);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (!other.CompareTag("Player")) return ;

        StartCoroutine(Attack());
    }

    private IEnumerator Attack()
    {
        MainManager.instance.SetPrompt("");
        MainManager.instance.AddTrigger("wait;0.1");
        MainManager.instance.AddTrigger("changescreen;#FF0000FF;#FF0000FF;0.5");
        MainManager.instance.AddTrigger("changescreen;#FF0000FF;#000000FF;0.8");
        MainManager.instance.AddTrigger("wait;1");
        MainManager.instance.AddTrigger("changescreen;#000000FF;#00000000;2");
        MainManager.instance.AddTrigger("wait;15");

        MainManager.instance.PlayEffect(hit);
        float t = 0, v = PlayerPrefs.GetFloat("Music", 30.0f);
        while(t < 2.3f)
        {
            MainManager.instance.SetMusicVolume(Mathf.Lerp(v, 0, t / 2.3f));
            t += Time.deltaTime;
            yield return null;
        }
        MainManager.instance.StopMusic();
        MainManager.instance.SetMusicVolume(v);
        transform.position = new Vector3(-100.5f, 28.4f, 16.3f);
        anim.SetBool("Attack", true);
        yield return new WaitForSeconds(0.1f);
        player.SetActive(false);
        attackedPlayer.SetActive(true);
        MainManager.instance.PlayEffect(scream);
        yield return new WaitForSeconds(2.0f);
        
        t = 0;
        while(t < 2.0f)
        {
            transform.Translate(Vector3.down * 15.0f * Time.deltaTime);
            t += Time.deltaTime;
            yield return null;
        }
        playerAd.clip = breath;
        playerAd.Play();
        MainManager.instance.PlayMusic(morning);
        alarmClock.Play();
        dirLight.SetActive(true);
        RenderSettings.skybox = blueSky;
        RenderSettings.fogColor = Color.gray;
        RenderSettings.fogDensity = 0.01f;
        RenderSettings.ambientIntensity = 1.0f;
        attackedPlayer.transform.position = new Vector3(-50.35f, 4.75f, -68.7f);
        yield return new WaitForSeconds(13.0f);
        player.SetActive(true);
        pc.SetPosition(new Vector3(-51.15f, 4.742f, -68.4f));
        pc.SetRotation(-180.0f, 15.0f);
        pc.CanRun(false);
        player.GetComponent<CharacterController>().height = 1.5f;
        Destroy(attackedPlayer);
        Destroy(dream);
    }
}
