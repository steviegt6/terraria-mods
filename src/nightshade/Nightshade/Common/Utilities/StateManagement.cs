using System.Collections.Generic;

namespace Nightshade.Common.Utilities;

public struct StateID
{
    internal string name;
    internal string id;
}

public abstract class State<T> where T : struct
{
    public StateID stateID;
    internal StateController<T>? enclosingController;

    /// <summary>
    /// Called when the state is pushed onto the stack; perform any necessary setup here. Use Resource Acquisition Is Initialization (RAII) as your entity does not necessarily know what is going on inside the state.
    /// </summary>
    /// <param name="parameters">A struct parameter to use inside the state.</param>
    /// <returns></returns>
    public abstract bool Enter(params T[] parameters);
    /// <summary>
    /// Called when the state is popped from the stack; perform any necessary cleanup of unmanaged resources here.
    /// </summary>
    /// <param name="parameters"></param>
    /// <returns></returns>
    public abstract bool Exit(params T[] parameters);
    public abstract bool Update(params T[] parameters);

    public bool PopSelf(params T[] parameters)
    {
        if (enclosingController != null)
        {
            Exit(parameters);
            return enclosingController.PopCurState();
        }
        return false;
    }
}

public class StateController<T> where T : struct
{
    public StateController()
    {
        States = new Stack<State<T>>();
    }

    protected Stack<State<T>> States;
    public State<T>? CurrentState => States.TryPeek(out var state) ? state : null;

    public delegate bool StateDelegate(StateID StateID, params T[] parameters);
    public event StateDelegate? OnStatePush;
    public event StateDelegate? OnStatePop;

    public bool PushState<S>(S state, params T[] arguments) where S : State<T>
    {
        if (state is State<T> newState)
        {
            newState.enclosingController = this;
            newState.stateID = new StateID
            {
                name = state.GetType().Name,
                id = state.GetHashCode().ToString()
            };

            newState.Enter(arguments);
            States.Push(newState);

            return OnStatePush?.Invoke(newState.stateID, arguments) ?? true;
        }

        return false;
    }
    public bool PopCurState()
    {
        if (States.TryPop(out var state))
        {
            state.Exit();
            return OnStatePop?.Invoke(state.stateID) ?? true;
        }
        return false;
    }

    public bool PopState(StateID id)
    {
        if (States.TryPeek(out var currentState) && currentState.stateID.Equals(id))
        {
            return PopCurState();
        }
        return false;
    }
    public bool PopState(string name)
    {
        if (States.TryPeek(out var currentState) && currentState.stateID.name.Equals(name))
        {
            return PopCurState();
        }
        return false;
    }

    public bool Update(params T[] parameters)
    {
        if (States.TryPeek(out var currentState))
        {
            return currentState.Update(parameters);
        }
        return false;
    }
}