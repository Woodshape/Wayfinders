using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelGenerator : MonoBehaviour
{
    public enum Direction
    {
        Up,
        Right,
        Down,
        Left
    }

    public Direction direction;

    public GameObject layoutRoom;

    public Color startColor, endColor;

    public int numOfRooms;

    public Transform generationPoint;

    public float xOff = 18f, yOff = 10f;

    public LayerMask roomLayerMask;

    private GameObject endRoom;

    private List<GameObject> layoutRoomGOs = new List<GameObject>();
    private List<GameObject> generatedRoomOutlines = new List<GameObject>();

    public LayoutRoomPrefabs layoutRooms;

    // Start is called before the first frame update
    void Start()
    {
        //  Instantiate the starting room and color it.
        Instantiate(layoutRoom, generationPoint.transform.position, generationPoint.transform.rotation).GetComponent<SpriteRenderer>().color = startColor;

        for (int i = 0; i < numOfRooms; i++)
        {

            //  Next, pick a random cardinal direction.
            direction = (Direction)Random.Range(0, 4);

            //  Move our generation point according to the random direction.
            MoveGenerationPoint(direction);

            //  FIXME: implement better level generation algorithm.
            //  For now, we just keep moving our room generation point in the same direction
            //  until we find a valid generation point 
            //  (i.e. one that is not overlapping with other room colliders). 
            while (CheckForOverlappingRooms())
            {
                MoveGenerationPoint(direction);
            }

            //  Eventually, we want to instantiate our room(s) at the new direction's generation point.
            GameObject newRoom = Instantiate(layoutRoom, generationPoint.transform.position, generationPoint.transform.rotation);

            layoutRoomGOs.Add(newRoom);

            if (i + 1 == numOfRooms)
            {
                //  Finally, we want to assign and color our end room.
                endRoom = newRoom;
                layoutRoomGOs.RemoveAt(i);
                endRoom.GetComponent<SpriteRenderer>().color = endColor;
            }
        }

        CreateRoomOutline(Vector3.zero);

        foreach (GameObject layoutRoom in layoutRoomGOs)
        {
            CreateRoomOutline(layoutRoom.transform.position);
        }

        CreateRoomOutline(endRoom.transform.position);
    }

    private void CreateRoomOutline(Vector3 roomPosition)
    {
        bool roomAbove = Physics2D.OverlapCircle(roomPosition + new Vector3(0f, yOff, 0f), 0.2f, roomLayerMask);
        bool roomRight = Physics2D.OverlapCircle(roomPosition + new Vector3(xOff, 0f, 0f), 0.2f, roomLayerMask);
        bool roomBelow = Physics2D.OverlapCircle(roomPosition + new Vector3(0f, -yOff, 0f), 0.2f, roomLayerMask);
        bool roomLeft = Physics2D.OverlapCircle(roomPosition + new Vector3(-xOff, 0f, 0f), 0.2f, roomLayerMask);

        int directionCount = 0;

        if (roomAbove) directionCount++;
        if (roomRight) directionCount++;
        if (roomBelow) directionCount++;
        if (roomLeft) directionCount++;

        switch (directionCount)
        {
            case 1:
                if (roomAbove)
                {
                    generatedRoomOutlines.Add(Instantiate(layoutRooms.sUp, roomPosition, transform.rotation));
                }
                if (roomRight)
                {
                    generatedRoomOutlines.Add(Instantiate(layoutRooms.sRight, roomPosition, transform.rotation));
                }
                if (roomBelow)
                {
                    generatedRoomOutlines.Add(Instantiate(layoutRooms.sDown, roomPosition, transform.rotation));
                }
                if (roomLeft)
                {
                    generatedRoomOutlines.Add(Instantiate(layoutRooms.sLeft, roomPosition, transform.rotation));
                }

                break;
            case 2:
                if (roomAbove && roomBelow)
                {
                    generatedRoomOutlines.Add(Instantiate(layoutRooms.dUpDown, roomPosition, transform.rotation));
                }
                if (roomLeft && roomRight)
                {
                    generatedRoomOutlines.Add(Instantiate(layoutRooms.dLeftRight, roomPosition, transform.rotation));
                }
                if (roomAbove && roomRight)
                {
                    generatedRoomOutlines.Add(Instantiate(layoutRooms.dRightUp, roomPosition, transform.rotation));
                }
                if (roomRight && roomBelow)
                {
                    generatedRoomOutlines.Add(Instantiate(layoutRooms.dDownRight, roomPosition, transform.rotation));
                }
                if (roomBelow && roomLeft)
                {
                    generatedRoomOutlines.Add(Instantiate(layoutRooms.dLeftDown, roomPosition, transform.rotation));
                }
                if (roomLeft && roomAbove)
                {
                    generatedRoomOutlines.Add(Instantiate(layoutRooms.dUpLeft, roomPosition, transform.rotation));
                }

                break;
            case 3:
                if (roomLeft && roomAbove && roomRight)
                {
                    generatedRoomOutlines.Add(Instantiate(layoutRooms.tLeftUpRight, roomPosition, transform.rotation));
                }
                if (roomAbove && roomRight && roomBelow)
                {
                    generatedRoomOutlines.Add(Instantiate(layoutRooms.tUpRightDown, roomPosition, transform.rotation));
                }
                if (roomRight && roomBelow && roomLeft)
                {
                    generatedRoomOutlines.Add(Instantiate(layoutRooms.tRightDownLeft, roomPosition, transform.rotation));
                }
                if (roomBelow && roomLeft && roomAbove)
                {
                    generatedRoomOutlines.Add(Instantiate(layoutRooms.tDownLeftUp, roomPosition, transform.rotation));
                }

                break;
            case 4:
                generatedRoomOutlines.Add(Instantiate(layoutRooms.fourway, roomPosition, transform.rotation));
                break;
            default:
                Debug.LogError("DIRECTION COUNT EXEEDS POSSIBLE MAXIMAL DIRECTION COUNT OR IS 0: " + directionCount);
                break;
        }
    }

    private void Update()
    {
        if (Input.GetKey(KeyCode.R))
        {
            SceneManager.LoadScene(SceneManager.GetActiveScene().name);
        }
    }

    public void MoveGenerationPoint(Direction dir)
    {
        switch (dir)
        {
            case Direction.Up:
                generationPoint.position = new Vector3(generationPoint.position.x, generationPoint.position.y + yOff, 0f);
                break;
            case Direction.Right:
                generationPoint.position = new Vector3(generationPoint.position.x + xOff, generationPoint.position.y, 0f);
                break;
            case Direction.Down:
                generationPoint.position = new Vector3(generationPoint.position.x, generationPoint.position.y - yOff, 0f);
                break;
            case Direction.Left:
                generationPoint.position = new Vector3(generationPoint.position.x - xOff, generationPoint.position.y, 0f);
                break;
            default:
                generationPoint.position = Vector3.zero;
                break;
        }
    }

    private bool CheckForOverlappingRooms()
    {
        return Physics2D.OverlapCircle(generationPoint.position, 0.2f, roomLayerMask) ? true : false;
    }
}

[System.Serializable]
public class LayoutRoomPrefabs
{
    public GameObject sUp, sRight, sDown, sLeft,
        dUpDown, dLeftRight, dUpLeft, dLeftDown, dDownRight, dRightUp,
        tUpRightDown, tRightDownLeft, tDownLeftUp, tLeftUpRight,
        fourway;
}
