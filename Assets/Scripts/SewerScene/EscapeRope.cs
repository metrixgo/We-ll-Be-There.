using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;

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
    private AudioSource ad;
    private Vector3 initPos = new Vector3(-0.5f, -3.025f, 0);
    private Quaternion initRot = Quaternion.Euler(0, 90.0f, 0);

    private void Start()
    {
        ad = player2.GetComponent<AudioSource>();
    }

    private void Update()
    {
        if (!climbedOn || MainManager.instance.gameState != 1) return;

        float key = Input.GetAxisRaw("Horizontal");
        if (key < 0 && isLeft || key > 0 && !isLeft)
        {
            isLeft = !isLeft;
            progress += 1.0f;
            if (!ad.isPlaying) ad.Play();
        }
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

        MainManager.instance.AddTrigger("wait;0.7");
        float t = 0;
        while(t < 0.7f)
        {
            player2.transform.localPosition = Vector3.Lerp(pos, initPos, t / 0.7f);
            player2.transform.rotation = Quaternion.Slerp(rot, initRot, t / 0.7f);
            t += Time.deltaTime;
            yield return null;
        }
        player2.transform.localPosition = initPos;
        player2.transform.rotation = initRot;
        climbedOn = true;
        MainManager.instance.SetPrompt("Press [A] and [D] to climb", true);
    }
}