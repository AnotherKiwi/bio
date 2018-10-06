using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Globalization;
using System.Reflection;
using System.Text.RegularExpressions;

namespace Bio.Util.ArgumentParser
{
    /// <summary>
    /// Command Line Argument value types.
    /// </summary>
    public enum ArgumentValueType
    {
        /// <summary>
        /// String type argument.
        /// </summary>
        String,

        /// <summary>
        /// String type argument.
        /// </summary>
        OptionalString,

        /// <summary>
        /// Integer type argument.
        /// </summary>
        Int,

        /// <summary>
        /// Integer type data. Used for optional parameter.
        /// </summary>
        OptionalInt,
        
        /// <summary>
        /// Boolean type argument.
        /// </summary>
        Bool,

        /// <summary>
        /// Inidicates that integer value may be specified more than once.
        /// Only valid if the argument is a collection.
        /// </summary>
        MultipleInts,

        /// <summary>
        /// Inidicates that string value may be specified more than once.
        /// If duplicate values are found an exception is raised.
        /// </summary>
        MultipleUniqueStrings
    }

    /// <summary>
    /// Used to control parsing of command line arguments. 
    /// </summary>
    public enum ArgumentType
    {
        /// <summary>
        /// Indicates that argument is not required.
        /// </summary>
        Optional,

        /// <summary>
        /// Indicates that the argument is mandatory.
        /// </summary>
        Required,

        /// <summary>
        /// Indicates the arugment is the default. If the parameter name is not found this param
        /// </summary>
        DefaultArgument
    }

    /// <summary>
    /// This class parses all the command line arguments.
    /// </summary>
    public class CommandLineArguments : IEnumerable, IEnumerator
    {
        /// <summary>
        /// All the parameters and values passed from commandline are stored after parsing.
        /// </summary>
        private StringDictionary parsedArguments;

        /// <summary>
        /// The Command Line enumerator.
        /// </summary>
        private IEnumerator enumerator;

        /// <summary>
        /// Contains the mapping between parameter name and the alias (shortName).
        /// </summary>
        private Dictionary<string, string> paramNameAliasMap = new Dictionary<string, string>();

        /// <summary>
        /// The target object to which the command line arguments are to be set.
        /// </summary>
        private object targetObject;

        /// <summary>
        /// All the required and optional parameters from commandline are stored.
        /// </summary>
        private SortedDictionary<string, Argument> argumentList;

        /// <summary>
        /// Initializes a new instance of the CommandLineArguments class.
        /// </summary>
        public CommandLineArguments()
        {
            parsedArguments = new StringDictionary();
            argumentList = new SortedDictionary<string, Argument>(StringComparer.OrdinalIgnoreCase);
            enumerator = parsedArguments.GetEnumerator();

            ArgumentSeparator = "-";
            AllowAdditionalArguments = false;
        }

        /// <summary>
        /// Gets or sets a value indicating whether additional arguments are reqiured or not.
        /// </summary>
        public bool AllowAdditionalArguments { get; set; }

        /// <summary>
        /// Gets or sets the argument separator character.
        /// </summary>
        public string ArgumentSeparator { get; set; }

        /// <summary>
        /// Gets the current found parameter from enumerator.
        /// </summary>
        public DictionaryEntry Current
        {
            get
            {
                return ((DictionaryEntry)enumerator.Current);
            }
        }

        /// <summary>
        /// Gets current value from enumerator.
        /// </summary>
        object IEnumerator.Current
        {
            get
            {
                return enumerator.Current;
            }
        }

        /// <summary>
        /// Defines the argument that the commandline utility supports.
        /// </summary>
        /// <param name="argType">Argument type.</param>
        /// <param name="parameterName">Name of the parameter.</param>
        /// <param name="argValueType">Defines the argument value type (int or string or bool) for the argument.</param>
        /// <param name="shortName">ShortName or alias for the argument.</param>
        /// <param name="helpDesc">Description of the argument.</param>
        public void Parameter(ArgumentType argType, string parameterName, ArgumentValueType argValueType, string shortName, string helpDesc)
        {
            // For the first value without parameter name only type string is accepted.
            if (string.IsNullOrEmpty(parameterName) && argValueType != ArgumentValueType.String)
            {
                throw new Exception("For the first value (without parameter name) only type ValueType.String is accepted! ");
            }

            var param = new Argument(parameterName, argType, argValueType, shortName, helpDesc);
            argumentList.Add(param.Name, param);
            if (!string.IsNullOrEmpty(param.ShortName))
            {
                paramNameAliasMap.Add(shortName, parameterName);
            }
        }

