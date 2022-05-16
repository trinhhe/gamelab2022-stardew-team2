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
        public static float submarineplayer_max_run_velocity = 3.5f;
        public static float submarineplayer_run_accelerate = 0.15f;

        //healthbar
        public static int max_player_health = 5000;
        public static int health_gain = 2000; //500
        public static int health_loss = 1; //1
        public static int submarine_oxygen_cooldown = 20000;

        //submarine
        public static float max_run_velocity_submarine = 6.5f;
        public static float run_accelerate_submarine = 0.2f;
        public static float submarine_bomb_velocity = 2.75f;
        public static int submarine_bomb_cooldown = 3000;
        public static float submarine_bullet_velocity = 20f;
        public static int submarine_machine_gun_cooldown = 3000; //milliseconds
        public static float machine_gun_turn_velocity = 1.5f;
        public static int machine_gun_shooting_frequency = 30; //lower is more bullets
        public static int submarine_light_cooldown = 2000;

        public static void init_constants()
        {
            switch (Game.CurrDifficulty)
            {
                case Game.Diffculty.Easy:
                    max_player_health = 6000;
                    health_gain = 4000;
                    submarine_oxygen_cooldown = 12000;
                    submarineplayer_max_run_velocity = 4.5f;
                    submarineplayer_run_accelerate = 0.3f;
                    Level1.spawn_timer = 16000;
                    Level2.spawn_timer = 18000;
                    break;
                case Game.Diffculty.Medium:
                    max_player_health = 5000;
                    health_gain = 2500;
                    submarine_oxygen_cooldown = 20000;
                    submarineplayer_max_run_velocity = 3.5f;
                    submarineplayer_run_accelerate = 0.2f;
                    Level1.spawn_timer = 13000;
                    Level2.spawn_timer = 16000;
                    break;
                case Game.Diffculty.Hard:
                    max_player_health = 4000;
                    health_gain = 1500;
                    submarine_oxygen_cooldown = 17500;
                    submarineplayer_max_run_velocity = 3f;
                    submarineplayer_run_accelerate = 0.15f;
                    Level1.spawn_timer = 8000;
                    Level2.spawn_timer = 12000;
                    break;
                default:
                    break;
            }
        }

        //darkness
        public static float light_width_scale = 1.2f;
        public static float light_height_scale = 1.1f;
        public static float waterplayer_light_width_scale = 1.2f;
        public static float waterplayer_light_height_scale = 1.1f;
        public static int darkness_transition_steps = 5; //steps from 0 to 255 alpha value darkness transition in boss fight

        //SFX VOLUMES
        //player
        public static float grunt_volume = 0.6f;
        public static float swim_volume = 0.8f;
        public static float jump_volume = 1f;
        //boss attacks
        public static float spatial_electricity_volume = 0.20f;
        public static float electro_attack_volume = 0.4f;
        public static float win_volume= 0.5f;

        //dialogs
        public static float textspeed = 1;
        public static float text_scale = 1.3f;
        //one dialog consists of multiple tuples of strings, where one tuple corresponds to one page in the dialog
        //the first item in the tuple is the talking person(i.e. wp for waterplayer, sp for submarine player)
        //the second item is the corresponding text that appears in the current page (not more than 100 characters long)
        public static Tuple<string, string>[] dialog_test = { new Tuple<string, string>("wp", "test, test, test, test, test, test, test, test, test, test, test" ), new Tuple<string, string>("sp","1,2,3,4"), new Tuple<string,string>("wp" , "Well maybe someone didn't take the 'No drinks near the mainboard' rule serious enough") };
        public static Tuple<string, string>[] dialog_first = { new Tuple<string, string>("sp", "Kenny! Kenny! Can you hear me?!!"),
                                                               new Tuple<string, string>("wp", "Yes, Yes, you don't have to shout."),
                                                               new Tuple<string, string>("sp", "Thank god, I thought you died after being sucked through the hole in the submarine."),
                                                               new Tuple<string, string>("sp", "But since you are out there you have to be fast, we need to collect the eggs we lost."),
                                                               new Tuple<string, string>("wp", "I will, but how did we hit that rock? I thought our navigation system is unfallible."),
                                                               new Tuple<string, string>("sp", "Well maybe someone didn't take the 'No drinks near the mainboard' rule serious enough."),
                                                               new Tuple<string, string>("wp", "..."),
                                                               new Tuple<string, string>("sp", "Nevertheless, let's focus on our task!"),
                                                               new Tuple<string, string>("sp", "First try to get on top of that rock to get a better view of the area."),
                                                               new Tuple<string, string>("sp", "Use the WASD keys. Move left and right with A and D."),
                                                               new Tuple<string, string>("sp", "You can jump with W and crouch with S."),
                                                               new Tuple<string, string>("sp", "Now move! And pay attention to the sea urchin above!"),};
        public static Tuple<string, string>[] dialog_submarine = { new Tuple<string, string>("wp", "It seems that the eggs are scattered all over the ocean floor."),
                                                               new Tuple<string, string>("sp", "We still need to gather as many as we possibly can, all of humanity is counting on us."),
                                                               new Tuple<string, string>("sp", "Unfortunately, you took the last suit and I can't help you."),
                                                               new Tuple<string, string>("wp", "Actually you can, you could just operate the submarine with all of its functionalities."),
                                                               new Tuple<string, string>("wp", "Use the oxygen station on the leftmost edge of the submarine to refill my oxygen tank."),
                                                               new Tuple<string, string>("wp", "However, it only fills up a certain amount each time and has a pretty long cooldown."),
                                                               new Tuple<string, string>("wp", "To the right of it is the bomb station. It drops bombs that destroy rocks and kill blowfish."),
                                                               new Tuple<string, string>("wp", "See that red crosshair directly under the bomb station?"),
                                                               new Tuple<string, string>("wp", "It shows you where the bomb will land."),
                                                               new Tuple<string, string>("sp", "(realizing she actually has to do work)"),
                                                               new Tuple<string, string>("wp", "To the right of the bomb station is the light station."),
                                                               new Tuple<string, string>("wp", "You can use it to illuminate dark areas. You'll need that later."),
                                                               new Tuple<string, string>("wp", "Next to it you have the control station, which moves the submarine."),
                                                               new Tuple<string, string>("wp", "Use the machine gun on the rightmost edge of the submarine to deal damage to blowfish."),
                                                               new Tuple<string, string>("wp", "Do you remember how to activate them?"),
                                                               new Tuple<string, string>("sp", "Of course, I can move around and interact with stations using the arrow keys."),
                                                               new Tuple<string, string>("sp", "The up arrow key allows me to enter and leave the stations."),
                                                               new Tuple<string, string>("sp", "And I can aim with the machine gun by moving the mouse."),
                                                               new Tuple<string, string>("wp", "If you still need help, click the tutorial button in the main menu."),
                                                               new Tuple<string, string>("wp", "You can access it at any time. Hit the ESC key to bring up the menu."),
                                                               new Tuple<string, string>("sp", "Gotcha!"),
                                                               new Tuple<string, string>("wp", "Perfect, now let us get these eggs.")};
        public static Tuple<string, string>[] dialog_maze = { new Tuple<string, string>("sp", "The computer is telling me that your jet-pack has reloaded."),
                                                               new Tuple<string, string>("sp", "You can now move much more freely around than before."),
                                                               new Tuple<string, string>("wp", "Oh, that's nice, however I can't see anything around here now."),
                                                               new Tuple<string, string>("sp", "I can help you there, the submarine possesses a light station."),
                                                               new Tuple<string, string>("wp", "Wow, Maya you are actually really helpful right now, what happened?"),
                                                               new Tuple<string, string>("sp", "I realized, that pretending we would live in a computer game,"),
                                                               new Tuple<string, string>("sp", "motivates me a lot more than doing actual work."),
                                                               new Tuple<string, string>("wp", "Living in a computer game? What a weird thought though.")};
        public static Tuple<string, string>[] dialog_second = { new Tuple<string, string>("wp", "It seems to get darker and darker."),
                                                               new Tuple<string, string>("sp", "Yeah, it sure reminds me of my soul."),
                                                               new Tuple<string, string>("wp", "..."),
                                                               new Tuple<string, string>("sp", "..."),
                                                               new Tuple<string, string>("wp", "...")};
        public static Tuple<string, string>[] dialog_torch = { new Tuple<string, string>("wp", "Maya, can you see this."),
                                                               new Tuple<string, string>("sp", "Do I have to remind you, that everything here is completely dark."),
                                                               new Tuple<string, string>("wp", "I mean the torch here right next to me."),
                                                               new Tuple<string, string>("sp", "Oh, you mean that big fancy thing, they always use at the underwater olympics."),
                                                               new Tuple<string, string>("wp", "Exactly, it uses a special chemical reaction, such that the flames can burn underwater!"),
                                                               new Tuple<string, string>("wp", "Maybe, we can enlight the whole area using these!"),
                                                               new Tuple<string, string>("wp", "Try shooting at them with a bomb or bullet, that should be enough."),
                                                               new Tuple<string, string>("sp", "Roger that.")};
        public static Tuple<string, string>[] dialog_torch_hit = { new Tuple<string, string>("wp", "It works! I can see much more now."),
                                                               new Tuple<string, string>("sp", "Somehow it is weird to find these torches here, I mean why should they be here."),
                                                               new Tuple<string, string>("wp", "True, it's almost like someone placed them there to help us.")};
        public static Tuple<string, string>[] dialog_boss = { new Tuple<string, string>("wp", "WHAT IS THIS????!!!!!"),
                                                               new Tuple<string, string>("sp", "Seems like a giant frog fish and it seems angry."),
                                                               new Tuple<string, string>("wp", "WHAT CAN WE DO TO CALM IT DOWN??!!"),
                                                               new Tuple<string, string>("sp", "Normally, we have to fight this, I can start by throwing bombs at its antenna."),
                                                               new Tuple<string, string>("sp", "According to my video game knowledge this must be its weak point."),
                                                               new Tuple<string, string>("wp", "OK,Ok, but tell me, HOW CAN YOU STAY THIS CALM???"),
                                                               new Tuple<string, string>("sp", "Up until now not a single fish tried to attack the submarine, so I'm pretty chill."),
                                                               new Tuple<string, string>("wp", "I hate you.")};
        public static Tuple<string, string>[] dialog_boss_hit = { new Tuple<string, string>("wp", "That's it, it's down!!"),
                                                               new Tuple<string, string>("sp", "I think it's barrely stun, now we have to deal damage to it."),
                                                               new Tuple<string, string>("sp", "You can start by taking some dynamite from the barrel, that is luckily placed there"),
                                                               new Tuple<string, string>("sp", "and throwing it at the stun fish, both action can be done by pressing E."),
                                                               new Tuple<string, string>("wp", "At the same time, you can try shooting it with your machine gun."),
                                                               new Tuple<string, string>("sp", "And Kenny,"),
                                                               new Tuple<string, string>("sp", "Please pay attention that the dynamite does not explode near you"),
                                                               new Tuple<string, string>("wp", "Since when am I the careless person between us two?")};
        public static Tuple<string, string>[] dialog_boss_stage = { new Tuple<string, string>("wp", "Its health was completely down, how can  it regenerate so fast"),
                                                               new Tuple<string, string>("sp", "Have you never played any video games before?"),
                                                               new Tuple<string, string>("sp", "A bossfight like this consists of multiple stages."),
                                                               new Tuple<string, string>("wp", "Normally, I would say, that we are not in a video game, but now I'm greatful for any information."),
                                                               new Tuple<string, string>("sp", "Okay, the boss should become faster and stronger after each cleared stage."),
                                                               new Tuple<string, string>("sp", "It probably shows new attacks as well, like turning everything really dark for a short period"),
                                                               new Tuple<string, string>("wp", "That was oddly specific. How many stages are there normally in your game."),
                                                               new Tuple<string, string>("sp", "Most of the time only 3.")};
        public static Tuple<string, string>[] dialog_boss_final = { new Tuple<string, string>("wp", "This must be it, a final effort and we    will defeat it!!"),
                                                               new Tuple<string, string>("wp", "I want you to know, that no matter what happens, it was an honor to fight beside you."),
                                                               new Tuple<string, string>("sp", "..."),
                                                               new Tuple<string, string>("wp", "MAYA!!! ARE YOU EVEN LISTENING??"),
                                                               new Tuple<string, string>("sp", "Oh, yeah, I was totally focused and not watching a movie on the side."),
                                                               new Tuple<string, string>("wp", "Sometimes, I wonder how we managed to survive until now.")};
    }

}