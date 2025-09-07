using UnityEngine;
using System.Collections.Generic;
public class TestGameLoader : MonoBehaviour
{
   
    public List<GameObject> BoxerPlayerPrefabs;
    public Transform playerSpawnPoint;
    public Transform opponentSpawnPoint;


    public PlayerBaseTest p1; // Make this a field
    public PlayerBaseTest op; // Make this a field

    void Start()
    {
        GameObject player = null;
        player = Instantiate(BoxerPlayerPrefabs[0], playerSpawnPoint.position, playerSpawnPoint.rotation);

        p1 = player ? player.GetComponent<PlayerBaseTest>() : null;

        GameObject oponent = null;
        oponent = Instantiate(BoxerPlayerPrefabs[1], opponentSpawnPoint.position, opponentSpawnPoint.rotation);
        op = oponent ? oponent.GetComponent<PlayerBaseTest>() : null;

        if (p1 != null)
        p1.attacker = op;
        op.attacker = p1;
    }

}

