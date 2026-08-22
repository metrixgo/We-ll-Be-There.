using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class AfterEnding4Manager : MonoBehaviour
{
    [SerializeField] private Image panel;
    [SerializeField] private CanvasGroup cg;

    private IEnumerator Start()
    {
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
        panel.raycastTarget = true;
        panel.color = Color.black;
        float t = 0;
        while (t < 1.5f)
        {
            t += Time.deltaTime;
            panel.color = Color.Lerp(Color.black, Color.clear, t / 1.5f);
            yield return null;
        }
        panel.color = Color.clear;
        yield return new WaitForSeconds(22.0f);
        t = 0;
        while(t < 1.5f)
        {
            t += Time.deltaTime;
            cg.alpha = t / 1.5f;
            yield return null;
        }
        cg.alpha = 1.0f;
        panel.raycastTarget = false;
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
    }
}
