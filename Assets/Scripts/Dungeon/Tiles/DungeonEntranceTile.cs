using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class DungeonEntranceTile : Tile
{
    public override void EnterTile()
    {
        GameController.Instance.Player.StopMoving();
        UIController.Instance.ShowLoading();
        StartCoroutine(COCreateDungeon(() => { UIController.Instance.HideLoading(); }));
    }

    private IEnumerator COCreateDungeon(Action OnLoadingComplete)
    {
        yield return new WaitForSeconds(2.0f);
        DungeonController.Instance.CreateNewDungeon(5, new Vector2Int(3, 3), new Vector2Int(6, 6));
        OnLoadingComplete?.Invoke();
    }
}
