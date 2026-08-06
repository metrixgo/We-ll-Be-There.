using System.Collections;
using UnityEngine;

public class Ending3Manager : MonoBehaviour
{
    [SerializeField] private GameObject player;
    [SerializeField] private Transform playerCam;
    [SerializeField] private Transform endPlayer;

    public void OpenDoor()
    {
        StartCoroutine(EndIt());
    }

    private IEnumerator EndIt()
    {
        float t = 0;
        endPlayer.position = playerCam.position;
        endPlayer.rotation = playerCam.rotation;
        endPlayer.gameObject.SetActive(true);
        player.SetActive(false);

        Vector3 startPos = playerCam.position;
        Quaternion startRot = playerCam.rotation;
        Vector3 endPos = new Vector3(0, 0, 0);
        Quaternion endRot = Quaternion.Euler(0, 0, 0);

        while (t < 1.0f)
        {
            endPlayer.position = Vector3.Lerp(startPos, endPos, t);
            endPlayer.rotation = Quaternion.Slerp(startRot, endRot, t);
            t += Time.deltaTime;
            yield return null;
        }
    }
}
