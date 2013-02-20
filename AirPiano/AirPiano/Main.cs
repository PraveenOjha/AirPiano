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

namespace AirPiano
{
    /// <summary>
    /// This is the main type for your game
    /// </summary>
    public class Main : Microsoft.Xna.Framework.Game
    {
          
        GraphicsDeviceManager graphics;
        SpriteBatch spriteBatch;
        public static intelperceptual Intelperceptual;
        public static Texture2D camerapre;
        public static Microsoft.Xna.Framework.Graphics.GraphicsDevice GrDev;
        public static Texture2D camerapre1;
        public static Texture2D camerapre2;
        public static Texture2D top;
        public static Texture2D side;
        public static Texture2D white1;
        public static Texture2D white2;
        public static Texture2D black1;
        public static Texture2D black2;
        public static Texture2D ripple,error1,error2,cam;
        public static SpriteFont font;
               
        Rectangle[] placholders;
        Vector3 [] presspoints;
        
        private int width;
        private int height;
        enum BS {Nostate,Pressed,Open }
        BS [] black,blackn;
        BS[] white,whiten;
        SoundEffect[] soundsfiles;
        SoundEffectInstance[] sounds;
        public static int node = 0;
        static public  float volume = 1;
        public static bool quit;
        string[] set = { "C#", "D#", "F#", "G#", "A#" };
        string[] set1 = { "c", "d","e", "f", "g", "a" ,"b"};
        private float mindis=0.12f;
        private float  maxdis=0.4f;
        public static uint confthre=45;
        
        public Main()
        {
            graphics = new GraphicsDeviceManager(this);


            width= this.graphics.PreferredBackBufferWidth = 1366;
            height=this.graphics.PreferredBackBufferHeight = 768;
           this.graphics.IsFullScreen = true;
            Content.RootDirectory = "Content";
            black = new BS[10];
            white = new BS[14];
            blackn = new BS[10];
            whiten = new BS[14];
            soundsfiles = new SoundEffect[72];
            sounds = new SoundEffectInstance[72];
            placholders = new Rectangle[24];
            int i = 0;
            quit = false;
            presspoints=new Vector3[10];
             
        }

        /// <summary>
        /// Allows the game to perform any initialization it needs to before starting to run.
        /// This is where it can query for any required services and load any non-graphic
        /// related content.  Calling base.Initialize will enumerate through any components
        /// and initialize them as well.
        /// </summary>
        protected override void Initialize()
        {
            // TODO: Add your initialization logic here

            base.Initialize();
            GrDev = GraphicsDevice;
        }

