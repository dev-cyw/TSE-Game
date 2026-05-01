using System;
using System.Collections.Generic;
using TMPro;
using UnityEditor.PackageManager;
using UnityEngine;

public class SyntaxHighlighter : MonoBehaviour
{
    public TMP_InputField editField;
    public TMP_Text displayText;

    private static readonly Dictionary<string, string> CommandColours = new()
    {
        { "move_right", "#FFFFFF" },
        { "jump", "#123456" }
    };

    private static readonly string argColour = "#EF9A9A";
    private static readonly string punctColour = "#EF9A9A";
    private static readonly string errorColour = "#EF9A9A";

    private void Start()
    {
        editField.onValueChanged.AddListener(Highlight);
        //editField.textComponent.color = Color.clear;
        Highlight(editField.text);
    }

    private void Highlight(string raw)
    {
        print("Highlight called: " + raw);
        var stringBuild = new System.Text.StringBuilder();
        string[] lines = raw.Split(';');

        foreach (var line in lines) { 
            stringBuild.Append(HighlightLine(line));
        }

        displayText.text = stringBuild.ToString();
    }

    private string HighlightLine(string line)
    {
        if (string.IsNullOrWhiteSpace(line)) return line;

        string trimmed = line.Trim();

        int parenOpen = trimmed.IndexOf('(');
        int parenClose = trimmed.IndexOf(')');

        string cmdName = parenOpen >= 0
            ? trimmed[..parenOpen].Trim()
            : trimmed.Trim();

        bool known = CommandColours.TryGetValue(cmdName, out string cmdColour);
        cmdColour ??= errorColour;

        if (parenOpen < 0)
            return $"<color={cmdColour}>{cmdName}</color>";

        string arg = parenClose > parenOpen
            ? trimmed[(parenOpen + 1)..parenClose].Trim()
            : string.Empty;


        return $"<color={cmdColour}>{cmdName}</color>"
             + $"<color={punctColour}>(</color>"
             + $"<color={argColour}>{arg}</color>"
             + $"<color={punctColour}>)</color>";
    }
}
