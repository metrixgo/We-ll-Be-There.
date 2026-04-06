using System.Collections;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    [SerializeField] private AudioClip tileWalk;
    [SerializeField] private AudioClip grassWalk;
    [SerializeField] private AudioClip rockWalk;
    [SerializeField] private AudioClip woodWalk;

    private AudioSource ad;
    private Camera cam;
    private float speed = 2.5f;
    private float runSpeed = 5.0f;
    private float reachRange = 1.5f;
    private float sensitivity;
    private float rotationX = 0;
    private float velocityY = -1.0f;
    private Vector3 move;
    private bool canRun = false;
    private int state = 0;
    private CharacterController characterController;
    private Interactable currentInteractable;

    private void Start()
    {
        StartCoroutine(CameraBobbing());
        ad = GetComponent<AudioSource>();
        characterController = GetComponent<CharacterController>();
        sensitivity = PlayerPrefs.GetFloat("Sensitivity", 10.0f);
        cam = transform.Find("Camera").GetComponent<Camera>();
        rotationX = cam.transform.localEulerAngles.x;
    }

    private void Update()
    {
        if (MainManager.instance.gameState != 1)
        {
            move = Vector3.zero;
            return;
        }

        float f = speed;
        if (canRun && (Input.GetKey(KeyCode.LeftShift) || Input.GetKey(KeyCode.RightShift))) f = runSpeed;
        move = (transform.right * Input.GetAxisRaw("Horizontal") + transform.forward * Input.GetAxisRaw("Vertical")).normalized * f;

        if (characterController.isGrounded) velocityY = -1.0f;
        else velocityY -= 9.8f * Time.deltaTime;
        characterController.Move((move + Vector3.up * velocityY) * Time.deltaTime);
        rotationX -= Input.GetAxis("Mouse Y") * sensitivity;
        rotationX = Mathf.Clamp(rotationX, -90.0f, 90.0f);
        cam.transform.localRotation = Quaternion.Euler(rotationX, 0, 0);
        transform.Rotate(0, Input.GetAxis("Mouse X") * sensitivity, 0);

        if (Physics.Raycast(cam.transform.position, cam.transform.forward, out RaycastHit hit, reachRange, ~0, QueryTriggerInteraction.Collide) && hit.collider.tag == "Interactable")
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

    }

    public void CanRun(bool b)
    {
        canRun = b;
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
        SetRotation(y, x, 0);
    }

    public void SetRotation(float y, float x, float l)
    {
        StartCoroutine(TurnTo(y, x, l));
    }

    private IEnumerator TurnTo(float y, float x, float l)
    {
        float t = 0;
        float startY = transform.eulerAngles.y;
        float startX = rotationX;
        while (t < l)
        {
            transform.rotation = Quaternion.Euler(0, Mathf.LerpAngle(startY, y, t / l), 0);
            rotationX = Mathf.Lerp(startX, x, t / l);
            cam.transform.localRotation = Quaternion.Euler(rotationX, 0, 0);
            t += Time.deltaTime;
            yield return null;
        }
        transform.rotation = Quaternion.Euler(0, y, 0);
        rotationX = x;
        cam.transform.localRotation = Quaternion.Euler(rotationX, 0, 0);
    }

    private IEnumerator CameraBobbing()
    {
        Transform cam = transform.Find("Camera");
        float mid = 0.75f;
        float t = 0;
        float[] ys = { mid, mid, mid };
        float[] bottom = { -1.0f, mid - 0.05f, mid - 0.08f };
        bool flg = false;
        while (true)
        {
            float f = move.magnitude;
            int prev = state;
            if (f < 0.2f) state = 0;
            else if (Mathf.Abs(f - speed) < 0.05f) state = 1;
            else if (Mathf.Abs(f - runSpeed) < 0.05f) state = 2;

            ys[0] = Mathf.Sin(t * 2 * Mathf.PI / 2.0f) * 0.02f;
            ys[1] = Mathf.Sin(t * 2 * Mathf.PI / 0.8f) * 0.05f;
            ys[2] = Mathf.Sin(t * 2 * Mathf.PI / 0.5f) * 0.08f;

            Vector3 temp = cam.localPosition;
            temp.y = ys[state] + mid;

            if (prev != state)
            {
                yield return StartCoroutine(ResetCamera(mid));
                t = 0;
            }
            else
            {
                cam.localPosition = temp;
                if(Mathf.Abs(temp.y - bottom[state]) < 0.02f && !flg)
                {
                    if (move.magnitude > 0.2f && Physics.Raycast(transform.position, -transform.up, out RaycastHit hit, characterController.height))
                    {
                        string s = hit.collider.tag.ToLower();
                        if (s == "tile") ad.clip = tileWalk;
                        else if (s == "grass") ad.clip = grassWalk;
                        else if (s == "rock") ad.clip = rockWalk;
                        else if (s == "wood") ad.clip = woodWalk;
                        else ad.clip = null;
                    }
                    flg = true;
                    if(ad.clip != null) ad.Play();
                }
                else if (Mathf.Abs(temp.y - bottom[state]) > 0.02f) flg = false;
            }

            t += Time.deltaTime;
            yield return null;
        }
    }

    private IEnumerator ResetCamera(float mid)
    {
        Transform cam = transform.Find("Camera");
        while (Mathf.Abs(cam.localPosition.y - mid) > 0.002f)
        {
            Vector3 temp = new Vector3(cam.localPosition.x, cam.localPosition.y, cam.localPosition.z);
            if (temp.y > mid) temp.y -= 0.0015f;
            else temp.y += 0.0015f;
            cam.localPosition = temp;
            yield return null;
        }
        cam.localPosition = new Vector3(cam.localPosition.x, mid, cam.localPosition.z);
    }
}