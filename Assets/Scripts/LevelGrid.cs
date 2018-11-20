using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//Main grid where everything is located, used to detect "collision"
public class LevelGrid : MonoBehaviour //Only really a monobehaviour so we can use OnDrawGizmos
{
    //Grid settings
    [SerializeField]
    private Vector2i m_GridSize;

    [SerializeField]
    [Range(0, 100)]
    private float m_CellSize;
    public float CellSize
    {
        get { return m_CellSize; }
    }

    //The actual grid
    private LevelGridObject[,] m_GridObjects;

    private void Awake()
    {
        m_GridObjects = new LevelGridObject[m_GridSize.x, m_GridSize.y];
    }

    //Mutators
    public void SetLevelObject(Vector2i gridPosition, LevelGridObject levelGridObject)
    {
        SetLevelObject(gridPosition.x, gridPosition.y, levelGridObject);
    }

    public void SetLevelObject(int x, int y, LevelGridObject levelGridObject)
    {
        if (IsWithinBounds(x, y) == false)
            return;

        //No checks, overwrites at your own risk!
        m_GridObjects[x, y] = levelGridObject;
    }

    public void ClearLevelObject(Vector2i gridPosition)
    {
        ClearLevelObject(gridPosition.x, gridPosition.y);
    }

    public void ClearLevelObject(int x, int y)
    {
        if (IsWithinBounds(x, y) == false)
            return;

        m_GridObjects[x, y] = null;
    }


    //Accessors
    public LevelGridObject GetLevelObject(Vector2i gridPosition)
    {
        return GetLevelObject(gridPosition.x, gridPosition.y);
    }

    public LevelGridObject GetLevelObject(int x, int y)
    {
        if (IsWithinBounds(x, y) == false)
            return null;

        return m_GridObjects[x, y];
    }

    public bool IsEmpty(Vector2i gridPosition)
    {
        return IsEmpty(gridPosition.x, gridPosition.y);
    }

    public bool IsEmpty(int x, int y)
    {
        if (IsWithinBounds(x, y) == false)
            return false;

        return (m_GridObjects[x, y] == null);
    }

    public Vector3 GetWorldPosition(Vector2i gridPosition)
    {
        return GetWorldPosition(gridPosition.x, gridPosition.y);
    }

    public Vector3 GetWorldPosition(int x, int y)
    {
        if (IsWithinBounds(x, y) == false)
            return Vector3.zero;

        return new Vector3((x * m_CellSize) + (m_CellSize * 0.5f), (-y * m_CellSize) - (m_CellSize * 0.5f), 0);
    }


    //Utility
    public Vector2i WrapGridPosition(Vector2i gridPosition)
    {
        //% in C# can give negative numbers, unlike in other languages so let's make everything positive first
        if (gridPosition.x < 0) { gridPosition.x = m_GridSize.x + gridPosition.x; }
        if (gridPosition.y < 0) { gridPosition.y = m_GridSize.y + gridPosition.y; }

        return new Vector2i(gridPosition.x % m_GridSize.x, gridPosition.y % m_GridSize.y);
    }

    public bool IsWithinBounds(Vector2i gridPosition)
    {
        return IsWithinBounds(gridPosition.x, gridPosition.y);
    }

    public bool IsBelowBounds(Vector2i gridPosition)
    {
        return (gridPosition.y >= m_GridSize.y);
    }


    public bool IsWithinBounds(int x, int y)
    {
        return ((x < 0 || x >= m_GridSize.x || y < 0 || y >= m_GridSize.y) == false);
    }


    //Debug
    private void OnDrawGizmos()
    {
        // Draw a yellow sphere at the transform's position
        Gizmos.color = Color.yellow;

        //Draw all the vertical lines
        for (int x = 0; x < m_GridSize.x + 1; ++x) //+1 So we draw a border around is
        {
            Vector3 startPos = new Vector3(x * m_CellSize, 0, 0);
            Vector3 endPos = new Vector3(x * m_CellSize, -(m_GridSize.y * m_CellSize), 0); //- because we want 0, 0 to be in the top left

            Gizmos.DrawLine(startPos, endPos);
        }

        //Draw all the horizontal lines
        for (int y = 0; y < m_GridSize.y + 1; ++y) //+1 So we draw a border around is
        {
            Vector3 startPos = new Vector3(0, -(y * m_CellSize), 0);
            Vector3 endPos = new Vector3(m_GridSize.x * m_CellSize, -(y * m_CellSize), 0); //- because we want 0, 0 to be in the top left

            Gizmos.DrawLine(startPos, endPos);
        }

        //Fill the squares where we have LevelGridObjects
        if (m_GridObjects == null)
            return;

        Gizmos.color = Color.red;

        for (int x = 0; x < m_GridSize.x; ++x)
        {
            for (int y = 0; y < m_GridSize.y; ++y)
            {
                if (m_GridObjects[x, y] != null)
                {
                    //First line
                    Vector3 startPos = new Vector3(x * m_CellSize, -(y * m_CellSize), 0);
                    Vector3 endPos = new Vector3((x + 1) * m_CellSize, -((y + 1) * m_CellSize), 0); //- because we want 0, 0 to be in the top left

                    Gizmos.DrawLine(startPos, endPos);

                    //Second line
                    startPos = new Vector3((x + 1) * m_CellSize, -(y * m_CellSize), 0);
                    endPos = new Vector3(x * m_CellSize, -((y + 1) * m_CellSize), 0); //- because we want 0, 0 to be in the top left

                    Gizmos.DrawLine(startPos, endPos);
                }
            }
        }
    }
}
