using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Temp
{
  public class StateMachineV2
  {
    private List<StateBaseV2> availableStates = new List<StateBaseV2>();
    public StateBaseV2 currentState { get; private set; }
    public StateBaseV2 previousState { get; private set; }

    public StateMachineV2(StateBaseV2[] states)
    {
      if (states.Length < 1)
      {
        Console.WriteLine("Error: states array less than 1");
        return;
      }
      foreach (var state in states)
      {
        availableStates.Add(state);
      }
      currentState = availableStates[0];
    }

    public void AddState(StateBaseV2 state)
    {
      var sameState = availableStates.Any(x => x.GetType() == state.GetType());
      if (sameState)
      {
        Console.WriteLine("Error: Same State Type");
        return;
      }
      availableStates.Add(state);
    }

    public void AddState(StateBaseV2[] states)
    {
      foreach (var state in states)
      {
        var sameState = availableStates.Any(x => x.GetType() == state.GetType());
        if (sameState)
        {
          Console.WriteLine("Error: Same State Type");
          return;
        }
        availableStates.Add(state);
      }
    }

    public bool ChangeState<T>() where T : StateBaseV2
    {
      var stateIndex = availableStates.FindIndex(x => x.GetType() == typeof(T));
      if (stateIndex < 0)
      {
        Console.WriteLine("Error: State not found");
        return false;
      }
      else
      {
        if (currentState.AvailableTransition<T>())
        {
          previousState = currentState;
          currentState = availableStates[stateIndex];
          Console.WriteLine($"New State: {currentState.name}");
          return true;
        }
        else
        {
          Console.WriteLine($"We can't change state from {currentState.name} to {typeof(T).Name}");
          return false;
        }
      }
    }

    public void Do(StateType stateType)
    {

    }

    private void JumpState()
    {
      Console.WriteLine("Jump");
    }
    private void IdleState()
    {
      Console.WriteLine("Idle..");
    }
    private void RunState()
    {
      Console.WriteLine("Running..");
    }
    private void WalkState()
    {
      Console.WriteLine("Walking..");
    }
    private void FallState()
    {
      Console.WriteLine("Falling..");
    }
  }

  public class StateQ
  {
    public Action Do;
    public StateQ(Action @do)
    {
      Do = @do;
    }
  }


  public abstract class StateBaseV2
  {
    public abstract string name { get; }
    protected abstract List<Type> availableTransitions { get; }

    public abstract Action Do { get; protected set; }

    public StateBaseV2(Action Do)
    {
      this.Do = Do;
    }

    public bool AvailableTransition<T>() where T : StateBaseV2
    {
      var possible = availableTransitions.Any(x => x == typeof(T));
      return possible;
    }

    //public abstract void Do();
  }

  public class State : StateBaseV2
  {
    public State(Action Do) : base(Do) { }

    public string _name => "State";
    public override string name => _name;

    public override Action Do { get; protected set; }

    protected override List<Type> availableTransitions => throw new NotImplementedException();
  }
}