using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class Command
{
    public string Name { get; }
    public bool HasArgs { get; }
    public Action<string> Execute { get; }

    public Command(string name, bool hasArgs, Action<string> execute)
    {
        Name = name;
        HasArgs = hasArgs;
        Execute = execute;
    }
}

public enum CommandResult { 
    Success,
    MissingArgument,
    UnexpectedArgument,
    Syntax
}

public class Compiler : MonoBehaviour
{
    public TMP_InputField inputField;
    public PlayerMovement2D playerMovement;
    public Button hideBTN;
    private List<Command> ValidCommands;

    private void Awake()
    {
        ValidCommands = new List<Command>
        {
            new Command("move_right", true, playerMovement.MoveRight),
            new Command("move_left", true, playerMovement.MoveLeft),
            new Command("jump", false, playerMovement.Jump),
            new Command("crouch", true, playerMovement.Crouch)
        };
    }

    public void ParseCommands()
    {
        string[] lines = inputField.text.Split(';');
        Queue<(Command cmd, string arg)> commandQueue = new();

        foreach (string line in lines)
        {
            string trimmed = line.Trim();
            
            if(string.IsNullOrEmpty(trimmed)) { continue; }

            var result = ValidateCommand(trimmed, out Command cmd, out string arg);
            switch (result)
            {
                case CommandResult.Success: commandQueue.Enqueue((cmd, arg)); break;
                case CommandResult.MissingArgument:
                    print("Missing Argument"); break;
                case CommandResult.UnexpectedArgument: 
                    print("Unexpected Arguement"); break;
                case CommandResult.Syntax:
                    print("Syntax Error"); break;
            }
        }

        inputField.gameObject.SetActive(false);
        //hideBTN.gameObject.SetActive(false);

        while (commandQueue.Count > 0)
        {
            var (cmd, arg) = commandQueue.Dequeue();
            cmd.Execute(arg);
        }
    }

    private CommandResult ValidateCommand(string input, out Command matched, out string arg)
    {
        matched = null;
        arg = string.Empty;

        int parenOpen = input.IndexOf('(');
        int parenClose = input.IndexOf(')');

        string commandName = parenOpen >= 0
            ? input[..parenOpen].Trim()
            : input.Trim();

        if (parenOpen >= 0 && parenClose > parenOpen)
        {
            arg = input[(parenOpen + 1)..parenClose].Trim();
        }
        foreach (Command cmd in ValidCommands)
        {
            if (cmd.Name == commandName)
            {
                if (cmd.HasArgs && string.IsNullOrEmpty(arg)) return CommandResult.MissingArgument;
                if (!cmd.HasArgs && !string.IsNullOrEmpty(arg)) return CommandResult.UnexpectedArgument;
                matched = cmd;
                return CommandResult.Success;
            }
        }
        return CommandResult.Syntax;
    }
}