using Godot;
using Microsoft.Extensions.DependencyInjection;
using OSK.Extensions.Petra.Godot;
using System;

namespace TowerDefenseLabs.Libraries.OSK.Godot.DependencyInjection;

/// <summary>
/// A set of service collection extensions utilized within Petra's Godot framework
/// </summary>
public static class GodotServiceCollectionExtensions
{
	#region GodotNode

	/// <summary>
	/// Adds a <see cref="Node"/> that is already in a level or scene to the dependency container using the provided root node as the intial object to scan
	/// </summary>
	/// <typeparam name="TNode">The type of Node that will be added to the DI container</typeparam>
	/// <param name="services">The service collection to add dependencies to</param>
	/// <param name="node">The root node to seek the given node from</param>
	/// <param name="searchDepth">Limits the search depth for the root tree to the depth provided. Null represents checking the entire tree</param>
	/// <returns>The service collection for chaining</returns>
	/// <exception cref="InvalidOperationException">This method assumes a node of the given type already exists in the scene when being used to add to the DI</exception>
	public static IServiceCollection AddSingletonNode<TNode>(this IServiceCollection services, Node node, int? searchDepth = null)
		where TNode : Node
	{
		var godotNode = node.FindNode<TNode>(searchDepth);
		if (godotNode is null)
		{
			throw new InvalidOperationException($"Node of type {typeof(TNode).Name} not found in the provided root node.");
		}

		return services.AddSingletonNode(godotNode);
	}

	/// <summary>
	/// Adds a <see cref="Node"/> that is already in a level or scene to the dependency container using the provided root node as the intial object to scan
	/// </summary>
	/// <typeparam name="TNode">The type of Node that will be added to the DI container</typeparam>
	/// <param name="services">The service collection to add dependencies to</param>
	/// <param name="nodeFunc">The function to return the node to seek the given node from</param>
	/// <param name="searchDepth">Limits the search depth for the root tree to the depth provided. Null represents checking the entire tree</param>
	/// <returns>The service collection for chaining</returns>
	/// <exception cref="InvalidOperationException">This method assumes a node of the given type already exists in the scene when being used to add to the DI</exception>
	public static IServiceCollection AddSingletonNode<TNode>(this IServiceCollection services, Func<IServiceProvider, Node> nodeFunc, int? searchDepth = null)
		where TNode : Node
	{
		return services.AddSingleton(serviceProvider =>
		{
			var node = nodeFunc(serviceProvider);
			var godotNode = node.FindNode<TNode>(searchDepth);
			if (godotNode is null)
			{
				throw new InvalidOperationException($"Node of type {typeof(TNode).Name} not found in the provided root node.");
			}

			return (TNode)godotNode;
		});
	}

	/// <summary>
	/// Adds a <see cref="Node"/> that is already in a level or scene to the dependency container using the provided node directly
	/// </summary>
	/// <typeparam name="TNode">The type of Node that will be added to the DI container</typeparam>
	/// <param name="services">The service collection to add dependencies to</param>
	/// <param name="node">The particular node to use for the DI container</param>
	/// <returns>The service collection for chaining</returns>
	public static IServiceCollection AddSingletonNode<TNode>(this IServiceCollection services, TNode node)
		where TNode : Node
	{
		return services.AddSingleton(_ => (TNode)node);
	}

	/// <summary>
	/// Adds a <see cref="Node"/> that is already in a level or scene to the dependency container using the provided root node as the intial object to scan
	/// </summary>
	/// <typeparam name="TInterface">The interface type that the Node implements</typeparam>
	/// <typeparam name="TNode">The type of Node that will be added to the DI container</typeparam>
	/// <param name="services">The service collection to add dependencies to</param>
	/// <param name="node">The node to seek the given node from</param>
	/// <param name="searchDepth">Limits the search depth for the root tree to the depth provided. Null represents checking the entire tree</param>
	/// <returns>The service collection for chaining</returns>
	/// <exception cref="InvalidOperationException">This method assumes a node of the given type already exists in the scene when being used to add to the DI</exception>
	public static IServiceCollection AddSingletonNode<TInterface, TNode>(this IServiceCollection services, Node node, int? searchDepth = null)
		where TNode : Node, TInterface
		where TInterface : class
	{
		var godotNode = node.FindNode<TNode>(searchDepth);
		if (godotNode is null)
		{
			throw new InvalidOperationException($"Node of type {typeof(TNode).Name} not found in the provided root node.");
		}
		if (godotNode is not TInterface)
		{
			throw new InvalidOperationException($"Node of type {typeof(TNode).Name} does not implement interface {typeof(TInterface).Name}.");
		}

		return services.AddSingletonNode<TInterface, TNode>(godotNode);
	}

	/// <summary>
	/// Adds a <see cref="Node"/> that is already in a level or scene to the dependency container using the provided node directly
	/// </summary>
	/// <typeparam name="TInterface">The interface type the node implements</typeparam>
	/// <typeparam name="TNode">The type of Node that will be added to the DI container</typeparam>
	/// <param name="services">The service collection to add dependencies to</param>
	/// <param name="node">The particular node to use for the DI container</param>
	/// <returns>The service collection for chaining</returns>
	public static IServiceCollection AddSingletonNode<TInterface, TNode>(this IServiceCollection services, TNode node)
		where TNode : Node, TInterface
		where TInterface : class
	{
		return services.AddSingleton<TInterface>(_ => (TNode)node);
	}

	/// <summary>
	/// Adds a <see cref="Node"/> that is already in a level or scene to the dependency container using the provided root node as the intial object to scan
	/// </summary>
	/// <typeparam name="TInterface">The interface type that the Node implements</typeparam>
	/// <typeparam name="TNode">The type of Node that will be added to the DI container</typeparam>
	/// <param name="services">The service collection to add dependencies to</param>
	/// <param name="rootNodeFunc">The function to get the root node to seek the given node from</param>
	/// <param name="searchDepth">Limits the search depth for the root tree to the depth provided. Null represents checking the entire tree</param>
	/// <returns>The service collection for chaining</returns>
	/// <exception cref="InvalidOperationException">This method assumes a node of the given type already exists in the scene when being used to add to the DI</exception>
	public static IServiceCollection AddSingletonNode<TInterface, TNode>(this IServiceCollection services, Func<IServiceProvider, Node> rootNodeFunc, int? searchDepth = null)
		where TNode : Node, TInterface
		where TInterface : class
	{
		return services.AddSingleton<TInterface>(serviceProvider =>
		{
			var node = rootNodeFunc(serviceProvider);
			var godotNode = node.FindNode<TNode>(searchDepth);
			if (godotNode is null)
			{
				throw new InvalidOperationException($"Node of type {typeof(TNode).Name} not found in the provided root node.");
			}

			return (TNode)godotNode;
		});
	}

	#endregion
}
