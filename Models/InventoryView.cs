using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

/**
 * This is the database class for storing inventory
 * @Author Zhili Xu
 * */

namespace FlexRental.Models
{
    public class InventoryView
    {
        public virtual ApplicationUser User { get; set; }

        public string ApplicationUserId { get; set;}
        [Key]
        public int InventoryId { get; set; }

        [Required]
        public string Name { get; set; }

        public string Color { get; set; }
        //[RegularExpression(@"\w{4}", ErrorMessage = "Year should start from 1000 to current year")]
        
        public string Year { get; set; }

        [Required]
        public string Condition { get; set; }

        [Required]
        public string Category { get; set; }

        [Required]
        [DataType(DataType.Currency)]
        public decimal Price { get; set; }

        public string Brand { get; set; }

        [Required]
        public int Quantity { get; set; }

        public string Material { get; set; }

        public string Status { get; set; }

        public string Description { get; set; }

        [DataType(DataType.DateTime)]
        public DateTime Date { get; set; }
    }
}