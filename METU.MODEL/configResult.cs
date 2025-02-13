using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace METU.MODEL
{
    public class configResult
    { /// <summary>
      ///Key
      /// </summary>
        [Display(Name = "Key")]
        public string setkey { get; set; }

        /// <summary>
        ///配置名
        /// </summary>
        [Display(Name = "配置名")]
        public string configname { get; set; }

        /// <summary>
        ///配置值
        /// </summary>
        [Display(Name = "配置值")]
        public string configvalue { get; set; }

        /// <summary>
        ///配置说明
        /// </summary>
        [Display(Name = "配置说明")]
        public string configexplain { get; set; }

        /// <summary>
        ///排序
        /// </summary>
        [Display(Name = "排序")]
        public string rank { get; set; }



    }
}
