using UnityEngine;

public class Flashlight : MonoBehaviour
{
    [SerializeField] private AudioClip pickUp;
    [SerializeField] private AudioClip flashlight;
    [SerializeField] private GameObject cam;
    [SerializeField] private GameObject bulb;

    private bool pickedUp = false;
    private bool opened = false;

    private void Update()
    {
        if (MainManager.instance.gameState != 1) return ;
        if (Input.GetKeyDown(KeyCode.F) && pickedUp)
        {
            bulb.SetActive(!opened);
            opened = !opened;
            MainManager.instance.PlayEffect(flashlight);
        }
    }

    public void PickUp()
    {
        if (MainManager.instance.gameState != 1) return;
        pickedUp = true;
        MainManager.instance.AddItem(name);
        MainManager.instance.PlayEffect(pickUp);
        transform.SetParent(cam.transform);
        transform.localPosition = new Vector3(0, 0, -0.2f);
        transform.localRotation = Quaternion.Euler(0, 0, 0);
        tag = "Untagged";
    }
}
