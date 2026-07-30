using UnityEngine;
using UnityEngine.UI;

public class StartKiller : MonoBehaviour
{
    [SerializeField] private GameObject killer;
    [SerializeField] private AudioClip chase;
    [SerializeField] private AudioClip jumpscare;
    [SerializeField] private RawImage ri;
    [SerializeField] private Material mat;
    [SerializeField] private GameObject[] books;

    private void OnTriggerEnter(Collider other)
    {
        killer.SetActive(true);
        SewerMusicManager.instance.ChangeTo(chase, 0, 100);
        MainManager.instance.PlayEffect(jumpscare);
        ri.material = mat;
        foreach (GameObject book in books)
        {
            book.tag = "Untagged";
        }
        Destroy(gameObject);
    }
}