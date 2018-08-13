using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Dashboard.Context;

using GeekBurger.Dashboard.Contract.Models;

namespace Dashboard.Controllers
{
    [Route("api/dashboard")]
    public class DashboardController : Controller
    {
        Contexto contexto = new Contexto();

        [Route("index")]
        [HttpGet]
        public IActionResult Index()
        {
            
            return View();
        }

        [Route("sales")]
        [HttpGet]
        public IActionResult Sales()
        {
            //var valor = 10.00;
            //var testUser1 = new Context.OrderChanged
            //{
            //    OrderId = 1,
            //    State = "Paid",
            //    StoredId = "110901091019901",
            //    Value = valor
            //};

            //contexto.OrderChanged.Add(testUser1);

            //contexto.SaveChanges();

            var buscaOrder = contexto.OrderChanged.ToList().Where(x => x.State == "Paid");

            //var SomaVendasStore1 = buscaOrder.Sum(x => x.Value);
            //var SomaVendasStore2 = buscaOrder.Sum(x => x.Value);

            //var SomaVendasStore1 = buscaOrder.Sum(x => x.Value);
            //var SomaVendasStore2 = buscaOrder.Sum(x => x.Value);

            var result = buscaOrder.GroupBy (rd => rd.StoredId,
                                     
	                                 rd => rd.Value,
                                    
                                    
					(state, popSum) => new {
                        
					                         StoreId = state,
								 Value = popSum.Sum ()
								});

            
            

            var list = new List<Sale>();
           
            var totalPedidos = buscaOrder.GroupBy(rd => rd.StoredId,

                                     rd => rd.OrderId,


                    (state, popSum) => new {

                        StoreId = state,
                        Total = popSum.Count()
                    });


            foreach (var item in result)
            {
                foreach (var item2 in totalPedidos)
                {
                    if(item.StoreId == item2.StoreId)
                    {
                        Sale sales = new Sale();
                        sales.StoreId = Convert.ToInt32(item.StoreId);
                        sales.Total = item2.Total;
                        sales.Value = Convert.ToDecimal(item.Value);


                        list.Add(sales);
                    }
                }
            }

            

            //var list = new List<Sale>();
            //list.Add(new Sale { StoreId = 1111, Total = 1000, Value = 59385.00m });

            return Ok(list);
        }

        [Route("sales/{Per}/{Value:int}")]
        [HttpGet]
        public IActionResult Sales(string Per, int Value)
        {
            //var valor = 10.00;
            //var testUser1 = new Context.OrderChanged
            //{
            //    OrderId = 1,
            //    State = "Paid",
            //    StoredId = "110901091019901",
            //    Value = valor
            //};

            //contexto.OrderChanged.Add(testUser1);

            //contexto.SaveChanges();

            var buscaOrder = contexto.OrderChanged.ToList().Where(x => x.State == "Paid");

            //var SomaVendasStore1 = buscaOrder.Sum(x => x.Value);
            //var SomaVendasStore2 = buscaOrder.Sum(x => x.Value);

            //var SomaVendasStore1 = buscaOrder.Sum(x => x.Value);
            //var SomaVendasStore2 = buscaOrder.Sum(x => x.Value);

            var result = buscaOrder.GroupBy(rd => rd.StoredId,

                                     rd => rd.Value,


                    (state, popSum) => new {

                        StoreId = state,
                        Value = popSum.Sum()
                    });




            var list = new List<Sale>();

            var totalPedidos = buscaOrder.GroupBy(rd => rd.StoredId,

                                     rd => rd.OrderId,


                    (state, popSum) => new {

                        StoreId = state,
                        Total = popSum.Count()
                    });


            foreach (var item in result)
            {
                foreach (var item2 in totalPedidos)
                {
                    if (item.StoreId == item2.StoreId)
                    {
                        Sale sales = new Sale();
                        sales.StoreId = Convert.ToInt32(item.StoreId);
                        sales.Total = item2.Total;
                        sales.Value = Convert.ToDecimal(item.Value);


                        list.Add(sales);
                    }
                }
            }



            //var list = new List<Sale>();
            //list.Add(new Sale { StoreId = 1111, Total = 1000, Value = 59385.00m });

            return Ok(list);
        }

        [Route("UsersWithLessOffer")]
        [HttpGet]
        public IActionResult UsersWithLessOffer()
        {
            var buscarUsersWithLessOffer = contexto.UserWithLessOffer.ToList();

            var result = buscarUsersWithLessOffer.GroupBy(rd => rd.UserId,

                                     rd => rd.restrictions,


                    (Users, Restrictions) => new {

                        users =  Users,
                        restricions = Restrictions
                    });

            List<Restriction> restrictions = new List<Restriction>();

            foreach (var item in result)
            {
                Restriction res = new Restriction();
                res.Users = item.users;
                res.Type =  item.restricions.ToString();
                restrictions.Add(res);
            }

            
            //restrictions.Add(new Restriction { Type = "soy,diary", Users = 2 });
            //restrictions.Add(new Restriction { Type = "gluten", Users = 8 });

            //User user = new User()
            //{
            //    Users = 10,
            //    Restrictions = restrictions,
            //    //Usage = 50
            //};

            return Ok(restrictions);
        }

        //[Route("teste")]
        //[HttpGet]
        //public IActionResult teste(int valor)
        //{
        //    var busca = contexto.OrderChanged.ToList();
        //    var list = new List<Sale>();
        //    OrderChanged pedido = new OrderChanged();
        //    //list.Add(new Sale { StoreId = 1111, Total = 1000, Value = 59385.00m });
        //    foreach (var item in busca)
        //    {
        //        pedido.OrderId = item.OrderId;
        //        pedido.State = item.StoredId;
        //        pedido.Value = item.Value;
        //        pedido.StoredId = item.StoredId;
        //    }

            
          

        //    return Ok(list);
        //}

    }
}