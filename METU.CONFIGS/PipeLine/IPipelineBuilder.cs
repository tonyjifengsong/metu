using System;

namespace METU.CONFIGS.PipeLine
{
    public interface IPipelineBuilder<TContext>
    {
        IPipelineBuilder<TContext> Use(Func<Action<TContext>, Action<TContext>> middleware);

        Action<TContext> Build();
        IPipelineBuilder<TContext> New();
    }
}