        /// <summary>
        /// LoadContent will be called once per game and is the place to load
        /// all of your content.
        /// </summary>
        protected override void LoadContent()
        {
            // Create a new SpriteBatch, which can be used to draw textures.
            spriteBatch = new SpriteBatch(GraphicsDevice);

            black1 = Content.Load < Texture2D>("b1");
            black2 = Content.Load<Texture2D>("b2");
            white1 = Content.Load<Texture2D>("w1");
            white2 = Content.Load<Texture2D>("w2");
            top = Content.Load<Texture2D>("top");
            side= Content.Load<Texture2D>("bridge");
            ripple = Content.Load<Texture2D>("ripple");
            cam = Content.Load<Texture2D>("cam");
            camerapre = Content.Load<Texture2D>("cam");
            error1= Content.Load<Texture2D>("camnd");
            error2 = Content.Load<Texture2D>("discon");
            font = Content.Load<SpriteFont>("scon");
            
            
            int i = 0;
            while (i < 6)
            {
                soundsfiles[i * 12] = Content.Load<SoundEffect>("Sound/C" + i);
                soundsfiles[i * 12 + 1] = Content.Load<SoundEffect>("Sound/D" + i);
                soundsfiles[i * 12 + 2] = Content.Load<SoundEffect>("Sound/E" + i);
                soundsfiles[i * 12 + 3] = Content.Load<SoundEffect>("Sound/F" + i);
                soundsfiles[i * 12 + 4] = Content.Load<SoundEffect>("Sound/G" + i);
                soundsfiles[i * 12 + 5] = Content.Load<SoundEffect>("Sound/A" + i);
                soundsfiles[i * 12 + 6] = Content.Load<SoundEffect>("Sound/B" + i);
                soundsfiles[i * 12 + 7] = Content.Load<SoundEffect>("Sound/C" + i + "SHARP");
                soundsfiles[i * 12 + 8] = Content.Load<SoundEffect>("Sound/D" + i + "SHARP");
                soundsfiles[i * 12 + 9] = Content.Load<SoundEffect>("Sound/F" + i + "SHARP");
                soundsfiles[i * 12 + 10] = Content.Load<SoundEffect>("Sound/G" + i + "SHARP");
                soundsfiles[i * 12 + 11] = Content.Load<SoundEffect>("Sound/A" + i + "SHARP"); 
                i++;

            }
            i = 0;
            while (i < 72)
            {                
                sounds[i]=soundsfiles[i].CreateInstance();
                i++;
            }


            // TODO: use this.Content to load your game content here
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
            // Allows the game to exit
            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed || Keyboard.GetState().IsKeyDown(Keys.Escape) || quit)
            {
                quit = true;
                if (Intelperceptual != null)
                    Intelperceptual.dispose();
                else
                    this.Exit();
            }   
          // TODO: Add your update logic here
            Updateoldkeys();
          base.Update(gameTime);
        }

        private void Updateoldkeys()
        {
            int i = 0; bool update = true;
            while(i<24)
            {
                update = true;
                int k = 0;
                while (k < 10)
                {
                    if (placholders[i].Contains((int)presspoints[k].X, (int)presspoints[k].Y)&&(presspoints[k].Z>mindis && presspoints[k].Z<maxdis))
                    {
                        update = false;
                        break;
                    }
                   
                    
                    k++;

                }

                if (update)
                {
                    if (i < 14)
                        white[i] = BS.Open;
                    else
                        black[i - 14] = BS.Open;
                }
                i++;
            }
        }

    /*(.    private void checkandplay()
        {
            int i = 0;
            whiten=new BS[14];
            blackn = new BS[10];
            while (i < 10)
            {
                if (presspoints[i].X <= 0 )
                {i++;
                 continue;
                }
                float presspointnear =  presspoints[i].X/(width/14);

                double a=Math.Ceiling(presspointnear);
                double b = Math.Floor(presspointnear);
              
              bool but1, but2, but3;
              but1 = but2 =but3= false;
                  
              but1= placholders[(int)a].Contains((int)presspoints[i].X,(int)presspoints[i].Y);
              but2= placholders[(int)b].Contains((int)presspoints[i].X, (int)presspoints[i].Y);
              
                
                int k=0;
                if (b > 2)
                     k++;
                if (b > 5)
                    k++;
                if (b > 7)
                    k++;
             

               if(b<13)
                   but3 = placholders[(int)(b - k) + 14].Contains((int)presspoints[i].X , (int)presspoints[i].Y);

               if (but3)
               {
                   but1 = but2 = false;
                   playnode((int)b - k + 14);
                   blackn[(int)b - k] = BS.Pressed;
                  
               }

               if (but1)
               {
                   playnode((int)a);
                   whiten[(int)a]=BS.Pressed;
               }
               if (but2)
               {
                   playnode((int)b);
                   whiten[(int)b]=BS.Pressed;
               
               }
               i++; but1 = but2 = but3 = false;
            }
       
       
        }*/
        private void playnode(int p)
        {

            if (p < 14)
            {
               if (white[p] == BS.Pressed)
                   return;

            }
            if (p >= 14)
            {
                if(black[p - 14] == BS.Pressed)
                    return;

            }

              
                int i = 0;
                int k = 0;
                if (p <=6)
                {
                    i = 0;
                    k = p;
                }
                if (p >6 && p <= 13)
                {
                    k = p - 7;
                    i = 1;
                }
                if (p >=14 && p < 19)
                {
                    k = 7+p - 14;
                    i = 0;
                }
                if (p >=19 && p < 24)
                {
                    k = 7+p - 19;
                    i = 1;
                }
                sounds[k* (node + 1+i)].Volume = volume;

                if (sounds[k* (node + 1+i)].State == SoundState.Playing)
                    return;
                
                sounds[k * (node + 1+i)].Play();


                if (p < 14)
                {
                    white[p] = BS.Pressed;

                }
                if (p >= 14)
                {
                    black[p - 14] = BS.Pressed;

                }

        
        }


