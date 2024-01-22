using System.Text.RegularExpressions;

namespace CasparLauncher.CasparCG;

public class LogLine
{
    public string Data { get; private set; } = "";
    public string Message { get; private set; } = "";
    public LogLevel Level { get; private set; } = LogLevel._info;
    public string Timestamp { get; private set; } = "";
    public bool DirectOutput { get; private set; } = true;
    private static readonly Regex find = new(@"^\[([0-9]{4}-[0-9]{2}-[0-9]{2}\s[0-9]{2}:[0-9]{2}:[0-9]{2}\.[0-9]+)\]\s\[(.+?)\]\s*(.*)");

    public LogLine(string data, bool serverFormat)
    {
        Data = data;
        if (string.IsNullOrEmpty(data) || !serverFormat) return;
        bool has_content = data.Length > 36;

        string to_match = has_content ? data[..36] : data;
        Match linedata = find.Match(to_match);
        if (linedata.Groups.Count > 1)
        {
            Timestamp = linedata.Groups[1].Value;
            Level = GetLevel(linedata.Groups[2].Value);
            Message = has_content ? data[36..] : "";
            DirectOutput = false;
        }
    }

    public static LogLevel GetLevel(string value)
    {
        var level = LogLevel._info;
        switch (value)
        {
            case "fatal":
                level = LogLevel._fatal;
                break;
            case "error":
                level = LogLevel._error;
                break;
            case "warning":
                level = LogLevel._warning;
                break;
            case "info":
                level = LogLevel._info;
                break;
            case "debug":
                level = LogLevel._debug;
                break;
            case "trace":
                level = LogLevel._trace;
                break;
        }
        return level;
    }
}
