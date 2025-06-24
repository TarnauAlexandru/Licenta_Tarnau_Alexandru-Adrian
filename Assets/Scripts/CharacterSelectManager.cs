using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class CharacterSelectManager : MonoBehaviour
{
    public List<GameObject> BoxerMenuPrefabs;
    public Transform player1SpawnPoint;
    public Transform player2SpawnPoint;
    public Transform player3SpawnPoint;
    public Transform player4SpawnPoint;

    GameObject player1 = null;
    GameObject player2 = null;
    GameObject player3 = null;
    GameObject player4 = null;

    private void Start()
    {
        player1 = Instantiate(BoxerMenuPrefabs[0], player1SpawnPoint.position, player1SpawnPoint.rotation);

        player2 = Instantiate(BoxerMenuPrefabs[1], player2SpawnPoint.position, player2SpawnPoint.rotation);

        player3 = Instantiate(BoxerMenuPrefabs[2], player3SpawnPoint.position, player3SpawnPoint.rotation);

        player4 = Instantiate(BoxerMenuPrefabs[3], player4SpawnPoint.position, player4SpawnPoint.rotation);
    }


    // Funcții pentru butoane, păstrează-le pentru fiecare boxer
    public void ConfirmBoxerSelection_Player1()
    {
        SetPlayerIndex(1);
    }
    public void ConfirmBoxerSelection_Player2()
    {
        SetPlayerIndex(2);
    }
    public void ConfirmBoxerSelection_Player3()
    {
        SetPlayerIndex(3);
    }
    public void ConfirmBoxerSelection_Player4()
    {
        SetPlayerIndex(4);
    }

    private void SetPlayerIndex(int index)
    {
        switch (MenuManager.selectingSlot)
        {
            case "Player":
                MenuManager.playerIndex = index;
                break;
            case "CPU":
                MenuManager.cpuIndex = index;
                break;
            case "Player1":
                MenuManager.player1Index = index;
                break;
            case "Player2":
                MenuManager.player2Index = index;
                break;
        }
        SceneManager.LoadScene(MenuManager.returnMenu);
    }
}