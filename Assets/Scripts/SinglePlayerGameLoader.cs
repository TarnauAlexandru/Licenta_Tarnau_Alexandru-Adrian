using System;
using System.Collections.Generic;
using UnityEngine;

public class SinglePlayerGameLoader : MonoBehaviour
{
    public List<GameObject> BoxerPlayerPrefabs; 
    public List<GameObject> BoxerCPUPrefabs;    
    public Transform playerSpawnPoint;
    public Transform cpuSpawnPoint;
    [SerializeField] private HUDControllerMicroBar hud;

    void Start()
    {
        int playerIndex = MenuManager.playerIndex;
        int cpuIndex = MenuManager.cpuIndex;

        GameObject player = null;
        if (playerIndex > 0 && playerIndex < BoxerPlayerPrefabs.Count)
        {
            player = Instantiate(BoxerPlayerPrefabs[playerIndex], playerSpawnPoint.position, playerSpawnPoint.rotation);
        }
        else
        {
            Debug.LogWarning("Player index invalid!");
        }

        GameObject cpu = null;

        if (cpuIndex > 0 && cpuIndex < BoxerCPUPrefabs.Count)
        {
            cpu = Instantiate(BoxerCPUPrefabs[cpuIndex], cpuSpawnPoint.position, cpuSpawnPoint.rotation);

            var aiCtrl = cpu.GetComponent<AIPlayerController>();
            if (aiCtrl != null && player != null)
                aiCtrl.targetPlayer = player.transform;
        }
        else
        {
            Debug.LogWarning("CPU index invalid!");
        }

        var p1 = player ? player.GetComponent<PlayerBaseNou>() : null;
        var p2 = cpu ? cpu.GetComponent<PlayerBaseNou>() : null;
        hud.Bind(p1, p2);

        p2.opponent = p1; 

        if (p1 == null) p1.SetSpawnIndex(playerIndex);
        if (p2 == null) p2.SetSpawnIndex(cpuIndex);

        
    }
}
