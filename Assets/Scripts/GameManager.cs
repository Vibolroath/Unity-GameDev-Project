using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    static private GameManager instance;
    static public GameManager Instance
    {
        get
        {
            if (instance == null)
            {
                Debug.LogError("There is no GameManager instance in the scene.");
            }
            return instance;
        }
    }

 [SerializeField] private Transform startPosition;

    private Transform lastCheckpoint;
    public Transform LastCheckpoint
    {
        get
        {
            return lastCheckpoint;
        }
    }

    private LogFile log;

    private Player player;
    public Player Player
    {
        get
        {
            return player;
        }
    }

    [SerializeField] private float respawnDelay;

    private void Awake()
    {
        if (instance != null)
        {
            Destroy(gameObject);
        }
        else
        {
            instance = this;
        }
    }

    void Start()
    {
        lastCheckpoint = startPosition;

        log = gameObject.GetComponent<LogFile>();
        player = FindObjectOfType<Player>();
        player.transform.position = startPosition.transform.position;
    }

    public void Die()
    {
        log.WriteLine("Player Died", Time.time,
            SceneManager.GetActiveScene().name, player.transform.position);
        StartCoroutine(RespawnTimer());
    }

    private IEnumerator RespawnTimer() 
    {
        yield return new WaitForSeconds(respawnDelay);
        player.transform.position = lastCheckpoint.transform.position;
        player.Revive();
    }


    public void Checkpoint(Transform checkpoint)
    {
        log.WriteLine("Checkpoint Activated", Time.time,
            SceneManager.GetActiveScene().name, player.transform.position);
        lastCheckpoint = checkpoint;
    }

    public void NextLevel()
    {
        Scene scene = SceneManager.GetActiveScene();
        int last = SceneManager.sceneCountInBuildSettings - 1;
        int next;

        log.WriteLine("Teleporter Used", Time.time,
            scene.name, player.transform.position);

        if (scene.buildIndex == last)
        {
            next = 0;  // returns to first level if player has finished the last level (TEMPORARY)
        }
        else
        {
            next = scene.buildIndex + 1;  // scene changes to next in build
        }
        SceneManager.LoadScene(next);
    }
}
