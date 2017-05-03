using ECommerce.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ECommerce.Classes
{
    public class MovementsHelper: IDisposable
    {
        private static ECommerceContext db = new ECommerceContext();

        public static Reponse NewOrder(NewOrderView newOrder, string UserName)
        {
            using(var transaction = db.Database.BeginTransaction())
            {
                try
                {
                    var user = db.Users.Where(u => u.UserName == UserName).FirstOrDefault();
                    var order = new Order
                    {
                        CompanyId = user.CompanyId,
                        CustomerId = newOrder.CustomerId,
                        Date = newOrder.Date,
                        Remarks = newOrder.Remarks,
                        StateId = DbHelper.GetState("Created", db),
                    };
                    db.Orders.Add(order);
                    db.SaveChanges();

                    var details = db.OrderDetailTmps.Where(o => o.UserName == UserName).ToList();
                    foreach (var detail in details)
                    {
                        var orderDetail = new OrderDetail
                        {
                            Description = detail.Description,
                            OrderId = order.OrderId,
                            Price = detail.Price,
                            ProductId = detail.ProductId,
                            Quantity = detail.Quantity,
                            TaxRate = detail.TaxRate,
                        };
                        db.OrderDetails.Add(orderDetail);
                        db.OrderDetailTmps.Remove(detail);
                    }
                    db.SaveChanges();
                    transaction.Commit();
                    return new Reponse { Succeeded = true };
                }
                catch (Exception ex)
                {
                    transaction.Rollback();
                    return new Reponse
                    {
                        Message = ex.Message,
                        Succeeded = false
                    };
                    
                }
            }
        }



        public void Dispose()
        {
            db.Dispose();
        }
    }
}