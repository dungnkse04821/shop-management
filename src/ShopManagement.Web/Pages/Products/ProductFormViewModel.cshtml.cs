using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using ShopManagement.EntityDto;
using System;
using System.Collections.Generic;

namespace ShopManagement.Web.Pages.Products
{
    public class ProductFormViewModel
    {
        public Guid Id { get; set; }
        [BindProperty]
        public CreateUpdateProductDto Product { get; set; } = new();
        [BindProperty(SupportsGet = false)]
        public List<IFormFile> ImageFiles { get; set; } = new();
        public string SubmitLabel { get; set; } = "Save";
    }
}