﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace Eu4Importer
{
    internal class Process
    {
        public static void GetOuterEntities(string path, Program program)
        {
            string[] lines = System.IO.File.ReadAllLines(path);
            GameFile file = new GameFile() { Path = path };
            StringBuilder sb = new StringBuilder();
            int currentPos = 0;
            int lastPos = 0;
            int brackets = 0;
            bool inBrackets = false;
            string st = RemoveComments(lines);
            OuterEntity entity = new OuterEntity();
            foreach (char c in st)
            {
                if (c == '{')
                {
                    inBrackets = true;
                    brackets++;
                }
                else if (c == '}')
                {
                    brackets--;
                }
                if (brackets == 0 && inBrackets)//End Outer
                {
                    sb.Clear();
                    inBrackets = false;
                    for (int i = lastPos; i <= currentPos; i++)
                    {
                        if (st[i] != '\n')
                        {
                            sb.Append(st[i]);
                        }
                    }
                    file.UnprocessedOuterEntites.Add(sb.ToString());
                    program.gameFiles.Add(file);
                    lastPos = currentPos + 1;
                }
                currentPos++;
            }
        }
        public static void ProcessOuterEntities(Program program)
        {
            foreach (GameFile file in program.gameFiles)
            {
                foreach (string UnprocessedOuterEntity in file.UnprocessedOuterEntites)
                {
                    file.OuterEntities.Add(ProcessOuterEntity(UnprocessedOuterEntity));
                }
                file.UnprocessedOuterEntites.Clear();
            }
        }
        public static void ProcessInnerEntities(Program program)
        {
            foreach (GameFile gameFile in program.gameFiles)
            {
                foreach (OuterEntity outerEntity in gameFile.OuterEntities)
                {
                    foreach (string unprocessedInnerEntity in outerEntity.UnprocessedInnerEntities)
                    {
                        InnerEntity innerEntity = ProcessInnerEntity(unprocessedInnerEntity);
                        outerEntity.InnerEntities.Add(innerEntity);
                    }
                    outerEntity.UnprocessedInnerEntities.Clear();
                }
            }
        }
        private static InnerEntity ProcessInnerEntity(string UnprocessedInnerEntity, bool WaitingForValue = false)
        {
            InnerEntity processedInnerEntity = new();
            int currentPos = 0;
            int lastPos = 0;
            int brackets = 0;
            bool inBrackets = false;
            bool waitingForValue = WaitingForValue;
            StringBuilder sb = new StringBuilder();
            foreach (char c in UnprocessedInnerEntity)
            {
               
                if (c == '{')//Brackets opening
                {
                    inBrackets = true;
                    brackets++;
                }
                else if (c == '}')//Brackets closing
                {
                    brackets--;
                }
                else if (c == '=')//Name behind, Value or NestedEntity in front
                {
                    for (int i = lastPos; i <= currentPos; i++)
                    {
                        sb.Append(UnprocessedInnerEntity[i]);
                    }
                    processedInnerEntity.Name = sb.ToString();
                    sb.Clear();
                    lastPos = currentPos + 1;
                    waitingForValue = true;
                    if (inBrackets)//In a nested bracket, find InnerEntity
                    {
                        processedInnerEntity.InnerEntities.Add(ProcessInnerEntity(UnprocessedInnerEntity.Substring(currentPos + 1), waitingForValue));//Search everything in front
                    }
                }
                else if (c<47 | c > 123)//Not text
                {
                    if (waitingForValue)//After =
                    {
                        for (int i = lastPos; i <= currentPos; i++)
                        {
                            if (c > 47 & c < 123)//Text
                            {
                                waitingForValue = false;
                                sb.Append(UnprocessedInnerEntity[i]); 
                            }
                        }
                        if (!waitingForValue)//Has value
                        {
                            processedInnerEntity.Value = sb.ToString();
                            sb.Clear();
                        }
                    }
                }
                if (brackets == 0 && inBrackets)//Brackets closed
                {
                    inBrackets = false;
                    currentPos++;
                    break;
                }
                currentPos++;
            }
            return processedInnerEntity;
        }
        private static string RemoveComments(string[] lines)
        {
            var sb = new StringBuilder();
            string currentLine;
            foreach (string line in lines)
            {
                if (line.Contains("#"))
                {
                    currentLine = line.Split("#")[0];
                    currentLine = Regex.Replace(currentLine, @"\n", "");
                    sb.Append(currentLine.ToString());
                }
                else
                {
                    currentLine = line;
                    currentLine = Regex.Replace(currentLine, @"\n", "");
                    sb.Append(currentLine + '\n');
                }
            }
            string st = sb.ToString();
            return st;
        }  
        private static OuterEntity ProcessOuterEntity(string UnprocessedOuterEntity)
        {
            OuterEntity processedOuterEntity = new();
            int currentPos = 0;
            int lastPos = 0;
            int brackets = 0;
            bool inBrackets = false;
            StringBuilder sb = new StringBuilder();
            foreach (char c in UnprocessedOuterEntity)
            {
                if (c == '{')
                {
                    if (processedOuterEntity.Name == "")
                    {
                        for (int i = lastPos; i <= currentPos; i++)
                        {
                            if (UnprocessedOuterEntity[i] != '\n')
                            {
                                sb.Append(UnprocessedOuterEntity[i]);
                            }
                        }
                        processedOuterEntity.Name = sb.ToString();
                        sb.Clear();
                        lastPos = currentPos + 1;
                    }
                    inBrackets = true;
                    brackets++;
                }
                else if (c == '}')
                {
                    brackets--;
                }
                if (brackets == 0 && inBrackets)
                {
                    inBrackets = false;
                    for (int i = lastPos; i <= currentPos; i++)
                    {
                        sb.Append(UnprocessedOuterEntity[i]);
                    }
                    processedOuterEntity.UnprocessedInnerEntities.Add(sb.ToString());
                    sb.Clear();
                    lastPos = currentPos + 1;
                }
                currentPos++;
            }
            return processedOuterEntity;
        }

    }
}
