﻿/*
 * Name: Bilal Ozdemir
 *
 * Final Project: BatmanGame
 *
 * Last Revision: 11 December, 2023
 *
 */
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Media;
using System;
using System.Collections.Generic;
using System.IO;


namespace BatmanGame
{
    public class BatmanGame : Game
    {
        // Define the different states of the game
        public enum GameState
        {
            MainMenu,
            Gameplay,
            GameOver,
            HowToPlay,
            AboutUs
        }

        //Declare various game variables
        private GraphicsDeviceManager _graphics;
        private SpriteBatch _spriteBatch;
        private Dictionary<string, Texture2D> _textures;
        private Rectangle replayButtonBounds;
        private Vector2 replayPosition;
        private Vector2 replaySize;
        private List<string> menuOptions = new List<string>() { "Enter Username: ", "Play", "How To Play", "About Us", "Quit" };
        private List<Vector2> menuOptionPositions = new List<Vector2>();
        private Vector2 mainMenuPosition;
        private Rectangle mainMenuButtonBounds;
        private Vector2 returnToMainMenuPosition;
        private Rectangle returnToMainMenuBounds;
        private SpriteFont menuFont;
        private KeyboardState _previousKeyboardState;
        //private int milestonePoints = 2000; //This variable doesn't do anything
        private float roadScrollSpeedIncrease = 0.05f;
        private float roadScrollSpeed = 1.0f;
        private float gameSpeedIncrease = 0.10f;

        //Initialize the current game state to the Main Menu
        private GameState _currentState = GameState.MainMenu;
        //Other variables
        private bool gameIsActive = true;
        private string frameName = "run0";
        private float frameStopwatch = 0;
        private float frameDuration = 47;
        private int currentName = 0;
        private float spikeStopWatch = 0;
        private float startingPos = 0;
        private float yPositionAdjustment = 0;
        private float jumpHeldTime = 0;
        private float maxJumpDuration = 250;
        private float fallTimer = 0;
        private float score = 0;
        private string _username = "";
        private bool _isEnteringUsername = false;
        private List<SpikeObstacle> spikeObstacles = new List<SpikeObstacle>();
        private SpriteFont font;

        //Constructor
        public BatmanGame()
        {
            //Initialize graphics device manager and content directory
            _graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
            IsMouseVisible = true; // Make the mouse cursor visible

        }

        protected override void Initialize()
        {
            //Initialize spriteBatch and load fonts
            _spriteBatch = new SpriteBatch(GraphicsDevice);
            font = Content.Load<SpriteFont>("File");
            menuFont = Content.Load<SpriteFont>("menuFont");






            base.Initialize();
        }

        //whether game is active or not
        public bool IsGameActive()
        {
            return gameIsActive;
        }

        //Load content such as textures and music
        protected override void LoadContent()
        {
            //Load background music
            Song backgroundMusic = Content.Load<Song>("backgroundMusic");
            MediaPlayer.Play(backgroundMusic);
            MediaPlayer.IsRepeating = true;



            int screenWidth = GraphicsDevice.Viewport.Width;
            int spacing = 20;
            float startY = 100;

            menuOptionPositions.Clear();
            foreach (var option in menuOptions)
            {
                Vector2 size = menuFont.MeasureString(option);
                Vector2 position = new Vector2((screenWidth - size.X) / 2, startY);
                menuOptionPositions.Add(position);
                startY += size.Y + spacing;
            }




            string returnText = "Return to Main Menu";
            Vector2 returnSize = font.MeasureString(returnText);
            returnToMainMenuPosition = new Vector2((Window.ClientBounds.Width - returnSize.X) / 2, Window.ClientBounds.Height - returnSize.Y - 50); returnToMainMenuBounds = new Rectangle((int)returnToMainMenuPosition.X, (int)returnToMainMenuPosition.Y, (int)returnSize.X, (int)returnSize.Y);


            _textures = new Dictionary<string, Texture2D>();
            _textures["run0"] = Content.Load<Texture2D>("Sprites/run0");

            // Loads the textures for the animation frames
            for (int i = 0; i <= 3; i++)
            {


                string textureName = "Sprites/run" + i;
                _textures[textureName] = Content.Load<Texture2D>(textureName);
            }

            // Loads the static textures (Roads and Spikes)
            _textures["road"] = Content.Load<Texture2D>("Sprites/road");
            _textures["spike"] = Content.Load<Texture2D>("Sprites/spike");

        }

