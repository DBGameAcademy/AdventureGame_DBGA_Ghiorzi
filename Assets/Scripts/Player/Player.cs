using UnityEngine;
using System.Collections.Generic;
using System.Collections;

public class Player : Actor
{ 
    public int MaxDarkValue { get => _maxDarkValue; }
    public int DarkValue { get; private set; }
    public bool IsMoving { get; private set; }
    public bool IsAttacking { get; private set; }
    public bool IsTransforming { get; private set; }
    public bool IsDark { get; private set; }
    public AdventureGame Controls { get => _controls; }

    public Vector2Int MovingDirection { get; private set; }
    public Vector3 AttackDirection { get; private set; }
    public Vector2Int TargetPosition { get => _targetPosition; }
    
    [SerializeField]
    private float moveSpeed;
    [SerializeField]
    private PlayerData playerData;

    private int experience;
    private Quest[] quests;
    private Weapon heldWeapon;
    private Armour equipedArmour;
    private Item[] consumables;
    private float potionCooldown;

    private AdventureGame _controls;
    private PlayerAnimation _playerAnimation;

    private Vector2Int _currentPosition;
    private Vector2Int _targetPosition;
    private Tile _lastTile;

    private float _attackStartTime;
    private float _attackDuration = 0.5f;

    private List<Actor> _targets = new List<Actor>();

    private int _currentTargetIndex = 0;

    private PlayerFall _playerFall;

    private int _maxDarkValue = 20;
    private int _lightDamage = 2;
    private int _darkDamage = 2;
    private int _darkAddAmount = 2;
    private int _darkRemoveAmount = 2;
    private int _damageIndex = 0;

    public void RestoreHealth()
    {
        currentHealth = maxHealth;
    }

    public void AddTarget(Actor target)
    {
        bool isFirst = false;
        if(_targets.Count == 0)
            isFirst = true;

        _targets.Add(target);

        if (isFirst)
        {
            Monster monst = (Monster)_targets[0];
            monst.TargetIndicator.Active();
        }
    }

