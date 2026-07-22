using Godot;
using System.Collections.Generic;
using System.Linq;

namespace OSK.Extensions.Petra.Godot;

/// <summary>
/// Extensions for <see cref="Node"/>
/// </summary>
public static class NodeExtensions
{
    #region Remove Children

    /// <summary>
    /// Frees all children for the node.
    /// </summary>
    /// <param name="node">The node that will have all of its children freed.</param>
    /// <param name="immediate">Determines if the freeing of resources can be delayed, to the end of frame, or must happen immediately</param>
    public static void FreeChildren(this Node node, bool immediate = false)
    {
        foreach (var child in node.GetChildren())
        {
            if (immediate)
            {
                child.Free();
            }
            else
            {
                child.QueueFree();
            }
        }
    }

    /// <summary>
    /// Removes all children for the node.
    /// </summary>
    /// <remarks>
    /// 💡Notes:
    /// <list type="bullet">
    /// <item>The removed children are NOT freed</item>
    /// </list>
    /// </remarks>
    /// <param name="node">The node that will have all of its children removed.</param>
    public static void RemoveChildren(this Node node)
    {
        foreach (var child in node.GetChildren())
        {
            node.RemoveChild(child);
        }
    }

    #endregion

    #region Add Children

    /// <summary>
    /// Adds all children to the node. This will reparent if a parent already exists for the child.
    /// </summary>
    /// <param name="node">The node to add children to</param>
    /// <param name="children">The children to add</param>
    public static void AddChildren(this Node node, params Node[] children)
        => node.AddChildren(true, children);

    /// <summary>
    /// Adds all children to the node.
    /// </summary>
    /// <param name="node">The node to add children to</param>
    /// <param name="reparent"></param>Determines if existing parents should be removed, if set to false and a parent exists the child will not be added.</param>
    /// <param name="children">The children to add</param>
    public static void AddChildren(this Node node, bool reparent, params Node[] children)
    {
        if (children is null)
        {
            return;
        }

        foreach (var child in children)
        {
            var parent = child.GetParent();
            if (parent is not null)
            {
                if (reparent && parent != node)
                {
                    child.Reparent(node);
                }
            }
            else
            {
                node.AddChild(child);
            }
        }
    }

    #endregion

    #region FindNode

    /// <summary>
    /// Attempts to find a node of a given type, using the node as the base for the scan and proceeding through the children
    /// </summary>
    /// <typeparam name="TNode">The type of node to check for</typeparam>
    /// <param name="node">The node to use as the starting point for checking a particular script</param>
    /// <param name="searchDepth">Limits the search depth for the search to the depth provided. Null represents checking the entire tree</param>
    /// <returns>The node if found, or null</returns>
    public static TNode? FindNode<TNode>(this Node node, int? searchDepth = null)
        => FindNodes<TNode>(node, searchDepth).FirstOrDefault();

    /// <summary>
    /// Attempts to find nodes of a given type, using the node as the base for the scan and proceeding through the children
    /// </summary>
    /// <typeparam name="TNode">The type of node to check for</typeparam>
    /// <param name="node">The node to use as the starting point for checking a particular script</param>
    /// <param name="searchDepth">Limits the search depth for the search to the depth provided. Null represents checking the entire tree</param>
    /// <returns>The nodes if found, or empty</returns>
    public static IEnumerable<TNode> FindNodes<TNode>(this Node node, int? searchDepth = null)
        => FindNodesOfType<TNode>(node, 0, searchDepth);

    private static IEnumerable<TNode> FindNodesOfType<TNode>(Node node, int currentDepth, int? searchDepth)
    {
        if (node is null)
        {
            yield break;
        }
        if (node is TNode typedNode)
        {
            yield return typedNode;
        }

        if (!searchDepth.HasValue || currentDepth + 1 <= searchDepth)
        {
            foreach (Node child in node.GetChildren())
            {
                if (child is TNode typedChildNode)
                {
                    yield return typedChildNode;
                }

                foreach (var subNode in FindNodesOfType<TNode>(child, currentDepth + 1, searchDepth))
                {
                    yield return subNode;
                }
            }
        }
    }

    #endregion

    #region FindParent

    /// <summary>
    /// Attempts to find a node of a given type, using the node as the base for the scan and proceeding through the parents
    /// </summary>
    /// <typeparam name="TNode">The type of node to check for</typeparam>
    /// <param name="node">The node to use as the starting point for checking a particular script</param>
    /// <param name="searchDepth">Limits the search depth for the search to the depth provided. Null represents checking the entire tree</param>
    /// <returns>The node if found, or null</returns>
    public static TNode? FindParent<TNode>(this Node node, int? searchDepth = null)
        => node.FindParents<TNode>(searchDepth).FirstOrDefault();

    /// <summary>
    /// Attempts to find nodes of a given type, using the node as the base for the scan and proceeding through the parents
    /// </summary>
    /// <typeparam name="TNode">The type of node to check for</typeparam>
    /// <param name="node">The node to use as the starting point for checking a particular script</param>
    /// <param name="searchDepth">Limits the search depth for the search to the depth provided. Null represents checking the entire tree</param>
    /// <returns>The nodes if found, or empty</returns>
    public static IEnumerable<TNode> FindParents<TNode>(this Node node, int? searchDepth = null)
    {
        if (node is TNode typedNode)
        {
            yield return typedNode;
        }

        var currentDepth = 0;
        while (!searchDepth.HasValue || currentDepth + 1 <= searchDepth)
        {
            var parentNode = node.GetParent();
            if (parentNode is null)
            {
                yield break;
            }

            if (parentNode is TNode typedParentNode)
            {
                yield return typedParentNode;
            }

            node = parentNode;
            currentDepth++;
        }
    }

    #endregion
}
