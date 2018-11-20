using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Snetris/Snake Visuals")]
public class SnakeVisualsData : ScriptableObject
{
    [Header("Head")]
    [SerializeField] private Sprite m_HeadNorth;
    [SerializeField] private Sprite m_HeadSouth;
    [SerializeField] private Sprite m_HeadEast;
    [SerializeField] private Sprite m_HeadWest;

    [Header("Tail")]
    [SerializeField] private Sprite m_TailNorth;
    [SerializeField] private Sprite m_TailSouth;
    [SerializeField] private Sprite m_TailEast;
    [SerializeField] private Sprite m_TailWest;

    [Header("Body")]
    [SerializeField] private Sprite m_BodyNorthSouth;
    [SerializeField] private Sprite m_BodyEastWest;

    [SerializeField] private Sprite m_BodyNorthEast;
    [SerializeField] private Sprite m_BodyNorthWest;
    [SerializeField] private Sprite m_BodySouthEast;
    [SerializeField] private Sprite m_BodySouthWest;

    //Super dirty function but hey!
    public Sprite GetSprite(Direction dir1, Direction dir2, bool isHead, bool isTail)
    {
        if (isHead)
        {
            if (dir1 == Direction.North) { return m_HeadNorth; }
            if (dir1 == Direction.South) { return m_HeadSouth; }
            if (dir1 == Direction.East)  { return m_HeadEast; }
            if (dir1 == Direction.West)  { return m_HeadWest; }

            return null;
        }

        if (isTail)
        {
            if (dir1 == Direction.North) { return m_TailNorth; }
            if (dir1 == Direction.South) { return m_TailSouth; }
            if (dir1 == Direction.East)  { return m_TailEast; }
            if (dir1 == Direction.West)  { return m_TailWest; }

            return null;
        }

        if ((dir1 == Direction.North && dir2 == Direction.South) || (dir2 == Direction.North && dir1 == Direction.South)) { return m_BodyNorthSouth; }
        if ((dir1 == Direction.East && dir2  == Direction.West)  || (dir2 == Direction.East && dir1 == Direction.West))   { return m_BodyEastWest; }

        if ((dir1 == Direction.North && dir2 == Direction.East)  || (dir2 == Direction.North && dir1 == Direction.East))  { return m_BodyNorthEast; }
        if ((dir1 == Direction.North && dir2 == Direction.West)  || (dir2 == Direction.North && dir1 == Direction.West))  { return m_BodyNorthWest; }
        if ((dir1 == Direction.South && dir2 == Direction.East)  || (dir2 == Direction.South && dir1 == Direction.East))  { return m_BodySouthEast; }
        if ((dir1 == Direction.South && dir2 == Direction.West)  || (dir2 == Direction.South && dir1 == Direction.West))  { return m_BodySouthWest; }

        return null;
    }
}
