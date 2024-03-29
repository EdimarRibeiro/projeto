﻿using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Text;

namespace ProjectManager.Domain.Utils.Expressions.Internal.Nodes
{
    internal class ArrayList : Node
    {
        public string DrawValue { get; set; }

        public List<string> StringValues { get; private set; }

        public Type Type { get; set; }

        public override Expression Build(BuildArgument arg)
        {
            ParseValues();
            var arr = Array.CreateInstance(Type, StringValues.Count);
            var i = 0;
            StringValues.ForEach((p) =>
            {
                var value = p.Cast(Type);
                arr.SetValue(value, i++);
            });

            return Expression.Constant(arr);
        }

        public void ParseValues()
        {
            StringValues = new List<string>();
            if (string.IsNullOrWhiteSpace(DrawValue)) return;

            StringBuilder value = null;
            var isSpecialChar = false;
            var started = false;
            char openChar = char.MinValue;
            var whiteSpaceCount = 0;
            while (DrawValue[whiteSpaceCount].IsWhiteSpace()) ++whiteSpaceCount;
            var drawValue = DrawValue.Trim();
            StartIndex += whiteSpaceCount;
            var dotCount = 0;

            for (var index = 0; index < drawValue.Length; ++index, ++StartIndex)
            {
                var c = drawValue[index];
                if (value == null) value = new StringBuilder();
                if (!started)
                {
                    dotCount = 0;
                    started = true;
                    if (c == '"' || c == '\'')
                    {
                        openChar = c;
                        continue;
                    }
                    else
                    {
                        openChar = char.MinValue;
                    }
                }

                if (!isSpecialChar && c == '\\')
                {
                    isSpecialChar = true;
                    continue;
                }

                if (isSpecialChar)
                {
                    isSpecialChar = false;
                    value.Append(c == '"' || c == '\'' ? c : '\\');
                    continue;
                }

                if (c == openChar)
                {
                    openChar = char.MinValue;
                    started = false;
                    StringValues.Add(value.ToString());
                    value = null;
                    IgnoreWhiteSpaceAndComma(drawValue, ref index);
                    continue;
                }

                if (c == ',')
                {
                    if (openChar != char.MinValue)
                    {
                        value.Append(c);
                    }
                    else
                    {
                        started = false;
                        StringValues.Add(value.ToString().Trim());
                        value = null;
                        IgnoreWhiteSpaceAndComma(drawValue, ref index);
                    }

                    continue;
                }

                if (openChar == char.MinValue)
                {
                    if (c == '.')
                    {
                        ++dotCount;
                        if (dotCount > 1) throw new FormatException(GetErrorMessage());
                    }

                    else if (!char.IsDigit(c)) throw new FormatException(GetErrorMessage());
                }

                value.Append(c);
            }

            if (value != null)
            {
                StringValues.Add(value.ToString().Trim());
            }
        }

        private void IgnoreWhiteSpaceAndComma(string str, ref int index)
        {
            if (index == str.Length - 1) return;
            ++index;
            ++StartIndex;
            var c = str[index];
            while (index < (str.Length - 1) && (c.IsWhiteSpace() || c == ','))
            {
                ++StartIndex;
                ++index;
                c = str[index];
            }

            --StartIndex;
            --index;
        }
    }
}
