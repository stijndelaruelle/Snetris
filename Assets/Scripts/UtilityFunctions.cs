using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum Direction
{
    None = 0x0000,   //0
    North = 0x0001,  //1
    East = 0x0010,   //2
    South = 0x0100,  //4
    West = 0x1000//,   //8

    //NorthEast = 0x0011, //3
    //NorthWest = 0x1001, //9
    //SouthEast = 0x0110, //6
    //SouthWest = 0x1100  //12
}

[Serializable]
public struct Vector2i
{
    public int x;
    public int y;

    public static Vector2i Zero
    {
        get { return new Vector2i(0, 0); }
    }

    public Vector2i(int x, int y)
    {
        this.x = x;
        this.y = y;
    }

    //Operator overloading
    public static Vector2i operator +(Vector2i me, Vector2i other)
    {
        return new Vector2i(me.x + other.x, me.y + other.y);
    }

    public static Vector2i operator -(Vector2i me, Vector2i other)
    {
        return new Vector2i(me.x - other.x, me.y - other.y);
    }

    public static Vector2i operator *(Vector2i me, Vector2i other)
    {
        return new Vector2i(me.x * other.x, me.y * other.y);
    }

    public static Vector2i operator /(Vector2i me, Vector2i other)
    {
        return new Vector2i(me.x / other.x, me.y / other.y);
    }


    public static Vector2i operator + (Vector2i me, int other)
    {
        return new Vector2i(me.x + other, me.y + other);
    }

    public static Vector2i operator - (Vector2i me, int other)
    {
        return new Vector2i(me.x - other, me.y - other);
    }

    public static Vector2i operator * (Vector2i me, int other)
    {
        return new Vector2i(me.x * other, me.y * other);
    }

    public static Vector2i operator / (Vector2i me, int other)
    {
        return new Vector2i(me.x / other, me.y / other);
    }
}

public static class UtilityFunctions
{
    public static Vector3 DirectionToVector(Direction direction)
    {
        switch (direction)
        {
            case Direction.North:
                return new Vector3(0.0f, -1.0f, 0.0f);

            case Direction.East:
                return new Vector3(1.0f, 0.0f, 0.0f);

            case Direction.South:
                return new Vector3(0.0f, 1.0f, 0.0f);

            case Direction.West:
                return new Vector3(-1.0f, 0.0f, 0.0f);
        }

        return Vector3.zero;
    }

    public static Vector2i DirectionToVector2i(Direction direction)
    {
        switch (direction)
        {
            case Direction.North:
                return new Vector2i(0, -1);

            case Direction.East:
                return new Vector2i(1, 0);

            case Direction.South:
                return new Vector2i(0, 1);

            case Direction.West:
                return new Vector2i(-1, 0);
        }

        return Vector2i.Zero;
    }

    public static Direction InverseDirection(Direction direction)
    {
        switch (direction)
        {
            case Direction.North:
                return Direction.South;

            case Direction.East:
                return Direction.West;

            case Direction.South:
                return Direction.North;

            case Direction.West:
                return Direction.East;
        }

        return Direction.None;
    }
}
