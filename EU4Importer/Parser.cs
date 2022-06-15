using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Eu4Importer
{
    internal class Parser
    {
        public Node[] ParsePath(string path)
        {
            Console.ForegroundColor = ConsoleColor.Gray;
            return ParseContent(ParseInCompleteContent(Lexer.Tokenize(File.ReadAllText(path))),path);
        }

        public void Test(string path)
        {
            foreach (Token token in ParseInCompleteContent(Lexer.Tokenize(File.ReadAllText(path))))
            {
                Console.WriteLine(token.GetType() +" " + token.content + " " + token.value);
            }
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
                            inCompleteToken = new();
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
                            ContainerToken containerToken = new(inCompleteToken.content);
                            output.Add(containerToken);
                            inCompleteToken = new();
                            equals = false;
                        }
                        break;
                    case CloseBracketsToken:
                        output.Add(tokens[i]);
                        break;
                    default:
                        if (inCompleteToken.content != null)
                        {
                            output.Add(inCompleteToken);
                        }
                        break;
                }
            }
            Console.ForegroundColor = ConsoleColor.Red;
            Console.WriteLine("Parsed InCompleteContent");
            Console.ForegroundColor = ConsoleColor.Gray;
            return output.ToArray();

        }
        private Node[] ParseContent(Token[] tokens, string path)
        {
            Node fileNode = new(path);
            Stack<Node> nodes = new();
            Node currentNode = new();
            Node parentNode = new();
            List<Node> output = new();
            nodes.Push(fileNode);
            Console.ForegroundColor = ConsoleColor.Red;
            Console.WriteLine("Parsing Content");
            Console.ForegroundColor = ConsoleColor.Gray;
            Console.WriteLine(nodes.Peek().Name);
            for (int i = 0; i < tokens.Length; i++)
            {
                Console.WriteLine("Received : " + tokens[i].GetType());
                switch (tokens[i])
                {
                    case ContainerToken: 
                        currentNode = new();
                        currentNode.Name = tokens[i].content;
                        nodes.Push(currentNode);
                        break;
                    case ValueToken:
                        currentNode = nodes.Pop();
                        currentNode.Nodes.Push(new Node(tokens[i].content, tokens[i].value));
                        nodes.Push(currentNode);
                        break;
                    case CloseBracketsToken:
                        if(nodes.TryPeek(out _))
                        {
                            currentNode = nodes.Pop();
                            if (nodes.TryPeek(out _))
                            {
                                parentNode = nodes.Pop();
                                parentNode.Nodes.Push(currentNode);
                                nodes.Push(parentNode);
                            }
                            else
                            {
                                output.Add(currentNode);
                            }
                        }
                        
                        break;
                    default: break;
                }
            }
            if(nodes.TryPeek(out _))
            {
                currentNode = nodes.Pop();
                if(nodes.TryPeek(out _))
                {
                    parentNode = nodes.Pop();
                    parentNode.Nodes.Push(currentNode);
                    output.Add(parentNode);
                }
                else
                {
                    output.Add(currentNode);
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
            Console.WriteLine("Created Nodes");
            return output.ToArray();
        }
    }
}
