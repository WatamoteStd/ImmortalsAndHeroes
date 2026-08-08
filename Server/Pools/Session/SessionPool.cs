using System;
using Server.Pools.Session;

namespace Server.Pools.Session;

public class SessionPool
{
    public UserSession[] Dense {get; private set;}
    public uint[] Sparse {get; private set;}
    public uint Count {get; private set;} = 0;
    

    public SessionPool(uint poolLength, uint activeLength)
    {
        Sparse = new uint[poolLength];

        for (int i = 0; i < poolLength; i++)
        {
            Sparse[i] = uint.MaxValue;
        }

        Dense = new UserSession[activeLength];

    }

    public void AddSession(UserSession user)
    {
        
        if (user.UserId >= Sparse.Length) 
        {
            Console.WriteLine($"[ERROR] Sparse array boundary reached for UserId {user.UserId}!");
            return;
        }

        if (Dense.Length <= Count) return;


        Dense[Count] = user;
        Sparse[user.UserId] = Count;
        Count++;

    }

    public UserSession GetSession(uint id)
    {

        if (id >= Sparse.Length) return null!;
        if (id == uint.MaxValue) return null!;

        return Dense[Sparse[id]];
    }

    public void DeleteSession(UserSession user)
    {
        
        if (user == null || user.UserId >= Sparse.Length) return;

        uint denseId = Sparse[user.UserId];
        if (denseId == uint.MaxValue) return;
        var lastSession = Dense[Count - 1];

        if (lastSession == user)
        {
            
            Dense[denseId] = null!;
            Sparse[user.UserId] = uint.MaxValue;
            Count --;
            return;

        }

        Dense[denseId] = lastSession;
        Sparse[user.UserId] = uint.MaxValue;
        Sparse[lastSession.UserId] = denseId;     

        Dense[Count - 1] = null!;
        Count--;   

    }




}