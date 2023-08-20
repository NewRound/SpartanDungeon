using System.Numerics;
using System.Xml.Linq;
using System.Threading;

namespace SpartanDungeon
{
    // 무기, 방어구, 악세사리 
    public enum EquipType
    {
        WEAPON,
        ARMOR,
        ACCESSORY,
    }

    internal class Program
    {
        // 대대적인 수정절차.
        // 1. SceneManager를 만들어서 화면의 출력 관리.
        // 2. 게임 끝내기 만들기.
        // 3. 상점 만들기.
        // 4. 장착 개선.
        // 5. 던전 만들기.
        // 6. 개임 저장하기.

        public static SceneManager sceneManager = new SceneManager();
        static void Main(string[] args)
        {
            int inputKey = 0;
            while (inputKey != 159)
            {
                switch (inputKey)
                {
                    case 0:
                        inputKey = sceneManager.DisplayGameIntro();
                        if(inputKey == 0) { inputKey = 159; }
                        break;
                    case 1:
                        inputKey = sceneManager.DisplayMyInfo();
                        break;
                    case 2:
                        inputKey = sceneManager.DisplayInventory();
                        break;
                    case 3:
                        inputKey = sceneManager.EquipManager();
                        break;
                }
            }
        }
    }


    public class Character
    {
        public string Name { get; }
        public string Job { get; }
        public int Level { get; }
        public int Atk { get; }
        public int Def { get; }
        public int Hp { get; }
        public int Gold { get; }
        public List<Item> hasItems = new List<Item>();

        public Character(string name, string job, int level, int atk, int def, int hp, int gold)
        {
            Name = name;
            Job = job;
            Level = level;
            Atk = atk;
            Def = def;
            Hp = hp;
            Gold = gold;
        }
    }
    public class Item
    {
        public bool isEquiped { get; set; }
        public string Name { get; }
        public int Atk { get; }
        public int Def { get; }
        public string explanation { get; }
        public EquipType equipType { get; }
        public int itemValue { get; }

        public Item(string name, int atk, int def, string explanation, EquipType equipType, int itemValue)
        {
            isEquiped = false;
            Name = name;
            Atk = atk;
            Def = def;
            this.explanation = explanation;
            this.equipType = equipType;
            this.itemValue = itemValue;
        }
    }

    public class SceneManager
    {
        public Character player;

        public SceneManager()
        {
            // 캐릭터 정보 세팅
            player = new Character("Chad", "전사", 1, 10, 5, 100, 1500);

            // 아이템 정보 세팅
            player.hasItems.Add(new Item("무쇠갑옷", 0, 5, "무쇠로 만들어져 튼튼한 갑옷입니다.", EquipType.ARMOR, 2000));
            player.hasItems.Add(new Item("낡은 검", 2, 0, "쉽게 볼 수 있는 낡은 검 입니다.", EquipType.WEAPON, 600));
        }

        public int DisplayGameIntro()
        {
            Console.Clear();

            Console.WriteLine("스파르타 마을에 오신 여러분 환영합니다.");
            Console.WriteLine("이곳에서 전전으로 들어가기 전 활동을 할 수 있습니다.");
            Console.WriteLine();
            Console.WriteLine("1. 상태보기");
            Console.WriteLine("2. 인벤토리");
            Console.WriteLine("0. 끝내기");
            Console.WriteLine();
            Console.WriteLine("원하시는 행동을 입력해주세요.");

            return CheckValidInput(0, 2);
        }

        public int DisplayMyInfo()
        {
            Console.Clear();
            // 변경되는 유저의 정보
            string playerAtk = player.Atk.ToString();
            string playerDef = player.Def.ToString();
            int itemAtk = 0;
            int itemDef = 0;

            // 착용상태 확인 및 정보적용.
            foreach (Item item in player.hasItems)
            {
                if (item.isEquiped)
                {
                    itemAtk += item.Atk;
                    itemDef += item.Def;
                }
            }
            if (itemAtk != 0)
            {
                playerAtk += $" (+{itemAtk})";
            }
            if (itemDef != 0)
            {
                playerDef += $" (+{itemDef})";
            }

            Console.WriteLine("상태보기");
            Console.WriteLine("캐릭터의 정보르 표시합니다.");
            Console.WriteLine();
            Console.WriteLine($"Lv.{player.Level}");
            Console.WriteLine($"{player.Name}({player.Job})");
            Console.WriteLine($"공격력 :{playerAtk}");
            Console.WriteLine($"방어력 : {playerDef}");
            Console.WriteLine($"체력 : {player.Hp}");
            Console.WriteLine($"Gold : {player.Gold} G");
            Console.WriteLine();
            Console.WriteLine("0. 나가기");

            return CheckValidInput(0, 0);
        }

