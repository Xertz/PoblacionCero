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

namespace PoblacionCero
{
    class Jugador
    {
        #region Variables:
            Texture2D jugadorTexture;
            float velocidadX, velocidadY;
            Vector2 jugadorPosicion;

            int vida, vidaMaxima;
            bool atacado;
            int cantidadMuertos;

            GameWindow window;
        #endregion

        public Jugador(ContentManager content, GameWindow win)
        {
            window = win;
            jugadorTexture = content.Load<Texture2D>("Sprites/Zero");

            velocidadX = 5f;
            velocidadY = 5f;

            vidaMaxima = 1000;
            vida = vidaMaxima;
            atacado = false;
            cantidadMuertos = 0;

            jugadorPosicion = new Vector2(window.ClientBounds.Width / 2, window.ClientBounds.Height - jugadorTexture.Height);
        }

        public void Mover(GameTime gameTime)
        {
            KeyboardState keyboard = Keyboard.GetState();

            if (keyboard.IsKeyDown(Keys.Up))
            {         
                jugadorPosicion.Y -= velocidadY;               
            }
            if (keyboard.IsKeyDown(Keys.Down))
            {                         
                jugadorPosicion.Y += velocidadY;               
            }
            if (keyboard.IsKeyDown(Keys.Left))
            {                           
                jugadorPosicion.X -= velocidadX;              
            }
            if (keyboard.IsKeyDown(Keys.Right))
            {                          
                jugadorPosicion.X += velocidadX;               
            }         
        }

        public void Frontera()
        {
            if (jugadorPosicion.X > (window.ClientBounds.Width - jugadorTexture.Width))
                jugadorPosicion.X -= velocidadX;
            else if(jugadorPosicion.X < 0)
                jugadorPosicion.X += velocidadX;


            if (jugadorPosicion.Y > (window.ClientBounds.Height - jugadorTexture.Height))
                jugadorPosicion.Y -= velocidadY;
            else if(jugadorPosicion.Y < 0)
                jugadorPosicion.Y += velocidadY;
        }

        #region Funciones Return:
            public Texture2D Sprite()
            {
                return this.jugadorTexture;
            }

            public Vector2 Posicion()
            {
                return this.jugadorPosicion;
            }

            public int Vida()
            {
                return vida;
            }

            public void AumentarVida()
            {
                if(vida < vidaMaxima)
                    vida++;
            }

            public void ReducirVida()
            {
                if (vida > 0)
                    vida--;
            }

            public void Mal()
            {
                atacado = true;
            }

            public void Bien()
            {
                atacado = false;
            }

            public bool Estado()
            {
                return atacado;
            }

            public int CantMuerto()
            {
                return cantidadMuertos;
            }

            public void Infectado()
            {
                cantidadMuertos++;
            }
        #endregion
    }
}
