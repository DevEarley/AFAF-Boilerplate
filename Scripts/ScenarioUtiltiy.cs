using System;
using System.Collections.Generic;
using System.Linq;

    public static class ScenarioUtility
    {
        public static int GetIndex(string markerKey, string[] lines)
        {
            var newIndex = Array.FindIndex(lines, possibleTargetLine =>
                    {
                        if (possibleTargetLine.StartsWith("\\\\"))
                        {
                            var otherMarkerKey = GetRestOfLine(possibleTargetLine, "\\\\");
                            if (otherMarkerKey == markerKey)
                            {
                                return true;
                            }
                        }
                        return false;
                    });
            return newIndex;
        }

        public static string Reverse(string s)
        {
            char[] charArray = s.ToCharArray();
            Array.Reverse(charArray);
            return new string(charArray);
        }

        public static string GetOuterExpression(string line, int firstTick, int lastTick)
        {
            return line.Substring(firstTick + 1, lastTick - firstTick - 1);
        }

        public static string GetRestOfLine(string line, string lastSymbol)
        {
            var indexEnd = line.IndexOf(lastSymbol) + lastSymbol.Length;
            var restOfLine = line.Substring(indexEnd, line.Length - indexEnd);
            return restOfLine;
        }

        public static string GetBefore(string line, string lastSymbol)
        {
            var indexEnd = line.IndexOf(lastSymbol);
            var restOfLine = line.Substring(0, indexEnd);
            return restOfLine;
        }

        public static int GetLastCharacterIndex(string line, string character)
        {
            return line.Length - ScenarioUtility.Reverse(line).IndexOf(character) - 1;
        }

        public static string GetSubstring(string line, string start, string end)
        {
            if (line.IndexOf(end) == -1) return line;
            var indexStart = line.IndexOf(start) + 1;
            var subStringAfterStart = line.Substring(indexStart, line.Length - indexStart);
            var indexEnd = subStringAfterStart.IndexOf(end) + indexStart;
            var sub = line.Substring(indexStart, indexEnd - indexStart);
            return sub;
        }

        public static string GetSubstring(string line, int indexStart, string end)
        {
            if (line.IndexOf(end) == -1) return line;
            var subStringAfterStart = line.Substring(indexStart, line.Length - indexStart);
            var indexEnd = subStringAfterStart.IndexOf(end) + indexStart;
            var sub = line.Substring(indexStart, indexEnd - indexStart);
            return sub;
        }

        //public static string EscapeSymbolsInLiteralStrings(string line)
        //{
        //    var literals = GetOuterExpressionsFromLine(line, '`');
        //    line = ReplaceAllSymbolsWithOddAsciiCharacters(line, literals);
        //    return line;
        //}

        //private static string ReplaceAllSymbolsWithOddAsciiCharacters(string line, List<string> literals)
        //{
        //    for (var i = 0; i < literals.Count; i++)
        //    {
        //        var literal = literals[i];
        //        var originalOuterLiteral = literal;
        //        var processedLiteral = literal.Replace("?", "♠♠")
        //                       .Replace("|", "♣♣")
        //                       .Replace("+", "♥♥")
        //                       .Replace("-", "♦♦")
        //                       .Replace("[", "♪♪")
        //                       .Replace("]", "♫♫")
        //                       .Replace("=", "▬▬")
        //                       .Replace("/", "▲▲")
        //                       .Replace(@"\", "◄◄")
        //                       .Replace("*", "██")
        //                       .Replace(",", "☻☻")
        //                       .Replace(".", "●●")
        //                       .Replace("%", "☼☼")
        //                       .Replace("`", "‴‴")
        //                       .Replace("<", "┌┌")
        //                       .Replace(">", "┐┐");
        //        line = line.Replace("`" + originalOuterLiteral + "`", processedLiteral);
        //    }
        //    return line;
        //}

        //public static string UnescapeSymbolsInLiteralStrings(string line)
        //{
        //    return line.Replace("♠♠", "?")
        //                .Replace("♣♣", "|")
        //                .Replace("♥♥", "+")
        //                .Replace("♦♦", "-")
        //                .Replace("♪♪", "[")
        //                .Replace("♫♫", "]")
        //                .Replace("▬▬", "=")
        //                .Replace("▲▲", "/")
        //                .Replace("◄◄", @"\")
        //                .Replace("██", "*")
        //                .Replace("☻☻", ",")
        //                .Replace("●●", ".")
        //                .Replace("☼☼", "%")
        //                .Replace("‴‴", "`")
        //                .Replace("┌┌", "<")
        //                .Replace("┐┐", ">");
        //}

        public static List<string> GetOuterExpressionsFromLine(string line, char specialCharacter)
        {
            var outerOpenIndexes = new List<int>();
            var outerClosedIndexes = new List<int>();
            var isOpened = false;
            for (var i = 0; i < line.Length; i++)
            {
                var character = line[i];
                if (character == specialCharacter && isOpened == false)
                {
                    isOpened = true;
                    outerOpenIndexes.Add(i);
                }
                else if (character == specialCharacter && isOpened == true)
                {
                    isOpened = false;
                    outerClosedIndexes.Add(i);
                }
            }
            var combined = outerClosedIndexes.Concat(outerOpenIndexes).OrderBy(x => x).ToArray();
            var expressions = new List<string>();
            for (var i = 0; i < combined.Count(); i += 2)
            {
                var expression = ScenarioUtility.GetOuterExpression(line, combined[i], combined[i + 1]);
                expressions.Add(expression);
            }
            return expressions;
        }

        public static string ReplaceHandledExpression(string line, bool firstExpressionIsVariableKey, string processedExpression, string originalOuterExpression)
        {
            if (firstExpressionIsVariableKey)
            {
                line = line.Replace("[" + originalOuterExpression + "]", "[" + processedExpression + "]");
            }
            else
            {
                processedExpression = processedExpression.Replace("[", "");
                processedExpression = processedExpression.Replace("]", "");
                line = line.Replace("[" + originalOuterExpression + "]", processedExpression);
            }
            return line;
        }

        public static List<string> GetOuterExpressionsFromLine(string line, char openCharacter, char closeCharacter)
        {
            var openCounter = 0;
            var outerOpenIndexes = new List<int>();
            var outerClosedIndexes = new List<int>();
            for (var i = 0; i < line.Length; i++)
            {
                var character = line[i];
                if (character == openCharacter)
                {
                    openCounter++;
                    if (openCounter == 1)
                    {
                        outerOpenIndexes.Add(i);
                    }
                }
                else if (character == closeCharacter)
                {
                    openCounter--;
                    if (openCounter == 0)
                    {
                        outerClosedIndexes.Add(i);
                    }
                }
            }
            var combined = outerClosedIndexes.Concat(outerOpenIndexes).OrderBy(x => x).ToArray();
            var expressions = new List<string>();
            for (var i = 0; i < combined.Count(); i += 2)
            {
                var expression = ScenarioUtility.GetOuterExpression(line, combined[i], combined[i + 1]);
                expressions.Add(expression);
            }
            return expressions;
        }

 

        
    }
