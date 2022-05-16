using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Monster: Actor
{
    public bool IsMoving { get; private set; }
    public Vector2Int MovingDirection { get; private set; }

    private float _moveSpeed = 3;

    private Reward _killReward;
    private int _damage;

    private StateMachine _stateMachine;

    private Vector2Int _currentPosition;
    private Vector2Int _targetPosition;
    private Tile _lastTile;

    public void SetupMonster(MonsterData monsterData)
    {
        currentHealth = monsterData.Health;
        maxHealth = monsterData.Health;
        _damage = monsterData.Damage;
    }

    // Maybe will inherite from Actor - let's see when coding movements
    public void SetPosition(Vector2Int position)
    {
        transform.position = new Vector3(position.x, transform.position.y, position.y);
    }

    protected override void OnKill()
    {

    }

    private void Awake()
    {
        _moveSpeed = 3;
        IsMoving = false;
        _stateMachine = new StateMachine();

        // States instantiation
        var idle = new Idle();

        // Transitions and Any-Transitions

        // Set the initial state
        _stateMachine.SetState(idle);
    }

    private void Update()
    {
        _stateMachine.Tick();
        
        if (IsMoving)
        {
            Vector3 targetPos = new Vector3(_targetPosition.x, 0.19f, _targetPosition.y);
            if (Vector3.Distance(transform.position, targetPos) > float.Epsilon)
            {
                transform.position = Vector3.MoveTowards(transform.position, targetPos, Time.deltaTime * _moveSpeed);
            }
            else
            {
                _currentPosition = _targetPosition;
                _lastTile.UnsetCharacterObject();
                Tile tile = DungeonController.Instance.GetTile(_currentPosition);
                tile.SetCharacterObject(this);
                _lastTile = tile;
                IsMoving = false;
            }
        }   
    }

    public void Move()
    {
        if (_stateMachine.CurrentState.GetType() != typeof(Idle))
            return;
        _currentPosition = new Vector2Int((int)transform.position.x, (int)transform.position.z);
        // Find free tiles in range
        List<Vector2Int> freeDirections = new List<Vector2Int>();
        Vector2Int monsterPos = new Vector2Int((int)transform.position.x, (int)transform.position.z);

        Vector2Int testPos = monsterPos + Vector2Int.up;
        if (!(testPos.x < 0 || testPos.y < 0
            || testPos.x >= DungeonController.Instance.CurrentRoom.Size.x
            || testPos.y >= DungeonController.Instance.CurrentRoom.Size.y)) 
        {
            if (DungeonController.Instance.GetTile(testPos) != null
            && DungeonController.Instance.GetTile(testPos).GetType() == typeof(EmptyTile)
            && DungeonController.Instance.GetTile(testPos).IsWalkable
            && testPos!=GameController.Instance.Player.TargetPosition)
                freeDirections.Add(Vector2Int.up);
        }

        testPos = monsterPos + Vector2Int.down;
        if (!(testPos.x < 0 || testPos.y < 0
            || testPos.x >= DungeonController.Instance.CurrentRoom.Size.x
            || testPos.y >= DungeonController.Instance.CurrentRoom.Size.y))
        {
            if (DungeonController.Instance.GetTile(testPos) != null
            && DungeonController.Instance.GetTile(testPos).GetType() == typeof(EmptyTile)
            && DungeonController.Instance.GetTile(testPos).IsWalkable
            && testPos != GameController.Instance.Player.TargetPosition)
                freeDirections.Add(Vector2Int.down);
        }

        testPos = monsterPos + Vector2Int.right;
        if (!(testPos.x < 0 || testPos.y < 0
            || testPos.x >= DungeonController.Instance.CurrentRoom.Size.x
            || testPos.y >= DungeonController.Instance.CurrentRoom.Size.y))
        {
            if (DungeonController.Instance.GetTile(testPos) != null
            && DungeonController.Instance.GetTile(testPos).GetType() == typeof(EmptyTile)
            && DungeonController.Instance.GetTile(testPos).IsWalkable
            && testPos != GameController.Instance.Player.TargetPosition)
                freeDirections.Add(Vector2Int.right);
        }

        testPos = monsterPos + Vector2Int.left;
        if (!(testPos.x < 0 || testPos.y < 0
            || testPos.x >= DungeonController.Instance.CurrentRoom.Size.x
            || testPos.y >= DungeonController.Instance.CurrentRoom.Size.y))
        {
            if (DungeonController.Instance.GetTile(testPos) != null
            && DungeonController.Instance.GetTile(testPos).GetType() == typeof(EmptyTile)
            && DungeonController.Instance.GetTile(testPos).IsWalkable
            && testPos != GameController.Instance.Player.TargetPosition)
                freeDirections.Add(Vector2Int.left);
        }

        // Update LastTile
        _lastTile = DungeonController.Instance.GetTile(_currentPosition);
        _lastTile.UnsetCharacterObject();

        // Get free random direction
        int randIndex = Random.Range(0, freeDirections.Count);
        Vector2Int moveDirection = freeDirections[randIndex];
        MovingDirection = moveDirection;
        BeginMove(moveDirection);
    }

    private void BeginMove(Vector2 direction)
    {
        Vector2Int intDirection = new Vector2Int((int)direction.x, (int)direction.y);
        //MovingDirection = intDirection;
        Vector2Int position = _currentPosition + intDirection;

        if (position.x < 0 || position.y < 0
            || position.x >= DungeonController.Instance.CurrentRoom.Size.x
            || position.y >= DungeonController.Instance.CurrentRoom.Size.y)
        {
            return;
        }

        Tile tile = DungeonController.Instance.CurrentRoom.Tiles[position.x, position.y];
        if (!IsMoving && tile != null && tile.IsWalkable)
        {
            IsMoving = true;
            _targetPosition = position;
        }
    }
}
