using System;
using System.Collections.Generic;
using System.Collections;
using System.Linq;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.GamerServices;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Media;
using Microsoft.Xna.Framework.Net;
using Microsoft.Xna.Framework.Storage;

using ProjectMercury;
using ProjectMercury.Emitters;
using ProjectMercury.Modifiers;
using ProjectMercury.Renderers;

namespace Bugzzz
{
    /// <summary>
    /// This is the main type for your game
    /// </summary>z
    /// 

    public class Game1 : Microsoft.Xna.Framework.Game
    {
        //arrays containing bullet entities and enemies
        GameObject[] bullets;
        GameObject[] bullets2;
        GameObject[] turretBullets1;
        GameObject[] turretBullets2;
        GameObject[] enemies;
        ArrayList pickups;
        ArrayList score;
        Turret turret1; //Player 1 turret
        Turret turret2; //Player 2 turret
        const int maxEnemies = 20;
        const int maxBullets = 30;
        int[] enemies_level;
        int level;
        int enemies_killed;
        bool press1a, press1b; //indicates if button already pressed
        bool press2a, press2b;
        
        

        // Score display time on screen as it fades out
        const int SCORE_TIME = 160;
        const int fade_length = 150;
        const float fade_increment = 0.5f;
        float current_fade;
       // int fadeNum;
        bool fade_in, fade_out, scoreScreen, act_fade;
        bool gameLoading;
        float progress, prog2;
        
        Player player1;
        Player player2;
        SpriteFont scorefont;
        SpriteFont levelfont;
        SpriteFont titlefont;

        GraphicsDeviceManager graphics;
        SpriteBatch spriteBatch;
        Viewport viewport;
        Rectangle viewportRect;
        LevelScore ls;
        GameMenu gm;
        PauseMenu pm;

        Random rand;
        //for fire delay
        float elapsedTime = 0;
        float elapsedTime2 = 0;
        float t_elapsedTime = 0;
        float t2_elapsedTime = 0;
        float fireDelay = 0.15f;
        Texture2D healthBar;

        Weapons p1_w;
        Weapons p2_w;

        //rotation increment
        float angle_rot = .18f;
        float turret_rot = .23f;

        KeyboardState previousKeyboardState = Keyboard.GetState();
        MouseState previousMouseState = Mouse.GetState();

        //particle effects zomg!
        ParticleEffect bloodExplosion, pickupGlow, bulletHit, stinkyBug;
        PointSpriteRenderer particleRenderer;

        GameTime gt;

        Texture2D[] level_backgrounds;
        Texture2D getReady;
        Texture2D logo;
        Texture2D antman_top;
        Texture2D spiderman_top;
        Texture2D antman_bottom;
        Texture2D spiderman_bottom;
        Texture2D[] sMenu;
        Texture2D healthBack;
        Texture2D healthFrontP1;
        Texture2D healthFrontP2;
        Texture2D poison;

        bool gameOver;

        // Sounds
        SoundEffect introSound;
        SoundEffect shootSound;
        SoundEffect shootSoundSlow;
        SoundEffect shotgunSound;
        SoundEffect shotgunSoundSlow;
        SoundEffect ambianceSound;

        bool introSoundPlayed;

        // Time Effects
        SlowTimeEffect timeEffect;

        // Refraction effect
        Effect refractionEffect;
        Texture2D waterfallTexture;

        public Game1()
        {
            graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
        }

        /// <summary>
        /// Attempt to set the display mode to the desired resolution.  Itterates through the display
        /// capabilities of the default graphics adapter to determine if the graphics adapter supports the
        /// requested resolution.  If so, the resolution is set and the function returns true.  If not,
        /// no change is made and the function returns false.
        /// </summary>
        /// <param name="iWidth">Desired screen width.</param>
        /// <param name="iHeight">Desired screen height.</param>
        /// <param name="bFullScreen">True if you wish to go to Full Screen, false for Windowed Mode.</param>
        private bool InitGraphicsMode(int iWidth, int iHeight, bool bFullScreen)
        {
            // If we aren't using a full screen mode, the height and width of the window can
            // be set to anything equal to or smaller than the actual screen size.
            if (bFullScreen == false)
            {
                if ((iWidth <= GraphicsAdapter.DefaultAdapter.CurrentDisplayMode.Width)
                    && (iHeight <= GraphicsAdapter.DefaultAdapter.CurrentDisplayMode.Height))
                {
                    graphics.PreferredBackBufferWidth = iWidth;
                    graphics.PreferredBackBufferHeight = iHeight;
                    graphics.IsFullScreen = bFullScreen;
                    graphics.ApplyChanges();
                    return true;
                }
            }
            else
            {
                // If we are using full screen mode, we should check to make sure that the display
                // adapter can handle the video mode we are trying to set.  To do this, we will
                // iterate thorugh the display modes supported by the adapter and check them against
                // the mode we want to set.
                foreach (DisplayMode dm in GraphicsAdapter.DefaultAdapter.SupportedDisplayModes)
                {
                    // Check the width and height of each mode against the passed values
                    if ((dm.Width == iWidth) && (dm.Height == iHeight))
                    {
                        // The mode is supported, so set the buffer formats, apply changes and return
                        graphics.PreferredBackBufferWidth = iWidth;
                        graphics.PreferredBackBufferHeight = iHeight;
                        graphics.IsFullScreen = bFullScreen;
                        graphics.ApplyChanges();
                        return true;
                    }
                }
            }
            return false;
        }

        /// <summary>
        /// Allows the game to perform any initialization it needs to before starting to run.
        /// This is where it can query for any required services and load any non-graphic
        /// related content.  Calling base.Infitialize will enumerate through any components
        /// and initialize them as well.
        /// </summary>
        protected override void Initialize()
        {
            // TODO: Add your initialization logic here
            //player's stating position
            InitGraphicsMode(1280, 720, false);
            this.gameLoading = true;
            this.progress = 0;
            this.prog2 = 0;
            this.current_fade = 0;
            this.fade_in = true;
            this.rand = new Random();
            this.enemies_killed = 0;
            this.enemies_level = new int[4];
            this.enemies_level[0] = 100;
            this.enemies_level[1] = 250;
            this.enemies_level[2] = 500;
            this.enemies_level[3] = 1000;
            this.press1a = false;
            this.press2a = false;
            this.press1b = false;
            this.press2b = false;


            
            this.viewport = GraphicsDevice.Viewport;
            this.viewportRect = new Rectangle(0, 0, GraphicsDevice.Viewport.Width, GraphicsDevice.Viewport.Height);

            this.level = 0;
            this.bullets = new GameObject[maxBullets];
            this.bullets2 = new GameObject[maxBullets];

            this.turretBullets1 = new GameObject[maxBullets];
            this.turretBullets2 = new GameObject[maxBullets];

            this.enemies = new GameObject[maxEnemies];

            this.gameOver = false;
            this.introSoundPlayed = false;

            this.timeEffect = new SlowTimeEffect();
            timeEffect.isActive = false;

            base.Initialize();

        }

