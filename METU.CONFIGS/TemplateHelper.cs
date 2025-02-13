using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static System.Net.Mime.MediaTypeNames;

namespace METU.CONFIGS
{

    public static class TemplateHelper
    {
        /// <summary>
        /// 加载HTTP服务器路径XML模板文件内容
        /// </summary>
        /// <param name="XMLTemplateName">XML模板文件名称</param>
        /// <returns></returns>
        public static string LoadHttpTemplate(string XMLTemplateName)
        {
            string path = XMLTemplateName;
            if (File.Exists(path))
            {
                StreamReader fs = File.OpenText(path);

                var content = fs.ReadToEnd().ToString();
                fs.Close();

                return content;
            }
            else
            {
                return "文件不存在！";
            }
        }

        /// <summary>
        /// 加载本地路径XML模板文件内容
        /// </summary>
        /// <param name="XMLTemplateName">XML模板文件名称</param>
        /// <returns></returns>
        public static string LoadTemplate(string XMLTemplateName)
        {
            string path =  XMLTemplateName;
            if (File.Exists(path))
            {
                StreamReader fs = File.OpenText(path);

                var content = fs.ReadToEnd().ToString();
                fs.Close();

                return content;
            }
            else
            {
                return "";
            }
        }

    }
}
