using System.Collections;
using UnityEngine;

public class StartingClimbPlayer : MonoBehaviour
{
    [SerializeField] private GameObject player;

    private void Start()
    {
        StartCoroutine(Climbing());
    }

    private IEnumerator Climbing()
    {
        MainManager.instance.AddTrigger("changescreen;#000000FF;#00000000;4");
        MainManager.instance.AddTrigger("wait;12");
        yield return new WaitForSeconds(16.0f);
        player.SetActive(true);
        Destroy(gameObject);
    }
}
