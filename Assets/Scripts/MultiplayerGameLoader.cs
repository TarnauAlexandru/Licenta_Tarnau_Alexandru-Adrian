using System.Collections.Generic;
using UnityEngine;

public class MultiplayerGameLoader : MonoBehaviour
{
    public List<GameObject> BoxerPlayerPrefabs;
    public Transform player1SpawnPoint;
    public Transform player2SpawnPoint;
    [SerializeField] private HUDControllerMicroBar hud;

    void Start()
    {
        int p1Index = MenuManager.player1Index;
        int p2Index = MenuManager.player2Index;
        GameObject player1 = null;
        GameObject player2 = null;

        if (p1Index > 0 && p1Index < BoxerPlayerPrefabs.Count)
            player1 = Instantiate(BoxerPlayerPrefabs[p1Index], player1SpawnPoint.position, player1SpawnPoint.rotation);
        else
            Debug.LogWarning("Player 1 index invalid!");
        if (p2Index > 0 && p2Index < BoxerPlayerPrefabs.Count)
            player2 = Instantiate(BoxerPlayerPrefabs[p2Index], player2SpawnPoint.position, player2SpawnPoint.rotation);
        else
            Debug.LogWarning("Player 2 index invalid!");

        var p1 = player1 ? player1.GetComponent<PlayerBaseNou>() : null;
        var p2 = player2 ? player2.GetComponent<PlayerBaseNou>() : null;

        if (p1 == null) p1.SetSpawnIndex(p1Index);
        if (p2 == null) p2.SetSpawnIndex(p2Index);
        hud.Bind(p1, p2);
    }
}
