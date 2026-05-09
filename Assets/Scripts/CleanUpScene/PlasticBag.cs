using UnityEngine;

public class PlasticBag : MonoBehaviour
{
    [SerializeField] private GameObject mop;

    public void Effects()
    {
        MainManager.instance.ClearTasks();
        MainManager.instance.AddTask("Go back to pack up the body");
        mop.transform.SetParent(null);
        mop.transform.position = new Vector3(-58.43557f, 1.02871f, 352.8873f);
        mop.transform.rotation = Quaternion.Euler(-80.0f, 180.0f, 0);
    }
}
