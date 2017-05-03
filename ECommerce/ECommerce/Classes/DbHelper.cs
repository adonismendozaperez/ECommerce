using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using ECommerce.Models;

namespace ECommerce.Classes
{
    public class DbHelper
    {
        public static Reponse SaveChanges(ECommerceContext db)
        {
            try
            {
                db.SaveChanges();
                return new Reponse { Succeeded = true, };
            }
            catch (Exception ex)
            {
                var reponse = new Reponse { Succeeded = false };
                if (ex.InnerException != null &&
                      ex.InnerException.InnerException != null &&
                      ex.InnerException.InnerException.Message.Contains("REFERENCE"))
                {
                    reponse.Message= "There are a record with the same value";


                }
                else
                {
                    reponse.Message= ex.Message;
                }
                return reponse;
            }
        }


        public static int GetState(string description, ECommerceContext db)
        {
            var state = db.States.Where(s => s.Description == description).FirstOrDefault();
            if (state == null)
            {
                state = new State { Description = description, };
                db.States.Add(state);
                db.SaveChanges();
            }

            return state.StateId;
        }

      
    }
}