        void stopnode(int p)
        {    int i=0;
            
                int k = 0;
                if (p <= 6)
                {
                    i = 0;
                    k = p;
                }
                if (p > 6 && p <= 13)
                {
                    k = p - 7;
                    i = 1;
                }
                if (p >= 14 && p < 19)
                {
                    k = 7 + p - 14;
                    i = 0;
                }
                if (p >= 19 && p < 24)
                {
                    k = 7 + p - 19;
                    i = 1;
                }

                if (sounds[k * (node + 1 + i)].State == SoundState.Playing)
                {
                    if (p < 14)
                    { if(whiten[p]!=BS.Pressed)
                      {white[p]=BS.Open;
                     // sounds[k * (node + 1 + i)].Stop();
             
                      }
                    }
                    if (p > 14)
                    {if( blackn[(p-14)]!=BS.Pressed)
                     {  black[p-14] = BS.Open;
                //     sounds[k * (node + 1 + i)].Stop();
             
                     }
                    }
                }
                else if (sounds[k * (node + 1 + i)].State == SoundState.Stopped)
                { 
                 if (p < 14)
                    {  white[p]=BS.Open;
                      }
                    if (p > 14)
                    { black[p-14] = BS.Open;
                     }
                  }
        
        }
        /// <summary>
        /// This is called when the game should draw itself.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Draw(GameTime gameTime)
        {
            if (Intelperceptual == null)
             Intelperceptual = new intelperceptual();

            GraphicsDevice.Clear(Color.CornflowerBlue);
            spriteBatch.Begin();
            drawkey(-1);

            int k = 0;
            while (k < 14)
            {
                if( white[k]==BS.Pressed)
                    drawkey(k);

                k++;
 
            }
            k = 0;
            drawblack(-1);
            while (k < 10)
            {
                if (black[k] == BS.Pressed)
                    drawblack(k);

                k++;

            }
            k = 0;
           
            drawbg();
            drawovericons();

            drawtouch();
           
            spriteBatch.End();
            // TODO: Add your drawing code here

            base.Draw(gameTime);
        }

