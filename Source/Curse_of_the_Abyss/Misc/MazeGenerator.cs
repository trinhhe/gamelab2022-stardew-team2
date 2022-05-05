using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace Curse_of_the_Abyss
{
    public class MazeGenerator
    {
        // maze nodes
        public List<Node> nodes;
        // maze walls
        public List<Wall> walls;
        // The current Maze node to be used in the Maze generation process.
        public Node currentGenerationNode;
        // The queue used to hold the Maze nodes that are still to be visited in the Maze generation process.
        public Stack<Node> unvisitedNodesQueue;
        // random number generator
        public Random rand;
        // seed used by rng to generate the Maze
        public int seed;
        // wall texture
        public Texture2D texture;
        // position of upperleft corner of maze in game window
        public Vector2 positionOnWindow;
        // coordinate where maze entry is
        public Vector2 mazeEntry;
        // The width of the Maze in coordinate spaces.
        public int widthInCoordinates;
        // The height of the Maze in coordinate spaces.
        public int heightInCoordinates;
        // The width and height of each of the coordinate spaces within the Maze.
        public Vector2 coordinateSize;
        // The length in coordinates of the longest walk distance of any path originating at the generation origin node within the Maze.
        public int longestWalkDistance;
        // Draw size of node / single unit of path
        public Vector2 nodeDrawSize;
        // Wall thickness
        public int wallDrawThickness;
        private SpriteBatch spriteBatch;
        public MazeGenerator(SpriteBatch _spritebatch, Texture2D texture, Vector2 positionOnWindow, Vector2 coordinateSize, Vector2 nodeDrawSize, int wallDrawThickness, int widthInCoordinates, int heightInCoordinates)
        {
            this.spriteBatch = _spritebatch;
            this.texture = texture;
            this.positionOnWindow = positionOnWindow;
            this.coordinateSize = coordinateSize;
            this.nodeDrawSize = nodeDrawSize;
            this.wallDrawThickness = wallDrawThickness;
            this.widthInCoordinates = widthInCoordinates;
            this.heightInCoordinates = heightInCoordinates;
            this.nodes = new List<Node>();
            this.walls = new List<Wall>();
            this.longestWalkDistance = 0;

        }

        public void Generate(Vector2 mazeEntry, int seed = 0)
        {
            //Fill Maze with nodes and walls
            for (int x = 0; x < widthInCoordinates; x++)
            {
                for (int y = 0; y < heightInCoordinates; y++)
                {
                    nodes.Add(new Node(new Vector2(x, y)));

                    // If the current x-coordinate is not at the rightmost side of the Maze, adds a wall between the current coordinate and the one to the right.
                    if (x < widthInCoordinates - 1)
                        walls.Add(new Wall(new Vector2(x, y), new Vector2(x + 1, y)));
                    // If the current y-coordinate is not at the bottom side of the Maze, adds a wall between the current coordinate and the one to the bottom.
                    if (y < heightInCoordinates - 1)
                        walls.Add(new Wall(new Vector2(x, y), new Vector2(x, y + 1)));
                }
            }

            if (seed == 0)
                rand = new Random();
            else
                rand = new Random(seed);


            DFS();

            // Adds walls to the left and right sides of the Maze.
            for (int i = 0; i < heightInCoordinates; ++i)
            {
                // left side
                walls.Add(new Wall(new Vector2(-1, i), new Vector2(0, i)));
                // right side
                walls.Add(new Wall(new Vector2(widthInCoordinates - 1, i), new Vector2(widthInCoordinates, i)));
            }
            // Adds walls to the top and bottom of the Maze.
            for (int i = 0; i < widthInCoordinates; ++i)
            {
                // top side
                walls.Add(new Wall(new Vector2(i, -1), new Vector2(i, 0)));
                // bottom side 
                walls.Add(new Wall(new Vector2(i, heightInCoordinates - 1), new Vector2(i, heightInCoordinates)));
            }
            RemoveWall(new Vector2(mazeEntry.X - 1, mazeEntry.Y), mazeEntry);
            //TODO: remove wall at end
        }

        public void DFS()
        {
            //Init Queue for unvisited nodes
            unvisitedNodesQueue = new Stack<Node>();
            var len = nodes.Count;
            for (int i = 0; i < len; i++)
            {
                if (nodes[i].coordinateInMaze == mazeEntry)
                {
                    currentGenerationNode = nodes[i];
                    break;
                }
            }

            unvisitedNodesQueue.Push(currentGenerationNode);
            currentGenerationNode.visited = true;

            while (unvisitedNodesQueue.Count > 0)
            {
                List<Node> unvisitedAdjacentNodes = currentGenerationNode.GetUnvisitedAdjacentNodes(this);
                // If there are no unvisited nodes adjacent to the current node, continues to pop nodes off the queue until a node fulfils these requirements and makes this node the current node.
                if (unvisitedAdjacentNodes.Count > 0)
                {
                    // Randomly selects one of the unvisited adjacent nodes of the current node.
                    Node adjacentNode = unvisitedAdjacentNodes[rand.Next(0, unvisitedAdjacentNodes.Count)];
                    // Finds the wall between the current node and the adjacent node within the Maze and removes it.
                    for (int i = 0; i < walls.Count; i++)
                    {
                        if (walls[i].firstNodeCoordinate == currentGenerationNode.coordinateInMaze && walls[i].secondNodeCoordinate == adjacentNode.coordinateInMaze ||
                            walls[i].firstNodeCoordinate == adjacentNode.coordinateInMaze && walls[i].secondNodeCoordinate == currentGenerationNode.coordinateInMaze)
                        {
                            walls.RemoveAt(i);

                            adjacentNode.walkDistance = currentGenerationNode.walkDistance + 1;
                            if (adjacentNode.walkDistance > longestWalkDistance)
                                longestWalkDistance = adjacentNode.walkDistance;
                            break;
                        }
                    }
                    // Sets the current node to be the adjacent node.
                    currentGenerationNode = adjacentNode;
                    currentGenerationNode.visited = true;
                    unvisitedNodesQueue.Push(currentGenerationNode);
                }
                else
                    currentGenerationNode = unvisitedNodesQueue.Pop();
            }
        }

        public void RemoveWall(Vector2 firstNodeCoordinate, Vector2 secondNodeCoordinate)
        {
            for (int i = 0; i < walls.Count; i++)
            {
                if (walls[(int)i].firstNodeCoordinate == firstNodeCoordinate && walls[(int)i].secondNodeCoordinate == secondNodeCoordinate)
                    walls.RemoveAt((int)i);
            }
        }

        //add walls as obstacles for collision detection
        public void AddObstacles(List<Sprite> sprites)
        {
            for (int i = 0; i < walls.Count; ++i)
                walls[i].AddObstacle(this, sprites);
        }
        public void Draw(Matrix matrix)
        //public void Draw()
        {
            // Draws all of the nodes within the Maze.
            //for (int i = 0; i < nodes.Count; ++i)
            //{
            //    // The lerp value used to blend between the two colors of the maze depending on how far the current node is away from the origin relative to the furthest node from the origin.
            //    float lerpValue = ((float)nodes[i].walkDistance + 1.0f) / longestWalkDistance;

            //    // Uses this lerp value to blend between the two maze colors.
            //    Color drawColor = new Color(MathHelper.Lerp(coordinateOriginBlendColor.R, coordinateDistanceBlendColor.R, lerpValue) / 255, MathHelper.Lerp(coordinateOriginBlendColor.G,
            //        coordinateDistanceBlendColor.G, lerpValue) / 255, MathHelper.Lerp(coordinateOriginBlendColor.B, coordinateDistanceBlendColor.B, lerpValue) / 255);

            //    // Draws the current node with the resulting color.
            //    nodes[i].Draw(drawColor, this, spriteBatch, texture);
            //}


            spriteBatch.Begin(transformMatrix: matrix);

            // Draws all of the walls within the Maze.
            for (int i = 0; i < walls.Count; ++i)
                walls[i].Draw(this, spriteBatch, texture);

            spriteBatch.End();
        }
    }
}
