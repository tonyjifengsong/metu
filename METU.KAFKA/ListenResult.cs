using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace METU.KAFKA
{
    public class ListenResult : IDisposable
    {
        CancellationTokenSource cancellationTokenSource;

        /// <summary>
        /// CancellationToken
        /// </summary>
        public CancellationToken Token { get { return cancellationTokenSource.Token; } }
        /// <summary>
        /// 是否已停止
        /// </summary>
        public bool Stoped { get { return cancellationTokenSource.IsCancellationRequested; } }

        public ListenResult()
        {
            cancellationTokenSource = new CancellationTokenSource();
        }

        /// <summary>
        /// 停止监听
        /// </summary>
        public void Stop()
        {
            cancellationTokenSource.Cancel();
        }

        public void Dispose()
        {
            Stop();
        }
    }
}
