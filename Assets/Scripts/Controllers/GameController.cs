using UnityEngine;
using Cinemachine;

public class GameController : Singleton<GameController>
{
    public Player Player { get; private set; }

    [SerializeField]
    private GameObject playerPrefab;
    [SerializeField]
    private CinemachineVirtualCamera virtualCamera;

    private void Start()
    {
        StartNewGame();
    }

    private void StartNewGame()
    {
        CreatePlayer();
        DungeonController.Instance.CreateNewDungeon();
        DungeonController.Instance.MakeCurrentRoom();
    }

    private void CreatePlayer()
    {
        GameObject playerObj = Instantiate(playerPrefab, Vector3.zero, Quaternion.identity);
        Player = playerObj.GetComponent<Player>();

        // Camera Set
        virtualCamera.Follow = Player.transform;
        virtualCamera.LookAt = Player.transform;
        
    }
}
