using System;
using System.Runtime.Serialization;

namespace Database.Model
{
    [DataContract]
    public class AppConfigurationModel : AppBaseModel
    {
        [DataMember]
        public String Lang { get; set; }

        [DataMember]
        public String Client { get; set; }

        #region Constructor
        public AppConfigurationModel()
        {

        }
        #endregion
    }
}
