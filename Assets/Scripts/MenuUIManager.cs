using UnityEngine;

public class MenuUIManager : MonoBehaviour
{
    public MenuPlayerPanelUI[] playerPanels; // [0]=Player1, [1]=Player2
    public MenuBoxerBase[] BoxerList;        // 0=Player1, 1=Player2, 2=Player3, 3=Player4

    void Start()
    {
        // Pentru singleplayer: [0]=PlayerPanel, [1]=CPUPanel
        if (playerPanels.Length > 0 && MenuManager.playerIndex > 0)
            playerPanels[0].SetBoxerModel(BoxerList[MenuManager.playerIndex]);
        if (playerPanels.Length > 1 && MenuManager.cpuIndex > 0)
            playerPanels[1].SetBoxerModel(BoxerList[MenuManager.cpuIndex]);

        // Pentru multiplayer: [0]=Player1Panel, [1]=Player2Panel
        if (playerPanels.Length > 0 && MenuManager.player1Index > 0)
            playerPanels[0].SetBoxerModel(BoxerList[MenuManager.player1Index]);
        if (playerPanels.Length > 1 && MenuManager.player2Index > 0)
            playerPanels[1].SetBoxerModel(BoxerList[MenuManager.player2Index]);
    }
}