        /// <summary>
        /// LoadContent will be called once per game and is the place to load
        /// all of your content.
        /// </summary>
        protected override void LoadContent()
        {
            // Create a new SpriteBatch, which can be used to draw textures.

            Texture2D[] menus = new Texture2D[9];
            menus[0] = Content.Load<Texture2D>("MainMenu\\play_sel");
            menus[1] = Content.Load<Texture2D>("MainMenu\\opt_sel");
            menus[2] = Content.Load<Texture2D>("MainMenu\\cred_sel");
            menus[3] = Content.Load<Texture2D>("MainMenu\\exit_sel");
            menus[4] = Content.Load<Texture2D>("MainMenu\\play_un");
            menus[5] = Content.Load<Texture2D>("MainMenu\\opt_un");
            menus[6] = Content.Load<Texture2D>("MainMenu\\cred_un");
            menus[7] = Content.Load<Texture2D>("MainMenu\\exit_un");
            menus[8] = Content.Load<Texture2D>("MainMenu\\menu");

            this.gm = new GameMenu(menus, viewport);

            menus = new Texture2D[7];
            menus[0] = Content.Load<Texture2D>("PauseMenu\\cont_act");
            menus[1] = Content.Load<Texture2D>("PauseMenu\\menu_act");
            menus[2] = Content.Load<Texture2D>("PauseMenu\\exit_act");
            menus[3] = Content.Load<Texture2D>("PauseMenu\\cont_un");
            menus[4] = Content.Load<Texture2D>("PauseMenu\\menu_un");
            menus[5] = Content.Load<Texture2D>("PauseMenu\\exit_un");
            menus[6] = Content.Load<Texture2D>("PauseMenu\\background");

            this.pm = new PauseMenu(menus, viewport);

            this.logo = Content.Load<Texture2D>("Backgrounds\\CompanyLogo");

            // Sounds
            introSound = Content.Load<SoundEffect>("Sounds\\rain_intro");
            shootSound = Content.Load<SoundEffect>("Sounds\\shoot");
            shootSoundSlow = Content.Load<SoundEffect>("Sounds\\shoot_slow");
            shotgunSound = Content.Load<SoundEffect>("Sounds\\shotgun");
            shotgunSoundSlow = Content.Load<SoundEffect>("Sounds\\shotgun_slow");
            ambianceSound = Content.Load<SoundEffect>("Sounds\\ambiance");

            // Refraction fx
            refractionEffect = Content.Load<Effect>("Content\\refraction");
            waterfallTexture = Content.Load<Texture2D>("Sprites\\waterfall");

            Texture2D temp = Content.Load<Texture2D>("Sprites\\cannon");
            turret1 = new Turret(temp);
            turret2 = new Turret(temp);
            
            healthBar = Content.Load<Texture2D>("Sprites\\healthBar");
            temp = null;
            temp = Content.Load<Texture2D>("Sprites\\cannonball");

            p1_w = new Weapons(temp, temp, temp);
            p2_w = new Weapons(temp, temp, temp);


            pickups = new ArrayList();
            //Initializes all of the bullets, enemies, etc.

            level_backgrounds = new Texture2D[5];
            level_backgrounds[0] = Content.Load<Texture2D>("Backgrounds\\grass_bg");
            poison = Content.Load<Texture2D>("Backgrounds\\poisoned");

            for (int i = 0; i < maxBullets; i++)
            {
                bullets2[i] = new GameObject(temp,4);
                bullets[i] = new GameObject(temp,4);
            }

            for (int i = 0; i < maxBullets; i++)
            {
                turretBullets1[i] = new GameObject(temp,4);
                turretBullets2[i] = new GameObject(temp,4);
            }

            temp = Content.Load<Texture2D>("Sprites\\roach");
            Texture2D littlebitch = Content.Load<Texture2D>("Sprites\\littlebitch");
            Texture2D bigboss = Content.Load<Texture2D>("Sprites\\bigboss");
            Texture2D stinky = Content.Load<Texture2D>("Sprites\\stinky");
            ArrayList roachSprites = new ArrayList();
            for (int i = 0; i < 10; i++)
            {
                roachSprites.Add(Content.Load<Texture2D>("Sprites\\roach\\roach" + i));
            }

            for (int j = 0; j < maxEnemies; j++)
            {
                if (j % 10 == 0)
                    enemies[j] = new GameObject(temp, 2);
                else if (j == maxEnemies - 2)
                    enemies[j] = new GameObject(stinky, 8);
                else if (j % 2 == 0)
                    enemies[j] = new GameObject(littlebitch, 1);
                else if (j == maxEnemies - 1)
                    enemies[j] = new GameObject(bigboss, 3);
                else
                    enemies[j] = new AnimatedGameObject(roachSprites, 9);
            }

            //spell menu textures
            sMenu = new Texture2D[4];
            sMenu[0] = Content.Load<Texture2D>("SpellMenus\\SpiderBar1Active");
            sMenu[1] = Content.Load<Texture2D>("SpellMenus\\SpiderBar2Active");
            sMenu[2] = Content.Load<Texture2D>("SpellMenus\\SpiderBar3Active");
            sMenu[3] = Content.Load<Texture2D>("SpellMenus\\SpiderBar4Active");

            
            // The maximum amount of scores to display on screen is the maximum number of dead enemies
            score = new ArrayList();

            // Loading in the font we will use for showing the killed enemies score value
            scorefont = Content.Load<SpriteFont>("ScoreFont");
            levelfont = Content.Load<SpriteFont>("LevelFont");
            titlefont = Content.Load<SpriteFont>("TitleFont");

            spriteBatch = new SpriteBatch(GraphicsDevice);
            getReady = Content.Load<Texture2D>("Backgrounds\\getready");


            healthBack = Content.Load<Texture2D>("Sprites\\HealthBars\\HealthBarBorder");
            healthFrontP1 = Content.Load<Texture2D>("Sprites\\HealthBars\\HealthBarFill1");
            healthFrontP2 = Content.Load<Texture2D>("Sprites\\HealthBars\\HealthBarFill2");
            antman_top = Content.Load<Texture2D>("Sprites\\antman_top");
            spiderman_top = Content.Load<Texture2D>("Sprites\\spidman_top");
            antman_bottom = Content.Load<Texture2D>("Sprites\\antman_bottom");
            spiderman_bottom = Content.Load<Texture2D>("Sprites\\spidman_bottom");
            player1 = new Player(1,antman_top, antman_bottom, p1_w, new Vector2(viewport.Width * 7 / 15, viewport.Height / 2), 1, new Statistics(true), sMenu, viewport, levelfont, healthBack, healthFrontP1);
            player2 = new Player(2, spiderman_top, spiderman_bottom, p2_w, new Vector2(viewport.Width * 8 / 15, viewport.Height / 2), 2, new Statistics(true), sMenu, viewport, levelfont, healthBack, healthFrontP2);


            bloodExplosion = Content.Load<ParticleEffect>("Particles\\bloody");
            pickupGlow = Content.Load<ParticleEffect>("Particles\\glow");
            bulletHit = Content.Load<ParticleEffect>("Particles\\bullet");
            stinkyBug = Content.Load<ParticleEffect>("Particles\\stinky");

            bloodExplosion.Initialise();
            pickupGlow.Initialise();
            bulletHit.Initialise();
            stinkyBug.Initialise();
            
            bloodExplosion.LoadContent(Content);
            pickupGlow.LoadContent(Content);
            bulletHit.LoadContent(Content);
            stinkyBug.LoadContent(Content);

            particleRenderer = new PointSpriteRenderer
            {
                GraphicsDeviceService = graphics
            };
            particleRenderer.LoadContent(Content);
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
        /// 


        private void fireBullets(Player p, GameObject[] b)
        {
            if (p.activeWeapon == 1)
            {
                if(timeEffect.isActive)
                    shotgunSoundSlow.Play();
                    else
                shotgunSound.Play();
            }
            else
            {
                if (timeEffect.isActive)
                    shootSoundSlow.Play();
                else
                    shootSound.Play();
            }
            //firing command
            //automatically resorts to unlimited weapon if current weapon is out of ammo
            if (p.spellMenu.bulletsLeft[p.activeWeapon] == 0)
            {
                p.spellMenu.stateReset();
                p.activeWeapon = 0;
            }

            if (p.activeWeapon == 0)
            {
                foreach (GameObject bullet in b)
                {
                    if (!bullet.alive)
                    {
                        Vector2 temp2 = new Vector2(p.position.X + p.spriteT.Width / 2, p.position.Y + p.spriteT.Height / 2 - 8);
                        Vector2 temp = MathFns.newPoint(p.position, temp2, (float)(-p.rotation+Math.PI/2));
                        bullet.alive = true;
                        bullet.position = temp;
                        bullet.velocity = new Vector2((float)Math.Cos(p.rotation + Math.PI / 2), (float)Math.Sin(p.rotation + Math.PI / 2)) * 8.0f;
                        return;
                    }
                }
            }
            if (p.activeWeapon == 1)
            {
                double spread = -0.2094;
                foreach (GameObject bullet in b)
                {
                    if (!bullet.alive)
                    {
                        Vector2 temp2 = new Vector2(p.position.X + p.spriteT.Width / 2, p.position.Y + p.spriteT.Height / 2 - 8);
                        Vector2 temp = MathFns.newPoint(p.position, temp2, (float)(-p.rotation + Math.PI / 2));
                        bullet.alive = true;
                        bullet.position = temp;
                        bullet.velocity = new Vector2((float)Math.Cos(p.rotation + Math.PI / 2 + spread), (float)Math.Sin(p.rotation + Math.PI / 2 + spread)) * 8.0f;
                        spread += .1047;
                        p.spellMenu.bulletsLeft[p.spellMenu.CurState]--;
                        if (spread > .21)
                            return;
                    }
                }
            }
            if (p.activeWeapon == 2)
            {
                foreach (GameObject bullet in b)
                {
                    if (!bullet.alive)
                    {
                        Vector2 temp2 = new Vector2(p.position.X + p.spriteT.Width / 2, p.position.Y + p.spriteT.Height / 2 - 8);
                        Vector2 temp = MathFns.newPoint(p.position, temp2, (float)(-p.rotation + Math.PI / 2));
                        bullet.alive = true;
                        bullet.position = temp;
                        bullet.velocity = new Vector2((float)Math.Cos(p.rotation + Math.PI / 2 + rand.NextDouble()), (float)Math.Sin(p.rotation + Math.PI / 2 + rand.NextDouble())) * 3.0f;
                        p.spellMenu.bulletsLeft[p.spellMenu.CurState]--;
                        return;
                    }
                }
            }
        }
        private void fireTurretBullets1()
        {
            //firing command

            foreach (GameObject bullet in turretBullets1)
            {
                if (!bullet.alive)
                {
                    bullet.alive = true;
                    bullet.position = turret1.position - bullet.center;
                    bullet.velocity = new Vector2((float)Math.Cos(turret1.rotation + Math.PI / 2), (float)Math.Sin(turret1.rotation + Math.PI / 2)) * 15.0f;
                    turret1.bulletsLeft -= 1;
                    return;
                }
            }
        }
        private void fireTurretBullets2()
        {
            foreach (GameObject bullet in turretBullets2)
            {
                if (!bullet.alive)
                {
                    bullet.alive = true;
                    bullet.position = turret2.position - bullet.center;
                    bullet.velocity = new Vector2((float)Math.Cos(turret2.rotation + Math.PI / 2), (float)Math.Sin(turret2.rotation + Math.PI / 2)) * 15.0f;
                    turret2.bulletsLeft -= 1;
                    return;
                }
            }
        }

        private void generateWeaponPickup(Vector2 pos)
        {
            Console.WriteLine("A new pickup was created"+pos);
            Texture2D sample_weapon = Content.Load<Texture2D>("Sprites\\raygun");
            pickups.Add(new WeaponPickup(sample_weapon, pos, rand.Next(2)+1));
        }



        protected override void Update(GameTime gameTime)
        {


            // Allows the game to exit

            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed)
                this.Exit();

            if (pm.Active || gm.Active)
            {
                this.updateInput();
            }
            else
            {


                if (!timeEffect.isActive || (timeEffect.isActive && timeEffect.counter % 2 == 0))
                {
                    updateInput();
                    if (!act_fade && !gm.Active)
                    {
                        if (GamePad.GetState(PlayerIndex.One).Buttons.A == ButtonState.Pressed)
                            player1.deploy = true;
                        if (GamePad.GetState(PlayerIndex.Two).Buttons.A == ButtonState.Pressed)
                            player2.deploy = true;

                        float elapsed = (float)gameTime.ElapsedGameTime.TotalSeconds;
                        gt = gameTime;
                        elapsedTime += elapsed;
                        elapsedTime2 += elapsed;
                        t_elapsedTime += elapsed;
                        t2_elapsedTime += elapsed;
                        // TODO: Add your update logic here
                        updateTurret(turret1, player1);
                        updateTurret(turret2, player2);
                        updateBullets();
                        updateEnemies();

                        //updates everything for players

                        updatePlayer(player1);
                        updatePlayer(player2);

                        #region Fire Delay
                        if ((elapsedTime >= player1.weapon.delays[player1.activeWeapon]) && player1.fire && !player1.narc.Active)
                        {
                            elapsedTime = 0.0f;
                            fireBullets(player1, this.bullets);
                        }
                        if ((elapsedTime2 >= player2.weapon.delays[player2.activeWeapon]) && player2.fire && !player2.narc.Active)
                        {
                            elapsedTime2 = 0.0f;
                            fireBullets(player2, this.bullets2);
                        }

                        if (turret1.fire && t_elapsedTime >= fireDelay + .5 && turret1.placed)
                        {
                            t_elapsedTime = 0.0f;
                            fireTurretBullets1();
                        }
                        if (turret2.fire && t2_elapsedTime >= fireDelay + .5 && turret2.placed)
                        {
                            t2_elapsedTime = 0.0f;
                            fireTurretBullets2();
                        }
                        #endregion

                        // Update the statistical time used to calculate player statistics
                        player1.stat.updateStatisticsTime(gameTime);
                        player2.stat.updateStatisticsTime(gameTime);

                        // particle test
                        float deltaSeconds = (float)gameTime.ElapsedGameTime.TotalSeconds;
                        bloodExplosion.Update(deltaSeconds);
                        pickupGlow.Update(deltaSeconds);
                        bulletHit.Update(deltaSeconds);
                        stinkyBug.Update(deltaSeconds);

                        timeEffect.counter = 1;

                    }
                }
                else if (timeEffect.isActive)
                {
                    timeEffect.counter++;
                    if (timeEffect.length > 0)
                    {
                        timeEffect.length--;
                        //Console.WriteLine(timeEffect.length);
                    }
                    else
                    {
                        timeEffect.isActive = false;
                        timeEffect.length = 200;
                        timeEffect.counter = 1;
                    }
                }
            }
            base.Update(gameTime);
        }
        
