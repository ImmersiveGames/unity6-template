﻿using System;
using _ImmersiveGames.Scripts.Utils.Extensions;
using UnityEngine;

namespace _ImmersiveGames.Scripts.Utils.Attributes {
    public class TypeFilterAttribute : PropertyAttribute {
        public Func<Type, bool> Filter { get; }
        
        public TypeFilterAttribute(Type filterType) {
            Filter = type => !type.IsAbstract &&
                             !type.IsInterface &&
                             !type.IsGenericType &&
                             type.InheritsOrImplements(filterType);
        }
    }
}