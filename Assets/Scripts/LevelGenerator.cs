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
                return;
            case Direction.Right:
                generationPoint.position = new Vector3(generationPoint.position.x + xOff, generationPoint.position.y, 0f);
                return;
            case Direction.Down:
                generationPoint.position = new Vector3(generationPoint.position.x, generationPoint.position.y - yOff, 0f);
                return;
            case Direction.Left:
                generationPoint.position = new Vector3(generationPoint.position.x - xOff, generationPoint.position.y, 0f);
                return;
            default:
                generationPoint.position = Vector3.zero;
                return;
        }
    }

    private bool CheckForOverlappingRooms()
    {
        return Physics2D.OverlapCircle(generationPoint.position, 0.2f, roomLayerMask) ? true : false;
    }
}
