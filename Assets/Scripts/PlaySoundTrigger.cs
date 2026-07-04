using UnityEngine;

public class PlaySoundTrigger : MonoBehaviour
{
    [SerializeField] private bool isEffect = true;
    [SerializeField] private bool selfDestructs = true;
    [SerializeField] private AudioClip clip;

    private void OnTriggerEnter(Collider other)
    {
        if (isEffect) MainManager.instance.PlayEffect(clip);
        else MainManager.instance.PlayMusic(clip);
        if (selfDestructs) Destroy(gameObject);
    }
}
