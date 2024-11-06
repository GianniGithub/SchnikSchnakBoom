using System.Collections.Generic;
using UnityEngine;
namespace GellosGames
{
    public class Sequence : Node
    {
        private List<Node> nodes = new List<Node>();
        public void Add(Node node)
        {
            nodes.Add(node);
        }
        public override void Update()
        {
            foreach (Node node in nodes)
            {
                node.Update();
            }
        }
    }
}