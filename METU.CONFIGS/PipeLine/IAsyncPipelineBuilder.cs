using System;
using System.Threading.Tasks;

namespace METU.CONFIGS.PipeLine
{
    public interface IAsyncPipelineBuilder<TContext>
    {
        IAsyncPipelineBuilder<TContext> Use(Func<Func<TContext, Task>, Func<TContext, Task>> middleware);

        Func<TContext, Task> Build();
        IAsyncPipelineBuilder<TContext> New();
    }
}
