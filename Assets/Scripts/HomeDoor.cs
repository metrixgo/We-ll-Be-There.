using System.Collections;
using UnityEngine;

public class HomeDoor : MonoBehaviour
{
    [SerializeField] private bool opened = false;
    private bool isTurning = false;
    private AudioSource ad;

    private void Start()
    {
        ad = GetComponent<AudioSource>();
    }

    public void InteractDoor()
    {
        if (!isTurning)
        {
            isTurning = true;
            StartCoroutine(Turn());
        }
    }

    public bool IsOpened()
    {
        return opened;
    }

    private IEnumerator Turn()
    {
        ad.Play();
        float rot = 0;
        Vector3 angles = transform.eulerAngles;
        float goal = angles.y;
        if (!opened)
        {
            goal += 95.0f;
            while(rot < 95.0f)
            {
                rot += 150.0f * Time.deltaTime;
                transform.Rotate(0, 150.0f * Time.deltaTime, 0, Space.World);
                yield return null;
            }
        }
        else
        {
            goal -= 95.0f;
            while (rot < 95.0f)
            {
                rot += 150.0f * Time.deltaTime;
                transform.Rotate(0, -150.0f * Time.deltaTime, 0, Space.World);
                yield return null;
            }
        }
        transform.rotation = Quaternion.Euler(angles.x, goal, angles.z);
        opened = !opened;
        isTurning = false;
    }
}
