using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SnakePart : LevelGridObject
{
    public delegate void SnakeCollisionDelegate(LevelGridObject otherObject);
    public delegate void TetrisCollisionDelegate();

    [SerializeField]
    private SpriteRenderer m_SpriteRenderer;

    [SerializeField]
    private SnakeVisualsData m_Visuals;

    //Current state
    private SnakePartState m_CurrentState;
    private SnakePartSnakeState m_SnakeState;
    private SnakePartTetrisState m_TetrisState;

    //Double linked list
    private SnakePart m_Parent;
    private SnakePart m_Child;

    private Direction m_Direction;
    public Direction Direction
    {
        get { return m_Direction; }
        set { m_Direction = value; }
    }

    public event SnakeCollisionDelegate SnakeCollisionEvent;
    public event TetrisCollisionDelegate TetrisCollisionEvent;

    //Mutators
    public void Initialize(LevelGrid grid, Vector2i gridPosition, SnakePart parent, Direction direction)
    {
        m_Grid = grid;
        m_GridPosition = gridPosition;
        m_Parent = parent;
        m_Direction = direction;

        m_SnakeState = new SnakePartSnakeState(this);
        m_TetrisState = new SnakePartTetrisState(this);
        m_CurrentState = m_SnakeState;

        //Set the correct "grid" position
        m_Grid.SetLevelObject(gridPosition, this);

        //Set the correct "visual" position
        SetVisualPosition(m_Grid.GetWorldPosition(gridPosition));
        UpdateVisuals();

        //We don't need all the (clone) naming
        if (parent == null)
            gameObject.name = "Snake Head";
        else
            gameObject.name = "Snake Tail";
    }


    public void Move()
    {
        if (m_CurrentState != null)
            m_CurrentState.Move();
    }

    public bool HandleCollision(LevelGridObject otherObject)
    {
        if (m_CurrentState != null)
            return m_CurrentState.HandleCollision(otherObject);

        return false;
    }

    public SnakePart Grow(Direction direction)
    {
        //If we already have a child, cascade
        if (m_Child != null)
            return m_Child.Grow(direction);

        //If not, check if we can grow
        Vector2i newGridPosition = m_GridPosition + UtilityFunctions.DirectionToVector2i(direction);

        if (m_Grid.IsEmpty(newGridPosition) == false)
            return null;

        //If so, let's go!
        m_Child = GameObject.Instantiate<SnakePart>(this);
        m_Child.Initialize(m_Grid, newGridPosition, this, UtilityFunctions.InverseDirection(direction));

        //We are no longer the tail!
        if (m_Parent != null)
            gameObject.name = "Snake Body";

        UpdateVisuals();

        return m_Child;
    }
     
    public void UpdateVisuals()
    {
        //If we have no parent we are the head
        bool isHead = (m_Parent == null);

        //If we have no child we are the tail
        bool isTail = (m_Child == null);

        //Get the required directions
        Direction dir1 = m_Direction;
        Direction dir2 = Direction.None;

        if (m_Parent != null)
            dir1 = m_Parent.Direction;

        if (m_Child != null)
            dir2 = UtilityFunctions.InverseDirection(m_Child.Direction);

        //Actually find the sprite & assign it
        m_SpriteRenderer.sprite = m_Visuals.GetSprite(dir1, dir2, isHead, isTail);
    }

    public void SetVisualPosition(Vector3 position)
    {
        transform.position = position;
    }

    public void ChangeToTetrisState()
    {
        m_CurrentState = m_TetrisState;

        if (m_Child != null)
            m_Child.ChangeToTetrisState();
    }

    //Accessors
    private bool IsPartOfParent(LevelGridObject gridObject)
    {
        if (this == gridObject)
            return true;

        if (m_Parent == null)
            return false;

        return m_Parent.IsPartOfParent(gridObject);
    }

    private bool IsPartOfChild(LevelGridObject gridObject)
    {
        if (this == gridObject)
            return true;

        if (m_Child == null)
            return false;

        return m_Child.IsPartOfChild(gridObject);
    }

    //---------------
    // States
    //---------------
    private abstract class SnakePartState
    {
        protected SnakePart SP; //Not my usual convention, but it m_SnakePart get's unreadable very quickly!

        public SnakePartState(SnakePart snakePart) { SP = snakePart; }
        public abstract void Move();
        public abstract bool HandleCollision(LevelGridObject otherObject);
    }

    private class SnakePartSnakeState : SnakePartState
    {
        public SnakePartSnakeState(SnakePart snakePart) : base(snakePart) { }

        public override void Move()
        {
            //Move in the chosen direction
            Vector2i newGridPosition = SP.m_GridPosition + UtilityFunctions.DirectionToVector2i(SP.m_Direction);

            //Wrap the grid position
            newGridPosition = SP.m_Grid.WrapGridPosition(newGridPosition);

            //Check for other colliders
            LevelGridObject otherObject = SP.m_Grid.GetLevelObject(newGridPosition);
            if (otherObject != null)
            {
                bool stop = HandleCollision(otherObject);
                if (stop)
                    return;
            }

            //Clear our old position & set a new one
            SP.m_Grid.ClearLevelObject(SP.m_GridPosition);
            SP.m_GridPosition = newGridPosition;
            SP.m_Grid.SetLevelObject(newGridPosition, SP);

            //Update visual position
            SP.SetVisualPosition(SP.m_Grid.GetWorldPosition(newGridPosition));

            //Cascade down
            if (SP.m_Child != null)
            {
                SP.m_Child.Move();

                //Update the directions of our child
                SP.m_Child.Direction = SP.m_Direction;
            }

            SP.UpdateVisuals();
        }

        public override bool HandleCollision(LevelGridObject otherObject)
        {
            //Reallistically only the head will ever reach this place, as other pieces will always move in empty spaces
            //But just in case...
            if (SP.m_Parent != null)
            {
                return SP.m_Parent.HandleCollision(otherObject);
            }

            if (SP.SnakeCollisionEvent != null)
                SP.SnakeCollisionEvent(otherObject);

            return true;
        }
    }

    private class SnakePartTetrisState : SnakePartState
    {
        public SnakePartTetrisState(SnakePart snakePart) : base(snakePart) { }

        public override void Move()
        {
            bool success = CheckMove();
            if (success == false)
                return;

            //It's still a success, let's execute!
            ExecuteMove();

            //Let's get all the colliders right again
            FinishMove();
        }

        public bool CheckMove()
        {
            //Move down
            Vector2i newGridPosition = SP.m_GridPosition + UtilityFunctions.DirectionToVector2i(Direction.South);

            //If we reach the bottom, also for an event
            if (HandleBottomCollision(newGridPosition))
                return false;

            //Check for other colliders
            LevelGridObject otherObject = SP.m_Grid.GetLevelObject(newGridPosition);
            if (otherObject != null)
            {
                bool stop = HandleCollision(otherObject);
                if (stop)
                    return false;
            }

            //Cascade down
            if (SP.m_Child != null)
                return SP.m_Child.m_TetrisState.CheckMove();

            return true;
        }

        public void ExecuteMove()
        {
            //Move down
            Vector2i newGridPosition = SP.m_GridPosition + UtilityFunctions.DirectionToVector2i(Direction.South);

            //Clear our old position & set a new one
            SP.m_Grid.ClearLevelObject(SP.m_GridPosition);
            SP.m_GridPosition = newGridPosition;

            //Update visual position
            SP.SetVisualPosition(SP.m_Grid.GetWorldPosition(newGridPosition));

            //Cascade down
            if (SP.m_Child != null)
                SP.m_Child.m_TetrisState.ExecuteMove();
        }

        public void FinishMove()
        {
            SP.m_Grid.SetLevelObject(SP.m_GridPosition, SP);

            //Cascade down
            if (SP.m_Child != null)
                SP.m_Child.m_TetrisState.FinishMove();
        }

        public override bool HandleCollision(LevelGridObject otherObject)
        {
            //Check if the other object is part of our snake.
            bool isPartOfSnake = SP.IsPartOfParent(otherObject);
            if (isPartOfSnake == false) { isPartOfSnake = SP.IsPartOfChild(otherObject); }

            if (isPartOfSnake == true)
                return false;

            //We only care for collision at the bottom, all other should be ignored
            if (SP.GridPosition.y <= otherObject.GridPosition.y)
                return false;

            //If not, we hit something for real.
            if (SP.TetrisCollisionEvent != null)
                SP.TetrisCollisionEvent();

            return true;
        }

        private bool HandleBottomCollision(Vector2i gridPosition)
        {
            if (SP.m_Grid.IsBelowBounds(gridPosition))
            {
                if (SP.TetrisCollisionEvent != null)
                    SP.TetrisCollisionEvent();

                return true;
            }

            return false;
        }
    }
}