        /// <summary>
        /// Parses the command line arguments passed from the utility.
        /// </summary>
        /// <param name="arguments">Arguments to be parsed.</param>
        /// <param name="destination">Resulting parsed arguments.</param>
        public void Parse(string[] arguments, object destination)
        {
            if (arguments == null)
            {
                throw new ArgumentNullException(nameof(arguments));
            }

            if (destination == null)
            {
                throw new ArgumentNullException(nameof(destination));
            }

            targetObject = destination;
            var defaultArgs = string.Empty;
            var defaultArgValues = string.Empty;
            
            // get the default parameter name
            foreach (var arg in argumentList)
            {
                if (arg.Value.AllowType == ArgumentType.DefaultArgument)
                {
                    defaultArgs = string.Concat(ArgumentSeparator, arg.Value.Name, "=");
                    break;
                }
            }

            var args = string.Empty;
            foreach (var s in arguments)
            {
                string val;
                if (s.StartsWith(ArgumentSeparator, StringComparison.OrdinalIgnoreCase) )
                {
                    var endIndex = s.IndexOfAny(new char[] { ':', '=' }, 1);
                    var parameter = s.Substring(1, endIndex == -1 ? s.Length - 1 : endIndex - 1);
                    string paramVal;
                    if (parameter.Length + 1 == s.Length)
                    {
                        paramVal = null;
                    }
                    else if (s.Length > 1 + parameter.Length && (s[1 + parameter.Length] == ':' || s[1 + parameter.Length] == '='))
                    {
                        paramVal = s.Substring(parameter.Length + 2);
                    }
                    else
                    {
                        paramVal = s.Substring(parameter.Length + 1);
                    }

                    val = ArgumentSeparator + parameter;
                    var encodedValue = EncodeValue(paramVal);
                    if (!string.IsNullOrEmpty(encodedValue))
                    {
                        val = string.Concat(val, "=", encodedValue);
                    }

                    args += val + " ";
                }
                else 
                {
                    if (!string.IsNullOrEmpty(defaultArgs))
                    {
                        defaultArgValues = string.Concat(defaultArgValues, EncodeValue(s), " ");
                    }
                    else
                    {
                        throw new ArgumentParserException(string.Format(CultureInfo.CurrentCulture, "Could not associate any parameter for the value {0}", s));
                    }
                }
            }

            if (!string.IsNullOrEmpty(defaultArgValues) && !string.IsNullOrEmpty(defaultArgs))
            {
                args = string.Concat(args, defaultArgs, defaultArgValues, " ");
            }

            // parse the arguments
            Parse(args);

            // assign the parsed values to the target object properties.
            AssignTargetObjectProperties();
        }
        
        /// <summary>
        /// Returns a enumerator which walks through the dictionary of found parameters.
        /// </summary>
        /// <returns>Enumerator of dictionary of found parameters.</returns>
        public IEnumerator GetEnumerator()
        {
            enumerator = parsedArguments.GetEnumerator();
            return enumerator;
        }

        /// <summary>
        /// Sets the enumerator to the next found parameter.
        /// </summary>
        /// <returns>true if there is a next found parameter, else false.</returns>
        public bool MoveNext()
        {
            return enumerator.MoveNext();
        }
       
        /// <summary>
        /// Resets the enumerator to the initial position in front of the first found parameter.
        /// </summary>
        public void Reset()
        {
            enumerator.Reset();
        }

