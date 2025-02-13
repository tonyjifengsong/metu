using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace METU.CONFIGS.PipeLine
{
    internal class AsyncPipelineBuilder<TContext> : IAsyncPipelineBuilder<TContext>
    {
        private readonly Func<TContext, Task> _completeFunc;
        private readonly IList<Func<Func<TContext, Task>, Func<TContext, Task>>> _pipelines = new List<Func<Func<TContext, Task>, Func<TContext, Task>>>();

        public AsyncPipelineBuilder(Func<TContext, Task> completeFunc)
        {
            _completeFunc = completeFunc;
        }

        public IAsyncPipelineBuilder<TContext> Use(Func<Func<TContext, Task>, Func<TContext, Task>> middleware)
        {
            _pipelines.Add(middleware);
            return this;
        }

        public Func<TContext, Task> Build()
        {
            var request = _completeFunc;
            foreach (var pipeline in _pipelines.Reverse())
            {
                request = pipeline(request);
            }
            return request;
        }
        public IAsyncPipelineBuilder<TContext> New() => new AsyncPipelineBuilder<TContext>(_completeFunc);
    }
}
