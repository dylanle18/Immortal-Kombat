//Dylan Le
//ICS 4U1
//Final Game: Immortal Kombat

using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.GamerServices;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Media;

namespace FinalGame
{
    /// <summary> 
    /// </summary>
    public class Game1 : Microsoft.Xna.Framework.Game
    {
        GraphicsDeviceManager graphics;
        SpriteBatch spriteBatch;

        GamePadState pad1, oldpad1 , pad2, oldpad2;

        //rectangles and pics for menu
        Rectangle menuBackgroundRec; //rec for the menu background
        Texture2D[] menuBackgroundPic = new Texture2D[8]; //in array because the background will be animating
        Rectangle immortalTitleRec; //rec for the word immortal for the title
        Texture2D immortalTitlePic; //pic of the word
        Rectangle kombatTitleRec; //rec for kombat title
        Texture2D kombatTitlePic; //pic saying kombat
        Rectangle menuOptionsRec; //rec for al the menu options
        Texture2D menuOptionsPic; //pic saying all the different options
        Rectangle pointerRec; //rec for option pointer
        Texture2D pointerPic; //pic of pointer so user knows which option they are on

        int screenCounter = 0; //to keep track of what screen to play / update
        int optionCounter = 1; //keeps track of which option the user is on in the menu
        double picCounter = 0; //will allow the background for the menu and game to animate

        Rectangle gameBackgroundRec; //rec for the game background
        Texture2D[] gameBackgroundPic = new Texture2D[8]; //array of all the game background pics
        Rectangle fightRec; //rec for fight pic
        Texture2D fightPic; //pic of the word fight for start of each round

        Character[] player = new Character[2]; //one fighter for each player
        Vector2[] playerPos = new Vector2[2]; //positions for the each player
        Texture2D[] playerPic = new Texture2D[2]; //holds each players sprite
        const int playerSpeed = 3; //speed for the players will be constant and the same for both
        Rectangle[] healthBarRec = new Rectangle[2]; //rec for the health bars for each player
        Texture2D[] healthBarPic = new Texture2D[2]; //pic of the health bars
        Rectangle[] healthValueRec = new Rectangle[2]; //rec for the health inside the health bars
        Texture2D[] healthValuePic = new Texture2D[2]; //of the bars
        Rectangle[] energyBarRec = new Rectangle[2]; //same as health bars but for energy for each player
        Texture2D[] energyBarPic = new Texture2D[2]; //^^^^^^^^
        Rectangle[] energyValueRec = new Rectangle[2]; //^^^^^^^^
        Texture2D[] energyValuePic = new Texture2D[2]; //^^^^^^^^

        Rectangle practiceBackgroundRec; //rectangle for the practice background screen
        Texture2D practiceBackgroundPic; //pic of the practice background

        TrainingDummy dummy; //create the training dummy for practice screen
        Texture2D dummyPic; //picture of the training dummy
        Vector2 dummyPos; //holds the postion of the training dummy
        Rectangle dummyHealthBarRec; //rec to hold the pic of the health bar for dummy
        Texture2D dummyHealthBarPic; //pic of the dummy health
        Rectangle dummyHealthValueRec; //rec for the balue of the dummy health
        Texture2D dummyHealthValuePic; //the picture of the dummy health

        Rectangle countDownRec; //rectangle to hold the pic for the count down at start of each round
        Rectangle countDownSourceRec; //source rec for the count down
        Texture2D countDownPic; //holds the count down pic

        int roundNum = 1; //keep track of what round the game is on
        int[] playerWins = new int[2]; //to keep track of how many wins each player has
        Rectangle roundRec; //rec for rounds pic
        Texture2D roundPic; //pic to orginize the round and wins of each player
        Rectangle playerWinRec; //rectangle to display which player is the winner
        Texture2D[] playerWinPic = new Texture2D[2]; //holds picture of the winner for each player

        float timer; //keeps track of the time of each round
        float wordWidth; //finds the word width to help position labels

        Rectangle controlsRec; //rec to hold the controls pic
        Texture2D controlsPic; //pic that tells the user the controls
        Rectangle aboutRec; //rec for the about pic
        Texture2D aboutPic; //pic containing stuff about the author

        SoundEffect countdownFightSound; //sound saying 321 fight plays at start of round
        SoundEffect clickSound; //sound in menu when selecting options
        Song backgroundMusic; //background music that will play during the game