        private void drawtouch()
        {
            int i = 2; int px = 10; int py = 10;
            while (i < 12)
            {
                presspoints[i - 2].Z = intelperceptual.z[i];
              
                if (!(intelperceptual.x[i] == 0 && intelperceptual.y[i] == 0))
                {
                    px = py =(int) ((1 - intelperceptual.z[i]) * 20);
                    spriteBatch.Draw(ripple, new Rectangle((int)(intelperceptual.x[i]*1.2f) - px, (int)
                        (intelperceptual.y[i]*1.2f )- py, 2*px, 2*py), Color.Aqua);
                 
                    if (intelperceptual.z[i] <mindis)
                    {
                        spriteBatch.Draw(ripple, new Rectangle((int)(width * (1 - intelperceptual.x[i] / 315)) - 25, (int)(intelperceptual.y[i] / 234 * (height - height / 2.5)) - 25 + (int)(height / 2.5f), 50, 50), Color.White);
                        setkey(presspoints[i - 2].X ,presspoints[i - 2].Y ,BS.Open); 
                        presspoints[i - 2].X = -20;
                    }
                    
                    
                    else if (intelperceptual.z[i] > maxdis)
                    {spriteBatch.Draw(ripple, new Rectangle((int)(width * (1 - intelperceptual.x[i] / 315)) - 25, (int)(intelperceptual.y[i] / 234 * (height - height / 2.5)) - 25 + (int)(height / 2.5f), 50, 50), Color.White);
                     setkey(presspoints[i - 2].X ,presspoints[i - 2].Y ,BS.Open); 
                     presspoints[i - 2].X = -20;
                    }
                    else
                    {
                        setkeydown(i - 2, (int)(width * (1 - intelperceptual.x[i] / 315)), (int)(intelperceptual.y[i] / 234 * (height - height / 2.5)) - 25 + (int)(height / 2.5f));
                        presspoints[i - 2].X = (int)(width * (1 - intelperceptual.x[i] / 315));
                        presspoints[i - 2].Y = (int)(intelperceptual.y[i] / 234 * (height - height / 2.5)) - 25 + (int)(height / 2.5f);
                        spriteBatch.Draw(ripple, new Rectangle((int)(width * (1 - intelperceptual.x[i] / 315)) - 25, (int)(intelperceptual.y[i] / 234 * (height - height / 2.5)) - 25 + (int)(height / 2.5f), 50, 50), Color.Red);
                          }
                }

           i++;
            }
        }

        private void setkey(float X, float Y, BS bS)
        {
           
            float presspointnear = X / (width / 14);
            double a = Math.Ceiling(presspointnear);
            double b = Math.Floor(presspointnear);

            bool but1, but2, but3;
            but1 = but2 = but3 = false;
            if (a < 0 || b < 0)
            { a = b = 0; }
            but1 = placholders[(int)a].Contains((int) X, (int) Y);
            but2 = placholders[(int)b].Contains((int) X, (int) Y);


            int k = 0;        if (b > 2)          k++;     if (b > 5)              k++;           if (b > 7)              k++;


            if (b < 13)     but3 = placholders[(int)(b - k) + 14].Contains((int)X,(int)Y);

            if (but3)
            {
                but1 = but2 = false;
               stopnode((int)b-k+14);
            }

            if (but1)
            {
                stopnode((int)a);
            
            }
            if (but2)
            {
                stopnode((int)b);
            
            }

           
        }

        private void setkeydown(int i,float X,float Y)
        {

            setfreshstate(X, Y, presspoints[i].X, presspoints[i].Y); 

            float presspointnear = X / (width / 14);
            double a = Math.Ceiling(presspointnear);
            double b = Math.Floor(presspointnear);
            if (a < 0 || b < 0)
            { a = b = 0; }
            bool but1, but2, but3;
            but1 = but2 = but3 = false;

            but1 = placholders[(int)a].Contains((int)X, (int)Y);
            but2 = placholders[(int)b].Contains((int)X, (int)Y);


            int k = 0; if (b > 2) k++; if (b > 5) k++; if (b > 7) k++;


            if (b < 13) but3 = placholders[(int)(b - k) + 14].Contains((int)X, (int)Y);

            if (but3)
            {
                but1 = but2 = false;
                if(black[(int)b - k] != BS.Pressed)
                playnode((int)b - k + 14);
            }

            if (but1)
            {
                if(white[(int)a]!= BS.Pressed)
                playnode((int)a);

            }
            if (but2)
            {
                if(white[(int)b] != BS.Pressed)
                playnode((int)b);

            }

           
            
        }