        /// <summary>
        /// Returns the position of the mismatch of arguments.
        /// </summary>
        /// <param name="regExpr">Regular expression used for matching.</param>
        /// <param name="srchExpr">Expression to be searched.</param>
        /// <returns>Returns the character position where there is a mismatch.</returns>
        private static int GetMismatchPosition(string regExpr, string srchExpr)
        {
            // split the regular expression using opening parenthesis and respective closing parenthesis.
            var validateParenthesis = new SortedDictionary<int, int>();
            var openParenthesis = new Stack<int>();
            try
            {
                for (var i = 0; i < regExpr.Length; i++)
                {
                    if (regExpr[i] == '(')
                    {
                        // Make sure that this ( is not escaped!
                        if (!((i == 1 && regExpr[i - 1] == '\\') ||
                               (i > 1 && regExpr[i - 1] == '\\' && regExpr[i - 2] != '\\')))
                        {
                            openParenthesis.Push(i);
                        }
                    }
                    else if (regExpr[i] == ')')
                    {
                        // Make sure that this ) is not escaped!
                        if (!((i == 1 && regExpr[i - 1] == '\\') ||
                               (i > 1 && regExpr[i - 1] == '\\' && regExpr[i - 2] != '\\')))
                        {
                            var pop = openParenthesis.Pop();
                            validateParenthesis.Add(pop, i);
                        }
                    }
                }

                // In a ideal situation this should not happen.
                if (openParenthesis.Count != 0)
                {
                    throw new Exception("Error in regular expression, parenthesis are not balanced");
                }
            }
            catch (Exception)
            {
                // since RegEx should be valid, this can never happen.
                throw new Exception("Internal Exception: Parentesis not balanced!");
            }

            // Parenthesis contains all parenthesis matches ordered by the position of the opening parenthesis
            IEnumerator e = validateParenthesis.GetEnumerator();
            var prevCorrectPosition = 0;
            while (e.MoveNext())
            {
                var c = (KeyValuePair<int, int>)e.Current;

                // Get sub-regular-expression of parenthesis grouping.
                var subRegEx = regExpr.Substring(c.Key, c.Value - c.Key + 1);
                Regex sub = null;
                try
                {
                    sub = new Regex(subRegEx);
                }
                catch (Exception)
                {
                    // This should never happen since subexpression of a valid regex should still be valid.
                    throw new Exception("Internal Exception: SubRegEx invalid: " + subRegEx.ToString());
                }

                var m = sub.Match(srchExpr);
                if (m.Success == true)
                {
                    // If there is a match this subexpression matches the SearchStr and the mismatch must
                    // follow afterwards.
                    // find the end position of the match and increase LastCorrectPosition count to that position.
                    // (warning: here the wrong match might be detected,
                    // but since its is unlikely that commandline argument contains several identical parts,
                    // this potential problem is ignored.)
                    var newLastCorrectPosition = srchExpr.IndexOf(m.Value, StringComparison.OrdinalIgnoreCase) + m.Value.Length;
                    if (newLastCorrectPosition > prevCorrectPosition)
                    {
                        prevCorrectPosition = newLastCorrectPosition;
                    }
                }
            }

            return prevCorrectPosition;
        }

        /// <summary>
        /// Encode Value.
        /// </summary>
        /// <param name="value">The Value.</param>
        /// <returns>The Encoded Value.</returns>
        private static string EncodeValue(string value)
        {
            var encodedVal = value;
            if (string.IsNullOrEmpty(value))
            {
                encodedVal = string.Empty;
            }
            else
            {
                if (value.StartsWith("-", StringComparison.OrdinalIgnoreCase))
                {
                    encodedVal = "\"" + value + "\"";
                }
                else if (value.Contains("-"))
                {
                    encodedVal = value.Replace("-", ">");
                }
                else
                {
                    // if the parmeter value has space encode it with "|" and decode it while setting the value
                    encodedVal = value.Trim().Replace(" ", "|");
                }
            }

            return encodedVal;
        }

