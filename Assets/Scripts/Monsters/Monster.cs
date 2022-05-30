using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Monster: Actor
{ 
    public TargetIndicator TargetIndicator { get; private set; }
    public Vector2Int CurrentPosition { get => _currentPosition; }
    public Vector3 AttackDirection { get; set; }
    public Actor Target { get => _target; }
    public bool IsMoving { get; private set; }
    public Vector2Int MovingDirection { get; private set; }

    private float _moveSpeed = 3;

    private Reward _killReward;
    private int _damage;
    private int _dropMoney;

    private StateMachine _stateMachine;

    private Vector2Int _currentPosition;
    private Vector2Int _targetPosition;
    private Tile _lastTile;

    private Actor _target;

    public void SetupMonster(MonsterData monsterData)
    {
        currentHealth = monsterData.Health;
        maxHealth = monsterData.Health;
        _damage = monsterData.Damage;
        _dropMoney = monsterData.MoneyDrop;
    }

    // Maybe will inherite from Actor - let's see when coding movements
    public void SetPosition(Vector2Int position)
    {
        transform.position = new Vector3(position.x, transform.position.y, position.y);
    }

    protected override void OnKill()
    {
        DungeonController.Instance.GetTile(_currentPosition).UnsetCharacterObject();
        Player player = (Player)_target;
        player.RemoveTarget(this);
        player.AddMoney(_dropMoney);
        MonsterController.Instance.RemoveMonster(this);
        base.OnKill();
    }

    private void Awake()
    {
        TargetIndicator = GetComponentInChildren<TargetIndicator>();

        _moveSpeed = 3;
        IsMoving = false;
        IsInBattle = false;
        _stateMachine = new StateMachine();

        // States instantiation
        var idle = new Idle();
        var waiting = new Waiting();
        var attacking = new Attacking(this);
        // Transitions and Any-Transitions
        _stateMachine.AddTransition(idle, waiting, () => IsInBattle);
        _stateMachine.AddTransition(waiting, attacking, () => (IsInBattle && GameController.Instance.State == GameController.eGameState.MonsterTurn));
        _stateMachine.AddTransition(attacking, waiting, () => IsInBattle && GameController.Instance.State != GameController.eGameState.MonsterTurn);
        // Set the initial state
        _stateMachine.SetState(idle);
    }

    private void Start()
    {
        _currentPosition = new Vector2Int((int)transform.position.x, (int)transform.position.z);
    }

    private void Update()
    {
        CheckForPlayer();
        _stateMachine.Tick();

        if (IsMoving)
        {
            Vector3 targetPos = new Vector3(_targetPosition.x, this.transform.position.y, _targetPosition.y);
            float dist = Vector3.Distance(transform.position, targetPos);
            if (dist > float.Epsilon)
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
        if(freeDirections.Count > 0)
        {
            int randIndex = Random.Range(0, freeDirections.Count);
            Vector2Int moveDirection = freeDirections[randIndex];
            MovingDirection = moveDirection;
            BeginMove(moveDirection);
        }
        // Else don't move 
    }

    private void BeginMove(Vector2Int direction)
    {
        Vector2Int intDirection = direction;
        //MovingDirection = intDirection;
        Vector2Int position = _currentPosition + intDirection;

        if (position.x < 0 || position.y < 0
            || position.x >= DungeonController.Instance.CurrentRoom.Size.x
            || position.y >= DungeonController.Instance.CurrentRoom.Size.y)
        {
            return;
        }

        Tile tile = DungeonController.Instance.GetTile(position);
        if (!IsMoving && tile != null && tile.IsWalkable)
        {
            IsMoving = true;
            _targetPosition = position;
            tile.SetCharacterObject(this); // Tile reservation
        }
    }

    public int GetDamage()
    {
        return _damage;
    }

    public bool CheckForPlayer()
    {
        if (DungeonController.Instance.CurrentRoom == null)
            return false;
        if (IsInBattle)
            return false;

        Vector2Int monsterPosition = new Vector2Int((int)transform.position.x, (int)transform.position.z);
        Player player = null;
        List<Vector2Int> directions = new List<Vector2Int>();
        directions.Add(Vector2Int.up);
        directions.Add(Vector2Int.down);
        directions.Add(Vector2Int.left);
        directions.Add(Vector2Int.right);

        foreach(Vector2Int direction in directions)
        {
            Vector2Int position = monsterPosition + direction;
            if(position.x < 0 || position.y < 0
            || position.x >= DungeonController.Instance.CurrentRoom.Size.x
            || position.y >= DungeonController.Instance.CurrentRoom.Size.y)
            {
                continue;
            }
            if(DungeonController.Instance.GetTile(position) != null 
               && DungeonController.Instance.GetTile(position).CharacterObject != null)
            {
                if (DungeonController.Instance.GetTile(position).CharacterObject.GetComponent<Player>() != null)
                {
                    player = DungeonController.Instance.GetTile(position).CharacterObject.GetComponent<Player>();
                    break;
                }
            }
        }

        if (player != null && (!player.IsInBuilding))
        {
            player.AddTarget(this);
            _target = player;
            IsInBattle = true;
            if(!player.IsInBattle)
                GameController.Instance.StartBattle();
            return true;
        }
        return false;
    }
}
