using System;
using System.Collections.Generic;

namespace Temp
{
  public class StateMachine
  {
    public List<StateBase> availableStates { get; private set; } = new List<StateBase>();
    public StateBase Curr { get; private set; }
    public StateBase Prev { get; private set; }

    public void AddState(StateBase state)
    {
      if (Curr == null)
        Curr = state;
      if (!availableStates.Contains(state))
        availableStates.Add(state);
    }

    public void AddState(List<StateType> statesToAdd)
    {
      StateBase state = null;
      foreach (var stateType in statesToAdd)
      {
        switch (stateType)
        {
          case StateType.Idle:
            state = new IdleState();
            break;
          case StateType.Run:
            state = new RunState();
            break;
          case StateType.Walk:
            state = new WalkState();
            break;
          case StateType.Fall:
            state = new FallState();
            break;
          case StateType.Jump:
            state = new JumpState();
            break;
          case StateType.Unexpected:
          default:
            Console.WriteLine("Current state is not implement");
            break;
        }
        if (Curr == null)
          Curr = state;
        if (!availableStates.Contains(state))
          availableStates.Add(state);
      }
    }

    public bool ChangeState(StateType typeToChange)
    {
      var index = availableStates.FindIndex(x => x.Type == typeToChange);
      if (index == -1)
        return false;
      else
      {
        if (Curr.AvailableStates.Contains(typeToChange))
        {
          Prev = Curr;
          Curr = availableStates[index];
          return true;
        }
        else
        {
          Console.WriteLine($"We can't change state from {Curr.Type} to {typeToChange} ");
          return false;
        }
      }
    }

    public void Do()
    {
      Curr.Do();
    }
  }

  public class StateBase
  {
    public readonly StateType Type;
    public readonly List<StateType> AvailableStates = new List<StateType>();
    public StateBase(StateType type)
    {
      Type = type;
      switch (type)
      {
        case StateType.Unexpected:
          break;
        case StateType.Idle:
          AvailableStates = new List<StateType>
          {
            StateType.Fall,
            StateType.Jump,
            StateType.Run,
            StateType.Walk
          };
          break;
        case StateType.Run:
          AvailableStates = new List<StateType>
          {
            StateType.Fall,
            StateType.Idle,
            StateType.Jump,
            StateType.Walk
          };
          break;
        case StateType.Walk:
          AvailableStates = new List<StateType>
          {
            StateType.Fall,
            StateType.Idle,
            StateType.Jump,
            StateType.Run,
          };
          break;
        case StateType.Jump:
          AvailableStates = new List<StateType>
          {
            StateType.Fall,
            StateType.Idle,
            StateType.Jump,
            StateType.Run,
            StateType.Walk
          };
          break;
        case StateType.Fall:
          AvailableStates = new List<StateType>
          {
            StateType.Idle,
            StateType.Run,
            StateType.Walk
          };
          break;
        default:
          break;
      }
    }

    public StateBase(StateBase other)
    {
      Type = other.Type;
    }

    public virtual void Do()
    { Console.WriteLine("This is unexpected Do"); }
  }

  public class IdleState : StateBase
  {
    public IdleState() : base(StateType.Idle) { }

    public override void Do()
    {
      Console.WriteLine("Idle..");
    }
  }

  public class RunState : StateBase
  {
    public RunState() : base(StateType.Run) { }

    public override void Do()
    {
      Console.WriteLine("Running..");
    }
  }

  public class WalkState : StateBase
  {
    public WalkState() : base(StateType.Walk) { }

    public override void Do()
    {
      Console.WriteLine("Walking..");
    }
  }
  public class JumpState : StateBase
  {
    public JumpState() : base(StateType.Jump) { }

    public override void Do()
    {
      Console.WriteLine("Jumping..");
    }
  }
  public class FallState : StateBase
  {
    public FallState() : base(StateType.Fall) { }

    public override void Do()
    {
      Console.WriteLine("Falling..");
    }
  }

  public enum StateType
  {
    Unexpected,
    Idle,
    Run,
    Walk,
    Jump,
    Fall,
  }
}
