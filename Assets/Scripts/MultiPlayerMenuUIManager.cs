using System.Collections.Generic;
using UnityEngine;

public class MultiPlayerMenuUIManager: MonoBehaviour
{
    public List<GameObject> BoxerPlayerPrefabs;
    public Transform player1SpawnPoint;
    public Transform player2SpawnPoint;

    void Start()
    {
        int p1Index = MenuManager.player1Index;
        int p2Index = MenuManager.player2Index;

        if (p1Index > 0 && p1Index < BoxerPlayerPrefabs.Count)
        {
            Instantiate(BoxerPlayerPrefabs[p1Index], player1SpawnPoint.position, player1SpawnPoint.rotation);
        }
        else { Debug.LogWarning("Player 1 index invalid!"); }

        if (p2Index > 0 && p2Index < BoxerPlayerPrefabs.Count)
        {
            Instantiate(BoxerPlayerPrefabs[p2Index], player2SpawnPoint.position, player2SpawnPoint.rotation);
        }
        else { Debug.LogWarning("Player 2 index invalid!"); }
    }
}
