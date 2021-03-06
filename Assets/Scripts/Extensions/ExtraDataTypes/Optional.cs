﻿using System;
using UnityEngine;

namespace Extension.ExtraTypes
{
    /// <summary>
    /// The base class of the generic Optional class.
    /// </summary>
    /// <remarks>
    /// This is an empty class; the reason it exists is so that a single property drawer can be used 
    /// for all classes that derive from the generic Optional class.
    /// </remarks>
    public class Optional { }

    /// <summary>
    /// Useful for displaying optional values in the inspector. 
    /// </summary>
    /// <typeparam name="T">The type of the optional value.</typeparam>
    /// <remarks>
    /// For this class to be displayable in the inspector you cannot use it directly. You have to use one of the provided
    /// subclasses (or derive your own).
    /// </remarks>
    [Serializable]
    public class Optional<T> : Optional
    {
        [Tooltip("Check this to set a value for this instance")]
        [SerializeField]
        private bool useValue;

        [Tooltip("The value of this instance")]
        [SerializeField]
        private T value;

        public bool UseValue
        {
            get { return useValue; }
            set { useValue = value; }
        }

        /// <summary>
        /// The value of this instance. It should only be used if UseValue is true. Otherwise, some
        /// other value should be used, or code that does not need it must be executed instead.
        /// </summary>
        /// <example>
        /// if (optionalMaterial.UseValue)
        /// {
        ///		renderer.material = material;
        /// } 
        /// // else do not modify the material.
        /// </example>
        public T Value
        {
            get { return value; }
            set { this.value = value; }
        }

        public override string ToString()
        {
            if (UseValue)
            {
                return Value.ToString();
            }
            else
            {
                return "[No Value]";
            }
        }
    }

    /// <summary>
    /// Represents an optional float value.
    /// </summary>
    [Serializable]
    public class OptionalFloat : Optional<float> { }

    /// <summary>
    /// Represents an optional string value.
    /// </summary>
    [Serializable]
    public class OptionalString : Optional<string> { }

    /// <summary>
    /// Represents an optional GameObject.
    /// </summary>
    [Serializable]
    public class OptionalGameObject : Optional<GameObject> { }

    /// <summary>
    /// Represents an optional Vector2 value.
    /// </summary>
    [Serializable]
    public class OptionalVector2 : Optional<Vector2> { }

    /// <summary>
    /// Represents an optional Vector3 value.
    /// </summary>
    [Serializable]
    public class OptionalVector3 : Optional<Vector3> { }

    /// <summary>
    /// Represents an optional MonoBehaviour.
    /// </summary>
    [Serializable]
    public class OptionalMonoBehaviour : Optional<MonoBehaviour> { }
}