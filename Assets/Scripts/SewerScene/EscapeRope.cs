using System.Collections;
using UnityEngine;

public class EscapeRope : MonoBehaviour
{
    [SerializeField] private SewerKiller killer;
    [SerializeField] private GameObject player;
    [SerializeField] private GameObject playerCam;
    [SerializeField] private SewerFlashlight playerfl;
    [SerializeField] private GameObject player2;
    [SerializeField] private SewerFlashlight player2fl;

    private bool climbedOn = false;
    private bool isLeft = true;
    private float progress = 0;
    private float velocity = 0;
    private float rotX;
    private float rotY;
    private float sensitivity;
    private AudioSource ad;
    private Vector3 initPos = new Vector3(-19.2359619f, 0.265543938f, -2.59992695f);
    private Quaternion initRot = Quaternion.Euler(0, 90.0f, 0);

    private void Start()
    {
        ad = player2.GetComponent<AudioSource>();
        sensitivity = PlayerPrefs.GetFloat("Sensitivity", 10.0f);
        rotX = initRot.eulerAngles.x;
        rotY = initRot.eulerAngles.y;
    }

    private void Update()
    {
        if (!climbedOn || MainManager.instance.gameState != 1) return;

        velocity -= 0.5f * Time.deltaTime;
        if (velocity <= 0) velocity = 0;
        sensitivity = PlayerPrefs.GetFloat("Sensitivity", 10.0f);

        float key = Input.GetAxisRaw("Horizontal");
        if (key < 0 && isLeft || key > 0 && !isLeft)
        {
            isLeft = !isLeft;
            velocity = Mathf.Min(velocity + 0.07f, 0.6f);
            if (!ad.isPlaying) ad.Play();
        }

        if (velocity < 0.15f) ad.Stop();

        progress += velocity * Time.deltaTime;

        player2.transform.localPosition = new Vector3(initPos.x + Mathf.Sin(progress * 7.5f) * 0.05f, initPos.y + progress, initPos.z + Mathf.Sin(progress * 5.2f) * 0.08f);
        rotX -= Input.GetAxis("Mouse Y") * sensitivity;
        rotX = Mathf.Clamp(rotX, -90.0f, 90.0f);
        rotY += Input.GetAxis("Mouse X") * sensitivity;
        player2.transform.rotation = Quaternion.Euler(rotX, rotY, 0);
    }

    public void ClimbOn()
    {
        StartCoroutine(StartClimbing());
    }

    private IEnumerator StartClimbing()
    {
        killer.SecondStage();
        player2.transform.position = playerCam.transform.position;
        player2.transform.rotation = playerCam.transform.rotation;
        player2.SetActive(true);
        player2fl.Open(playerfl.IsOpened());
        player.SetActive(false);
        Vector3 pos = player2.transform.localPosition;
        Quaternion rot = player2.transform.rotation;

        float l = 1.0f;
        MainManager.instance.AddTrigger("wait;" + l);
        float t = 0;
        while (t < l)
        {
            player2.transform.localPosition = Vector3.Lerp(pos, initPos, t / l);
            player2.transform.rotation = Quaternion.Slerp(rot, initRot, t / l);
            t += Time.deltaTime;
            yield return null;
        }
        player2.transform.localPosition = initPos;
        player2.transform.rotation = initRot;
        climbedOn = true;
        MainManager.instance.SetPrompt("Press [A] and [D] to climb", true);
    }
}