        /// <summary>
        /// Parse Value.
        /// </summary>
        /// <param name="type">The Type of value.</param>
        /// <param name="stringData">String Data.</param>
        /// <param name="value">The object Value.</param>
        /// <returns>True if value parsed.</returns>
        private static bool ParseValue(Type type, string stringData, out object value)
        {
            // null is only valid for bool variables
            // empty string is never valid
            if ((stringData != null || type == typeof(bool)) && (stringData == null || stringData.Length > 0))
            {
                try
                {
                    if (type == typeof(string))
                    {
                        var tempVal = stringData;
                        tempVal = (tempVal.IndexOf(">", StringComparison.OrdinalIgnoreCase) > -1) ? tempVal.Replace(">", "-") : tempVal;
                        tempVal = (tempVal.IndexOf("|", StringComparison.OrdinalIgnoreCase) > -1) ? stringData.Replace("|", " ") : tempVal;
                        value = tempVal;
                        return true;
                    }
                    else if (type == typeof(bool))
                    {
                        if (stringData == null || stringData == "+")
                        {
                            value = true;
                            return true;
                        }
                        else if (stringData == "-")
                        {
                            value = false;
                            return true;
                        }
                    }
                    else if (type == typeof(int))
                    {
                        value = int.Parse(stringData, CultureInfo.CurrentCulture);
                        return true;
                    }
                    else if (type == typeof(uint))
                    {
                        value = int.Parse(stringData, CultureInfo.CurrentCulture);
                        return true;
                    }
                    else if (type == typeof(double))
                    {
                        value = double.Parse(stringData, CultureInfo.CurrentCulture);
                        return true;
                    }
                    else if (type == typeof(float))
                    {
                        value = float.Parse(stringData, CultureInfo.CurrentCulture);
                        return true;
                    }
                    else
                    {
                        var valid = false;
                        foreach (var name in Enum.GetNames(type))
                        {
                            if (name == stringData)
                            {
                                valid = true;
                                break;
                            }
                        }

                        if (valid)
                        {
                            value = Enum.Parse(type, stringData, true);
                            return true;
                        }
                    }
                }
                catch
                {
                    // catch parse errors
                }
            }

            value = null;
            return false;
        }
        
        /// <summary>
        /// Assign Target Object Properties.
        /// </summary>
        private void AssignTargetObjectProperties()
        {
            object value;
            foreach (var field in targetObject.GetType().GetFields())
            {
                if (parsedArguments.ContainsKey(field.Name))
                {
                    if (field.FieldType.IsArray)
                    {
                        var arg = argumentList[field.Name];
                        var valList = new ArrayList();

                        // Add the data to a array list and set it to field.
                        var spaceExpr = new Regex("[\\s]+");
                        foreach (var val in spaceExpr.Split(parsedArguments[field.Name]))
                        {
                            if (ParseValue(field.FieldType.GetElementType(), val, out value))
                            {
                                if (arg.AllowType == ArgumentType.DefaultArgument && valList.Contains(value))
                                {
                                    throw new DuplicateArgumentValueException(
                                        string.Format(
                                        CultureInfo.CurrentCulture,
                                        "Duplicate values are passed to the parameter {0}", 
                                        field.Name));
                                }

                                valList.Add(value);
                            }
                        }

                        field.SetValue(targetObject, valList.ToArray(field.FieldType.GetElementType()));
                    }
                    else
                    {
                        if (ParseValue(field.FieldType, parsedArguments[field.Name], out value))
                        {
                            field.SetValue(targetObject, value);
                        }
                    }
                }
            }
        }

