using UnityEngine;

public class Control : MonoBehaviour
{
    [SerializeField] private GameObject tv;
    [SerializeField] private AudioClip turnOffTV;

    private bool turned = false;

    public void TurnOff()
    {
        MainManager.instance.PlayEffect(turnOffTV);
        if (!turned)
        {
            turned = true;
            Destroy(tv);
        }
    }

    public bool IsClosed()
    {
        return turned; 
    }
}
