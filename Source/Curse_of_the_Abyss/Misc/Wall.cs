using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace Curse_of_the_Abyss
{
    public class Wall
    {
        // Enumerated values used to represent the orientation of the MazeWall.
        public enum MazeOrientation
        {
            Horizontal,
            Vertical
        }

        // The orientation of the MazeWall.
        public MazeOrientation orientation;
        // The coordinate position within the maze of the first MazeNode of the two which are separated by the MazeWall.
        public Vector2 firstNodeCoordinate;
        // The coordinate position within the maze of the second MazeNode of the two which are separated by the MazeWall.
        public Vector2 secondNodeCoordinate;

        public Wall(Vector2 firstNodeCoordinate, Vector2 secondNodeCoordinate)
        {
            this.firstNodeCoordinate = firstNodeCoordinate;
            this.secondNodeCoordinate = secondNodeCoordinate;

            // Sets the orientation of the MazeWall.
            if (firstNodeCoordinate.X != secondNodeCoordinate.X)
                orientation = MazeOrientation.Horizontal;
            else
                orientation = MazeOrientation.Vertical;
        }

        public void AddObstacle(MazeGenerator maze, List<Sprite> sprites)
        {
            if (orientation == MazeOrientation.Horizontal)
            {
                int wallHorizontalDrawPosition = (int)(maze.positionOnWindow.X + firstNodeCoordinate.X * maze.coordinateSize.X + maze.coordinateSize.X);
                int wallVerticalDrawPosition = (int)(maze.positionOnWindow.Y + firstNodeCoordinate.Y * maze.coordinateSize.Y);
                sprites.Add(new Obstacle(new Rectangle(wallHorizontalDrawPosition, wallVerticalDrawPosition, maze.wallDrawThickness, (int) (maze.coordinateSize.Y + maze.wallDrawThickness))));
            }
            else if (orientation == MazeOrientation.Vertical)
            {
                int wallHorizontalDrawPosition = (int)(maze.positionOnWindow.X + firstNodeCoordinate.X * maze.coordinateSize.X);
                int wallVerticalDrawPosition = (int)(maze.positionOnWindow.Y + firstNodeCoordinate.Y * maze.coordinateSize.Y + maze.coordinateSize.Y);
                sprites.Add(new Obstacle(new Rectangle(wallHorizontalDrawPosition, wallVerticalDrawPosition, (int)(maze.coordinateSize.X + maze.wallDrawThickness), maze.wallDrawThickness)));
            }
        }
        public void Draw(MazeGenerator maze, SpriteBatch spriteBatch, Texture2D texture)
        {
            // If the MazeWall is horizontal, draws the wall horizontally between the specified coordinates of the maze.
            if (orientation == MazeOrientation.Horizontal)
            {
                // The horizontal draw position of the wall on the window.
                int wallHorizontalDrawPosition = (int)(maze.positionOnWindow.X + firstNodeCoordinate.X * maze.coordinateSize.X + maze.coordinateSize.X);
                // The vertical draw position of the wall on the window.
                int wallVerticalDrawPosition = (int)(maze.positionOnWindow.Y + firstNodeCoordinate.Y * maze.coordinateSize.Y);
                spriteBatch.Draw(texture, new Rectangle(wallHorizontalDrawPosition, wallVerticalDrawPosition, (int)maze.wallDrawThickness, (int)(maze.coordinateSize.Y + maze.wallDrawThickness)), Color.White);
            }
            // Else if the MazeWall is vertical, draws the wall vertically.
            else if (orientation == MazeOrientation.Vertical)
            {
                // The horizontal draw position of the wall on the window.
                int wallHorizontalDrawPosition = (int)(maze.positionOnWindow.X + firstNodeCoordinate.X * maze.coordinateSize.X);
                // The vertical draw position of the wall on the window.
                int wallVerticalDrawPosition = (int)(maze.positionOnWindow.Y + firstNodeCoordinate.Y * maze.coordinateSize.Y + maze.coordinateSize.Y);
                spriteBatch.Draw(texture, new Rectangle(wallHorizontalDrawPosition, wallVerticalDrawPosition, (int)(maze.coordinateSize.X + maze.wallDrawThickness), (int)maze.wallDrawThickness), Color.White);
            }
        }
    }
}
