using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

/**
 * This is the model deal with data used by seller
 * @Author Zhili Xu
 * */

namespace FlexRental.Models
{
    public class AddInventoryViewModel
    {
        [Required]
        public string Name { get; set; }

        public string Color { get; set; }
        //[RegularExpression(@"\w{4}", ErrorMessage = "Year should start from 1000 to current year")]

        public string Year { get; set; }

        [Required]
        public string Category { get; set; }

        [Required]
        public string Condition { get; set; }

        [Required]
        [DataType(DataType.Currency)]
        public decimal Price { get; set; }

        public string Brand { get; set; }

        [Required]
        public int Quantity { get; set; }

        public string Material { get; set; }

        public string Status { get; set; }

        public string Description { get; set; }

        [DataType(DataType.Upload)]
        public IEnumerable<HttpPostedFileBase> ImageUpload { get; set; }
    }

    public class InventoyIndexViewModel
    {
        [Key]
        public int InventoryID { get; set; }

        public string Name { get; set; }

        public string Condition { get; set; }

        public string Status { get; set; }

        public decimal Price { get; set; }

        public int Quantity { get; set; }
    }
    public class InventoryDetailsViewModel
    {
        [Key]
        public int InventoryId { get; set; }

        public string Category { get; set; }

        public string Name { get; set; }

        public string Color { get; set; }
        
        public string Year { get; set; }

        public string Condition { get; set; }

        [DataType(DataType.DateTime)]
        [Display(Name = "Modified Date")]
        public DateTime Date { get; set; }

        public string Description { get; set; }

        [DataType(DataType.Currency)]
        public decimal Price { get; set; }

        public string Brand { get; set; }

        public int Quantity { get; set; }

        public string Material { get; set; }

        public string Status { get; set; }

        [DataType(DataType.ImageUrl)]
        public IEnumerable<string> Images { get; set; }

    }

    public class NewsViewModel
    {
        [Key]
        public int InventoryId { get; set; }

        public string name { get; set; }
        public NewsViewModel()
        {

        }
        public NewsViewModel(int id, string itemname)
        {
            InventoryId = id;
            name = itemname;
        }
    }
}