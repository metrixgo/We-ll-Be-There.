using TMPro;
using UnityEngine;

public class SewerFlashlight : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI prompt;
    [SerializeField] private AudioClip flashlight;

    private bool opened = false;
    private Light bulb;

    private void Start()
    {
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

        if (MainManager.instance.gameState != 1) return;

        if (Input.GetKeyDown(KeyCode.F))
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
}
