using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
namespace AirPiano
{
    public class intelperceptual  : UtilMPipeline
    {


        static public PXCMSession session;
        static public PXCMBase gesture_t;
        static public int error;
        static public pxcmStatus sts;
        static public PXCMGesture gesture;
        static public UtilMCapture capture;
        static public bool device_lost = false;
        static public bool loop;
        static public float clickdis=0.3f;
        private System.Drawing.Bitmap bmp;
  public static float []x,y,z;
       public  intelperceptual():base()
       {init();
       }
        public  void init()
        { 
            x = new float[12]; 
            y = new float[12]; 
            z = new float[12];

            EnableGesture();
            EnableImage(PXCMImage.ColorFormat.COLOR_FORMAT_RGB24);
            EnableImage(PXCMImage.ColorFormat.COLOR_FORMAT_DEPTH);
            device_lost = false;
            loop = true;
            Thread t=new Thread(loopframes);
            t.IsBackground = true;
            t.Start();
	    }

        private void loopframes()
        {
            error = 0;
            if (!LoopFrames())
            {
                Console.WriteLine("Unable to intialise pipline");
                error = 1;
                Main.camerapre = Main.error1;
                Main.Intelperceptual = null;
            }
             try
            {
                if(Main.Intelperceptual!=null&&!device_lost)
                 Dispose();
            }catch(Exception E){ }
            Main.Intelperceptual = null;
        }
	    
        public void dispose()
        { loop=false;
         }

        public override void OnGesture(ref PXCMGesture.Gesture data) 
        {
            if (data.label == PXCMGesture.Gesture.Label.LABEL_POSE_THUMB_DOWN || data.label == PXCMGesture.Gesture.Label.LABEL_NAV_SWIPE_LEFT)
            {
              if(Main.node>0)
                 Main.node--;

            }
            if (data.label == PXCMGesture.Gesture.Label.LABEL_POSE_THUMB_UP || data.label == PXCMGesture.Gesture.Label.LABEL_NAV_SWIPE_RIGHT)
            {
                if (Main.node <4)
                    Main.node++;

            }

            if (data.label == PXCMGesture.Gesture.Label.LABEL_NAV_SWIPE_DOWN)
            {
                if (Main.volume>0)
                    Main.volume-=0.1f;

            }
            if (data.label == PXCMGesture.Gesture.Label.LABEL_NAV_SWIPE_UP)
            {
                if (Main.volume <1)
                    Main.volume += 0.1f;

            }
             


            if (data.label == PXCMGesture.Gesture.Label.LABEL_HAND_CIRCLE)
            {
                Main.quit = true;
            }


		    if (data.active) Console.WriteLine("OnGesture("+data.label+")");
	    }
        
        public override bool OnDisconnect()
        {
            if (!device_lost) Console.WriteLine("Device disconnected");
            device_lost = true;
            Main.camerapre1 = Main.error2;
            Main.Intelperceptual = null;
            loop = false;
        
            return base.OnDisconnect();
        }
        
        public override void OnReconnect()
        {
            Console.WriteLine("Device reconnected");
            Main.camerapre1 = Main.cam;

            device_lost = false;
        }
	    
