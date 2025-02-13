using System;
using System.Threading.Tasks;

namespace METU.CONFIGS.PipeLine
{
    public static class PipeBuilder
    {
        public static IPipelineBuilder<TContext> Create<TContext>()
        {
            return new PipelineBuilder<TContext>(c => { });
        }

        public static IPipelineBuilder<TContext> Create<TContext>(Action<TContext> completeAction)
        {
            return new PipelineBuilder<TContext>(completeAction);
        }

        public static IAsyncPipelineBuilder<TContext> CreateAsync<TContext>()
        {
            return new AsyncPipelineBuilder<TContext>(c => Task.CompletedTask);
        }

        public static IAsyncPipelineBuilder<TContext> CreateAsync<TContext>(Func<TContext, Task> completeFunc)
        {
            return new AsyncPipelineBuilder<TContext>(completeFunc);
        }
    }
}
