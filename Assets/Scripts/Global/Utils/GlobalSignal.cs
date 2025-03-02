using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class GlobalSignal
{
    // USE LAMBDAS FOR METHODS WITH ARGUMENTS
    private List<UnityAction> connections = new ();


    public void connect(UnityAction action){
        if(connections.Contains(action)) return;
        connections.Add(action);
    }

    public void disconnect(UnityAction action){
        if(!connections.Contains(action)) return;
        connections.Remove(action);
    }

    public void emit(){
        foreach(UnityAction action in connections){
            action.Invoke();
        }
    }
}