        #region UpdateHelpers (updatePlayer, updatePlayerMovement, updatePickups, UpdateTurret, UpdateInput, updateBullets, updateEnemies)
        private void updatePlayer(Player p)
        {
            p.healthBar.Update();
            if (p.ID == 1)
                p.narc.Update(GamePad.GetState(PlayerIndex.One), Keyboard.GetState());
            else
                p.narc.Update(GamePad.GetState(PlayerIndex.Two), Keyboard.GetState());
            if (!p.narc.Active)
            {
                updatePlayerMovement(p);
                updatePickups(p);
                if (p.spellMenu.Active)
                    p.spellMenu.moveOn();
                else
                    p.spellMenu.moveOff();
                p.activeWeapon = p.spellMenu.CurState;
            }
        }
        private void updatePlayerMovement(Player p)
        {
            if (!p.narc.Active)
            {
                float xmove = p.position.X + p.velocity.X;
                float ymove = p.position.Y + p.velocity.Y;
                if ((xmove > (p.spriteB.Width / 2) && xmove < (viewport.Width - p.spriteB.Width / 2)) && (ymove > (p.spriteB.Height / 2) && ymove < (viewport.Height - p.spriteB.Height / 2)))
                    p.position += p.velocity;
                if (p.energy < 100)
                    p.energy++;
            }
        }
        private void updatePickups(Player p)
            {
                WeaponPickup destroyedPickup = null;

                Rectangle playerRect = new Rectangle(
                        (int)p.position.X,
                        (int)p.position.Y,
                        p.spriteB.Width,
                        p.spriteB.Height);
                foreach (WeaponPickup pickup in pickups)
                {
                        Rectangle pickupRect = new Rectangle(
                          (int)pickup.position.X,
                          (int)pickup.position.Y,
                          pickup.sprite.Width,
                          pickup.sprite.Height);

                        if (playerRect.Intersects(pickupRect))
                        {
                            destroyedPickup = pickup;
                            p.spellMenu.bulletsLeft[pickup.weaponIndex] += 20;
                            p.spellMenu.Active = true;
                            p.spellMenu.CurState = pickup.weaponIndex;
                            p.activeWeapon = pickup.weaponIndex;
                            p.stat.incrementWeaponPickup(pickup);
                            break;
                        }
                    } 
                if (destroyedPickup != null)
                    pickups.Remove(destroyedPickup);
            }
        private void updateTurret(Turret turret, Player player)
        {
            if (turret.bulletsLeft > 0)
            {

                if (player.deploy)
                {
                    turret.position = player.position;
                    turret.rotation = 0f;
                    turret.placed = true;
                    player.deploy = false;
                }
                if (turret.placed)
                {
                    double temp = MathFns.Distance(turret.position, enemies[0].position);
                    turret.closestEnemy = 0;
                    Vector2 offset = new Vector2(); //allows for aiming at center of target

                    for (int i = 1; i < enemies.Length; i++)
                    {
                        if (MathFns.Distance(enemies[i].position,turret.position) < temp)
                        {
                           turret.closestEnemy = i;
                           offset.X = enemies[i].sprite.Width/2;
                           offset.Y = enemies[i].sprite.Height/2;
                        }
                    }

                    Vector2 aim = new Vector2(enemies[turret.closestEnemy].position.X + offset.X, enemies[turret.closestEnemy].position.Y + offset.Y);
                    Vector2 direction = turret.position - aim;
                    
                    direction.Normalize();
                    float desiredAngle = (float)Math.Acos((double)direction.X);

                    if (direction.Y < 0)
                    {
                        desiredAngle = (float)(2.0f * Math.PI) - (float)desiredAngle;
                    }

                    //Smooth Rotation
                    if (desiredAngle != turret.rotation)
                    {
                      // Console.WriteLine("Turret: " + turret.rotation + "Desired: " + desiredAngle);
                       turret.rotation = MathFns.Clerp(turret.rotation, desiredAngle + (float)Math.PI / 2, this.turret_rot);
                    }
                    
                    //previous rotation function
                    //turret.rotation = desiredAngle + (float)Math.PI / 2;
                    turret.fire = true;
                }
            }
            else
            {
                turret.placed = false;
                player.deploy = false;
                turret.bulletsLeft = 25;

            }
        }
        private void updateInput()
        {


            GamePadState currentState;
            currentState = GamePad.GetState(PlayerIndex.One);


            if (gm.Active)
            {
                #region Xbox Menu Controls
                if (currentState.IsConnected)
                {
                    if (currentState.Buttons.A == ButtonState.Pressed && !gm.Select)
                    {
                        switch (gm.State)
                        {
                            case 0:
                                gm.Active = false;
                                act_fade = true;
                                fade_out = true;
                                scoreScreen = false;
                                fade_in = false;
                                current_fade = 255;
                                break;
                            default:
                                Exit();
                                break;
                        }
                    }
                    else if (currentState.ThumbSticks.Left.Y > 0.5 && !gm.Select)
                    {
                        gm.stateDec();
                        gm.Select = true;
                    }
                    else if (currentState.ThumbSticks.Left.Y < -0.5 && !gm.Select)
                    {
                        gm.stateInc();
                        gm.Select = true;
                    }
                    else if (-0.5 < currentState.ThumbSticks.Left.Y && currentState.ThumbSticks.Left.Y < 0.5 && currentState.Buttons.A == ButtonState.Released)
                        gm.Select = false;
                #endregion

                }
                else
                {
                    #region Keyboard Menu Controls
                    KeyboardState k = Keyboard.GetState();
                    if (k.IsKeyDown(Keys.Enter) && !gm.Select)
                    {
                        switch (gm.State)
                        {
                            case 0:
                                gm.Active = false;
                                act_fade = true;
                                fade_out = true;
                                scoreScreen = false;
                                fade_in = false;
                                current_fade = 255;
                                break;
                            default:
                                Exit();
                                break;
                        }
                    }
                    else if (k.IsKeyDown(Keys.Up) && !gm.Select)
                    {
                        gm.stateDec();
                        gm.Select = true;
                    }
                    else if (k.IsKeyDown(Keys.Down) && !gm.Select)
                    {
                        gm.stateInc();
                        gm.Select = true;
                    }
                    else if (k.IsKeyUp(Keys.Down) && k.IsKeyUp(Keys.Up) && k.IsKeyUp(Keys.Enter))
                        gm.Select = false;
                    #endregion
                }

            }

            else if (pm.Active)
            {
                #region Xbox Menu Controls
                if (currentState.IsConnected)
                {
                    if (currentState.Buttons.A == ButtonState.Pressed)
                    {
                        switch (gm.State)
                        {
                            case 0:
                                pm.Active = false;
                                break;
                            case 1:
                                pm.Active = false;
                                gm.Active = true;
                                gm.Select = true;
                                break;
                            default:
                                Exit();
                                break;
                        }
                    }
                    else if (currentState.ThumbSticks.Left.Y > 0.5 && !pm.Select)
                    {
                        pm.stateDec();
                        pm.Select = true;
                    }
                    else if (currentState.ThumbSticks.Left.Y < -0.5 && !pm.Select)
                    {
                        pm.stateInc();
                        pm.Select = true;
                    }
                    else if (-0.5 < currentState.ThumbSticks.Left.Y && currentState.ThumbSticks.Left.Y < 0.5)
                        pm.Select = false;
                #endregion

                }
                else
                {
                    #region Keyboard Menu Controls
                    KeyboardState k = Keyboard.GetState();
                    if (k.IsKeyDown(Keys.Enter))
                    {
                        switch (pm.State)
                        {
                            case 0:
                                pm.Active = false;
                                break;
                            case 1:
                                pm.Active = false;
                                gm.Active = true;
                                gm.Select = true;
                                break;
                            default:
                                Exit();
                                break;
                        }
                    }
                    else if (k.IsKeyDown(Keys.Up) && !pm.Select)
                    {
                        pm.stateDec();
                        pm.Select = true;
                    }
                    else if (k.IsKeyDown(Keys.Down) && !pm.Select)
                    {
                        pm.stateInc();
                        pm.Select = true;
                    }
                    else if (k.IsKeyUp(Keys.Down) && k.IsKeyUp(Keys.Up))
                        pm.Select = false;
                    #endregion
                }
            }
            else
            {
                #region Player 1 Control Scheme
                if (!player1.narc.Active)
                {
                    if (currentState.IsConnected)
                    {
                        #region XBox Controls Player1
                        if (currentState.Buttons.Start == ButtonState.Pressed)
                            this.pm.Active = true;
                        
                        player1.velocity.X = currentState.ThumbSticks.Left.X * 5;
                        player1.velocity.Y = -currentState.ThumbSticks.Left.Y * 5;
                        //player1.rotation = -(float)((Math.Tan(currentState.ThumbSticks.Right.Y / currentState.ThumbSticks.Right.X)*2*Math.PI)/180);
                        const float DEADZONE = 0.2f;
                        const float FIREDEADZONE = 0.3f;

                        Vector2 direction = GamePad.GetState(PlayerIndex.One).ThumbSticks.Right;
                        float magnitude = direction.Length();
                        player1.fire = false;
                        if (GamePad.GetState(PlayerIndex.One).ThumbSticks.Left.Length() > DEADZONE)
                        {
                            player1.rotation_b = (float)(-1 * (3.14 / 2 + Math.Atan2(currentState.ThumbSticks.Left.Y, currentState.ThumbSticks.Left.X)));
                        }

                        if (magnitude > DEADZONE)
                        {
                            //Smooth Rotation
                            float angle = (float)(-1 * (3.14 / 2 + Math.Atan2(currentState.ThumbSticks.Right.Y, currentState.ThumbSticks.Right.X)));

                            if (angle != player1.rotation)
                                player1.rotation = MathFns.Clerp(player1.rotation, angle, angle_rot);

                            if (magnitude > FIREDEADZONE)
                            {
                                player1.fire = true;
                            }

                        }
                        //move spell on/off
                        if (GamePad.GetState(PlayerIndex.One).DPad.Up == ButtonState.Pressed)
                            player1.spellMenu.Active = true;
                        if (GamePad.GetState(PlayerIndex.One).DPad.Down == ButtonState.Pressed)
                            player2.spellMenu.Active = false;

                        //move indication of spell
                        if (player1.spellMenu.Active)
                        {
                            //move right
                            if (GamePad.GetState(PlayerIndex.One).Buttons.RightShoulder == ButtonState.Pressed && !press1a)
                            {
                                player1.spellMenu.stateInc();
                                player1.activeWeapon = player1.spellMenu.CurState;
                                press1a = true;
                            }
                            else if (GamePad.GetState(PlayerIndex.One).Buttons.RightShoulder == ButtonState.Released)
                                press1a = false;

                            //move left
                            if (GamePad.GetState(PlayerIndex.One).Buttons.LeftShoulder == ButtonState.Pressed && !press1b)
                            {
                                player1.spellMenu.stateDec();
                                player1.activeWeapon = player1.spellMenu.CurState;
                                press1b = true;
                            }
                            else if (GamePad.GetState(PlayerIndex.One).Buttons.RightShoulder == ButtonState.Released)
                                press1b = false;
                        }
                        #endregion

                    }
                    //keyboard controls
                    else
                    {
                        #region Keyboard Controls Player1
                        //player1.fire = false;
                        KeyboardState keyboardState = Keyboard.GetState();
                        MouseState mouse = Mouse.GetState();
                        float XDistance = player1.position.X - mouse.X;
                        float YDistance = player1.position.Y - mouse.Y;
                        float xdim = 800;
                        float ydim = 800;

                        float rotation = (float)(Math.Atan2(YDistance, XDistance) + Math.PI / 2);
                        player1.rotation = rotation;

                        if (mouse.LeftButton == ButtonState.Pressed)
                        {
                            player1.fire = true;
                            xdim = mouse.X;
                            ydim = mouse.Y;
                        }
                        else
                        {
                            player1.fire = false;
                        }
                        if (keyboardState.IsKeyDown(Keys.Left))
                        {
                            player1.velocity.X = -5;
                        }
                        else if (keyboardState.IsKeyDown(Keys.Right))
                        {
                            player1.velocity.X = 5;
                        }
                        else
                        {
                            player1.velocity.X = 0;
                        }
                        if (keyboardState.IsKeyDown(Keys.Up))
                        {
                            player1.velocity.Y = -5;
                        }
                        else if (keyboardState.IsKeyDown(Keys.Down))
                        {
                            player1.velocity.Y = 5;
                        }
                        else
                        {
                            player1.velocity.Y = 0;
                        }
                        if (keyboardState.IsKeyDown(Keys.Z))
                        {
                            player1.deploy = true;
                        }

                        float a1 = (float)((-1 * (3.14 / 2 + Math.Atan2(player1.velocity.X, player1.velocity.Y))) + Math.PI / 2);
                        if (a1 != player1.rotation_b)
                            player1.rotation_b = MathFns.Clerp(player1.rotation_b, a1, angle_rot);


                        if (keyboardState.IsKeyDown(Keys.Escape))
                        {
                            this.pm.Active = true;
                        }


                        //move spell menu on/off
                        if (keyboardState.IsKeyDown(Keys.I))
                            player1.spellMenu.Active = true;
                        if (keyboardState.IsKeyDown(Keys.K))
                            player1.spellMenu.Active = false;


                        if (player1.spellMenu.Active)
                        {
                            //move left
                            if (keyboardState.IsKeyDown(Keys.U) && !press1a)
                            {
                                //TODO:: Fix from rotating through too fast
                                player1.spellMenu.stateDec();
                                player1.activeWeapon = player1.spellMenu.CurState;
                                press1a = true;
                            }
                            else if (keyboardState.IsKeyUp(Keys.U))
                                press1a = false;

                            //move right
                            if (keyboardState.IsKeyDown(Keys.O) && !press1b)
                            {
                                player1.spellMenu.stateInc();
                                player1.activeWeapon = player1.spellMenu.CurState;
                                press1b = true;
                            }
                            else if (keyboardState.IsKeyUp(Keys.O))
                                press1b = false;
                        }





                        //cannon.rotation = MathHelper.Clamp(cannon.rotation, MathHelper.PiOver2, 0);
                        // TODO: Add your update logic here
                        previousKeyboardState = keyboardState;
                        previousMouseState = mouse;

                        #endregion
                    }
                }
                #endregion



                #region Player 2 Control Scheme
                currentState = GamePad.GetState(PlayerIndex.Two);
                if (!player2.narc.Active)
                {
                    if (currentState.IsConnected)
                    {
                        #region XBox Controller Controls Player 2
                        if (currentState.Buttons.Start == ButtonState.Pressed)
                            this.pm.Active = true;
                        
                        player2.velocity.X = currentState.ThumbSticks.Left.X * 5;
                        player2.velocity.Y = -currentState.ThumbSticks.Left.Y * 5;
                        //player2.rotation = -(float)((Math.Tan(currentState.ThumbSticks.Right.Y / currentState.ThumbSticks.Right.X)*2*Math.PI)/180);
                        const float DEADZONE = 0.2f;
                        const float FIREDEADZONE = 0.3f;

                        Vector2 direction = GamePad.GetState(PlayerIndex.Two).ThumbSticks.Right;
                        float magnitude = direction.Length();
                        player2.fire = false;
                        if (GamePad.GetState(PlayerIndex.Two).ThumbSticks.Left.Length() > DEADZONE)
                        {
                            player2.rotation_b = (float)(-1 * (3.14 / 2 + Math.Atan2(currentState.ThumbSticks.Left.Y, currentState.ThumbSticks.Left.X)));
                        }

                        if (magnitude > DEADZONE)
                        {
                            //Smooth Rotation
                            float angle = (float)(-1 * (3.14 / 2 + Math.Atan2(currentState.ThumbSticks.Right.Y, currentState.ThumbSticks.Right.X)));

                            if (angle != player2.rotation)
                                player2.rotation = MathFns.Clerp(player2.rotation, angle, angle_rot);

                            if (magnitude > FIREDEADZONE)
                            {
                                player2.fire = true;
                            }
                        }

                        if (GamePad.GetState(PlayerIndex.One).DPad.Up == ButtonState.Pressed)
                            player1.spellMenu.Active = true;
                        if (GamePad.GetState(PlayerIndex.One).DPad.Down == ButtonState.Pressed)
                            player2.spellMenu.Active = false;



                        if (player2.spellMenu.Active)
                        {
                            //Move right
                            if (GamePad.GetState(PlayerIndex.One).Buttons.RightShoulder == ButtonState.Pressed && !press2a)
                            {
                                player2.spellMenu.stateInc();
                                player2.activeWeapon = player2.spellMenu.CurState;
                                press2a = true;
                            }
                            else if (GamePad.GetState(PlayerIndex.Two).Buttons.LeftShoulder == ButtonState.Released)
                                press2a = false;

                            //move left
                            if (GamePad.GetState(PlayerIndex.One).Buttons.LeftShoulder == ButtonState.Pressed && !press2b)
                            {
                                player2.spellMenu.stateDec();
                                player2.activeWeapon = player2.spellMenu.CurState;
                                press2b = true;
                            }
                            else if (GamePad.GetState(PlayerIndex.Two).Buttons.RightShoulder == ButtonState.Released)
                                press2b = false;
                        }

                        #endregion

                    }
                    //keyboard controls
                    else
                    {
                        #region Keyboard Controls Player 2

                        //player2.fire = false;
                        KeyboardState keyboardState = Keyboard.GetState();
                        MouseState mouse = Mouse.GetState();
                        float XDistance = player2.position.X - mouse.X;
                        float YDistance = player2.position.Y - mouse.Y;
                        float xdim = 800;
                        float ydim = 800;

                        float rotation = (float)(Math.Atan2(YDistance, XDistance) + Math.PI / 2);
                        player2.rotation = rotation;

                        if (keyboardState.IsKeyDown(Keys.Space))
                        {
                            player2.fire = true;
                            xdim = mouse.X;
                            ydim = mouse.Y;
                        }
                        else
                        {
                            player2.fire = false;
                        }
                        if (keyboardState.IsKeyDown(Keys.A))
                        {
                            player2.velocity.X = -5;
                        }
                        else if (keyboardState.IsKeyDown(Keys.D))
                        {
                            player2.velocity.X = 5;
                        }
                        else
                        {
                            player2.velocity.X = 0;
                        }
                        if (keyboardState.IsKeyDown(Keys.W))
                        {
                            player2.velocity.Y = -5;
                        }
                        else if (keyboardState.IsKeyDown(Keys.S))
                        {
                            player2.velocity.Y = 5;
                        }
                        else
                        {
                            player2.velocity.Y = 0;
                        }
                        if (keyboardState.IsKeyDown(Keys.Q))
                        {
                            player2.deploy = true;
                        }


                        float a2 = (float)((-1 * (3.14 / 2 + Math.Atan2(player2.velocity.X, player2.velocity.Y))) + Math.PI / 2);
                        if (a2 != player2.rotation_b)
                            player2.rotation_b = MathFns.Clerp(player2.rotation_b, a2, angle_rot);

                        if (keyboardState.IsKeyDown(Keys.Escape))
                        {
                            this.pm.Active = true;
                        }


                        if (keyboardState.IsKeyDown(Keys.NumPad8))
                            player2.spellMenu.Active = true;
                        if (keyboardState.IsKeyDown(Keys.NumPad5))
                            player2.spellMenu.Active = false;

                        if (player2.spellMenu.Active)
                        {
                            if (keyboardState.IsKeyDown(Keys.NumPad7) && !press2a)
                            {
                                //TODO:: Fix from rotating through too fast
                                player2.spellMenu.stateDec();
                                player2.activeWeapon = player2.spellMenu.CurState;
                                press2a = true;
                            }
                            else if (keyboardState.IsKeyUp(Keys.NumPad7))
                                press2a = false;

                            //move right
                            if (keyboardState.IsKeyDown(Keys.NumPad9) && !press2b)
                            {
                                player2.spellMenu.stateInc();
                                player2.activeWeapon = player2.spellMenu.CurState;
                                press2b = true;
                            }
                            else if (keyboardState.IsKeyUp(Keys.NumPad9))
                                press2b = false;
                        }






                        //cannon.rotation = MathHelper.Clamp(cannon.rotation, MathHelper.PiOver2, 0);
                        // TODO: Add your update logic here
                        previousKeyboardState = keyboardState;
                        previousMouseState = mouse;
                        #endregion Keyboard Controls

                    }

                }
                #endregion
            }
        }
        public void updateBullets()
        {
            #region Player 1 Bullets
            foreach (GameObject bullet in bullets)
            {
                if (bullet.alive)
                {
                    if (player1.activeWeapon == 2)
                    {
                        Rectangle flameRect = new Rectangle((int)player1.position.X - 100, (int)player1.position.Y - 100, 200, 200);
                        Rectangle flameBulletRect = new Rectangle((int)bullet.position.X, (int)bullet.position.Y, bullet.sprite.Width, bullet.sprite.Height);
                        if (!flameRect.Intersects(flameBulletRect))
                            bullet.alive = false;
                    }

                    bullet.position += bullet.velocity;
                    if (!viewportRect.Contains(new Point((int)bullet.position.X, (int)bullet.position.Y)))
                    {
                        bullet.alive = false;
                        continue;
                    }
                    Rectangle bulletRect = new Rectangle(
                        (int)bullet.position.X,
                        (int)bullet.position.Y,
                        bullet.sprite.Width,
                        bullet.sprite.Height);
                    //enemy-bullet collision detection.
                    foreach (GameObject enemy in enemies)
                    {
                        if (enemy.alive)
                        {
                            Rectangle enemyRect = new Rectangle(
                                (int)enemy.position.X,
                                (int)enemy.position.Y,
                                enemy.sprite.Width,
                                enemy.sprite.Height);

                            if (MathFns.broadPhaseCollision(bulletRect, enemyRect, enemy.rotation))
                            {

                                bullet.alive = false;
                                enemy.Update(bullet.Damage);
                                bulletHit.Trigger(bullet.position);
                                if (!enemy.alive)
                                {
                                    // Bloody Explosion Particle Effect
                                    bloodExplosion.Trigger(enemy.position);

                                    score.Add(new ScoreDisplay(20, SCORE_TIME, enemy.position, true, 1));
                                    player1.score += 20;
                                    enemies_killed++;

                                    player1.stat.enemyKilled();

                                    // Possibly Generate a new WeaponPickup
                                    int wpn_pickup_prob = rand.Next(100);
                                    if (wpn_pickup_prob < 10)
                                    {
                                        generateWeaponPickup(enemy.position);
                                    }
                                    break;
                                }
                            }
                        }
                    }
                }
            }
            #endregion
            #region Player 2 Bullets
            foreach (GameObject bullet in bullets2)
            {
                if (bullet.alive)
                {
                    bullet.position += bullet.velocity;
                    if (!viewportRect.Contains(new Point((int)bullet.position.X, (int)bullet.position.Y)))
                    {
                        bullet.alive = false;
                        continue;
                    }
                    Rectangle bulletRect = new Rectangle(
                        (int)bullet.position.X,
                        (int)bullet.position.Y,
                        bullet.sprite.Width,
                        bullet.sprite.Height);
                    //enemy-bullet collision detection.
                    foreach (GameObject enemy in enemies)
                    {
                        if (enemy.alive)
                        {
                            Rectangle enemyRect = new Rectangle(
                                (int)enemy.position.X,
                                (int)enemy.position.Y,
                                enemy.sprite.Width,
                                enemy.sprite.Height);

                            if (MathFns.broadPhaseCollision(bulletRect, enemyRect, enemy.rotation))
                            {
                                enemy.Update(bullet.Damage);
                                bullet.alive = false;
                                bulletHit.Trigger(bullet.position);
                                if (!enemy.alive)
                                {
                                    // Bloody Explosion Particle Effect
                                    bloodExplosion.Trigger(new Vector2(enemy.position.X, enemy.position.Y));

                                    score.Add(new ScoreDisplay(20, SCORE_TIME, enemy.position, true, 2));
                                    player2.score += 20;
                                    enemies_killed++;

                                    player2.stat.enemyKilled();

                                    // Possibly Generate a new WeaponPickup
                                    int wpn_pickup_prob = rand.Next(100);
                                    if (wpn_pickup_prob < 10)
                                    {
                                        generateWeaponPickup(enemy.position);
                                    }
                                    break;
                                }
                            }
                        }
                    }
                }
            }
            #endregion
            #region Turret 1 Bullets
            foreach (GameObject bullet in turretBullets1)
            {
                if (bullet.alive)
                {
                    bullet.position += bullet.velocity;
                    if (!viewportRect.Contains(new Point((int)bullet.position.X, (int)bullet.position.Y)))
                    {
                        bullet.alive = false;
                        continue;
                    }
                    Rectangle bulletRect = new Rectangle(
                        (int)bullet.position.X,
                        (int)bullet.position.Y,
                        bullet.sprite.Width,
                        bullet.sprite.Height);
                    //enemy-bullet collision detection.
                    foreach (GameObject enemy in enemies)
                    {
                        if (enemy.alive)
                        {
                            Rectangle enemyRect = new Rectangle(
                                (int)enemy.position.X,
                                (int)enemy.position.Y,
                                enemy.sprite.Width,
                                enemy.sprite.Height);

                            if (MathFns.broadPhaseCollision(bulletRect, enemyRect, enemy.rotation))
                            {
                                bullet.alive = false;
                                enemy.Update(bullet.Damage);
                                bulletHit.Trigger(bullet.position);
                                if (!enemy.alive)
                                {
                                    // Bloody Explosion Particle Effect
                                    bloodExplosion.Trigger(new Vector2(enemy.position.X, enemy.position.Y));

                                    score.Add(new ScoreDisplay(10, SCORE_TIME, enemy.position, true, 1));
                                    player1.score += 10;
                                    enemies_killed++;
                                    break;
                                }
                            }
                        }
                    }
                }
            }
            #endregion
            #region Turret 2 Tullets
            foreach (GameObject bullet in turretBullets2)
            {
                if (bullet.alive)
                {
                    bullet.position += bullet.velocity;
                    if (!viewportRect.Contains(new Point((int)bullet.position.X, (int)bullet.position.Y)))
                    {
                        bullet.alive = false;
                        continue;
                    }
                    Rectangle bulletRect = new Rectangle(
                        (int)bullet.position.X,
                        (int)bullet.position.Y,
                        bullet.sprite.Width,
                        bullet.sprite.Height);
                    //enemy-bullet collision detection.
                    foreach (GameObject enemy in enemies)
                    {
                        if (enemy.alive)
                        {
                            Rectangle enemyRect = new Rectangle(
                                (int)enemy.position.X,
                                (int)enemy.position.Y,
                                enemy.sprite.Width,
                                enemy.sprite.Height);

                            if (MathFns.broadPhaseCollision(bulletRect, enemyRect, (float)(enemy.rotation + Math.PI / 2)))
                            {
                                bullet.alive = false;
                                enemy.Update(bullet.Damage);
                                bulletHit.Trigger(bullet.position);
                                if (!enemy.alive)
                                {
                                    // Bloody Explosion Particle Effect
                                    bloodExplosion.Trigger(new Vector2(enemy.position.X, enemy.position.Y));

                                    score.Add(new ScoreDisplay(10, SCORE_TIME, enemy.position, true, 2));
                                    player2.score += 10;
                                    enemies_killed++;
                                    break;
                                }
                            }
                        }
                    }
                }
            }
            #endregion
        }
        public void updateEnemies()
        {
            foreach (GameObject enemy in enemies)
            {
                if (enemy.alive)
                {
                    //checks for collision with the player

                    Rectangle playerRect = new Rectangle((int)player1.position.X - player1.spriteB.Width / 2, (int)player1.position.Y - player1.spriteB.Height / 2, player1.spriteB.Width, player1.spriteB.Height);
                    Rectangle enemyRect = new Rectangle((int)enemy.position.X, (int)enemy.position.Y, enemy.sprite.Width, enemy.sprite.Height);

                    //detect p1 collision
                    if (MathFns.broadPhaseCollision(playerRect, enemyRect, (float)(enemy.rotation + Math.PI / 2)))
                    {
                        //alive = false;
                        enemy.alive = false;

                        // Bloody Explosion Particle Effect
                        bloodExplosion.Trigger(new Vector2(enemy.position.X, enemy.position.Y));

                        if (player1.healthBar.Current > 0)
                        {
                            player1.healthBar.Decrement(enemy.Damage);
                        }
                        else
                        {
                            if (player1.healthBar.LivesLeft > 0)
                            {
                                player1.healthBar.LivesLeft--;
                                player1.healthBar.Current = 100;
                                player1.stat.playerDied();
                            }
                            else
                            {
                                gameOver = true;
                            }
                        }
                        
                        if(enemy.ID==8){
                            timeEffect.isActive = true;
                            ambianceSound.Play();
                        }

                        enemies_killed++;

                        break;
                    }
                    playerRect = new Rectangle((int)player2.position.X - player2.spriteB.Width / 2, (int)player2.position.Y - player2.spriteB.Height / 2, player2.spriteB.Width, player2.spriteB.Height);

                    //detect p2 collision
                    if (MathFns.broadPhaseCollision(playerRect, enemyRect, (float)(enemy.rotation + Math.PI / 2)))
                    {
                        //alive = false;
                        enemy.alive = false;

                        // Bloody Explosion Particle Effect
                        bloodExplosion.Trigger(new Vector2(enemy.position.X, enemy.position.Y));
                        if (player2.healthBar.Current > 0)
                        {
                            player2.healthBar.Decrement(enemy.Damage);
                        }
                        else
                        {
                            if (player2.healthBar.LivesLeft > 0)
                            {
                                player2.healthBar.LivesLeft--;
                                player2.healthBar.Current = 100;
                                player2.stat.playerDied();
                            }
                            else
                            {
                                gameOver = true;
                            }
                        }

                        // If it is a stink bug, players are poisoned
                        if (enemy.ID==8)
                        {
                            timeEffect.isActive = true;
                            ambianceSound.Play();
                        }
                        enemies_killed++;
                        break;
                    }
                }
                else
                {
                    //limits number of enemies to number per level
                    if (enemies_killed < (enemies_level[level] - maxEnemies + 1))
                    {
                        enemy.alive = true;

                        int rand1 = rand.Next(100);
                        int rand2 = rand.Next(100);
                        if (rand1 < 33)
                        {
                            enemy.position.X = -enemy.sprite.Width - rand.Next(5, 100);
                            enemy.position.Y = rand.Next(viewport.Height);
                        }
                        else if (rand1 < 66)
                        {
                            enemy.position.X = rand.Next(viewport.Width);
                            if (rand2 < 50)
                                enemy.position.Y = -enemy.sprite.Height + rand.Next(5, 100);
                            else
                                enemy.position.Y = viewport.Height + rand.Next(5, 100);
                        }
                        else
                        {
                            enemy.position.X = viewport.Width + rand.Next(5, 100);
                            enemy.position.Y = rand.Next(viewport.Height);
                        }
                    }

                }
                //only update movement if enemy is alive
                if (enemy.alive)
                {
                    //makes enemies move towards the player
                    Vector2 target;

                    if (MathFns.Distance(enemy.position, player1.position) > MathFns.Distance(enemy.position, player2.position))
                        target = player2.position;
                    else
                        target = player1.position;

                    enemy.velocity = target - enemy.position;
                    enemy.velocity.Normalize();
                    enemy.UpdateVelocity();
                    enemy.position += enemy.velocity * 2;
                    float angle = (float)(-1 * (Math.PI / 2 + Math.Atan2(enemy.velocity.X, enemy.velocity.Y)));

                    if (angle != enemy.rotation)
                        enemy.rotation = MathFns.Clerp(enemy.rotation, angle, angle_rot);

                    if (enemy is AnimatedGameObject)
                    {
                        ((AnimatedGameObject)enemy).updateAnim();
                    }
                }
            }
        }

