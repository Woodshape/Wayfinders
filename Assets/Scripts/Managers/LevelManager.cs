using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelManager : MonoBehaviour
{
    public static LevelManager Instance;

    public float waitToLoad = 5f;

    public string nextLevel;

    void Awake()
    {
        if (Instance == null)
            Instance = this;
    }

    public IEnumerator NextLevel()
    {
        PlayerController.Instance.FreezePlayer();
        PlayerHealthController.Instance.MakeInvincible(waitToLoad);

        AudioManager.Instance.PlayVictory();

        yield return new WaitForSeconds(waitToLoad);

        SceneManager.LoadScene(nextLevel);
    }
}
