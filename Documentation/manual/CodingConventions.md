[![Generic badge](https://img.shields.io/badge/Status-WIP-yellow.svg)](https://shields.io/)

# C# Coding Conventions

## Introduction

This section explains the set of C# coding conventions I usually use.

## Naming

Naming conventions must follow the default settings of [Resharper](https://www.jetbrains.com/resharper/), such as:
- Pascal casing for methods
- Camel casing with a leading underscore for private fields
- Pascal casing for properties
- Camel casing for variable names
- etc.

There is only one exception that is made for serialized fields displayed in the Unity inspector: they use camel casing without a leading underscore (out of historical habit and to differentiate them from other fields).

## Files and types

Resharper should be used to enforce the following:
- Only one type (class, struct, enum) per file
- File name must match type name
- Namespace of the type must match the folder the file is in

Note that in the case of two closely-related types like `IFoo` and `IFoo<Bar>`, the two corresponding files should be:
- "IFoo.cs" for `IFoo`
- "IFoo{Bar}.cs" for `IFoo<Bar>`

## Class structure
Here is the usual order that should be applied:
- Inspector fields
  - Tweakable values
  - References to other parts of the GameObject (preserved if put into a prefab)
  - References to external GameObjects (lost in a prefab)
- Constants
- Static fields
- Private fields
- Properties
- Constructor
- Awake/Start (initialization callbacks)
- Public methods
- Update/OnTriggerEnter/etc. (main callbacks)
- Protected methods
- Private methods
- OnDestroy/OnDisable (end of life callbacks)

The main idea is to make the object lifecycle easy to understand while reading the file from top to bottom. 

Note that Unity callbacks may omit the 'private' keyword (again out of historical habit and to pop out from actual private methods).

Here is an dummy example demonstrating these conventions:

```csharp
using UnityEngine;

namespace Assets.Scripts.Foo.Bar
{
    public class Detector : MonoBehaviour
    {
        // -- Inspector

        [Header("Values")] 
		[Tooltip("Max distance at which detection can occur (meters).")]
        public float maxDistance = 50;

        [Header("Parts")] 
		[Tooltip("Source point of detection.")]
        public Transform eye;

		[Header("References")]
        public AbstractInputManager inputManager;


        // -- Class
        
        private const string AlertTriggerName = "AlertTrigger";
        
		private int _detectionCount = 0;
        private float _lastLineOfSightTime = 0;
		
        public Transform Target { get; private set; }

        void Start()
        {
            // ...
        }

		void Update()
		{
			// ...
		}
		
        public void Alert()
        {			
			// ...
        }

        private bool TargetIsInLineOfSight()
        {
            // ...
        }
		
		void OnDestroy()
		{
			// ...
		}
    }
}
```

### Coding style
Here is my general coding style:
- A class should have a clearly defined API
- A class should perform a small and defined task (no God objects)
- Methods/fields/properties should be kept private by default *unless they need to be accessed from the outside*
- Public methods should be unit-testable and private methods should *not need to be*
- A very carefull and thorough thinking should be performed before writing any *static* keyword


Note about the `var` keyword: I do not have a strong opinion on it and may or may not use it depending on the situation. My usual trend is to *not* use it for built-in types (int, bool, string,) and structs (Vector3, Color, etc.), but use it on longer type names (like ICollection<Tuple<int,bool>>).

