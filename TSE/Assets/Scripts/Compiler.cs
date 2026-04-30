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



public class Compiler : MonoBehaviour
{
    public TMP_InputField inputField;
    private static readonly List<Command> ValidCommands = new()
    {
        new Command("move_right", true, move_right),
        new Command("jump", false, jump)
    };
    private static void move_right(string arg)
    {
        print("Move right " + arg);
    }

    private static void jump(string arg)
    {
        print("JUMP!");
    }

    public void ParseCommands()
    {
        string[] lines = inputField.text.Split(';');
        Queue<(Command cmd, string arg)> commandQueue = new();

        foreach (string line in lines)
        {
            string trimmed = line.Trim();
            
            if(string.IsNullOrEmpty(trimmed)) { continue; }
            
            if (ValidateCommand(trimmed, out Command cmd, out string arg))
            {
                commandQueue.Enqueue((cmd, arg));
            }
            else
            {
                Debug.LogWarning("Error command not valid");
            }
        }

        while (commandQueue.Count > 0)
        {
            var (cmd, arg) = commandQueue.Dequeue();
            cmd.Execute(arg);
        }
    }

    private bool ValidateCommand(string input, out Command matched, out string arg)
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
                if (cmd.HasArgs && string.IsNullOrEmpty(arg)) return false;
                if (!cmd.HasArgs && !string.IsNullOrEmpty(arg)) return false;
                matched = cmd;
                return true;
            }
        }
        return false;
    }
}