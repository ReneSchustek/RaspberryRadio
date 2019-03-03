using System;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Runtime.Serialization;

namespace Database.Model
{
    /// <summary>
    /// Basis für alle Tabellen der Datenbank
    /// </summary>
    [DataContract]
    public abstract class AppBaseModel
    {
        #region Id
        //Id des Eintrags
        [Key]
        [Required]
        [DataMember]
        [Display(Name = "Id")]
        [DataType(DataType.Text)]
        public Int32 Id { get; set; }
        #endregion

        #region Standard
        //Erstellt am
        [Required]
        [DataMember]
        [Display(Name = "Created")]
        [DataType(DataType.DateTime)]
        public DateTime CreatedDate { get; set; }

        //Erstellt von
        [Required]
        [DefaultValue(0)]
        [DataMember]
        [Display(Name = "CreatedId")]
        [DataType(DataType.Text)]
        public Int32 CreatedById { get; set; }

        //Geändert am
        [DataMember]
        [Display(Name = "Updated")]
        [DataType(DataType.DateTime)]
        public DateTime? UpdatedDate { get; set; }

        //Geändert von
        [DataMember]
        [Display(Name = "UpdatedId")]
        [DataType(DataType.Text)]
        public Int32? UpdatedById { get; set; }
        #endregion

        #region Constructor
        public AppBaseModel()
        {
            UpdatedById = 0;
            UpdatedDate = DateTime.Now;
        }
        #endregion
    }
}
