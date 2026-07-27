using UnityEngine;

public class SewerMusicChange : MonoBehaviour
{
    [SerializeField] private AudioClip music;
    [SerializeField] private float changeTime;
    [SerializeField] private float volume;

    private void OnTriggerEnter(Collider other)
    {
        SewerMusicManager.instance.ChangeTo(music, changeTime, volume);
        Destroy(gameObject);
    }
}
