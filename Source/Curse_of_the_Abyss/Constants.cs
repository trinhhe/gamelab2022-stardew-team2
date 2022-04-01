namespace Curse_of_the_Abyss 
{ 

    public static class Constants{
        public static float max_run_velocity = 6;
        public static float jump_velocity = -3;
        public static float fall_velocity = 0.15f;
        public static float max_y_velocity = 5;
        public static float run_accelerate = 0.1f;
        public static float max_jumping_height = 7;

        public static float max_run_velocity_submarine = 3;
        public static float run_accelerate_submarine = 0.1f;
        public static int health_gain = 4;
        public static float submarine_bomb_velocity = 2f;
        public static int submarine_bomb_cooldown = 3000; //milliseconds
        public static float submarine_bullet_velocity = 4f;
        public static int submarine_machine_gun_cooldown = 3000; //milliseconds
        public static float machine_gun_turn_velocity = 2.5f;
        public static int machine_gun_shooting_frequency = 25; //lower is more bullets
    }

}