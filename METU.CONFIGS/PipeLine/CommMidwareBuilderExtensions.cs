using Microsoft.AspNetCore.Builder;

namespace METU.CONFIGS.PipeLine
{
    public  static class CommMidwareBuilderExtensions
    {    //
        // 摘要:
        //     Register the Swagger middleware with provided options
        public static IApplicationBuilder UseCommon(this IApplicationBuilder app, params object[] options )
        {
            System.Diagnostics.Debug.WriteLine("UseCommon");
            return app.UseMiddleware<CommonMiddleware>(new object[] {options});
        }



    }
}
