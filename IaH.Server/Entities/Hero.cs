using IaH.Shared.Networking;
using System;
using System.Numerics;
using System.Collections.Generic;
using System.Text;

namespace IaH.Server.Entities
{
    public class Hero : BaseEntity
    {
        public float Speed = 5.0f;
        float _floatX, _floatY, _floatZ;
        public Vector3 TargetPosition;

        public enum StateMachine
        {
            Idle,
            Chase,
            Move,
            Attack,
            Cast,
            Respawning
        }
        public StateMachine CurrentState = StateMachine.Idle;

        public Hero(ushort id, short x, short y, short z, CharacterType hero) : base(id, x, y, z, hero)
        {

            _floatX = x / 100.0f;
            _floatY = y / 100.0f;
            _floatZ = z / 100.0f;

        }

        public void Update(float deltaTime)
        {
       
            switch (CurrentState)
            {

                case StateMachine.Move:

                    MoveLogic(deltaTime);
                    
                    break;

                case StateMachine.Idle:

                    

                    break;

            }
            X = (short)(_floatX * 100);
            Y = (short)(_floatY * 100);
            Z = (short)(_floatZ * 100);



        }

        public void MoveLogic(float deltaTime)
        {
            Vector3 MyPosition = new Vector3(_floatX, _floatY, _floatZ);

            Vector3 rawDirection = (TargetPosition - MyPosition);
            rawDirection.Y = 0;

            float Distance = rawDirection.Length();
            Console.WriteLine($"[MOVE] ID: {Id}, Dist: {Distance:F2}, Pos: {MyPosition}, Target: {TargetPosition}");

            if (Distance > 0.1f)
            {

                Vector3 direction = Vector3.Normalize(rawDirection);

                Vector3 velocity = direction * Speed * deltaTime;

                _floatX += velocity.X;
                _floatY += velocity.Y;
                _floatZ += velocity.Z;

                X = (short)(_floatX * 100);
                Y = (short)(_floatY * 100);
                Z = (short)(_floatZ * 100);

            }
            else
            {
                Console.WriteLine($"[MOVE] Прибыли в точку или слишком близко. Смена на Idle.");
                CurrentState = StateMachine.Idle;
            }
        }


    }
}
