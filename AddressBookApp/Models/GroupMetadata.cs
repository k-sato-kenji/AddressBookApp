using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace AddressBookApp.Models
{
    [MetadataType(typeof(GroupMetadata))]
    public partial class Group
    {

    }
  
    public class GroupMetadata
    {
        [Required]
        [DisplayName("グループ名")]
        public string Name { get; set; }

    }
}