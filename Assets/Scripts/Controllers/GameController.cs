using UnityEngine;
using Cinemachine;
using UnityEngine.SceneManagement;

public class GameController : Singleton<GameController>
{
    public enum eGameState
    {
        InTown,
        PlayerTurn,
        MonsterTurn
    }

    public Player Player { get; private set; }
    public eGameState State { get; private set; }

    [SerializeField]
    private GameObject playerPrefab;
    [SerializeField]
    private CinemachineVirtualCamera virtualCamera;

    private void Start()
    {
        StartNewGame();
    }

    public void GameOver()
    {
        UIController.Instance.ShowLoading();
        SceneManager.LoadScene("Game");
    }

    private void StartNewGame()
    {
        CreatePlayer();
        DungeonController.Instance.CreateNewMap();
    }

    public void CreatePlayer()
    {
        GameObject playerObj = Instantiate(playerPrefab, Vector3.zero, Quaternion.identity);
        Player = playerObj.GetComponent<Player>();

        // Camera Set
        virtualCamera.Follow = Player.transform;
        virtualCamera.LookAt = Player.transform;
    }

    public void StartBattle()
    {
        Player.IsInBattle = true;
        CinematicController.Instance.StartBattleCinematic();
        State = eGameState.PlayerTurn;
    }

    public void EndTurn()
    {
        if (State == eGameState.PlayerTurn)
            State = eGameState.MonsterTurn;
        else
            State = eGameState.PlayerTurn;
    }

    public void EndBattle()
    {
        Player.IsInBattle = false;
        CinematicController.Instance.EndBattleCinematic();
    }
}
