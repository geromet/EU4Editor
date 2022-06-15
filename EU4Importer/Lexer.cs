using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Eu4Importer
{
    internal static class Lexer
    {
        private static string ignoredChar = "()/:;<>?^~%*+@";
        private static StringBuilder stringBuilder = new StringBuilder();
        public static Token[] Tokenize(string input)
        {
            bool comment = false;
            bool quote = false;
            List<Token> tokens = new List<Token>();
            for (int i = 0; i < input.Length; i++)
            {
                switch (input[i])
                {
                    case ' ':
                        {
                            if (stringBuilder.Length > 0)
                            {
                                ContentToken contentToken = new ContentToken(stringBuilder.ToString());
                                stringBuilder.Clear();
                                tokens.Add(contentToken);
                            }
                            break;
                        }
                    case '=':
                        tokens.Add(new EqualsToken());
                        break;
                    case '{':
                        tokens.Add(new OpenBracketsToken());
                        break;
                    case '}':
                        tokens.Add(new CloseBracketsToken());
                        break;
                    case '"':
                        quote = !quote;
                        break;
                    case '#':
                        if (!quote)
                        {
                            comment = true;
                        }
                        break ;
                    case '\n':
                        comment = false;
                        if (stringBuilder.Length > 0)
                        {
                            ContentToken contentToken = new ContentToken(stringBuilder.ToString());
                            stringBuilder.Clear();
                            tokens.Add(contentToken);
                        }
                        break;
                    case '\r':
                        comment = false;
                        if (stringBuilder.Length > 0)
                        {
                            ContentToken contentToken = new ContentToken(stringBuilder.ToString());
                            stringBuilder.Clear();
                            tokens.Add(contentToken);
                        }
                        break;
                    case '\t':
                        comment = false;
                        if (stringBuilder.Length > 0)
                        {
                            ContentToken contentToken = new ContentToken(stringBuilder.ToString());
                            stringBuilder.Clear();
                            tokens.Add(contentToken);
                        }
                        break;
                    default:
                        if (ignoredChar.Contains(input[i]))
                        {
                            break;
                        }
                        else
                        {
                            if (!comment)
                            {
                                stringBuilder.Append(input[i]);
                            }
                            break;
                        }
                }
            }
            return tokens.ToArray();
        }
    }
}