        public override bool OnNewFrame()
        {
            
            
            PXCMGesture gesture = QueryGesture();
            PXCMGesture.GeoNode fingerpA;
            PXCMGesture.GeoNode fingerpB;
            PXCMGesture.GeoNode fingerpC;
            PXCMGesture.GeoNode fingerpD;
            PXCMGesture.GeoNode fingerpE;

            PXCMGesture.GeoNode fingersA;
            PXCMGesture.GeoNode fingersB;
            PXCMGesture.GeoNode fingersC;
            PXCMGesture.GeoNode fingersD;
            PXCMGesture.GeoNode fingersE;

            PXCMGesture.GeoNode hand1;

            PXCMGesture.GeoNode hand2;
            pxcmStatus [] sts;
            sts = new pxcmStatus[12];
            int i = 0;
            x = new float[12];
            y = new float[12];
            z = new float[12];
            sts[i] = gesture.QueryNodeData(0, PXCMGesture.GeoNode.Label.LABEL_BODY_HAND_PRIMARY, out hand1);
            if (sts[i] >= pxcmStatus.PXCM_STATUS_NO_ERROR)
            {
 
                x[i]=hand1.positionImage.x;
                y[i] = hand1.positionImage.y;
                z[i] = hand1.positionWorld.y;
                if (hand1.confidence < Main.confthre)//< 40)
                {
                    x[i] = -1.0f;
                }
            
            }

            i = 1;
            sts[i] = gesture.QueryNodeData(0, PXCMGesture.GeoNode.Label.LABEL_BODY_HAND_SECONDARY, out hand2);
            if (sts[i] >= pxcmStatus.PXCM_STATUS_NO_ERROR)
            {

                x[i] = hand2.positionImage.x;
                y[i] = hand2.positionImage.y;
                z[i] = hand2.positionWorld.y;
                if (hand2.confidence < Main.confthre)//< 40)
                {
                    x[i] = -1.0f; 
                }

            }

            i = 2;
            sts[i] = gesture.QueryNodeData(0, PXCMGesture.GeoNode.Label.LABEL_BODY_HAND_PRIMARY|PXCMGesture.GeoNode.Label.LABEL_FINGER_INDEX, out fingerpA);
            if (sts[i] >= pxcmStatus.PXCM_STATUS_NO_ERROR)
            {
                

                x[i] = fingerpA.positionImage.x;
                y[i] = fingerpA.positionImage.y;
                z[i] = fingerpA.positionWorld.y;
                if (fingerpA.confidence < Main.confthre)//< 40)
                {
                    x[i] = -1.0f;
                }

            }

            i = 3;
            sts[i] = gesture.QueryNodeData(0, PXCMGesture.GeoNode.Label.LABEL_BODY_HAND_PRIMARY|PXCMGesture.GeoNode.Label.LABEL_FINGER_MIDDLE, out fingerpB);
            if (sts[i] >= pxcmStatus.PXCM_STATUS_NO_ERROR)
            {

                x[i] = fingerpB.positionImage.x;
                y[i] = fingerpB.positionImage.y;
                z[i] = fingerpB.positionWorld.y;
                if (fingerpB.confidence < Main.confthre)//< 40)
                {
                    x[i] =-1.0f;
                }

            }
            i = 4;
            sts[i] = gesture.QueryNodeData(0, PXCMGesture.GeoNode.Label.LABEL_BODY_HAND_PRIMARY | PXCMGesture.GeoNode.Label.LABEL_FINGER_PINKY, out fingerpC);
            if (sts[i] >= pxcmStatus.PXCM_STATUS_NO_ERROR)
            {

                x[i] = fingerpC.positionImage.x;
                y[i] = fingerpC.positionImage.y;
                z[i] = fingerpC.positionWorld.y;

                if (fingerpC.confidence < Main.confthre)//< 40)
                {
                    x[i] =-1.0f;
                }
            }

            i = 5;
            sts[i] = gesture.QueryNodeData(0, PXCMGesture.GeoNode.Label.LABEL_BODY_HAND_PRIMARY | PXCMGesture.GeoNode.Label.LABEL_FINGER_RING, out fingerpD);
            if (sts[i] >= pxcmStatus.PXCM_STATUS_NO_ERROR)
            {

                x[i] = fingerpD.positionImage.x;
                y[i] = fingerpD.positionImage.y;
                z[i] = fingerpD.positionWorld.y;

                if (fingerpD.confidence < Main.confthre)//< 40)
                {
                    x[i] = -1.0f;
                }
            }

            i = 6;
            sts[i] = gesture.QueryNodeData(0, PXCMGesture.GeoNode.Label.LABEL_BODY_HAND_PRIMARY | PXCMGesture.GeoNode.Label.LABEL_FINGER_THUMB, out fingerpE);
            if (sts[i] >= pxcmStatus.PXCM_STATUS_NO_ERROR)
            {

                x[i] = fingerpE.positionImage.x;
                y[i] = fingerpE.positionImage.y;
                z[i] = fingerpE.positionWorld.y;
                if (fingerpE.confidence < Main.confthre)//< 40)
                {
                    x[i] = -1.0f;
                }

            }

            i = 7;
            sts[i] = gesture.QueryNodeData(0, PXCMGesture.GeoNode.Label.LABEL_BODY_HAND_SECONDARY| PXCMGesture.GeoNode.Label.LABEL_FINGER_INDEX, out fingersA);
            if (sts[i] >= pxcmStatus.PXCM_STATUS_NO_ERROR)
            {

                x[i] = fingersA.positionImage.x;
                y[i] = fingersA.positionImage.y;
                z[i] = fingersA.positionWorld.y;
                if (fingersA.confidence < Main.confthre)//< 40)
                {
                    x[i] = -1.0f;
                }

            }

            i = 8;
            sts[i] = gesture.QueryNodeData(0, PXCMGesture.GeoNode.Label.LABEL_BODY_HAND_SECONDARY | PXCMGesture.GeoNode.Label.LABEL_FINGER_MIDDLE, out fingersB);
            if (sts[i] >= pxcmStatus.PXCM_STATUS_NO_ERROR)
            {

                x[i] = fingersB.positionImage.x;
                y[i] = fingersB.positionImage.y;
                z[i] = fingersB.positionWorld.y;
                if (fingersB.confidence < Main.confthre)//< 40)
                {
                    x[i] = -1.0f;
                }

            }

            i = 9;
            sts[i] = gesture.QueryNodeData(0, PXCMGesture.GeoNode.Label.LABEL_BODY_HAND_SECONDARY | PXCMGesture.GeoNode.Label.LABEL_FINGER_PINKY, out fingersC);
            if (sts[i] >= pxcmStatus.PXCM_STATUS_NO_ERROR)
            {

                x[i] = fingersC.positionImage.x;
                y[i] = fingersC.positionImage.y;
                z[i] = fingersC.positionWorld.y;

                if (fingersC.confidence < Main.confthre)//< 40)
                {
                    x[i] = -1.0f;
                }
            }

            i =10;
            sts[i] = gesture.QueryNodeData(0, PXCMGesture.GeoNode.Label.LABEL_BODY_HAND_SECONDARY | PXCMGesture.GeoNode.Label.LABEL_FINGER_RING, out fingersD);
            if (sts[i] >= pxcmStatus.PXCM_STATUS_NO_ERROR)
            {

                x[i] = fingersD.positionImage.x;
                y[i] = fingersD.positionImage.y;
                z[i] = fingersD.positionWorld.y;
                if (fingersD.confidence < Main.confthre)//< 40)
                {
                    x[i] = -1.0f;
                }

            }

            i = 11;
            sts[i] = gesture.QueryNodeData(0, PXCMGesture.GeoNode.Label.LABEL_BODY_HAND_SECONDARY | PXCMGesture.GeoNode.Label.LABEL_FINGER_THUMB, out fingersE);
            if (sts[i] >= pxcmStatus.PXCM_STATUS_NO_ERROR)
            {

                x[i] = fingersE.positionImage.x;
                y[i] = fingersE.positionImage.y;
                z[i] = fingersE.positionWorld.y;
                if (fingersE.confidence < Main.confthre) 
                {
                    x[i] = -1.0f;
                }

            }








             PXCMImage img = QueryImage(PXCMImage.ImageType.IMAGE_TYPE_COLOR);
             if(img!=null)
             img.QueryBitmap(QuerySession(),out bmp);
             if(bmp!=null) 
             Main.camerapre = GetTexture(Main.GrDev,bmp);
             bmp = null; 
             
             img = QueryImage(PXCMImage.ImageType.IMAGE_TYPE_DEPTH);
             if (img != null)
                 img.QueryBitmap(QuerySession(), out bmp);
             if (bmp != null)
                 Main.camerapre1 = GetTexture(Main.GrDev, bmp);

             bmp = null;

             img = QueryImage(PXCMImage.ImageType.IMAGE_TYPE_MASK);
             if (img != null)
                 img.QueryBitmap(QuerySession(), out bmp);
             if (bmp != null)
                 Main.camerapre2 = GetTexture(Main.GrDev, bmp);


            
            
            
            return (loop);


	    }

