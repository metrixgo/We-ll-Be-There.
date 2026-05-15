using UnityEngine;
using UnityEngine.UI;

public class BlurryCamera : MonoBehaviour
{
    [SerializeField] private RawImage img;

    private Camera cam;

    private void Start()
    {
        cam = GetComponent<Camera>();
    }

    private void Update()
    {
        
    }
}
