using System.Collections.Generic;
using System.Text.RegularExpressions;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class Compiler : MonoBehaviour
{
    public TMP_InputField inputField;

    /*
     * Valid Commands
     * - move_right(10)
     * - Move Left
     * - Jump ?
     * - 
     * 
     */

    void Start()
    {

    }

    void Update()
    {

    }

    public void ParseCommands()
    {
        string[] lines = inputField.text.Split(';');
        List<string> commands = new List<string>();

        foreach (string line in lines)
        {
            string trimmed = line.Trim();
            if (string.IsNullOrEmpty(trimmed)) continue;

            if (!ValidateCommand(trimmed))     
            {
                print($"Aborting at: \"{trimmed}\"");
                return;
            }

            commands.Add(trimmed);
        }

        foreach (string command in commands)
            print($"Executing: {command}");
    }

    private static readonly Dictionary<string, bool> ValidCommands = new Dictionary<string, bool>
    {
        { "move_right", true },
        { "move_left", true },
        { "jump", false }
    };

    private bool ValidateCommand(string command)
    {
        // requires brackets always, only allows integers
        Match m = Regex.Match(command, @"^(\w+)\(\s*(-?\d+)?\s*\)$");
        if (!m.Success)
        {
            print($"Syntax error: \"{command}\" — did you forget brackets?");
            return false;
        }

        string name = m.Groups[1].Value;
        bool hasArg = m.Groups[2].Success;

        if (!ValidCommands.TryGetValue(name, out bool needsArg))
        {
            print($"Unknown command: \"{name}\"");
            return false;
        }

        if (needsArg != hasArg)
        {
            print(needsArg
                ? $"{name} requires a number e.g. {name}(10)"
                : $"{name} takes no arguments e.g. {name}()");
            return false;
        }

        return true;
    }
}