        /// <summary>
        /// Parses the command line arguments passed from the utility.
        /// </summary>
        /// <exception cref="ArgumentParserException">Thrown when any error is found during parsing.</exception>
        /// <param name="arguments">Arguments passed via command line utility.</param>
        private void Parse(string arguments)
        {
            // regular expression to split the arguments.
            var parserExpression = "^([\\s]*)([-](?<name>[^\\s-/:=]+)([:=]?)([\\s]*)(?<value>(\"[^\"]*\")|('[^']*')|([\\s]*[^/-][^\\s]+[\\s]*)|([^/-]+)|)?([\\s]*))*$";

            var ro = new RegexOptions();
            ro = ro | RegexOptions.IgnoreCase;
            ro = ro | RegexOptions.Multiline;
            var cmdLineParseExpr = new Regex(parserExpression, ro);

            var m = cmdLineParseExpr.Match(arguments.ToString());
            if (m.Success == false)
            {
                // Regular expression did not match arguments. 
                var lastCorrectPosition = GetMismatchPosition(parserExpression, arguments);
                var errorExpr = arguments.Substring(lastCorrectPosition);
                throw new ArgumentSyntaxException(
                    string.Format(
                    CultureInfo.CurrentCulture,
                    "{0}{1}{2}{3}{4}{5}",
                    Properties.Resource.CmdLineParserException,
                    Properties.Resource.CmdLineParserExceptionSyntaxError,
                    arguments,
                    Properties.Resource.CmdLineParserExceptionSyntaxError2,
                    errorExpr,
                    Properties.Resource.CmdLineParserExceptionSyntaxError3));
            }
            else
            {
                // No issues with the syntax.
                var unknownGroupValues = m.Groups["unknownvalues"];
                if (!string.IsNullOrEmpty(unknownGroupValues.Value))
                {
                    var unknownParamValue = unknownGroupValues.Value.Trim();
                    var quotesExpr = new Regex("^(\".*\")|('.*')$");
                    var e = quotesExpr.Match(unknownParamValue);
                    if (e.Length != 0)
                    {
                        unknownParamValue = unknownParamValue.Substring(1, unknownParamValue.Length - 2);
                    }

                    AddParsedParameter(string.Empty, unknownParamValue);
                }

                var param_grp = m.Groups["name"];
                var value_grp = m.Groups["value"];
                if (param_grp == null || value_grp == null)
                {
                    // this should never happen.
                    throw new Exception("Internal Exception: Commandline parameter(s) incorrect.");
                }

                // RegEx find always pairs of name- and value-group. their count should thus always match.
                if (param_grp.Captures.Count != value_grp.Captures.Count)
                {
                    throw new Exception("Internal Exception: Number of parameters and number of values is not equal. This should never happen.");
                }

                // add each parameter and the respective value.
                for (var i = 0; i < param_grp.Captures.Count; i++)
                {
                    // if there are spaces at either side of value or param, trim those.
                    var value = value_grp.Captures[i].ToString().Trim();
                    var param = param_grp.Captures[i].ToString().Trim();
                    var quoteExpr = new Regex("^(\".*\")|('.*')$");
                    var e = quoteExpr.Match(value);
                    if (e.Length != 0)
                    {
                        value = value.Substring(1, value.Length - 2);
                    }

                    // if alias is passed get the actual name and add it to parsed list
                    if (paramNameAliasMap.ContainsKey(param))
                    {
                        param = paramNameAliasMap[param];
                    }

                    AddParsedParameter(param, value);
                }
            }

            CheckRequiredParameters();
        }

