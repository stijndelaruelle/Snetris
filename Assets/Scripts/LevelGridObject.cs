using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//An object that lives in the grid and has a single position within that grid
//This can range from snake-parts, pickups, walls, etc...
public class LevelGridObject : MonoBehaviour
{
    protected LevelGrid m_Grid;

    protected Vector2i m_GridPosition;
    public Vector2i GridPosition
    {
        get { return m_GridPosition; }
    }
}