        #endregion


        #region Draw Helpers (DrawPlayers, DrawInformation, DrawPickups, DrawBullets, DrawEnemies, DrawScore)
        private void DrawPlayers(SpriteBatch s)
        {
            s.Draw(player1.spriteB, new Rectangle((int)player1.position.X, (int)player1.position.Y, player1.spriteB.Width, player1.spriteB.Height), null, Color.White, player1.rotation_b, new Vector2(player1.spriteB.Width / 2, player1.spriteB.Height / 2), SpriteEffects.None, 0);
            s.Draw(player1.spriteT, new Rectangle((int)player1.position.X, (int)player1.position.Y, player1.spriteT.Width, player1.spriteT.Height), null, Color.White, (float)(player1.rotation + .5 * Math.PI), new Vector2(player1.spriteT.Width / 2, player1.spriteT.Height / 2), SpriteEffects.None, 0);
            s.Draw(player2.spriteB, new Rectangle((int)player2.position.X, (int)player2.position.Y, player2.spriteB.Width, player2.spriteB.Height), null, Color.White, player2.rotation_b, new Vector2(player2.spriteB.Width / 2, player2.spriteB.Height / 2), SpriteEffects.None, 0);
            s.Draw(player2.spriteT, new Rectangle((int)player2.position.X, (int)player2.position.Y, player2.spriteT.Width, player2.spriteT.Height), null, Color.White, (float)(player2.rotation + .5 * Math.PI), new Vector2(player2.spriteT.Width / 2, player2.spriteT.Height / 2), SpriteEffects.None, 0);
            if (turret1.placed)
                s.Draw(turret1.sprite, new Rectangle((int)turret1.position.X, (int)turret1.position.Y, turret1.sprite.Width, turret1.sprite.Height), null, Color.White, (float)(turret1.rotation + .5 * Math.PI), new Vector2(turret1.sprite.Width / 2, turret1.sprite.Height / 2), SpriteEffects.None, 0);
            if (turret2.placed)
                s.Draw(turret2.sprite, new Rectangle((int)turret2.position.X, (int)turret2.position.Y, turret2.sprite.Width, turret2.sprite.Height), null, Color.White, (float)(turret2.rotation + .5 * Math.PI), new Vector2(turret2.sprite.Width / 2, turret2.sprite.Height / 2), SpriteEffects.None, 0);
        }
        private void DrawInformation(SpriteBatch s)
        {
            player1.narc.Draw(s);
            player2.narc.Draw(s);
            player1.healthBar.Draw(s);
            player2.healthBar.Draw(s);
            s.DrawString(scorefont, "Player 1 Score: " + player1.score.ToString(), new Vector2(this.viewport.Width / 15, this.viewport.Height / 60), new Color(Color.White, (byte)130));
            s.DrawString(scorefont, "Player 2 Score: " + player2.score.ToString(), new Vector2(this.viewport.Width * 12 / 16, this.viewport.Height / 60), new Color(Color.White, (byte)130));
            s.DrawString(scorefont, "Enemies Killed: " + enemies_killed.ToString() + "/" + enemies_level[level], new Vector2(this.viewport.Width * 7 / 16, this.viewport.Height / 60), new Color(Color.Beige, (byte)130));
            //s.Draw(healthBar, new Rectangle(this.viewport.Width / 15, this.viewport.Height / 15, (int)this.viewport.Width * player1.health / 600, this.viewport.Height / 30), Color.Red);
            //s.Draw(healthBar, new Rectangle(this.viewport.Width * 12 / 16, this.viewport.Height / 15, (int)this.viewport.Width * player2.health / 600, this.viewport.Height / 30), Color.Red);

            // Draw lives left for player 1 and 2
            // Milas if you want to make a sprite to draw just replace "healthBar" with the sprite
            // And fudge the drawing math a bit and it will be all set to work
            for (int i = 0; i < player1.healthBar.LivesLeft; i++)
                s.Draw(healthBar, new Rectangle(this.viewport.Width / 15 + ((i * 20)), (this.viewport.Height / 15) + 40, 10, 10), new Color(Color.DarkRed, (byte)200));
            for (int i = 0; i < player2.healthBar.LivesLeft; i++)
                s.Draw(healthBar, new Rectangle(this.viewport.Width * 12 / 16 + ((i * 20)), (this.viewport.Height / 15) + 40, 10, 10), new Color(Color.DarkRed, (byte)200));

            //draw spell menus
            spriteBatch.Draw(player1.spellMenu.State, new Rectangle((int)player1.spellMenu.Position.X, (int)player1.spellMenu.Position.Y, player1.spellMenu.Width, player1.spellMenu.Height), new Color(Color.White, (byte)210));
            spriteBatch.Draw(player2.spellMenu.State, new Rectangle((int)player2.spellMenu.Position.X, (int)player2.spellMenu.Position.Y, player2.spellMenu.Width, player2.spellMenu.Height), new Color(Color.White, (byte)210));
                         
        }
        private void DrawPickups(SpriteBatch s)
        {
            foreach (WeaponPickup pickup in pickups)
            {
                Vector2 glowPos = pickup.position;
                glowPos.X += pickup.sprite.Width / 2;
                glowPos.Y += pickup.sprite.Height / 2;
                pickupGlow.Trigger(glowPos);
                s.Draw(pickup.sprite, pickup.position, Color.White);
            }
        }
        private void DrawBullets(SpriteBatch s)
        {
            //player 1
            foreach (GameObject bullet in bullets)
                if (bullet.alive)
                    s.Draw(bullet.sprite, bullet.position, Color.White);
            //player 2
            foreach (GameObject bullet in bullets2)
                if (bullet.alive)
                    s.Draw(bullet.sprite, bullet.position, Color.White);
            //turret 1
            foreach (GameObject bullet in turretBullets1)
                if (bullet.alive)
                    s.Draw(bullet.sprite, bullet.position, Color.White);
            //turret 2
            foreach (GameObject bullet in turretBullets2)
                if (bullet.alive)
                    s.Draw(bullet.sprite, bullet.position, Color.White);
        }
        private void DrawEnemies(SpriteBatch s)
        {
            foreach (GameObject enemy in enemies)
            {
                if (enemy.alive)
                {
                    spriteBatch.Draw(enemy.sprite, new Rectangle((int)enemy.position.X, (int)enemy.position.Y, enemy.sprite.Width, enemy.sprite.Height), null, Color.White, (float)(enemy.rotation + Math.PI / 2), new Vector2(enemy.sprite.Width / 2, enemy.sprite.Height / 2), SpriteEffects.None, 0);
                    if (enemy.ID == 8)
                        stinkyBug.Trigger(enemy.position);
                }
            }
        }
        private void DrawScore(SpriteBatch s1)
        {
            ArrayList deadScores = new ArrayList(); //Used for determing what scores need to be deleted. 
            //Output Scores
            foreach (ScoreDisplay s in score)
            {
                if (s.Alive)
                {
                    if (s.Time > 0)
                    {
                        if (s.Player == 1)
                            s1.DrawString(scorefont, s.PointVal.ToString(), s.Position, new Color(Color.Red, (byte)(s.Time * 2.5)));
                        else
                            s1.DrawString(scorefont, s.PointVal.ToString(), s.Position, new Color(Color.Green, (byte)(s.Time * 2.5)));
                        s.Time--;
                    }
                    else
                    {
                        s.Alive = false;
                        deadScores.Add(s);
                    }
                }
            }
            //Remove Scores
            foreach (ScoreDisplay s in deadScores)
                score.Remove(s);
        }
        #endregion


