using System.Collections;
using UnityEngine;

public class EscapeRope : MonoBehaviour
{
    [SerializeField] private SewerKiller killer;
    [SerializeField] private GameObject player;
    [SerializeField] private SewerFlashlight playerfl;
    [SerializeField] private GameObject player2;
    [SerializeField] private SewerFlashlight player2fl;

    private Vector3 initPos = new Vector3(-0.5f, -3.4f, 0);
    private Quaternion initRot = Quaternion.Euler(0, 90.0f, 0);

    private void Update()
    {
        
    }

    public void ClimbOn()
    {
        StartCoroutine(StartClimbing());
    }

    private IEnumerator StartClimbing()
    {
        player2fl.Open(playerfl.IsOpened());
        killer.SecondStage();
        player2.transform.position = player.transform.position;
        player2.transform.rotation = player.transform.rotation;
        player.SetActive(false);
        player2.SetActive(true);
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
        MainManager.instance.SetPrompt("We made it", true);
    }
}