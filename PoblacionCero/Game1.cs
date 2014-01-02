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
    public class Game1 : Microsoft.Xna.Framework.Game
    {
        #region Variables:
            GraphicsDeviceManager graphics;
            SpriteBatch spriteBatch;

            SpriteFont font;
            KeyboardState kb;
            Random rand = new Random();

            enum GameState { Inicio, Juego, Final, GameOver, Win };
            GameState currentGameState = GameState.Inicio;

            int cantCiudadanos = 8, cantMedicos = 3;
        #endregion

        #region Objetos:
            private Jugador jugador;
            private Ciudadano[] ciudadanos;
            private Medico[] medicos;
        #endregion

        public Game1()
        {
            graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
        }

        protected override void Initialize()
        {
            // TODO: Add your initialization logic here
            this.graphics.PreferredBackBufferWidth = 800;
            this.graphics.PreferredBackBufferHeight = 600;
            this.graphics.ApplyChanges();

            kb = Keyboard.GetState();
            jugador = new Jugador(this.Content, this.Window);
            ciudadanos = new Ciudadano[cantCiudadanos];
            medicos = new Medico[cantMedicos];

            for (int i = 0; i < cantCiudadanos; i++)
            {
                ciudadanos[i] = new Ciudadano(this.Content, this.Window,
                    rand.Next(Window.ClientBounds.Width - 32), rand.Next(Window.ClientBounds.Height / 2), rand.Next(3));
            }

            for (int i = 0; i < cantMedicos; i++)
            {
                medicos[i] = new Medico(this.Content, this.Window,
                    rand.Next(Window.ClientBounds.Width - 32), rand.Next(Window.ClientBounds.Height / 2), rand.Next(3));
            }

            base.Initialize();
        }

        protected override void LoadContent()
        {
            // Create a new SpriteBatch, which can be used to draw textures.
            spriteBatch = new SpriteBatch(GraphicsDevice);

            // TODO: use this.Content to load your game content here
            font = Content.Load<SpriteFont>(@"Fonts\Arial");
            //spriteSheet = Content.Load<Texture2D>(@"Sprites\Zero");

            //jugador = new Jugador(this.spriteSheet, this.Window);
        }

        protected override void UnloadContent()
        {
            // TODO: Unload any non ContentManager content here
        }

        protected override void Update(GameTime gameTime)
        {
            // TODO: Add your update logic here
            #region GameState:
            switch (currentGameState)
            {

                #region Incio:
                case GameState.Inicio:
                    if (Keyboard.GetState().IsKeyDown(Keys.Space) && !kb.IsKeyDown(Keys.Space))
                        currentGameState = GameState.Juego;

                    if (Keyboard.GetState().IsKeyDown(Keys.Q) && !kb.IsKeyDown(Keys.Q))
                        this.Exit();

                    kb = Keyboard.GetState();
                    break;
                #endregion

                #region Juego:
                case GameState.Juego:
                    if (Keyboard.GetState().IsKeyDown(Keys.Escape) && !kb.IsKeyDown(Keys.Escape))
                        currentGameState = GameState.Final;

                    kb = Keyboard.GetState();

                    //Movimiento Jugador
                    jugador.Mover(gameTime);
                    jugador.Frontera();
                    if (!jugador.Estado())
                        jugador.AumentarVida();

                    if (jugador.Vida() == 0)
                        currentGameState = GameState.GameOver;

                    if (jugador.CantMuerto() == cantCiudadanos)
                        currentGameState = GameState.Win;

                    //Movimiento Ciudadanos
                    for (int i = 0; i < cantCiudadanos; i++)
                    {
                        if (!ciudadanos[i].Muerto())
                        {
                            ciudadanos[i].Mover();
                            ciudadanos[i].Frontera();

                            for (int g = 0; g < cantCiudadanos; g++)
                            {
                                if (g != i)
                                {
                                    ciudadanos[i].CiuCoalision(ciudadanos[i], ciudadanos[g]);
                                }
                            }

                            ciudadanos[i].JugCoalision(jugador, ciudadanos[i]);

                            ciudadanos[i].Vida();

                            if (ciudadanos[i].Muerto())
                                this.jugador.Infectado();
                        }
                    }

                    //Movimiento Medicos
                    for (int i = 0; i < cantMedicos; i++)
                    {
                        for (int g = 0; g < cantCiudadanos; g++)
                        {
                            medicos[i].Mover(ciudadanos[g], jugador);
                            ciudadanos[g].MedCoalision(medicos[i], ciudadanos[g]);
                            medicos[i].JugCoalision(jugador, medicos[i]);
                        }
                    }
                    break;
                #endregion

                #region Final:
                case GameState.Final:
                    if (Keyboard.GetState().IsKeyDown(Keys.M) && !kb.IsKeyDown(Keys.M))
                        currentGameState = GameState.Inicio;
                    else if (Keyboard.GetState().IsKeyDown(Keys.Escape) && !kb.IsKeyDown(Keys.Escape))
                        currentGameState = GameState.Juego;

                    kb = Keyboard.GetState();
                    break;
                #endregion

                #region GameOver:
                case GameState.GameOver:
                    if (Keyboard.GetState().GetPressedKeys().Length > 0)
                        currentGameState = GameState.Inicio;
                    break;
                #endregion

                #region Win:
                case GameState.Win:
                    if (Keyboard.GetState().IsKeyDown(Keys.Enter) && !kb.IsKeyDown(Keys.Enter))
                        currentGameState = GameState.Inicio;
                    break;
                #endregion
            }
            #endregion

            base.Update(gameTime);
        }

        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.CornflowerBlue);

            // TODO: Add your drawing code here
            spriteBatch.Begin();
                #region GameState:
                    switch (currentGameState)
                    {
                        #region Inicio:
                        case GameState.Inicio:
                            spriteBatch.DrawString(font, "Presione Espacio para comenzar", new Vector2((Window.ClientBounds.Width / 2) - 130, 
                                Window.ClientBounds.Height / 2), Color.White, 0, Vector2.Zero, 1, SpriteEffects.None, 1);

                            spriteBatch.DrawString(font, "Presione Q para salir", new Vector2((Window.ClientBounds.Width / 2) - 130,
                                (Window.ClientBounds.Height / 2) + 50), Color.White, 0, Vector2.Zero, 1, SpriteEffects.None, 1);
                            break;
                        #endregion

                        #region Juego:
                        case GameState.Juego:
                            //Texto
                            spriteBatch.DrawString(font, "Presione Esc para pausar", new Vector2(Window.ClientBounds.Width - 300,
                                Window.ClientBounds.Height - 50), Color.White, 0, Vector2.Zero, 1, SpriteEffects.None, 1);

                            spriteBatch.DrawString(font, "Vida: " + jugador.Vida(), new Vector2(0, 0), Color.White, 0,
                                Vector2.Zero, 1, SpriteEffects.None, 1);

                            spriteBatch.DrawString(font, "Muertos: " + jugador.CantMuerto(), new Vector2(0, Window.ClientBounds.Height - 50), Color.White, 0,
                                Vector2.Zero, 1, SpriteEffects.None, 1);

                            //Jugador
                            spriteBatch.Draw(jugador.Sprite(), jugador.Posicion(), null,
                                Color.White, 0, Vector2.Zero, 1, SpriteEffects.None, 0);

                            //Ciudadanos
                            for (int i = 0; i < cantCiudadanos; i++)
                            {
                                if (!ciudadanos[i].Muerto())
                                {
                                    if (!ciudadanos[i].Infectado())
                                    {
                                        spriteBatch.Draw(ciudadanos[i].Sprite(), ciudadanos[i].Posicion(), null, Color.White, 0,
                                            Vector2.Zero, 1, SpriteEffects.None, 0);

                                        spriteBatch.DrawString(font, "Vida: " + ciudadanos[i].CantVida(),
                                            new Vector2(ciudadanos[i].Posicion().X, (ciudadanos[i].Posicion().Y - 10)), Color.White, 0,
                                            Vector2.Zero, 1, SpriteEffects.None, 1);
                                    }

                                    if (ciudadanos[i].Infectado())
                                    {
                                        spriteBatch.Draw(ciudadanos[i].Sprite(), ciudadanos[i].Posicion(), null, Color.White, 0,
                                            Vector2.Zero, 1, SpriteEffects.None, 0);

                                        spriteBatch.DrawString(font, "Vida: " + ciudadanos[i].CantVida(),
                                            new Vector2(ciudadanos[i].Posicion().X, (ciudadanos[i].Posicion().Y - 10)), Color.White, 0,
                                            Vector2.Zero, 1, SpriteEffects.None, 1);
                                    }
                                }
                            }

                            //Medicos
                            for (int i = 0; i < cantMedicos; i++)
                                spriteBatch.Draw(medicos[i].Sprite(), medicos[i].Posicion(), null, Color.White, 0,
                                    Vector2.Zero, 1, SpriteEffects.None, 0);
                            break;
                        #endregion

                        #region Final:
                        case GameState.Final:
                            spriteBatch.DrawString(font, "Presione Esc para regresar al juego", new Vector2((Window.ClientBounds.Width / 2) - 130,
                                Window.ClientBounds.Height / 2), Color.White, 0, Vector2.Zero, 1, SpriteEffects.None, 1);

                            spriteBatch.DrawString(font, "Presione M para regresar al menu", new Vector2((Window.ClientBounds.Width / 2) - 130,
                                Window.ClientBounds.Height / 2 + 50), Color.White, 0, Vector2.Zero, 1, SpriteEffects.None, 1);
                            break;
                        #endregion

                        #region GameOver:
                        case GameState.GameOver:
                            spriteBatch.DrawString(font, "Presione cualquier tecla", new Vector2((Window.ClientBounds.Width / 2) - 130, 50),
                                Color.White, 0, Vector2.Zero, 1, SpriteEffects.None, 1);

                            spriteBatch.DrawString(font, "GAME OVER", new Vector2((Window.ClientBounds.Width / 2) - 130,
                                (Window.ClientBounds.Height / 2)), Color.White, 0, Vector2.Zero, 3, SpriteEffects.None, 1);
                            break;
                        #endregion

                        #region Win:
                        case GameState.Win:
                            spriteBatch.DrawString(font, "Presione Enter para continuar", new Vector2((Window.ClientBounds.Width / 2) - 130, 50),
                                Color.White, 0, Vector2.Zero, 1, SpriteEffects.None, 1);

                            spriteBatch.DrawString(font, "VICTORY!", new Vector2((Window.ClientBounds.Width / 2) - 130,
                                (Window.ClientBounds.Height / 2)), Color.White, 0, Vector2.Zero, 3, SpriteEffects.None, 1);
                            break;
                        #endregion
                    }
                #endregion
            spriteBatch.End();

            base.Draw(gameTime);
        }

        #region Funciones:
        
        #endregion
    }
}