        /// <summary>
        /// This is called when the game should draw itself.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Draw(GameTime gameTime)
        {
            if (gameLoading)
            {
                if (!introSoundPlayed)
                {
                    introSound.Play();
                    introSoundPlayed = true;
                }
                GraphicsDevice.Clear(Color.Black);


            spriteBatch.Begin(SpriteBlendMode.AlphaBlend);
            spriteBatch.Draw(logo, new Rectangle(0, 0, (int)viewport.Width, (int)viewport.Height), new Color(Color.White, (byte)(int)progress));
                if (progress <= 255)
                {
                    progress += 1.3f*fade_increment;
                }
                else if (prog2 <= 50)
                {
                    prog2 += 1.3f * fade_increment;
                }
                else
                    gameLoading = false;
                spriteBatch.End();
            }
            else
            {
                if (gm.Active)
                {
                    spriteBatch.Begin(SpriteBlendMode.AlphaBlend);
                    gm.Draw(spriteBatch);
                    spriteBatch.End();
                }
                else if (pm.Active)
                {
                    spriteBatch.Begin(SpriteBlendMode.AlphaBlend);
                    this.DrawBullets(spriteBatch);
                    this.DrawEnemies(spriteBatch);
                    this.DrawInformation(spriteBatch);
                    this.DrawPickups(spriteBatch);
                    this.DrawPlayers(spriteBatch);
                    this.DrawScore(spriteBatch);
                    this.pm.Draw(spriteBatch); 
                    spriteBatch.End();
                }
                // Draw the game over screen which should lead back to the menu
                else
                {
                    if (!act_fade && (gameOver || (enemies_level[level] == enemies_killed)))
                    {
                        ls = new LevelScore(this.level + 1, player1, player2, true, 200, levelfont, titlefont, GraphicsDevice, this.healthBar, this.gameOver);
                        act_fade = true;
                        enemies_killed = 0;
                        level++;
                        if (gameOver)
                        {
                            level = 0;
                            // Need to add more code so that when we go back to the menu
                            // and hit play again, it fades in without the remnants of the last game still on screen
                        }
                    }

                    if (act_fade)
                    {
                        //Fade to Black
                        float elapsed = (float)gameTime.ElapsedGameTime.TotalMilliseconds;
                        spriteBatch.Begin(SpriteBlendMode.AlphaBlend);
                        #region Fade In Logic
                        if (fade_in)
                        {

                            spriteBatch.Draw(healthBar, new Rectangle(0, 0, this.viewport.Width, this.viewport.Height), new Color(Color.DarkBlue, (byte)(int)(current_fade)));
                            if (elapsed >= fade_length / 12)
                            {
                                current_fade += fade_increment;
                            }
                            //Console.Out.WriteLine(current_fade);
                            if (current_fade == 22)
                            {
                                current_fade = 255;
                                //fadeNum = 0;
                                scoreScreen = true;
                                fade_in = false;
                            }
                            //Fade out
                        }
                        #endregion

                        #region Score Screen Logic
                        if (scoreScreen)
                        {
                            //TODO: Add Score Screen Here
                            ls.Draw(this.viewport);
                            if (!GamePad.GetState(PlayerIndex.One).IsConnected)
                            {
                                if (Keyboard.GetState().IsKeyDown(Keys.Enter))
                                {
                                    if (gameOver)
                                    {
                                        gm.Active = true;
                                        gm.Select = true;
                                        scoreScreen = false;
                                        fade_out = true;
                                        player1 = new Player(1, antman_top, antman_bottom, p1_w, new Vector2(viewport.Width * 7 / 15, viewport.Height / 2), 1, new Statistics(true), sMenu, viewport, levelfont, healthBack, healthFrontP1);
                                        player2 = new Player(2, spiderman_top, spiderman_bottom, p2_w, new Vector2(viewport.Width * 8 / 15, viewport.Height / 2), 2, new Statistics(true), sMenu, viewport, levelfont, healthBack, healthFrontP2);
                                        this.gameOver = false;
                                        this.level = 0;
                                    }
                                    else
                                    {
                                        fade_out = true;
                                        scoreScreen = false;
                                    }
                                }
                            }
                            else
                            {
                                if (GamePad.GetState(PlayerIndex.One).Buttons.A == ButtonState.Pressed)
                                {
                                    if (gameOver)
                                    {
                                        gm.Active = true;
                                        gm.Select = true;
                                        scoreScreen = false;
                                        fade_out = true;
                                        player1 = new Player(1, antman_top, antman_bottom, p1_w, new Vector2(viewport.Width * 7 / 15, viewport.Height / 2), 1, new Statistics(true), sMenu, viewport, levelfont, healthBack, healthFrontP1);
                                        player2 = new Player(2, spiderman_top, spiderman_bottom, p2_w, new Vector2(viewport.Width * 8 / 15, viewport.Height / 2), 2, new Statistics(true), sMenu, viewport, levelfont, healthBack, healthFrontP2);
                                        this.gameOver = false;
                                        this.level = 0;
                                    }
                                    else
                                    {
                                        fade_out = true;
                                        scoreScreen = false;
                                    }
                                }
                            }

                            if (!GamePad.GetState(PlayerIndex.Two).IsConnected)
                            {
                                if (Keyboard.GetState().IsKeyDown(Keys.Enter))
                                {
                                    if (gameOver)
                                    {
                                        gm.Active = true;
                                        gm.Select = true;
                                        scoreScreen = false;
                                        fade_out = true;
                                        player1 = new Player(1, antman_top, antman_bottom, p1_w, new Vector2(viewport.Width * 7 / 15, viewport.Height / 2), 1, new Statistics(true), sMenu, viewport, levelfont, healthBack, healthFrontP1);
                                        player2 = new Player(2, spiderman_top, spiderman_bottom, p2_w, new Vector2(viewport.Width * 8 / 15, viewport.Height / 2), 2, new Statistics(true), sMenu, viewport, levelfont, healthBack, healthFrontP2);
                                        this.gameOver = false;
                                        this.level = 0;
                                    }
                                    else
                                    {
                                        fade_out = true;
                                        scoreScreen = false;
                                    }
                                }
                            }
                            else
                            {
                                if (GamePad.GetState(PlayerIndex.Two).Buttons.A == ButtonState.Pressed)
                                {
                                    if (gameOver)
                                    {
                                        gm.Active = true;
                                        gm.Select = true;
                                        scoreScreen = false;
                                        fade_out = true;
                                        player1 = new Player(1, antman_top, antman_bottom, p1_w, new Vector2(viewport.Width * 7 / 15, viewport.Height / 2), 1, new Statistics(true), sMenu, viewport, levelfont, healthBack, healthFrontP1);
                                        player2 = new Player(2, spiderman_top, spiderman_bottom, p2_w, new Vector2(viewport.Width * 8 / 15, viewport.Height / 2), 2, new Statistics(true), sMenu, viewport, levelfont, healthBack, healthFrontP2);
                                        this.gameOver = false;
                                        this.level = 0;
                                    }
                                    else
                                    {
                                        fade_out = true;
                                        scoreScreen = false;
                                    }
                                }
                            }


                        }
                        #endregion

                        #region Fade Back In

                        if (fade_out)
                        {
                            // Reset the pickups for the next level
                            pickups = new ArrayList();

                            spriteBatch.Draw(level_backgrounds[0], new Rectangle(0, 0, viewport.Width, viewport.Height), Color.White);
                            this.DrawPlayers(spriteBatch);
                            this.DrawInformation(spriteBatch);
                            this.DrawPickups(spriteBatch);

                            particleRenderer.RenderEffect(pickupGlow);
                            //draw Get Ready
                            spriteBatch.Draw(getReady, new Rectangle(0, 0, viewport.Width, viewport.Height), new Color(Color.White, (byte)(int)(current_fade)));


                            #region Level Reset
                            current_fade -= 4 * fade_increment;
                            if (current_fade <= 0)
                            {
                                //refresh everything
                                for (int i = 0; i < maxBullets; i++)
                                {
                                    bullets[i].alive = false;
                                    turretBullets1[i].alive = false;
                                    bullets2[i].alive = false;
                                    turretBullets2[i].alive = false;
                                }
                                foreach (GameObject enm in enemies)
                                {
                                    enm.alive = false;
                                }
                                score.Clear();
                                fade_out = false;
                                act_fade = false;
                            }
                            #endregion

                        }
                        #endregion

                        spriteBatch.End();
                    }
                    else
                    {
                        current_fade = 0;
                        fade_in = true;
                        fade_out = false;
                        scoreScreen = false;
                        //GraphicsDevice.Clear(Color.CornflowerBlue);

                        // Begin the sprite batch.
                        spriteBatch.Begin(SpriteBlendMode.AlphaBlend,
                                          SpriteSortMode.Immediate,
                                          SaveStateMode.None);

                        // Set the displacement texture.
                        graphics.GraphicsDevice.Textures[1] = waterfallTexture;

                        // Set an effect parameter to make the
                        // displacement texture scroll in a giant circle.
                        refractionEffect.Parameters["DisplacementScroll"].SetValue(
                                                                    MoveInCircle(gameTime, 0.2f));

                        if (timeEffect.isActive)
                        {
                            // Begin the custom effect.
                            refractionEffect.Begin();
                            refractionEffect.CurrentTechnique.Passes[0].Begin();
                        }
                        // Because the effect will displace the texture coordinates before
                        // sampling the main texture, the coordinates could sometimes go right
                        // off the edges of the texture, which looks ugly. To prevent this, we
                        // adjust our sprite source region to leave a little border around the
                        // edge of the texture. The displacement effect will then just move the
                        // texture coordinates into this border region, without ever hitting
                        // the edge of the texture.

                        //draw background
                        spriteBatch.Draw(level_backgrounds[0], new Rectangle(0, 0, viewport.Width, viewport.Height), Color.White);
                        

                        //Call draw Functions
                        this.DrawPickups(spriteBatch);
                        this.DrawPlayers(spriteBatch);
                        
                        this.DrawEnemies(spriteBatch);
                        this.DrawBullets(spriteBatch);
                        
                        // Draw a green tint if we are poisoned
                        //spriteBatch.Draw()

                        // End the sprite batch, then end our custom effect.
                        

                        if (timeEffect.isActive)
                        {
                            

                            refractionEffect.CurrentTechnique.Passes[0].End();
                            refractionEffect.End();
                        }

                        spriteBatch.End();

                        spriteBatch.Begin(SpriteBlendMode.AlphaBlend);

                        if (timeEffect.isActive)
                        {
                            spriteBatch.Draw(poison, new Rectangle(0, 0, viewport.Width, viewport.Height),new Rectangle(250-timeEffect.length,0,1280+250-timeEffect.length,720), new Color(Color.White, (byte)(60)));
                        }
                        this.DrawInformation(spriteBatch);
                        this.DrawScore(spriteBatch);
                        spriteBatch.End();

                        //render particles
                        particleRenderer.RenderEffect(pickupGlow);
                        particleRenderer.RenderEffect(bloodExplosion);
                        particleRenderer.RenderEffect(bulletHit);
                        particleRenderer.RenderEffect(stinkyBug);
                        
                    }
                    base.Draw(gameTime);
                }
            }
        }


        /// <summary>
        /// Helper for moving a value around in a circle.
        /// </summary>
        static Vector2 MoveInCircle(GameTime gameTime, float speed)
        {
            double time = gameTime.TotalGameTime.TotalSeconds * speed;

            float x = (float)Math.Cos(time);
            float y = (float)Math.Sin(time);

            return new Vector2(x, y);
        }

    }
}
