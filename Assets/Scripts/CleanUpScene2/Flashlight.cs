using UnityEngine;

public class Flashlight : MonoBehaviour
{
    [SerializeField] private AudioClip pickUp;
    [SerializeField] private AudioClip flashlight;
    [SerializeField] private GameObject cam;

    private bool pickedUp = false;
    private bool opened = false;
    private Light bulb;
    private MeshRenderer mr;

    private void Start()
    {
        mr = GetComponent<MeshRenderer>();
        bulb = GetComponent<Light>();
    }

    private void Update()
    {
        if (opened)
        {
            Shader.SetGlobalFloat("_LightOn", 1.0f);
            Shader.SetGlobalVector("_LightPos", bulb.transform.position);
            Shader.SetGlobalVector("_LightDir", bulb.transform.forward);
            Shader.SetGlobalFloat("_LightCosAngle", Mathf.Cos(bulb.spotAngle * 0.5f * Mathf.Deg2Rad));
            Shader.SetGlobalFloat("_LightRange", bulb.range / 2.5f);
        }
        else
        {
            Shader.SetGlobalInt("_LightOn", 0);
        }

        if (MainManager.instance.gameState != 1) return ;
        if (Input.GetKeyDown(KeyCode.F) && pickedUp)
        {
            opened = !opened;
            bulb.enabled = opened;
            if (opened)
            {
                MainManager.instance.SetPromptColor(Color.red);
                MainManager.instance.SetFocusColor(Color.red);
            }
            else
            {
                MainManager.instance.SetPromptColor(Color.white);
                MainManager.instance.SetFocusColor(Color.white);
            }
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
        mr.enabled = false;
    }

    public bool IsOpened()
    {
        return opened;
    }
}
