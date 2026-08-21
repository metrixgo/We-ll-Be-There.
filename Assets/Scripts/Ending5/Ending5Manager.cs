using TMPro;
using UnityEngine;

public class Ending5Manager : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI danger;

    private bool reached = false;
    private float t = 0;
    private float hideT = 0;

    private void Update()
    {
        if (MainManager.instance.gameState != 1 || reached) return;

        if (t > 30.0f)
        {
            reached = true;
            MainManager.instance.StopMusic();
            MainManager.instance.AddTrigger("ending;SECRET ENDING 5/5 - .;Call... Mom?");
            return;
        }

        t += Time.deltaTime;
        hideT -= Time.deltaTime;
        float prog = Mathf.Min(1, Mathf.Max(0, t - 15.0f) / 15.0f);
        if(hideT <= 0 && Random.Range(0, 300) == 0)
        {
            hideT = Random.Range(0.1f, 0.3f);
        }
        if(hideT > 0) danger.color = Color.clear;
        else danger.color = Color.red * prog;
        RenderSettings.fogDensity = prog / 5.0f + 0.01f;
        RenderSettings.fogColor = Color.Lerp(Color.white, Color.black, prog);
        RenderSettings.ambientIntensity = 1 - prog;
    }
}