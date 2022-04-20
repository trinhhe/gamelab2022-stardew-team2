using Microsoft.Xna.Framework;

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
        public static int health_gain = 500;
        public static int health_loss = 1;
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
    }

}