namespace METU.CONFIGS.PipeLine
{
    public class TestPipeLine
    {
        public static void Test()
        {
            
            var builder = PipeBuilder.Create<RequestContext>(context =>
            {
              System.Diagnostics.Debug.WriteLine  ($"{context.RequesterName} {context.Hour}h apply failed");
            })
                    .AddWorkStep((context, next) =>
                    {
                        if (context.Hour <= 2)
                        {
                            System.Diagnostics.Debug.WriteLine("pass 1");
                        }
                        else
                        {
                            next();
                        }
                    })
                    .AddWorkStep((context, next) =>
                    {
                        if (context.Hour <= 4)
                        {
                            System.Diagnostics.Debug.WriteLine("pass 2");
                        }
                        else
                        {
                            next();
                        }
                    })
                    .AddWorkStep((context, next) =>
                    {
                        if (context.Hour <= 6)
                        {
                            System.Diagnostics.Debug.WriteLine("pass 3");
                        }
                        else
                        {
                            next();
                        }
                    })
                ;
            var requestPipeline = builder.Build();
            
           
        }
    }
}
