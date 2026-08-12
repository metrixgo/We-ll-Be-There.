using System.Collections;
using UnityEngine;

public class BathroomDrawer : MonoBehaviour
{
    private bool opened = false;
    private bool isMoving = false;
    private AudioSource ad;

    private void Start()
    {
        ad = GetComponent<AudioSource>();
    }

    public void InteractDrawer()
    {
        if (!isMoving)
        {
            isMoving = true;
            StartCoroutine(Move());
        }
    }

    private IEnumerator Move()
    {
        ad.Play();
        float dis = 0;
        Vector3 pos = transform.position;
        float goal = transform.position.z;
        if (!opened)
        {
            goal -= 0.3f;
            while (dis < 0.3f)
            {
                dis += 0.8f * Time.deltaTime;
                transform.Translate(0, 0, -0.8f * Time.deltaTime, Space.World);
                yield return null;
            }
        }
        else
        {
            goal += 0.3f;
            while (dis < 0.3f)
            {
                dis += 0.8f * Time.deltaTime;
                transform.Translate(0, 0, 0.8f * Time.deltaTime, Space.World);
                yield return null;
            }
        }
        transform.position = new Vector3(pos.x, pos.y, goal);
        opened = !opened;
        isMoving = false;
    }
}