    public void RemoveTarget(Actor target)
    {
        _targets.Remove(target);
        if (_targets.Count == 0)
        {
            GameController.Instance.EndBattle();
            return;
        }

        _currentTargetIndex = 0;
        Monster monst = (Monster)_targets[_currentTargetIndex];
        monst.TargetIndicator.Active();
    }

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
        DarkValue = 0;
        currentHealth = playerData.MaxHealth;
        maxHealth = playerData.MaxHealth;
        _darkDamage = playerData.BasicDarkDamages[_damageIndex];
        _lightDamage = playerData.BasicLightDamages[_damageIndex];
        _darkAddAmount = playerData.DarkAddAmount;
        _darkRemoveAmount = playerData.DarkRemoveAmount;
        _maxDarkValue = playerData.MaxDark;
        IsMoving = false;
        IsInBattle = false;
        _controls = new AdventureGame();
        _playerFall = GetComponent<PlayerFall>();
        _playerAnimation = GetComponentInChildren<PlayerAnimation>();
        _controls.Player.Move.performed += context => BeginMove(context.ReadValue<Vector2>());
        _controls.Player.Attack.performed += context => Attack();
        _controls.Player.SwitchTarget.performed += context => SwitchTarget();
        _controls.Player.Transform.performed += context => TransformToDark();
    }

    private void Update()
    {
        if (IsMoving)
        {
            if (!IsInBattle && !_playerFall.IsFalling)
            {
                Vector3 targetPos = new Vector3(_targetPosition.x, 0.28f, _targetPosition.y);
                if (Vector3.Distance(transform.position, targetPos) > float.Epsilon)
                {
                    transform.position = Vector3.MoveTowards(transform.position, targetPos, Time.deltaTime * moveSpeed);
                }
                else
                {
                    _currentPosition = _targetPosition;

                    Tile tile = DungeonController.Instance.CurrentRoom.Tiles[_currentPosition.x, _currentPosition.y];
                    if (tile != null && tile.IsWalkable)
                    {
                        if (_lastTile != null)
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
            else
            {
                IsMoving = false;
            }
           
        }

        if (IsAttacking)
        {
            if (!IsInBattle)
            {
                float t = (Time.time - _attackStartTime) / _attackDuration;
                Vector3 dir = new Vector3(MovingDirection.x,0.28f,MovingDirection.y);
                transform.position = DungeonController.Instance.GetTile(_currentPosition).TileObj.transform.position + dir * Mathf.PingPong(t,0.5f);
                if (t > 1f)
                {
                    // Finish
                    // procees damages and stuff
                    if (!IsDark)
                    {
                        _lightDamage = playerData.BasicLightDamages[_damageIndex];
                        _damageIndex = (_damageIndex + 1) % playerData.BasicLightDamages.Length;
                    }
                    else
                    {
                        _darkDamage = playerData.BasicDarkDamages[_damageIndex];
                        _damageIndex = (_damageIndex + 1) % playerData.BasicDarkDamages.Length;
                    }
                    MonsterController.Instance.MoveMonsters();
                    IsAttacking = false;
                }
            }
            else if(GameController.Instance.State == GameController.eGameState.PlayerTurn)
            {
                float t = (Time.time - _attackStartTime) / _attackDuration;
                Vector3 attackPos = new Vector3(_targets[_currentTargetIndex].transform.position.x, 0.0f, _targets[_currentTargetIndex].transform.position.z);
                Vector3 dir = attackPos - transform.position;
                AttackDirection = dir;
                transform.position = DungeonController.Instance.GetTile(_currentPosition).TileObj.transform.position + dir * Mathf.PingPong(t, 0.5f);
                if (t > 1f)
                {
                    // Finish
                    // procees damages and stuff
                    if (!IsDark) 
                    {
                        _lightDamage = playerData.BasicLightDamages[_damageIndex];
                        _targets[_currentTargetIndex].Damage(_lightDamage);
                        _damageIndex = (_damageIndex + 1) % playerData.BasicLightDamages.Length;
                    }
                    else
                    {
                        _darkDamage = playerData.BasicDarkDamages[_damageIndex];
                        _targets[_currentTargetIndex].Damage(_darkDamage);
                        _damageIndex = (_damageIndex + 1) % playerData.BasicDarkDamages.Length;
                    }
                    if (!IsDark)
                        AddDarkness(_darkAddAmount);
                    else
                        RemoveDarkness(_darkRemoveAmount);
                    MonsterController.Instance.MoveMonsters();
                    IsAttacking = false;
                    GameController.Instance.EndTurn();
                }
            }
        }
    }

    private void OnEnable()
    {
        _controls.Enable();
        UIController.Instance.SetUpPlayer(this);
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

        if (_playerFall.IsFalling)
            return;

        IsMoving = true;
        _targetPosition = position;
    }

    public void BeginMove(Vector2 direction)
    {
        if (CinematicController.Instance.IsPlaying || IsTransforming)
            return;

        if (_playerFall.IsFalling)
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

    private void Attack()
    {
        if (IsMoving || IsAttacking || IsTransforming)
            return;

        if (CinematicController.Instance.IsPlaying)
            return;
        if (GameController.Instance.State != GameController.eGameState.PlayerTurn && IsInBattle)
            return;

        _attackStartTime = Time.time;
        _playerAnimation.AttackAnimation();
        IsAttacking = true;
    }

    private void SwitchTarget()
    {
        if (_targets.Count <= 1)
            return;

        Monster monst = (Monster)_targets[_currentTargetIndex];
        monst.TargetIndicator.Deactive();
        
        _currentTargetIndex = (_currentTargetIndex+1)%_targets.Count;

        monst = (Monster)_targets[_currentTargetIndex];
        monst.TargetIndicator.Active();
    }

    protected override void OnKill()
    {
        // Handle Game Over Here
        foreach(Actor target in _targets)
        {
            Monster monst = (Monster)target;
            monst.TargetIndicator.Deactive();
        }
        GameController.Instance.EndBattle();
        base.OnKill();
    }

    private void AddDarkness(int amount)
    {
        DarkValue += amount;
        if(DarkValue>_maxDarkValue)
            DarkValue = _maxDarkValue;
    }

    private void RemoveDarkness(int amount)
    {
        DarkValue -= amount;
        if (DarkValue <= 0)
            DarkValue = 0;
        if (DarkValue == 0)
            TransformToLight();
    }

    private void TransformToDark()
    {
        if (IsDark || IsTransforming)
            return;
        if (DarkValue != _maxDarkValue)
            return;
        if (!IsInBattle)
            return;
        IsTransforming = true;
        _playerAnimation.PlayerTransfrom();
    }

    private void TransformToLight()
    {
        if ((!IsDark) || IsTransforming)
            return;
        IsTransforming = true;
        _playerAnimation.PlayerTransfrom();
    }

    public void DarkTransformationEnd()
    {
        Debug.Log("Dark transformation end");
        IsDark = true;
        Debug.Log(IsDark);
        IsTransforming = false;
        CinematicController.Instance.ResetZoom();
    }

    public void LigthTransformationEnd()
    {
        IsDark = false;
        IsTransforming = false;
        CinematicController.Instance.ResetZoom();
    }

    void OnLevelUp()
    {

    }
}