        private void setfreshstate(float X, float Y, float OX, float OY)
        {


            float presspointnear = X / (width / 14);
            double a = Math.Ceiling(presspointnear);
            double b = Math.Floor(presspointnear);

            bool but1, but2, but3,old1,old2,old3;
            but1 = but2 = but3 = false;
            old1 = old2 = old3 = false;
            if (a < 0 || b < 0)
            { a = b = 0; }
            but1 = placholders[(int)a].Contains((int)X, (int)Y);
            but2 = placholders[(int)b].Contains((int)X, (int)Y);


            int k = 0; if (b > 2) k++; if (b > 5) k++; if (b > 7) k++;


            if (b < 13) but3 = placholders[(int)(b - k) + 14].Contains((int)X, (int)Y);


            presspointnear = OX / (width / 14);
            a = Math.Ceiling(presspointnear);
            b = Math.Floor(presspointnear);

            if (a < 0 || b < 0)
            { a = b = 0; }
            old1 = placholders[(int)a].Contains((int)X, (int)Y);
            old2 = placholders[(int)b].Contains((int)X, (int)Y);


           k = 0; if (b > 2) k++; if (b > 5) k++; if (b > 7) k++;


            if (b < 13) old3 = placholders[(int)(b - k) + 14].Contains((int)OX, (int)OY);

            if (old1)
            {
                if(!but1)
                    stopnode((int)a);
            }
            if(old2)
            {if(!but2)
            stopnode((int)b);
            
            }
            if (old3)
            {if(!but3)
                stopnode((int)b-k+14);
            }

        }

        private void drawovericons()
        {if(camerapre!=null)
            spriteBatch.Draw(camerapre, new Rectangle(0,0,384,284), Color.White);
            
        }

        private void drawkey(int a)
        {
            int i = 0;
            while (i < 14)
            {

                if (placholders[i].Height==0)
                {placholders[i]= new Rectangle(i * width / 14, (int)(height / 2.5f), (int)width / 14, height - (int)(height / 2.5f));
                }


                if(a==-1)
                    spriteBatch.Draw(white1, placholders[i], Color.White);
                else if(a==i)
                spriteBatch.Draw(white2, placholders[i], Color.White);
                spriteBatch.DrawString(font, set1[i % 7] + (int)(node + i / 7), new Vector2(placholders[i ].X + placholders[i ].Width / 2 - 20, placholders[i ].Y + 250), Color.Black);
            
                i++;
            }
        }


        private void drawblack(int a)
        {
            int i = 0;
            int wd = width /14;
            int wid = (int)(wd*.65f); 
            int hite=(int)(0.65*height / 2.5f);
            while (i < 10)
            {

                if (placholders[i+14].Height == 0)
                {
                    placholders[i+14] = new Rectangle(wd - wid / 2, (int)(height / 2.5f), wid, hite);
                }

                if (a == -1)
                    spriteBatch.Draw(black1, placholders[i + 14], Color.White);
                else if (a == i)
                spriteBatch.Draw(black2, placholders[i+14], Color.White);

                spriteBatch.DrawString(font,set[i%5]+(int)(node+i/5),new Vector2(placholders[i+14].X+placholders[i+14].Width/2-20,placholders[i+14].Y+50),Color.White);
                i++;
                wd += width /14;
                if (i ==2||i==5||i==7)
                {
                    wd += width / 14;
                }
            }
        }
        

        private void drawbg()
        {
            spriteBatch.Draw(top, new Rectangle(0,0,width,(int)(height/2.5f)), Color.White);
            spriteBatch.DrawString(font, "Volume = " +(int)(volume*100)+"%", new Vector2(600,70), Color.Black);
            spriteBatch.DrawString(font, "Starting C Note = " + (int)(node) , new Vector2(890, 70), Color.Black);

            spriteBatch.DrawString(font, "Swipe Up/Down to control Volume ", new Vector2(550, 120), Color.Tan);
            spriteBatch.DrawString(font, "Swipe Left/Right to change starting C note", new Vector2(550, 170), Color.WhiteSmoke);
            spriteBatch.DrawString(font, "Move Hand in Circlar motion or press Esc to quit", new Vector2(550, 220), Color.AliceBlue);
           
        }


        

    }
}
