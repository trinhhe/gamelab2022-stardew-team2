using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System.Linq;

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
        // wall textures
        public Texture2D wall_horizontal, wall_vertical;
        // position of upperleft corner of maze in game window
        public Vector2 positionOnWindow;
        // coordinate where maze entry is
        public Vector2 mazeEntry, mazeExit;
        // The width of the Maze in coordinate spaces.
        public int widthInCoordinates;
        // The height of the Maze in coordinate spaces.
        public int heightInCoordinates;
        // The width and height of each of the coordinate spaces within the Maze.
        public Vector2 coordinateSize;
        // The length in coordinates of the longest walk distance of any path originating at the generation origin node within the Maze.
        public int longestWalkDistance;
        // Draw size of node / single unit of path if needed
        public Vector2 nodeDrawSize;
        // Wall thickness
        public int wallDrawThickness;
        private SpriteBatch spriteBatch;
        //deadend nodes
        public List<Node> deadend_nodes;
        public MazeGenerator(SpriteBatch _spritebatch, Texture2D wallhorizontal, Texture2D wallvertical, Vector2 positionOnWindow, Vector2 coordinateSize, Vector2 nodeDrawSize, int wallDrawThickness, int widthInCoordinates, int heightInCoordinates, int seed = 0)
        {
            this.spriteBatch = _spritebatch;
            this.wall_horizontal = wallhorizontal;
            this.wall_vertical = wallvertical;
            this.positionOnWindow = positionOnWindow;
            this.coordinateSize = coordinateSize;
            this.nodeDrawSize = nodeDrawSize;
            this.wallDrawThickness = wallDrawThickness;
            this.widthInCoordinates = widthInCoordinates;
            this.heightInCoordinates = heightInCoordinates;
            this.nodes = new List<Node>();
            this.walls = new List<Wall>();
            this.longestWalkDistance = 0;
            this.deadend_nodes = new List<Node>();

            if (seed == 0)
                this.rand = new Random();
            else
                this.rand = new Random(seed);

        }
        //mazeEntry and mazeExit in  coordinate space e.g. upperleft node is (0,0)
        public void Generate(Vector2 mazeEntry, Vector2 mazeExit, int seed = 0)
        {
            this.mazeExit = mazeExit;
            this.mazeEntry = mazeEntry;
            //Fill Maze with nodes and walls
            for (int y = 0; y < heightInCoordinates ; y++)
            {
                for (int x = 0; x < widthInCoordinates; x++)
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
            RemoveWall(mazeExit, new Vector2(mazeExit.X+1, mazeExit.Y));
            //walls.Clear(); //CHANGE

            //add deadend nodes to list
            foreach(Node i in nodes)
            {
                if (i.walls == 3)
                    deadend_nodes.Add(i);
            }
            //sort deadends furthest from mazeExit in descending order
            deadend_nodes.Sort((x, y) => y.walkDistance.CompareTo(x.walkDistance));
        }

        public void DFS()
        {
            //Init Queue for unvisited nodes
            unvisitedNodesQueue = new Stack<Node>();
            var len = nodes.Count;
            int x, y;
            for (int i = 0; i < len; i++)
            {
                //start from mazeExit, so we can place eggs in deadends that are furthest away from exit
                if (nodes[i].coordinateInMaze == mazeExit)
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
                            //remove wall count for nodes
                            x = (int)walls[i].firstNodeCoordinate.X;
                            y = (int)walls[i].firstNodeCoordinate.Y;
                            nodes[y * widthInCoordinates + x].walls--;
                            x = (int)walls[i].secondNodeCoordinate.X;
                            y = (int)walls[i].secondNodeCoordinate.Y;
                            nodes[y * widthInCoordinates + x].walls--;
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
                {
                    currentGenerationNode = unvisitedNodesQueue.Pop();
                }
                    
            }
        }

        public void RemoveWall(Vector2 firstNodeCoordinate, Vector2 secondNodeCoordinate)
        {
            int x, y;
            for (int i = 0; i < walls.Count; i++)
            {
                if (walls[(int)i].firstNodeCoordinate == firstNodeCoordinate && walls[(int)i].secondNodeCoordinate == secondNodeCoordinate)
                {
                    x = (int)walls[i].firstNodeCoordinate.X;
                    y = (int)walls[i].firstNodeCoordinate.Y;
                    if(y < heightInCoordinates && x < widthInCoordinates)
                        nodes[y * widthInCoordinates + x].walls--;
                    x = (int)walls[i].secondNodeCoordinate.X;
                    y = (int)walls[i].secondNodeCoordinate.Y;
                    if(y < heightInCoordinates && x < widthInCoordinates)
                        nodes[y * widthInCoordinates + x].walls--;
                    walls.RemoveAt((int)i);
                }
                    
            }
        }

        /*add walls as obstacles for collision detection*/
        public void AddAsObstacles(List<Sprite> sprites)
        {
            for (int i = 0; i < walls.Count; ++i)
                walls[i].AddAsObstacle(this, sprites);
        }
        //get worldcoordinate of where sprite is to be placed in NodeCoordinate of maze s.t. sprite is in center of that node.
        public Vector2 placeInCenterOfNode(Vector2 NodeCoordinate, int width, int height)
        {
            Vector2 res = new Vector2(positionOnWindow.X + NodeCoordinate.X * coordinateSize.X + (nodeDrawSize.X - width) / 2,
                positionOnWindow.Y + NodeCoordinate.Y * coordinateSize.Y + (nodeDrawSize.Y - height) / 2);
            return res;
        }

        //get a place for an egg in a deadend node
        public List<Node> getEggPlaces(int num_eggs)
        {
            int len = deadend_nodes.Count;
            if (len < num_eggs)
                return deadend_nodes;
            else
            {
                List<Node> res = new List<Node>();
                
                for(int i = 0; i < num_eggs; i++)
                {
                    res.Add(deadend_nodes[(2*i)%len]);
                }
                return res;
            }
        }
        public void Draw(Matrix matrix)
        {
            spriteBatch.Begin(transformMatrix: matrix);
            //Draws all of the nodes within the Maze.
            //Color red = Color.Red;
            //Color blue = Color.Blue;
            //for (int i = 0; i < nodes.Count; ++i)
            //{
            //    if (i % 2 == 0)
            //        // Draws the current node with the resulting color.
            //        nodes[i].Draw(red, this, spriteBatch, wall_horizontal);
            //    else
            //        nodes[i].Draw(blue, this, spriteBatch, wall_horizontal);
            //}
            //foreach (var n in nodes)
            //{
            //    if (n.walls == 3)
            //        n.Draw(red, this, spriteBatch, wall_horizontal);
            //}

            // Draws all of the walls within the Maze.
            for (int i = 0; i < walls.Count; ++i)
                walls[i].Draw(this, spriteBatch, wall_horizontal, wall_vertical);

            spriteBatch.End();
        }
    }
}
