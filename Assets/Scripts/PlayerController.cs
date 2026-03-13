using NUnit.Framework.Constraints;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    [SerializeField] private AudioClip tileWalk;
    [SerializeField] private AudioClip grassWalk;
    [SerializeField] private AudioClip rockWalk;

    private Animator animator;
    private AudioSource ad;
    private Camera cam;
    private float speed = 2.5f;
    private float reachRange = 1.5f;
    private float sensitivity;
    private float rotationX = 0;
    private float velocityY = -1.0f;
    private CharacterController characterController;
    private Interactable currentInteractable;

    private void Start()
    {
        ad = GetComponent<AudioSource>();
        characterController = GetComponent<CharacterController>();
        sensitivity = PlayerPrefs.GetFloat("Sensitivity", 10.0f);
        cam = transform.Find("Camera").GetComponent<Camera>();
        animator = transform.Find("Camera").GetComponent<Animator>();
        rotationX = cam.transform.localEulerAngles.x;
    }

    private void Update()
    {
        if (MainManager.instance.gameState != 1)
        {
            animator.SetFloat("Speed", 0);
            return;
        }

        Vector3 move = (transform.right * Input.GetAxisRaw("Horizontal") + transform.forward * Input.GetAxisRaw("Vertical")).normalized * speed;
        animator.SetFloat("Speed", move.magnitude);
        if (characterController.isGrounded) velocityY = -1.0f;
        else velocityY -= 9.8f * Time.deltaTime;
        move.y += velocityY;
        characterController.Move(move * Time.deltaTime);
        rotationX -= Input.GetAxis("Mouse Y") * sensitivity;
        rotationX = Mathf.Clamp(rotationX, -90.0f, 90.0f);
        cam.transform.localRotation = Quaternion.Euler(rotationX, 0, 0);
        transform.Rotate(0, Input.GetAxis("Mouse X") * sensitivity, 0);

        RaycastHit hit;

        if (Physics.Raycast(cam.transform.position, cam.transform.forward, out hit, reachRange) && hit.collider.tag == "Interactable")
        {
            Interactable newInteractable = hit.collider.GetComponent<Interactable>();
            if (currentInteractable != null && currentInteractable != newInteractable) currentInteractable.SetFocused(false);
            currentInteractable = newInteractable;
            currentInteractable.SetFocused(true);
        }
        else if (currentInteractable != null)
        {
            currentInteractable.SetFocused(false);
            currentInteractable = null;
        }
        if (Input.GetMouseButtonDown(0) && MainManager.instance.gameState == 1 && currentInteractable)
        {
            currentInteractable.Interact();
        }

        if (animator.GetFloat("Speed") > 0.2f && !ad.isPlaying && Physics.Raycast(transform.position, -transform.up, out hit, characterController.height))
        {
            string s = hit.collider.tag.ToLower();
            bool flg = true;
            if (s == "tile") ad.clip = tileWalk;
            else if (s == "grass") ad.clip = grassWalk;
            else if (s == "rock") ad.clip = rockWalk;
            else flg = false;
            if (flg) ad.Play();
        }

    }

    public void Move(Vector3 dir)
    {
        characterController.enabled = false;
        transform.position += dir;
        characterController.enabled = true;
    }

    public void SetPosition(Vector3 pos)
    {
        characterController.enabled = false;
        transform.position = pos;
        characterController.enabled = true;
    }

    public void SetRotation(float y, float x)
    {
        transform.rotation = Quaternion.Euler(0, y, 0);
        rotationX = x;
        cam.transform.localRotation = Quaternion.Euler(rotationX, 0, 0);
    }
}