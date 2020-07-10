using System;
using UnityEngine;

namespace Assets.Scripts.LoadingSystems.Trackings
{
    internal class LoadingTracker : ILoadingTracker
    {
        private readonly AsyncOperation _asyncOperation;

        public bool LoadingIsDone => _asyncOperation.isDone;
        public float Progress => _asyncOperation.progress;

        public LoadingTracker(AsyncOperation asyncOperation)
        {
            _asyncOperation = asyncOperation ?? throw new ArgumentNullException(nameof(asyncOperation));
        }
    }
}