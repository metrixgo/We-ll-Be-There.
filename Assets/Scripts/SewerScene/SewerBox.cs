using UnityEngine;

public class SewerBox : MonoBehaviour
{
    [SerializeField] private GameObject opened;
    [SerializeField] private AudioClip open;

    public void OpenBox()
    {
        MainManager.instance.PlayEffect(open);
        opened.SetActive(true);
        Destroy(gameObject);
    }
}
