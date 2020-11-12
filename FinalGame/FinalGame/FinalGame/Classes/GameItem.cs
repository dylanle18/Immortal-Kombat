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
    class GameItem
    {
        //fields
        protected Texture2D pic; //holds the picture for the game item
        protected Vector2 pos; //for where the game item will be placed
        protected double health; //the health of the item
        protected double damage; //how much damage it will take when hit
        protected Rectangle hitBoxRec; //the hitbox for the item
        protected Texture2D hitBoxPic; //a pic for testing to see where the hitbox is

        //constructor
        public GameItem(Texture2D _pic, Vector2 _pos, double _health, double _damage, ContentManager Content)
        {
            //calls the setters
            setHealth(_health);
            setDamage(_damage);
            setPic(_pic);
            setPos(_pos);
            LoadContent(Content);
        }

        public virtual void LoadContent(ContentManager Content)
        {
            hitBoxPic = Content.Load<Texture2D>("hitBox"); //loads in the hitbox pic
        }

        //getters and setters for all the variables above
        public Texture2D getPic() //for the sprite
        {
            return pic;
        }
        public void setPic(Texture2D aPic)
        {
            pic = aPic;
        }

        public Vector2 getPos() //position of game item
        {
            return pos;
        }
        public void setPos(Vector2 aPos)
        {
            pos = aPos;
        }

        public double getHealth() //health
        {
            return health;
        }
        public void setHealth(double aHealth)
        {
            health = aHealth;
        }

        public double getDamage() //damage that gets taken
        {
            return damage;
        }
        public void setDamage(double aDamage)
        {
            damage = aDamage;
        }

        public Rectangle getHitBox() //returns the hitbox for collisions
        {
            return hitBoxRec;
        }

        public void DoDamage() //for when the training dummy or player takes damage without blocking
        {
            health -= damage;
        }

        public void DoEnergyBallDamage() //for when the training dummy or player takes damage without blocking by energy ball
        {
            health -= damage * 2;
        }
    }
}
