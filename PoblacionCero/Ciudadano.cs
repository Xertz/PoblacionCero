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
    class Ciudadano
    {
        #region Variables:
            Texture2D ciudadanoTexture;
            Texture2D ciudadanoTextureSano;
            Texture2D ciudadanoTextureInfectado;
            Vector2 ciudadanoPosicion, ciudadanoCentro;
            float velocidadX, velocidadY;
            GameWindow window;

            int vida;
            bool infectado, muerto;

            Random rand = new Random();
        #endregion

        public Ciudadano(ContentManager content, GameWindow win, float X, float Y, int direccion)
        {
            window = win;
            ciudadanoTextureSano = content.Load<Texture2D>("Sprites/sano");
            ciudadanoTextureInfectado = content.Load<Texture2D>("Sprites/infectado");

            ciudadanoTexture = ciudadanoTextureSano;

            ciudadanoCentro = new Vector2(ciudadanoTexture.Width / 2, ciudadanoTexture.Height / 2);

            ciudadanoPosicion = new Vector2(X , Y);

            velocidadX = 2f;
            velocidadY = 2f;

            if (direccion == 0)
            {
                velocidadX *= -1;
            }
            if (direccion == 1)
                velocidadY *= -1;

            infectado = false;
            muerto = false;
            vida = 1000;
        }

        public void Frontera()
        {
            if (ciudadanoPosicion.X > (window.ClientBounds.Width - ciudadanoTexture.Width)
                || ciudadanoPosicion.X < 0)
                velocidadX *= -1;

            if (ciudadanoPosicion.Y > (window.ClientBounds.Height - ciudadanoTexture.Height)
                || ciudadanoPosicion.Y < 0)
                velocidadY *= -1;
        }

        public void CiuCoalision(Ciudadano ciu1, Ciudadano ciu2)
        {
            Rectangle recCiu1 = new Rectangle((int)ciu1.Posicion().X, (int)ciu1.Posicion().Y, (int)ciu1.Sprite().Width, (int)ciu1.Sprite().Height);
            Rectangle recCiu2 = new Rectangle((int)ciu2.Posicion().X, (int)ciu2.Posicion().Y, (int)ciu2.Sprite().Width, (int)ciu2.Sprite().Height);
            
            if (recCiu1.Intersects(recCiu2))
            {
                velocidadX *= -1;
                velocidadY *= -1;
            }
        }

        public void JugCoalision(Jugador jug, Ciudadano ciu2)
        {
            Rectangle recJug = new Rectangle((int)jug.Posicion().X, (int)jug.Posicion().Y, (int)jug.Sprite().Width, (int)jug.Sprite().Height);
            Rectangle recCiu = new Rectangle((int)ciu2.Posicion().X, (int)ciu2.Posicion().Y, (int)ciu2.Sprite().Width, (int)ciu2.Sprite().Height);

            if (recJug.Intersects(recCiu) && !infectado)
            {
                infectado = true;

                if (velocidadX < 0)
                    velocidadX = -1f;
                else if (velocidadX > 0)
                    velocidadX = 1f;

                if (velocidadY < 0)
                    velocidadY = -1f;
                if (velocidadY > 0)
                    velocidadY = 1f;
            }
        }

        public void MedCoalision(Medico med, Ciudadano ciu2)
        {
            Rectangle recMed = new Rectangle((int)med.Posicion().X, (int)med.Posicion().Y, (int)med.Sprite().Width, (int)med.Sprite().Height);
            Rectangle recCiu = new Rectangle((int)ciu2.Posicion().X, (int)ciu2.Posicion().Y, (int)ciu2.Sprite().Width, (int)ciu2.Sprite().Height);

            if (recMed.Intersects(recCiu) && infectado)
            {
                infectado = false;

                if (velocidadX < 0)
                    velocidadX = -2f;
                else if (velocidadX > 0)
                    velocidadX = 2f;

                if (velocidadY < 0)
                    velocidadY = -2f;
                if (velocidadY > 0)
                    velocidadY = 2f;
            }
        }

        public void Mover()
        {
            this.ciudadanoPosicion.X += velocidadX;
            this.ciudadanoPosicion.Y += velocidadY;

            if (infectado)
                ciudadanoTexture = ciudadanoTextureInfectado;
            else if (!infectado)
                ciudadanoTexture = ciudadanoTextureSano;
        }

        public void Vida()
        {
            if (infectado)
                vida--;

            if (vida <= 0)
                muerto = true;
        }

        #region Funciones Return:
            public Texture2D Sprite()
            {
                return this.ciudadanoTexture;
            }

            public Vector2 Posicion()
            {
                return this.ciudadanoPosicion;
            }

            public Vector2 Centro()
            {
                return this.ciudadanoCentro;
            }

            public bool Infectado()
            {
                return infectado;
            }

            public bool Muerto()
            {
                return muerto;
            }

            public int CantVida()
            {
                return vida;
            }
        #endregion
    }
}