        SpriteFont font; //font for labels, like players, rounds, etc.
        public Game1()
        {
            graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
        }

        /// <summary>
        /// Allows the game to perform any initialization it needs to before starting to run.
        /// This is where it can query for any required services and load any non-graphic
        /// related content.  Calling base.Initialize will enumerate through any components
        /// and initialize them as well.
        /// </summary>
        protected override void Initialize()
        {
            //sets the title of the window
            Window.Title = "Immortal Kombat";
            //changes the size of the screen
            graphics.PreferredBackBufferWidth = 800;
            graphics.PreferredBackBufferHeight = 450;
            graphics.ApplyChanges();

            //sizes and position all the rectangles for the menu screen
            menuBackgroundRec = new Rectangle(0, 0, GraphicsDevice.Viewport.Width, GraphicsDevice.Viewport.Height);
            immortalTitleRec = new Rectangle(-500, 0, 500, 80);
            kombatTitleRec = new Rectangle(800, 50, 400, 80);
            menuOptionsRec = new Rectangle(10, 240, 200, 200);
            //starts the pointer at the play option
            pointerRec = new Rectangle(110, 245, 30, 25);

            //makes the background rec the same size as the screen
            gameBackgroundRec = new Rectangle(0, 0, GraphicsDevice.Viewport.Width, GraphicsDevice.Viewport.Height);
            //postion and sizes the the rec that will be displaying FIGHT at start of rounds
            fightRec = new Rectangle(GraphicsDevice.Viewport.Width / 2 - 300, GraphicsDevice.Viewport.Height / 2 - 130, 600, 300);

            //sets the postion for each player
            playerPos[0] = new Vector2(10, 200);
            playerPos[1] = new Vector2(GraphicsDevice.Viewport.Width - 135, 200);

            //sets the size and pos for each players health bars
            healthBarRec[0] = new Rectangle(0, 25, 300, 20); //player 1
            healthValueRec[0] = new Rectangle(0, 25, 300, 20);
            
            healthBarRec[1] = new Rectangle(500, 25, 300, 20); //player 2
            healthValueRec[1] = new Rectangle(500, 25, 300, 20);
            //sets the size and pos for each players energy bars
            energyBarRec[0] = new Rectangle(0, 41, 300, 20); //player 1
            energyValueRec[0] = new Rectangle(-300, 41, 300, 20);

            energyBarRec[1] = new Rectangle(500, 41, 300, 20); //player 2
            energyValueRec[1] = new Rectangle(800, 41, 300, 20);

            //postion for the training dummy center of the screen, horizontally
            dummyPos = new Vector2(GraphicsDevice.Viewport.Width / 2 - 75, 220);
            //pos and size helth bar for dummy, top right
            dummyHealthBarRec = new Rectangle(500, 25, 300, 20);
            dummyHealthValueRec = new Rectangle(500, 25, 300, 20);

            //makes background rec for practice screen size of whole screen
            practiceBackgroundRec = new Rectangle(0, 0, GraphicsDevice.Viewport.Width, GraphicsDevice.Viewport.Height);

            //pos and size rec that will display 3 2 1 at start of each round
            countDownRec = new Rectangle(GraphicsDevice.Viewport.Width / 2 - 100, GraphicsDevice.Viewport.Height / 2 - 100, 200, 200);
            //source rec for countdown picture, so it only displays 1 number at a time
            countDownSourceRec = new Rectangle(0, 0, 100, 100);

            //pos and size the round / wins orginizer
            roundRec = new Rectangle(GraphicsDevice.Viewport.Width / 2 - 49, 25, 98, 34);
            //makes the winner rec same size as the background
            playerWinRec = gameBackgroundRec;
            //pos and sizes the controlls & about rec so its centered
            controlsRec = new Rectangle(GraphicsDevice.Viewport.Width / 2 - 300, 0, 600, 450);
            aboutRec = controlsRec;

            base.Initialize();
        }

