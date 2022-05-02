using Microsoft.Xna.Framework;
using System;

namespace Curse_of_the_Abyss 
{ 

    public static class Constants{

        //screen related
        public static Matrix transform_matrix;
        //waterplayer
        public static float max_run_velocity = 5;
        public static float jump_velocity = -2;
        public static float fall_velocity = 0.3f;
        public static float max_y_velocity = 5;
        public static float run_accelerate = 0.2f;
        public static float max_jumping_height = 20;

        //submarineplayer
        public static float submarineplayer_max_run_velocity = 3;
        public static float submarineplayer_run_accelerate = 0.1f;

        //healthbar
        public static int max_player_health = 5000;
        public static int health_gain = 500; //500
        public static int health_loss = -1000; //1
        public static int submarine_oxygen_cooldown = 4000;

        //submarine
        public static float max_run_velocity_submarine = 5f;
        public static float run_accelerate_submarine = 0.15f;
        public static float submarine_bomb_velocity = 1;
        public static int submarine_bomb_cooldown = 3000;
        public static float submarine_bullet_velocity = 20f;
        public static int submarine_machine_gun_cooldown = 3000; //milliseconds
        public static float machine_gun_turn_velocity = 1.5f;
        public static int machine_gun_shooting_frequency = 30; //lower is more bullets
        public static int submarine_light_cooldown = 2000;

        //darkness
        public static float light_width_scale = 1.2f;
        public static float light_height_scale = 1.5f;
        public static float waterplayer_light_width_scale = 1.5f;
        public static float waterplayer_light_height_scale = 1.1f;

        //dialogs
        public static float textspeed = 1;
        public static float text_scale = 1.3f;
        //one dialog consists of multiple tuples of strings, where one tuple corresponds to one page in the dialog
        //the first item in the tuple is the talking person(i.e. wp for waterplayer, sp for submarine player)
        //the second item is the corresponding text that appears in the current page (not more than 100 characters long)
        public static Tuple<string, string>[] dialog_test = { new Tuple<string, string>("wp", "test, test, test, test, test, test, test, test, test, test, test" ), new Tuple<string, string>("sp","1,2,3,4"), new Tuple<string,string>("wp" ,"nice it works" )};
        public static Tuple<string, string>[] dialog_first = { new Tuple<string, string>("sp", "Kenny! Kenny! Can you hear me?!!"),
                                                               new Tuple<string, string>("wp", "Yes, Yes, you don't have to scream."),
                                                               new Tuple<string, string>("sp", "Thank god, I thought you died after being sucked through the hole in the submarine."),
                                                               new Tuple<string, string>("sp", "But since you are out there you have to be fast, we need to collect the eggs we lost."),
                                                               new Tuple<string, string>("wp", "I will, but how did we hit that rock? I thought our navigation system is unfallible."),
                                                               new Tuple<string, string>("sp", "Well maybe someone didn't take the 'No drinks near the mainboard' rule serious enough"),
                                                               new Tuple<string, string>("wp", "..."),
                                                               new Tuple<string, string>("sp", "Nethertheless, let's focus on our task!"),
                                                               new Tuple<string, string>("sp", "First try to get on top of that rock to get a better view of the area."),
                                                               new Tuple<string, string>("sp", "Use the WASD keys to move around. And pay attention to the sea Urchin!")};
    }

}