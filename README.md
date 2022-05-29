# Poly.Collections
a minimal struct based collections for Unity and any C# (or .Net) project.

This package provides unmanaged data structures that can be used in struct based Ecs.

## Features
- Zero dependencies
- Minimal core (< 1000 lines)
- Lightweight and fast
- Collections for Ecs
- Adapted to all C# game engine

## Installation

## Overview

```csharp

public struct TestStruct
{
    public int Value;
}

var array = ArrayPool<TestStruct>.Shared.Rent(5);
ArrayPool<TestStruct>.Shared.Return(array);

var array = new FastArray<TestStruct>(3);
array[2] = new TestStruct { Value = 13 };
array.Length = 5;
ref var item = ref array.ElementAt(4);
item.Value = 23;

```

## License
The software is released under the terms of the [MIT license](./LICENSE.md).

## FAQ

## References

### Documents
- [Unity Collections package](https://docs.unity3d.com/Packages/com.unity.collections@1.2/manual/index.html)
- [NativeArray<T0>](https://docs.unity3d.com/ScriptReference/Unity.Collections.NativeArray_1.html)

### Projects

### Benchmarks