        /// <summary>
        /// LoadContent will be called once per game and is the place to load
        /// all of your content.
        /// </summary>
        protected override void LoadContent()
        {
            // Create a new SpriteBatch, which can be used to draw textures.
            spriteBatch = new SpriteBatch(GraphicsDevice);

            //loads the pictures for the menu screen
            immortalTitlePic = Content.Load<Texture2D>("menuScreenPics/immortal");
            kombatTitlePic = Content.Load<Texture2D>("menuScreenPics/kombat");
            menuOptionsPic = Content.Load<Texture2D>("menuScreenPics/menuOptions");
            pointerPic = Content.Load<Texture2D>("menuScreenPics/pointer");

            //loads the pictures for the 321 fight at start of round
            countDownPic = Content.Load<Texture2D>("gameScreenPics/countDown");
            fightPic = Content.Load<Texture2D>("gameScreenPics/fight");
            //loads the sprites, health and energy bar pictures for each player
            playerPic[0] = Content.Load<Texture2D>("players/player1Sprite");
            playerPic[1] = Content.Load<Texture2D>("players/player2Sprite");
            healthBarPic[0] = Content.Load<Texture2D>("players/healthBar");
            healthValuePic[0] = Content.Load<Texture2D>("players/healthValue");
            healthBarPic[1] = Content.Load<Texture2D>("players/healthBarFlipped");
            healthValuePic[1] = Content.Load<Texture2D>("players/healthValueFlipped");
            energyBarPic[0] = Content.Load<Texture2D>("players/energyBar");
            energyValuePic[0] = Content.Load<Texture2D>("players/energyValue");
            energyBarPic[1] = Content.Load<Texture2D>("players/energyBarFlipped");
            energyValuePic[1] = Content.Load<Texture2D>("players/energyValueFlipped");

            //loads all the pictures for the training dummy
            dummyPic = Content.Load<Texture2D>("practiceScreenPics/trainingDummy");
            dummyHealthBarPic = Content.Load<Texture2D>("players/healthBarFlipped");
            dummyHealthValuePic = Content.Load<Texture2D>("players/healthValueFlipped");

            //loads the background picture for practice screen
            practiceBackgroundPic = Content.Load<Texture2D>("practiceScreenBackground/practiceBackground");

            //loops 8 times because there are 8 pics for each screens background
            for (int i = 0; i < 8; i++)
            {
                //load each picture into each array to later animation
                menuBackgroundPic[i] = Content.Load<Texture2D>("menuScreenBackground/menuBackground (" + ( i + 1 ) + ")");
                gameBackgroundPic[i] = Content.Load<Texture2D>("gameScreenBackground/gameBackground (" + (i + 1) + ")");
            }

            //instantiates each player
            player[0] = new Character(playerPic[0], playerPos[0], 300, 2, 0, playerSpeed, false, Content);
            player[1] = new Character(playerPic[1], playerPos[1], 300, 2, 0, playerSpeed, true, Content);

            //instantiate the trainging dummy
            dummy = new TrainingDummy(dummyPic, dummyPos, 300, 2, 0, Content);

            //loads the pictures that will display when a player wins
            playerWinPic[0] = Content.Load<Texture2D>("gameScreenPics/player1Wins");
            playerWinPic[1] = Content.Load<Texture2D>("gameScreenPics/player2Wins");

            //loads the font for the labels
            font = Content.Load<SpriteFont>("font");

            //loads the rounds organizer picture
            roundPic = Content.Load<Texture2D>("gameScreenPics/rounds");
            //loads the controls and about pictures
            controlsPic = Content.Load<Texture2D>("infoScreenPics/controls");
            aboutPic = Content.Load<Texture2D>("infoScreenPics/about");
            //loads the sounds for menu and 321 fight
            countdownFightSound = Content.Load<SoundEffect>("sounds/321fight");
            clickSound = Content.Load<SoundEffect>("sounds/click");
            //loads the background music and sets the value of player to 0.1 so its not overwhelming
            backgroundMusic = Content.Load<Song>("sounds/music");
            MediaPlayer.Volume = 0.1f;
        }

        /// <summary>
        /// UnloadContent will be called once per game and is the place to unload
        /// all content.
        /// </summary>
        protected override void UnloadContent()
        {
            // TODO: Unload any non ContentManager content here
        }

