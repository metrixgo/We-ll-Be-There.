using UnityEngine;

public class ExitDoor : MonoBehaviour 
{
    private AudioSource ad;
    private bool first = false;

    private void Start()
    {
        ad = GetComponent<AudioSource>();
    }

    public void OpenDoor()
    {
        if (!first)
        {
            ad.Play();
            first = true;
            MainManager.instance.LoadScene("GoHomeScene", 2.0f);
        }
    }
}
