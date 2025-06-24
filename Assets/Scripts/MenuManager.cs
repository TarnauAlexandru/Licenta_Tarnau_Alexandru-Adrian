using System.Diagnostics;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MenuManager : MonoBehaviour
{
    // Static ca să reții selecția la întoarcerea între scene
    public static int player1Index = 0;
    public static int player2Index = 0;
    public static int selectingPlayer = 1; // cine selectează acum
    public static int playerIndex = 0; // index boxer Player
    public static int cpuIndex = 0;    // index boxer CPU
    public static string selectingSlot = ""; // "Player" sau "CPU"
    public static string returnMenu = "";    // "SinglePlayer_Menu"

    // Navigare
    public void GoToCharacterSelect(int player)
    {
        selectingPlayer = player; // Ex: 1 sau 2
        SceneManager.LoadScene("CharacterSelect");
    }

    public virtual void SelectPlayer(int player, int boxerIndex)
    {
        if (player == 1)
            player1Index = boxerIndex;
        else if (player == 2)
            player2Index = boxerIndex;
    }


    // Setare boxer selectat (chemată din CharacterSelectScene)
    public static void SetPlayerSelection(int boxerIndex)
    {
        if (selectingPlayer == 1)
            player1Index = boxerIndex;
        else if (selectingPlayer == 2)
            player2Index = boxerIndex;
    }

    public void SelectCurrentBoxer(string slot, int selectedBoxerIndex)
    {
        if (slot == "Player")
            MenuManager.playerIndex = selectedBoxerIndex;
        else if (slot == "CPU")
            MenuManager.cpuIndex = selectedBoxerIndex;

        MenuManager.selectingSlot = slot;
        UnityEngine.SceneManagement.SceneManager.LoadScene(MenuManager.returnMenu);
    }

    /*public void UpdateUIBasedOnSelection()
    {
        if (MenuManager.selectingSlot == "Player" && MenuManager.playerIndex != -1)
            playerPanelUI.UpdateStats(listaBoxeri[MenuManager.playerIndex]);
        else if (MenuManager.selectingSlot == "CPU" && MenuManager.cpuIndex != -1)
            cpuPanelUI.UpdateStats(listaBoxeri[MenuManager.cpuIndex]);
    }*/

    public void GoToMainMenu() { SceneManager.LoadScene("MainMenu"); } 
    public void GoToSinglePlayerMenu() { SceneManager.LoadScene("SinglePlayer_Menu"); }
    public void GoToMultiPlayerMenu() { SceneManager.LoadScene("MultiPlayer_Menu"); }
    public void StartGame() { SceneManager.LoadScene("GameScene"); }
    public void ExitGame() { UnityEngine.Application.Quit(); }

    public void GoToChoosePlayer(string menu, string slot)
    {
        MenuManager.returnMenu = menu; // "SinglePlayer_Menu" sau "MultiPlayer_Menu"
        MenuManager.selectingSlot = slot; // "Player" sau "CPU"
        SceneManager.LoadScene("ChosePlayer");
    }

    public void GoToChoosePlayer_Player()
    {
        GoToChoosePlayer("SinglePlayer_Menu", "Player");
    }

    public void GoToChoosePlayer_CPU()
    {
        GoToChoosePlayer("SinglePlayer_Menu", "CPU");
    }

    public void GoToChoosePlayer_Player1()
    {
        GoToChoosePlayer("MultiPlayer_Menu", "Player1");
    }

    public void GoToChoosePlayer_Player2()
    {
        GoToChoosePlayer("MultiPlayer_Menu", "Player2");
    }

    public void StartSinglePlayerGame()
    {
        // Verifică dacă ambele caractere au fost selectate
        if (playerIndex != -1 && cpuIndex != -1)
        {
            UnityEngine.SceneManagement.SceneManager.LoadScene("SinglePlayer_Game");
        }
        else
        {
            UnityEngine.Debug.LogWarning("Selectează atât Player cât și CPU înainte de a începe jocul!");
            // Aici poți adăuga și un mesaj UI pentru utilizator, dacă vrei
        }
    }

    public void StartMultiPlayerGame()
    {
        
        if (player1Index != -1 && player2Index != -1)
        {
            UnityEngine.SceneManagement.SceneManager.LoadScene("MultiPlayer_Game");
        }
        else
        {
            UnityEngine.Debug.LogWarning("Selectează atât Player1 cât și Player2 înainte de a începe jocul!");
            // Aici poți adăuga și un mesaj UI pentru utilizator, dacă vrei
        }
    }
}

// Exemplu: ai currentBoxerIndex ca variabilă în CharacterSelectManager
//menuManager.SelectCurrentBoxer(MenuManager.selectingSlot, currentBoxerIndex);
