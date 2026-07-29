using UnityEngine;
using UnityEngine.UI;

public class StartKiller : MonoBehaviour
{
    [SerializeField] private GameObject killer;
    [SerializeField] private AudioClip chase;
    [SerializeField] private AudioClip jumpscare;
    [SerializeField] private RawImage ri;
    [SerializeField] private Material mat;

    private void OnTriggerEnter(Collider other)
    {
        killer.SetActive(true);
        SewerMusicManager.instance.ChangeTo(chase, 0, 100);
        MainManager.instance.PlayEffect(jumpscare);
        ri.material = mat;
        Destroy(gameObject);
    }
}
