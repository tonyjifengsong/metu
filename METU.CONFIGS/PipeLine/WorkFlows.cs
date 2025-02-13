using Newtonsoft.Json;
using System;

namespace METU.CONFIGS.PipeLine
{
    public class WorkFlows 
    {
      public static  Action<METUContext>? WorkSteps;
        public static Action<METUContext, object[]>? WorkStep;
        public static void DoWork(METUContext requestContext)
        {
            var builder = PipeBuilder.Create<METUContext>(context =>
            {
                System.Diagnostics.Debug.WriteLine("context:" + JsonConvert.SerializeObject(context));
                  System.Diagnostics.Debug.WriteLine($"{context.Name} -{context.TraceID}");
            });

 foreach (var itm in WorkSteps.GetInvocationList())
                           {
                builder.AddWorkStep((context, next) =>
                           {                               
                               itm.DynamicInvoke(context);
                               next();
                           });
                        }
            var requestPipeline = builder.Build();
               requestPipeline.Invoke(requestContext);
          
        }
        public static void DoWork(METUContext requestContext,params object[] options)
        {
            var builder = PipeBuilder.Create<METUContext>(context =>
            {
                System.Diagnostics.Debug.WriteLine($"{context.Name} -{context.TraceID}h apply failed");
            });

            foreach (var itm in WorkSteps.GetInvocationList())
            {
                builder.AddWorkStep((context, next) =>
                {

                    itm.DynamicInvoke(context);

                    next();

                });
            }
            var requestPipeline = builder.Build();
                requestPipeline.Invoke(requestContext);
           
        }

    }
    public class WorkFlows<T> where T :  class
    {
        public static Action<T>?  WorkSteps;
        public static Action<T,object[] >?  WorkStep;
        public static void DoWork(T requestContext)
        {
            var builder = PipeBuilder.Create<T>(context =>
            {
                System.Diagnostics.Debug.WriteLine("context:");
                System.Diagnostics.Debug.WriteLine( "Initial Start!");
            });
            if (WorkSteps != null)
            {
                foreach (var itm in WorkSteps.GetInvocationList())
                {
                    builder.AddWorkStep((context, next) =>
                    {

                        itm.DynamicInvoke(context);

                        next();

                    });
                }
            }
            var requestPipeline = builder.Build();
            
            requestPipeline.Invoke(requestContext);
        }
        public static void DoWork(T requestContext, params object[] options)
        {
            var builder = PipeBuilder.Create<T>(context =>
            {
                System.Diagnostics.Debug.WriteLine("context:");
                System.Diagnostics.Debug.WriteLine("Initial Start!");
            });
            if (WorkSteps != null)
            {
                foreach (var itm in WorkSteps.GetInvocationList())
                {
                    builder.AddWorkStep((context, next) =>
                    {

                        itm.DynamicInvoke(context);

                        next();

                    });
                }
            }
            var requestPipeline = builder.Build();
          
            requestPipeline.Invoke(requestContext);
        }
    }
}
