using System.Collections.Generic;
using UnityEngine;

public class GameOver: MonoBehaviour
{
    public List<GameObject> BoxerPlayerPrefabs;
    public Transform player1SpawnPoint;
    public Transform player2SpawnPoint;
    GameObject player1 = null;
    GameObject player2 = null;

    void Start()
    {
        int p1Index = MenuManager.player1Index;
        int p2Index = MenuManager.player2Index;

        if (p1Index > 0 && p1Index < BoxerPlayerPrefabs.Count)
        {
            player1 = Instantiate(BoxerPlayerPrefabs[p1Index], player1SpawnPoint.position, player1SpawnPoint.rotation);
        }
        else { Debug.LogWarning("Player 1 index invalid!"); }

        var p1 = player1 ? player1.GetComponent<MenuBoxerBase>() : null;

        if (p1Index == MultiplayerGameResult.LoserIndex) { p1.SetLoser(); } 
        else if (p1Index == MultiplayerGameResult.WinnerIndex) { p1.SetWinner(); } 


        if (p2Index > 0 && p2Index < BoxerPlayerPrefabs.Count)
        {
            player2 = Instantiate(BoxerPlayerPrefabs[p2Index], player2SpawnPoint.position, player2SpawnPoint.rotation);
        }
        else { Debug.LogWarning("Player 2 index invalid!"); }
        var p2 = player2 ? player2.GetComponent<MenuBoxerBase>() : null;

        if (p2Index == MultiplayerGameResult.LoserIndex) { p2.SetLoser(); }
        else if (p2Index == MultiplayerGameResult.WinnerIndex) { p2.SetWinner(); }
    }
}
