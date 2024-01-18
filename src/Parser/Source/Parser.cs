namespace Parser;

using System.Text;
using RtfModels;

public static class Parser
{
    private static readonly int EOF = -1;
    public static Group ParseIntoGroups(ref StreamReader strm)
    {
        Stack<Group> groups = new();
        groups.Push(new Group());
        char prev, ch;

        for(int next; (next = strm.Read()) != EOF; prev = ch)
        {
            ch = (char)next;

            if (ch == '\\')
            {
                ControlWord word = ParseControlWord(ref strm);
                groups.Peek().Children.Add(word);
            }
            else if (ch == '{')
            {
                groups.Push(new Group());
            }
            else if (ch == '}')
            {
                if (groups.Count <= 1)
                {
                    throw new FormatException("Group Underflow");
                }

                Group last = groups.Pop();
                groups.Peek().Children.Add(last);
            }
            else if (char.IsWhiteSpace(ch))
            {
                // Pass on CR, LF, and other whitespace characters.
                continue;
            }
            else
            {
                groups.Peek().Children.Add(ParseText(ref strm, ch));
            }
        }

        if (groups.Count > 1)
        {
            throw new FormatException("Group Overflow");
        }

        return groups.Pop();
    }

    /// <summary>
    /// ParseControlWord parses out the next control word in the stream.
    /// See <i>Microsoft®️ Office Rich Text Format (RTF) Specification Version 1.9.1</i>, page 11, 143
    /// </summary>
    /// <param name="strm">The <see cref="StreamReader"/> containing the RTF text.</param>
    /// <returns>The parsed <see cref="ControlWord"/>.</returns>
    /// <exception cref="FormatException">Thrown if the control word does not follow the RTF spec.</exception>
    public static ControlWord ParseControlWord(ref StreamReader strm)
    {
        StringBuilder builder = new();
        StringBuilder param = new();

        // Control word format (generally):
        // r"\\[A-Za-z]+(-?[0-9]+)?"
        for (int next; (next = strm.Peek()) != EOF; strm.Read())
        {
            char ch = (char)next;

            // If the character is a special character.
            if (IsSpecialCharacter(ch))
            {
                if (builder.Length != 0)
                {
                    // Break if another control word or group delimiter is found.
                    if (ch == '\\' || ch == '{' || ch == '}' || char.IsWhiteSpace(ch)) break;

                    // Deal with the minus char special case later.
                    if (ch != '-')
                    {
                        // Otherwise, throw a format error.
                        throw new FormatException($"invalid character: {ch}, built value: {builder.ToString()}");
                    }
                }

                // Hex value control word.
                if (ch == '\'')
                {
                    // Consume the current token
                    strm.Read();

                    // Consume the hex value tokens.
                    for(int i = 0; i < 2; i++)
                    {
                        char val = (char)strm.Peek();
                        if (char.IsAsciiHexDigit(val)) {
                            builder.Append(val);
                            if (i == 0) strm.Read();
                        }
                        else
                        {
                            throw new FormatException();
                        }
                    }
                }
                else if (ch == '-')
                {
                    if (builder.Length == 0)
                    {
                        builder.Append('-');
                    }
                    else if (param.Length == 0)
                    {
                        param.Append('-');
                    }
                    else
                    {
                        throw new FormatException();
                    }
                }
                else
                {
                    builder.Append(ch);
                }
            }
            // Add characters to control word
            else if (char.IsLetter(ch))
            {
                if (param.Length > 0)
                {
                    throw new FormatException();
                }

                builder.Append(ch);
            }
            // Add values to parameter.
            else if (char.IsDigit(ch))
            {
                if (builder.Length == 0)
                {
                    throw new FormatException();
                }

                param.Append(ch);
            }
            else
            {
                break;
            }
        }

        if (builder.Length == 0)
        {
            throw new FormatException();
        }

        return ControlWordMapper.GetControlWord(builder.ToString(), param.Length > 0 ? param.ToString() : null);
    }

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
            if (IsControlDelimiter(ch)) break;

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

    private static bool IsControlDelimiter(char ch)
    {
        return ch == '\\' || ch == '{' || ch == '}' || ch == '\r' || ch == '\n';
    }
}