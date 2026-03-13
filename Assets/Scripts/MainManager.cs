using System.Collections;
using System.Collections.Generic;
using System.Net.Mime;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MainManager : MonoBehaviour
{
    public static MainManager instance;

    [SerializeField] private Image screen;
    [SerializeField] private GameObject dialogueScreen;
    [SerializeField] private GameObject pausedScreen;
    [SerializeField] private GameObject focus;
    [SerializeField] private PlayerController player;
    [SerializeField] private TextMeshProUGUI dialogueText;
    [SerializeField] private TextMeshProUGUI dialogueSpeaker;
    [SerializeField] private TextMeshProUGUI promptText;
    [SerializeField] private AudioSource musicPlayer;
    [SerializeField] private AudioSource effectsPlayer;
    [SerializeField] private AudioClip writtingEffect;

    public int gameState { get; private set; } = 1;
    private bool isExecutingTriggers = false;
    private bool atPausedScreen = false;
    
    private List<string> inventory = new List<string>();
    private List<string> triggers = new List<string>();

    private Dictionary<string, string> translations = new Dictionary<string, string>()
    {
        {"Am I... asleep?", "我这是...睡着了？"},
        {"Looks like everyone has left the school...", "看起来所有人都离开学校了..."},
        {"It's too dark now... I can barely see anything.", "这里太黑了...我几乎什么都看不到。"},
        {"I need to get home fast...", "我得赶紧回家..."},
        {"You", "你"},
        {"Door", "门"},
        {"Classroom Key", "教室钥匙"},
        {"Apple", "苹果"},
        {"Book", "书"},
        {"The door is locked... I knew it.", "门被锁了...我就知道。"},
        {"They must've not noticed me when they locked this classroom...", "他们在锁教室的时候一定没注意到我..."},
        {"I don't think the exit is this way...", "我不记得出口在这边..."},
        {"What was that?!", "刚才那什么玩意？！"},
        {"Damn it, I must get outta here real quick.", "该死的，我得赶紧离开这里。"},
        {"Phew... Feels good breathing fresh air.", "呼...能呼吸到新鲜空气真好。"},
        {"I should get on my bike and get home now.", "我现在得骑车回家了。"},
        {"Walking home this late might be a bad idea.", "这么晚走路回家可能不太好。"},
        {"I need to ride to get home faster.", "我需要骑车快点回家。"},
        {"Press [A] and [D] to ride", "按 [A] 和 [D] 骑车"},
        {"Turn Left", "左转"},
        {"Turn Right", "右转"},
        {"Bicycle", "自行车"},
        {"I think I'm not feeling well...", "我觉得我现在很难受..."},
        {"I still remember that scene I saw at school...", "我依然记得在学校看到的那个场景..."},
        {"Was is just my illusion...? But everything feels so real...", "难道只是我的幻觉吗...? 但是一切都感觉好真实..."},
        {"I still have a lot of work to do... I'm so tired...", "我还有很多事要做...我好累..."},
        {"Alright I need to go faster. I want to get home now. Now.", "好的，我需要骑得快一点。我想现在就回家。现在。"},
        {"What am I doing?! I need to go home! Why would I want to go back?!", "我在干什么？！我需要回家！为什么我还想回去？！"},
        {"Please... don't go back... please...", "求求了...别回去...求求了..."},
        {"What... what... what... happened?!", "什么...什么...这...发生了什么？！"},
        {"Shit... why... how... where did he pop up?!", "该死...为什么...为啥...他从哪出现的？！"},
        {"Oh no. Oh no. FUCK.", "不...不...该死。"},
        {"This is it. I'm done. It's over.", "完了。我完了。一切都结束了。"},
        {"Maybe... maybe... I can clean this up? ...... H-i-d-e him? Haha.", "也许...也许...我能收拾干净？...把他...藏起来？哈哈。"},
        {"I... well... I should rush home to get this shit cleaned up. Damn it.", "我...呃...我得赶紧回家把这处理了。该死。"},
        {"This is all for now! Follow me on itch to see how I work out this game!", "目前就这些内容啦！在 itch 上关注我以获得我的进程。"},
        {"Also, I'm a pretty new game developer, so if you have any suggestions, feel free to leave a comment!", "还有，我是一个挺新的游戏制作者，所以如果你有任何建议，请随便留个言！"}
    };

    private void Awake()
    {
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
        instance = this;
    }

    private void Start()
    {
        musicPlayer.volume = PlayerPrefs.GetFloat("Music", 30.0f) / 100.0f;
        effectsPlayer.volume = PlayerPrefs.GetFloat("Effects", 80.0f) / 100.0f;
    }

    private void Update()
    {
        if (!isExecutingTriggers && triggers.Count > 0)
        {
            isExecutingTriggers = true;
            gameState = 0;
            promptText.enabled = false;
            focus.SetActive(false);
            StartCoroutine(ExecuteTriggers());
        }

        if (Input.GetKeyDown(KeyCode.Escape) && !isExecutingTriggers)
        {
            if (atPausedScreen)
            {
                atPausedScreen = false;
                promptText.enabled = true;
                focus.SetActive(true);
                Cursor.lockState = CursorLockMode.Locked;
                Cursor.visible = false;
                gameState = 1;
                pausedScreen.SetActive(false);
            }
            else
            {
                atPausedScreen = true;
                promptText.enabled = false;
                focus.SetActive(false);
                Cursor.lockState = CursorLockMode.None;
                Cursor.visible = true;
                gameState = 0;
                pausedScreen.SetActive(true);
            }
        }

    }

    public void LoadScene(string s, float t)
    {
        StartCoroutine(LoadSceneCoroutine(s, t));
    }

    public void LoadScene(string s)
    {
        LoadScene(s, 0.5f);
    }

    public void PlayMusic(AudioClip ac)
    {
        musicPlayer.clip = ac;
        musicPlayer.Play();
    }

    public void PlayEffect(AudioClip ac)
    {
        effectsPlayer.clip = ac;
        effectsPlayer.Play();
    }

    public int ItemCount(string s)
    {
        int cnt = 0;
        foreach(string item in inventory)
        {
            if (s == item) cnt++;
        }
        return cnt;
    }

    public bool HasItem(string s)
    {
        return inventory.Contains(s);
    }

    public void AddItem(string s)
    {
        inventory.Add(s);
    }

    public void AddTrigger(string s)
    {
        triggers.Add(s);
    }

    public void SetPrompt(string s)
    {
        promptText.text = Translate(s);
    }

    private Color ParseColor(string colorString)
    {
        if (ColorUtility.TryParseHtmlString(colorString, out Color color)) return color;
        else return Color.black;
    }

    private string Translate(string s)
    {
        if (PlayerPrefs.GetString("Language", "English") == "English") return s;
        if (translations.ContainsKey(s)) return translations[s];
        return s;
    }

    private IEnumerator LoadSceneCoroutine(string s, float t)
    {
        AddTrigger("changescreen;#00000000;#000000FF;"+t);
        yield return new WaitForSeconds(t);
        SceneManager.LoadScene(s);
    }

    private IEnumerator ExecuteTriggers()
    {
        while (triggers.Count > 0)
        {
            string trig = triggers[0];
            triggers.RemoveAt(0);
            string[] s = trig.Split(";");
            string key = s[0].ToLower();
            if (key == "dialogue")
            {
                yield return StartCoroutine(DisplayDialogue(s[1], s[2]));
            }
            else if (key == "changescreen")
            {
                yield return StartCoroutine(ChangeScreen(ParseColor(s[1]), ParseColor(s[2]), float.Parse(s[3])));
            }
            else if (key == "moveplayer")
            {
                Vector3 dir = new Vector3(float.Parse(s[1]), float.Parse(s[2]), float.Parse(s[3]));
                player.Move(dir);
            }
            else if (key == "moveplayerto")
            {
                Vector3 pos = new Vector3(float.Parse(s[1]), float.Parse(s[2]), float.Parse(s[3]));
                player.SetPosition(pos);
            }
            else if (key == "prompt")
            {
                SetPrompt(s[1]);
            }
            else if (key == "flashdialogue")
            {
                gameState = 1;
                yield return StartCoroutine(DisplayDialogue(s[1], s[2], float.Parse(s[3])));
                gameState = 0;
            }
            else if (key == "wait")
            {
                yield return new WaitForSeconds(float.Parse(s[1]));
            }
            else
            {
                Debug.LogError("Trigger Not Found: " + trig);
            }
        }
        isExecutingTriggers = false;
        gameState = 1;
        promptText.enabled = true;
        focus.SetActive(true);
    }

    private IEnumerator ChangeScreen(Color s, Color e, float dur)
    {
        float t = 0;
        screen.color = s;
        while (t < dur)
        {
            yield return null;
            t += Time.deltaTime;
            screen.color = Color.Lerp(s, e, t / dur);
        }
        screen.color = e;
    }

    private IEnumerator DisplayDialogue(string speaker, string content, float length)
    {
        effectsPlayer.clip = writtingEffect;
        effectsPlayer.Play();
        content = Translate(content);
        dialogueSpeaker.text = Translate(speaker);
        dialogueText.text = "";
        dialogueScreen.SetActive(true);
        int idx = 0;
        float t = 0, gap = 0.04f;
        if (PlayerPrefs.GetString("Language", "English") == "Chinese") gap = 0.08f;
        yield return new WaitForSeconds(0.05f);
        while (idx < content.Length)
        {
            t += Time.deltaTime;
            if (t >= gap)
            {
                t -= gap;
                dialogueText.text += content[idx];
                idx++;
            }
            if (Input.GetMouseButtonDown(0) && length < 0)
            {
                dialogueText.text = content;
                break;
            }
            yield return null;
        }
        yield return new WaitForSeconds(0.05f);
        effectsPlayer.Stop();
        if(length >= 0) yield return new WaitForSeconds(length);
        else yield return new WaitUntil(() => Input.GetMouseButtonDown(0));
        dialogueScreen.SetActive(false);
    }

    private IEnumerator DisplayDialogue(string speaker, string content)
    {
        yield return StartCoroutine(DisplayDialogue(speaker, content, -1.0f));
    }
}
