using SharedData.Models;
using System;
using System.Collections.Generic;
using System.Text.Json;

namespace BE_TravelDestinations.DataPart
{
    public static class DeserializationValidator
    {
        private static readonly HashSet<Type> _allowedTypes = new HashSet<Type>
        {
            typeof(Destinations) 
        };

        public static bool IsAllowedType(Type type)
        {
            return _allowedTypes.Contains(type);
        }
    }
}