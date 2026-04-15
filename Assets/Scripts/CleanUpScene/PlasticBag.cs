using System;
using UnityEngine;

public class PlasticBag : MonoBehaviour
{
    [SerializeField] private AudioClip soundEffect;
    [SerializeField] private GameObject hold;
    [SerializeField] private GameObject environment;
    [SerializeField] private GameObject mop;
    [SerializeField] private Vector3 angle;

    public void PickUp()
    {
        if (MainManager.instance.gameState != 1) return;

        MainManager.instance.AddItem(name);
        MainManager.instance.PlayEffect(soundEffect);
        MainManager.instance.ClearTasks();
        MainManager.instance.AddTask("Go back to pack up the body");
        transform.SetParent(hold.transform);
        transform.localPosition = Vector3.zero;
        transform.localRotation = Quaternion.Euler(angle);
        tag = "Untagged";
        mop.transform.SetParent(environment.transform);
        mop.transform.position = new Vector3(-58.43557f, 1.02871f, 352.8873f);
        mop.transform.rotation = Quaternion.Euler(-80.0f, 180.0f, 0);
    }
}
