using UnityEngine;

public class Player : Actor
{ 
    public bool IsMoving { get; private set; }
    public Vector2Int MovingDirection { get; private set; }
    public Vector2Int TargetPosition { get => _targetPosition; }
    
    [SerializeField]
    private float moveSpeed;

    private int experience;
    private Quest[] quests;
    private Weapon heldWeapon;
    private Armour equipedArmour;
    private Item[] consumables;
    private float potionCooldown;

    private AdventureGame _controls;

    private Vector2Int _currentPosition;
    private Vector2Int _targetPosition;
    private Tile _lastTile;
   
    public void SetPosition(Vector2Int position)
    {
        _currentPosition = position;
        transform.position = new Vector3(_currentPosition.x, transform.position.y, _currentPosition.y);

    }

    public void SetHeight(float height)
    {
        transform.position = new Vector3(_currentPosition.x, height, _currentPosition.y);
    }

    public void StopMoving()
    {
        IsMoving = false;
    }

    private void Awake()
    {
        IsMoving = false;
        _controls = new AdventureGame();
        _controls.Player.Move.performed += context => BeginMove(context.ReadValue<Vector2>());
        
    }

    private void Update()
    {
        if (IsMoving)
        {
            Vector3 targetPos = new Vector3(_targetPosition.x, 0.28f, _targetPosition.y);
            if(Vector3.Distance(transform.position, targetPos) > float.Epsilon)
            {
                transform.position = Vector3.MoveTowards(transform.position, targetPos, Time.deltaTime * moveSpeed);
            }
            else
            {
                _currentPosition = _targetPosition;

                Tile tile = DungeonController.Instance.CurrentRoom.Tiles[_currentPosition.x, _currentPosition.y];
                if(tile != null && tile.IsWalkable)
                {
                    if(_lastTile!=null)
                        _lastTile.UnsetCharacterObject();

                    tile.EnterTile();
                    tile.SetCharacterObject(this);
                    _lastTile = tile;

                    

                    if (_controls.Player.Move.inProgress && IsMoving)
                    {
                        FindNewTargetPosition(MovingDirection);
                        MonsterController.Instance.MoveMonsters();
                        return;
                    }
                }
                MonsterController.Instance.MoveMonsters();
                IsMoving = false;
            }
        }
    }

    private void OnEnable()
    {
        _controls.Enable();
    }

    private void OnDestroy()
    {
        _controls.Disable();
    }

    private void FindNewTargetPosition(Vector2 direction)
    {
        Vector2Int intDirection = new Vector2Int((int)direction.x, (int)direction.y);
        MovingDirection = intDirection;
        Vector2Int position = _currentPosition + intDirection;

        if (position.x < 0 || position.y < 0
            || position.x >= DungeonController.Instance.CurrentRoom.Size.x
            || position.y >= DungeonController.Instance.CurrentRoom.Size.y
            || DungeonController.Instance.CurrentRoom.Tiles[position.x,position.y]==null
            || !DungeonController.Instance.GetTile(position).IsWalkable)
        {
            return;
        }

        IsMoving = true;
        _targetPosition = position;
    }

    private void BeginMove(Vector2 direction)
    {
        if (CinematicController.Instance.IsPlaying)
            return;

        Vector2Int intDirection = new Vector2Int((int)direction.x, (int)direction.y);
        MovingDirection = intDirection;
        Vector2Int position = _currentPosition + intDirection;

        if(position.x<0 || position.y < 0 
            || position.x >= DungeonController.Instance.CurrentRoom.Size.x
            || position.y >= DungeonController.Instance.CurrentRoom.Size.y
            || DungeonController.Instance.CurrentRoom.Tiles[position.x, position.y] == null
            || !DungeonController.Instance.GetTile(position).IsWalkable)
        {
            return;
        }

        Tile tile = DungeonController.Instance.CurrentRoom.Tiles[position.x, position.y];
        if(!IsMoving && tile != null && tile.IsWalkable)
        {
            IsMoving = true;
            _targetPosition = position;
        }
    }

    void OnLevelUp()
    {

    }
}
