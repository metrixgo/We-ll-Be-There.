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
    [SerializeField] private AudioClip endingSound;

    public int gameState { get; private set; } = 1;
    private bool isExecutingTriggers = false;
    private bool isLoadingScene = false;
    private bool atPausedScreen = false;
    private bool atEndingScreen = false;
    private Color promptColor = Color.white;
    private Image focusImg;
    private Coroutine curExeTrig;
    private string chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZ" +
        "abcdefghijklmnopqrstuvwxyz" +
        "0123456789" +
        "!@#$%^&*()-_=+[]{};:,.<>?/";

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
        {"Thanks.", "谢谢。"},
        {"Book", "书"},
        {"The door is locked... I knew it.", "门被锁了...我就知道。"},
        {"They must've not noticed me when they locked this classroom...", "他们在锁教室的时候一定没注意到我..."},
        {"I don't think the exit is this way...", "我不记得出口在这边..."},
        {"What was that?!", "刚才那什么玩意？！"},
        {"Damn it, I must get outta here real quick.", "该死的，我得赶紧离开这里。"},
        {"Phew... Feels good breathing fresh air.", "呼...能呼吸到新鲜空气真好。"},
        {"I should get on my bike and get home now.", "我现在得骑车回家了。"},
        {"Almost forgot I need to turn left.", "差点忘了我得左转了。"},
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
        {"ENDING 1/5 - SURRENDER", "结局 1/5：自首"},
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
        {"A group of police will arrive after three minutes with a warrant to search your house. Any evidence of crimes will be used directly against you. Please be prepared.", "一队警察会在三分钟后带着搜查令到你的房子进行搜查。任何犯罪证据都将直接用于指控你。请你做好准备。"},
        {"Alright...?!", "好吧...？!"},
        {"Looks like I need to hurry...", "看起来我得快一点了..."},
        {"Magazines", "杂志"},
        {"Tool Note", "工具笔记"},
        {"Ultimate Blood Trace Detector is now ON SALE!!! Use it like a flashlight, and discover the unseen mystery!", "超级血迹探测器现已特价发售！！！像手电筒一样使用它，然后探索无法看到奥秘！"},
        {"Not everything is as clean as it looks. Everything leaves a trace. Use the Ultimate Blood Trace Detector to lose your tail!", "不是所有东西都如它所见一样干净。所有东西都会留下痕迹。使用超级血迹探测器去甩掉跟踪你的尾巴！"},
        {"They can see it. The question is, can you? The Ultimate Blood Trace Detector can help you see the things they can see.", "他们能看到。问题是，你能吗？超级血迹探测器能帮你看到他们能看到的东西。"},
        {"Flashlight", "手电筒"},
        {"Washing Machine", "洗衣机"},
        {"Press [F] to use flashlight", "按 [F] 用手电筒"},
        {"I see no reason to use a flashlight right now.", "我看不出有什么理由在现在用一个手电筒。"},
        {"I can't clean these up with my hands. I need to get a mop.", "我没法用手收拾这些。我得拿个拖把。"},
        {"I don't want to hold two items at the same time...", "我不想同时拿着两样东西..."},
        {"I need a shovel to cover this.", "我需要铲子才能把这个覆盖了。"},
        {"I can clean the mop here.", "我可以在这里清洗拖把。"},
        {"Mop?", "拖把？"},
        {"Shovel?", "铲子？"},
        {"Clothes?", "衣服？"},
        {"Backyard?", "后院？"},
        {"Blood?", "血？"},
        {"Mop bucket?", "拖把桶？"},
        {"Sink", "水池"},
        {"It's still washing...", "它还在洗..."},
        {"It's done. I think I'll just leave the clothes in there.", "它结束了。我觉得我把衣服放里面就行了。"},
        {"Mop Bucket", "拖把桶"},
        {"I can wash my mop bucket here. I believe it's somewhere on the first floor.", "我可以在这里洗我的拖把桶。我相信它在一层的某个地方。"},
        {"I got him. This thing can finally end... How did you know it was him?", "我搞定他了。这件事终于可以结束了...你怎么知道是他的？"},
        {"We found his mop on the second floor with blood traces on it. We believe it was used to clean up the crime scene.", "我们在二楼发现了他沾有血迹的拖把。我们相信那是用来清理案发现场的。"},
        {"We found his shovel in the garage covered with plastic fibers and blood. We believe it was used to hide the body.", "我们在车库里发现了他沾有塑料纤维和血迹的铲子。我们相信那是用来掩盖尸体的。"},
        {"We found his clothes with blood traces on them. We believe the blood came from the crime scene.", "我们发现了他带有血迹的衣服。我们相信那血迹是来自案发现场的。"},
        {"We found a suspicious spot in the backyard. We dug down and saw the actual body.", "我们在后院发现了一处可疑地点。我们往下挖开以后看到了尸体。"},
        {"We used ultraviolet lights to find blood traces on the ground. We believe the blood was from the crime scene that he forgot to clean up.", "我们用紫外线灯光发现了地上的血迹。我们相信那是他忘记清理的从案发现场带来的血迹。"},
        {"We found a mop bucket on the first floor storage closet with blood in it. We believe it was used to clean up blood on items.", "我们在一楼储物间里发现了一个里面带有血迹的拖把桶。我们相信那是用于清理带有血迹的物品的。"},
        {"Nice. Now shall we go eat lunch?", "不错。现在咱们去吃午饭吧？"},
        {"ENDING 2/5 - EXPOSED", "结局 2/5：暴露"},
        {"You thought you took care of everything, but the police still managed to spot the trace. Now, you could only watch everything happen to you.", "你以为你把一切都搞定了，但是警察还是查出了蛛丝马迹。现在，你只能看着一切事情对你发生。"},
        {"Stop where you are!", "别动！"},
        {"Policeman", "男警"},
        {"We are now arresting you for committing first-degree murder!", "我们现在以故意杀人罪逮捕你！"},
        {"Um... Sorry...", "呃...抱歉..."},
        {"What?", "什么？"},
        {"I don't think we have any direct evidence to arrest him...", "我不认为我们有任何直接证据来逮捕他..."},
        {"What are you saying?! We knew it was him!!", "你在说什么？！我们早就知道是他了！！"},
        {"No... We couldn't find anything...", "不...我们什么都没找到..."},
        {"I... I saw him last night!!! He... he was... he was definitely cleaning up the crime scene!!!", "我...我昨晚看到他了！！！他...他当时...他当时绝对是在清理犯罪现场！！！"},
        {"We cannot convict him just because you saw him. We have to leave now, since we didn't find anything.", "我们不能仅凭你看到了他就定罪。我们现在必须离开，因为我们什么都没找到。"},
        {"NO NO NO do the search again!!!", "不 不 不 再搜查一次！！！"},
        {"If we stay here, we will be the ones committing crimes.", "如果我们继续呆在这，我们就会成犯罪的。"},
        {"Fine...", "行..."},
        {"You might have got away this time. But... hehehe... you won't get away next time... and they will be here to FIND YOU... heheheheehahahahah", "你可能这次侥幸逃过一劫了。但是...嘿嘿嘿...下一次就不一定了...并且他们会过来找你...呵呵呵呵哈哈哈哈哈"},
        {"What are you doing?! We need to leave!", "你在干什么？我们得走了！"},
        {"Phew... Looks like I got away with it.", "呼...看来一切都归于平静了。"},
        {"I can't believe they just broke my glass...", "我无法相信他们刚刚把我玻璃给打碎了..."},
        {"Glass Door", "玻璃门"},
        {"Where did all the police cars went?!", "那些警车全都去哪了？！"},
        {"Where did they go? I don't see them leaving at all.", "他们去哪了？我完全没看到他们离开。"},
        {"That's so weird. I need to go check all the rooms on the second floor.", "这有点奇怪。我得去检查所有二楼的房间。"},
        {"Check rooms (", "检查房间 （"},
        {")", "）"},
        {"I can't find them...", "我找不到他们..."},
        {"Maybe it's just me hallucinating...", "也许只是我产生幻觉了..."},
        {"Anyways, I should just go and relax on the couch. It has been such a tiring and stressful day.", "事已至此，我还是去沙发上放松一下吧。今天真是又累又让我压力山大。"},
        {"Go relax on the sofa", "去沙发上放松"},
        {"Sofa", "沙发"},
        {"What's wrong with the TV?", "电视出什么问题了？"},
        {"???", "？？？"},
        {"L-E-T-U-S-P-L-A-Y-A-G-A-M-E-?", "让-我-们-玩-个-游-戏-不-？"},
        {"I think I need to get out of here...", "我觉得我得离开这里..."},
        {"Now.", "现在。"},
        {"Control", "遥控器"},
        {"Hole", "洞口"},
        {"Entrance", "入口"},
        {"Rope", "绳子"},
        {"Rock", "石头"},
        {"Crowbar", "撬棍"},
        {"Plank", "木板"},
        {"What... I can't open this door...", "什么...我打不开这扇门..."},
        {"I need a shovel to dig open this.", "我需要铲子才能挖开这里。"},
        {"It seems pretty dangerous to go down here...", "这样下去看起来很危险..."},
        {"It's out of reach...", "我够不到..."},
        {"Box", "盒子"},
        {"Music Box", "音乐盒"},
        {"White Key", "白钥匙"},
        {"Copper Key", "铜钥匙"},
        {"It's locked.", "这被锁上了。"},
        {"It's blocked.", "这被挡住了。"},
        {"I need to use a crowbar to remove these planks.", "我需要一个撬棍把木板撬掉。"},
        {"Once upon a time, there was a little boy who lived with a lovely family.", "很久以前，有一个小男孩在一个幸福的家庭生活。"},
        {"One day, his parents suddenly decided to move away. The little boy was so scared of being left alone.", "有一天，他的父母突然决定离家出走。那个小男孩非常害怕被独自一人留下。"},
        {"He asked his parents for the reason they were moving away, but all his parents said was, \"We promise that when you are able to buy a bike on your own, we will be back home.\" The little boy stopped, and then nodded.", "他问了他的父母为什么要离开，但是他的父母只是回答，“当你能够自己买一辆自行车的时候，我们就保证会回到家。”小男孩停顿了一下，随后点了点头。"},
        {"After his parents left, he started to earn money by doing house chores in the neighborhood. It was tough, but he managed to save a lot of money in his piggy bank. The little boy was so proud of himself!", "父母离开以后，他开始通过在邻居家里做家务来挣钱。这很艰难，但是他设法在自己的存钱罐里存了很多钱。那个小男孩为自己感到非常骄傲！"},
        {"After three months of hard work, he finally had enough money to buy a bicycle. He carried his piggy bank to the bicycle store. \"I'll have a nice and big bicycle with a headlight and all the decorations, please,\" he said to the store manager.", "经过三个月的埋头苦干，他终于攒够了买自行车的钱。他拿着自己的存钱罐去了自行车店。“我要一个前车灯，一大辆上好的自行车，再加上所有的装饰，”他对商店管理员说道。"},
        {"The store manager stared at him. \"It'll cost a tidy bit,\" the manager replied. \"That's understood. And a metal basket on the back of the bicycle, please,\" the little boy commanded with a sense of determination.", "商店管理员瞪着他。“这得花不少钱，”管理员回答道。“知道，另外再来一个铁车筐放到自行车后面，”小男孩用坚定的语气命令着。"},
        {"The little boy exited the store with a brand new bicycle. He rode home happily, humming songs along the way and looking around curiously. He knew his parents must be at home in no time!", "那个小男孩拿着一辆崭新的自行车离开了商店。他开心地骑车回家，路上不断哼着歌，好奇地看着周围的景色。他知道他的父母肯定马上就到家了！"},
        {"Eight years later, the little boy went into high school. He lived in a big, empty house. His parents were still nowhere to be found. He was lonely. Sad. Helpless. He still waited in front of his house every day to see if his parents had come back. Life was rough, but he still lived happily every day. He believed that as long as he worked hard, everything would be fine.", "八年以后，那个小男孩进了高中。他生活在一个空荡荡的大房子里。他的父母还是无处可寻。他很孤独。伤心。无助。他每天仍然在家门前等着去看他的父母有没有回来。生活很艰难，但是他仍然乐观地生活着。他相信只要他努力，一切都会好的。"},
        {"But one day the boy was so exhausted at school and accidentally fell asleep since he was extremely tired illusions of him being at school collecting stupid books to escape overwhelmed his mind and when he managed to get rid of that illusion only to find that he was already outside of school so he decided to go home but he was so tired and distracted so he accidentally crashed into the mayor's son on the ride home at night and killed him and the boy went crazy he did not know what to do so he decided to hide the body the boy managed to hide most of them but there were still traces of evidence left behind that night the boy had a terrible dream and when he woke up he noticed that a group of police will come soon he thought about lots of places and managed to clean up all the traces before the police came but something was not right and the boy looked to be manipulated was strangely lured down to a sewer that should not exist and now the boy is likely still reading books inside his illusion but he still does not know what to do he did not know that <size=20><color=red>the escape code was simply 0000</color></size> and he will likely be hunted down by a killer next so the destiny of the boy is determined and nothing can be changed and his life is ruined because he made a mistake a really stupid mistake that cost his life.", "但是有一天那个男孩在学校太累了然后不小心睡着了因为他实在太困了他在学校收集该死的书去逃离的幻觉侵蚀了他的大脑并且当他设法消除这幻觉的时候只看到自己已经在学校外面了于是他决定回家但是他太困太不清醒了所以他不小心撞死了市长的儿子然后那个男孩发疯了他不知道该做什么所以他决定把尸体藏起来那个男孩设法把大部分都藏起来了但是仍有部分残留证据被遗忘那晚那个男孩做了一个恐怖至极的噩梦而且当他醒来的时候他注意到会有一群警察来所以他想到了很多地方并且在警察赶来之前清理了所有证据但是有什么东西不对劲然后那个男孩好象是被控制似的被诱导到了一个本不该存在的下水道里然后现在那个男孩应该还在自己的幻觉里读书但是他不知道该做什么他不知道该做什么他不知道<size=20><color=red>逃离密码就只不过是0000</color></size>然后他待会很可能会被一个杀手追杀所以他的命运已经被注定了没有什么可以被改变并且它的生活被毁了因为他犯下了一个错误一个非常愚蠢的错误让他付出了人生的代价。"},
        {"\"Oh, sorry everyone. I made a mistake. These texts were added by a strange kid. The story was not like that. The boy bought his bike and went home. His parents were waiting for him. They hugged together. His mom said, 'I knew you could do this! You know, as long as you persist, everything can be solved! We are so proud of you!' Then, the family lived happily ever after. The End. Okay everyone, now go back to your seats. Story time is over.\" \n\"Ms. Bartlett!!! <size=20><color=red>The real escape code is 0419!!! The real escape code is 0419!!!</size></color> I saw it with my eyes!!! The little boy got tricked!!! Ms.-\" \n\"Enough of that, Eric. If you say this nonsense again, I'm going to take away all your stars for this week! Now everyone please be quiet and look at the whiteboard.\"", "“哦，对不起大家，我犯了个错。这些字全都是被一个奇怪的小孩加上去的。故事不是那样的。那个男孩买了他的自行车后回到了家。他的父母已经在等着他了。他们相拥在一起。他的妈妈说道，‘我就知道你可以做到！你懂的，只要你持续坚持，一切都可以被解决！我们为你感到骄傲！’那个家庭之后过上了圆满幸福的生活。完。好了各位，现在回到你的位子上。故事时间结束了。”\n“巴特老师！！！<size=20><color=red>真正的逃离密码是0419！！！真正的逃离密码是0419！！！</size></color>我亲眼看到的！！！那个小男孩被骗了！！！老-”\n“够了，埃里克。如果你再胡说八道，我会把你这周的星星全没收！现在请大家都安静看白板。”"},
        {"Carving", "刻印"},
        {"Keypad", "密码锁"},
        {"You just can't stop looping the music you like. It's such an addiction. Stop the music. Live a better life.", "你根本无法停止循环播放你喜欢的音乐。这简直就是上瘾。停下音乐。享受更好的生活。"},
        {"How about looking behind you?", "再看看你的后面呢？"},
        {"I don't know the code yet.", "我还不知道密码是什么。"},
        {"Why does this look so much like a ritual...", "为什么这看起来这么像一个仪式..."},
        {"Props", "杂物"},
        {"Screen", "屏幕"},
        {"I think I need to close it... I believe there's a control somewhere.", "我觉得我得把这个关了...我相信某处有个遥控器。"},
        {"Press [A] and [D] to climb", "按 [A] 和 [D] 攀爬"},
        {"Investigations suggested that 90% of deaths are caused by music. Don't play music alone. It will cause panic. Illusions. Maybe death. It affects you even when you can't hear it. So please, never listen to music.", "调查表明90%的死亡都是音乐引起的。不要一个人放音乐。这会导致痛苦。幻觉。也许死亡。就算你听不到音乐的时候它也会影响到你。所以求你了，永远不要听音乐。"},
        {"STOP", "停下来"},
        {"PLEASE", "求你了"},
        {"DON'T RUN", "不要跑"},
        {"YOU'LL REGRET THIS", "你会后悔的"},
        {"NO!", "不！"},
        {"Refrigerator Door", "冰箱门"},
        {"Stove", "灶台"},
        {"There's no way I'm going back down there.", "我不可能再回去了。"},
        {"What... Is... Happening...", "发生...了...什么..."},
        {"I need to get out... I need to get out of here...", "我得离开...我得离开这里..."},
        {"It's locked?! I need to find the key NOW.", "被锁住了？！我得找到钥匙。现在。"},
        {"Fuck... The key won't fit... It's over...", "该死的...钥匙塞不进去...完了..."},
        {"Cabinet Door", "柜门"},
        {"Toilet Lid", "马桶盖"},
        {"Key", "钥匙"},
        {"Drawer", "抽屉"},
        {"There's nothing inside this trash can.", "这垃圾桶里面什么都没有。"},
        {"Trash Can", "垃圾桶"},
        {"Shower", "淋浴"},
        {"you've killed me.", "你杀了我。"},
        {"why didn't you confess.", "为什么你不承认。"},
        {"stop running. it's useless.", "不要跑了。没用的。"},
        {"you can't get away.", "你逃不了的。"},
        {"it's about time.", "是时候了。"},
        {"Dining Table", "餐桌"},
        {"Why is there food??? It seems like someone is at my house.", "为什么会有食物？？？看起来有人在我家。"},
        {"I think I hear someone showering upstairs. I think I need to check it out.", "我好像听到有人在楼上洗澡。我觉得我得去检查一下。"},
        {"It seems like the phone line has been cut...", "好像电话线被剪断了..."},
        {"Radio", "广播"},
        {"Mom", "妈妈"},
        {"M...Mom?!", "妈...妈妈？！"},
        {"What took you so long up here?", "你在上面这么久干什么呢？"},
        {"Mom! Where were you all these time?!", "妈妈！你这些日子都去哪了？！"},
        {"Son, what are you talking about? You said you wanted to use the bathroom and you've been up here for like thirty minutes.", "不是儿子你说啥呢？你说你想去厕所然后在上面呆了三十分钟。"},
        {"Anyways, please come down and eat dinner with us. I don't want you to leave in the middle and hide in the bathroom with your phone.", "不管怎么样，请下来跟我们吃晚餐。我不想让你中途离开然后带着个手机在厕所藏着。"},
        {"Mom... I... Ok sure! Let's eat together!", "妈妈...我...当然！咱们一起吃吧！"},
        {"Finally you're acting normal. I hope you can keep this up. Don't act like we haven't seen each other for ten years.", "你终于表现正常了。我希望你能保持下去，别跟十年没见过我一样。"},
        {"...and I was so surprised you know, we both didn't see when that happened.", "...然后我当时好惊讶你懂的，我们俩都没看到那什么时候发生的。"},
        {"I suspect someone broke it with a hammer, or else the hole on the glass door wouldn't be so uniform.", "我怀疑是有人用锤子砸的，否则玻璃门上的洞不会这么整齐。"},
        {"Son, do you know who broke the glass door?", "儿子，你知道是谁砸碎了那个玻璃门吗？"},
        {"I... don't know...", "我...不知道..."},
        {"Mom, is there a hole in the backyard?", "妈妈，在后院里有一个洞吗？"},
        {"I do think there are some traces of dirt there. But there should be no holes.", "我的确认为那里有些泥土的痕迹。但是应该没有洞。"},
        {"What are you saying, didn't we already see that? Like the one that's very deep into the ground, with all kinds of...", "你说啥呢，我们不都已经看到了吗？就是那个在地底下很深的，里头有各种..."},
        {"What? What do you mean? Dad!", "什么？你什么意思？爸爸！"},
        {"N... Nothing.", "没...没什么。"},
        {"Haha, got you with a joke, huh? There are no holes, what are you even worrying about.", "哈哈，开个玩笑你就被吓到了，是吧？根本就没有洞，你到底在担心什么。"},
        {"Come on, are you not feeling well right now?", "哎呀，你现在感觉不舒服吗？"},
        {"Let's sing together, how about that?", "咱们一起来唱歌吧，怎么样？"},
        {"Um... he's not feeling good right now. Give him some space. Let's just eat.", "额...他现在不舒服。给他点空间吧。咱们就吃饭。"},
        {"Ok. Yeah. Sure. Let's eat.", "哦。好。当然。咱们吃饭。"},
        {"Dad", "爸爸"},
        {"Mayor", "市长"},
        {"Is it about time?", "是时候了？"},
        {"It is about time.", "是时候了。"},
        {"...speaking of which, do you hear anything?", "...说起这个，你听到什么东西了吗？"},
        {"I don't hear anything.", "我啥也没听到。"},
        {"I'm sorry...", "我很抱歉..."},
        {"It's too late...", "太迟了..."},
        {"Press [xxxxx] to run", "按 [xxxxx] 奔跑"},
        {"What is happening?!", "发生啥事？！"},
        {"......Please......Help......Me............And............", "...求你了...救...我......和......"},
        {"FINAL ENDING 4/5 - We'll Be There.", "最终结局 4/5 - 我们会在。"},
        {"You managed to survive to the end. You went through all the hallucinations. You realized they would be here. You faced your ending calmly. You knew it was going to happen. You've paid back your mistake.", "你设法活到了最后。你经历了所有幻觉。你意识到他们会在。你平静地面对了你的结局。你早已知道这会发生。你已经弥补了你的过错。"},
        {"\"The Supernaturals\" - Ep. 09: Sewer Tense. What's up everyone, the story for today happens inside the sewer. Wooooo really spooky, isn't it?", "《超自然现象》 - Ep. 09：管道惊魂。大家好啊，今天的故事发生在下水道里。哇啊啊非常阴森，对吧？"},
        {"Beside me is Eric, who's gonna talk about some of the, uh, INTERESTING parts down there. Come Eric, say hi to everyone!", "在我旁边的是埃里克，他会说一些管道里面的，呃，有趣的事情。来吧埃里克，跟大家打声招呼！"},
        {"Hi, I'm Eric. With one sentence to summarize today's theme, NEVER trust ANYTHING you see in the sewer, especially if they give you some CLUES that you seemingly need.", "嗨，我是埃里克。用一句话概括今天的主题，永远不要相信任何在下水道里看到的东西，尤其是当他们给你看起来你很需要的线索。"},
        {"I've been down the sewer once, and the *******s tricked me into inputting the wrong escape code. Until then I realized that ************", "我之前就去过一次下水道，然后 ****** 骗我输入了错误的逃离密码。直到那时我才意识到 ******"},
        {"Internet safety tip! Never set your password to something simple like 0000. No one will do that, okay?", "网络安全小贴士！永远不要把你的密码设得太简单，比如0000。没有人会这么做的，好不？"},
        {"Cake! Yum! nom nom nom nom nom nom nom nom nom nom nom nom nom nom nom nom nom nom nom nom nom nom nom nom nom nom nom nom nom", "蛋糕！美味！ nom nom nom nom nom nom nom nom nom nom nom nom nom nom nom nom nom nom nom nom nom nom nom nom nom nom nom nom nom"},
        {"\"Blank\": Time is flying by? It's because you're doing so many things. Now sit down and meditate. Feel the air around your body.", "《空白》：时间飞逝？这是因为你在做太多东西了。现在坐下然后冥想。感受你身体周围的空气。"},
        {"You will see that if you don't move for over 20 seconds, your mind can be teleported to anywhere you want.", "你会发现如果你静止不动超过20秒，你的思想会被传送到你想去的任何地方。"},
        {"My world is a mess. It's like a game where the developer puts whatever they dream about inside the game.", "我的世界简直是一团糟。属于是一个游戏作者梦到什么往游戏里加什么了。"},
    };

    private void Awake()
    {
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
        instance = this;
        musicPlayer.volume = PlayerPrefs.GetFloat("Music", 30.0f) / 100.0f;
        effectsPlayer.volume = PlayerPrefs.GetFloat("Effects", 80.0f) / 100.0f;
        focusImg = focus.GetComponent<Image>();
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
            curExeTrig = StartCoroutine(ExecuteTriggers());
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

    public bool IsExecutingTriggers()
    {
        return isExecutingTriggers;
    }

    public bool AtPausedScreen()
    {
        return atPausedScreen;
    }

    public void DisplayEnding(string t, string c)
    {
        StartCoroutine(Ending(t, c));
    }

    public void ClearTriggers()
    {
        if (curExeTrig != null) StopCoroutine(curExeTrig);
        triggers.Clear();
        isExecutingTriggers = false;
        gameState = 1;
        promptText.enabled = true;
        taskText.enabled = true;
        focus.SetActive(true);
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

    public void StopEffect()
    {
        effectsPlayer.Stop();
    }

    public int ItemCount(string s)
    {
        int cnt = 0;
        foreach (string item in inventory)
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
        if (tasks.Contains(s)) return;
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
        promptText.color = promptColor;
        StopCoroutine("FlashPrompt");
        if (b && s.Length > 0) StartCoroutine("FlashPrompt");
    }

    public void SetPromptColor(Color c)
    {
        promptColor = c;
    }

    public void SetFocusColor(Color c)
    {
        focusImg.color = c;
    }

    private Color ParseColor(string colorString)
    {
        if (ColorUtility.TryParseHtmlString(colorString, out Color color)) return color;
        else return Color.black;
    }

    public string Translate(string s)
    {
        if (PlayerPrefs.GetString("Language", "English") == "English") return s;
        if (translations.ContainsKey(s)) return translations[s];
        return s;
    }

    private IEnumerator FlashPrompt()
    {
        float t = 0;
        float speed = 4.0f;
        while (true)
        {
            promptText.color = Color.Lerp(Color.clear, promptColor, (Mathf.Cos(t * speed) + 1) / 2 * 0.75f + 0.25f);
            t += Time.deltaTime;
            yield return null;
        }
    }

    private IEnumerator LoadSceneCoroutine(string s, float t)
    {
        if (s != "MainMenu" && !s.Contains("Ending")) PlayerPrefs.SetString("Save", s);
        AddTrigger("changescreen;#00000000;#000000FF;" + t);
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
                if (s.Length > 3)
                {
                    promptText.enabled = true;
                    taskText.enabled = true;
                    focus.SetActive(true);
                    gameState = 1;
                }
                yield return StartCoroutine(DisplayDialogue(s[1], s[2]));
                if (s.Length > 3)
                {
                    promptText.enabled = false;
                    taskText.enabled = false;
                    focus.SetActive(false);
                    gameState = 0;
                }
            }
            else if (key == "changescreen")
            {
                yield return StartCoroutine(ChangeScreen(ParseColor(s[1]), ParseColor(s[2]), float.Parse(s[3])));
            }
            else if (key == "flashscreen")
            {
                promptText.enabled = true;
                taskText.enabled = true;
                focus.SetActive(true);
                gameState = 1;
                yield return StartCoroutine(ChangeScreen(ParseColor(s[1]), ParseColor(s[2]), float.Parse(s[3])));
                promptText.enabled = false;
                taskText.enabled = false;
                focus.SetActive(false);
                gameState = 0;
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
                if (s[1] == "1") player.CanRun(true);
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
                promptText.enabled = true;
                taskText.enabled = true;
                focus.SetActive(true);
                gameState = 1;
                yield return StartCoroutine(DisplayDialogue(s[1], s[2], float.Parse(s[3])));
                promptText.enabled = false;
                taskText.enabled = false;
                focus.SetActive(false);
                gameState = 0;
            }
            else if (key == "flashwait")
            {
                promptText.enabled = true;
                taskText.enabled = true;
                focus.SetActive(true);
                gameState = 1;
                yield return new WaitForSeconds(float.Parse(s[1]));
                promptText.enabled = false;
                taskText.enabled = false;
                focus.SetActive(false);
                gameState = 0;
            }
            else if (key == "chaosdialogue")
            {
                string temp = "";
                for (int i = 1; i <= 30; i++) temp += chars[Random.Range(0, chars.Length)];
                yield return StartCoroutine(DisplayDialogue(s[1], temp));
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
            else if (key == "waitesc")
            {
                yield return new WaitUntil(() => Input.GetKeyDown(KeyCode.Escape));
                yield return new WaitForEndOfFrame();
            }
            else if (key == "loadscene")
            {
                if (s.Length == 2) StartCoroutine(LoadSceneCoroutine(s[1], 0));
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
        yield return new WaitForEndOfFrame();
        yield return new WaitForEndOfFrame();
        if (triggers.Count == 0)
        {
            promptText.enabled = true;
            taskText.enabled = true;
            focus.SetActive(true);
        }
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
            if ((Input.GetMouseButtonDown(0) || Input.GetKeyDown(KeyCode.Space) || Input.GetKeyDown(KeyCode.Return)) && length < 0)
            {
                dialogueText.text = content;
                break;
            }
            yield return null;
        }
        yield return new WaitForSeconds(0.05f);
        effectsPlayer.Stop();
        if (length >= 0) yield return new WaitForSeconds(length);
        else yield return new WaitUntil(() => Input.GetMouseButtonDown(0) || Input.GetKeyDown(KeyCode.Space) || Input.GetKeyDown(KeyCode.Return));
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
        screen.color = Color.clear;
        effectsPlayer.Play();

        float t = 0;
        int idx = 0;
        float gap = 0.02f;
        if (PlayerPrefs.GetString("Language", "English") == "Chinese") gap = 0.04f;
        while (idx < content.Length)
        {
            t += Time.deltaTime;
            if (t >= gap)
            {
                t -= gap;
                endingText.text += content[idx];
                idx++;
            }
            if (Input.GetMouseButtonDown(0) || Input.GetKeyDown(KeyCode.Space) || Input.GetKeyDown(KeyCode.Return))
            {
                endingText.text = content;
                break;
            }
            yield return null;
        }
        endingText.text = content;
        yield return new WaitForSeconds(0.05f);
        effectsPlayer.Stop();
        yield return new WaitUntil(() => Input.GetMouseButtonDown(0) || Input.GetKeyDown(KeyCode.Space) || Input.GetKeyDown(KeyCode.Return));
        yield return new WaitForSeconds(0.1f);
        effectsPlayer.Play();

        t = 0;
        idx = content.Length;
        gap = 0.004f;
        if (PlayerPrefs.GetString("Language", "English") == "Chinese") gap = 0.008f;
        while (idx >= 0)
        {
            t += Time.deltaTime;
            if (t >= gap)
            {
                t -= gap;
                endingText.text = endingText.text.Substring(0, idx);
                idx--;
            }
            if (Input.GetMouseButtonDown(0) || Input.GetKeyDown(KeyCode.Space) || Input.GetKeyDown(KeyCode.Return))
            {
                endingText.text = "";
                break;
            }
            yield return null;
        }
        endingText.text = "";
        effectsPlayer.Stop();
        effectsPlayer.clip = endingSound;
        yield return new WaitForSeconds(2.0f);

        endingTitle.text = title;
        effectsPlayer.Play();
        yield return new WaitForSeconds(1.0f);
        endingReturnMenu.SetActive(true);
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
    }
}
