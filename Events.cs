using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Breakout
{
    

    public class UpdateScoreArgs : EventArgs
    {
        public int Points { get; set; }
    }

    public class PaddleMovedArgs : EventArgs
    {
        public int X { get; set; }
    }


    public static class Events
    {
        public static event EventHandler BallLaunched;
        public static event EventHandler<PaddleMovedArgs> PaddleMoved;
        public static event EventHandler<UpdateScoreArgs> UpdateScore;
        public static event EventHandler LoseLife;

        public static void FireBallLaunched(object sender, EventArgs e)
        {
            if (BallLaunched != null) BallLaunched.Invoke(sender, e);
        }

        public static void FirePaddleMoved(object sender, PaddleMovedArgs e)
        {
            if (PaddleMoved != null) PaddleMoved.Invoke(sender, e);
        }

        public static void FireUpdateScore(object sender, UpdateScoreArgs e)
        {
            UpdateScore.Invoke(sender, e);
        }

        public static void FireLoseLife(object sender, EventArgs e)
        {
            LoseLife.Invoke(sender, e);
        }

        public static void AttachToBallLaunch(EventHandler callback)
        {
            BallLaunched += callback;
        }

        public static void DetachFromBallLaunched(EventHandler callback)
        {
            BallLaunched -= callback;
        }

        public static void AttachToPaddleMoved(EventHandler<PaddleMovedArgs> callback)
        {
            PaddleMoved += callback;
        }

        public static void DetachFromPaddleMoved(EventHandler<PaddleMovedArgs> callback)
        {
            PaddleMoved -= callback;
        }

        public static void AttachToUpdateScore(EventHandler<UpdateScoreArgs> callback)
        {
            UpdateScore += callback;
        }

        public static void AttachToLoseLife(EventHandler callback)
        {
            LoseLife += callback;
        }
    }
}
