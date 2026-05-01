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
    MissingArguement,
    UnexpectedArguement,
    Syntax
}

public class Compiler : MonoBehaviour
{
    public TMP_InputField inputField;
    public PlayerMovement2D playerMovement;
    private List<Command> ValidCommands;

    private void Awake()
    {
        ValidCommands = new List<Command>
        {
            new Command("move_right", true, move_right),
            new Command("jump", false, jump)
        };
    }

    private void move_right(string arg)
    {
        playerMovement.MoveRight(arg);
        print("Move right " + arg);
    }

    private void jump(string arg)
    {
        print("JUMP!");
    }

    public void ParseCommands()
    {
        print("Parsing Commands...");
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
                case CommandResult.MissingArguement:
                    print("Missing Arguement"); break;
                case CommandResult.UnexpectedArguement: 
                    print("Unexpected Arguement"); break;
                case CommandResult.Syntax:
                    print("Syntax Error"); break;
            }
        }

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
                if (cmd.HasArgs && string.IsNullOrEmpty(arg)) return CommandResult.MissingArguement;
                if (!cmd.HasArgs && !string.IsNullOrEmpty(arg)) return CommandResult.UnexpectedArguement;
                matched = cmd;
                return CommandResult.Success;
            }
        }
        return CommandResult.Syntax;
    }
}