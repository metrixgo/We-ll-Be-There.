using System.Collections;
using UnityEngine;

public class WakeUp : MonoBehaviour
{
    [SerializeField] private GameObject player;

    private void Start()
    {
        player.SetActive(false);
        StartCoroutine(Wait());
    }

    private IEnumerator Wait()
    {
        MainManager.instance.AddTrigger("changescreen;#000000FF;#00000000;15");
        yield return new WaitForSeconds(15.0f);
        player.SetActive(true);
        Destroy(gameObject);
    }

}
