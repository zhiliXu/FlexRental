using System;
using System.ComponentModel.DataAnnotations;
using System.Web;

/**
 * This is the model deal with data used by buyer
 * @Author Zhili Xu
 * */

namespace FlexRental.Models
{
    public class ListViewModel 
    {
        [Key]
        public int InventoryID { get; set; }

        public string Name { get; set; }

        [Required]
        [DataType(DataType.ImageUrl)]
        public string ImageUrl { get; set; }
    }
}
