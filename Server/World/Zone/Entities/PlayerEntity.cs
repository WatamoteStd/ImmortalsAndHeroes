using Shared.Characters;
using System.Numerics;
using Server.World.Zone.Entities;
using Server.World.Inventory;
using Shared.Items;
using Server.World.Zone.Intefaces;
using Server.World.MasteryTree;
using Shared.MasteryTree;
using Shared.MasteryTree.Rewards;
using Shared.Udp.Packets.Category.Game;

namespace Server.World;

public class PlayerEntity : LivingEntity
{

    public event Action<ushort, ItemType, ushort>? OnInventoryChanged;
    public event Action<int, int>? OnExpChanged; // this newExp totalExp
    public event Action<MasteryNodeId, uint, ushort>? OnBranchUpdate;
    public event Action<S2C_StatsSyncPacket>? OnStatsUpdated; 

    public enum State { Idle, Move, Chase, Attack, Cast, ProtectedCast, Controlled, Respawning}
    public State CurrentState = State.Idle;
    
    public uint PlayerId {get; private set;}
    public uint Silver {get; private set;}
    public int Exp {get; private set;}
    protected EntityBase _currentEnemy = null!;

    public InventoryBase Inventory = new InventoryBase(10);
    public PlayerMasteryTree MasteryTree {get;}

    public PlayerEntity(uint entityId, Vector3 pos, EntityType type, string name, uint playerId, uint regionId, uint silver) : base(entityId, pos, type, regionId)
    {
        Name = name;
        PlayerId = playerId;
        Silver = silver;

        Inventory.OnItemChanged += (slotIndex, item, count) =>
        {
            OnInventoryChanged?.Invoke(slotIndex, item,count);
        };

        MasteryTree = new PlayerMasteryTree(this);

        MasteryTree!.OnBranchUpdate += (branchId, exp, lvl) =>
        {
            OnBranchUpdate?.Invoke(branchId, exp, lvl);
        };



    }

    public override void Update(float deltaTime)
    {
        base.Update(deltaTime);
        if (!IsAlive) return;

        switch (CurrentState)
        {
            
            case State.Idle:

            break;

            case State.Move:
                {
                    
                    Move(deltaTime);

                }
            break;

            case State.Chase:
                {
                    
                    if (!_currentEnemy.IsValidEntity())
                    {
                        _currentEnemy = null!;
                        CurrentState = State.Idle;
                        _moveTarget = Position;
                        return;

                    }
                    if (!IsInAttackRadius(_currentEnemy))
                    {
                        
                        _moveTarget = _currentEnemy.Position;
                        Move(deltaTime);
                        return;

                    }
                    CurrentState = State.Attack;

                }
            break;

            case State.Attack:
                {
                    
                    if (!_currentEnemy.IsValidEntity())
                    {
                        
                        _currentEnemy = null!;
                        CurrentState = State.Idle;
                        return;

                    }

                    if (!IsInAttackRadius(_currentEnemy))
                    {
                        CurrentState= State.Chase;
                        return;
                    }

                    if (_currentAttackCooldown > 0) return;

                    if (_currentEnemy is IDamageable damageable)
                    {
                        damageable.TakeDamage(DamageTypes.Physical, (int)BaseDamage, this);
                        _currentAttackCooldown = _attackCooldown;
                    }


                }
            break;

        }
    }

    public void MoveToPosition(Vector3 pos)
    {
        
        _moveTarget = pos;
        CurrentState = State.Move;

    }

    public virtual void SetAttackTarget(EntityBase entity)
    {
    
        _currentEnemy = entity;
        CurrentState = State.Chase;

    }

    public void AddExp(int exp)
    {
        
        Exp += exp;
        OnExpChanged?.Invoke(exp, Exp);

    }

    public void AddBranchExp(MasteryNodeId id)
    {
        
        MasteryTree.AddExp(id);

    }


    public void UpdateStat(StatType stat, int value)
    {
        
        switch (stat)
        {
            
            case StatType.Armor:
                {
                    Armor += value;
                }
            break;
            case StatType.Health:
                {
                    MaxHealth += value;
                }
            break;
            case StatType.Mana:
                {
                    MaxMana += value;
                }
            break;
            case StatType.PhysicalDamage:
                {
                    int newDamage = (int)BaseDamage + value;
                    BaseDamage = (uint)Math.Max(0, newDamage);
                }
            break;
            case StatType.AttackSpeed:
                {
                    AttackSpeed += value;
                }
            break;
            case StatType.HealthRegen:
                {
                    HealthRegeneration += value;
                }
            break;
            case StatType.ManaRegen:
                {
                    ManaRegeneration += value;
                }
            break;
            case StatType.MagicResistance:
                {
                    MagicResistance += value;
                }
            break;
            default:

            break;

        }

        var packet = new S2C_StatsSyncPacket
        {
            Health = (uint)Health,
            Mana = (uint)Mana,
            HealthRegen = HealthRegeneration,
            ManaRegen = ManaRegeneration,
            Damage = BaseDamage,
            Armor = Armor,
            MagicResistance = MagicResistance,
            AttackSpeed = (uint)AttackSpeed,
            MaxHealth = (uint)MaxHealth,
            MaxMana = (uint)MaxMana,
            Speed = BaseSpeed
        };
        OnStatsUpdated?.Invoke(packet);

    }

    #region INVENTORY

    public ushort AddItem(ItemType item, ushort count)
    {
        ushort less = Inventory.AddItem(item, count);

        return less;
    }

    public bool RemoveItem(ItemType item, ushort count)
    {
        
        bool isOk = Inventory.RemoveItem(item, count);
        return isOk;


    }

    #endregion

    public override void ClearAllSubscriptions()
    {
        base.ClearAllSubscriptions();
        OnInventoryChanged = null!; 
        OnExpChanged = null!;
        OnBranchUpdate = null!;
        OnStatsUpdated = null!;
    }
    

}