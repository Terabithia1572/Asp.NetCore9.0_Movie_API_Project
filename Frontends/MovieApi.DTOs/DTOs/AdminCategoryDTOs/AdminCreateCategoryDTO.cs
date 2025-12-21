using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MovieApi.DTOs.DTOs.AdminCategoryDTOs
{
    public class AdminCreateCategoryDTO
    {
        public string CategoryName { get; set; } //Ekleme işlemi için Kategori Adını Aldık    
        public bool IsActive { get; set; } //Kategori Aktiflik Durumu
    }
}
