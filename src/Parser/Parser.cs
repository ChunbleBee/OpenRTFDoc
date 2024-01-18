namespace Parser;

using System.Text;
using RtfModels;

public class Parser
{
    private static readonly int EOF = -1;
    public static Group ParseIntoGroups(StreamReader strm)
    {
        Stack<Group> groups = new();
        Group? document = null;
        char prev, ch;

        for(int next; (next = strm.Read()) != EOF; prev = ch)
        {
            ch = (char)next;

            if (ch == '\\')
            {
                ControlWord word = ParseControlWord(strm);
                groups.Peek().Children.Add(word);
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
                if (groups.Count == 0)
                {
                    document = last;
                }
                else
                {
                    groups.Peek().Children.Add(last);
                }

            }
            else if (ch == '\r' || ch == '\n')
            {
                // Pass on CRLF
                continue;
            }
            else
            {

                groups.Peek().Children.Add(new Run()
                {
                    InnerText = ParseText(strm)
                });
            }
        }

        if (groups.Count > 0)
        {
            throw new FormatException("Group Overflow");
        }

        return document!;
    }

    /// <summary>
    /// ParseControlWord parses out the next control word in the stream.
    /// See <i>Microsoft®️ Office Rich Text Format (RTF) Specification Version 1.9.1</i>, page 11, 143
    /// </summary>
    /// <param name="strm">The <see cref="StreamReader"/> containing the RTF text.</param>
    /// <returns>The parsed <see cref="ControlWord"/>.</returns>
    /// <exception cref="FormatException">Thrown if the control word does not follow the RTF spec.</exception>
    public static ControlWord ParseControlWord(StreamReader strm)
    {
        StringBuilder builder = new();
        StringBuilder param = new();

        char ch;

        // Control word format (generally):
        // r"\\[A-Za-z]+(-?[0-9]+)?"
        for (int next; (next = strm.Peek()) != EOF; strm.Read())
        {
            ch = (char)next;

            // If the character is a special character.
            if (IsSpecialCharacter(ch))
            {
                if (builder.Length != 0)
                {
                    // Break if another control word is found.
                    if (ch == '\\') break;

                    // Deal with the minus char special case later.
                    if (ch != '-')
                    {
                        // Otherwise, throw a format error.
                        throw new FormatException();
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
                // CR/LF special case.
                else if (ch == '\r' || ch == '\n')
                {
                    builder.Append("par");
                }
                // Minus/- special case.
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

        return ControlWordMapper.GetControlWord(builder.ToString());
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

    public static string ParseText(StreamReader strm)
    {
        throw new NotImplementedException();
    }
}