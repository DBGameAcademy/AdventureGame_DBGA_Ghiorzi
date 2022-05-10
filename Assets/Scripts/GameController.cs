using UnityEngine;

public class GameController : Singleton<GameController>
{ 
    private void Start()
    {
        StartNewGame();
    }

    private void StartNewGame()
    {
        DungeonController.Instance.CreateNewDungeon();
        DungeonController.Instance.MakeCurrentRoom();
    }
}
