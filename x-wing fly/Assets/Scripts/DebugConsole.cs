using System.Collections.Generic;
using UnityEngine;

public class DebugConsole : MonoBehaviour
{
    static List<string> logMessages;
    static List<string> LogMessages
    {
        get
        {
            if (logMessages == null)
            {
                logMessages = new List<string>();
            }
            return logMessages;

        }
        set
        {
            logMessages = value;
        }
    }

    static string log = "";
    static int height;
    static int width;
    int buttonSize;

    const int sizeLimit = 18;
    private void OnEnable()
    {
        if (Application.platform == RuntimePlatform.Android)
            buttonSize = 100;
        else
            buttonSize = 30;

    }

    public static void Open()
    {
        DebugConsole.height = 0;
        DebugConsole.width = 0;
    }

    public static void Log(string text, bool clear = false)
    {
        if (clear)
        {
            LogMessages.Clear();
        }
        LogMessages.Add(text);
        if (LogMessages.Count > sizeLimit)
            LogMessages.RemoveAt(0);

        ConvertIntoString();
    }

    private static void ConvertIntoString()
    {
        log = "";

        for (int i = 0; i < LogMessages.Count; i++)
        {
            log += LogMessages[i] + "\n";
        }
    }

    private void OnGUI()
    {
        GUI.TextArea(new Rect(10, 10, width, height), log, 5000);
        if (GUI.Button(new Rect(0, 0, buttonSize, buttonSize), "-"))
        {
            if (height == 0)
            {
                height = 300;
                width = 500;
            }
            else
            {
                height = 0;
                width = 0;

            }
        }
    }
}
