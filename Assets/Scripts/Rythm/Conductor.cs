using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Conductor : MonoBehaviour
{
    public static Conductor Instance;

    //Song beats per minute
    //This is determined by the song you're trying to sync up to
    public float songBpm;

    //The offset to the first beat of the song in seconds
    public float firstBeatOffset;

    //The number of seconds for each song beat
    public float secPerBeat;

    //Current song position, in seconds
    public float songPosition;

    //Current song position, in beats
    public float songPositionInBeats;

    //How many seconds have passed since the song started
    public float dspSongTime;

    //the number of beats in each loop
    public float beatsPerLoop;

    //the total number of loops completed since the looping clip first started
    public int completedLoops = 0;

    //The current position of the song within the loop in beats.
    public float loopPositionInBeats;

    //The current relative position of the song within the loop measured between 0 and 1.
    public float loopPositionInAnalog;

    //an AudioSource attached to this GameObject that will play the music.
    public AudioSource musicSource;

    private float tolerance = 0.1f;

    public Beat beat;

    public List<GameObject> notes = new List<GameObject>();
    public List<GameObject> spawnedNotes = new List<GameObject>();
    public GameObject noteSpawnLocation;
    public GameObject noteTarget;
    public Canvas beatCanvas;

    public int beatsShownInAdvance = 2;

    //the index of the next note to be spawned
    private int nextIndex = 0;

    bool noteHit;

    void Awake()
    {
        if (Instance == null)
            Instance = this;
    }

    // Start is called before the first frame update
    void Start()
    {
        //Load the AudioSource attached to the Conductor GameObject
        // musicSource = GetComponent<AudioSource>();

        //Calculate the number of seconds in each beat
        secPerBeat = 60f / songBpm;

        //Record the time when the music starts
        dspSongTime = (float)AudioSettings.dspTime;

        //Start the music
        musicSource.Play();
    }

    // Update is called once per frame
    void Update()
    {
        //determine how many seconds since the song started, accounting for off-beat start
        songPosition = (float)(AudioSettings.dspTime - dspSongTime - firstBeatOffset);

        //determine how many beats since the song started
        songPositionInBeats = songPosition / secPerBeat;

        //calculate the loop position
        if (songPositionInBeats >= (completedLoops + 1) * beatsPerLoop)
            completedLoops++;
        loopPositionInBeats = songPositionInBeats - completedLoops * beatsPerLoop;

        loopPositionInAnalog = loopPositionInBeats / beatsPerLoop;

        LoopNotes();

        // CheckForBeat();
    }

    void LoopNotes()
    {
        if (nextIndex < beat.notes.Count && beat.notes[nextIndex].GetComponent<Note>().beat < songPositionInBeats + beatsShownInAdvance)
        {
            spawnedNotes.Add(Instantiate(beat.notes[nextIndex], noteSpawnLocation.transform.position, noteSpawnLocation.transform.rotation));
            // Instantiate(beat.notes[nextIndex], noteSpawnLocation.transform.position, noteSpawnLocation.transform.rotation);

            spawnedNotes[nextIndex].transform.SetParent(beatCanvas.transform, true);
            spawnedNotes[nextIndex].SetActive(true);

            nextIndex++;

        }

        for (int i = 0; i < spawnedNotes.Count; i++)
        {
            spawnedNotes[i].GetComponent<RectTransform>().position = Vector3.Lerp(noteSpawnLocation.transform.position,
            noteTarget.transform.position,
            (beatsShownInAdvance - (spawnedNotes[i].GetComponent<Note>().beat - loopPositionInBeats)) / beatsShownInAdvance);
        }
    }

    // void CheckForBeat()
    // {
    //     int hitCount = 0;

    //     for (int i = 0; i < spawnedNotes.Count; i++)
    //     {
    //         if (spawnedNotes[i].GetComponent<Note>().IsHittable())
    //         {
    //             hitCount++;
    //         }
    //     }

    //     if (hitCount > 0)
    //     {
    //         noteHit = true;
    //     }
    //     else
    //     {
    //         noteHit = false;
    //     }
    // }

    public bool IsOnBeat()
    {
        return noteHit;
    }
}
