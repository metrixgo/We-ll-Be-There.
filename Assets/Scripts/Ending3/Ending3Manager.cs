using System.Collections;
using Unity.VisualScripting;
using UnityEngine;

public class Ending3Manager : MonoBehaviour
{
    [SerializeField] private GameObject player;
    [SerializeField] private Transform playerCam;
    [SerializeField] private Transform endPlayer;
    [SerializeField] private Transform door;
    [SerializeField] private Transform door2;
    [SerializeField] private Transform water;

    private ParticleSystem ps;
    private AudioSource ad;

    public void OpenDoor()
    {
        ps = water.GetComponent<ParticleSystem>();
        ad = door.GetComponent<AudioSource>();
        StartCoroutine(EndIt());
    }

    private IEnumerator EndIt()
    {
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
        yield return new WaitForSeconds(1.0f);
        
        t = 0;
        ps.Play();
        ad.Play();
        float rot = 0;
        Vector3 angles = door.eulerAngles;
        float goal = angles.y + 95.0f;
        while (rot < 95.0f)
        {
            rot += 150.0f * Time.deltaTime;
            door.Rotate(0, 150.0f * Time.deltaTime, 0, Space.World);
            yield return null;
        }
        door.rotation = Quaternion.Euler(angles.x, goal, angles.z);
    }
}
