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
    class TrainingDummy : GameItem
    {
        double totalDamage; //to keep track of the total damage the player is doing to the dummy

        public TrainingDummy(Texture2D _pic, Vector2 _pos, double _health, double _damage, double _totalDamage, ContentManager Content) : base(_pic, _pos, _health, _damage, Content)
        {
            setTotalDamage(_totalDamage); //calls setter for total damage
        }

        public double getTotalDamage() //returns the total damage
        {
            return totalDamage;
        }
        public void setTotalDamage(double aTotalDamage) //sets the total damage
        {
            totalDamage = aTotalDamage;
        }

        public void DummyUpdate(GameTime gameTime)
        {
            HitBox(); //updates the hitbox method for the dummy
        }

        public void addAttackDamage() //regular attacks
        {
            totalDamage += damage; //adds the damage dealt to the total damage
        }

        public void addEnergyDamage() //energy ball attacks
        {
            totalDamage += damage * 2; //adds double damage to the total damage
        }

        public void HitBox()
        {
            //spawns in the hitbox for the dummy at the correct pos and size
            hitBoxRec.X = (int)pos.X + 40;
            hitBoxRec.Y = (int)pos.Y;
            hitBoxRec.Width = pic.Width - 40;
            hitBoxRec.Height = (int)(pic.Height * 1.4);
        }

        public void DummyDraw(SpriteBatch spriteBatch)
        {
            //draws the dummy
            spriteBatch.Draw(pic, pos, new Rectangle(0, 0, pic.Width, pic.Height), Color.White, 0f, Vector2.Zero, 1.4f, SpriteEffects.None, 0f);
            //draws hitbox of dummy for testing
            //spriteBatch.Draw(hitBoxPic, hitBoxRec, Color.White);
        }
    }
}
