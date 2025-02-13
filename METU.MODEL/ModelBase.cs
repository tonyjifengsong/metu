
using METU.INTERFACE.ICore;
using System;
using System.ComponentModel;

namespace METU.MODEL
{
    public   class ModelBase:IEntityBase
    {
        [DisplayName("唯一ID")]
        public string uniid
        {
            get; set;
        }

        [DisplayName("创建日期")]
        public DateTime CreateDate
        {
            get;

            set;
        }
        [DisplayName("创建用户ID")]
        public string CreateUserID
        {
            get;

            set;
        }
        [DisplayName("GUIDKEY")]
        public string GuidKey
        {
            get;

            set;
        }
        [DisplayName("ID")]
        public long ID
        {
            get;

            set;
        }
         public string Name
        {
            get;

            set;
        }

        [DisplayName("版本号")]
        public string Versions
        {
            get;

            set;
        }
      
    }
}