        public int DisplayInventory()
        {
            Console.Clear();

            Console.WriteLine("인벤토리");
            Console.WriteLine("보유 중인 아이템을 관리할 수 있습니다.");
            Console.WriteLine();
            Console.WriteLine("[아이템 목록]");

            if (player.hasItems.Count == 0)
            {
                Console.WriteLine("보유하신 아이템이 없습니다.");
            }
            else
            {
                for (int i = 0; i < player.hasItems.Count; i++)
                {
                    Console.WriteLine("{0}", ItemTextManager(player.hasItems[i], false));
                }
            }
            Console.WriteLine();

            Console.WriteLine("1. 장착 관리");
            Console.WriteLine("0. 나가기");
            Console.WriteLine();
            Console.WriteLine("원하시는 행동을 입력해주세요.");

            int input = CheckValidInput(0, 1);
            switch (input)
            {
                case 0:
                    break;
                case 1:
                    input = 3;
                    break;

            }

            return input;
        }

        public string ItemTextManager(Item item, bool epuip)
        {
            // - [E]무쇠갑옷      | 방어력 +5 | 무쇠로 만들어져 튼튼한 갑옷입니다.
            string text = "- ";
            // 장착관리시
            if (epuip)
            {
                text += (player.hasItems.IndexOf(item) + 1).ToString() + " ";
            }

            if (item.isEquiped)
            {
                text += "[E]";
            }
            text += item.Name;
            text += "\t | ";

            if (item.Atk != 0)
            {
                text += "공격력 ";
                if (item.Atk > 0)
                {
                    text += "+";
                }
                else
                {
                    text += "-";
                }
                text += item.Atk.ToString();
            }
            if (item.Def != 0)
            {
                text += "방어력 ";
                if (item.Def > 0)
                {
                    text += "+";
                }
                else
                {
                    text += "-";
                }
                text += item.Def.ToString();
            }
            text += "\t | ";

            text += item.explanation;

            return text;
        }

        public int EquipManager()
        {
            Console.Clear();

            Console.WriteLine("인벤토리 - 장착관리");
            Console.WriteLine("보유 중인 아이템을 관리할 수 있습니다.");
            Console.WriteLine();
            Console.WriteLine("[아이템 목록]");

            if (player.hasItems.Count == 0)
            {
                Console.WriteLine("보유하신 아이템이 없습니다.");
            }
            else
            {
                for (int i = 0; i < player.hasItems.Count; i++)
                {
                    Console.WriteLine("{0}", ItemTextManager(player.hasItems[i], true));
                }
            }

            Console.WriteLine();

            Console.WriteLine("0. 나가기");
            Console.WriteLine();
            Console.WriteLine("원하시는 행동을 입력해주세요.");

            int input = CheckValidInput(0, player.hasItems.Count);
            switch (input)
            {
                case 0:
                    return 2;
                default:
                    if (player.hasItems[input - 1].isEquiped)
                        player.hasItems[input - 1].isEquiped = false;
                    else
                        player.hasItems[input - 1].isEquiped = true;
                    break;
            }
            input = EquipManager();
            return input;
        }

        public int CheckValidInput(int min, int max)
        {
            int cursorLeft = Console.CursorLeft;
            int cursorTop = Console.CursorTop;

            while (true)
            {
                Console.SetCursorPosition(cursorLeft, cursorTop);
                Console.WriteLine("      ");
                Console.WriteLine("                     ");
                Console.SetCursorPosition(cursorLeft, cursorTop);

                string input = Console.ReadLine();

                bool parseSuccess = int.TryParse(input, out var ret);
                if (parseSuccess)
                {
                    if (ret >= min && ret <= max)
                        return ret;
                }
                
                Console.WriteLine("잘못된 입력입니다.");
                Thread.Sleep(1000);
                
            }
        }
    }
}