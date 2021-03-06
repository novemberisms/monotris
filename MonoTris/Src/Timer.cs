﻿using System;
using Microsoft.Xna.Framework;

namespace MonoTris
{
    public class Timer
    {
        public double TimeOut { get; private set; }
        public double TimeLeft { get; private set; }
        public bool IsRepeat { get; private set; } = true;
        public bool IsRunning { get; private set; } = true;
        public double Period
        {
            get => 1.0 / TimeOut;
            set => TimeOut = 1.0 / value;
        }

        public event Action OnTimeOut = delegate () { };

        public Timer(double timeOut, bool isRepeat)
        {
            TimeOut = timeOut;
            IsRepeat = isRepeat;

            Start();
        }

        public void Start()
        {
            TimeLeft = TimeOut;
            IsRunning = true;
        }

        public void Pause()
        {
            IsRunning = false;
        }

        public void Stop()
        {
            TimeLeft = TimeOut;
            IsRunning = false;
        }

        public void Update(GameTime gameTime)
        {
            if (!IsRunning) return;

            TimeLeft -= gameTime.ElapsedGameTime.TotalSeconds;
        
            if (TimeLeft > 0) return;

            OnTimeOut();

            if (IsRepeat)
            {
                TimeLeft = TimeOut;
            }
            else
            {
                TimeLeft = 0;
                IsRunning = false;
            }

        }

    }
}
