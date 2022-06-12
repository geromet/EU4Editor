using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Eu4Importer
{
    internal static class Lexer
    {
        private static StringBuilder stringBuilder = new StringBuilder();
        public static Token[] Tokenize(string input)
        {
            List<Token> tokens = new List<Token>();
            for (int i = 0; i < input.Length; i++)
            {
                tokens.Add(ReadChar(input[i]));
            }
            return tokens.ToArray();
        }
        private static Token ReadChar(char input)
        {
            switch (input)
            {
                case '=':
                    return new EqualsToken();
                case '{':
                    return new OpenBracketsToken();
                case '}':
                    return new CloseBracketsToken();
                default:
                    if (input > 44 & input < 123)
                    {
                        stringBuilder.Append(input);
                        return new EmptyToken();
                    }
                    else
                    {
                        if (stringBuilder.Length > 0)
                        {
                            ContentToken contentToken = new ContentToken(stringBuilder.ToString());
                            stringBuilder.Clear();
                            return contentToken;
                        }
                        else
                        {
                            return new EmptyToken();
                        }
                    }

            }
        }
    }
}
