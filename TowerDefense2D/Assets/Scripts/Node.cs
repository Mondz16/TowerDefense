namespace TowerDefense.PathFinding
{
    public class Node
    {
        public int x;
        public int y;
        public bool isWalkable;
        public Node parent;
        public int gCost = 0;
        public int hCost = 0;

        public int fCost => gCost + hCost;

        public Node(int x, int y, bool isWalkable)
        {
            this.x = x;
            this.y = y;
            this.isWalkable = isWalkable;
        }

        public override string ToString()
        {
             return $"Node: x:{x} | y:{y} | fcost:{fCost} | hcost:{hCost} | gcost:{gCost} | isWalkable: {isWalkable}";
        }
    }
    
}