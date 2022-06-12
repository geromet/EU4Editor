using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Eu4Importer
{
    internal class Parser
    {
        public Node[] ParseString(string input)
        {
            Console.ForegroundColor = ConsoleColor.Gray;
            return ParseContent(ParseInCompleteContent(Lexer.Tokenize(input)));
        }

        private Token[] ParseInCompleteContent(Token[] tokens)
        {
            ContentToken inCompleteToken = new();
            bool equals = false;
            List<Token> output = new();
            for (int i = 0; i < tokens.Length; i++)
            {
                switch (tokens[i])
                {
                    case ContentToken:
                        if (equals)
                        {
                            ValueToken valueToken = new ValueToken(inCompleteToken.content, tokens[i].content);
                            equals = false;
                            output.Add(valueToken);
                        }
                        else
                        {
                            inCompleteToken = (ContentToken)tokens[i];
                        }
                        break;
                    case EqualsToken:
                        equals = true;
                        break;
                    case OpenBracketsToken:
                        if (equals)
                        {
                            output.Add(inCompleteToken);
                            equals = false;
                        }
                        break;
                    case CloseBracketsToken:
                        output.Add(tokens[i]);
                        break;
                    default:
                        break;
                }
            }
            Console.ForegroundColor = ConsoleColor.Red;
            Console.WriteLine("Parsed InCompleteContent");
            Console.ForegroundColor = ConsoleColor.Gray;
            foreach (Token token in output)
            {
                Console.WriteLine(token.GetType() + "     " + token.content + "     " + token.value);
            }
            return output.ToArray();

        }
        private Node[] ParseContent(Token[] tokens)
        {
            Stack<Node> nodes = new();
            Node currentNode = new();
            Node parentNode = new();
            List<Node> output = new();
            Console.ForegroundColor = ConsoleColor.Red;
            Console.WriteLine("Parsing Content");
            Console.ForegroundColor = ConsoleColor.Gray;
            for (int i = 0; i < tokens.Length; i++)
            {
                switch (tokens[i])
                {
                    case ContentToken:
                        currentNode = new();
                        currentNode.Name = tokens[i].content;
                        nodes.Push(currentNode);
                        Console.WriteLine("ContentToken     " + currentNode.Name);
                        break;
                    case ValueToken:
                        currentNode = nodes.Pop();
                        currentNode.Nodes.Push(new Node(tokens[i].content, tokens[i].value));
                        nodes.Push(currentNode);
                        Console.WriteLine("ValueToken       " + tokens[i].content + "       " + tokens[i].value);
                        break;
                    case CloseBracketsToken:
                        currentNode = nodes.Pop();
                        if (nodes.TryPeek(out _))
                        {
                            Console.WriteLine("Close");
                            parentNode = nodes.Pop();
                            parentNode.Nodes.Push(currentNode);
                            nodes.Push(parentNode);
                        }
                        else
                        {
                            Console.WriteLine("End");
                            output.Add(currentNode);
                        }
                        break;
                    default: break;
                }
            }
            Console.ForegroundColor = ConsoleColor.Red;
            Console.WriteLine("Parsed Content");
            Console.ForegroundColor = ConsoleColor.Gray;
            foreach (Node node in output)
            {
                Console.WriteLine(node.Name);
                foreach (Node childNode in node.Nodes)
                {
                    Console.Write(childNode.Name);
                    if (childNode.Value != null)
                    {
                        Console.Write("     " + childNode.Value);
                    }
                    else
                    {
                        Console.Write("\n");
                    }
                    if (childNode.Nodes.Count > 0)
                    {
                        foreach (Node nestedNode in childNode.Nodes)
                        {
                            Console.Write(nestedNode.Name);
                            if (nestedNode.Value != null)
                            {
                                Console.Write("     " + nestedNode.Value);
                            }
                            Console.Write("\n");
                        }
                    }
                    Console.Write("\n");
                }
            }
            return output.ToArray();
        }
    }
}
