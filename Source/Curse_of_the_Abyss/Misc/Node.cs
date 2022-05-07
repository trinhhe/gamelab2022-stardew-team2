using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace Curse_of_the_Abyss
{
    public class Node
    {
        // coordinate position of Node within the maze (e.g. (0,0) upper-left corder of maze)
        public Vector2 coordinateInMaze;
        // used for DFS in maze generation
        public bool visited;
        // distance from entry to this node
        public int walkDistance;
        // deadend if around 3 wall
        public int walls;

        public Node(Vector2 coordinateInMaze)
        {
            this.coordinateInMaze = coordinateInMaze;
            visited = false;
            walkDistance = 0;
            walls = 4;
        }

        // Returns a list containing the unvisited nodes adjacent to this Node within the Maze.
        public List<Node> GetUnvisitedAdjacentNodes(MazeGenerator maze)
        {
            // The list to be returned.
            List<Node> adjacentNodes = new List<Node>();

            int len = maze.nodes.Count;
            // Checks each of the nodes in the maze and adds them to the list if they are adjacent to this Node.
            for (int i = 0; i < len; i++)
            {
                if (maze.nodes[i].coordinateInMaze.X == coordinateInMaze.X - 1 && maze.nodes[i].coordinateInMaze.Y == coordinateInMaze.Y ||
                    maze.nodes[i].coordinateInMaze.X == coordinateInMaze.X + 1 && maze.nodes[i].coordinateInMaze.Y == coordinateInMaze.Y ||
                    maze.nodes[i].coordinateInMaze.X == coordinateInMaze.X && maze.nodes[i].coordinateInMaze.Y == coordinateInMaze.Y - 1 ||
                    maze.nodes[i].coordinateInMaze.X == coordinateInMaze.X && maze.nodes[i].coordinateInMaze.Y == coordinateInMaze.Y + 1)
                    adjacentNodes.Add(maze.nodes[i]);
            }

            // Removes any nodes from the list that have been visited.
            len = adjacentNodes.Count - 1;
            for (int i = len; i > -1; i--)
            {
                if (adjacentNodes[i].visited)
                    adjacentNodes.RemoveAt(i);
            }

            return adjacentNodes;
        }

        public void Draw(Color color, MazeGenerator maze, SpriteBatch spriteBatch, Texture2D texture)
        {
            // The horizontal position of the upper-leftmost point of the MazeNode on the game window.
            float nodeHorizontalWindowPosition = maze.positionOnWindow.X + (coordinateInMaze.X * maze.coordinateSize.X) + maze.coordinateSize.Y / 2 + maze.wallDrawThickness / 2 - maze.nodeDrawSize.X / 2;
            // The vertical position of the upper-leftmost point of the MazeNode on the game window.
            float nodeVerticalWindowPosition = maze.positionOnWindow.Y + coordinateInMaze.Y * maze.coordinateSize.Y + maze.coordinateSize.Y / 2 + maze.wallDrawThickness / 2 - maze.nodeDrawSize.Y / 2;

            // Draws the MazeNode on the game window.
            spriteBatch.Draw(texture, new Rectangle((int)nodeHorizontalWindowPosition, (int)nodeVerticalWindowPosition, (int)maze.nodeDrawSize.X, (int)maze.nodeDrawSize.Y), color);
        }
    }
}
