 

namespace Quartz
{
    /// <summary>
    /// 
    /// </summary>
    public interface ITonyJob : IJob
    {
        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        string GetTimerConfig();

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        int GetTimerFrequent();
    }
}