        private Texture2D GetTexture(GraphicsDevice dev, System.Drawing.Bitmap bmp)
        {
            try
            {
                int[] imgData = new int[bmp.Width * bmp.Height];
                Texture2D texture = new Texture2D(dev, bmp.Width, bmp.Height);

                unsafe
                {
                    // lock bitmap
                    System.Drawing.Imaging.BitmapData origdata =
                        bmp.LockBits(new System.Drawing.Rectangle(0, 0, bmp.Width, bmp.Height), System.Drawing.Imaging.ImageLockMode.ReadOnly, bmp.PixelFormat);

                    uint* byteData = (uint*)origdata.Scan0;

                    // Switch bgra -> rgba
                    for (int i = 0; i < imgData.Length; i++)
                    {
                        byteData[i] = (byteData[i] & 0x000000ff) << 16 | (byteData[i] & 0x0000FF00) | (byteData[i] & 0x00FF0000) >> 16 | (byteData[i] & 0xFF000000);
                    }

                    // copy data
                    System.Runtime.InteropServices.Marshal.Copy(origdata.Scan0, imgData, 0, bmp.Width * bmp.Height);

                    byteData = null;

                    // unlock bitmap
                    bmp.UnlockBits(origdata);
                }

                texture.SetData(imgData);

                return texture;
            }
            catch (Exception e)
            {
                return null;
            }
        }

        
    /*


         
         static public PXCMSession session;
         static public PXCMBase gesture_t;
         static public int error;
         static public pxcmStatus sts;
         static public PXCMGesture gesture;
         static public UtilMCapture capture;
         static public bool device_lost = false;
         static public bool loop;
         static public void init()
         {
              sts = PXCMSession.CreateInstance(out session);

              error = 0;
             if (sts < pxcmStatus.PXCM_STATUS_NO_ERROR)
             {
                 error = 1;
                 Console.WriteLine("Failed to create the SDK session");
                 return;
             }

             // Gesture Module

             sts = session.CreateImpl(PXCMGesture.CUID, out gesture_t);
             if (sts < pxcmStatus.PXCM_STATUS_NO_ERROR)
             {
                 error = 2;
                 Console.WriteLine("Failed to load any gesture recognition module");
                 session.Dispose();
                 return;
             }
              gesture = (PXCMGesture)gesture_t;

             PXCMGesture.ProfileInfo pinfo;
             sts = gesture.QueryProfile(0, out pinfo);
             capture = new UtilMCapture(session);
             sts = capture.LocateStreams(ref pinfo.inputs);
             if (sts < pxcmStatus.PXCM_STATUS_NO_ERROR)
             {
                 error = 3;
                 Console.WriteLine("Failed to locate a capture module");
                 gesture.Dispose();
                 capture.Dispose();
                 session.Dispose();
                 return;
             }
             sts = gesture.SetProfile(ref pinfo);
             sts = gesture.SubscribeGesture(100, OnGesure);
            
             device_lost = false;
             loop = true;
             Thread t = new Thread(onimage);
             t.IsBackground = true;
            
             t.Start();
             
             
         }


         static public void onimage()
           {
             PXCMImage[] images = new PXCMImage[PXCMCapture.VideoStream.STREAM_LIMIT];
             PXCMScheduler.SyncPoint[] sps = new PXCMScheduler.SyncPoint[2];
             
             for (;loop ; )
             {
                 sts = capture.ReadStreamAsync(images, out sps[0]);
                 if (sts < pxcmStatus.PXCM_STATUS_NO_ERROR)
                 {
                     if (sts == pxcmStatus.PXCM_STATUS_DEVICE_LOST)
                     {
                         if (!device_lost) Console.WriteLine("Device disconnected");
                         device_lost = true; 
                         continue;
                     }
                     Console.WriteLine("Device failed\n");
                     break;
                 }
                 if (device_lost)
                 {
                     Console.WriteLine("Device reconnected");
                     device_lost = false;
                 }

                 sts = gesture.ProcessImageAsync(images, out sps[1]);
                 if (sts < pxcmStatus.PXCM_STATUS_NO_ERROR) break;

                 PXCMScheduler.SyncPoint.SynchronizeEx(sps);
                 if (sps[0].Synchronize(0) >= pxcmStatus.PXCM_STATUS_NO_ERROR)
                 {int state=0;
                     PXCMGesture.GeoNode data;
                     sts = gesture.QueryNodeData(0, PXCMGesture.GeoNode.Label.LABEL_BODY_HAND_PRIMARY | PXCMGesture.GeoNode.Label.LABEL_HAND_MIDDLE, out data);
                     if (sts >= pxcmStatus.PXCM_STATUS_NO_ERROR)
                     Console.WriteLine("[node] {0}, {1}, {2}", data.positionImage.x, data.positionImage.y, data.timeStamp);
                     Game.Update1((float)(160-data.positionImage.x)/ 160, 1, 1);
                    if(data.openness>50)
                         state=1;
                     else
                        state=0;

                 // Game.Update((float)Program.game.Width * (160-data.positionImage.x ) / 160, (float)Program.game.Height* (data.positionImage.y - 120) / 120, state, 0);
                 }

                 foreach (PXCMScheduler.SyncPoint s in sps) if (s != null) s.Dispose();
                 foreach (PXCMImage i in images) if (i != null) i.Dispose();
             }
             gesture.Dispose();
             capture.Dispose();
             session.Dispose();

          
         }
         static public void dispose()
         {
             loop = false;
             
         }
         static void OnGesure(ref PXCMGesture.Gesture gesture)
         {
              
         }
    */
    }
}
