using System;
using System.Threading.Tasks;

namespace METU.CONFIGS.PipeLine
{
    public static class PipelineBuilderExtention
    {
     
        public static IPipelineBuilder<TContext> AddWorkStep<TContext>(this IPipelineBuilder<TContext> builder, Action<TContext, Action> action)

        {//Use(Func<Action<TContext>, Action<TContext>> middleware);
            return builder.Use(next =>
                               context =>
                               {
                                   action(context, () => next(context));
                               });
        }
       
        public static IAsyncPipelineBuilder<TContext> AddWorkStep<TContext>(this IAsyncPipelineBuilder<TContext> builder, Func<TContext, Func<Task>, Task> func)
        {
            return builder.Use(next =>
                               context =>
                               {
                                   return func(context, () => next(context));
                               });
        }

        #region IPipelineBuilder

       

        public static IPipelineBuilder<TContext> AddWorkStep<TContext>(this IPipelineBuilder<TContext> builder, Action<TContext, Action<TContext>> action)
        {
            return builder.Use(next =>
                context =>
                {
                    action(context, next);
                });
        }

        public static IPipelineBuilder<TContext> Run<TContext>(this IPipelineBuilder<TContext> builder, Action<TContext> handler)
        {
            return builder.Use(_ => handler);
        }

        public static IPipelineBuilder<TContext> When<TContext>(this IPipelineBuilder<TContext> builder, Func<TContext, bool> predict, Action<IPipelineBuilder<TContext>> configureAction)
        {
            builder.AddWorkStep((context, next) =>
            {
                if (predict.Invoke(context))
                {
                    var branchPipelineBuilder = builder.New();
                    configureAction(branchPipelineBuilder);
                    var branchPipeline = branchPipelineBuilder.Build();
                    branchPipeline.Invoke(context);
                }
                else
                {
                    next();
                }
            });

            return builder;
        }

        #endregion IPipelineBuilder

        #region IAsyncPipelineBuilder

       
        public static IAsyncPipelineBuilder<TContext> AddWorkStep<TContext>(this IAsyncPipelineBuilder<TContext> builder,
            Func<TContext, Func<TContext, Task>, Task> func)
        {
            return builder.Use(next =>
                context => func(context, next));
        }

        public static IAsyncPipelineBuilder<TContext> When<TContext>(this IAsyncPipelineBuilder<TContext> builder, Func<TContext, bool> predict, Action<IAsyncPipelineBuilder<TContext>> configureAction)
        {
            builder.AddWorkStep((context, next) =>
            {
                if (predict.Invoke(context))
                {
                    var branchPipelineBuilder = builder.New();
                    configureAction(branchPipelineBuilder);
                    var branchPipeline = branchPipelineBuilder.Build();
                    return branchPipeline.Invoke(context);
                }

                return next();
            });

            return builder;
        }

        public static IAsyncPipelineBuilder<TContext> Run<TContext>(this IAsyncPipelineBuilder<TContext> builder, Func<TContext, Task> handler)
        {
            return builder.Use(_ => handler);
        }

        #endregion IAsyncPipelineBuilder
    }
}
