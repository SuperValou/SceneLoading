[![Generic badge](https://img.shields.io/badge/Status-WIP-yellow.svg)](https://shields.io/)

# C# Coding Standards

## Introduction

The aim of this section is to explain the set of C# coding standards used in this project.

*TODO*

### Callbacks


### Project structure
- Use asmdef to split libraries into smaller chunks 

### File structure
A C# file should follow this conventions:
- Only one type (class, struct, enum) per file
- File name and type name must match
- Namespace of the type must match the folder the file is in

### Class structure
Here is the order:
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
- OnDestroy/OnDisable (end of lifecycle callbacks)

The main idea is to understand the object lifecycle more easily while reading the file from top to bottom. 

Note that Unity callbacks should always be `private` (or can be made `protected` only if inheriting classes have to override them). They may omit the 'private' keyword to pop out from other methods.

Here is an example:

```csharp
using Assets.Scripts.Utilities;
using UnityEngine;

namespace Assets.Scripts.Foo.Bar
{
    public class Detector : MonoBehaviour
    {
        // -- Inspector

        [Header("Values")] 
		[Tooltip("The max distance at which detection can occur (meters).")]
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

### General philosophy
- Divide into smaller chunks
- Define the API
- Restrict access whenever possible