using System.Collections;
using UnityEngine;

public class CrushTrigger : MonoBehaviour
{
    [SerializeField] private Bicycle bicycle;
    [SerializeField] private AudioClip crush;
    [SerializeField] private GameObject man;

    private void OnTriggerEnter(Collider other)
    {
        if (!other.CompareTag("Bicycle")) return ;
        StartCoroutine(Crush());
    }

    private IEnumerator Crush()
    {
        man.SetActive(false);
        MainManager.instance.AddTrigger("changescreen;#000000FF;#000000FF;1");
        MainManager.instance.PlayEffect(crush);
        bicycle.GetOff();
        yield return new WaitForSeconds(0.5f);
        MainManager.instance.LoadScene("CleanUpScene");
    }
}