        /// <summary>
        /// Allows the game to run logic such as updating the world,
        /// checking for collisions, gathering input, and playing audio.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Update(GameTime gameTime)
        {
            //gets the state of each gamepad
            pad1 = GamePad.GetState(PlayerIndex.One);
            pad2 = GamePad.GetState(PlayerIndex.Two);

            //moves the health bars acordingly to how much health each player or dummy has
            //health bar on right side of screen
            healthValueRec[0].X = (int)player[0].getHealth() - 300;
            energyValueRec[0].X = (int)player[0].getEnergy() - 300;
            //health bars that are on the left side of the screen
            healthValueRec[1].X = 800 - (int)player[1].getHealth();
            energyValueRec[1].X = 800 - (int)player[1].getEnergy();
            dummyHealthValueRec.X = 800 - (int)dummy.getHealth();

            //if its displaying the game screen or practice screen
            if (screenCounter == 1 || screenCounter == 2)
            {
                timer += gameTime.ElapsedGameTime.Milliseconds; //start the timer
                //make sure they cant move or take damage before the game starts
                if (timer < 3000)
                {
                    for (int i = 0; i < 2; i++)
                    {
                        player[i].setHealth(300);
                        player[i].setEnergy(0);
                        player[i].setPos(playerPos[i]);
                    }
                }
            }
            else //if its displaying anyother screen the timer will stop and be set to 0
            {
                timer = 0;
            }

            //slowly cycle which picture to draw for the background
            picCounter += 0.2;
            if (picCounter > 7) //if its on the last pic
            {
                picCounter = 0; //loops back to the start
            }

            //loops twice for the number of players
            for (int i = 0; i < 2; i++)
            {
                //if its not on game screen or playing the 321 fight
                if (timer > 4000 || playerWins[i] == 3 || screenCounter == 3 || screenCounter == 4)
                {
                    //allow them to press back for the menu
                    if (pad1.Buttons.Back == ButtonState.Pressed && screenCounter != 0 && screenCounter != 1 || pad1.Buttons.Back == ButtonState.Pressed && playerWins[i] == 3)
                    {
                        //displays the menu screen
                        screenCounter = 0;
                        //plays the selecting sound
                        clickSound.Play();
                        //resets all the values for the players and training dummy when returned to the menu
                        immortalTitleRec.X = -600;
                        kombatTitleRec.X = 800;
                        roundNum = 1;
                        playerWins[0] = 0;
                        playerWins[1] = 0;
                        player[0].setIsleft(false);
                        player[1].setIsleft(true);
                        dummy.setHealth(300);
                        dummy.setTotalDamage(0);
                    }
                }
            }

            //if a player hasnt won or on pratice screen
            if (playerWins[0] < 3 && playerWins[1] < 3 && screenCounter == 1 || screenCounter == 2)
            {
                //play the 321 fight sound at start of each round
                if (timer <= 20)
                {
                    countdownFightSound.Play();
                }
            }

            //if menu screen
            if (screenCounter == 0)
            {
                pauseMusic(); //stops the game music
                menu(); //calls the menu method
            }
            //if game screen
            else if (screenCounter == 1)
            {
                playMusic(); //plays the game music
                game(gameTime); //calls the game method
            }
            //if practice screen
            else if (screenCounter == 2)
            {
                playMusic(); //play the game music
                practice(gameTime); //call the practice method
            }

            //sets old pad to what player game pad start 1 tick before
            oldpad1 = pad1;
            oldpad2 = pad2;

            base.Update(gameTime);
        }

        public void playMusic() //play music method
        {
            if (MediaPlayer.State != MediaState.Playing) //if its not already playing
            {
                MediaPlayer.Play(backgroundMusic); //then play it
            }
        }

        public void pauseMusic() //pause music method
        {
            if (MediaPlayer.State == MediaState.Playing) //if it is playing
            {
                MediaPlayer.Pause(); //then pause it
            }
        }