        /// <summary>
        /// Adds the parsed parameter and the value to the parsed argument list.
        /// </summary>
        /// <param name="paramName">The new parameter which is to be added to FoundParameters.</param>
        /// <param name="paramValue">Value which corresponds to NewParam.</param>
        private void AddParsedParameter(string paramName, string paramValue)
        {
            if (paramName == null)
            {
                throw new ArgumentNullException(nameof(paramName));
            }

            if (paramValue == null)
            {
                throw new ArgumentNullException(nameof(paramValue));
            }

            if (string.IsNullOrEmpty(paramName) && !argumentList.ContainsKey(paramName) && AllowAdditionalArguments == false)
            {
                var message = string.Concat(
                    Properties.Resource.CmdLineParserException,
                    Properties.Resource.CmdLineParserExceptionValueWithoutParameterFound,
                    paramValue,
                    Properties.Resource.CmdLineParserExceptionValueWithoutParameterFound2);
                throw new ArgumentNullValueException(message);
            }

            // if /? is passed set help parameter to true
            if (paramName.Equals("?", StringComparison.OrdinalIgnoreCase))
            {
                paramName = "help";
            }

            if (!argumentList.ContainsKey(paramName) && AllowAdditionalArguments == false)
            {
                var message = string.Concat(
                    Properties.Resource.CmdLineParserException,
                    Properties.Resource.CmdLineParserExceptionUnknownParameterFound,
                    paramName,
                    Properties.Resource.CmdLineParserExceptionUnknownParameterFound2);
                throw new ArgumentNotFoundException(message);
            }
            else if (!argumentList.ContainsKey(paramName) && AllowAdditionalArguments == true)
            {
                parsedArguments.Add(paramName, paramValue);
            }
            else if (argumentList.ContainsKey(paramName))
            {
                // the parameter is available, check for each ValueType.
                switch (argumentList[paramName].ValueType)
                {
                    // boolean parameters do not accept any value.
                    case ArgumentValueType.Bool:
                        if (string.IsNullOrEmpty(paramValue))
                        {
                            paramValue = "+";
                        }

                        break;
                    case ArgumentValueType.OptionalInt:
                    case ArgumentValueType.Int:
                        if (string.IsNullOrEmpty(paramValue))
                        {
                            paramValue = "0";
                        }

                        object val;
                        var intField = targetObject.GetType().GetField(paramName);
                        if (!ParseValue(intField.FieldType, paramValue, out val))
                        {
                            var message = string.Concat(
                                Properties.Resource.CmdLineParserException,
                                Properties.Resource.CmdLineParserExceptionInvalidValueFound,
                                paramName,
                                Properties.Resource.CmdLineParserExceptionInvalidValueFoundInt);
                            throw new InvalidArgumentValueException(message);
                        }

                        break;
                    case ArgumentValueType.MultipleInts:
                        // split the value and add it.
                        var field = targetObject.GetType().GetField(paramName);
                        var multiValueSeparator = new Regex("[\\s]+");
                        foreach (var value in multiValueSeparator.Split(paramValue))
                        {
                            object fieldVal;
                            if (!ParseValue(field.FieldType.GetElementType(), value, out fieldVal))
                            {
                                var message = string.Concat(
                                        Properties.Resource.CmdLineParserException,
                                        Properties.Resource.CmdLineParserExceptionInvalidValueFound,
                                        paramName,
                                        Properties.Resource.CmdLineParserExceptionInvalidValueFoundInts);
                                throw new InvalidArgumentValueException(message);
                            }
                        }

                        break;
                    case ArgumentValueType.String:
                    case ArgumentValueType.MultipleUniqueStrings:
                        if (string.IsNullOrEmpty(paramValue))
                        {
                            var message = string.Concat(
                                Properties.Resource.CmdLineParserException,
                                Properties.Resource.CmdLineParserExceptionInvalidValueFound,
                                paramName,
                                Properties.Resource.CmdLineParserExceptionInvalidValueFoundString);
                            throw new InvalidArgumentValueException(message);
                        }

                        break;
                    case ArgumentValueType.OptionalString:
                        break;
                    default: throw new Exception("Internal Exception: Unmatch case in AddNewFoundParameter()!");
                }

                if (parsedArguments.ContainsKey(paramName))
                {
                    var message = string.Concat(
                        Properties.Resource.CmdLineParserException,
                        Properties.Resource.CmdLineParserExceptionRepeatedParameterFound,
                        paramName,
                        Properties.Resource.CmdLineParserExceptionRepeatedParameterFoundOnce);
                    throw new ArgumentRepeatedException(message);

                }
                else
                {
                    parsedArguments.Add(paramName, paramValue);
                }
            }
        }

        /// <summary>
        /// Check if the values for required parameters are passed or not.
        /// </summary>
        private void CheckRequiredParameters()
        {
            if (parsedArguments.ContainsKey("help"))
            {
                return;
            }

            foreach (var argument in argumentList)
            {
                if ((argument.Value.AllowType == ArgumentType.Required || argument.Value.AllowType == ArgumentType.DefaultArgument) 
                    && (!parsedArguments.ContainsKey(argument.Key)))
                {
                    if (string.IsNullOrEmpty(argument.Key))
                    {
                        var message = string.Concat(
                            Properties.Resource.CmdLineParserException,
                            Properties.Resource.CmdLineParserExceptionRequiredFirstParameterMissing);
                        throw new RequiredArgumentMissingException(message);
                    }
                    else
                    {
                        var message = string.Concat(
                            Properties.Resource.CmdLineParserException,
                            Properties.Resource.CmdLineParserExceptionRequiredParameterMissing,
                            argument.Key,
                            Properties.Resource.CmdLineParserExceptionRequiredParameterMissing2);
                        throw new RequiredArgumentMissingException(message);
                    }
                }
            }
        }

        /// <summary>
        /// This class saves the details of a single argument.
        /// </summary>
        private class Argument
        {
            public Argument(string parameterName, ArgumentType allowType, ArgumentValueType valueType, string shortName, string parameterHelp)
            {
                Name = parameterName;
                AllowType = allowType;
                ValueType = valueType;
                Help = parameterHelp;
                ShortName = shortName;
            }

            public string Name { get; set; }
            public ArgumentType AllowType { get; set; }
            public ArgumentValueType ValueType { get; set; }
            public string Help { get; set; }
            public string ShortName { get; set; }
        }
    }
}
