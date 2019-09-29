using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CommandProcessor : MonoBehaviour
{
    Stack<ICommand> commands=new Stack<ICommand>();

    public void ExecuteCommand(ICommand command){
        commands.Push(command);
        command.Execute();
        Debug.Log("Command Stack:"+commands.Count);
    }
    public void Undo()
    {
        if (commands.Count <= 0)
            return;
        ICommand command = commands.Pop();
        command.Undo();
        Debug.Log("Command Stack:"+commands.Count);
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
