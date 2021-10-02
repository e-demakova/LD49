using System;

using Deblue.Models;
using Deblue.ObservingSystem;

namespace Deblue.Achivments
{
    public class AchivmentModel : IModel
    {
        private static Handler<AchivmentUnlock> _handler;

        public static void SubscribeOnAchivmentUnlock(Action<AchivmentUnlock> action)
        {
            _handler.Subscribe(action);
        }

        public static void UnsubscribeOnAchivmentUnlock(Action<AchivmentUnlock> action)
        {
            _handler.Subscribe(action);
        }

        public void DeInit()
        {
            _handler.Clear();
        }
    }
}
