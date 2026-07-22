# OSK.Extensions.Petra.Godot

A project that provides many common and useful extensions for a variety Godot related objects. Extensions include, but not limited to:
 - `[Node] FindNode<T>`: finds a node or nodes of a given type within the base node. This can be performed either for a parent or child.
 - `[Node] AddChildren`: adds many children nodes to a parent
 - `[Node] RemoveChildren | FreeChildren`: removes or frees all children from the parent
 - `[CSharpScript] GetScriptType`: gets the underlying script type, which is the actual C# class type

# OSK.Extensions.Petra.Godot.DependencyInjection

A project that provides commond and useful extensions relating to the dependency injection system. Extensions include:
 - `ServiceCollectin`: Adds a node to a dependency container that utilizes standard .NET service collection and providers