        //Updates game logic and states.
        //The player is static, and what moves is the world. The spikes are moved towards the player as they spawn, and the background also has movement simulated
        protected override void Update(GameTime gameTime)
        {
            //Handle player input depending on game state
            KeyboardState currentKeyboardState = Keyboard.GetState();

            if (_currentState == GameState.MainMenu)
            {
                MouseState mouseState = Mouse.GetState();
                if (mouseState.LeftButton == ButtonState.Pressed)
                {
                    for (int i = 0; i < menuOptions.Count; i++)
                    {
                        Rectangle textBounds = new Rectangle((int)menuOptionPositions[i].X,
                                                             (int)menuOptionPositions[i].Y,
                                                             (int)font.MeasureString(menuOptions[i]).X,
                                                             font.LineSpacing);

                        if (textBounds.Contains(mouseState.Position))
                        {
                            HandleMenuSelection(i);
                            break;
                        }
                    }
                }
            }



            if (_currentState == GameState.HowToPlay)
            {
                MouseState mouseState = Mouse.GetState();
                if (mouseState.LeftButton == ButtonState.Pressed)
                {
                    if (returnToMainMenuBounds.Contains(mouseState.Position))
                    {
                        _currentState = GameState.MainMenu;
                    }
                }
            }
            if (_currentState == GameState.AboutUs)
            {
                MouseState mouseState = Mouse.GetState();
                if (mouseState.LeftButton == ButtonState.Pressed)
                {
                    if (returnToMainMenuBounds.Contains(mouseState.Position))
                    {
                        _currentState = GameState.MainMenu;
                    }
                }
            }

            if (_isEnteringUsername)
            {
                foreach (var key in currentKeyboardState.GetPressedKeys())
                {
                    if (_previousKeyboardState.IsKeyUp(key))
                    {
                        if (key == Keys.Enter)
                        {
                            _isEnteringUsername = false;
                        }
                        else if (key == Keys.Back && _username.Length > 0)
                        {
                            _username = _username[0..^1];
                        }
                        else
                        {
                            char c = GetCharFromKey(key);
                            if (!char.IsControl(c))
                            {
                                _username += c;
                            }
                        }
                    }
                }

                _previousKeyboardState = currentKeyboardState;
            }



            //Exit the game
            if (Keyboard.GetState().IsKeyDown(Keys.Escape))
                Exit();

            //This doesn't actually do anything, but i assume the author intended to increase the overall game speed according to milestonePoints, but his game speed is actually just tied to DeltaTime
            //I assume its supposed to go like this: Everytime the player reacches the milestone point, the speed increases by 0.05. It then adds 2000 to the next milestone points, preventing it from constantly increasing
            //if (score >= milestonePoints)
            //{
            //    gameSpeedIncrease += 0.05f;

            //    milestonePoints += 2000;
            //}

            //Update game objects based on current state
            if (gameIsActive)
            {
                float dt = (float)gameTime.TotalGameTime.TotalSeconds;

                //Score increases by a function of DeltaTime
                score = (int)(100 * dt);

                //This checks if the player has pressed the jump key, and how long he has held it.
                if ((Keyboard.GetState().IsKeyDown(Keys.W) || Keyboard.GetState().IsKeyDown(Keys.Space) || Keyboard.GetState().IsKeyDown(Keys.Up)) && jumpHeldTime < maxJumpDuration)
                {
                    //While the player is holding tje jump key, jumpHeldTime is incremented by delta time, until it reaches maxJumpDuration
                    jumpHeldTime += dt;
                    yPositionAdjustment = yPositionAdjustment - 20 + jumpHeldTime / 25;
                }
                else if (yPositionAdjustment < 0)
                {
                    fallTimer = fallTimer + dt;
                    yPositionAdjustment += (fallTimer / 25) - 5;

                    if (yPositionAdjustment > 0)
                    {
                        fallTimer = 0;
                        yPositionAdjustment = 0;
                        jumpHeldTime = 0;
                    }
                }

                //Score increases by a function of DeltaTime. Again. For some reason :/ ? Author forgot? Why not just double the value of the other one?
                score += (int)(100 * dt);

                //Same as before, this code doesn't actually do anything other than sum milestonePoints
                //if (score >= milestonePoints)
                //{
                //    roadScrollSpeed += roadScrollSpeed * roadScrollSpeedIncrease;

                //    milestonePoints += 2000;
                //}


                //This checks if the spikes are within an x position of [-15,15] (Where the player is), and if player yPositionAdjustment is above -15 (Player is Grounded). 
                //If it is, it considers that the player has collided with the spikes and ends the game
                foreach (var item in spikeObstacles)
                {
                    if (item.x < 15 && item.x > -15)
                    {
                        if (yPositionAdjustment > -15)
                        {
                            GameEnded();
                        }
                    }
                }

                //This will randomly spawn new spikes as time passes
                //The spikeStopWatch constantly increases until reaches the value of GenerateRandomSpikeSpawnRate(), and then is reset to 0, and spawns the spikeObstacle
                spikeStopWatch += dt;
                if (spikeStopWatch >= GenerateRandomSpikeSpawnRate())
                {
                    spikeStopWatch = 0;
                    spikeObstacles.Add(new SpikeObstacle());
                }
                //This will move the spikes leftwards, at a function of deltaTime * 7.5. Reducing the 7.5 will make them move faster, and increasing it will make them move slower
                foreach (var spikeObject in spikeObstacles)
                {
                    if (gameIsActive)
                        spikeObject.x -= (dt / 7.5f);
                }


            }

            if (!gameIsActive)
            {
                MouseState mouseState = Mouse.GetState();
                if (mouseState.LeftButton == ButtonState.Pressed)
                {
                    if (replayButtonBounds.Contains(mouseState.Position))
                    {
                        StartGame();
                    }
                }
            }
            if (_currentState == GameState.GameOver)
            {
                MouseState mouseState = Mouse.GetState();
                if (mouseState.LeftButton == ButtonState.Pressed)
                {
                    if (replayButtonBounds.Contains(mouseState.Position))
                    {
                        StartGame();
                    }
                    else if (mainMenuButtonBounds.Contains(mouseState.Position))
                    {
                        _currentState = GameState.MainMenu;
                    }
                }
            }

            base.Update(gameTime);
        }


