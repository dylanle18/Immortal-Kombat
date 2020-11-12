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
    class Character : GameItem
    {
        protected Rectangle sourceRec; //source rec for the player sprite to animate
        protected double energy; //holds how much energy the player has
        protected int speed; //speed of player in x plane
        protected bool isLeft; //keeps track whether the player is facing left or right

        protected float elapsed; //keeps track of how much time elapesed for the amination of the player
        protected int frames = 0; //to keep track of what fram to draw

        protected float idleDelay = 200f; //how much delay each frame has in the idle animation
        protected float walkingDelay = 150f; //delay for walking animation

        //bools to check what action the player is doing for animation and hitboxes
        protected bool isIdle = false;
        protected bool isWalking = false;
        protected bool isPunch = false;
        protected bool isKick = false;
        protected bool isCrouch = false;
        protected bool isBlock = false;
        protected bool isBlasting = false;

        //hitbox the spawns at the players fist or foot when attacking
        protected Rectangle attackHitBoxRec;

        //to make a delay on the attacks so it is not just intant
        protected float elapsedAttackTimer;

        protected List<bool> movingLeft = new List<bool>(); //list to check if the player is moving left for the blast
        protected List<Rectangle> energyBallRec = new List<Rectangle>(); //list for the blast recs
        protected Texture2D energyBallPic; //picture of the energy ball
        protected Texture2D energyBallPicFlipped; //pic of energy ball facing the other direction

        protected SoundEffect attackSound; //sounds for when the person punches or kicks
        protected SoundEffect blastSound; //sound for when a player shoots

        protected bool hasJumped = false; //to check if the player in the air so they cant jump again
        protected float yVelocity; //used for gravity when the player jumps

        //constructor
        public Character(Texture2D _pic, Vector2 _pos, double _health, double _damage, double _energy, int _speed, bool _isLeft, ContentManager Content) : base(_pic, _pos, _health, _damage, Content)
        {
            //calls the setters
            setEnergy(_energy);
            setSpeed(_speed);
            setIsleft(_isLeft);

            //creates the hitboxes for the players
            hitBoxRec = new Rectangle((int)pos.X + 25, (int)pos.Y + 25, 75, 200);
            attackHitBoxRec = new Rectangle(0, 0, 10, 10);
            LoadContent(Content);
        }

        public override void LoadContent(ContentManager Content)
        {
            //loads the pictures needed for the player
            energyBallPic = Content.Load<Texture2D>("players/energyBall");
            energyBallPicFlipped = Content.Load<Texture2D>("players/energyBallFlipped");
            attackSound = Content.Load<SoundEffect>("sounds/attack");
            blastSound = Content.Load<SoundEffect>("sounds/blast");
            hitBoxPic = Content.Load<Texture2D>("hitBox");
        }

        //getters and setters
        public double getEnergy() //energy
        {
            return energy;
        }
        public void setEnergy(double aEnergy)
        {
            energy = aEnergy;
        }

        public int getSpeed() //speed
        {
            return speed;
        }
        public void setSpeed(int aSpeed)
        {
            speed = aSpeed;
        }

        public bool getIsLeft() //direction facing
        {
            return isLeft;
        }
        public void setIsleft(bool aIsLeft)
        {
            isLeft = aIsLeft;
        }

        public Rectangle getAttackHitBox() //gets the attack hit box
        {
            return attackHitBoxRec;
        }

        public bool getBlock() //check if they are blocking
        {
            return isBlock;
        }

        public Rectangle getEnergyBall(int num) //returns the blast ball
        {
            return energyBallRec[num];
        }

        public int getEnergyBallAmount() //returns how many balls are are the screen for that player
        {
            return energyBallRec.Count();
        }

        //update method
        public void PlayerUpdate(GameTime gameTime, GamePadState pad, GamePadState oldpad, float timer)
        {
            if(hasJumped == false) //only allow them to attack when they are not jumping
            {
                elapsedAttackTimer += gameTime.ElapsedGameTime.Milliseconds;
            }

            hitBoxRec.X = (int)pos.X + 25; //updates the htibox rec to where the player is

            if (isPunch == false && isKick == false && isCrouch == false && isBlock == false && isBlasting == false) //if the player isnt attacking
            {
                if (timer > 3000) //after the countdown
                {
                    if (hitBoxRec.X > 0) //so the player cant move out the screen
                    {
                        if (pad.ThumbSticks.Left.X < 0) //moves them left and says they are facing left
                        {
                            pos.X -= speed;
                            isLeft = true;
                        }
                    }
                    if (hitBoxRec.X < 800 - hitBoxRec.Width) //moves them right and makes them face right
                    {
                        if (pad.ThumbSticks.Left.X > 0)
                        {
                            pos.X += speed;
                            isLeft = false;
                        }
                    }
                }

                if (pad.ThumbSticks.Left.X == 0) //if they arent moving
                {
                    isIdle = true; //sets idle to true so it knows to draw the idle animation
                }
                else
                {
                    isIdle = false;
                }

                if (pad.ThumbSticks.Left.X != 0) //if they are moving
                {
                    isWalking = true; //sets walking to true for animation
                }
                else
                {
                    isWalking = false;
                }
            }
            if (hasJumped == false) //they can only attack while not jumping
            {
                if (pad.Buttons.B == ButtonState.Pressed) //b = crouch
                {
                    //sets everything else to false so they cant do multiple moves at once
                    isPunch = false;
                    isKick = false;
                    isCrouch = true;
                    isBlock = false;
                    isBlasting = false;
                }
                else if (pad.Buttons.X == ButtonState.Pressed) //x = block
                {
                    isPunch = false;
                    isKick = false;
                    isCrouch = false;
                    isBlock = true;
                    isBlasting = false;
                }
                else if (pad.Buttons.Y == ButtonState.Pressed && energy >= 100) //y = blast
                {
                    isPunch = false;
                    isKick = false;
                    isCrouch = false;
                    isBlock = false;
                    isBlasting = true;
                }
                else if (pad.Buttons.LeftShoulder == ButtonState.Pressed && elapsedAttackTimer >= 400) //LB = punch, they can only punch every 400 mil sec
                {
                    attackSound.Play(); //plays the attacking sound
                    isPunch = true;
                    isKick = false;
                    isCrouch = false;
                    isBlock = false;
                    isBlasting = false;

                    elapsedAttackTimer = 0; //sets the attack timer back to 0 so the delay is reset
                }
                else if (pad.Buttons.RightShoulder == ButtonState.Pressed && elapsedAttackTimer >= 800) //RB = kick only kick every 800 mil sec
                {
                    attackSound.Play();
                    isPunch = false;
                    isKick = true;
                    isCrouch = false;
                    isBlock = false;
                    isBlasting = false;

                    elapsedAttackTimer = 0;
                }
                else //all moves are false because nothing is pressed
                {
                    //so the punch / kick isnt just drawn for 1 tick but for 100 or 200 mil sec
                    if (elapsedAttackTimer > 100)
                    {
                        isPunch = false;
                    }
                    if (elapsedAttackTimer > 200)
                    {
                        isKick = false;
                    }
                    isCrouch = false;
                    isBlock = false;
                    isBlasting = false;
                }
            }
            Blast(pad, oldpad); //calls the blasting method
            HitBox(gameTime); //calls the hitbox method
            Animate(gameTime); //calls the animate method
            if (timer >= 3000) //so they cant jump before the countdown
            {
                Jumping(pad, oldpad); //cals the jump method
            }
        }

        public void Jumping(GamePadState pad, GamePadState oldpad) //jump method
        {
            pos.Y += yVelocity; //adds the y vel to the y pos for gravity
            if (pad.Buttons.A == ButtonState.Pressed && oldpad.Buttons.A == ButtonState.Released && hasJumped == false)
            {
                //if the a button is pressed
                pos.Y -= 80f; //moves the player up 80
                yVelocity = -20f; //makes the player move up more
                hasJumped = true;
                elapsedAttackTimer = 0; //so they cant attack while jumping
            }
            if (hasJumped == true)
            {
                yVelocity += 2f; //slowly adds more vel so the player speeds up down as time goes on
            }
            if (pos.Y >= 200) //200 is the ground level
            {
                hasJumped = false; //so they dont continue to call
                pos.Y = 200; //makes sure they dont go below 200
            }
            if (hasJumped == false) //so they dont move in y direction if not jumping
            {
                yVelocity = 0f;
            }
        }

        public void Blast(GamePadState pad, GamePadState oldpad) //blast method
        {
            if (hasJumped == false) //if player isnt jumping
            {
                if (isLeft) //facing left
                {
                    //if they shoot and have enough energy
                    if (pad.Buttons.Y == ButtonState.Pressed && oldpad.Buttons.Y == ButtonState.Released && energy >= 100)
                    {
                        blastSound.Play(); //plays the blasting sound
                        movingLeft.Add(true); //adds moving left as true to list so blast knows what direction to move
                        energyBallRec.Add(new Rectangle((int)pos.X - 75, (int)pos.Y + 75, 50, 50)); //spawns a energy ball at the players hands
                        energy -= 100; //takes energy away from the player
                    }
                }
                else //facing right
                {
                    //the same as above but for if the player is facing right
                    if (pad.Buttons.Y == ButtonState.Pressed && oldpad.Buttons.Y == ButtonState.Released && energy >= 100)
                    {
                        blastSound.Play();
                        movingLeft.Add(false);
                        energyBallRec.Add(new Rectangle((int)pos.X + 150, (int)pos.Y + 75, 50, 50));
                        energy -= 100;
                    }
                }
            }
            //for the number of balls for that player
            for (int i = 0; i < energyBallRec.Count; i++)
            {
                Rectangle temp; //creates a temp re that guides the ball where to go
                if (movingLeft[i])
                {
                    temp = energyBallRec[i]; //makes the temp rec the same pos and size as that ball
                    temp.X -= 15; //moves the temp rec left
                    energyBallRec[i] = temp; //but ball at temp rec therefore moving it left too
                }
                else //same thing but for facing right
                {
                    temp = energyBallRec[i];
                    temp.X += 15;
                    energyBallRec[i] = temp;
                }
                if (temp.X < -50 || temp.X > 800) //if the ball travels outside the screen
                {
                    //removes the ball and the direction it was moving from both lists
                    movingLeft.Remove(movingLeft[i]);
                    energyBallRec.Remove(energyBallRec[i]);
                }
            }
        }

        public void HitBox(GameTime gameTime) //hitbox method
        {
            //if they are pressing the punch button
            if (isPunch && isCrouch == false && isBlock == false)
            {
                //spawn the hitbox based on which direction the player is facing
                if (isLeft == false)
                {
                    attackHitBoxRec.X = (int)pos.X + 150;
                    attackHitBoxRec.Y = (int)pos.Y + 55;
                }
                else if (isLeft)
                {
                    attackHitBoxRec.X = (int)pos.X - 35;
                    attackHitBoxRec.Y = (int)pos.Y + 55;
                }
            }
            //same thing as the punching but for kicking
            if (isKick && isCrouch == false && isBlock == false)
            {
                if (isLeft == false)
                {
                    attackHitBoxRec.X = (int)pos.X + 165;
                    attackHitBoxRec.Y = (int)pos.Y + 30;
                }
                else if (isLeft)
                {
                    attackHitBoxRec.X = (int)pos.X - 50;
                    attackHitBoxRec.Y = (int)pos.Y + 30;
                }
            }
            //if they arent attacking then move the attack hit box out of way
            if (isPunch == false && isKick == false)
            {
                attackHitBoxRec.X = 0;
                attackHitBoxRec.Y = 0;
            }
            if (hasJumped) //if they are jumping
            {
                hitBoxRec.Y = (int)pos.Y + 25; //moves the hitbox so it follows the player
                hitBoxRec.Height = 150; //shortens the hitbox
            }
            else if (isCrouch) //if they are crouching
            {
                hitBoxRec.Y = (int)pos.Y + 85; //move the hitbox down
                hitBoxRec.Height = 140; //shortens the hitbox to fit within the player
            }
            else //if they arent jumping or crouching
            {
                //makes the hitbox normal size and pos again
                hitBoxRec.Y = (int)pos.Y + 25;
                hitBoxRec.Height = 200;
            }
        }

        public void Animate(GameTime gameTime) //animate method
        {
            if (isIdle)
            {
                //keeps track of time past
                elapsed += (float)gameTime.ElapsedGameTime.TotalMilliseconds;

                if (elapsed >= idleDelay) //if the time past is greater then the delay for the animation
                {
                    //changes to the next frame and if on the last frame it will loop back to the first frame
                    if (frames >= 3)
                    {
                        frames = 0;
                    }
                    else
                    {
                        frames++;
                    }
                    elapsed = 0; //resets the timer
                }
                //moves the source rec based on what frame it is on
                sourceRec = new Rectangle(50 * frames, 0, 50, 90);
            }
            //the same as the idle animation but for walking
            if (isWalking)
            {
                elapsed += (float)gameTime.ElapsedGameTime.TotalMilliseconds;

                if (elapsed >= walkingDelay)
                {
                    if (frames >= 4)
                    {
                        frames = 0;
                    }
                    else
                    {
                        frames++;
                    }
                    elapsed = 0;
                }
                sourceRec = new Rectangle(50 * frames, 91, 50, 89);
            }

            //for the attack or movement the player is doing it will move the source rec
            //to the correct spot on the sprite sheet
            if (isPunch)
            {
                sourceRec = new Rectangle(0, 271, 100, 89);
            }
            if (isKick)
            {
                sourceRec = new Rectangle(0, 361, 100, 90);
            }
            if (isBlasting)
            {
                sourceRec = new Rectangle(0, 451, 100, 90);
            }
            if (isCrouch)
            {
                sourceRec = new Rectangle(0, 631, 50, 90);
            }
            if (isBlock)
            {
                sourceRec = new Rectangle(0, 541, 50, 90);
            }
            if (hasJumped)
            {
                sourceRec = new Rectangle(100, 181, 50, 90);
            }
        }

        //damage done if the player is hit while blocking
        //is half of original
        public void DoBlockDamage()
        {
            health -= damage / 2;
        }
        //damage done to player by energy ball while blocking
        //half or regular ball, which is double for normal attack
        public void DoBlockEnergyBallDamage()
        {
            health -= damage;
        }
        //if the player who does the damage will recieve more eneregy
        public void attackEnergy()
        {
            energy += damage * 2;
        }
        //then the player who gets hit
        public void damageEnergy()
        {
            energy += damage / 2;
        }

        public void PlayerDraw(SpriteBatch spriteBatch) //draw method
        {
            if (isLeft) //if they are facing left
            {
                //and jumped
                if (hasJumped || isPunch == false && isKick == false && isBlasting == false)
                {
                    //draw the sprite at this pos
                    spriteBatch.Draw(pic, pos, sourceRec, Color.White, 0f, Vector2.Zero, 2.5f, SpriteEffects.FlipHorizontally, 0f);
                }
                else
                {
                    //else draw at this pos because attacking pic has bigger width then idle or walking, which shifts the player over
                    spriteBatch.Draw(pic, new Vector2(pos.X - 125, pos.Y), sourceRec, Color.White, 0f, Vector2.Zero, 2.5f, SpriteEffects.FlipHorizontally, 0f);
                }
            }
            else //else draw the sprite normally
            {
                spriteBatch.Draw(pic, pos, sourceRec, Color.White, 0f, Vector2.Zero, 2.5f, SpriteEffects.None, 0f);
            }
            for (int i = 0; i < energyBallRec.Count; i++) //draws for the amount of energy balls
            {
                if (movingLeft[i]) //if the ball is moving left
                {
                    spriteBatch.Draw(energyBallPicFlipped, energyBallRec[i], Color.White); //draw the left pic on the ball
                }
                else //else try the right facing pic on the ball
                {
                    spriteBatch.Draw(energyBallPic, energyBallRec[i], Color.White);
                }
            }
            //draws the hit boxes for testing
            //spriteBatch.Draw(hitBoxPic, hitBoxRec, Color.White);
            //spriteBatch.Draw(hitBoxPic, attackHitBoxRec, Color.White);
        }
    }
}
