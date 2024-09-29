﻿using ProductManagement.Models;
using System;
using System.Collections.Generic;
using System.Data;
using System.Diagnostics;
using System.Linq;
using System.Linq.Expressions;

namespace ProductManagement.Repositories
{
    public class ProductRepository
    {
        public List<ProductDTO> GetAllProducts(Expression<Func<Product, bool>> predicate = null)
        {
            using (ProductEntities db = new ProductEntities())
            {
                IQueryable<Product> query = db.Products;

                if (predicate != null)
                {
                    Debug.Print("Predicate is not null.");
                    query = query.Where(predicate);
                }

                var result = query.Select(a => new ProductDTO
                {
                    ProdID = a.ProdID ?? "N/A",
                    ProdName = a.ProdName ?? "N/A",
                    Quantity = a.Quantity ?? 0,
                    Price = a.Price ?? 0,
                    Origin = a.Origin ?? "N/A",
                    ExpireDate = a.ExpireDate ?? DateTime.Now
                }).ToList();

                return result;
            }
        }



        public ProductDTO GetMaxPriceProduct()
        {
            using (ProductEntities db = new ProductEntities())
            {
                var maxPriceProduct = (from a in db.Products
                                       select new ProductDTO
                                       {
                                           ProdID = a.ProdID ?? "N/A",
                                           ProdName = a.ProdName ?? "N/A",
                                           Quantity = a.Quantity ?? 0,
                                           Price = a.Price ?? 0,
                                           Origin = a.Origin ?? "N/A",
                                           ExpireDate = a.ExpireDate ?? DateTime.Now
                                       })
                                .OrderByDescending(x => x.Price)
                                .FirstOrDefault();
                return maxPriceProduct;
            }
        }

        public void SaveProduct(ProductDTO productDTO)
        {
            using (ProductEntities db = new ProductEntities())
            {
                var product = db.Products.Where(p => p.ProdID == productDTO.ProdID).FirstOrDefault();

                if (product != null)
                {
                    SetProduct(product, productDTO, false);
                }
                else
                {
                    product = new Product();
                    ResetValues(product);
                    SetProduct(product, productDTO, true);
                    db.Products.Add(product);
                }
                db.SaveChanges();
            }
        }
        private void ResetValues(Product product)
        {
            product.ProdID = "";
            product.ProdName = "";
            product.Price = 0;
            product.Quantity = 0;
            product.Origin = "";
            product.ExpireDate = null;
        }

        private void SetProduct(Product product, ProductDTO productDTO, bool isNew)
        {
            if (isNew)
            {
                product.ProdID = productDTO.ProdID;
            }
            product.ProdName = productDTO.ProdName;
            product.Price = productDTO.Price;
            product.Quantity = productDTO.Quantity;
            product.Origin = productDTO.Origin;
            product.ExpireDate = productDTO.ExpireDate;
        }

        public void DeleteProduct(Func<Product, bool> predicate)
        {
            using (ProductEntities db = new ProductEntities())
            {
                // Get all products matching the predicate
                var productsToDelete = db.Products.Where(predicate).ToList();

                // Delete products using a for loop
                for (int i = 0; i < productsToDelete.Count; i++)
                {
                    db.Products.Remove(productsToDelete[i]);
                }

                db.SaveChanges();
            }
        }
        public void DeleteAllProduct()
        {
            using (ProductEntities db = new ProductEntities())
            {
                db.Database.ExecuteSqlCommand("delete from Product");
            }
        }
    }
}
