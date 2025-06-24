using System.Collections.Generic;
using UnityEngine;

public class SinglePlayerGameLoader : MonoBehaviour
{
    public List<GameObject> BoxerPlayerPrefabs; // Player1-4 (din Player_Caracters)
    public List<GameObject> BoxerCPUPrefabs;    // CPU1-4 (din CPU_Caracters)
    public Transform playerSpawnPoint;
    public Transform cpuSpawnPoint;

    void Start()
    {
        int playerIndex = MenuManager.playerIndex;
        int cpuIndex = MenuManager.cpuIndex;

        GameObject player = null;
        if (playerIndex > 0 && playerIndex < BoxerPlayerPrefabs.Count)
        {
            player = Instantiate(BoxerPlayerPrefabs[playerIndex], playerSpawnPoint.position, playerSpawnPoint.rotation);
            // Nu mai e nevoie să setezi manual referința la player în controller!
        }
        else
        {
            Debug.LogWarning("Player index invalid!");
        }

        if (cpuIndex > 0 && cpuIndex < BoxerCPUPrefabs.Count)
        {
            GameObject cpu = Instantiate(BoxerCPUPrefabs[cpuIndex], cpuSpawnPoint.position, cpuSpawnPoint.rotation);

            var aiCtrl = cpu.GetComponent<AIPlayerController>();
            if (aiCtrl != null && player != null)
                aiCtrl.targetPlayer = player.transform;
        }
        else
        {
            Debug.LogWarning("CPU index invalid!");
        }
    }
}
