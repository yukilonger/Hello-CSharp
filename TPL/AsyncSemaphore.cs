﻿using System;
namespace Task_
{
	public class AsyncSemaphore
	{
        private class SemaphoreReleaser : IDisposable
        {
            private SemaphoreSlim _semaphore;

            public SemaphoreReleaser(SemaphoreSlim semaphore) =>
              _semaphore = semaphore;

            public void Dispose() => _semaphore.Release();
        }

        private SemaphoreSlim _semaphore;
        public AsyncSemaphore() =>
          _semaphore = new SemaphoreSlim(1);

        public async Task<IDisposable> WaitAsync()
        {
            await _semaphore.WaitAsync();
            return new SemaphoreReleaser(_semaphore) as IDisposable;
        }
	}
}

