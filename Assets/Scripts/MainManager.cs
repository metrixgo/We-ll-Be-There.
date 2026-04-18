using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MainManager : MonoBehaviour
{
    public static MainManager instance;

    [SerializeField] private Image screen;
    [SerializeField] private Image endScreen;
    [SerializeField] private GameObject dialogueScreen;
    [SerializeField] private GameObject pausedScreen;
    [SerializeField] private GameObject endingScreen;
    [SerializeField] private GameObject focus;
    [SerializeField] private GameObject endingReturnMenu;
    [SerializeField] private PlayerController player;
    [SerializeField] private TextMeshProUGUI dialogueText;
    [SerializeField] private TextMeshProUGUI dialogueSpeaker;
    [SerializeField] private TextMeshProUGUI promptText;
    [SerializeField] private TextMeshProUGUI taskText;
    [SerializeField] private TextMeshProUGUI endingTitle;
    [SerializeField] private TextMeshProUGUI endingText;
    [SerializeField] private AudioSource musicPlayer;
    [SerializeField] private AudioSource effectsPlayer;
    [SerializeField] private AudioClip writtingEffect;

    public int gameState { get; private set; } = 1;
    private bool isExecutingTriggers = false;
    private bool isLoadingScene = false;
    private bool atPausedScreen = false;
    private bool atEndingScreen = false;

    private List<string> inventory = new List<string>();
    private List<string> triggers = new List<string>();
    private List<string> tasks = new List<string>();

    private Dictionary<string, string> translations = new Dictionary<string, string>()
    {
        {"Am I... asleep?", "我这是...睡着了？"},
        {"Looks like everyone has left the school...", "看起来所有人都离开学校了..."},
        {"It's too dark now... I can barely see anything.", "这里太黑了...我几乎什么都看不到。"},
        {"I need to get home fast...", "我得赶紧回家..."},
        {"Leave school", "离开学校"},
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
        {"Get on the bike", "上自行车"},
        {"Ride home", "骑车回家"},
        {"Get home", "赶回家"},
        {"Get a mop from the bathroom", "从厕所拿一个拖把"},
        {"Go back to clean up the blood", "回去把血迹清理干净"},
        {"Get a plastic bag from the garage", "从车库拿一个塑料袋"},
        {"Go back to pack up the body", "回去把尸体包好"},
        {"Bury the body in the backyard", "将尸体埋在后院"},
        {"Take the shovel from the garage", "从车库拿一把铲子"},
        {"Bury the body", "将尸体埋了"},
        {"Take a shower", "去冲澡"},
        {"Go to bed", "上床睡觉"},
        {"Walking home this late might be a bad idea.", "这么晚走路回家可能不太好。"},
        {"I need to ride to get home faster.", "我需要骑车快点回家。"},
        {"Press [A] and [D] to ride", "按 [A] 和 [D] 骑车"},
        {"Press [Shift] to run", "按 [Shift] 奔跑"},
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
        {"Also, I'm a pretty new game developer, so if you have any suggestions, feel free to leave a comment!", "还有，我是一个挺新的游戏制作者，所以如果你有任何建议，请随便留个言！"},
        {"Hello, sir.", "你好，先生。"},
        {"H... Hello?", "你...你好？"},
        {"What are you doing out here so late?", "这么晚了你在外面做什么？"},
        {"Um... I... I just came back from school...", "呃...我...我刚刚从学校回来。"},
        {"Are you sure? It's already 2 o'clock midnight.", "你确定吗？现在已经半夜2点了。"},
        {"Well... um... I just accidentally fell asleep in my classroom.", "呃...嗯...我就是不小心在我教室睡着了。"},
        {"...Ok. I just heard a large sound somewhere, it's like a car crash. Do you know where it came from?", "......好吧。我刚刚听到在某处传来一声巨响，像是车祸。你知道这是从哪里传来的吗？"},
        {"Oh. I have no idea. You must be imagining.", "哦。我不知道。你肯定是在想象。"},
        {"I don't think that's true, sir. It woke me up from my sleep.", "我不这么认为，先生。那声音把我都给惊醒了。"},
        {"Alright. Seems like you have no idea what you're doing as well. I'm going to call backups to come and check this out. I don't want anything bad going on here to disrupt my sleep.", "好吧。看来你也没主意。我到时候会叫后援来检查这事。我可不想这里发生任何坏事来打扰我休息。"},
        {"Wait... No...", "等等...不..."},
        {"Mhm?", "嗯？"},
        {"Well... Never mind.", "呃...没什么。"},
        {"It's dangerous out here. Get home now and we'll investigate this.", "这里很危险。你现在就赶紧回家吧，我们会调查这事。"},
        {"Okay... Thanks for the advice...", "好吧...谢谢你的提醒..."},
        {"Policewoman", "女警"},
        {"I don't want to give up. I can still hide him. I still have a chance.", "我不想放弃。我仍然能把他藏起来。我还有机会。"},
        {"No. No. No. My life will be ruined. Everything will fall apart.", "不。不。不。我的人生会被毁掉。我的一切都会崩塌。"},
        {"Why... I'm still in school... I can have a good future...", "为什么...我还在上学...我是可以有一个美好的未来的..."},
        {"Please... I just made a mistake... we all make mistakes... I don't want this to cost my entire life...", "求求了...我就犯了个错...我们都会犯错...我不想为此付出整个人生..."},
        {"Maybe... maybe you're right...", "也许...也许你是对的..."},
        {"Bed", "床"},
        {"Put Down", "放下"},
        {"Dig", "挖"},
        {"Take a Shower", "冲澡"},
        {"Packed Body", "尸袋"},
        {"Shovel", "铲子"},
        {"Call the Police", "报警"},
        {"Plastic Bag", "塑料袋"},
        {"Body", "尸体"},
        {"Blood", "血"},
        {"Mop", "拖把"},
        {"Cover", "覆盖"},
        {"Talk To", "说话"},
        {"I can't believe it was you...", "我真不敢相信是你..."},
        {"I'm... I'm sorry...", "我...我很抱歉..."},
        {"I'm glad you turned in yourself. This makes things much easier for both you and me.", "我很高兴你选择了自首。这让对你我的事情都变得很简单了。"},
        {"Yeah... Shall we go now?", "是的...我们现在走吗？"},
        {"Yes. Please sit on the back. We'll make sure to give you a fair result.", "嗯。请坐到后座吧。我们会确保给你一个公平的结果。"},
        {"Thanks...", "谢谢..."},
        {"Ok, I think this is fine now.", "好了。我觉得现在这个应该没问题了。"},
        {"Now I need to grab a plastic bag from home to pack this up.", "现在我需要回家拿个塑料袋把这个装起来。"},
        {"I can't clean these up with my hands. I need to go home to get a mop.", "我没法用手收拾这些。我得回家拿个拖把。"},
        {"I need to get a mop to clean up all the blood.", "我得拿个拖把把血都清理干净。"},
        {"I believe it's somewhere in a bathroom.", "我记得应该在一个浴室的某个地方里。"},
        {"I think the plastic bags are in the garage.", "我认为塑料袋都在车库里。"},
        {"This door is blocked...", "这个门被挡住了..."},
        {"I need to grab a plastic bag from home to pack this body in.", "我得回家拿个塑料袋把这个尸体装进去。"},
        {"Ok this looks... fine...", "好了。这看起来...还可以..."},
        {"Time to bury it in my backyard.", "是时候把它埋在我后院了。"},
        {"I hope no one will notice me on my way back... hopefully...", "希望我回去的路上没人注意到我...希望吧..."},
        {"Whoo...", "呼..."},
        {"It's all done now... Finally I can have a good rest...", "现在一切都做完了...我总算可以好好休息了..."},
        {"I didn't do anything wrong, right? I didn't do anything. No one saw it. Nothing happened.", "我没有做错任何事，对吧？我什么都没做。没人看见。什么都没发生。"},
        {"Tomorrow will be a fresh day. I'll eat. I'll work. I'll walk. I'll play. I'll watch. I'll sleep. I'll relax. I'll be a normal person. I'll be like everyone else. I'll be fine. I'll be fine.  I'll be fine.", "明天会是新的一天。我会吃饭。我会学习。我会散步。我会玩东西。我会看东西。我会睡觉。我会放松。我会是个正常人。我会像其他人一样。我会没事。我会没事。我会没事。"},
        {"Please let this thing end...", "请让这一切结束吧..."},
        {"They are watching me...", "他们在看着我..."},
        {"Alright... Time to go to bed...", "好了...该去睡觉了..."},
        {"Ahhh I just love showering...", "啊啊啊我就是喜欢冲澡..."},
        {"Water. Please bring away my feelings...", "水啊。请带走我的心情..."},
        {"Alright... I really need to go to bed now.", "好了...我现在真的得去睡觉了.."},
        {"I think that's enough showering. I really need to go to bed. I'm just so exhausted.", "我觉得我已经洗够了。我真的需要去睡觉。我就是太累了。"},
        {"Why did I come back without picking up the body?!", "我怎么没把尸体拿上就回来了？！"},
        {"I'm so stupid. I need to go back to pick up that packed body.", "我太笨了。我得回去把那尸袋拿回来。"},
        {"Nice.", "不错。"},
        {"Now I need to take the shovel from the garage to bury this body.", "现在我需要从车库拿那个铲子把这尸体埋了。"},
        {"I need a shovel to bury this.", "我需要铲子才能把这个埋了。"},
        {"Now I just need to cover all of these up.", "现在我只需要把这些都盖上就行了。"},
        {"Phew... Finally...", "呼...终于..."},
        {"I'm so tired...", "我好累..."},
        {"I should take a shower and sleep...", "我该去洗个澡然后睡觉了..."},
        {"May god bless me...", "愿上帝保佑我..."},
        {"I don't really want to sleep here...", "我不是很想睡在这..."},
        {"ENDING 1/5: SURRENDER", "结局 1/5：自首"},
        {"You surrendered yourself to the police. They brought you to the police station and asked what happened. After describing what you had gone through, they let you stay in a private room to rest. You thought you made the right choice and were very relieved, but you could feel something strange was going on. And when you realized it, it was too late.", "你向警察自首了。他们把你带到了警察局并询问发生了什么。当你描述完事情经过后，他们让你去了一个私人房间休息。你认为你做出了正确的选择并放下心来，但是你总感觉有什么奇怪的地方。并且当你意识到的时候，一切都太迟了。"},
        {"I can't see anything... Everything is black...", "我什么都看不到...一切都是黑色..."},
        {"Window", "窗户"},
        {"Wh... What?", "什... 什么？"},
        {"It looks like there're some candles in a distance...", "好像不远处有些蜡烛..."},
        {"Hammer", "锤子"},
        {"CRASHED BIKE FOUND: The local police discovered an abandoned bike broken into pieces on the sidewalk.", "被发现的撞毁的自行车：当地警方在人行道上发现了一辆被撞得支离破碎的废弃自行车。"},
        {"There were blood traces on the wheels and investigators believe there might have been an accident last night that no one noticed.", "在轮胎上发现了血迹，调查者认为昨晚发生了一起不为人知的事故。"},
        {"MAYOR'S SON WENT MISSING: The mayor announced that his son went missing last night.", "市长的儿子失踪：市长昨晚宣布他的儿子失踪了。"},
        {"The police have opened an investigation for this. Almost all the police force was involved.", "警方已经开始了调查。几乎所有警力都参与其中。"},
        {"The mayor said that he will use all his power and resources to find his son, and if he knows anyone who did anything to his son, they will be harshly punished.", "市长表示他会用自己所有的权力和资源去找他的儿子，并且一旦他知道有任何人对他的儿子干了任何事，他们都会被严厉地惩罚。"},
        {"MYSTERIOUS FIGURE REPORTED: A 12-year-old boy named Charlie woke up last night and saw a tall, black figure outside the window.", "神秘的身影被报告：一名叫查理的12岁的男孩从昨晚醒来时在窗外发现了一个高大的黑色身影。"},
        {"He said he saw the figure carrying a large bag with both of his hands. When asked for more details, Charlie said he was scared at that time and ran to his mom.", "他说他看到那身影双手拿着一个很大的袋子。当被询问具体情况时，查理表示他当时太害怕了并跑向了他的妈妈。"},
        {"When they went to the window again, the figure was gone. They then reported this incident to the police later.", "当他们一起再去那个窗户时，身影已经消失了。他们随后将这一情况报告给了警察。"},
        {"Newspaper", "报纸"},
        {"I still want to see the last newspaper...", "我还是想看那最后一个报纸..."},
        {"I still want to see the remaining two newspapers...", "我还是想看剩下的那两份报纸..."},
        {"I really want to se all the newspapers...", "我真的很想看看所有报纸..."},
        {"Alarm Clock", "闹钟"},
        {"Instant Ramen", "方便面"},
        {"Eat", "吃"},
        {"Microwave", "微波炉"},
        {"Go eat breakfast", "去吃早饭"},
        {"Microwave the instant ramen", "用微波炉加热方便面"},
        {"Put the instant ramen on the table", "把方便面放桌子上"},
        {"Eat the instant ramen", "吃方便面"},
        {"I'm feeling dizzy right now...", "我现在感觉有点晕..."},
        {"That dream feels so real...", "那个梦感觉太真实了..."},
        {"But anyways, I'm still here, right?", "不过不管怎样，我还在这，对吧？"},
        {"Well it already happened. I should eat first.", "事已至此，先吃饭吧。"},
        {"I remember I left some instant ramen last morning.", "我记得我昨天早上还剩了些方便面。"},
        {"I should go microwave that and eat it.", "我应该去用微波炉热一下然后吃了。"},
        {"Okay, now I should put it on the table and eat it.", "好了，我现在应该放桌子上吃了。"},
        {"Mmm... Very good.", "嗯...真不错。"},
        {"Hello...?", "你...好？"},
        {"Hello. The local police discovered a crashed bike last night. You were acting very suspiciously, and you were right near the bicycle when I heard the crash. We have listed you as a suspect in the death of an important person.", "你好。当地警方在昨晚发现了一辆被撞毁的自行车。你当时的行为非常可疑，并且在我听到撞击声时你就在自行车附近。我们已经将你列为导致一个重要人物死亡的嫌疑人。"},
        {"Oh I'm sorry I didn't do anything I was just going back home...", "哦对不起我啥都没做我就是在回家..."},
        {"You have the right to remain silent. Anything you say can and will be used against you in a court of law. You have the right to talk to a lawyer for advice before we ask you any questions.", "你有权保持沉默，但你所说的每一句话都可以在法庭上作为指控你的不利证据。审问前，你有权与律师谈话。"},
        {"A group of police will arrive after 2 minutes with a warrant to search your house. Any evidence of crimes will be used directly against you. Please be prepared.", "一队警察会在两分钟后带着搜查令到你的房子进行搜查。任何犯罪证据都将直接用于指控你。请你做好准备。"},
        {"Alright...?!", "好吧...？!"},
        {"Looks like I need to hurry...", "看起来我得快一点了..."},
        {"Magazines", "杂志"},
        {"Tool Note", "工具笔记"},
        {"Ultimate Blood Trace Detector is now ON SALE!!! Use it like a flashlight, and discover the unseen mystery!", "超级血迹探测器现已特价发售！！！像手电筒一样使用它，然后探索无法看到奥秘！"},
        {"Not everything is as clean as it looks. Everything leaves a trace. Use the Ultimate Blood Trace Detector to lose your tail!", "不是所有东西都如它所见一样干净。所有东西都会留下痕迹。使用超级血迹探测器去甩掉跟踪你的尾巴！"},
        {"They can see it. The question is, can you? The Untimate Blood Trace Detector can help you see the things they can see.", "他们能看到。问题是，你能吗？超级血迹探测器能帮你看到他们能看到的东西。"},
        {"Flashlight", "手电筒"},
        {"Press [F] to use flashlight", "按 [F] 用手电筒"},
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
        if (atEndingScreen) gameState = 0;
        
        if (!isExecutingTriggers && triggers.Count > 0)
        {
            isExecutingTriggers = true;
            gameState = 0;
            promptText.enabled = false;
            taskText.enabled = false;
            focus.SetActive(false);
            StartCoroutine(ExecuteTriggers());
        }

        if (Input.GetKeyDown(KeyCode.Escape) && !isExecutingTriggers && !atEndingScreen)
        {
            if (atPausedScreen)
            {
                atPausedScreen = false;
                promptText.enabled = true;
                taskText.enabled = true;
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
                taskText.enabled = false;
                focus.SetActive(false);
                Cursor.lockState = CursorLockMode.None;
                Cursor.visible = true;
                gameState = 0;
                pausedScreen.SetActive(true);
            }
        }

    }

    public void DisplayEnding(string t, string c)
    {
        StartCoroutine(Ending(t, c));
    }

    public void LoadScene(string s, float t)
    {
        if (!isLoadingScene)
        {
            isLoadingScene = true;
            StartCoroutine(LoadSceneCoroutine(s, t));
        }
    }

    public void LoadScene(string s)
    {
        if (s == "MainMenu") LoadScene(s, 2.0f);
        else LoadScene(s, 0);
    }

    public void PlayMusic(AudioClip ac)
    {
        musicPlayer.clip = ac;
        musicPlayer.Play();
    }

    public void StopMusic()
    {
        musicPlayer.Stop();
    }

    public void SetMusicVolume(float f)
    {
        musicPlayer.volume = f / 100.0f;
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

    public void RemoveItem(string s)
    {
        inventory.Remove(s);
    }

    public void AddTrigger(string s)
    {
        triggers.Add(s);
    }

    public void AddTask(string s)
    {
        tasks.Add(s);
        UpdateTask();
    }

    public void ClearTasks()
    {
        tasks.Clear();
        UpdateTask();
    }

    public void RemoveTask(string s)
    {
        tasks.Remove(s);
        UpdateTask();
    }

    public void UpdateTask()
    {
        string s = "";
        foreach (string task in tasks)
        {
            s += "- " + Translate(task) + "\n";
        }
        taskText.text = s;
    }

    public void SetPrompt(string s)
    {
        SetPrompt(s, false);
    }

    public void SetPrompt(string s, bool b)
    {
        promptText.text = Translate(s);
        promptText.color = Color.white;
        StopCoroutine("FlashPrompt");
        if (b && s.Length > 0) StartCoroutine("FlashPrompt");
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

    private IEnumerator FlashPrompt()
    {
        float t = 0;
        float speed = 4.0f;
        while(true)
        {
            promptText.color = Color.Lerp(Color.clear, Color.white, (Mathf.Cos(t * speed) + 1) / 2 * 0.75f + 0.25f);
            t += Time.deltaTime;
            yield return null;
        }
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
            else if (key == "rotateplayerto")
            {
                player.SetRotation(float.Parse(s[1]), float.Parse(s[2]));
            }
            else if (key == "canrun")
            {
                if (s[1].ToLower() == "1") player.CanRun(true);
                else player.CanRun(false);
            }
            else if (key == "prompt")
            {
                SetPrompt(s[1]);
            }
            else if (key == "flashprompt")
            {
                SetPrompt(s[1], true);
            }
            else if (key == "flashdialogue")
            {
                gameState = 1;
                yield return StartCoroutine(DisplayDialogue(s[1], s[2], float.Parse(s[3])));
                gameState = 0;
            }
            else if (key == "task")
            {
                AddTask(s[1]);
            }
            else if (key == "cleartasks")
            {
                ClearTasks();
            }
            else if (key == "wait")
            {
                yield return new WaitForSeconds(float.Parse(s[1]));
            }
            else if (key == "loadscene")
            {
                if (s.Length == 1) StartCoroutine(LoadSceneCoroutine(s[1], 0));
                else StartCoroutine(LoadSceneCoroutine(s[1], float.Parse(s[2])));
            }
            else if (key == "ending")
            {
                yield return StartCoroutine(Ending(s[1], s[2]));
            }
            else
            {
                Debug.LogError("Trigger Not Found: " + trig);
            }
        }
        isExecutingTriggers = false;
        gameState = 1;
        promptText.enabled = true;
        taskText.enabled = true;
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
        float t = 0, gap = 0.02f;
        if (PlayerPrefs.GetString("Language", "English") == "Chinese") gap = 0.04f;
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
    private IEnumerator Ending(string title, string content)
    {
        atEndingScreen = true;
        effectsPlayer.clip = writtingEffect;
        title = Translate(title);
        content = Translate(content);
        endingTitle.text = "";
        endingText.text = "";
        endingReturnMenu.SetActive(false);
        endingScreen.SetActive(true);
        float t = 0;
        endScreen.color = Color.clear;
        while (t < 4.0f)
        {
            yield return null;
            t += Time.deltaTime;
            endScreen.color = Color.Lerp(Color.clear, Color.black, t / 4.0f);
        }
        endScreen.color = Color.black;
        yield return new WaitForSeconds(2.0f);
        effectsPlayer.Play();

        t = 0;
        int idx = 0;
        float gap = 0.02f;
        if (PlayerPrefs.GetString("Language", "English") == "Chinese") gap = 0.04f;
        while (idx < title.Length)
        {
            t += Time.deltaTime;
            if (t >= gap)
            {
                t -= gap;
                endingTitle.text += title[idx];
                idx++;
            }
            if (Input.GetMouseButtonDown(0))
            {
                endingTitle.text = title;
                break;
            }
            yield return null;
        }

        effectsPlayer.Stop();
        yield return new WaitForSeconds(2.0f);
        effectsPlayer.Play();

        idx = 0;
        t = 0;
        while (idx < content.Length)
        {
            t += Time.deltaTime;
            if (t >= gap)
            {
                t -= gap;
                endingText.text += content[idx];
                idx++;
            }
            if (Input.GetMouseButtonDown(0))
            {
                endingText.text = content;
                break;
            }
            yield return null;
        }

        effectsPlayer.Stop();
        yield return new WaitForSeconds(2.0f);
        endingReturnMenu.SetActive(true);
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
    }
}