        public void menu() //the menu method
        {
            //if up on the d pad is pressed once
            if (pad1.DPad.Up == ButtonState.Pressed && oldpad1.DPad.Up == ButtonState.Released)
            {
                clickSound.Play(); //playings the select sound
                optionCounter--; //makes the option the one above it
            }
            else if (pad1.DPad.Down == ButtonState.Pressed && oldpad1.DPad.Down == ButtonState.Released)
            {
                clickSound.Play(); //playings the select sound
                optionCounter++; //makes the option the one below it
            }
            if (optionCounter == 6) //loops to the first option
            {
                optionCounter = 1;
            }
            else if (optionCounter == 0) //loops down to the last option
            {
                optionCounter = 5;
            }

            //whatever the menu option is it will move the pointer to the correct option so
            //the user knows what option they are on
            if (optionCounter == 1)
            {
                //moves the pointer
                pointerRec.X = 110;
                pointerRec.Y = 244;
            }
            else if (optionCounter == 2)
            {
                pointerRec.X = 200;
                pointerRec.Y = 286;
            }
            else if (optionCounter == 3)
            {
                pointerRec.X = 220;
                pointerRec.Y = 326;
            }
            else if (optionCounter == 4)
            {
                pointerRec.X = 145;
                pointerRec.Y = 368;
            }
            else if (optionCounter == 5)
            {
                pointerRec.X = 105;
                pointerRec.Y = 408;
            }

            //moves the immortal and kombat titles to the menu of the screen
            //looks like they are coming from outside the screen
            if (immortalTitleRec.X < 140)
            {
                immortalTitleRec.X += 12;
            }
            if (kombatTitleRec.X > 200)
            {
                kombatTitleRec.X -= 10;
            }
            //if the a button is pressed
            if (pad1.Buttons.A == ButtonState.Pressed)
            {
                //if will make the screen whatever option the user selected
                if (optionCounter == 1) //game scren
                {
                    screenCounter = 1;
                }
                else if (optionCounter == 2) //practice screen
                {
                    screenCounter = 2;
                }
                else if (optionCounter == 3) //controls screen
                {
                    screenCounter = 3;
                }
                else if (optionCounter == 4) //about screen
                {
                    screenCounter = 4;
                }
                else if (optionCounter == 5) //quit option
                {
                    MediaPlayer.Stop(); //stops the media player
                    this.Exit(); //exits the program
                }
            }
        }

        public void game(GameTime gameTime) //game screen method
        {
            if (playerWins[0] < 3 && playerWins[1] < 3) //if no player won yet
            {
                //call the updates methods for each character
                player[0].PlayerUpdate(gameTime, pad1, oldpad1, timer);
                player[1].PlayerUpdate(gameTime, pad2, oldpad2, timer);
                //calls the methods for the health and energy bars for each player
                gameBars(0, 1);
                gameBars(1, 0);
            }
        }

        public void practice(GameTime gameTime) //practice screen method
        {
            //only updates players one character in practice screen
            player[0].PlayerUpdate(gameTime, pad1, oldpad1, timer);
            //displays health and energy for player 1
            gameBars(0, 1);
            //calls upodate for the training dummt
            dummy.DummyUpdate(gameTime);
        }

        //for the health and energy bars of the players
        public void gameBars(int num1, int num2) //num1 and num2 and for what number each player is in the array of, 0 = player 1 and 1 = player 2
        {
            //if on the game screen
            if (screenCounter == 1)
            {
                //detects if a player its another player while the other player is blocking
                if (player[num1].getAttackHitBox().Intersects(player[num2].getHitBox()) && player[num2].getBlock() == true)
                {
                    player[num2].DoBlockDamage(); //player getting hit takes damage, not as much because blocking
                    player[num1].attackEnergy(); //attacking player gets more energy
                    player[num2].damageEnergy(); //then the other player
                }
                //detects if a player hits another player while they are not blocking
                else if (player[num1].getAttackHitBox().Intersects(player[num2].getHitBox()) && player[num2].getBlock() == false)
                {
                    player[num2].DoDamage(); //deals the regular amount of damage
                    player[num1].attackEnergy();
                    player[num2].damageEnergy();
                }
                for (int i = 0; i < (player[num1].getEnergyBallAmount()); i++) //loops for the amount of energy balls there are bot that player
                {
                    //if one its another playing while they are playing
                    if (player[num1].getEnergyBall(i).Intersects(player[num2].getHitBox()) && player[num2].getBlock() == true)
                    {
                        player[num2].DoBlockEnergyBallDamage(); //deals blocking damage to player getting hit
                    }
                    //if they arent blocking and get hit
                    if (player[num1].getEnergyBall(i).Intersects(player[num2].getHitBox()) && player[num2].getBlock() == false)
                    {
                        player[num2].DoEnergyBallDamage(); //deal the regular energy ball damage
                    }
                }
                //if a player health gets to 0
                if (player[num1].getHealth() <= 0)
                {
                    playerWins[num2]++; //then the other player wins
                    timer = 0; //timer resets because next round
                    roundNum++; //adds 1 to see what round they are on
                    //makes each player face in the correct direction again
                    player[0].setIsleft(false);
                    player[1].setIsleft(true);
                }
            }
            //if the user is on the practice screen
            if (screenCounter == 2)
            {
                //regenerates the dummy health when below 300 health
                if (dummy.getHealth() <= 300)
                {
                    dummy.setHealth(dummy.getHealth() + 0.3); //regenerates 0.3 each tic
                }
                if (dummy.getHealth() < 0) //so the dummy cant go below 0 health
                {
                    dummy.setHealth(0);
                }
                //if the dummy gets hit deal damage and add energy to the player
                if (player[num1].getAttackHitBox().Intersects(dummy.getHitBox()))
                {
                    dummy.DoDamage();
                    dummy.addAttackDamage(); //adds the amount of the damage dealt to the total damage
                    player[num1].attackEnergy();
                }
                //loops for amount of energy balls there are on the screen
                for (int i = 0; i < (player[num1].getEnergyBallAmount()); i++)
                {
                    //if the dummy gets it deal the energy ball damage and add to total damage
                    if (player[num1].getEnergyBall(i).Intersects(dummy.getHitBox()))
                    {
                        dummy.DoEnergyBallDamage();
                        dummy.addEnergyDamage();
                    }
                }
            }
            //so the player cant go over 300 energy
            if (player[num1].getEnergy() >= 300)
            {
                player[num1].setEnergy(300);
            }
        }

