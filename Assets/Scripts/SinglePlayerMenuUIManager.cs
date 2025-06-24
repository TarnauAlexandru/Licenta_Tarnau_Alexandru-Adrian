using System.Collections.Generic;
using UnityEngine;

public class SinglePlayerMenuUIManager : MonoBehaviour
{
    public List<GameObject> BoxerMenuPrefabs;

    public Transform playerSpawnPoint;
    public Transform cpuSpawnPoint;
    void Start()
    {
        int playerIndex = MenuManager.playerIndex;
        int cpuIndex = MenuManager.cpuIndex;

        GameObject player = null;
        if (playerIndex > 0 && playerIndex < BoxerMenuPrefabs.Count)
        {
            player = Instantiate(BoxerMenuPrefabs[playerIndex], playerSpawnPoint.position, playerSpawnPoint.rotation);
        } 
        else { Debug.LogWarning("Player index invalid!"); }

        if (player != null)
        {
            var anim = player.GetComponent<Animator>();
            Debug.Log($"Animator: {anim}, Controller: {anim?.runtimeAnimatorController}, Avatar: {anim?.avatar}");
        }

        if (cpuIndex > 0 && cpuIndex < BoxerMenuPrefabs.Count)
        {
            GameObject cpu = Instantiate(BoxerMenuPrefabs[cpuIndex], cpuSpawnPoint.position, cpuSpawnPoint.rotation);
        } 
        else { Debug.LogWarning("CPU index invalid!"); }
    }


}

    
