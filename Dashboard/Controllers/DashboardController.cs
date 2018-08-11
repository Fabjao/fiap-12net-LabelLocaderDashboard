using Dashboard.Context;
using GeekBurger.Dashboard.Contract.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Dashboard.Controllers
{
    [Route("api/dashboard")]
    public class DashboardController : Controller
    {
        [Route("sales")]
        [HttpGet]
        public async Task<IActionResult> SalesAsync()
        {
            var list = new List<Sale>();

            DbContextOptionsBuilder DbContextOptionsBuilder = new DbContextOptionsBuilder<DashboardDB>();

            using (var context = new DashboardDB(DbContextOptionsBuilder.Options))
            {
                #region MOCK
                context.Orders.Add(new Models.Order { StoreId = new Random().Next(1, 3), OrderId = new Random().Next(1, 99999), Value = new Random().Next(1, 100), Date = DateTime.Now.AddMinutes(-30 * 1) });
                context.Orders.Add(new Models.Order { StoreId = new Random().Next(1, 3), OrderId = new Random().Next(1, 99999), Value = new Random().Next(1, 100), Date = DateTime.Now.AddMinutes(-30 * 2) });
                context.Orders.Add(new Models.Order { StoreId = new Random().Next(1, 3), OrderId = new Random().Next(1, 99999), Value = new Random().Next(1, 100), Date = DateTime.Now.AddMinutes(-30 * 3) });
                context.Orders.Add(new Models.Order { StoreId = new Random().Next(1, 3), OrderId = new Random().Next(1, 99999), Value = new Random().Next(1, 100), Date = DateTime.Now.AddMinutes(-30 * 4) });
                context.Orders.Add(new Models.Order { StoreId = new Random().Next(1, 3), OrderId = new Random().Next(1, 99999), Value = new Random().Next(1, 100), Date = DateTime.Now.AddMinutes(-30 * 5) });
                context.Orders.Add(new Models.Order { StoreId = new Random().Next(1, 3), OrderId = new Random().Next(1, 99999), Value = new Random().Next(1, 100), Date = DateTime.Now.AddMinutes(-30 * 6) });
                context.Orders.Add(new Models.Order { StoreId = new Random().Next(1, 3), OrderId = new Random().Next(1, 99999), Value = new Random().Next(1, 100), Date = DateTime.Now.AddMinutes(-30 * 7) });
                context.SaveChanges();
                #endregion

                if (context.Orders.Any())
                {
                    var orders = await context.Orders.ToListAsync();

                    list = orders.GroupBy(x => x.StoreId).Select(g => new Sale
                    {
                        StoreId = g.Key,
                        Total = g.Count(),
                        Value = g.Sum(c => c.Value)
                    }).ToList();
                }
            }

            return Ok(list);
        }

        [Route("sales/{Per}/{Value:int}")]
        [HttpGet]
        public async Task<IActionResult> Sales(string Per, int Value)
        {
            var list = new List<Sale>();

            DbContextOptionsBuilder DbContextOptionsBuilder = new DbContextOptionsBuilder<DashboardDB>();

            using (var context = new DashboardDB(DbContextOptionsBuilder.Options))
            {
                #region MOCK
                context.Orders.Add(new Models.Order { StoreId = new Random().Next(1, 3), OrderId = new Random().Next(1, 99999), Value = new Random().Next(1, 100), Date = DateTime.Now.AddMinutes(-30 * 1) });
                context.Orders.Add(new Models.Order { StoreId = new Random().Next(1, 3), OrderId = new Random().Next(1, 99999), Value = new Random().Next(1, 100), Date = DateTime.Now.AddMinutes(-30 * 2) });
                context.Orders.Add(new Models.Order { StoreId = new Random().Next(1, 3), OrderId = new Random().Next(1, 99999), Value = new Random().Next(1, 100), Date = DateTime.Now.AddMinutes(-30 * 3) });
                context.Orders.Add(new Models.Order { StoreId = new Random().Next(1, 3), OrderId = new Random().Next(1, 99999), Value = new Random().Next(1, 100), Date = DateTime.Now.AddMinutes(-30 * 4) });
                context.Orders.Add(new Models.Order { StoreId = new Random().Next(1, 3), OrderId = new Random().Next(1, 99999), Value = new Random().Next(1, 100), Date = DateTime.Now.AddMinutes(-30 * 5) });
                context.Orders.Add(new Models.Order { StoreId = new Random().Next(1, 3), OrderId = new Random().Next(1, 99999), Value = new Random().Next(1, 100), Date = DateTime.Now.AddMinutes(-30 * 6) });
                context.Orders.Add(new Models.Order { StoreId = new Random().Next(1, 3), OrderId = new Random().Next(1, 99999), Value = new Random().Next(1, 100), Date = DateTime.Now.AddMinutes(-30 * 7) });
                context.SaveChanges();
                #endregion

                if (context.Orders.Any())
                {
                    var orders = await context.Orders.ToListAsync();

                    DateTime compareDate = DateTime.Now;
                    switch (Per.ToLower().Trim())
                    {
                        case "minute":
                            compareDate = DateTime.Now.AddMinutes(-Value);
                            break;
                        case "hour":
                            compareDate = DateTime.Now.AddHours(-Value);
                            break;
                        case "day":
                            compareDate = DateTime.Now.AddDays(-Value);
                            break;
                        case "month":
                            compareDate = DateTime.Now.AddMonths(-Value);
                            break;
                        case "year":
                            compareDate = DateTime.Now.AddYears(-Value);
                            break;
                        default:
                            compareDate = DateTime.Now;
                            break;
                    }

                    list = orders.Where(x => x.Date >= compareDate).GroupBy(x => x.StoreId).Select(g => new Sale
                    {
                        StoreId = g.Key,
                        Total = g.Count(),
                        Value = g.Sum(c => c.Value)
                    }).ToList();
                }
            }

            return Ok(list);
        }

        [Route("UsersWithLessOffer")]
        [HttpGet]
        public async Task<IActionResult> UsersWithLessOffer()
        {
            var list = new List<Restriction>();

            DbContextOptionsBuilder DbContextOptionsBuilder = new DbContextOptionsBuilder<DashboardDB>();

            using (var context = new DashboardDB(DbContextOptionsBuilder.Options))
            {
                #region MOCK
                context.FoodRestrictions.Add(new Models.FoodRestriction { UserId = new Random().Next(1, 99999), Restrictions = string.Join(", ", "soja".Split(",").OrderBy(p => p)) });
                context.FoodRestrictions.Add(new Models.FoodRestriction { UserId = new Random().Next(1, 99999), Restrictions = string.Join(", ", "soja,leite".Split(",").OrderBy(p => p)) });
                context.FoodRestrictions.Add(new Models.FoodRestriction { UserId = new Random().Next(1, 99999), Restrictions = string.Join(", ", "soja,leite".Split(",").OrderBy(p => p)) });
                context.FoodRestrictions.Add(new Models.FoodRestriction { UserId = new Random().Next(1, 99999), Restrictions = string.Join(", ", "soja,leite,pimenta".Split(",").OrderBy(p => p)) });
                context.FoodRestrictions.Add(new Models.FoodRestriction { UserId = new Random().Next(1, 99999), Restrictions = string.Join(", ", "soja,leite,pimenta".Split(",").OrderBy(p => p)) });
                context.FoodRestrictions.Add(new Models.FoodRestriction { UserId = new Random().Next(1, 99999), Restrictions = string.Join(", ", "soja,leite,pimenta".Split(",").OrderBy(p => p)) });
                context.FoodRestrictions.Add(new Models.FoodRestriction { UserId = new Random().Next(1, 99999), Restrictions = string.Join(", ", "soja,leite,pimenta,mel".Split(",").OrderBy(p => p)) });
                context.FoodRestrictions.Add(new Models.FoodRestriction { UserId = new Random().Next(1, 99999), Restrictions = string.Join(", ", "soja,leite,pimenta,mel,carne".Split(",").OrderBy(p => p)) });
                context.FoodRestrictions.Add(new Models.FoodRestriction { UserId = new Random().Next(1, 99999), Restrictions = string.Join(", ", "soja,leite,pimenta,mel,carne".Split(",").OrderBy(p => p)) });
                context.FoodRestrictions.Add(new Models.FoodRestriction { UserId = new Random().Next(1, 99999), Restrictions = string.Join(", ", "soja,leite,pimenta,mel,carne".Split(",").OrderBy(p => p)) });
                context.FoodRestrictions.Add(new Models.FoodRestriction { UserId = new Random().Next(1, 99999), Restrictions = string.Join(", ", "soja,leite,pimenta,mel,carne".Split(",").OrderBy(p => p)) });
                context.FoodRestrictions.Add(new Models.FoodRestriction { UserId = new Random().Next(1, 99999), Restrictions = string.Join(", ", "soja,leite,pimenta,mel,carne".Split(",").OrderBy(p => p)) });
                context.SaveChanges();
                #endregion

                if (context.FoodRestrictions.Any())
                {
                    var foodRestrictions = await context.FoodRestrictions.ToListAsync();

                    list = foodRestrictions.GroupBy(x => x.Restrictions).Select(g => new Restriction
                    {
                        Users = g.Count(),
                        Type = string.Join(", ", g.Key)
                    }).OrderByDescending(x => x.Users).ToList();
                }
            }

            return Ok(list);
        }
    }
}