        //Generates a random number that will then be compared on the part where the spikeObstacles are created
        private int GenerateRandomSpikeSpawnRate()
        {
            Random random = new Random();

            double closeSpawnProbability = 0.2;


            double spawnType = random.NextDouble();

            if (spawnType < closeSpawnProbability)
            {
                return random.Next(1000, 3000);
            }
            else // Far spawn probability
            {
                return random.Next(5000, 7000);
            }
        }

        //Get character from keyboard input
        private char GetCharFromKey(Keys key)
        {
            return key.ToString().Length == 1 ? key.ToString()[0] : '\0';
        }

        //Deals with user's menu actions
        private void HandleMenuSelection(int index)
        {
            switch (index)
            {
                case 0:
                    _isEnteringUsername = true;
                    break;
                case 1:
                    if (!string.IsNullOrWhiteSpace(_username))
                    {
                        StartGame();
                    }
                    break;
                case 2:
                    _currentState = GameState.HowToPlay;
                    break;
                case 3:
                    _currentState = GameState.AboutUs;
                    break;
                case 4:
                    Exit();
                    break;
            }
        }

        // Renders the visual elements of the game, main menu etc
        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(new Color(55, 55, 55));
            _spriteBatch.Begin();


            switch (_currentState)
            {
                case GameState.MainMenu:
                    for (int i = 0; i < menuOptions.Count; i++)
                    {
                        string displayText = menuOptions[i];
                        if (i == 0)
                        {
                            displayText += _username;
                        }
                        _spriteBatch.DrawString(menuFont, displayText, menuOptionPositions[i], Color.White);
                    }
                    break;

                case GameState.Gameplay:

                    //The road will constantly be moved leftwards, and when its x position reaches a certain point, its translated back to the initial position
                    Vector2 roadScale = new Vector2(1.0f, 4.0f);
                    float roadYPosition = Window.ClientBounds.Height - _textures["road"].Height * roadScale.Y;

                    startingPos -= (float)gameTime.ElapsedGameTime.TotalMilliseconds / (7.5f * (1.0f + gameSpeedIncrease));

                    if (startingPos < -100 * roadScale.X)
                    {
                        startingPos += 96 * roadScale.X;
                    }

                    //Draws the road
                    for (int i = 0; i < 60; i++)
                    {
                        _spriteBatch.Draw(_textures["road"], new Vector2(36 * i + startingPos, roadYPosition), null, Color.White, 0f, Vector2.Zero, roadScale, SpriteEffects.None, 0f);
                    }

                    //Draws the spikes
                    float spikeYPosition = roadYPosition - _textures["spike"].Height + 10;
                    foreach (var spikeObject in spikeObstacles)
                    {
                        _spriteBatch.Draw(_textures["spike"], new Vector2(spikeObject.x, spikeYPosition), Color.White);
                    }

                    //This is where the batman sprite is animated every frame according to a stopwatch
                    frameStopwatch += (float)gameTime.ElapsedGameTime.TotalMilliseconds;
                    if (frameStopwatch > frameDuration)
                    {
                        frameStopwatch -= frameDuration;
                        currentName = (currentName + 1) % 4;
                        frameName = "Sprites/run" + currentName;
                    }
                    Vector2 characterScale = new Vector2(1.5f, 1.5f);
                    float scaledCharacterHeight = _textures[frameName].Height * characterScale.Y;
                    float characterYPosition = roadYPosition - scaledCharacterHeight + yPositionAdjustment;
                    _spriteBatch.Draw(_textures[frameName], new Vector2(0, characterYPosition), null, Color.White, 0f, Vector2.Zero, characterScale, SpriteEffects.None, 0f);

                    //Draws the score
                    Vector2 textMiddlePoint = font.MeasureString("Score: " + score.ToString()) / 2;
                    Vector2 position = new Vector2(Window.ClientBounds.Width / 2, 30);
                    _spriteBatch.DrawString(font, "Score: " + score.ToString(), position, Color.White, 0, textMiddlePoint, 1.0f, SpriteEffects.None, 0.5f);
                    break;

                case GameState.GameOver:
                    //Draws the Game Over message
                    string gameOverMessage = "Game Over! " + _username + " your Score is : " + score.ToString("f0");
                    Vector2 gameOverSize = font.MeasureString(gameOverMessage);
                    Vector2 gameOverPosition = new Vector2((Window.ClientBounds.Width - gameOverSize.X) / 2, (Window.ClientBounds.Height / 2) - 100); _spriteBatch.DrawString(font, gameOverMessage, gameOverPosition, Color.White);

                    //Draws the Replay Button
                    _spriteBatch.DrawString(font, "Click to Replay", replayPosition, Color.Yellow);

                    //Draws the Main Menu Button
                    _spriteBatch.DrawString(font, "Back to Main Menu", mainMenuPosition, Color.Yellow);
                    break;
                case GameState.HowToPlay:
                    //Draws the How to Play 
                    string howToPlayText = "How to Play:\n\n Welcome!\n In this game, you play as Batman and you have to jump over\n the spikes in order to survive!\n Use the 'W', 'Space', or the 'Up' arrow key to jump.";
                    Vector2 howToPlaySize = font.MeasureString(howToPlayText);
                    Vector2 howToPlayPosition = new Vector2((Window.ClientBounds.Width - menuFont.MeasureString(howToPlayText).X) / 2 - 20, (Window.ClientBounds.Height / 2) - (menuFont.MeasureString(howToPlayText).Y / 2 - 20));
                    _spriteBatch.DrawString(menuFont, howToPlayText, howToPlayPosition, Color.White);

                    _spriteBatch.DrawString(font, "Return to Main Menu", returnToMainMenuPosition, Color.Yellow);

                    break;
                case GameState.AboutUs:
                    //Draws About Us content
                    string aboutUsText = "About Us:\n\n This game was developed by Bilal Ozdemir\n as a final project for his Gaming course";
                    Vector2 aboutUsPosition = new Vector2(50, 50);
                    _spriteBatch.DrawString(menuFont, aboutUsText, aboutUsPosition, Color.White);

                    _spriteBatch.DrawString(font, "Return to Main Menu", returnToMainMenuPosition, Color.Yellow);
                    break;
            }

