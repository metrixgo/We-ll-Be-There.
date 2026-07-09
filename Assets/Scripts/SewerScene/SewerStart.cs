using System.Collections;
using UnityEngine;

public class SewerStart : MonoBehaviour
{
    [SerializeField] private GameObject player;

    private void Start()
    {
        StartCoroutine(Wait());
    }

    private IEnumerator Wait()
    {
        MainManager.instance.AddTrigger("wait;1");
        MainManager.instance.AddTrigger("changescreen;#000000FF;#00000000;5");
        yield return new WaitForSeconds(6.0f);
        player.SetActive(true);
        Destroy(gameObject);
    }
}
