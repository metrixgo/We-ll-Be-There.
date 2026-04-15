using UnityEngine;

public class Bicycle : MonoBehaviour
{
    [SerializeField] private GameObject player;
    [SerializeField] private GameObject ridePlayer;
    [SerializeField] private GameObject wall;
    [SerializeField] private GameObject trigger;
    [SerializeField] private AudioClip ridingSound;

    private bool getOn = false;
    private bool isLeft = true;
    private bool controlling = false;
    private float maxSpeed = 13.0f;
    private float stepSpeed = 1.0f;
    private float velocity = 0;
    private float angle = 0;
    private float rotationX;
    private float sensitivity;
    private float angCount = 0;
    private float angAim = 0.114f;
    private Camera cam;
    private AudioSource ad;

    private void Start()
    {
        ad = GetComponent<AudioSource>();
        sensitivity = PlayerPrefs.GetFloat("Sensitivity", 10.0f);
        cam = ridePlayer.transform.Find("Camera").GetComponent<Camera>();
        rotationX = cam.transform.localEulerAngles.x;
    }

    public void GetOn()
    {
        tag = "Bicycle";
        player.SetActive(false);
        ridePlayer.SetActive(true);
        Destroy(wall);
        Destroy(trigger);
        ad.Play();
        MainManager.instance.SetPrompt("Press [A] and [D] to ride", true);
        MainManager.instance.ClearTasks();
        MainManager.instance.AddTask("Ride home");
        getOn = true;
        controlling = true;
        ad.clip = ridingSound;
    }

    private void Update()
    {
        if (MainManager.instance.gameState != 1 || !getOn)
        {
            ad.Stop();
            return ;
        }
        if (velocity > 0) velocity -= Time.deltaTime * 7.0f;
        else velocity = 0;

        float key = Input.GetAxisRaw("Horizontal");
        if (controlling)
        {
            if (key < 0 && isLeft || key > 0 && !isLeft)
            {
                isLeft = !isLeft;
                velocity += stepSpeed;
                velocity = Mathf.Min(velocity, maxSpeed);
                if (!ad.isPlaying) ad.Play();
            }
        }
        else
        {
            velocity += stepSpeed;
            velocity = Mathf.Min(velocity, maxSpeed);
            if (!ad.isPlaying) ad.Play();
        }

        if (velocity < 1.0f) ad.Stop();

            Vector3 move = velocity * Time.deltaTime * transform.forward;
        transform.Translate(move, Space.World);
        
        if (angle != 0)
        {
            if(angAim == 0.114f)
            {
                if (angle < 0) angAim = transform.eulerAngles.y - 90.0f;
                else angAim = transform.eulerAngles.y + 90.0f;
            }
            angCount += velocity * angle * Time.deltaTime;
            transform.Rotate(Vector3.up * velocity * angle * Time.deltaTime);
            if (Mathf.Abs(angCount) >= 90.0f)
            {
                transform.rotation = Quaternion.Euler(0, angAim, 0);
                angle = 0;
                angAim = 0.114f;
                angCount = 0;
            }
        }

        rotationX -= Input.GetAxis("Mouse Y") * sensitivity;
        rotationX = Mathf.Clamp(rotationX, -90.0f, 90.0f);
        cam.transform.localRotation = Quaternion.Euler(rotationX, 0, 0);
        ridePlayer.transform.Rotate(0, Input.GetAxis("Mouse X") * sensitivity, 0);
    }

    public void Accelerate()
    {
        maxSpeed = 20.0f;
        stepSpeed = 5.0f;
    }

    public void LoseControl()
    {
        controlling = false;
    }

    public void GetOff()
    {
        ad.Stop();
        getOn = false;
    }

    public void SetAngle(float f)
    {
        angle = f;
    }
}