        /// <summary>
        /// This is called when the game should draw itself.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.Black);
            spriteBatch.Begin();

            //draws everything for the menu screen
            if (screenCounter == 0)
            {
                //cycles though the background pics
                spriteBatch.Draw(menuBackgroundPic[(int)picCounter], menuBackgroundRec, Color.White);
                spriteBatch.Draw(immortalTitlePic, immortalTitleRec, Color.White);
                spriteBatch.Draw(kombatTitlePic, kombatTitleRec, Color.White);
                spriteBatch.Draw(menuOptionsPic, menuOptionsRec, Color.White);
                spriteBatch.Draw(pointerPic, pointerRec, Color.White);
                //draws messages showing the user how to navigate the menu
                wordWidth = font.MeasureString("USE \"D-PAD\" TO NAVIGATE").X;
                spriteBatch.DrawString(font, "USE \"D-PAD\" TO NAVIGATE", new Vector2(GraphicsDevice.Viewport.Width - wordWidth - 5, GraphicsDevice.Viewport.Height - 50), Color.White);
                wordWidth = font.MeasureString("PRESS \"A\" TO SELECT").X;
                spriteBatch.DrawString(font, "PRESS \"A\" TO SELECT", new Vector2(GraphicsDevice.Viewport.Width - wordWidth - 5, GraphicsDevice.Viewport.Height - 25), Color.White);
            }
            //draws everything for the game screen
            else if (screenCounter == 1)
            {
                //cycles though the game background pics
                spriteBatch.Draw(gameBackgroundPic[(int)picCounter], gameBackgroundRec, Color.White);

                if (playerWins[0] == 3) //if player 1 wins
                {
                    spriteBatch.Draw(playerWinPic[0], playerWinRec, Color.White); //display player 1 wining picture
                }
                else if (playerWins[1] == 3) //if player 2 wins
                {
                    spriteBatch.Draw(playerWinPic[1], playerWinRec, Color.White); //display player 2 winning picture
                }
                if (playerWins[0] < 3 && playerWins[1] < 3) //if no one won yet
                {
                    for (int i = 0; i < 2; i++) //loops for number of players
                    {
                        player[i].PlayerDraw(spriteBatch); //draw each player
                        //draws each players health and energy bars
                        spriteBatch.Draw(healthValuePic[i], healthValueRec[i], Color.White);
                        spriteBatch.Draw(healthBarPic[i], healthBarRec[i], Color.White);
                        spriteBatch.Draw(energyValuePic[i], energyValueRec[i], Color.White);
                        spriteBatch.Draw(energyBarPic[i], energyBarRec[i], Color.White);
                    }
                    //draws the rounds and wins organizer
                    spriteBatch.Draw(roundPic, roundRec, Color.White);
                    spriteBatch.DrawString(font, "Round " + roundNum, new Vector2(358, 0), Color.White); //displays what round they are on
                    spriteBatch.DrawString(font, Convert.ToString(playerWins[0]), new Vector2(360, 30), Color.White); //says how many wins player 1 has
                    spriteBatch.DrawString(font, Convert.ToString(playerWins[1]), new Vector2(420, 30), Color.White); //says how many wins player 2 has
                    spriteBatch.DrawString(font, "PLAYER 1", new Vector2(0, 0), Color.White); //labels which health bar is player 1 and 2s
                    wordWidth = font.MeasureString("PLAYER 2").X; //help position player 2 label at right side of screen
                    spriteBatch.DrawString(font, "PLAYER 2", new Vector2(GraphicsDevice.Viewport.Width - wordWidth - 2, 0), Color.White);
                }
            }
            //draws everything for the practice screen
            else if (screenCounter == 2)
            {
                //draws the practice screen background
                spriteBatch.Draw(practiceBackgroundPic, practiceBackgroundRec, Color.White);
                dummy.DummyDraw(spriteBatch); //draws the trainging dummy
                player[0].PlayerDraw(spriteBatch); //draws only player 1
                //draws the health and energy bars for player 1
                spriteBatch.Draw(healthValuePic[0], healthValueRec[0], Color.White);
                spriteBatch.Draw(healthBarPic[0], healthBarRec[0], Color.White);
                spriteBatch.Draw(energyValuePic[0], energyValueRec[0], Color.White);
                spriteBatch.Draw(energyBarPic[0], energyBarRec[0], Color.White);
                //draws the player 1 label
                spriteBatch.DrawString(font, "PLAYER 1", new Vector2(0, 0), Color.White);
                //draws the health bar for the dummy
                spriteBatch.Draw(dummyHealthValuePic, dummyHealthValueRec, Color.White);
                spriteBatch.Draw(dummyHealthBarPic, dummyHealthBarRec, Color.White);
                wordWidth = font.MeasureString("TRAINING DUMMY").X; //labels the training dummys health bar
                spriteBatch.DrawString(font, "TRAINING DUMMY", new Vector2(GraphicsDevice.Viewport.Width - wordWidth - 2, 0), Color.White);
                wordWidth = font.MeasureString("TOTAL DAMAGE: " + dummy.getTotalDamage()).X; //draws the total damage dealth to the dummy centered above its head
                spriteBatch.DrawString(font, "TOTAL DAMAGE: " + dummy.getTotalDamage(), new Vector2(GraphicsDevice.Viewport.Width / 2 - wordWidth / 2, 200), Color.White);

                if (timer > 4000) //after the 321fight is done show they user they can go back to menu
                {
                    spriteBatch.DrawString(font, "PRESS \"BACK\" TO RETURN TO MENU", new Vector2(0, GraphicsDevice.Viewport.Height - 25), Color.White);
                }
            }
            else if (screenCounter == 3) //draws everything for the controls screen
            {
                //draws the menu background behind the controls
                spriteBatch.Draw(menuBackgroundPic[(int)picCounter], menuBackgroundRec, Color.White);
                spriteBatch.Draw(controlsPic, controlsRec, Color.White); //draws the controlls and show the user how to get back to the menu
                spriteBatch.DrawString(font, "PRESS \"BACK\" TO RETURN TO MENU", new Vector2(0, GraphicsDevice.Viewport.Height - 25), Color.White);
            }
            else if (screenCounter == 4) //draws everything for the about screen
            {
                spriteBatch.Draw(menuBackgroundPic[(int)picCounter], menuBackgroundRec, Color.White);
                spriteBatch.Draw(aboutPic, aboutRec, Color.White);
                spriteBatch.DrawString(font, "PRESS \"BACK\" TO RETURN TO MENU", new Vector2(0, GraphicsDevice.Viewport.Height - 25), Color.White);
            }
            //if its meant to display the game or practice screen
            if (screenCounter == 1 || screenCounter == 2)
            {
                if (playerWins[0] < 3 && playerWins[1] < 3) //and no player won yet
                {
                    if (timer < 1000) //display number 3 at start if round
                    {
                        countDownSourceRec.Y = 0;
                        spriteBatch.Draw(countDownPic, countDownRec, countDownSourceRec, Color.White);
                    }
                    else if (timer >= 1000 && timer < 2000) //then 2 after a second
                    {
                        countDownSourceRec.Y = 100;
                        spriteBatch.Draw(countDownPic, countDownRec, countDownSourceRec, Color.White);
                    }
                    else if (timer >= 2000 && timer < 3000) //then 3 afetr another secont
                    {
                        countDownSourceRec.Y = 200;
                        spriteBatch.Draw(countDownPic, countDownRec, countDownSourceRec, Color.White);
                    }
                    else if (timer >= 3000 && timer < 4000) //then draw the fight picture
                    {
                        spriteBatch.Draw(fightPic, fightRec, Color.White);
                    }
                }
            }
            spriteBatch.End();
            base.Draw(gameTime);
        }
    }
}
