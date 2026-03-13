using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MainMenuManager : MonoBehaviour
{
    [SerializeField] private GameObject[] cars;
    [SerializeField] private Image panel;
    [SerializeField] private GameObject startScreen;
    [SerializeField] private GameObject optionScreen;
    [SerializeField] private Image chinese;
    [SerializeField] private Image english;
    [SerializeField] private AudioSource musicPlayer;
    [SerializeField] private AudioSource effectsPlayer;

    private void Start()
    {
        startScreen.SetActive(true);
        optionScreen.SetActive(false);
        musicPlayer.volume = PlayerPrefs.GetFloat("Music", 30.0f) / 100.0f;
        effectsPlayer.volume = PlayerPrefs.GetFloat("Effects", 80.0f) / 100.0f;
        StartCoroutine(SetUp());
        StartCoroutine(GenerateCars());
    }

    public void ToChinese()
    {
        effectsPlayer.Play();
        chinese.color = new Color(0.25f, 0.25f, 0.25f);
        english.color = new Color(0.75f, 0.75f, 0.75f);
        PlayerPrefs.SetString("Language", "Chinese");
    }

    public void ToEnglish()
    {
        effectsPlayer.Play();
        english.color = new Color(0.25f, 0.25f, 0.25f);
        chinese.color = new Color(0.75f, 0.75f, 0.75f);
        PlayerPrefs.SetString("Language", "English");
    }

    public void ToOptions()
    {
        startScreen.SetActive(false);
        optionScreen.SetActive(true);
        if (PlayerPrefs.GetString("Language", "English") == "English") ToEnglish();
        else ToChinese();
    }

    public void ToStart()
    {
        effectsPlayer.Play();
        startScreen.SetActive(true);
        optionScreen.SetActive(false);
    }

    public void StartGame()
    {
        StartCoroutine(StartGameCoroutine());
    }

    public void QuitGame()
    {
        effectsPlayer.Play();
        Application.Quit();
    }

    private IEnumerator StartGameCoroutine()
    {
        effectsPlayer.Play();
        panel.raycastTarget = true;
        panel.color = Color.clear;
        yield return new WaitForSeconds(0.5f);
        float t = 0;
        while (t < 1.0f)
        {
            t += Time.deltaTime;
            panel.color = Color.Lerp(Color.clear, Color.black, t / 1.0f);
            yield return null;
        }
        panel.color = Color.black;
        SceneManager.LoadScene("SchoolScene");
    }

    private IEnumerator SetUp()
    {
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
        panel.raycastTarget = false;
    }

    private IEnumerator GenerateCars()
    {
        while (true)
        {
            yield return new WaitForSeconds(Random.Range(0.5f, 5.0f));
            GameObject o = cars[Random.Range(0, cars.Length)];
            if(Random.Range(0,2) == 0)
            {
                Instantiate(o, new Vector3(30.0f, 0, -2.5f), Quaternion.Euler(0, -90.0f, 0));
            }
            else
            {
                Instantiate(o, new Vector3(-70.0f, 0, -9.0f), Quaternion.Euler(0, 90.0f, 0));
            }
        }
    }
}
