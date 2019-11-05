using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RoomCenter : MonoBehaviour
{
    public bool openWhenEnemiesCleared;

    public List<GameObject> enemies = new List<GameObject>();

    public Room _myRoom;

    void Start()
    {
        //  If we want to open this room center's room only when all enemies are cleared,
        //  we have to make sure our assigned room's doors act accordingly.
        if (openWhenEnemiesCleared)
        {
            _myRoom.closeWhenEntered = true;
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (_myRoom.IsActiveRoom() && enemies.Count > 0)
        {
            for (int i = 0; i < enemies.Count; i++)
            {
                if (enemies[i] == null)
                {
                    enemies.RemoveAt(i);
                    i--;
                }
            }
        }

        if (openWhenEnemiesCleared && enemies.Count == 0)
        {
            _myRoom.OpenDoors();
        }
    }
}
