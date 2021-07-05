using System;
using System.Collections.Generic;
using System.Linq;

namespace Assets.Scripts.LoadingSystems.Editor.SceneClassesGenerations
{
    public abstract class EnumFileGenerator : FileGenerator
    {
        protected readonly Dictionary<string, int> EnumMembers = new Dictionary<string, int>();

        protected abstract string EnumName { get; }
        
        public void Append(string enumMemberName, int enumMemberValue)
        {
            if (EnumMembers.ContainsKey(enumMemberName))
            {
                int definedValue = EnumMembers[enumMemberName];
                if (definedValue == enumMemberValue)
                {
                    return;
                }

                throw new InvalidOperationException($"{EnumName} named '{enumMemberName}' was provided with value '{enumMemberValue}', " +
                                                    $"but it is already defined with value '{enumMemberValue}'. ");
            }

            if (EnumMembers.ContainsValue(enumMemberValue))
            {
                var conflictingType = EnumMembers.First(kvp => kvp.Value == enumMemberValue);
                throw new InvalidOperationException($"{EnumName} named '{enumMemberName}' was provided with value '{enumMemberValue}', " +
                                                    $"but this value is already used by '{conflictingType.Key}'.");
            }

            EnumMembers.Add(enumMemberName, enumMemberValue);
        }
    }
}