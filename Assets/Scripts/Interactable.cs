using System.Collections;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class Interactable : MonoBehaviour
{
    private Outline outline;
    [SerializeField] private UnityEvent onInteraction;

    private void Start()
    {
        outline = GetComponent<Outline>();
        SetFocused(false);
    }

    public void Interact()
    {
        SetFocused(false);
        onInteraction.Invoke();
    }

    public void SetFocused(bool b)
    {
        if (outline) outline.enabled = b;
        if(b) MainManager.instance.SetPrompt(gameObject.name);
        else MainManager.instance.SetPrompt("");
    }
}
