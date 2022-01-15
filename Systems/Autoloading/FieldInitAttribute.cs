#pragma warning disable CS0649 // default value
using System;

namespace EEMod.Autoloading
{
    /// <summary>
    /// Apply this to fields for instantiating them during <see cref="EEMod.Load"/><br />
    /// Can also be applied to methods (it will call them during field initializing)
    /// </summary>
    [AttributeUsage(AttributeTargets.Field | AttributeTargets.Method)]
    internal class FieldInitAttribute : Attribute
    {
        public object InitInfo1 { get; internal set; }
        public object InitInfo2 { get; internal set; }
        public FieldInitType InitType { get; internal set; }

        public FieldInitAttribute() : this(FieldInitType.DefaultConstructor)
        {
        }

        public FieldInitAttribute(FieldInitType initType, object initInfo1 = null, object initInfo2 = null)
        {
            InitType = initType;
            InitInfo1 = initInfo1;
            InitInfo2 = initInfo2;
        }

        public FieldInitAttribute(FieldInitType initType, bool privateconstructor) : this(initType, initInfo1: privateconstructor)
        {
        }

        public FieldInitAttribute(FieldInitType initType, object value) : this(initType, initInfo1: value)
        {
        }

        public FieldInitAttribute(FieldInitType initType, int arrayCapcity) : this(initType, initInfo1: arrayCapcity)
        {
        }

        public FieldInitAttribute(FieldInitType initType, params int[] arrayLengths) : this(initType, initInfo1: arrayLengths)
        {
        }

        public FieldInitAttribute(FieldInitType initType, Type subtype, FieldInitType subInitType) : this(initType, initInfo1: subtype, initInfo2: subInitType)
        {
        }
        // TODO
        internal LoadMode loadMode; // fields instantiating and method calling
    }

    internal enum FieldInitType
    {
        /// <summary>
        /// Attempts to initialize the field looking for the default constructor
        /// </summary>
        DefaultConstructor,

        /// <summary>
        /// Attempts to initialize the field looking for the default constructor, even if it's private
        /// </summary>
        DefaultConstructorPrivate,

        /// <summary>
        /// The provided object will be the value for initializing the field
        /// </summary>
        [Obsolete("Use a method for initializing the field instead.")] 
        CustomValue,

        /// <summary>
        /// Initialize the current array with a given capacity. <br />
        /// Note this only takes an int as parameter which indicates the capcity, <br />
        /// if you need to initialize with already existing values (e.g. new int[3] { 1, 2, 3 }), use <see cref="CustomValue"/>.
        /// </summary>
        [Obsolete("Use a method for initializing the field instead.")] 
        ArrayIntialization,

        /// <summary>
        /// The arguments shall be (<see cref="SubType"/>, <see cref="Type"/> type, <see cref="FieldInitType"/> subInitType) <br />
        /// e.g. <br />
        /// <c>class A { } <br />
        /// class B : A { } <br />
        /// [FieldInitAttribute(FieldInitType.SubType, typeof(B), FieldInitType.DefaultConstructor)] <br />
        /// static A field;</c>
        /// </summary>
        [Obsolete("Use a method for initializing the field instead.")] 
        SubType,

        [Obsolete("Use a method for initializing the field instead.")] 
        ArrayMultipleLengths,
    }
}