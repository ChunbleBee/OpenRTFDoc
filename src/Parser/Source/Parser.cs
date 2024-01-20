namespace Parser;

using System.Text;
using RtfModels;

public static class Parser
{
    private static readonly int EOF = -1;

    /// <summary>
    /// Parse attempts to parse the given string into <see cref="ControlWord"/> nodes.
    /// </summary>
    /// <param name="str">the string to parse.</param>
    /// <returns>The document root <see cref="Group"/> node.</returns>
    public static Group Parse(string str)
    {
        return Parse(new MemoryStream(Encoding.ASCII.GetBytes(str)));
    }

    /// <summary>
    /// Parse attempts to parse the given stream into <see cref="ControlWord"/> nodes.
    /// </summary>
    /// <param name="strm">The <see cref="Stream"/> to read from.</param>
    /// <returns>The document root <see cref="Group"/> node.</returns>
    public static Group Parse(Stream strm)
    {
        return Parse(new StreamReader(strm));
    }

    /// <summary>
    /// Parse attempts to parse the given stream into <see cref="ControlWord"/> nodes.
    /// </summary>
    /// <param name="strm">The <see cref="StreamReader"/> to read from.</param>
    /// <returns>The document root <see cref="Group"/> node.</returns>
    public static Group Parse(StreamReader strm)
    {
        return ParseIntoGroups(ref strm);
    }

    /// <summary>
    /// ParseIntoGroups parses the given stream into group tokens.
    /// </summary>
    /// <param name="strm">The <see cref="StreamReader"/> to consume.</param>
    /// <returns>The <see cref="Group"/> that forms the document.</returns>
    /// <exception cref="FormatException">Thrown if either there are group mismatches, or characters out of bounds.</exception>
    public static Group ParseIntoGroups(ref StreamReader strm)
    {
        Stack<Group> groups = new();
        Group? doc = null;
        char prev, ch;

        for (int next; (next = strm.Read()) != EOF; prev = ch)
        {
            ch = (char)next;

            if (ch == '\\')
            {
                ControlWord word = ParseControlWord(ref strm);
                if (!groups.TryPeek(out _))
                {
                    throw new FormatException("Tokens outside document bounds.");
                }
                groups.Peek().Children.Add(word);
                if (groups.Peek().Type == GroupType.Default && word is DestinationWord dWord)
                {
                    groups.Peek().Type = dWord.IsGlobal ? GroupType.Global : GroupType.Local;
                }
            }
            else if (ch == '{')
            {
                groups.Push(new Group());
            }
            else if (ch == '}')
            {
                if (groups.Count == 0)
                {
                    throw new FormatException("Group Underflow");
                }

                Group last = groups.Pop();
                if (groups.Count > 0)
                {
                    groups.Peek().Children.Add(last);
                }
                else
                {
                    if (doc != null)
                    {
                        throw new FormatException("Multiple Document Groups");
                    }

                    doc = last;
                }
            }
            else if (char.IsWhiteSpace(ch))
            {
                // Pass on CR, LF, and other whitespace characters.
                continue;
            }
            else
            {
                if (!groups.TryPeek(out _))
                {
                    throw new FormatException("Tokens outside document bounds.");
                }

                groups.Peek().Children.Add(ParseText(ref strm, ch));
            }
        }

        if (groups.Count > 0 || doc == null)
        {
            throw new FormatException("Group Overflow");
        }

        return doc;
    }

    /// <summary>
    /// ParseControlWord parses out the next control word in the stream.
    /// See <i>Microsoft®️ Office Rich Text Format (RTF) Specification Version 1.9.1</i>, page 11, 143
    /// </summary>
    /// <param name="strm">The <see cref="StreamReader"/> to consume.</param>
    /// <returns>The parsed <see cref="ControlWord"/>.</returns>
    /// <exception cref="FormatException">Thrown if the control word does not follow the RTF spec.</exception>
    public static ControlWord ParseControlWord(ref StreamReader strm)
    {
        StringBuilder ctrl = new();
        StringBuilder param = new();

        // Control Symbols
        if (strm.Peek() != EOF && IsSpecialCharacter((char)strm.Peek()))
        {
            ctrl.Append((char)strm.Read());
        }
        else
        {
            for (int next; (next = strm.Peek()) != EOF; strm.Read())
            {
                char ch = (char)next;
                if (IsControlWordDelimiter(ch)) break;
                ctrl.Append(ch);
            }
        }

        if (strm.Peek() >= 0 && strm.Peek() == ' ')
            strm.Read();

        for (int next; (next = strm.Peek()) != EOF; strm.Read())
        {
            char ch = (char)next;
            if (ch == '-' && param.Length == 0)
            {
                // Append the character, and short-circuit.
                param.Append(ch);
                continue;
            }

            if (!char.IsAsciiDigit(ch)) break;
            param.Append(ch);
        }

        return ControlWordMapper.GetControlWord(ctrl.ToString(), param.Length > 0 ? param.ToString() : null);
    }

    /// <summary>
    /// ParseText parses out the next run of plain text from the stream.
    /// </summary>
    /// <param name="strm">The <see cref="StreamReader"/> to consume.</param>
    /// <param name="start">Optional character to append to the builder.</param>
    /// <returns>The <see cref="Run"/> of plain text.</returns>
    public static Run ParseText(ref StreamReader strm, char? start = null)
    {
        StringBuilder builder = new();
        if (start != null)
        {
            builder.Append(start);
        }

        for (int next; (next = strm.Peek()) != EOF; strm.Read())
        {
            char ch = (char)next;
            if (IsControlWordBoundary(ch)) break;

            builder.Append(ch);
        }

        return new Run(builder.ToString());
    }

    private static bool IsSpecialCharacter(char ch)
    {
        return ch switch
        {
            '\'' => true,
            '-' => true,
            '*' => true,
            ':' => true,
            '\\' => true,
            '_' => true,
            '{' => true,
            '|' => true,
            '}' => true,
            '~' => true,
            '\r' => true,
            '\n' => true,
            _ => false
        };
    }

    private static bool IsControlWordBoundary(char ch)
    {
        return ch == '\\' || ch == '{' || ch == '}' || ch == '\r' || ch == '\n';
    }

    private static bool IsControlWordDelimiter(char ch)
    {
        return char.IsWhiteSpace(ch) || ch == '-' || char.IsDigit(ch) || !char.IsAsciiLetterOrDigit(ch);
    }
}