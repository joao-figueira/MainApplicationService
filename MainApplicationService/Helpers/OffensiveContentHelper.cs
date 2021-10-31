using System;
using System.Collections.Generic;
using System.Linq;

namespace MainApplicationService.Helpers
{
    public static class OffensiveContentHelper
    {
        private static IEnumerable<string>? _offensiveExpressionsList;

        public static void SetOffensiveExpressions(string offensiveExpressionsAsString)
        {
            
            _offensiveExpressionsList = offensiveExpressionsAsString.Split(";", StringSplitOptions.RemoveEmptyEntries);
        }

        public static bool ContainsOffensiveExpressions(string text, out List<string> offensiveExpressionsFound)
        {
            text = text.ToLower();
            offensiveExpressionsFound = new List<string>();
            if (_offensiveExpressionsList == null || !_offensiveExpressionsList.Any())
                return false;
            foreach (var expression in _offensiveExpressionsList)
            {
                if (text.Contains(expression.ToLower())) //ToDO: compare performance between regular expression and string.contains
                {
                    offensiveExpressionsFound.Add(expression);
                    return true;
                }
            }
            return false;
        }
    }
}
