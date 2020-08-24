munitude
========

portable runtime for .NET managed language engines.

Current Status
--------------

* can enumerate and load engine assemblies
* can enumerate types and access managed code methods from the engine
* initial stubs of internal call methods
* not hooked up to native libraries at this point
* no game loop implementation

Dependencies
------------

* [libstubborn](https://github.com/rfht/libstubborn)
* [Harmony](https://github.com/pardeike/Harmony)

How to Build
------------

Copy dependencies into the build directory, then
```
$ make
```

How to Use
----------

* copy Munitude.exe into the directory with the managed code (`$ cp Munitude.exe /path/to/Managed/`)
* copy `0Harmony.dll` (from [Harmony](https://github.com/pardeike/Harmony)) to the same directory
* note `munitude` needs to be executed from a location with W|X at this point (mono limitation without full AOT).
* from the managed code directory, run
```
$ /path/to/munitude Munitude.exe
```

**License:** ISC
**Disclaimer:** None of the code, tool, or repository owner is affiliated with, sponsored or authorized by Unity Technologies or its affiliates.
