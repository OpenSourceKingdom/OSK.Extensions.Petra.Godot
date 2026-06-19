using Godot;
using System.Collections.Generic;
using System.Linq;

namespace OSK.Extensions.Petra.Godot;

/// <summary>
/// Extensions for <see cref="Node"/>
/// </summary>
public static class NodeExtensions
{
    #region FindNode

    /// <summary>
    /// Attempts to find a node of a given type, using the node as the base for the scan and proceeding through the children
    /// </summary>
    /// <typeparam name="TNode">The type of node to check for</typeparam>
    /// <param name="node">The node to use as the starting point for checking a particular script</param>
    /// <param name="searchDepth">Limits the search depth for the search to the depth provided. Null represents checking the entire tree</param>
    /// <returns>The node if found, or null</returns>
    public static TNode? FindNodeOfType<TNode>(this Node node, int? searchDepth = null)
        => FindNodesOfType<TNode>(node, searchDepth).FirstOrDefault();

    /// <summary>
    /// Attempts to find nodes of a given type, using the node as the base for the scan and proceeding through the children
    /// </summary>
    /// <typeparam name="TNode">The type of node to check for</typeparam>
    /// <param name="node">The node to use as the starting point for checking a particular script</param>
    /// <param name="searchDepth">Limits the search depth for the search to the depth provided. Null represents checking the entire tree</param>
    /// <returns>The nodes if found, or empty</returns>
    public static IEnumerable<TNode> FindNodesOfType<TNode>(this Node node, int? searchDepth = null)
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
    public static TNode? FindParentOfType<TNode>(Node node, int? searchDepth = null)
        => FindParentsOfType<TNode>(node, searchDepth).FirstOrDefault();

    /// <summary>
    /// Attempts to find nodes of a given type, using the node as the base for the scan and proceeding through the parents
    /// </summary>
    /// <typeparam name="TNode">The type of node to check for</typeparam>
    /// <param name="node">The node to use as the starting point for checking a particular script</param>
    /// <param name="searchDepth">Limits the search depth for the search to the depth provided. Null represents checking the entire tree</param>
    /// <returns>The nodes if found, or empty</returns>
    public static IEnumerable<TNode> FindParentsOfType<TNode>(Node node, int? searchDepth = null)
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
