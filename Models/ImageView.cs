using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

/**
 * This is the database class for storing image
 * @Author Zhili Xu
 * */

namespace FlexRental.Models
{
    public class ImageView
    {
    [Key]
    public int ImageId { get; set; }

    public virtual InventoryView Inventory { get; set; }

    public int InventoryViewId  { get; set; }

    public int Index { get; set; }

    [Required]
    [DataType(DataType.ImageUrl)]
    public string ImageUrl { get; set; }

    }
}