            _spriteBatch.End();
            base.Draw(gameTime);
        }


        //Handles the game over state
        void GameEnded()
        {
            SaveScore(_username, score);

            _currentState = GameState.GameOver;
            gameIsActive = false;

            string replayText = "Click to Replay";
            replaySize = font.MeasureString(replayText);
            replayPosition = new Vector2((Window.ClientBounds.Width - replaySize.X) / 2,
                                         (Window.ClientBounds.Height / 2) + 50); //50 pixels below the center of the screen
            replayButtonBounds = new Rectangle((int)replayPosition.X, (int)replayPosition.Y, (int)replaySize.X, (int)replaySize.Y);

            string mainMenuText = "Back to Main Menu";
            Vector2 mainMenuSize = font.MeasureString(mainMenuText);
            mainMenuPosition = new Vector2((Window.ClientBounds.Width - mainMenuSize.X) / 2,
                                           replayPosition.Y + 50); //Below the replay button
            mainMenuButtonBounds = new Rectangle((int)mainMenuPosition.X, (int)mainMenuPosition.Y, (int)mainMenuSize.X, (int)mainMenuSize.Y);
        }

        //Saves the username and the score in the text file
        private void SaveScore(string username, float score)
        {
            string filePath = "scores.txt";
            string scoreEntry = $"{username}: {score}\n";

            File.AppendAllText(filePath, scoreEntry);
        }

        //Initializes game variables.
        public void StartGame()
        {
            _currentState = GameState.Gameplay;
            gameIsActive = true;
            score = 0;
            currentName = 0;
            yPositionAdjustment = 0;
            jumpHeldTime = 0;
            fallTimer = 0;
            startingPos = 0;
            frameStopwatch = 0;
            spikeStopWatch = 0;
            spikeObstacles.Clear();
        }


    }
}
