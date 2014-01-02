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
    class Medico
    {
        #region Variables:
            Texture2D medicoTexture;
            Vector2 medicoPosicion, medicoCentro;
            float velocidadX, velocidadY;
            GameWindow window;

            int vida;

            bool curando = false;
            Ciudadano ciudadano;
        #endregion

        public Medico(ContentManager content, GameWindow win, float X, float Y, int direccion)
        {
            window = win;
            medicoTexture = content.Load<Texture2D>("Sprites/medico");

            medicoCentro = new Vector2(medicoTexture.Width / 2, medicoTexture.Height / 2);

            medicoPosicion = new Vector2(X, Y);

            velocidadX = 0.4f;
            velocidadY = 0.4f;

            vida = 1000;
        }

        public void Mover(Ciudadano ciu, Jugador jug)
        {
            if (ciu.Infectado() && !curando)
            {
                ciudadano = ciu;
                curando = true;              
            }
            else if (curando)
            {
                if (ciudadano.Posicion().X > medicoPosicion.X)
                {
                    medicoPosicion.X += velocidadX;
                }
                if (ciudadano.Posicion().X < medicoPosicion.X)
                {
                    medicoPosicion.X -= velocidadX;
                }
                if (ciudadano.Posicion().Y > medicoPosicion.Y)
                {
                    medicoPosicion.Y += velocidadY;
                }
                if (ciudadano.Posicion().Y < medicoPosicion.Y)
                {
                    medicoPosicion.Y -= velocidadY;
                }

                if (!ciudadano.Infectado())
                {
                    curando = false;
                }
            }
            else
            {
                if (jug.Posicion().X > medicoPosicion.X)
                {
                    medicoPosicion.X += velocidadX;
                }
                if (jug.Posicion().X < medicoPosicion.X)
                {
                    medicoPosicion.X -= velocidadX;
                }
                if (jug.Posicion().Y > medicoPosicion.Y)
                {
                    medicoPosicion.Y += velocidadY;
                }
                if (jug.Posicion().Y < medicoPosicion.Y)
                {
                    medicoPosicion.Y -= velocidadY;
                }
            }      
        }

        public void JugCoalision(Jugador jug, Medico med)
        {
            Rectangle recJug = new Rectangle((int)jug.Posicion().X, (int)jug.Posicion().Y, (int)jug.Sprite().Width, (int)jug.Sprite().Height);
            Rectangle recMed = new Rectangle((int)med.Posicion().X, (int)med.Posicion().Y, (int)med.Sprite().Width, (int)med.Sprite().Height);

            if (recJug.Intersects(recMed))
            {
                jug.ReducirVida();
                jug.Mal();
            }
            else
                jug.Bien();
        }

        #region Funciones Return:
            public Texture2D Sprite()
            {
                return this.medicoTexture;
            }

            public Vector2 Posicion()
            {
                return this.medicoPosicion;
            }

            public Vector2 Centro()
            {
                return this.medicoCentro;
            }
        #endregion
    }
}
