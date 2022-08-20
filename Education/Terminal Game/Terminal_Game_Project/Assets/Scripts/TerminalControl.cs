using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TerminalControl : MonoBehaviour {
    enum Screen {
        MainMenu, 
        Password, 
        Win
    };
    private Screen currentScreen = Screen.MainMenu;

    private int level;

    private string password;
    private string[] LVL_1_PASSWORD = {"level", "key", "book", "pen"};
    private string[] LVL_2_PASSWORD = {"terminal", "ground", "solution"};
    private string[] LVL_3_PASSWORD = {"sunshine", "relationship", "headphones"};


    // Start is called before the first frame update
    void Start() {
        ShowMainMenu();
    }
    void ShowMainMenu() {
        currentScreen = Screen.MainMenu;
        level = 0;
        password = null;

        Terminal.ClearScreen();
        Terminal.WriteLine("Hello Player! Which one terminal do you want to hack now?\n");
        
        Terminal.WriteLine(
            "1 - First lvl\n" + 
            "2 - Second lvl\n" + 
            "3 - Third lvl\n" + 
            "Make your choice!");
    }

    void OnUserInput(string input) {
        if (currentScreen == Screen.MainMenu) {
            RunMainMenu(input);
        } else if (currentScreen == Screen.Password) {
            CheckPassword(input);
        } else if (currentScreen == Screen.Win) {
            if (input == "q") {
                ShowMainMenu();
            }
        }
    }

    void RunMainMenu(string input) {
        if (input == "1" || input == "2" || input == "3") {
            GameStart(int.Parse(input));
        } else if (input == "q") {
            ShowMainMenu();
        } else {
            switch (input) {
                case "007":
                    Terminal.WriteLine("Hello Mr. Bond!");
                    break;
                default:
                    Terminal.WriteLine("Uncorrect choise!");
                break;
            }
        }
    }
    
    void CheckPassword(string input) {
        if (password == input) {
                ShowWinScreen(level);
        } else if (input == "q") {
            ShowMainMenu();
        } else {
            GameStart(level);
        }
    }

    void GameStart(int lvl) {
        level = lvl;
        currentScreen = Screen.Password;
        password = SetPassword(lvl);

        Terminal.ClearScreen();
        Terminal.WriteLine("You have choised " + lvl + " lvl!");
        Terminal.WriteLine("Enter 'q' to back");
        Terminal.WriteLine("Your hint: " + password.Anagram());
    }

    string SetPassword(int lvl) {
        string password = null;

        switch(lvl) {
            case 1:
                password = LVL_1_PASSWORD[Random.Range(0, LVL_1_PASSWORD.Length)];
            break;
            case 2:
                password = LVL_2_PASSWORD[Random.Range(0, LVL_2_PASSWORD.Length)];
            break;
            case 3:
                password = LVL_3_PASSWORD[Random.Range(0, LVL_3_PASSWORD.Length)];
            break;
        }

        return password;
    }

    void ShowWinScreen(int lvl) {
        Terminal.ClearScreen();
        currentScreen = Screen.Win;
        Terminal.WriteLine("You have win!\nLevel " + lvl + " completle!");
        GiveAward(lvl);
    }

    void GiveAward(int lvl) {
         switch(lvl) {
            case 1:
                Terminal.WriteLine("Take your book!");
                Terminal.WriteLine(@"
      ______ ______
    _/      Y      \_
   // ~~ ~~ | ~~ ~  \\
  // ~ ~ ~~ | ~~~ ~~ \\      
 //________.|.________\\  
`----------`-'----------'
                ");
                break;
            case 2:
                Terminal.WriteLine("Take your apple!");
                Terminal.WriteLine(@"
  ,--./,-.
 / #      \
|          |
 \        /    
  `._,._,'
                ");
                break;
            case 3:
                Terminal.WriteLine("Take your airplane!");
                Terminal.WriteLine(@"
  __________________________        
/____________________________\
  __||__||__/.--.\__||__||__
 /__|___|___( >< )___|___|__\
           _/`--`\_
          (/------\)
                ");
                break;
            default:
                Debug.LogError("The level doesn`t exixt!");
                break;
        }
        Terminal.WriteLine("Enter 'q' to back");
    }

    // Update is called once per frame
    void Update() {
        
    }
}
