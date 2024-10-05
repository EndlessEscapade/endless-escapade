// copied from https://github.com/dotnet/runtime/blob/5535e31a712343a63f5d7d796cd874e563e5ac14/src/libraries/System.Private.CoreLib/src/System/Runtime/CompilerServices/InterpolatedStringHandlerAttribute.cs

// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

namespace System.Runtime.CompilerServices
{
    /// <summary>Indicates the attributed type is to be used as an interpolated string handler.</summary>
    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Struct, AllowMultiple = false, Inherited = false)]
    internal sealed class InterpolatedStringHandlerAttribute : Attribute
    {
        /// <summary>Initializes the <see cref="InterpolatedStringHandlerAttribute"/>.</summary>
        public InterpolatedStringHandlerAttribute() { }
    }
}